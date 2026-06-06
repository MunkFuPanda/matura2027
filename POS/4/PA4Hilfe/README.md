# PA-Hilfe: Interpreter + WPF (C#)

Dein kompletter Werkzeugkasten fuer die 2-stuendige PA am Montag. Alles dreht sich
um EIN Skelett: **Text -> Tokenizer (Regex) -> Parser (Interpreter-Pattern) ->
Fehlermeldungen -> getaktete Visualisierung**, plus die EF-Core-Variante aus
Waldwunder.

## Wo fange ich an?
1. **`docs/00_PA_Spielplan.md`** lesen - Zeitbudget + die ersten 5 Minuten.
2. **Das Referenzprojekt einmal bauen und laufen lassen** (siehe unten). Wenn du
   die `Examples/` durchspielst und die Ausgaben stimmen, sitzt das Skelett.
3. Die `docs/`-Cheatsheets sind je eine Teilaufgabe - am Montag pro Teil den
   passenden aufschlagen.

## Ordnerinhalt
```
PA4Hilfe/
├─ README.md                  <- diese Datei
├─ ANGABE_uebung.md           <- Uebungs-Angabe + handgetracete Soll-Ausgaben
├─ docs/
│  ├─ 00_PA_Spielplan.md      Strategie, Zeitbudget, 5-Minuten-Checkliste
│  ├─ 01_ABNF_Cheatsheet.md   Teil 1: Grammatik + Uebersetzungstabelle
│  ├─ 02_Tokenizer_Regex.md   Teil 4: Lexer mit Regex
│  ├─ 03_Interpreter_Pattern.md Teil 2: AST-Klassen + UML
│  ├─ 04_Parser.md            Teil 5: rekursiver Abstieg
│  ├─ 05_WPF_Cheatsheet.md    Teil 3+6: GUI, OpenFileDialog, Timer, Grid
│  ├─ 06_Fehler_Zeilennummer.md Teil 7: Fehlerbehandlung
│  ├─ 07_EFCore_SQLite.md     Waldwunder-Variante: DB + LINQ
│  ├─ 08_XML_laden.md         Feld aus XML laden (Roboter-Stil)
│  └─ 09_CustomControl_DLL.md falls eine .dll mitkommt
└─ MosaikWerkstatt/           <- lauffaehiges Referenzprojekt (.NET 8 WPF)
   ├─ MosaikWerkstatt.csproj
   ├─ App.xaml(.cs)
   ├─ MainWindow.xaml(.cs)    die "Verdrahtung"
   ├─ InputDialog.xaml(.cs)   wiederverwendbarer Eingabe-Dialog
   ├─ Language/               GUI-FREIE Sprach-Library (drop-in)
   │  ├─ Token.cs
   │  ├─ Lexer.cs             Tokenizer (Regex)
   │  ├─ Exceptions.cs        ParseException / RuntimeException
   │  ├─ Ast.cs               IStatement + alle Anweisungs-Klassen
   │  ├─ Context.cs           Laufzeit-Zustand + Frames fuer Animation
   │  ├─ Parser.cs            rekursiver Abstieg
   │  └─ MosaikInterpreter.cs Fassade: Run(source) -> Frames
   ├─ Persistence/            EF Core / SQLite
   │  ├─ SavedProgram.cs      Entity
   │  ├─ AppDbContext.cs      DbContext
   │  └─ ProgramStore.cs      Repository (CRUD + LINQ)
   └─ Examples/               Beispiel- und Fehlerprogramme
```

## Das Wichtigste in einem Satz
Der Ordner `MosaikWerkstatt/Language/` ist **komplett GUI-frei**. Bei einem
Vorgabe-Projekt am Montag wirfst du diese `.cs`-Dateien einfach hinein und
schreibst im vorgegebenen Code-Behind nur noch 5 Zeilen Verdrahtung:
```csharp
var interp = new MosaikInterpreter(rows, cols);
var frames = interp.Run(CodeBox.Text);   // wirft ParseException/RuntimeException
foreach (var f in frames) { RenderFrame(f); await Task.Delay(1000); }
```

## Projekt bauen & starten (Visual Studio)
1. `MosaikWerkstatt/MosaikWerkstatt.csproj` oeffnen.
2. F5 (Debug starten). Beim ersten Build laedt NuGet das EF-Core-Paket.
3. Im Fenster: Code steht schon drin -> **"Ausfuehren"** klicken -> rotes Quadrat
   wird Zelle fuer Zelle gemalt (1 s Pause).
4. **"Laden..."** -> eine Datei aus `Examples/` -> ausfuehren/parsen.

Kein Visual Studio zur Hand? `dotnet run` im Projektordner (mit installiertem
.NET-8-SDK + Windows, da WPF).

## Die Mosaik-Sprache (Kurzreferenz)
```
SET COLOR <Black|Red|Green|Blue|Yellow|White>
MOVE <UP|DOWN|LEFT|RIGHT> [anzahl]
REPEAT <zahl> { ... }
WHILE CAN <richtung> { ... }
IF CAN <richtung> { ... }
```
Cursor startet oben-links (0,0). `MOVE` faerbt die Zielzelle, `SET COLOR` faerbt
die aktuelle Zelle. Vollstaendige Grammatik in `docs/01_ABNF_Cheatsheet.md`.

## Anpassen an eine neue Angabe (der ganze Trick)
- **Lexer**: meist unveraendert; nur neue Symbole als Regex-Gruppe ergaenzen.
- **Parser**: nur den `switch` in `ParseStatement` + die gueltigen Mengen
  (`Directions`/`Colors`) + neue `ParseXxx`-Methoden.
- **AST**: pro neuer Anweisung eine Klasse mit `Interpret(Context)`.
- **Context/Visualisierung**: die Methoden, die die neue Sprache braucht
  (`DrawLine`, `Turn`, `Collect`...). Bei Custom-Control-DLL: nur hier die
  Control-Methoden aufrufen statt eigenes Gitter.
- Peek/Next/Expect, das Schleifenmuster, Fehlerbehandlung: **bleiben gleich**.
