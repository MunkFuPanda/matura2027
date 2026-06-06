# Parser-Cheatsheet (Teil 5: rekursiver Abstieg)

Der Parser baut aus der Token-Liste den AST. **Jede ABNF-Regel = eine
`ParseXxx()`-Methode.** Voll ausgefuehrt in `Language/Parser.cs`.

## Die drei Helfer (sprachunabhaengig - immer gleich)
```csharp
private Token Peek()  { return _tokens[_pos]; }       // anschauen, nicht verbrauchen
private Token Next()  { return _tokens[_pos++]; }     // verbrauchen

private Token Expect(TokenType type, string what)     // Typ erwarten
{
    Token t = Peek();
    if (t.Type != type)
        throw new ParseException("Erwartet " + what + ", gefunden '" + t.Text + "'",
                                 t.Line, t.Column);
    return Next();
}

private Token ExpectWord(string keyword)              // bestimmtes Schluesselwort
{
    Token t = Peek();
    if (t.Type != TokenType.Word || t.Text != keyword)
        throw new ParseException("Erwartet '" + keyword + "', gefunden '" + t.Text + "'",
                                 t.Line, t.Column);
    return Next();
}
```

## Einstieg + Fallunterscheidung
```csharp
public Block ParseProgram()                  // program = *statement
{
    var program = new Block();
    while (Peek().Type != TokenType.End)
        program.Statements.Add(ParseStatement());
    return program;
}

private IStatement ParseStatement()          // statement = a / b / c ...
{
    Token t = Peek();
    switch (t.Text)                          // schaut aufs erste Schluesselwort
    {
        case "SET":    return ParseSetColor();
        case "MOVE":   return ParseMove();
        case "REPEAT": return ParseRepeat();
        case "WHILE":  return ParseWhile();
        case "IF":     return ParseIf();
        default:
            throw new ParseException("Unbekanntes Schluesselwort '" + t.Text + "'",
                                     t.Line, t.Column);
    }
}
```

## Das Muster pro Regel
Lies die ABNF-Regel von links nach rechts und uebersetze Token fuer Token:
- woertliches Schluesselwort -> `ExpectWord("...")`
- `number` / `direction` / `color` -> `Expect(...)` + ggf. Wertpruefung
- `[ ... ]` (optional) -> `if (Peek().Type == ...)`
- `block` -> `ParseBlock()`
- `*x` (Wiederholung) -> `while (...)`-Schleife

```csharp
private IStatement ParseMove()               // move = "MOVE" direction [number]
{
    ExpectWord("MOVE");
    string dir = ParseDirection();
    int steps = 1;                           // [number] -> optional
    if (Peek().Type == TokenType.Number)
        steps = int.Parse(Next().Text);
    return new MoveStatement(dir, steps);
}
```

## Wertpruefung (gehoert zur Fehlerbehandlung, Teil 7)
Token-Typ stimmt, aber der Wert ist ungueltig (z.B. Farbe "Purple"):
```csharp
private static readonly HashSet<string> Colors =
    new HashSet<string> { "Black","Red","Green","Blue","Yellow","White" };

Token c = Expect(TokenType.Word, "eine Farbe");
if (!Colors.Contains(c.Text))
    throw new ParseException("Ungueltige Farbe '" + c.Text + "'", c.Line, c.Column);
```

## Klammern korrekt matchen (haeufige Fehlerquelle der Angaben)
```csharp
private Block ParseBlock()                   // block = "{" *statement "}"
{
    Expect(TokenType.LBrace, "'{'");
    var block = new Block();
    while (Peek().Type != TokenType.RBrace)
    {
        if (Peek().Type == TokenType.End)    // Datei aus, '}' fehlt
            throw new ParseException("Erwartet '}', aber Programmende erreicht",
                                     Peek().Line, Peek().Column);
        block.Statements.Add(ParseStatement());
    }
    Expect(TokenType.RBrace, "'}'");
    return block;
}
```

## Was du bei neuer Angabe aenderst
Nur den `switch` in `ParseStatement`, die gueltigen Mengen
(`Directions`/`Colors`) und die `ParseXxx`-Methoden fuer neue Anweisungen.
Peek/Next/Expect/ExpectWord und das Schleifenmuster bleiben identisch.
