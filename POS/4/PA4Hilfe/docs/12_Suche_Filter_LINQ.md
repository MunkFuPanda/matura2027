# Suche & Filter mit LINQ in der GUI (Waldwunder-Teil)

Waldwunder-Muster: Suchfeld + Filter ueber den Datenbestand, Ergebnis in der
ListBox/DataGrid. Baut direkt auf `07_EFCore_SQLite.md` und
`10_Verwaltungs_GUI_MasterDetail.md` auf.

## Einfache Suche: TextBox + Button
```xml
<StackPanel Orientation="Horizontal" Margin="0,0,0,6">
    <TextBox x:Name="SearchBox" Width="200"/>
    <Button Content="Suchen" Click="OnSearch_Click" Margin="6,0,0,0" Width="80"/>
    <Button Content="Alle" Click="OnShowAll_Click" Margin="6,0,0,0" Width="60"/>
</StackPanel>
```
```csharp
private void OnSearch_Click(object sender, RoutedEventArgs e)
{
    string term = SearchBox.Text.Trim();
    using (var db = new AppDbContext())
    {
        ItemList.ItemsSource = db.Forests
            .Where(f => f.Name.Contains(term))
            .OrderBy(f => f.Name)
            .ToList();
    }
}

private void OnShowAll_Click(object sender, RoutedEventArgs e)
{
    RefreshList();   // alles wieder anzeigen
}
```

## Live-Suche waehrend des Tippens (TextChanged)
```xml
<TextBox x:Name="SearchBox" TextChanged="OnSearch_TextChanged"/>
```
```csharp
private void OnSearch_TextChanged(object sender, TextChangedEventArgs e)
{
    string term = SearchBox.Text.Trim();
    using (var db = new AppDbContext())
    {
        ItemList.ItemsSource = (term.Length == 0)
            ? db.Forests.OrderBy(f => f.Name).ToList()
            : db.Forests.Where(f => f.Name.Contains(term))
                        .OrderBy(f => f.Name).ToList();
    }
}
```
Bei kleinen Datenmengen (PA-Groesse) voellig ok, jede Aenderung fragt die DB ab.

## Gross-/Kleinschreibung ignorieren
SQLite-`Contains` ist bei ASCII meist schon case-insensitive. Wer sichergehen
will, filtert im Speicher:
```csharp
var all = db.Forests.ToList();   // erst holen...
var hits = all.Where(f => f.Name.ToLower().Contains(term.ToLower())).ToList();
```

## Filter ueber ComboBox (Kategorie/Bezirk)
```xml
<ComboBox x:Name="DistrictFilter" Width="150"
          SelectionChanged="OnFilter_Changed"/>
```
```csharp
// Befuellen: alle vorkommenden Bezirke + "(alle)" als erster Eintrag
private void FillFilter()
{
    using (var db = new AppDbContext())
    {
        var districts = db.Forests
            .Select(f => f.District)
            .Distinct()
            .OrderBy(d => d)
            .ToList();
        districts.Insert(0, "(alle)");
        DistrictFilter.ItemsSource = districts;
        DistrictFilter.SelectedIndex = 0;
    }
}

private void OnFilter_Changed(object sender, SelectionChangedEventArgs e)
{
    string d = DistrictFilter.SelectedItem as string;
    using (var db = new AppDbContext())
    {
        ItemList.ItemsSource = (d == null || d == "(alle)")
            ? db.Forests.ToList()
            : db.Forests.Where(f => f.District == d).ToList();
    }
}
```

## Mehrere Kriterien kombinieren (Abfrage schrittweise aufbauen)
```csharp
using (var db = new AppDbContext())
{
    IQueryable<Forest> q = db.Forests;             // noch keine DB-Abfrage!

    if (term.Length > 0)
        q = q.Where(f => f.Name.Contains(term));
    if (district != "(alle)")
        q = q.Where(f => f.District == district);
    if (minArea > 0)
        q = q.Where(f => f.Area >= minArea);

    ItemList.ItemsSource = q.OrderBy(f => f.Name).ToList();  // HIER laeuft sie
}
```
Das `IQueryable`-Muster ist der sauberste Weg fuer "Suche + Filter gleichzeitig" -
jede Bedingung haengt nur an, ausgefuehrt wird erst bei `ToList()`.

## Aggregat-Anzeigen (typische Zusatzpunkte)
```csharp
using (var db = new AppDbContext())
{
    int    count = db.Forests.Count();
    double total = db.Forests.Sum(f => f.Area);
    double avg   = db.Forests.Average(f => f.Area);
    var biggest  = db.Forests.OrderByDescending(f => f.Area).First();

    StatusText.Text = count + " Waelder, " + total + " ha gesamt";
}
```

## Stolperfallen
- Nach jedem Filter `DisplayMemberPath`/`ItemTemplate` bleibt erhalten -
  nur `ItemsSource` neu setzen, mehr nicht.
- `Average`/`First` werfen bei leerer Tabelle -> vorher `Any()` pruefen.
- Filter + Detail-Formular: nach dem Filtern kann `SelectedItem` null sein ->
  `is`-Pattern in `SelectionChanged` (siehe `10_...md`) faengt das ab.
