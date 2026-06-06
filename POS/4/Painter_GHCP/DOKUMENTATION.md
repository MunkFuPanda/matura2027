# Painter-Programmiersprache Interpreter - Dokumentation

## Übersicht

Dieses Projekt ist ein **Interpreter** für eine einfache textbasierte Programmiersprache, die speziell für Kinder entwickelt wurde, um die Grundlagen der Programmierung spielerisch zu erlernen.

Das System besteht aus vier Hauptkomponenten:

1. **Tokenizer** (Lexer) - Wandelt Text in Tokens um
2. **Parser** (Syntax-Analyse) - Wandelt Tokens in Command-Objekte um
3. **Interpreter** - Orchestriert Tokenizer und Parser
4. **Ausführungsumgebung** - Führt die Commands aus und visualisiert das Ergebnis

---

## Grammatik (ABNF)

```
program        = *( statement CRLF )
statement      = turn-command / draw-command / color-command / for-loop / block

turn-command   = "TURN" ( "LEFT" / "RIGHT" ) number
draw-command   = "DRAW" number
color-command  = "COLOR" color-name
for-loop       = "FOR" number block
block          = "{" *( statement CRLF ) "}"

color-name     = "Red" / "Green" / "Blue" / "Yellow" / "White" / "Black" / "Cyan" / "Magenta" / "Gray"
number         = 1*DIGIT
```

---

## Befehle

### TURN
**Syntax:** `TURN LEFT <winkel>` oder `TURN RIGHT <winkel>`

Dreht den Stift um einen bestimmten Winkel (in Grad). Der Stift startet zeigend nach rechts (0°).

**Beispiel:**
```
TURN RIGHT 90    ; Dreht 90° nach rechts
TURN LEFT 45     ; Dreht 45° nach links
```

### DRAW
**Syntax:** `DRAW <länge>`

Zeichnet eine Linie mit der angegebenen Länge in die aktuelle Richtung.

**Beispiel:**
```
DRAW 100   ; Zeichnet eine 100 Pixel lange Linie
DRAW 250   ; Zeichnet eine 250 Pixel lange Linie
```

### COLOR
**Syntax:** `COLOR <farbenname>`

Setzt die Farbe für nachfolgende Linien.

**Gültige Farben:** Red, Green, Blue, Yellow, White, Black, Cyan, Magenta, Gray

**Beispiel:**
```
COLOR Red      ; Weitere Linien werden rot gezeichnet
COLOR Blue     ; Weitere Linien werden blau gezeichnet
```

### FOR
**Syntax:** `FOR <anzahl> { ... }`

Wiederholt die Befehle im Block eine bestimmte Anzahl.

**Beispiel:**
```
FOR 4 {
    DRAW 50
    TURN RIGHT 90
}
; Diese 4 Befehle werden 4 Mal wiederholt
```

---

## Beispiel 1: Einfaches Quadrat

```
COLOR Red
DRAW 100
TURN RIGHT 90
DRAW 100
TURN RIGHT 90
DRAW 100
TURN RIGHT 90
DRAW 100
```

oder kompakter mit FOR:

```
COLOR Red
FOR 4 {
    DRAW 100
    TURN RIGHT 90
}
```

---

## Beispiel 2: Stern aus der Aufgabe

```
TURN RIGHT 45
COLOR White
DRAW 250
FOR 6 {
    COLOR Red
    TURN LEFT 150
    DRAW 150
    COLOR Blue
    TURN LEFT 150
    DRAW 150
}
TURN RIGHT 90
COLOR Green
FOR 12 {
    TURN RIGHT 30
    DRAW 40
}
```

---

## Fehlerbehandlung

Der Interpreter gibt aussagekräftige Fehlermeldungen mit **Zeilennummer und Spaltennummer** aus.

### Beispiel: Fehlerhafte Code

```
TURN RIGHT 45
COLOR                    ← FEHLER: Es fehlt der Farbname
DRAW 250
123                      ← FEHLER: "123" ist kein gültiger Befehl
FOR 6 {
    COLOR Red
    TURN LEFT 150 ABC    ← FEHLER: "ABC" ist keine Zahl
    DRAW 150
}
```

**Fehlerausgabe:**
```
Fehler in Zeile 2, Spalte 6: Nach COLOR muss eine Farbe folgen (Red, Green, Blue, ...)
Fehler in Zeile 4, Spalte 1: Unerwartetes Token: '123'. Erwartet: TURN, DRAW, COLOR, FOR oder {
Fehler in Zeile 7, Spalte 24: Eine Zahl erwartet
...
```

---

## Architektur (Interpreter Pattern)

