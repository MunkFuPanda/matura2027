## I. WPF Grundlagen (XAML & Code-Behind)

| Konzept | Kurzbeschreibung | XAML / C# Snippet | Quelle |
| :--- | :--- | :--- | :--- |
| **WPF** | Neue Programmierschnittstelle (.NET 3.0), trennt Präsentation (XAML) und Logik (Code) strikt. Nutzt DirectX für verbesserte Performance (Vektorbasiert). | N/A | |
| **XAML** | **eXtensible Application Markup Language.** Deklarative, XML-ähnliche Sprache zur UI-Beschreibung. | `xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"`<br>`xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"` | |
| **Dateistruktur** | `MainWindow.xaml` (UI-Beschreibung).<br>`MainWindow.xaml.cs` (Code-Behind/Programmlogik).<br>`App.xaml` (Anwendungsweite Ressourcen, Startfenster `StartupUri`). | *MainWindow.xaml:*<br>`<Window x:Class="App.MainWindow" ...>` | |
| **Verbindung Code**| Die Verbindung zwischen XAML und Code-Behind wird in der ersten Zeile der XAML-Datei hergestellt (`x:Class`). | `<Window x:Class="WpfApplication1.MainWindow" ...>` | |

---

## II. XAML Syntax & Eigenschaften

| Syntax | Anwendung / Beschreibung | XAML Snippet | Quelle |
| :--- | :--- | :--- | :--- |
| **Attribut-Syntax** | Einfache Eigenschaften (z. B. einfache Datentypen). Werte sind immer Strings (Typkonvertierung implizit). | `<Button Height="50" Width="100"/>` | |
| **Property-Element-Syntax** | Komplexe Eigenschaften (z. B. `Brush`, Farbverläufe). | `<Button><Button.Background><SolidColorBrush Color="Blue"/></Button.Background></Button>` | |
| **Content Eigenschaft** | Die Standard-Inhaltseigenschaft (z. B. bei `Button` oder `Label`). Kann weggelassen werden, da sie als Inhaltseigenschaft definiert ist. | `<Button Content="OK"/>`<br>Oder kürzer:<br>`<Button>OK</Button>` | |
| **x:Class** | Stellt die Beziehung zur Code-Behind-Datei her. | `<Window x:Class="Namespace.Klasse" ...>` | |
| **x:Name** | Gibt einem Element einen Namen, falls es keine eigene `Name`-Eigenschaft hat. | `<Grid x:Name="MyGrid"/>` | |
| **CLR-Namespace einbinden** | Einbinden von Klassen aus externen Assemblies (z.B. `GeometricObjects.dll`). | `xmlns:geo="clr-namespace:GeometricObjects;assembly=GeometricObjects"` | |

---

## III. Layout Container (Panel)

WPF nutzt Layoutcontainer für die Anordnung (keine Absolutkoordinaten).

| Container | Kurzbeschreibung | XAML Snippet | Quelle |
| :--- | :--- | :--- | :--- |
| **Grid** | Tabellenstruktur (Zeilen/Spalten). Zeilen/Spalten sind 0-basiert. | `<Grid.ColumnDefinitions><ColumnDefinition Width="*"/><ColumnDefinition Width="Auto"/></Grid.ColumnDefinitions><Button Grid.Column="1">Rechts</Button>` | |
| **Grid Span** | Element über mehrere Zeilen/Spalten ausdehnen. | `<Button Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="2"/>` | |
| **StackPanel** | Elemente werden vertikal (Standard) oder horizontal gestapelt. | `<StackPanel Orientation="Horizontal"><Button>B1</Button><Button>B2</Button></StackPanel>` | |
| **Canvas** | Positionierung über kartesisches System (Left/Top/Right/Bottom). Sollte vermieden werden. | `<Canvas><Button Canvas.Left="10" Canvas.Top="20">B1</Button></Canvas>` | |
| **Margin** | **Außenabstand** (zum umgebenden Container). Reihenfolge: Links, Oben, Rechts, Unten. | `Margin="5"` (alle Seiten 5px)<br>`Margin="10, 20"` (H:10, V:20)<br>`Margin="10, 20, 5, 25"` (L,O,R,U) | |
| **Padding** | **Innenabstand** (des Elements zum eigenen Inhalt). | `Padding="10"` | |
| **Visibility** | Steuert die Sichtbarkeit eines Elements. | `Visibility="Hidden"` (unsichtbar, belegt Platz)<br>`Visibility="Collapsed"` (unsichtbar, belegt KEINEN Platz) | |

---

## IV. Ereignisbehandlung (Routed Events)

