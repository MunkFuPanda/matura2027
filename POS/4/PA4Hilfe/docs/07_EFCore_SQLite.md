# EF Core + SQLite + LINQ (Waldwunder-Variante)

Kommt evtl. statt Teil 6 oder 7: Programme/Ergebnisse speichern und per LINQ
suchen. Vollstaendig in `MosaikWerkstatt/Persistence/`.

## NuGet-Paket
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.6" />
```
Auf .NET Framework ggf. aeltere EF-Core-Version (z.B. 3.1.x) - im Vorgabe-Projekt
nachschauen, welche schon eingebunden ist.

## 1. Entity (= eine Tabellenzeile)
```csharp
public class SavedProgram
{
    public int Id { get; set; }          // EF erkennt "Id" als Primaerschluessel
    public string Name { get; set; }
    public string Source { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## 2. DbContext (Bruecke zu SQLite)
```csharp
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<SavedProgram> SavedPrograms { get; set; }   // eine pro Tabelle

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=mosaik.db");  // Datei im Programmordner
    }
}
```

## 3. Tabellen anlegen (ohne Migrations - simpel)
```csharp
using (var db = new AppDbContext())
    db.Database.EnsureCreated();   // legt DB + Tabellen an, falls nicht da
```

## 4. CRUD + LINQ
```csharp
// CREATE
using (var db = new AppDbContext())
{
    db.SavedPrograms.Add(new SavedProgram {
        Name = name, Source = code, CreatedAt = DateTime.Now });
    db.SaveChanges();                                  // schreibt in die DB
}

// READ (alle, sortiert)
using (var db = new AppDbContext())
{
    var list = db.SavedPrograms
                 .OrderByDescending(p => p.CreatedAt)
                 .ToList();
}

// READ mit Filter (Suche)
var treffer = db.SavedPrograms
                .Where(p => p.Name.Contains(term))
                .OrderBy(p => p.Name)
                .ToList();

// UPDATE
var p = db.SavedPrograms.First(x => x.Id == id);
p.Name = "neu";
db.SaveChanges();

// DELETE
var e = db.SavedPrograms.FirstOrDefault(x => x.Id == id);
if (e != null) { db.SavedPrograms.Remove(e); db.SaveChanges(); }
```

## LINQ-Spickzettel (haeufig gebraucht)
| Methode | Tut |
|---------|-----|
| `.Where(p => ...)` | filtern |
| `.OrderBy / .OrderByDescending` | sortieren |
| `.Select(p => p.Name)` | projizieren (nur ein Feld) |
| `.First() / .FirstOrDefault()` | erstes (oder null) |
| `.Single() / .SingleOrDefault()` | genau eines |
| `.Any(p => ...)` | gibt es eins? (bool) |
| `.Count()` | Anzahl |
| `.ToList()` | Abfrage ausfuehren -> Liste |

## In die GUI haengen (siehe ProgramStore.cs)
Kapsle alle DB-Zugriffe in eine Repository-Klasse (`ProgramStore`), die GUI ruft
nur `Save` / `GetAll` / `Search` / `Delete` auf. Dann sieht die GUI von EF Core
nichts und du kannst es leicht erklaeren.

## Stolperfalle
`DbContext` ist nicht thread-safe und sollte kurzlebig sein -> immer in `using`
erzeugen und gleich wieder schliessen (so wie oben). Nicht ein Context-Objekt
durch die ganze App reichen.
