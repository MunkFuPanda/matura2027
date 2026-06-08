# Datei-I/O: Textdateien & CSV einlesen/exportieren (Waldwunder-Teil)

Waldwunder-Muster: Daten kommen aus einer Datei (CSV/Text), werden eingelesen,
in Objekte umgewandelt und in die DB importiert bzw. in einer Liste angezeigt.

## Textdatei komplett lesen / schreiben
```csharp
using System.IO;

string text   = File.ReadAllText(pfad);            // alles als ein String
string[] rows = File.ReadAllLines(pfad);           // Zeilen-Array

File.WriteAllText(pfad, inhalt);                   // schreiben (ueberschreibt)
File.WriteAllLines(pfad, zeilenListe);
```
Encoding-Falle bei Umlauten: `File.ReadAllLines(pfad, Encoding.UTF8)` bzw.
`Encoding.GetEncoding("ISO-8859-1")` fuer alte Windows-Dateien
(`using System.Text;`). Wenn ä/ö/ü kaputt aussehen -> Encoding wechseln.

## CSV-Zeilen in Objekte umwandeln (das Kernmuster)
CSV-Beispiel `waelder.csv`:
```
Name;Flaeche;Bezirk
Kobernausserwald;120,5;Braunau
Hausruck;89,0;Ried
```
```csharp
public List<Forest> ImportCsv(string pfad)
{
    var result = new List<Forest>();
    string[] lines = File.ReadAllLines(pfad);

    // i = 1: Kopfzeile ueberspringen!
    for (int i = 1; i < lines.Length; i++)
    {
        string line = lines[i].Trim();
        if (line.Length == 0) continue;          // Leerzeilen ignorieren

        string[] parts = line.Split(';');
        if (parts.Length < 3)                    // kaputte Zeile -> melden
            throw new FormatException("Zeile " + (i + 1) + ": erwartet 3 Spalten, "
                                      + "gefunden " + parts.Length);

        double area;
        // Achtung Dezimal-KOMMA in oesterreichischen Dateien:
        if (!double.TryParse(parts[1], NumberStyles.Float,
                             CultureInfo.GetCultureInfo("de-AT"), out area))
            throw new FormatException("Zeile " + (i + 1) + ": '" + parts[1]
                                      + "' ist keine Zahl");

        result.Add(new Forest {
            Name = parts[0].Trim(),
            Area = area,
            District = parts[2].Trim()
        });
    }
    return result;
}
// usings: System.IO, System.Globalization
```
Fehler mit **Zeilennummer** melden - dasselbe Prinzip wie beim Parser (Teil 7),
gibt auch hier Punkte.

## Import in die DB (kombiniert mit 07_EFCore_SQLite.md)
```csharp
private void OnImport_Click(object sender, RoutedEventArgs e)
{
    var dlg = new OpenFileDialog { Filter = "CSV-Dateien (*.csv)|*.csv" };
    if (dlg.ShowDialog() != true) return;

    try
    {
        List<Forest> items = ImportCsv(dlg.FileName);
        using (var db = new AppDbContext())
        {
            db.Forests.AddRange(items);   // alle auf einmal
            db.SaveChanges();
        }
        RefreshList();
        SetStatus(items.Count + " Datensaetze importiert.", false);
    }
    catch (FormatException ex)
    {
        SetStatus("IMPORTFEHLER: " + ex.Message, true);
    }
}
```

## Export (SaveFileDialog + CSV schreiben)
```csharp
var dlg = new SaveFileDialog
{
    Filter = "CSV-Dateien (*.csv)|*.csv",
    FileName = "export.csv"
};
if (dlg.ShowDialog() == true)
{
    var lines = new List<string> { "Name;Flaeche;Bezirk" };   // Kopfzeile
    foreach (var f in _store.GetAll())
        lines.Add(f.Name + ";" + f.Area.ToString(CultureInfo.GetCultureInfo("de-AT"))
                  + ";" + f.District);
    File.WriteAllLines(dlg.FileName, lines);
}
```

## Doppelte beim Import vermeiden (typische Zusatzanforderung)
```csharp
using (var db = new AppDbContext())
{
    foreach (var item in items)
    {
        bool exists = db.Forests.Any(f => f.Name == item.Name);  // LINQ
        if (!exists) db.Forests.Add(item);
    }
    db.SaveChanges();
}
```

## Stolperfallen-Checkliste
- Kopfzeile ueberspringen (`i = 1`).
- Leerzeilen am Dateiende (`Trim` + `continue`).
- Dezimal-Komma vs. -Punkt -> immer `TryParse` mit `CultureInfo`.
- Trennzeichen pruefen: `;` (deutsch/oesterreichisch ueblich) vs `,`.
- Umlaute kaputt -> Encoding-Parameter setzen.
