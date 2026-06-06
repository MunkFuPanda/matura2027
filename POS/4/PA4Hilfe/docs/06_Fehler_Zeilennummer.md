# Fehlerbehandlung + Zeilennummer (Teil 7)

Der Teil, der am Schluss "draufgebolzt" wird - aber die billigsten Punkte, wenn
das Geruest steht. Ziel: **alle Fehler im Beispiel-Code erkennen und eine
aussagekraeftige Meldung mit Zeilennummer ausgeben.**

## Zwei Fehlerklassen, die die Angaben testen
| Klasse | Beispiel | Wo erkannt |
|--------|----------|------------|
| **Token-/Wertfehler** | ungueltige Richtung `DIAGONAL`, Farbe `Purple`, Zahl wo Keyword erwartet | im Lexer oder beim `Expect`/Wertpruefung |
| **Strukturfehler** | fehlende `}`, `FOR 12` ohne Block, leeres `COLOR` | beim Klammer-Matching im Parser |

Die zwei Fehler-Beispieldateien (`Examples/fehler1.mosaik`, `fehler2.mosaik`)
decken genau diese zwei ab.

## Schritt 1: Zeile/Spalte ins Token packen
Schon im Lexer (siehe `Lexer.cs`): jedes `Token` traegt `Line` und `Column`.
Beim Verschlucken von Whitespace die `\n` zaehlen:
```csharp
foreach (char c in text)
    if (c == '\n') { line++; lineStart = pos + 1; }
// Spalte = pos - lineStart + 1
```

## Schritt 2: Eigene Exception mit Position
```csharp
public class ParseException : Exception
{
    public int Line { get; }
    public int Column { get; }
    public ParseException(string message, int line, int column)
        : base("Zeile " + line + ", Spalte " + column + ": " + message)
    { Line = line; Column = column; }
}
```

## Schritt 3: An den richtigen Stellen werfen
Immer das **aktuelle Token** fuer Zeile/Spalte verwenden:
```csharp
// Falscher Token-Typ
throw new ParseException("Erwartet eine Zahl, gefunden '" + t.Text + "'",
                         t.Line, t.Column);

// Richtiger Typ, ungueltiger Wert
if (!Colors.Contains(c.Text))
    throw new ParseException("Ungueltige Farbe '" + c.Text + "'", c.Line, c.Column);

// Struktur: '}' fehlt (Datei zu Ende)
if (Peek().Type == TokenType.End)
    throw new ParseException("Erwartet '}', aber Programmende erreicht",
                             Peek().Line, Peek().Column);
```

## Schritt 4: In der GUI fangen und anzeigen
```csharp
try
{
    var frames = interp.Run(CodeBox.Text);
    // ... animieren
}
catch (ParseException ex)
{
    StatusText.Text = "SYNTAXFEHLER: " + ex.Message;   // enthaelt schon "Zeile X"
    StatusText.Foreground = Brushes.Red;
}
catch (RuntimeException ex)
{
    StatusText.Text = "LAUFZEITFEHLER: " + ex.Message;
    StatusText.Foreground = Brushes.Red;
}
```

## Erwartete Ausgaben (gegen die Beispiele gepruefft)
- `fehler1.mosaik` -> `Zeile 3, Spalte 6: Ungueltige Richtung 'DIAGONAL'`
- `fehler2.mosaik` -> `Zeile 4, Spalte 1: Erwartet '}', aber Programmende erreicht`

## Tipp fuer maximale Punkte
- **Eine** Fehlermeldung mit Zeile reicht oft fuer die volle Wertung. Sammle
  nicht alle Fehler auf einmal - der erste mit korrekter Zeile genuegt meist.
- Schreib in die Meldung, **was** erwartet wurde UND **was** gefunden wurde.
  ("Erwartet '}', gefunden 'MOVE'") - das ist die "aussagekraeftige Meldung",
  die die Angabe verlangt.
- Laufzeitfehler (Feld verlassen) nicht vergessen - eigener `catch`.
