# Interpreter-Pattern + AST (Teil 2)

## Idee in einem Satz
**Jede Grammatik-Regel wird zu einer Klasse, und jede Klasse weiss, wie sie sich
selbst ausfuehrt** (`Interpret(Context)`). Verschachtelte Konstrukte rufen
`Interpret(...)` ihrer Kinder auf -> der Baum fuehrt sich selbst aus.

## Die drei Bausteine
1. **Interface** mit der gemeinsamen Methode:
   ```csharp
   public interface IStatement { void Interpret(Context ctx); }
   ```
2. **Context** = der Zustand, den alle Knoten lesen/aendern (Cursor, Farbe,
   Gitter). Siehe `Language/Context.cs`.
3. **Eine Klasse pro Anweisung**, jede implementiert `Interpret`.

## Terminal vs. Nonterminal (so nennt es die Theorie)
- **Terminal-Ausdruck** = Blatt, macht etwas Konkretes: `MoveStatement`,
  `SetColorStatement`.
- **Nonterminal-Ausdruck** = enthaelt andere Knoten: `RepeatStatement`,
  `WhileStatement`, `IfStatement`, `Block`. Ruft `Interpret` der Kinder auf.

## Beispielklassen (gekuerzt, vollstaendig in Ast.cs)
```csharp
// Terminal: macht selbst die Arbeit
public class MoveStatement : IStatement
{
    private readonly string _direction;
    private readonly int _steps;
    public MoveStatement(string d, int s) { _direction = d; _steps = s; }
    public void Interpret(Context ctx)
    {
        for (int i = 0; i < _steps; i++) ctx.Move(_direction);
    }
}

// Nonterminal: delegiert an Kinder
public class RepeatStatement : IStatement
{
    private readonly int _count;
    private readonly Block _body;
    public RepeatStatement(int c, Block b) { _count = c; _body = b; }
    public void Interpret(Context ctx)
    {
        for (int i = 0; i < _count; i++) _body.Interpret(ctx);
    }
}

// Block: Liste von Anweisungen
public class Block : IStatement
{
    public List<IStatement> Statements { get; } = new List<IStatement>();
    public void Interpret(Context ctx)
    {
        foreach (var s in Statements) s.Interpret(ctx);
    }
}
```

## UML (falls als Diagramm verlangt)
```
        +--------------+
        |  IStatement  |  <<interface>>
        +--------------+
        | Interpret()  |
        +--------------+
              ^
   +----------+-----------+-----------+-----------+
   |          |           |           |           |
+--------+ +--------+ +--------+  +--------+  +--------+
| Move   | | SetCol | | Repeat |  | While  |  | Block  |
+--------+ +--------+ +--------+  +--------+  +--------+
                          |           |           |
                          +-----+-----+-----------+
                                | enthaelt
                                v
                            (Block / IStatement)

   Context  <-- bekommt jede Interpret(ctx)-Methode uebergeben
   (CursorRow, CursorCol, CurrentColor, Cells[,], Frames)
```

## Warum dieses Pattern (falls gefragt)
Neue Anweisung = neue Klasse, ohne bestehende anzufassen. Der Parser baut nur
noch die Objekte zusammen, die Ausfuehrungslogik steckt in den Klassen selbst.

## Was du bei neuer Angabe aenderst
Pro neuer Anweisung eine neue Klasse mit `Interpret(...)`. Das Interface und der
`Block` bleiben gleich. Der `Context` bekommt die Felder/Methoden, die die neue
Sprache braucht (z.B. `DrawLine`, `Turn`, statt `Move`/`SetColor`).
