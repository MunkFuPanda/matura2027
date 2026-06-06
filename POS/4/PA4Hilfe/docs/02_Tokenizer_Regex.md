# Tokenizer / Regex-Cheatsheet (Teil 4)

Der Tokenizer wandelt rohen Text in eine Liste von Tokens. Halte den Lexer
**dumm**: er klassifiziert nur grob (Wort/Zahl/Klammer). Ob ein Wort ein
Schluesselwort, eine Richtung oder eine Farbe ist, entscheidet erst der Parser.
-> Bei neuer Angabe musst du den Lexer kaum anfassen.

## Variante A: Eine Master-Regex mit benannten Gruppen (empfohlen)
Genau der Stil, den die Painter-Angabe verlangt. Siehe `MosaikWerkstatt/Language/Lexer.cs`.

```csharp
private static readonly Regex TokenRegex = new Regex(
    @"\G(" +
    @"(?<WS>\s+)" +                       // Whitespace -> verwerfen
    @"|(?<NUMBER>\d+)" +                  // Zahl
    @"|(?<WORD>[A-Za-z][A-Za-z\-]*)" +    // Wort (Buchstaben + '-')
    @"|(?<LBRACE>\{)" +
    @"|(?<RBRACE>\})" +
    @")",
    RegexOptions.Compiled);
```
- `\G` = "matche genau ab der angegebenen Position" (kein Ueberspringen).
- Erste passende Gruppe gewinnt -> Reihenfolge zaehlt.
- Pruefe `m.Index == pos`; wenn nicht, hast du ein ungueltiges Zeichen -> Fehler
  mit Zeile/Spalte werfen.

## Variante B: Eine Regex je Token-Typ (auch ok, etwas mehr Code)
```csharp
var specs = new (string Name, string Pattern)[] {
    ("WS",     @"\s+"),
    ("NUMBER", @"\d+"),
    ("WORD",   @"[A-Za-z][A-Za-z\-]*"),
    ("LBRACE", @"\{"),
    ("RBRACE", @"\}"),
};
```
In Schleife jede `Regex.Match(src, pos)` mit `\G`-Anker probieren.

## Variante C: Quick & dirty mit Split (nur wenn Zeit knapp)
Wenn die Sprache nur durch Leerzeichen/Zeilen getrennte Tokens hat und `{`/`}`
immer von Leerzeichen umgeben sind:
```csharp
string[] raw = source.Split(new[] {' ', '\t', '\r', '\n'},
                            StringSplitOptions.RemoveEmptyEntries);
```
Nachteil: keine Zeilennummern, `{MOVE` ohne Leerzeichen wird ein Token.
Nur als Notnagel.

## Regex-Bausteine, die du oft brauchst
| Muster | Trifft |
|--------|--------|
| `\d+` | eine oder mehr Ziffern |
| `-?\d+` | ganze Zahl mit optionalem Minus |
| `\d+(\.\d+)?` | Dezimalzahl |
| `[A-Za-z]+` | nur Buchstaben |
| `[A-Za-z][A-Za-z0-9]*` | Bezeichner (Buchstabe, dann alphanum.) |
| `"[^"]*"` | String in Anfuehrungszeichen |
| `\s+` | Whitespace |
| `\{` `\}` `\(` `\)` | geschweifte/runde Klammern (escapen!) |
| `#.*` | Kommentar bis Zeilenende |

## Zeilennummern korrekt mitzaehlen
Beim Verschlucken von Whitespace die `\n` zaehlen und `lineStart` merken
(Spalte = `pos - lineStart + 1`). Siehe `Lexer.cs`. Das ist die Grundlage fuer
Teil 7 (Fehler mit Zeilennummer).

## Was du bei neuer Angabe am Lexer aenderst
- Neue Symbole (`(`, `)`, `,`, `+`)? -> je eine benannte Gruppe ergaenzen.
- Strings/Dezimalzahlen erlaubt? -> Muster aus der Tabelle oben einsetzen.
- Alles andere (Zeilenzaehlung, EOF-Token, Fehlerwurf) bleibt gleich.