### 1. Tokenizer (Lexer)

**Datei:** `Lexer/Tokenizer.cs`

Der Tokenizer zerlegt den Quellcode in Token. Er verwendet **Regular Expressions** (Regex):

```csharp
// Erkenne Keywords (TURN, DRAW, COLOR, FOR)
public static readonly string KEYWORD = @"^(TURN|LEFT|RIGHT|DRAW|COLOR|FOR|...)(?=[\s\{\}]|$)";

// Erkenne Zahlen
public static readonly string NUMBER = @"^\d+";

// Erkenne Klammern
public static readonly string LBRACE = @"^\{";
public static readonly string RBRACE = @"^\}";
```

**Ausgabe: Liste von Token-Objekten mit Zeilennummer und Spalte**

### 2. Parser (Syntax-Analyse)

**Datei:** `Interpreter/Parser.cs`

Der Parser analysiert die Tokens und erstellt Command-Objekte. Er verwendet **Recursive Descent Parsing**:

```csharp
// Jede grammatikalische Regel ist eine Methode
public ICommand[] ParseProgram()          // program
private ICommand ParseStatement()         // statement
private ICommand ParseTurnCommand()       // turn-command
private ICommand ParseDrawCommand()       // draw-command
private ICommand ParseColorCommand()      // color-command
private ICommand ParseForLoop()           // for-loop
private ICommand ParseBlock()             // block
```

**Fehlerbehandlung:** Jede Methode kann `ParseException` werfen mit Zeilennummer und Spalte.

### 3. Command Pattern (Interpreter Pattern)

**Datei:** `Interpreter/Commands/Commands.cs`

Jeder Befehl wird als separates Objekt repräsentiert:

```csharp
public interface ICommand
{
    void Execute(DrawingContext context);
}

public class TurnCommand : ICommand { ... }
public class DrawCommand : ICommand { ... }
public class ColorCommand : ICommand { ... }
public class RepeatCommand : ICommand { ... }
```

### 4. Ausführungsumgebung

**Datei:** `Interpreter/Commands/DrawingContext.cs`

Speichert den Zustand des Zeichnens:

```csharp
public class DrawingContext
{
    public double CurrentX { get; set; }        // X-Position
    public double CurrentY { get; set; }        // Y-Position
    public int CurrentAngle { get; set; }       // Richtung (0-360°)
    public Color CurrentColor { get; set; }     // Aktuelle Farbe
    public List<LineSegment> Lines { get; }     // Gezeichnete Linien
}
```

### 5. Visualisierung

**Datei:** `Controls/PainterControl.cs`

Ein Custom WPF Control, das die Linien zeichnet. Es:

- Berechnet automatisch Zoom und Versatz
- Zeichnet alle Linien in der richtigen Farbe
- Zentriert die Zeichnung im Fenster

---

## Wie das System zusammenarbeitet

```
┌─────────────────────┐
│  Quellcode (Text)   │
│  "TURN RIGHT 45"    │
│  "DRAW 100"         │
└──────────┬──────────┘
           │
           ▼
     ┌──────────────┐
     │  TOKENIZER   │  Regex-basierte Analyse
     └──────┬───────┘
            │
            ▼
     ┌──────────────────────────────┐
     │  Token-Liste                 │
     │  (Token, LineNum, ColNum)    │
     └──────┬───────────────────────┘
            │
            ▼
     ┌──────────────┐
     │   PARSER     │  Recursive Descent Parsing
     └──────┬───────┘
            │
            ▼
     ┌──────────────────────────┐
     │  Command-Objekte        │
     │  (TurnCommand, etc.)    │
     └──────┬───────────────────┘
            │
            ▼
     ┌──────────────┐
     │ INTERPRETER  │  Führt Commands aus
     └──────┬───────┘
            │
            ▼
     ┌──────────────────────────┐
     │  DrawingContext          │
     │  (Linien, Position, ...) │
     └──────┬───────────────────┘
            │
            ▼
     ┌──────────────┐
     │ VISUALIZATION│  WPF-Control
     └──────────────┘
```

---

## Fehlerbehandlung im Detail

### Fehler mit Zeilennummern

Der Tokenizer speichert bei jedem Token die **Zeilennummer** und **Spaltennummer**:

```csharp
public Token(TokenType type, string value, int lineNumber, int columnNumber)
{
    // ...
}
```

Der Parser wirft `ParseException` mit diesen Informationen:

```csharp
throw new ParseException(
    "Fehlermeldung",
    CurrentToken().LineNumber,
    CurrentToken().ColumnNumber);
```

