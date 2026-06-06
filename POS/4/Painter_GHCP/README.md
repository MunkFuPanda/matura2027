# README - Painter Programmiersprache Interpreter

## Was ist dieses Projekt?

Ein **Interpreter** für eine einfache textbasierte Programmiersprache, mit der Kinder
die Grundlagen der Programmierung spielerisch erlernen können. Der Interpreter
visualisiert die Befehle durch Zeichnungen.

## Features

✅ **Vollständige Grammatik (ABNF)** - Dokumentiert in Grammar.abnf

✅ **Tokenizer mit Regex** - Wandelt Code in Tokens um mit Zeilennummern und Spaltennummern

✅ **Recursive Descent Parser** - Wandelt Tokens in Command-Objekte um

✅ **Interpreter Pattern** - Saubere Architektur mit Command-Pattern

✅ **Fehlerbehandlung** - Aussagekräftige Fehlermeldungen mit Zeilennummern

✅ **WPF GUI** - Einfache, funktionierende Benutzeroberfläche

✅ **Custom Control** - PainterControl für Visualisierung

✅ **Gute Kommentare** - Für Lernzwecke

## Befehle der Sprache

```
TURN LEFT <winkel>     - Dreht den Stift um einen Winkel nach links
TURN RIGHT <winkel>    - Dreht den Stift um einen Winkel nach rechts
DRAW <länge>           - Zeichnet eine Linie mit der angegebenen Länge
COLOR <farbe>          - Setzt die Zeichenfarbe
FOR <anzahl> { ... }   - Wiederholt die Befehle im Block
```

**Gültige Farben:**
Red, Green, Blue, Yellow, White, Black, Cyan, Magenta, Gray

## Beispiel-Code

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

## Projektstruktur

```
Painter/
├── Grammar.abnf                        # ABNF-Grammatik
├── DOKUMENTATION.md                    # Ausführliche Dokumentation
├── KOMPONENTEN_ERKLÄRUNG.md           # Detaillierte Erklärung aller Teile
├── TESTBEISPIELE.md                   # Code-Beispiele zum Testen
├── MainWindow.xaml                     # GUI-Layout
├── MainWindow.xaml.cs                  # GUI-Logik
│
├── Models/
│   └── Token.cs                       # Token-Klasse und TokenType enum
│
├── Lexer/
│   └── Tokenizer.cs                   # Regex-basierter Tokenizer
│
├── Interpreter/
│   ├── Parser.cs                      # Recursive Descent Parser
│   ├── ProgramInterpreter.cs          # Haupt-Interpreter
│   └── Commands/
│       ├── Commands.cs                # ICommand und Implementierungen
│       └── DrawingContext.cs          # Zustand des Zeichnens
│
└── Controls/
    └── PainterControl.cs              # WPF Custom Control
```

## So verwenden Sie das Programm

1. Starten Sie das Programm
2. Geben Sie einen Painter-Befehl in die linke Textbox ein
3. Klicken Sie auf "Ausführen"
4. Sehen Sie das Ergebnis rechts oder Fehlermeldungen

## Fehlerbehandlung

Das System erkennt und meldet folgende Fehler mit Zeilennummer und Spalte:

- ❌ Unbekannte Tokens
- ❌ Fehlende Parameter
- ❌ Ungültige Werte
- ❌ Ungültige Struktur (z.B. fehlende Klammern)
- ❌ Ungültige Farbnamen

**Beispiel-Fehlermeldung:**
```
Fehler in Zeile 2, Spalte 6: Nach COLOR muss eine Farbe folgen (Red, Green, Blue, ...)
```

## Komponenten-Übersicht

### 1. Tokenizer (Lexikalische Analyse)
- Wandelt Text in Tokens um
- Nutzt Regular Expressions
- Verfolgt Zeilennummern und Spaltennummern

### 2. Parser (Syntaktische Analyse)
- Wandelt Tokens in Command-Objekte um
- Nutzt Recursive Descent Parsing
- Erkennt Fehler und wirft ParseException

### 3. Interpreter (Ausführung)
- Verbindet Tokenizer und Parser
- Orchestriert die Ausführung

### 4. Command Pattern
- Jeder Befehl ist ein eigenes Objekt (TurnCommand, DrawCommand, etc.)
- Alle implementieren ICommand mit Execute(DrawingContext)
- DrawingContext speichert den Zustand

### 5. PainterControl (Visualisierung)
- Zeichnet alle Linien in einem WPF Canvas
- Berechnet automatisch Zoom und Offset
- Zentriert die Zeichnung

## Lernbereiche

Dieses Projekt ist hervorragend zum Lernen folgender Konzepte:

✅ **Regular Expressions** (Regex) - Im Tokenizer
✅ **Recursive Descent Parsing** - Im Parser
✅ **Design Patterns** - Command Pattern, State Pattern
✅ **Fehlerbehandlung** - ParseException mit Zeilennummern
✅ **WPF Custom Controls** - PainterControl
✅ **Architektur** - Saubere Trennung der Verantwortlichkeiten
✅ **Graphik-Programmierung** - Transformationen, Zeichnen von Linien
✅ **Event-Handling** - GUI-Ereignisse

## Dateiverzeichnis zum Verstehen

1. **Anfänger:** Beginnen Sie mit DOKUMENTATION.md
2. **Fortgeschrittene:** KOMPONENTEN_ERKLÄRUNG.md für Tiefe
3. **Praktisch:** TESTBEISPIELE.md zum Experimentieren
4. **Code:** Lesen Sie die .cs Dateien mit Kommentaren

## Wichtige Design-Entscheidungen

1. **Tokenizer speichert Zeilennummern** - Für aussagekräftige Fehlermeldungen
2. **Parser wirft ParseException mit Zeilennummern** - Fehlerbehandlung auf hoher Ebene
3. **Command Pattern** - Flexible, erweiterbare Architektur
4. **DrawingContext als State** - Einfacher zu verstehen und zu testen
5. **Separate ABNF-Grammatik** - Dokumentiert die Syntax formal

## Erweiterungsmöglichkeiten

- 🔄 Variablen und Zuweisungen
- 🔄 Funktionsdefinitionen
- 🔄 Bedingte Ausführung (IF-Statements)
- 🔄 Schleifen mit Bedingung (WHILE)
- 🔄 Speichern und Laden von Zeichnungen
- 🔄 Graphische Code-Blöcke (für Kinder)
- 🔄 Rückgängigmachen (mit Command Pattern leicht möglich)

## Technologie-Stack

- **Sprache:** C# 10
- **Framework:** .NET 10 / WPF
- **IDE:** Visual Studio Community 2026

## Autor & Lizenz

Dieses Projekt wurde als Lernmaterial erstellt.

---

**Viel Spaß beim Lernen! 🎨**

Für weitere Informationen siehe die Dokumentationsdateien:
- DOKUMENTATION.md - Ausführliche Erklärung
- KOMPONENTEN_ERKLÄRUNG.md - Detaillierte Analyse jeder Komponente
- TESTBEISPIELE.md - Praktische Code-Beispiele
