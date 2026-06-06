# Painter Interpreter - Detaillierte Erklärung der Komponenten

## 1. Token und TokenType (Models/Token.cs)

### Was ist ein Token?

Ein Token ist die kleinste bedeutungsvolle Einheit eines Programms. Der Tokenizer zerlegt
den Quellcode in Tokens.

**Beispiel:**
```
Quellcode:  TURN RIGHT 45
Tokens:     [Token(TURN), Token(RIGHT), Token(NUMBER, "45")]
```

### Die Token-Klasse

```csharp
public class Token
{
    public TokenType Type { get; set; }        // Art des Tokens (TURN, NUMBER, etc.)
    public string Value { get; set; }          // Der Text des Tokens
    public int LineNumber { get; set; }        // Zeilennummer (für Fehler)
    public int ColumnNumber { get; set; }      // Spaltennummer (für Fehler)
}
```

### TokenType Enum

```csharp
public enum TokenType
{
    // Keywords
    TURN,           // "TURN"
    LEFT,           // "LEFT"
    RIGHT,          // "RIGHT"
    DRAW,           // "DRAW"
    COLOR,          // "COLOR"
    FOR,            // "FOR"

    // Werte
    NUMBER,         // Zahlen wie 45, 100, 250
    COLOR_NAME,     // Farbnamen wie Red, Blue, Green

    // Symbole
    LBRACE,         // {
    RBRACE,         // }

    // Spezielle
    NEWLINE,        // Zeilenumbruch
    EOF,            // End of File
    UNKNOWN         // Unbekannter Token (Fehler)
}
```

---

## 2. Tokenizer (Lexer/Tokenizer.cs)

### Was macht der Tokenizer?

Der Tokenizer (auch "Lexer" genannt) liest den Quellcode Zeichen für Zeichen
und erkennt Token mit Hilfe von **Regular Expressions (Regex)**.

Dies ist der erste Schritt der **Lexikalischen Analyse** (Lexical Analysis).

### Die Token-Patterns (Regular Expressions)

```csharp
// Whitespace (Leerzeichen, Tabs) - werden ignoriert
public static readonly string WHITESPACE = @"^[ \t]+";

// Keywords und Farbnamen (case-insensitive)
// Das (?=[\s\{\}]|$) bedeutet: nur wenn danach Whitespace, Klammer oder Ende kommt
public static readonly string KEYWORD = @"^(TURN|LEFT|RIGHT|DRAW|COLOR|FOR|Red|Green|...)(?=[\s\{\}]|$)";

// Zahlen: eine oder mehr Ziffern
public static readonly string NUMBER = @"^\d+";

// Geschweifte Klammern
public static readonly string LBRACE = @"^\{";
public static readonly string RBRACE = @"^\}";

// Zeilenumbruch (verschiedene Formate: \n, \r\n, \r)
public static readonly string NEWLINE = @"^[\r\n]+";
```

### Wie der Tokenizer arbeitet

```csharp
public List<Token> Tokenize(string source)
{
    var tokens = new List<Token>();
    int lineNumber = 1;
    int columnNumber = 1;
    int index = 0;

    // Solange noch Text übrig ist
    while (index < source.Length)
    {
        // 1. Versuche Whitespace zu erkennen → ÜBERSPRINGEN
        // 2. Versuche Zeilenumbruch zu erkennen → Token hinzufügen, Zeilennummer++
        // 3. Versuche Keyword zu erkennen → Token hinzufügen
        // 4. Versuche Zahl zu erkennen → Token hinzufügen
        // 5. Versuche Klammern zu erkennen → Token hinzufügen
        // 6. Falls nichts passt → UNKNOWN Token (Fehler)
    }

    tokens.Add(new Token(TokenType.EOF, "", lineNumber, columnNumber));  // Markiere Ende
    return tokens;
}
```

### Beispiel: Tokenisierung von "TURN RIGHT 45"

```
Quellcode: "TURN RIGHT 45"

Schritt 1: Erkenne "TURN"   → Token(TURN, "TURN", 1, 1)
Schritt 2: Erkenne " "      → Überspringe (Whitespace)
Schritt 3: Erkenne "RIGHT"  → Token(RIGHT, "RIGHT", 1, 6)
Schritt 4: Erkenne " "      → Überspringe (Whitespace)
Schritt 5: Erkenne "45"     → Token(NUMBER, "45", 1, 12)
Schritt 6: Erkenne EOF      → Token(EOF, "", 1, 14)

Ausgabe: [TURN, RIGHT, NUMBER(45), EOF]
```

---

## 3. Parser (Interpreter/Parser.cs)

### Was macht der Parser?

Der Parser liest die Token-Liste und organisiert sie zu Command-Objekten.
Dies ist der zweite Schritt der **Syntaktischen Analyse** (Syntax Analysis).

### Recursive Descent Parsing

Der Parser verwendet eine Technik namens **Recursive Descent Parsing**:

- Jede Regel der Grammatik ist eine Methode
- Methoden rufen sich gegenseitig auf
- Das spiegelt die hierarchische Struktur der Grammatik wider

