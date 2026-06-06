# Datei-Übersicht des Painter-Projekts

## 📋 Dokumentation (9 Dateien)

### Schnelle Orientierung
- **README.md** - Projektübersicht (START HIER!)
- **QUICKSTART.md** - Schneller Einstieg und erste Schritte
- **CHECKLISTE.md** - Was wurde implementiert? Aufgaben-Abhakling

### Ausführliche Dokumentation
- **DOKUMENTATION.md** - Vollständige Erklärung mit Beispielen
- **KOMPONENTEN_ERKLÄRUNG.md** - Detaillierte Analyse jeder Komponente
- **FÜR_LEHRENDE.md** - Didaktisches Konzept & Lernziele

### Code-Beispiele & Grammatik
- **TESTBEISPIELE.md** - Praktische Code-Beispiele (gültig & fehlerhaft)
- **Grammar.abnf** - Formale Grammatik in ABNF-Notation

### Diese Datei
- **DATEIÜBERSICHT.md** - Was du gerade liest

---

## 🔧 Quellcode (11 Dateien)

### GUI & Einstiegspunkt
```
MainWindow.xaml              GUI-Layout (Code-Editor, Fehlerausgabe, Visualisierung)
MainWindow.xaml.cs           GUI-Logik (Event-Handler)
```

### Tokenizer (Lexer)
```
Lexer/
└── Tokenizer.cs            Regex-basierte Zerlegung in Tokens
                            (mit Zeilennummern und Spaltennummern)
```

### Parser (Syntaxanalyse)
```
Interpreter/
├── Parser.cs               Recursive Descent Parser
│                          (wandelt Tokens in Commands um)
│                          (mit ParseException für Fehlerbehandlung)
└── ProgramInterpreter.cs   Hauptinterpreter (orchestriert alles)
```

### Command Pattern & Ausführung
```
Interpreter/Commands/
├── Commands.cs             Command-Implementierungen
│                          (TurnCommand, DrawCommand, ColorCommand, 
│                           RepeatCommand, BlockCommand)
└── DrawingContext.cs       State Management
                           (Position, Winkel, Farbe, Linien)
```

### Datenmodelle
```
Models/
└── Token.cs               Token-Klasse und TokenType-Enum
```

### Visualisierung
```
Controls/
└── PainterControl.cs      WPF Custom Control
                          (zeichnet die Linien mit automatischem Zoom)
```

---

## 📂 Projektstruktur (Baum-Ansicht)

```
Painter/                          Projekt-Root-Verzeichnis
│
├── README.md                      ← START HIER!
├── QUICKSTART.md                  ← Schneller Einstieg
├── CHECKLISTE.md                  ← Aufgaben-Abhakling
├── DATEIÜBERSICHT.md              Diese Datei
├── DOKUMENTATION.md               Ausführliche Doku
├── KOMPONENTEN_ERKLÄRUNG.md       Detaillierte Analyse
├── FÜR_LEHRENDE.md               Didaktisches Konzept
├── TESTBEISPIELE.md              Code-Beispiele
├── Grammar.abnf                  ABNF-Grammatik
│
├── MainWindow.xaml               GUI-Layout
├── MainWindow.xaml.cs            GUI-Logik
│
├── Models/
│   └── Token.cs                 Token-Klasse
│
├── Lexer/
│   └── Tokenizer.cs             Tokenizer
│
├── Interpreter/
│   ├── Parser.cs                Parser
│   ├── ProgramInterpreter.cs    Hauptinterpreter
│   └── Commands/
│       ├── Commands.cs          Commands
│       └── DrawingContext.cs    State
│
└── Controls/
    └── PainterControl.cs        Custom Control
```

---

## 🎯 Welche Datei für was?

### Ich will das Projekt **starten**
→ Baue das Projekt und starten Sie es (F5)

### Ich will **CODE SCHREIBEN**
→ MainWindow.xaml → Code eingeben → "Ausführen" klicken

### Ich will das System **verstehen**
→ 1. DOKUMENTATION.md lesen
→ 2. KOMPONENTEN_ERKLÄRUNG.md lesen
→ 3. Code-Dateien mit Kommentaren lesen

### Ich will **neue Features** hinzufügen
→ 1. Grammar.abnf aktualisieren (Grammatik)
→ 2. Tokenizer.cs aktualisieren (falls neues Token-Type nötig)
→ 3. Parser.cs aktualisieren (Parsing-Logik)
→ 4. Commands.cs aktualisieren (neue Command-Klasse)

### Ich will ein **Fehler-Beispiel** sehen
→ TESTBEISPIELE.md → Copy-Paste in die GUI

### Ich bin **Lehrer**
→ FÜR_LEHRENDE.md → Didaktisches Konzept & Lernziele

### Ich will den **Code kommentiert** lesen
→ Alle .cs Dateien haben ausführliche Kommentare

