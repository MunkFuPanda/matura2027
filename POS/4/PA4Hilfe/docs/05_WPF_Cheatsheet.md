# WPF-Cheatsheet (Teil 3: GUI + Teil 6: Visualisierung)

Kopierfertige Snippets. Vollstaendig zusammengesetzt in
`MosaikWerkstatt/MainWindow.xaml(.cs)`.

## Datei laden (OpenFileDialog)
```csharp
using Microsoft.Win32;   // WICHTIG: nicht System.Windows.Forms

var dlg = new OpenFileDialog
{
    Filter = "Programme (*.txt;*.mosaik)|*.txt;*.mosaik|Alle Dateien (*.*)|*.*"
};
if (dlg.ShowDialog() == true)
{
    string text = System.IO.File.ReadAllText(dlg.FileName);
    CodeBox.Text = text;
}
```
Speichern analog mit `SaveFileDialog` + `File.WriteAllText`.

## Grid mit Star-Sizing (skaliert beim Vergroessern)
```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="2*"/>   <!-- 2 Teile -->
        <ColumnDefinition Width="6"/>     <!-- fixe Pixel (Trenner) -->
        <ColumnDefinition Width="3*"/>   <!-- 3 Teile -->
    </Grid.ColumnDefinitions>
    <TextBox Grid.Column="0"/>
    <Border  Grid.Column="2"/>
</Grid>
```
`*` = anteilig, `Auto` = so gross wie noetig, Zahl = fixe Pixel. Das ist der
"skalierende GUI"-Punkt aus Waldwunder.

## Gitterfeld bauen (UniformGrid, im Code-Behind befuellt)
```xml
<Viewbox Stretch="Uniform">                  <!-- haelt das Feld quadratisch -->
    <UniformGrid x:Name="FieldHost" Width="360" Height="360"/>
</Viewbox>
```
```csharp
FieldHost.Rows = 9; FieldHost.Columns = 9;
_cells = new Border[9, 9];
for (int r = 0; r < 9; r++)
  for (int c = 0; c < 9; c++)
  {
    var cell = new Border { BorderBrush = Brushes.LightGray,
                            BorderThickness = new Thickness(0.5),
                            Background = Brushes.White };
    _cells[r, c] = cell;
    FieldHost.Children.Add(cell);     // Reihenfolge = zeilenweise
  }
```
Zelle einfaerben: `_cells[r, c].Background = Brushes.Red;`

## Alternative: frei zeichnen auf Canvas (fuer Painter/Linien)
```xml
<Canvas x:Name="DrawArea" Background="White"/>
```
```csharp
var line = new System.Windows.Shapes.Line
{
    X1 = x1, Y1 = y1, X2 = x2, Y2 = y2,
    Stroke = Brushes.Red, StrokeThickness = 2
};
DrawArea.Children.Add(line);
```

## Getaktete Ausfuehrung - zwei Wege

### Weg A: async + await Task.Delay (einfach, empfohlen)
```csharp
private async void OnRun_Click(object sender, RoutedEventArgs e)
{
    var frames = interp.Run(CodeBox.Text);   // erst komplett rechnen
    foreach (var frame in frames)            // dann abspielen
    {
        RenderFrame(frame);
        await Task.Delay(1000);              // 1 Sekunde Pause
    }
}
```
`await` haelt die GUI responsiv (kein Einfrieren). Tradeoff: ein Laufzeitfehler
faellt schon beim Rechnen auf, vor der Animation - fuer die Angabe ok.

### Weg B: DispatcherTimer (falls Schritt-fuer-Schritt live gewuenscht)
```csharp
private DispatcherTimer _timer;   // using System.Windows.Threading;
private int _step;
private List<Frame> _frames;

private void StartAnimation(List<Frame> frames)
{
    _frames = frames; _step = 0;
    _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
    _timer.Tick += (s, e) =>
    {
        if (_step >= _frames.Count) { _timer.Stop(); return; }
        RenderFrame(_frames[_step++]);
    };
    _timer.Start();
}
```

## ListBox mit Auswahl (gespeicherte Programme)
```xml
<ListBox x:Name="SavedList" MouseDoubleClick="OnSavedList_DoubleClick"/>
```
```csharp
SavedList.ItemsSource = store.GetAll();      // Liste von Objekten
SavedList.DisplayMemberPath = "Name";        // welche Property anzeigen

private void OnSavedList_DoubleClick(object s, RoutedEventArgs e)
{
    if (SavedList.SelectedItem is SavedProgram p)
        CodeBox.Text = p.Source;
}
```
Fuer Live-Sync zwischen Auswahl und Anzeige: `SelectionChanged`-Event nutzen.

## Eigener Eingabe-Dialog
Siehe `MosaikWerkstatt/InputDialog.xaml(.cs)`. Aufruf:
```csharp
string name = InputDialog.Ask(this, "Speichern", "Name?", "Vorgabe");
if (name != null) { /* OK gedrueckt */ }
```
Im Dialog: `IsDefault="True"` (Enter = OK), `IsCancel="True"` (Esc = Abbrechen),
`DialogResult = true;` schliesst und liefert `true` an `ShowDialog()`.

## Schnelle Meldungen
```csharp
MessageBox.Show("Text", "Titel", MessageBoxButton.OK, MessageBoxImage.Error);
```

## Framework-Variante (falls .NET Framework statt .NET 8)
- `OpenFileDialog` kommt weiterhin aus `Microsoft.Win32` (WPF) - **nicht**
  `System.Windows.Forms` referenzieren.
- `async`/`await Task.Delay`, `DispatcherTimer`, Star-Sizing: alles identisch.
- Vermeide moderne C#-Features (`record`, `string?`, file-scoped namespaces) -
  der Code in diesem Ordner ist bereits konservativ geschrieben und kompiliert
  auf beidem.
- `.csproj` sieht anders aus (nicht SDK-style). Aber: Vorgabe-Projekt benutzen,
  nicht selbst anlegen. Du legst nur `.cs`-Dateien dazu.
