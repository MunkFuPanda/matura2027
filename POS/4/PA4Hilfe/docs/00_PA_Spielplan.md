# PA-Spielplan (2 Stunden)

Das Wichtigste zuerst: **vier Teile grob fertig schlagen 35 Punkte, einen Teil
perfektionieren bringt dich durch.** Die ersten vier Teilaufgaben sind die
billigen, voneinander unabhaengigen Punkte.

## Punkte-Mathematik
- 7 Teile, 70 Punkte, **35 zum Bestehen**.
- Grammatik + AST-Klassen + GUI-Skelett + Tokenizer = ~40 Punkte und alle
  unabhaengig voneinander. Das ist dein sicheres Ziel in Stunde 1.
- Parser + Ausfuehrung + Fehlerbehandlung = Stunde 2 (dort gibt es Teilpunkte,
  auch wenn nicht alles perfekt laeuft).

## Zeitbudget (Richtwert)
| Zeit | Teil | Hinweis |
|------|------|---------|
| 0:00-0:05 | **Vorgabe-Projekt checken** | siehe Checkliste unten |
| 0:05-0:20 | ABNF-Grammatik | direkt aus `01_ABNF_Cheatsheet.md` ableiten |
| 0:20-0:35 | AST-/Interpreter-Klassen | `Ast.cs` kopieren, Klassen umbenennen |
| 0:35-0:50 | GUI-Skelett | `MainWindow.xaml` kopieren, NICHT polieren |
| 0:50-1:10 | Tokenizer (Regex) | `Lexer.cs` kopieren, Symbole anpassen |
| 1:10-1:40 | Parser | `Parser.cs` kopieren, `ParseStatement`-switch anpassen |
| 1:40-1:55 | Ausfuehrung/Visualisierung | Frames getaktet abspielen |
| 1:55-2:00 | Fehlerbehandlung feilen | aussagekraeftige Meldungen + Zeilennummer |

## Erste 5 Minuten, wenn ein Vorgabe-Projekt da ist
1. **Kompiliert es so wie es ist?** Erst bauen, dann anfassen. Fehler im
   Vorgabe-Code koennen Absicht sein (TODOs).
2. **Namespace + Target Framework ablesen** (`.csproj`). Bestimmt, ob du modernes
   C# nehmen darfst oder die konservative Variante brauchst (.NET Framework).
3. **Nach Stubs suchen:** leere Methoden, `throw new NotImplementedException()`,
   abstrakte Klassen, `// TODO`. Das ist deine To-do-Liste.
4. **Referenzen checken:** ist eine `.dll` eingebunden? Liegen `.xml`-Datendateien
   dabei (Feld wird geladen statt fix)?
5. **Dann erst** deine Logik einsetzen - Klasse fuer Klasse, in die vorgegebene
   Struktur. Die Sprach-Klassen (Lexer/Parser/AST/Context) als eigene `.cs`
   dazulegen, nur die Verdrahtung im Code-Behind ergaenzen.

## Der Denkfehler, den du vermeiden willst
Dein komplettes `MainWindow` ueber das vorgegebene drueberkopieren. Stattdessen:
Sprach-Logik als eigene Dateien dazu, im vorgegebenen Code-Behind nur die Kette
**"Datei laden -> tokenize -> parse -> interpret -> zeichnen"** verdrahten.

## Faustregel bei Vorgabe-Projekt
Kommt etwas im Vorgabe-Projekt schon vor (eine `Context`-Klasse, ein
`IStatement`-Interface), **nimmst du das** und implementierst dagegen. Nur was
fehlt, bringst du aus deinen Snippets mit. Sonst hast du zwei `Context`-Klassen,
die sich beissen.

## Reihenfolge der Dateien in diesem Ordner
- `docs/01..09` = Cheatsheets, einer pro Teilaufgabe.
- `MosaikWerkstatt/` = komplettes lauffaehiges Referenzprojekt. Einmal vorm
  Montag bauen und laufen lassen -> dann sitzt das Muskelgedaechtnis.
- `MosaikWerkstatt/Examples/` = Beispiel- und Fehlerprogramme zum Gegentesten.
