# Technische Architektur

## System-Übersicht

```
┌─────────────────────────────────────────────────────────┐
│                    GUI Layer                            │
│              (MainWindow.xaml/.cs)                      │
│                                                         │
│  File Upload → Program Input → Parse/Execute Buttons   │
└────────────────────┬────────────────────────────────────┘
                     │
                     ↓
┌─────────────────────────────────────────────────────────┐
│              Parser & Interpreter                       │
│  ┌──────────┐  ┌────────┐  ┌──────────────────┐        │
│  │  Lexer   │→ │ Tokens │→ │     Parser       │        │
│  └──────────┘  └────────┘  └────────┬─────────┘        │
│                                      │                  │
│                                      ↓                  │
│                              ┌──────────────┐           │
│                              │  AST Nodes   │           │
│                              │ (Commands)   │           │
│                              └──────┬───────┘           │
│                                     │                  │
│                                     ↓                  │
│                      ┌──────────────────────────┐      │
│                      │   RobotInterpreter       │      │
│                      │   (IASTVisitor impl.)    │      │
│                      └──────────┬───────────────┘      │
└──────────────────────────────────┼────────────────────┘
                                   │
                                   ↓
┌─────────────────────────────────────────────────────────┐
│         Models & Control Wrapper                        │
│                                                         │
│      ┌──────────────────────────────────────┐          │
│      │    RobotFieldWrapper                 │          │
│      │  (Adapter zum Custom Control)        │          │
│      └──────────────────┬───────────────────┘          │
│                         │                              │
│           ┌─────────────┴──────────────┐               │
│           ↓                            ↓               │
│  ┌──────────────────┐    ┌──────────────────────┐     │
│  │Direction Converter    │  Old GameField/Robot │     │
│  │(Enum-Mapping)        │  (optional)          │     │
│  └──────────────────┘    └──────────────────────┘     │
└─────────────────────────────────────────────────────────┘
                     │
                     ↓
        ┌─────────────────────────────┐
        │  AbcRobotCore.RobotField    │
        │     (Custom Control)        │
        │                             │
        │  - Move()                   │
        │  - Collect()                │
        │  - IsLetter()               │
        │  - IsObstacle()             │
        │  - LoadField()              │
        └─────────────────────────────┘
                     │
                     ↓
        ┌─────────────────────────────┐
        │   WPF Visual Rendering      │
        │  (Custom Control Display)   │
        └─────────────────────────────┘
```

## Schichtenarchitektur

### 1. **Präsentation (GUI Layer)**

**MainWindow.xaml/.cs**
- Benutzerinteraktion
- Datei-Upload
- Programm-Input
- Button-Events
- Error/Success-Anzeige

### 2. **Parser Layer**

**Lexer.cs**
- Tokenisierung
- Lexikalische Analyse
- Token-Stream

**Parser.cs**
- Syntaktische Analyse
- AST-Konstruktion
- Fehlerbehandlung mit Position

**AST.cs**
- Knoten-Definitionen
- Visitor-Pattern Interface

### 3. **Interpreter Layer**

**RobotInterpreter.cs**
- Traverse AST
- Execute Commands
- Maintain State
- Call Control Methods

### 4. **Model & Adapter Layer**

**RobotFieldWrapper.cs**
- Adapter-Pattern
- Vereinfachte API
- Direction-Konvertierung

**DirectionConverter**
- Enum-Mapping
- Interne ↔ AbcRobotCore Direction

### 5. **External Control**

**AbcRobotCore.RobotField**
- Custom WPF Control
- Visual Rendering
- Field State Management

---

## Design Patterns

### 1. **Interpreter Pattern** (Hauptmuster)

```csharp
interface IASTVisitor {
    void Visit(Program p);
    void Visit(MoveCommand c);
    void Visit(RepeatCommand c);
    // ...
}

abstract class ASTNode {
    abstract void Accept(IASTVisitor v);
}

class RobotInterpreter : IASTVisitor {
    public void Visit(MoveCommand c) { /* Execute */ }
    // ...
}
```

