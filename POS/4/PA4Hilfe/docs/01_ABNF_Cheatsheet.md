# ABNF-Cheatsheet (Teil 1: Grammatik)

## Wofuer brauche ich die ABNF?
Die ABNF wird **nirgends im Code geparst**. Sie ist reine
Dokumentation/Spezifikation - ein eigenes Abgabe-Stueck. Niemand fuettert den
ABNF-Text in eine Bibliothek.

Der eigentliche Nutzen: **jede ABNF-Regel wird zu genau einer `ParseXxx()`-Methode**
im rekursiven Abstieg. Die ABNF ist der Bauplan, den du von Hand in C# uebersetzt.
Schreibst du sie zuerst sauber hin, schreibt sich der Parser fast von selbst.

## ABNF-Notation (die Symbole, die du brauchst)
| Symbol | Bedeutung | Wird im Parser zu |
|--------|-----------|-------------------|
| `=` | Regel-Definition | eine `ParseXxx()`-Methode |
| `/` | Alternative (oder) | `switch` / `if-else` |
| `*element` | 0 oder mehr | `while`-Schleife |
| `1*element` | 1 oder mehr | `do...while` / mind. 1x dann `while` |
| `[element]` | optional (0 oder 1) | `if (peek == ...)` |
| `( ... )` | Gruppierung | Klammerung im Code |
| `"text"` | woertliches Schluesselwort | `ExpectWord("text")` |
| `SP` | ein Leerzeichen | (vom Lexer geschluckt) |
| `DIGIT` | eine Ziffer 0-9 | Teil von `number` |

## Vollstaendige Grammatik der Mosaik-Sprache (= Vorlage)
```abnf
program    = *statement

statement  = setcolor / move / repeat / while / if

setcolor   = "SET" SP "COLOR" SP color
move       = "MOVE" SP direction [ SP number ]
repeat     = "REPEAT" SP number SP block
while      = "WHILE" SP "CAN" SP direction SP block
if         = "IF" SP "CAN" SP direction SP block

block      = "{" *statement "}"

direction  = "UP" / "DOWN" / "LEFT" / "RIGHT"
color      = "Black" / "Red" / "Green" / "Blue" / "Yellow" / "White"
number     = 1*DIGIT
```

## Direkte Uebersetzung Regel -> Parser-Methode
ABNF:
```abnf
repeat = "REPEAT" SP number SP block
```
wird 1:1 zu:
```csharp
private IStatement ParseRepeat()
{
    ExpectWord("REPEAT");                          // "REPEAT"
    Token n = Expect(TokenType.Number, "Zahl");    // number
    Block body = ParseBlock();                     // block
    return new RepeatStatement(int.Parse(n.Text), body);
}
```

ABNF:
```abnf
block = "{" *statement "}"
```
wird zu (das `*statement` ist die `while`-Schleife):
```csharp
private Block ParseBlock()
{
    Expect(TokenType.LBrace, "'{'");
    var block = new Block();
    while (Peek().Type != TokenType.RBrace)     // *statement
        block.Statements.Add(ParseStatement());
    Expect(TokenType.RBrace, "'}'");
    return block;
}
```

## Merksatz
`/` -> `switch`/`if` · `*x` -> `while` · `[x]` -> `if (peek...)` · `"WORT"` ->
`ExpectWord`. Wenn ABNF und Parser-Methoden nebeneinanderliegen, ist es eine
direkte Uebersetzungstabelle.

## Hinweis zu Parser-Generatoren
Es gibt Werkzeuge (ANTLR, yacc), die eine Grammatik einlesen und den Parser
automatisch erzeugen. Das ist hier **explizit nicht gewollt** - die Angabe
verlangt das Interpreter-Pattern von Hand. ABNF bleibt Papier/Kommentar.
