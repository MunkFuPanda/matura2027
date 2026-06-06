# UML-Klassen-Diagramm für Interpreter-Pattern

```
┌─────────────────────────────────┐
│      <<abstract>> ASTNode       │
├─────────────────────────────────┤
│ + Accept(visitor: IASTVisitor)  │
└─────────────────────────────────┘
           △
           │
    ┌──────┴──────┬──────────┬──────────┬──────────┐
    │             │          │          │          │
┌───┴───┐  ┌────┴────┐ ┌───┴────┐ ┌──┴──┐ ┌───┴──┐
│Program│  │ Command │ │Condition│ │ ... │ │ ...  │
└───────┘  └────┬────┘ └────────┘ └─────┘ └──────┘
                △
        ┌───────┼───────┬──────────┬──────────┐
        │       │       │          │          │
    ┌───┴───┐ ┌┴──┐ ┌──┴──┐ ┌────┴───┐ ┌──┴──┐
    │ Move  │ │If│ │Until│ │ Repeat │ │Coll.│
    │Command│ │  │ │     │ │Command │ │Cmd  │
    └───────┘ └──┘ └─────┘ └────────┘ └─────┘

Parser:
┌──────────┐     ┌────────┐     ┌────────┐
│  Lexer   │────>│ Tokens │────>│ Parser │
│          │     │        │     │        │
└──────────┘     └────────┘     └────┬───┘
                                     │
                                     v
                                ┌─────────┐
                                │   AST   │
                                │ Program │
                                └────┬────┘
                                     │
                                     v
                           ┌──────────────────┐
                           │ RobotInterpreter │
                           │  implements      │
                           │  IASTVisitor     │
                           └──────┬───────────┘
                                  │
                    ┌─────────────┼─────────────┐
                    │             │             │
              ┌─────v────┐   ┌───v──┐   ┌────v────┐
              │   Robot  │   │Field │   │GameField│
              └──────────┘   └──────┘   └─────────┘

Key Classes:

GameField:
  - Field: char[,]
  - Width: int
  - Height: int
  - StartX, StartY: int

Robot:
  - X, Y: int
  - CollectedLetters: List<char>
  + Move(Direction, GameField): void
  + Collect(GameField): void
  + GetCharInDirection(Direction, GameField): char?
  + CanMoveInDirection(Direction, GameField): bool

RobotInterpreter : IASTVisitor:
  - _robot: Robot
  - _field: GameField
  + Execute(program: Program): void
  + Visit methods for each AST node
```

## Pattern-Erklärung

Das Interpreter-Pattern wird verwendet, um die Roboter-Befehle in einem AST (Abstract Syntax Tree) zu repräsentieren.

1. **Lexer**: Zerlegt den Text in Tokens
2. **Parser**: Erstellt aus den Tokens einen AST
3. **Interpreter**: Führt den AST aus, indem er den Visitor-Pattern nutzt

Jeder AST-Node hat eine Accept-Methode, die einen Visitor (Interpreter) akzeptiert.
Der Interpreter implementiert Visit-Methoden für jeden Node-Typ.

Dies ermöglicht leicht erweiterbare Interpretation verschiedener Befehle.