| Konzept | Kurzbeschreibung | C# Snippet / XAML Snippet | Quelle |
| :--- | :--- | :--- | :--- |
| **Ereignishandler** | Standardmäßig: `object sender, EventArgs e`. In WPF oft spezialisierte Ableitungen von `EventArgs`. | `Click="button1_Click"` (XAML-Zuweisung) | |
| **Dynamische Zuweisung** | Event-Handler zur Laufzeit im Code zuweisen. | `button1.Click += new RoutedEventHandler(button1_Click);` | |
| **Routed Events** | Spezialform in WPF aufgrund verschachtelter Elemente (Elementbäume). | N/A | |
| **Tunneling** | Ereignis startet an der **Wurzel** (z. B. `Window`) und geht **abwärts** zum auslösenden Element. Präfix **`Preview`**. | `PreviewMouseLeftButtonDown` | |
| **Bubbling** | Ereignis startet am **Auslöser** und wandert **aufwärts** zur Wurzel. | `MouseLeftButtonDown` | |
| **Kette abbrechen** | Setzt `Handled` auf `true` im Event-Argument. | `e.Handled = true;` | |

---

## V. Datenbindung

Datenbindung funktioniert nur mit **abhängigen Eigenschaften (Dependency Properties)**.

| Konzept | Kurzbeschreibung | XAML Snippet | Quelle |
| :--- | :--- | :--- | :--- |
| **Binding Markup Ext.** | Kompakte Schreibweise für Bindung im XAML. | `Text="{Binding ElementName=txtOben, Path=Text}"` | |
| **ElementName** | Definiert die Datenquelle als ein anderes Element im XAML. | `ElementName=txtOben` | |
| **Path** | Definiert die Eigenschaft der Datenquelle, an die gebunden werden soll. | `Path=Text` | |
| **DataContext** | Bindeglied zwischen Datenquelle und UI-Elementen. Wird oft dem Container oder dem `Window` zugewiesen, um Kaskadierung zu nutzen. | *C#:* `this.DataContext = personObject;` | |
| **Bindungsmodus: OneWay** | Aktualisierung nur von **Quelle** zum **Ziel**. | `Mode=OneWay` | |
| **Bindungsmodus: TwoWay** | Aktualisierung in **beide Richtungen** (Quelle und Ziel). Standardmodus für viele Eigenschaften. | `Mode=TwoWay` | |
| **Änderungsbenachrichtigung** | Damit die **Quelle** (z. B. eine Klasse) Änderungen an das **Ziel** (UI-Element) melden kann, muss die Quelle **`INotifyPropertyChanged`** implementieren. | *C#:* `public class Person : INotifyPropertyChanged { ... }` | |
| **Listenbindung** | Bindung von Auflistungen an Steuerelemente (z. B. `ListBox`). Eigenschaft **`ItemsSource`**. Beste Praxis: **`ObservableCollection<>`** verwenden. | `<ListBox ItemsSource="{Binding}"/>` | |

### C# Beispiel: INotifyPropertyChanged Struktur

```csharp
using System.ComponentModel;
// ...
public class Person : INotifyPropertyChanged {
    private string _Name;
    public event PropertyChangedEventHandler PropertyChanged; // Muss implementiert werden

    public string Name {
        get { return _Name; }
        set {
            _Name = value;
            // Ereignis auslösen, wenn Eigenschaft geändert
            OnPropertyChanged(new PropertyChangedEventArgs("Name"));
        }
    }
    
    // Hilfsmethode zur Kapselung der Event-Auslösung
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
        if (PropertyChanged != null)
            PropertyChanged(this, e);
    }
}
```

---

## VI. Ressourcen

Logische Ressourcen sind wiederverwendbare .NET-Objekte, die zentral definiert werden.

| Ressourcentyp | Beschreibung | XAML Snippet | Quelle |
| :--- | :--- | :--- | :--- |
| **Statische Ressource** | **`StaticResource`**: Wert wird nur einmal beim Laden ausgewertet. Ist danach fix. Muss existieren. | `Background="{StaticResource MyBrush}"` | |
| **Dynamische Ressource** | **`DynamicResource`**: Wert wird zur Laufzeit aktualisiert, falls die Ressource im `ResourceDictionary` geändert wird. | `Background="{DynamicResource MyBrush}"` | |
| **Definition** | Ressourcen werden über einen eindeutigen Schlüssel (`x:Key`) definiert. | `<SolidColorBrush x:Key="MyBrush" Color="Red"/>` | |
| **Definition im Code** | Ressourcen zur Laufzeit hinzufügen/ändern. | *C#:* `this.Resources["MyBrush"] = new SolidColorBrush(Colors.Green);` | |
| **Suche** | Die Suche nach einer Ressource beginnt beim aktuellen Element, wandert aufwärts im Logical Tree, dann zu Application-Ressourcen, zuletzt zu System-Ressourcen. | N/A | |
| **Zuweisung Dynamic im Code** | Nur möglich über `SetResourceReference` (erfordert Dependency Property). | *C#:* `button1.SetResourceReference(Button.BackgroundProperty, "MyBrush");` | |

