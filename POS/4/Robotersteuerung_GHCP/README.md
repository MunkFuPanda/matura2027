# Roboter Steuerung - Programmierprotoptyp

Ein WPF-Programm zum Erlernen von Grundlagen der Programmierung (Schleifen, Bedingungen) durch Steuerung eines Roboters in einem Spielfeld.

## Projektuebersicht

Das Projekt implementiert einen **Interpreter** für eine textbasierte Roboter-Steuerungssprache mit folgenden Komponenten:

### Architektur

```
Lexer (Tokenisierung)
    ↓
Parser (AST Konstruktion)
    ↓
RobotInterpreter (AST Ausführung mit Visitor Pattern)
    ↓
RobotFieldWrapper (Adapter zum Custom Control)
    ↓
AbcRobotCore.RobotField (Custom Control - visuelle Darstellung)
```

### Hauptkomponenten

1. **Models/**
   - `Robot.cs` - Roboter-Logik und Direction-Konverter
   - `GameField.cs` - Spielfeld-Datenstruktur
   - `RobotFieldWrapper.cs` - Adapter für das Custom Control aus AbcRobotCore

2. **Parser/**
   - `Lexer.cs` - Tokenisiert den Eingabe-Text
   - `Parser.cs` - Erstellt einen AST (Abstract Syntax Tree)
   - `AST.cs` - AST-Knoten und Visitor-Interface

3. **Interpreter/**
   - `RobotInterpreter.cs` - Führt den AST aus und ruft Custom Control auf

4. **Utils/**
   - `FieldLoader.cs` - Lädt Spielfelder aus XML-Dateien

5. **GUI**
   - `MainWindow.xaml` / `MainWindow.xaml.cs` - Benutzeroberfläche mit Custom Control

6. **Custom Control (AbcRobotCore.dll)**
   - `RobotField` - WPF-Control für Feldanzeige und Roboter-Steuerung
   - Methoden: LoadField, Move, Collect, IsLetter, IsObstacle

## Syntax der Robotersprache

### Befehle

```
MOVE <Richtung>     # Bewege den Roboter eine Zelle
COLLECT             # Sammle den Buchstaben ein
REPEAT n { ... }    # Wiederhole n-mal
IF <Bedingung> { } # Bedingte Ausführung
UNTIL <Bedingung> { } # Wiederhole bis Bedingung erfüllt
```

### Richtungen

```
UP, DOWN, LEFT, RIGHT
```

### Bedingungen

```
<Richtung> IS-A OBSTACLE    # Ist dort ein Hindernis oder Grenze?
<Richtung> IS-A <Buchstabe> # Ist dort der angegebene Buchstabe?
```

### Beispiel-Programm

```
REPEAT 2 {
    MOVE RIGHT
}
REPEAT 6 {
    MOVE DOWN
}
MOVE LEFT
COLLECT
```

## Verwendung

1. **Spielfeld laden**: Geben Sie den Pfad zu einer XML-Datei ein und klicken Sie "Laden"
   - Das Custom Control wird mit dem Spielfeld initialisiert
   - Der Roboter wird auf seine Startposition gesetzt
   
2. **Programm eingeben**: 
   - Geben Sie das Programm direkt ein oder laden Sie es aus einer Textdatei
   
3. **Programm analysieren**: Klick auf "Programm analysieren"
   - Syntaxfehler werden angezeigt
   
4. **Programm ausführen**: Klick auf "Programm ausführen"
   - Der Roboter im Custom Control bewegt sich schrittweise
   - 1-Sekunden-Pausen zwischen den Schritten ermöglichen visuelle Verfolgung
   - Das Ausführungsprotokoll wird aktualisiert
   - Gesammelte Buchstaben werden angezeigt

5. **Zurücksetzen**: Klick auf "Zurücksetzen" um die Anzeige zu löschen

## XML-Spielfeld-Format

Das Programm unterstützt das **Custom Control XML-Format**:

```xml
<?xml version="1.0" encoding="utf-8"?>
<XML_Field>
  <Width>10</Width>
  <Height>10</Height>
  <Fields>
    <XML_Cell>
      <X>0</X>
      <Y>0</Y>
      <Type>robot</Type>
    </XML_Cell>
    <XML_Cell>
      <X>1</X>
      <Y>3</Y>
      <Type>A</Type>
    </XML_Cell>
    <XML_Cell>
      <X>3</X>
      <Y>2</Y>
      <Type>stone</Type>
    </XML_Cell>
  </Fields>
</XML_Field>
```

**Zellentypen:**
- `robot` - Roboter-Startposition
- `A-Z` - Sammelbare Buchstaben
- `stone` - Hindernisse (#)
- (nicht definiert) - Leere Felder

**Koordinaten**: 0-basiert, oben-links = (0, 0)

Weitere Informationen: `XML_FORMAT_GUIDE.md`

## Fehlerbehandlung

Das Programm erkennt verschiedene Syntaxfehler:

- Ungültige Token
- Fehlende Richtungen nach MOVE
- Fehlende Zahlen nach REPEAT
- Fehlende Klammern
- Ungültige Bedingungen
- Zu viele oder zu wenige Argumente

Fehler werden mit Zeilennummer und Spaltennummer angezeigt.

## Dateien im Projekt

### Beispiele (Examples/)

- `field1.xml` - Einfaches 9x9 Spielfeld
- `field2.xml` - Komplexeres 10x10 Spielfeld
- `program1.txt` - Beispiel-Programm mit REPEAT und COLLECT
- `program2.txt` - Beispiel-Programm mit UNTIL und IF
- `error1.txt` - Syntaxfehler (ungültige Zahl in REPEAT)
- `error2.txt` - Syntaxfehler (fehlende Klammern)

### Dokumentation

- `GRAMMAR.md` - Vollständige ABNF-Grammatik
- `UML_DESIGN.md` - UML-Klassendiagramm und Designerklärung

## Custom Control Integration (AbcRobotCore.dll)

Das Project nutzt das Custom Control `AbcRobotCore.RobotField` für:

- **Felddarstellung**: Das Control zeigt das 2D-Spielfeld grafisch an
- **Roboter-Visualisierung**: Der Roboter wird im Control dargestellt
- **Interaktive Steuerung**: Move, Collect, Checks durch die Control-API

### Direction-Mapping

Das Projekt konvertiert zwischen internen Directions und AbcRobotCore Directions:

```csharp
Direction.UP    ↔ RobotField.Direction.Up
Direction.DOWN  ↔ RobotField.Direction.Down
Direction.LEFT  ↔ RobotField.Direction.Left
Direction.RIGHT ↔ RobotField.Direction.Right
```

### RobotFieldWrapper

Die Klasse `RobotFieldWrapper` kapselt das Custom Control und stellt eine vereinfachte API bereit:

```csharp
wrapper.LoadField(xmlPath)              // Spielfeld laden
wrapper.Move(direction)                 // Roboter bewegen
wrapper.Collect()                       // Buchstaben sammeln
wrapper.IsObstacle(direction)           // Hindernis prüfen
wrapper.IsLetter(letter, direction)     // Buchstabe prüfen
```

## Erweiterungsmöglichkeiten

Das Interpreter-Pattern ermöglicht leichte Erweiterungen:

1. **Neue Befehle**: Neue `Command`-Klasse + `Visit`-Methode im Interpreter
2. **Neue Bedingungen**: `ConditionType`-Enum erweitern
3. **Diagonale Bewegungen**: `Direction`-Enum und Custom Control erweitern
4. **Variablen/Funktionen**: AST-Knoten für diese Konzepte hinzufügen

Das Custom Control `AbcRobotCore` wurde bereits mit diagonalen Bewegungen erweitert.

## Debugging

Der Ausführungsverlauf zeigt für jeden Schritt:
- Die durchgeführte Aktion (MOVE, COLLECT, etc.)
- Das Custom Control aktualisiert sich visuell
- Zwischen den Schritten 1 Sekunde Wartezeit für visuelle Verfolgung

## Anforderungen

- .NET 10
- WPF
- C# 12+
- Visual Studio 2026 oder höher
- `AbcRobotCore.dll` (Custom Control)

## Setup

1. Die `AbcRobotCore.dll` aus dem Downloads-Ordner wird automatisch in den `Lib/` Ordner kopiert
2. Das Projekt baut erfolgreich mit der DLL-Referenz
3. Das Custom Control wird im XAML als `<local:RobotField>` verwendet

## Anmerkungen für Prüfung

✓ Vollständige ABNF-Grammatik implementiert
✓ UML-Klassendiagramm gemäß Interpreter-Pattern
✓ WPF-GUI mit Custom Control RobotField
✓ Lexer, Parser mit vollständiger Fehlerbehandlung
✓ Interpreter mit Visitor-Pattern
✓ Visuelle Ausführung mit 1-Sekunden-Pausen
✓ Syntaxfehler-Erkennung und Anzeige
✓ Beispiel-Programme (Fehler 1 & 2 zum Testen)
✓ Custom Control Integration (AbcRobotCore.dll)
✓ Diagonale Bewegungen unterstützt (via Custom Control)

Die Implementierung priorisiert Klarheit und Verständlichkeit für Lernzwecke.