**Vorteil**: 
- Trennung von AST-Struktur und Ausführung
- Erweiterbar: neue Commands = neue Klasse + Visit-Methode

### 2. **Adapter Pattern**

```csharp
class RobotFieldWrapper {
    private RobotField _robotField;

    public void Move(AbcRobotCore.RobotField.Direction d) {
        _robotField.Move(d);
    }
}
```

**Vorteil**:
- Entkopplung vom Custom Control
- Vereinfachte Schnittstelle
- Leichte Ausschaltung möglich

### 3. **Factory Pattern** (implizit)

```csharp
public Program Parse() {
    List<Command> commands = new List<Command>();
    while (/* ... */) {
        if (Match(MOVE)) commands.Add(ParseMoveCommand());
        if (Match(REPEAT)) commands.Add(ParseRepeatCommand());
    }
    return new Program(commands);
}
```

**Vorteil**: Zentrale Command-Erstellung

---

## Ausführungsfluss

### 1. Programm-Laden

```
User.Click("Laden")
  ↓
MainWindow.LoadProgramButton_Click()
  ↓
File.ReadAllText(path)
  ↓
ProgramText.Text = content
```

### 2. Programm-Analyse

```
User.Click("Analysieren")
  ↓
Lexer tokenize = new Lexer(programText)
  ↓
List<Token> tokens = lexer.Tokenize()
  ↓
Parser parser = new Parser(tokens)
  ↓
Program ast = parser.Parse()
  ↓
if (parser.Errors.empty) ExecuteButton.Enable()
else ErrorDisplay.Show(errors)
```

### 3. Programm-Ausführung

```
User.Click("Ausführen")
  ↓
interpreter.Execute(program)
  ↓
for each command:
    if (MOVE):
        direction = DirectionConverter.ToAbcDirection(cmd.Direction)
        fieldWrapper.Move(direction)
        ExecutionHistory.Add(action)

    if (COLLECT):
        fieldWrapper.Collect()
        ExecutionHistory.Add(action)

    if (REPEAT):
        for i in 0..count:
            Execute(cmd.Commands)

    if (IF):
        if (EvaluateCondition(cond)):
            Execute(cmd.Commands)

    if (UNTIL):
        while (!EvaluateCondition(cond)):
            Execute(cmd.Commands)
  ↓
Display ExecutionHistory
  ↓
Show CollectedLetters
```

---

## Datenfluss

### AST Structure (Example)

```
Program
  ├─ RepeatCommand(2)
  │  ├─ MoveCommand(RIGHT)
  │  └─ MoveCommand(DOWN)
  ├─ RepeatCommand(3)
  │  └─ MoveCommand(UP)
  ├─ CollectCommand()
  ├─ IfCommand
  │  ├─ Condition: DOWN IS-A A
  │  └─ MoveCommand(DOWN)
  └─ UntilCommand
     ├─ Condition: RIGHT IS-A OBSTACLE
     └─ MoveCommand(RIGHT)
```

### Token Stream (Example)

```
REPEAT → 2 → { → MOVE → RIGHT → MOVE → DOWN → } → ...
```

### Direction Mapping

```
Internal Direction    →    AbcRobotCore Direction
    UP              →    RobotField.Direction.Up
    DOWN            →    RobotField.Direction.Down
    LEFT            →    RobotField.Direction.Left
    RIGHT           →    RobotField.Direction.Right
```

---

## Fehlerbehandlung

### Parser-Ebene

```
1. Lexer erkannt Invalid Token
   → Parser.Errors.Add("Ungültiges Token: ...")

2. Parser erwartet Token, findet anderen
   → Parser.Errors.Add("Token X erwartet, Y gefunden")

3. Parser erkennt Syntax-Fehler
   → Parser.Errors.Add("Zeile X, Spalte Y: Fehler")
```

### Interpreter-Ebene