---

## VII. Wichtige Steuerelemente (Controls)

| Steuerelement | Wichtige Eigenschaften | XAML / C# Snippet | Quelle |
| :--- | :--- | :--- | :--- |
| **Button** | `Click` Ereignis. `IsDefault=true` (reagiert auf <kbd>Enter</kbd>), `IsCancel=true` (reagiert auf <kbd>ESC</kbd>). | `<Button Content="_Speichern" IsDefault="True"/>` (Hotkey: Alt+S) | |
| **ToggleButton** | Behält Zustand bei. Zustand über `IsChecked`. | `<ToggleButton IsChecked="True" Checked="Tgl_Checked">Toggle</ToggleButton>` | |
| **TextBox** | Texteingabe/Anzeige. `Text` Eigenschaft. Automatische Rechtschreibprüfung (`SpellCheck.IsEnabled=true`). | `<TextBox TextAlignment="Center" AcceptsReturn="True" TextWrapping="Wrap"/>` | |
| **PasswordBox** | Einfache Textbox für Passwörter. Kein `Text`-Property; Passwort über `Password` abrufbar. | `<PasswordBox PasswordChar="*"/>` | |
| **Label** | Zeigt Text an. Kann Hotkeys über `_` definieren, die den Fokus an das `Target`-Control weitergeben. | `<Label Content="_Name:" Target="{Binding ElementName=txtInput}"/>` | |
| **ListBox** | Zeigt auswählbare Liste. `SelectionMode` (`Single`, `Multiple`, `Extended`). | `SelectionMode="Multiple">` | |
| **ListBox Auswahl** | Abruf des/der ausgewählten Elemente(s). | *C#:* `ListBoxItem item = (ListBoxItem)ListBox1.SelectedItem;` | |
| **ListView** | Abgeleitet von `ListBox`, kann Einträge unterschiedlich darstellen (z. B. mehrspaltig mit `GridView`). | `<ListView ItemsSource="{Binding ListOfPersons}"><ListView.View><GridView><GridViewColumn Header="Name" DisplayMemberBinding="{Binding Path=Name}"/></GridView></ListView.View></ListView>` | |
| **GroupBox** | Fasst Steuerelemente visuell zusammen. | `<GroupBox Header="Optionen"><StackPanel><RadioButton>R1</RadioButton></StackPanel></GroupBox>` | |

---

## VIII. Erweiterte Eigenschaften

| Typ | Klasse / Konzept | Beschreibung & Beispiel | Quelle |
| :--- | :--- | :--- | :--- |
| **Dependency Property (DP)** | Basis für viele WPF-Konzepte (Binding, Animationen, Ressourcen). Statische Felder vom Typ `DependencyProperty`. | *C#:* `public static readonly DependencyProperty LengthProperty;` | |
| **Attached Property** | Eigenschaften, die in einem **übergeordneten** Element definiert sind, aber auf einem **untergeordneten** Element zugewiesen werden (z. B. Layoutcontainer). | `<Button Grid.Row="0" Grid.Column="1"/>` (Grid stellt Row/Column bereit) | |
| **IValueConverter** | Ermöglicht die Konvertierung eines Datenwerts, wenn Quelldatentyp und Zieldatentyp nicht kompatibel sind. Konvertierung von Quelle zu Ziel (`Convert`), oder Ziel zu Quelle (`ConvertBack`). | *XAML:* `Converter="{StaticResource converter}"` | |
| **IMultiValueConverter** | Konverter für `MultiBinding`. Ermöglicht die Bindung mehrerer Datenquellen an eine Zieleigenschaft. | *C#:* Die Methode `Convert` erhält ein `object[] values`. | |
| **FlowDocuments** | Umfangreiche Text- und Dokumentdarstellung (Listen, Tabellen, Bilder). Viewer: `FlowDocumentReader`, `FlowDocumentPageViewer`, `FlowDocumentScrollViewer`. | `<FlowDocumentScrollViewer><FlowDocument><Paragraph>Text...</Paragraph></FlowDocument></FlowDocumentScrollViewer>` | |

---

### Analogie zur Konsolidierung

Stellen Sie sich WPF wie ein flexibles, modulares Spielzeug-Bausteinsystem vor (XAML und Layoutcontainer). Die **abgehängten Eigenschaften** (`Grid.Row`, `Canvas.Left`) sind wie spezielle Verbindungsstücke, die nur funktionieren, wenn sie in einen kompatiblen Behälter (den Layoutcontainer) eingesetzt werden. Die **Datenbindung** ist der Kommunikationsdraht, der dafür sorgt, dass sich die Eigenschaften der Bausteine automatisch synchronisieren. Wenn Sie diesen Draht verwenden, müssen Sie darauf achten, dass die Datenquelle selbst Signale senden kann (durch **`INotifyPropertyChanged`**), falls sie sich ändert.