**Beispiel der Grammatik:**
```
program    = statement*
statement  = turn-command / draw-command / color-command / for-loop / block
turn-command = "TURN" ("LEFT" / "RIGHT") number
```

**Entsprechende Methoden:**
```csharp
public ICommand[] ParseProgram()           // program
private ICommand ParseStatement()          // statement
private ICommand ParseTurnCommand()        // turn-command
```

### Hilfsmethoden

```csharp
// Gibt aktuelles Token zurück, ohne es zu konsumieren
private Token CurrentToken()

// Konsumiert aktuelles Token und bewegt sich zum nächsten
private Token Advance()

// Prüft ob aktuelles Token vom erwarteten Typ ist
private bool Check(TokenType type)

// Konsumiert Token mit Typprüfung, sonst Exception
private Token Consume(TokenType type, string message)

// Prüft ob wir am Ende sind
private bool IsAtEnd()

// Überspringt alle Zeilenumbrüche
private void SkipNewlines()
```

### Beispiel: ParseTurnCommand

```csharp
private ICommand ParseTurnCommand()
{
    Consume(TokenType.TURN);          // Muss "TURN" sein

    // Prüfe ob LEFT oder RIGHT kommt
    TurnDirection direction;
    if (Check(TokenType.LEFT))
    {
        Advance();
        direction = TurnDirection.LEFT;
    }
    else if (Check(TokenType.RIGHT))
    {
        Advance();
        direction = TurnDirection.RIGHT;
    }
    else
        throw new ParseException("LEFT oder RIGHT erwartet");

    // Parst die Zahl
    Token numberToken = ParseNumberToken();
    int angle = int.Parse(numberToken.Value);

    // Erstelle und gib das Command-Objekt zurück
    return new TurnCommand(direction, angle);
}
```

### Fehlerbehandlung

Der Parser wirft `ParseException` mit Zeilennummer und Spalte:

```csharp
throw new ParseException(
    "Fehlermeldung",
    CurrentToken().LineNumber,
    CurrentToken().ColumnNumber);
```

Diese Exception wird bis zur GUI weitergeleitet und dort angezeigt.

---

## 4. Command Pattern (Interpreter/Commands/Commands.cs)

### Was ist das Command Pattern?

Das Command Pattern ist ein **Design Pattern**, das Aktionen (Befehle) als
separate Objekte modelliert. Jeder Befehl hat eine `Execute()`-Methode.

**Vorteil:** Einfach Befehle speichern, rückgängig machen, repetieren, etc.

### ICommand Interface

```csharp
public interface ICommand
{
    void Execute(DrawingContext context);
}
```

### Command-Implementierungen

#### TurnCommand
```csharp
public class TurnCommand : ICommand
{
    private readonly TurnDirection direction;  // LEFT oder RIGHT
    private readonly int angle;                 // Winkel in Grad

    public void Execute(DrawingContext context)
    {
        // Ändere den Winkel des Kontexts
        int rotationAmount = direction == TurnDirection.RIGHT ? angle : -angle;
        context.CurrentAngle = (context.CurrentAngle + rotationAmount) % 360;
    }
}
```

#### DrawCommand
```csharp
public class DrawCommand : ICommand
{
    private readonly int length;

    public void Execute(DrawingContext context)
    {
        // Berechne neue Position basierend auf Winkel und Länge
        double angleInRadians = context.CurrentAngle * Math.PI / 180.0;
        double newX = context.CurrentX + length * Math.Cos(angleInRadians);
        double newY = context.CurrentY + length * Math.Sin(angleInRadians);

        // Füge Linie hinzu und aktualisiere Position
        context.AddLine(context.CurrentX, context.CurrentY, newX, newY, context.CurrentColor);
        context.CurrentX = newX;
        context.CurrentY = newY;
    }
}
```

#### ColorCommand
```csharp
public class ColorCommand : ICommand
{
    private readonly Color color;

    public void Execute(DrawingContext context)
    {
        context.CurrentColor = color;
    }
}
```

#### RepeatCommand (FOR-Schleife)
```csharp
public class RepeatCommand : ICommand
{
    private readonly int count;
    private readonly ICommand[] commands;

    public void Execute(DrawingContext context)
    {
        // Führe alle Commands 'count' Mal aus
        for (int i = 0; i < count; i++)
        {
            foreach (var command in commands)
            {
                command.Execute(context);
            }
        }
    }
}
```

---

## 5. DrawingContext (Interpreter/Commands/DrawingContext.cs)

### Was ist der DrawingContext?

Der DrawingContext repräsentiert den **Zustand der Zeichnung** während der Ausführung.
Alle Commands lesen und ändern seinen Zustand.

```csharp
public class DrawingContext
{
    // Aktuelle Position des "Stiftes"
    public double CurrentX { get; set; }
    public double CurrentY { get; set; }

    // Aktuelle Richtung (0° = rechts, 90° = unten, etc.)
    public int CurrentAngle { get; set; }

    // Aktuelle Zeichenfarbe
    public Color CurrentColor { get; set; }

    // Alle bereits gezeichneten Linien
    public List<LineSegment> Lines { get; }
}
```

### Koordinatensystem

