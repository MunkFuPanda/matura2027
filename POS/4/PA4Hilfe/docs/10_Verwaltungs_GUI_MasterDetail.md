# Verwaltungs-GUI: Master-Detail mit ListBox-Sync (Waldwunder-Teil)

Das Waldwunder-Grundgeruest: **Liste links, Detailansicht rechts, Auswahl haelt
beides synchron**, dazu Neu/Speichern/Loeschen. Kommt als Variante, wenn die PA
einen Verwaltungs-Teil hat (Datensaetze anzeigen/bearbeiten).

## Layout (skalierend, Star-Sizing)
```xml
<Grid Margin="6">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="1*"/>   <!-- Liste -->
        <ColumnDefinition Width="2*"/>   <!-- Detail -->
    </Grid.ColumnDefinitions>
    <Grid.RowDefinitions>
        <RowDefinition Height="*"/>      <!-- Inhalt -->
        <RowDefinition Height="Auto"/>   <!-- Button-Leiste -->
    </Grid.RowDefinitions>

    <ListBox x:Name="ItemList" Grid.Column="0" Grid.Row="0"
             SelectionChanged="OnItemSelected"/>

    <!-- Detail-Formular -->
    <StackPanel Grid.Column="1" Grid.Row="0" Margin="12,0,0,0">
        <TextBlock Text="Name:"/>
        <TextBox x:Name="NameBox"/>
        <TextBlock Text="Flaeche (ha):" Margin="0,8,0,0"/>
        <TextBox x:Name="AreaBox"/>
        <TextBlock Text="Bemerkung:" Margin="0,8,0,0"/>
        <TextBox x:Name="NoteBox" AcceptsReturn="True" Height="80"/>
    </StackPanel>

    <StackPanel Grid.Column="1" Grid.Row="1" Orientation="Horizontal"
                HorizontalAlignment="Right" Margin="0,8,0,0">
        <Button Content="Neu"       Click="OnNew_Click"    Width="80" Margin="0,0,6,0"/>
        <Button Content="Speichern" Click="OnSave_Click"   Width="80" Margin="0,0,6,0"/>
        <Button Content="Loeschen"  Click="OnDelete_Click" Width="80"/>
    </StackPanel>
</Grid>
```

## Selektions-Sync: Liste -> Formular
```csharp
// Liste befuellen (Repository siehe 07_EFCore_SQLite.md)
private void RefreshList()
{
    ItemList.ItemsSource = null;            // erzwingt Neuaufbau
    ItemList.ItemsSource = _store.GetAll(); // List<Forest> o.ae.
    ItemList.DisplayMemberPath = "Name";    // welche Property anzeigen
}

// Auswahl geaendert -> Formular fuellen
private void OnItemSelected(object sender, SelectionChangedEventArgs e)
{
    if (ItemList.SelectedItem is Forest f)   // dein Entity-Typ
    {
        NameBox.Text = f.Name;
        AreaBox.Text = f.Area.ToString();
        NoteBox.Text = f.Note;
    }
}
```

## Formular -> Datensatz (Speichern)
```csharp
private void OnSave_Click(object sender, RoutedEventArgs e)
{
    // Eingabe VALIDIEREN, bevor gespeichert wird (gibt oft Punkte):
    double area;
    if (!double.TryParse(AreaBox.Text, out area))
    {
        MessageBox.Show("Flaeche muss eine Zahl sein.", "Eingabefehler",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    if (ItemList.SelectedItem is Forest f)       // bestehenden aendern
        _store.Update(f.Id, NameBox.Text, area, NoteBox.Text);
    else                                          // oder neu anlegen
        _store.Add(NameBox.Text, area, NoteBox.Text);

    RefreshList();
}

private void OnNew_Click(object sender, RoutedEventArgs e)
{
    ItemList.SelectedItem = null;   // Auswahl aufheben = "Neu-Modus"
    NameBox.Text = ""; AreaBox.Text = ""; NoteBox.Text = "";
    NameBox.Focus();
}

private void OnDelete_Click(object sender, RoutedEventArgs e)
{
    if (!(ItemList.SelectedItem is Forest f)) return;

    // Sicherheitsabfrage (typischer Punkt bei Verwaltungs-Aufgaben):
    var result = MessageBox.Show("'" + f.Name + "' wirklich loeschen?",
        "Loeschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
    if (result != MessageBoxResult.Yes) return;

    _store.Delete(f.Id);
    RefreshList();
}
```

## Mehrere Properties in der Liste anzeigen (statt DisplayMemberPath)
```xml
<ListBox x:Name="ItemList">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="{Binding Name}" FontWeight="Bold"/>
                <TextBlock Text="{Binding Area, StringFormat=' ({0} ha)'}"/>
            </StackPanel>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

## Alternative: DataGrid (Tabelle statt Liste)
```xml
<DataGrid x:Name="ItemGrid" AutoGenerateColumns="False" IsReadOnly="True"
          SelectionChanged="OnItemSelected">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Name"    Binding="{Binding Name}" Width="*"/>
        <DataGridTextColumn Header="Flaeche" Binding="{Binding Area}" Width="Auto"/>
    </DataGrid.Columns>
</DataGrid>
```
`ItemGrid.ItemsSource = _store.GetAll();` - SelectedItem-Logik identisch zur ListBox.

## ObservableCollection (falls die Liste live mitgehen soll)
```csharp
using System.Collections.ObjectModel;

private ObservableCollection<Forest> _items;

_items = new ObservableCollection<Forest>(_store.GetAll());
ItemList.ItemsSource = _items;     // EINMAL setzen

_items.Add(neu);        // ListBox aktualisiert sich automatisch
_items.Remove(alt);     // ebenso
```
Fuer die PA reicht meist das simple `RefreshList()`-Muster - weniger Fehlerquellen.
ObservableCollection nur, wenn die Angabe "automatische Aktualisierung" verlangt.

## Stolperfallen
- `SelectionChanged` feuert auch bei `SelectedItem = null` und beim
  `ItemsSource`-Neusetzen -> immer mit `is`-Pattern pruefen (siehe oben).
- Nach Speichern/Loeschen `RefreshList()` nicht vergessen, sonst zeigt die
  Liste alte Daten.
- `double.Parse` wirft bei Komma/Punkt-Problemen -> `TryParse` verwenden.