Die GUI zeigt die Fehler an:

```
Fehler in Zeile 2, Spalte 6: ...
```

### Erkennung von Fehlertypen

Das System erkennt folgende Fehler:

1. **Unbekannte Tokens**
   ```
   123 ← Nicht Teil der Grammatik
   ```

2. **Fehlende Parameter**
   ```
   TURN RIGHT     ← Winkel fehlt
   COLOR          ← Farbe fehlt
   DRAW           ← Länge fehlt
   ```

3. **Ungültige Werte**
   ```
   TURN RIGHT abc    ← "abc" ist keine Zahl
   COLOR Unknown     ← "Unknown" ist keine gültige Farbe
   ```

4. **Ungültige Struktur**
   ```
   FOR 6         ← Block {} erwartet
   { ... }       ← Keine zugehörige FOR
   ```

---

## Die Erweiterung: Zeilennummern

Die Aufgabe fordert, dass Fehlermeldungen Zeilennummern enthalten. Dies ist bereits implementiert!

**Im Tokenizer** (Datei: Lexer/Tokenizer.cs):
```csharp
int lineNumber = 1;  // Zeilennummer wird mitgezählt
int columnNumber = 1;

while (index < source.Length)
{
    // ... Token erkennen ...

    if (match.Success)
    {
        tokens.Add(new Token(type, value, lineNumber, columnNumber));
        // Zeilennummern aktualisieren
    }
}
```

**Im Parser** (Datei: Interpreter/Parser.cs):
```csharp
throw new ParseException(
    "Fehlermeldung",
    CurrentToken().LineNumber,      // ← Zeilennummer
    CurrentToken().ColumnNumber);   // ← Spaltennummer
```

**In der GUI** (Datei: MainWindow.xaml.cs):
```csharp
catch (ParseException ex)
{
    // Die ToString-Methode der ParseException formatiert die Ausgabe:
    // "Fehler in Zeile 2, Spalte 6: Fehlermeldung"
    ErrorTextBox.Text = ex.ToString();
}
```

---

## Quellcode-Struktur

```
Painter/
├── Grammar.abnf                      // ABNF-Grammatik
├── Models/
│   └── Token.cs                     // Token-Klasse und TokenType enum
├── Lexer/
│   └── Tokenizer.cs                 // Regex-basierter Tokenizer
├── Interpreter/
│   ├── Parser.cs                    // Recursive Descent Parser
│   ├── ProgramInterpreter.cs        // Hauptinterpreter
│   └── Commands/
│       ├── Commands.cs              // ICommand und Implementierungen
│       └── DrawingContext.cs        // Zustand des Zeichnens
├── Controls/
│   └── PainterControl.cs            // WPF-Custom-Control
├── MainWindow.xaml                  // GUI-Layout
└── MainWindow.xaml.cs               // GUI-Logik
```

---

## So verwendest du das Programm

1. **Code eingeben** - Gib einen Painter-Befehl in die linke Textbox ein
2. **Ausführen-Button klicken** - Der Interpreter wird aufgerufen
3. **Fehler prüfen** - Die rote Fehlerbox zeigt Fehler mit Zeilennummern
4. **Zeichnung ansehen** - Das Ergebnis wird in der rechten Seite visualisiert

---

## Lernressourcen (in diesem Code)

### 1. Regular Expressions (Regex)
- Datei: `Lexer/Tokenizer.cs`
- Wie man Muster in Text erkennt

### 2. Recursive Descent Parsing
- Datei: `Interpreter/Parser.cs`
- Wie man eine Grammatik in Code umsetzt

### 3. Command Pattern (Design Pattern)
- Datei: `Interpreter/Commands/Commands.cs`
- Wie man Befehle als Objekte modelliert

### 4. WPF Custom Controls
- Datei: `Controls/PainterControl.cs`
- Wie man eigene graphische Controls erstellt

### 5. Exception Handling
- Dateien: `Interpreter/Parser.cs`, `MainWindow.xaml.cs`
- Wie man Fehler korrekt behandelt und berichtet

---

## Zusätzliche Ideen für Erweiterungen

1. **Variablen** - `SET x 100`, `DRAW x`
2. **Funktionen** - `FUNC draw_square { ... }`
3. **Bedingte Ausführung** - `IF angle > 90 { ... }`
4. **Eingabe vom Benutzer** - `INPUT x`
5. **Loops mit Bedingung** - `WHILE x < 100 { ... }`
6. **Prozeduren** - `PROCEDURE`
7. **Speichern und Laden** - Zeichnungen speichern
8. **Grafische Blöcke** - Code-Blöcke farblich darstellen