```
1. Move außerhalb Spielfeld
   → RobotField.Move() schlägt fehl (silently oder Exception)

2. IsLetter bei ungültigem Position
   → RobotField.IsLetter() gibt false zurück

3. Exception während Ausführung
   → Try-Catch in MainWindow.ExecuteButton_Click()
   → ErrorDisplay.Text = error.Message
```

---

## Klassenzuständigkeiten

| Klasse | Verantwortung | Abhängigkeiten |
|--------|---------------|-----------------|
| Lexer | Tokenisierung | - |
| Parser | Syntaxanalyse, AST | Lexer, Token |
| RobotInterpreter | AST-Ausführung | AST, RobotFieldWrapper |
| RobotFieldWrapper | Custom Control API | AbcRobotCore |
| DirectionConverter | Enum-Mapping | - |
| MainWindow | GUI & Orchestrierung | Parser, Interpreter |
| GameField | Feldlogik (optional) | - |
| Robot | Roboter-Logik (optional) | GameField |

---

## Erweiterungspunkte

### 1. Neue Commands

```csharp
// 1. AST-Knoten hinzufügen
class TurnCommand : Command {
    public int Degrees { get; set; }
    public override void Accept(IASTVisitor v) => v.Visit(this);
}

// 2. Parser erweitern
if (Match(TURN)) {
    int degrees = int.Parse(Current.Value);
    return new TurnCommand(degrees);
}

// 3. Interpreter erweitern
public void Visit(TurnCommand cmd) {
    fieldWrapper.Turn(cmd.Degrees);
}
```

### 2. Neue Bedingungen

```csharp
// 1. ConditionType erweitern
public enum ConditionType {
    IS_OBSTACLE,
    IS_LETTER,
    IS_PASSABLE  // Neu
}

// 2. Parser erweitern
if (Match(PASSABLE)) {
    return new Condition(direction, ConditionType.IS_PASSABLE);
}

// 3. Interpreter erweitern
case ConditionType.IS_PASSABLE:
    return !_fieldWrapper.IsObstacle(abcDirection);
```

### 3. Diagonale Bewegungen

```csharp
// In AbcRobotCore.dll bereits unterstützt
public enum Direction {
    Up, Down, Left, Right,
    UpLeft, UpRight,
    DownLeft, DownRight
}

// Mapping erweitern
case Direction.UP_LEFT:
    return AbcRobotCore.RobotField.Direction.UpLeft;
```

---

## Performance-Überlegungen

| Operation | Zeit | Anmerkungen |
|-----------|------|-------------|
| Lexing | <10ms | Linear zu Textlänge |
| Parsing | <20ms | Abhängig von Verschachtelung |
| Interpretation | <1ms/Befehl | Ohne Custom Control Rendering |
| Custom Control Render | ~100ms | Abhängig von Feldgröße |
| Gesamtausführung mit Pausen | ~1s pro Befehl | Durch Delay() |

---

## Unit Test Struktur

```csharp
// Lexer Tests
LexerTests.TokenizeSimpleCommand()
LexerTests.TokenizeComplexExpression()
LexerTests.InvalidTokenHandling()

// Parser Tests
ParserTests.ParseMoveCommand()
ParserTests.ParseRepeatBlock()
ParserTests.ParseCondition()
ParserTests.ErrorDetection()

// Interpreter Tests
InterpreterTests.ExecuteMoveCommand()
InterpreterTests.ExecuteRepeatCommand()
InterpreterTests.EvaluateConditions()
```

---

## Build & Deployment

```
Build:
  ↓
1. Compile C# Code
2. Link AbcRobotCore.dll (from Lib/)
3. Package Resources (Examples/, XAML)
4. Create Exe

Distribution:
  ↓
- Robotersteuerung.exe (executable)
- Bin/Net10-windows/* (dependencies)
- Examples/ (sample files)
- Lib/AbcRobotCore.dll (must be with exe or in GAC)
```