Das Koordinatensystem ist mathematisch üblich:
- X-Achse: von links nach rechts (positiv nach rechts)
- Y-Achse: von oben nach unten (positiv nach unten in Bildschirm)
- Winkel 0°: zeigt nach rechts (Osten)
- Winkel 90°: zeigt nach unten (Süden)
- Winkel 180°: zeigt nach links (Westen)
- Winkel 270°: zeigt nach oben (Norden)

### LineSegment

```csharp
public class LineSegment
{
    public double X1, Y1;      // Startpunkt
    public double X2, Y2;      // Endpunkt
    public Color Color { get; set; }  // Farbe
}
```

---

## 6. ProgramInterpreter (Interpreter/ProgramInterpreter.cs)

### Was macht der Interpreter?

Der Interpreter orchestriert die Zusammenarbeit aller Komponenten:

```csharp
public DrawingContext Execute(string sourceCode)
{
    // 1. Tokenisierung (Lexikalische Analyse)
    var tokens = tokenizer.Tokenize(sourceCode);

    // 2. Parsing (Syntaktische Analyse)
    var commands = parser.ParseProgram();

    // 3. Ausführung
    DrawingContext context = new DrawingContext();
    foreach (var command in commands)
    {
        command.Execute(context);
    }

    return context;
}
```

---

## 7. PainterControl (Controls/PainterControl.cs)

### Was ist das PainterControl?

Ein benutzerdefinertes WPF-Control (abgeleitet von Canvas), das die Linien visualisiert.

### Wichtige Methoden

```csharp
public void SetDrawingContext(DrawingContext context)
{
    // Speichere den Kontext und zeichne neu
    this.drawingContext = context;
    CalculateViewport();  // Berechne Zoom und Offset
    InvalidateVisual();   // Veranlasse Neuzeichnen
}

protected override void OnRender(DrawingContext wpfDrawingContext)
{
    // WPF ruft diese Methode auf, um zu zeichnen
    // Zeichne alle Linien aus dem DrawingContext
}

private void CalculateViewport()
{
    // Berechne automatisch Zoom und Offset, um die Zeichnung optimal anzuzeigen
}
```

---

## 8. MainWindow (MainWindow.xaml + MainWindow.xaml.cs)

### XAML Layout

Die XAML-Datei definiert die Benutzeroberfläche:
- **Linke Seite:** Code-Editor (TextBox) und Fehlerausgabe
- **Rechte Seite:** Visualisierung (PainterControl)

### Code-Behind

```csharp
private void ExecuteButton_Click(object sender, RoutedEventArgs e)
{
    try
    {
        string sourceCode = CodeTextBox.Text;
        var drawingContext = interpreter.Execute(sourceCode);
        PainterControl.SetDrawingContext(drawingContext);
        ErrorTextBox.Text = $"Erfolgreich! {drawingContext.Lines.Count} Linien.";
    }
    catch (ParseException ex)
    {
        // ex.ToString() gibt "Fehler in Zeile X, Spalte Y: ..."
        ErrorTextBox.Text = ex.ToString();
        PainterControl.Clear();
    }
}
```

---

## Zusammenfassung des Datenflusses

```
1. QUELLCODE (Text)
   ↓
2. TOKENIZER
   ↓ (mit Regex)
3. TOKENS (Liste mit Zeilennummern)
   ↓
4. PARSER
   ↓ (Recursive Descent)
5. COMMANDS (ICommand-Objekte)
   ↓
6. INTERPRETER
   ↓ (Execute)
7. DRAWING CONTEXT
   ↓ (Linien, Position, Farbe)
8. PAINTER CONTROL
   ↓ (Visualisierung)
9. VISUALISIERTE ZEICHNUNG
```

---

## Fehlerbehandlung: Vom Tokenizer zur GUI

```
Tokenizer erkennt ungültigen Token
        ↓
Parser wirft ParseException
        ↓
MainWindow fängt ParseException
        ↓
ex.ToString() gibt "Fehler in Zeile X, Spalte Y: ..."
        ↓
ErrorTextBox.Text = ex.ToString()
        ↓
Fehler wird dem Benutzer angezeigt
```

---

## Lernziele für jeden Bereich

### Tokenizer-Bereich
- ✅ Regular Expressions verstehen
- ✅ Zeilennummern und Spaltennummern verfolgen
- ✅ Token als Objekte modellieren

### Parser-Bereich
- ✅ Recursive Descent Parsing implementieren
- ✅ Grammatik in Code umsetzen
- ✅ Fehlerbehandlung mit aussagekräftigen Fehlermeldungen

### Command-Pattern-Bereich
- ✅ Design Pattern verstehen
- ✅ Befehle als Objekte modellieren
- ✅ State Pattern (DrawingContext) verstehen

### WPF-Bereich
- ✅ Custom Controls erstellen
- ✅ Daten visualisieren
- ✅ Event-Handling

### GUI-Bereich
- ✅ Fehlerbehandlung in der GUI
- ✅ User-Feedback (Fehlermeldungen)
- ✅ Asynchrone Verarbeitung (nicht implementiert, aber möglich)