---

## 📊 Datei-Größen und Komplexität

| Datei | Zeilen | Komplexität | Wichtigkeit |
|-------|--------|-------------|-------------|
| Tokenizer.cs | ~200 | Mittel | ⭐⭐⭐ |
| Parser.cs | ~320 | Hoch | ⭐⭐⭐ |
| Commands.cs | ~150 | Mittel | ⭐⭐⭐ |
| DrawingContext.cs | ~70 | Niedrig | ⭐⭐ |
| PainterControl.cs | ~150 | Mittel | ⭐⭐ |
| Token.cs | ~50 | Niedrig | ⭐⭐ |
| ProgramInterpreter.cs | ~40 | Niedrig | ⭐⭐ |
| MainWindow.xaml.cs | ~50 | Niedrig | ⭐⭐ |

---

## 🔍 Code-Leseanleitung

### Anfänger (Was funktioniert?)
1. QUICKSTART.md
2. MainWindow.xaml und MainWindow.xaml.cs (GUI verstehen)
3. TESTBEISPIELE.md (Beispiele ausprobieren)

### Fortgeschrittene (Wie funktioniert es?)
1. DOKUMENTATION.md
2. Grammar.abnf (Grammatik verstehen)
3. Tokenizer.cs (mit Kommentaren lesen)
4. Parser.cs (Logik verstehen)
5. Commands.cs (Ausführung verstehen)

### Experte (Tiefe verstehen & erweitern)
1. KOMPONENTEN_ERKLÄRUNG.md
2. Alle .cs Dateien im Detail lesen
3. Neue Features implementieren
4. Code optimieren

---

## 📖 Dokumentations-Struktur

```
README.md
    ↓
QUICKSTART.md          (für Anfänger)
    ↓
DOKUMENTATION.md       (allgemein verständlich)
    ↓
KOMPONENTEN_ERKLÄRUNG.md  (technisch detailliert)
    ↓
FÜR_LEHRENDE.md        (pädagogisch)
    ↓
Quellcode + Kommentare (tiefste Ebene)
```

---

## 🎓 Lernpfad nach Datei

### Tag 1: Grundlagen
- README.md (Übersicht)
- QUICKSTART.md (erste Schritte)
- TESTBEISPIELE.md (Code ausprobieren)
- MainWindow.xaml (GUI)

### Tag 2: Tiefergehend
- DOKUMENTATION.md (Konzepte verstehen)
- Grammar.abnf (Sprache definieren)
- Tokenizer.cs (Code mit Kommentaren)
- Parser.cs (Parsing verstehen)

### Tag 3: Architektur
- KOMPONENTEN_ERKLÄRUNG.md (Detailanalyse)
- Commands.cs (Command Pattern)
- DrawingContext.cs (State Management)
- PainterControl.cs (Visualisierung)

### Tag 4+: Erweitern
- Alle Dateien als Referenz
- Neue Features planen
- Code implementieren
- Testen und debuggen

---

## 🔗 Datei-Abhängigkeiten

```
MainWindow.xaml.cs
    ↓
    └─→ ProgramInterpreter.cs
        ├─→ Tokenizer.cs
        │   └─→ Token.cs
        └─→ Parser.cs
            ├─→ Token.cs
            ├─→ Commands.cs
            └─→ DrawingContext.cs

PainterControl.cs
    ├─→ DrawingContext.cs
    └─→ Commands.cs (LineSegment)
```

---

## ✅ Checkliste zum Durchgehen

- [ ] README.md gelesen
- [ ] QUICKSTART.md durchgearbeitet
- [ ] Code in der GUI ausprobiert
- [ ] DOKUMENTATION.md gelesen
- [ ] Grammar.abnf verstanden
- [ ] Tokenizer.cs gelesen
- [ ] Parser.cs gelesen
- [ ] Commands.cs verstanden
- [ ] KOMPONENTEN_ERKLÄRUNG.md studiert
- [ ] Quellcode mit Kommentaren gelesen
- [ ] Neue Features geplant

---

## 💡 Tipps zum Code lesen

1. **Top-Down** - Beginne mit MainWindow.xaml.cs
2. **Kommentare** - Alle Dateien sind gut kommentiert
3. **Schrittweise** - Einen Tag pro Komponente
4. **Hände schmutzig machen** - Code ausprobieren!
5. **Fehler verursachen** - Absichtlich Bugs einbauen, dann fixen

---

## 🚀 Nächste Schritte

1. **Projekt starten** (F5)
2. **README.md lesen** (2 Min)
3. **QUICKSTART.md durchgehen** (5 Min)
4. **Code ausprobieren** (10 Min)
5. **DOKUMENTATION.md studieren** (30 Min)
6. **Quellcode lesen** (1-2 Stunden)
7. **Neue Features planen** (∞)

---

Viel Spaß beim Entdecken! 🎨
