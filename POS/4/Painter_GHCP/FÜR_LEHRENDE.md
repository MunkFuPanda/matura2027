# Für Lehrende: Didaktisches Konzept & Lernziele

## Übersicht: Was lernen Schüler?

Dieses Projekt vermittelt die **Grundlagen der Informatik** auf praktische, visuelle Weise:

### 1. Programmierkonzepte
- **Sequenz** - Befehle nacheinander ausführen
- **Wiederholung** - FOR-Schleifen
- **Daten** - Variablenzustand (Position, Winkel, Farbe)
- **Fehlerbehandlung** - Was geht schief und warum?

### 2. Informatik-Theorie
- **Lexikalische Analyse** - Text in Tokens zerlegen
- **Syntaktische Analyse** - Tokens zu Befehlen
- **Semantische Analyse** - Befehle ausführen
- **Compilerbau** - Die Basics

### 3. Design Patterns
- **Command Pattern** - Befehle als Objekte
- **State Pattern** - Zustand speichern
- **Recursive Descent Parsing** - Grammatik in Code

### 4. Mathematik & Grafik
- **Trigonometrie** - sin/cos für Linien
- **Koordinatensysteme** - X, Y, Winkel
- **Geometrie** - Formen zeichnen

## Lernpfad

### Modul 1: Grundlagen (2-3 Unterrichtsstunden)
**Ziel:** Schüler verstehen TURN, DRAW, COLOR, FOR

**Aktivitäten:**
1. Einfache Formen zeichnen (Quadrat, Dreieck)
2. Winkel experimentieren
3. Farben kombinieren
4. Erste Fehler beheben

**Code-Beispiele:**
```
COLOR Red
DRAW 100
TURN RIGHT 90
```

### Modul 2: Wiederholungen (1-2 Unterrichtsstunden)
**Ziel:** Schüler verstehen FOR-Schleifen

**Aktivitäten:**
1. Quadrate mit FOR zeichnen
2. Polygone generieren (Hexagon, Oktagon)
3. Sterne zeichnen
4. Muster erkunden

**Code-Beispiele:**
```
FOR 4 {
    DRAW 100
    TURN RIGHT 90
}
```

### Modul 3: Fehlerbehandlung (1 Unterrichtsstunde)
**Ziel:** Schüler verstehen Fehler und Debugging

**Aktivitäten:**
1. Absichtliche Fehler machen
2. Fehlermeldungen lesen
3. Fehler korrigieren
4. Zeilennummern nutzen

**Code-Beispiele:**
```
COLOR           ← Fehler: Was ist vergessen?
TURN LEFT abc   ← Fehler: Was ist falsch?
```

### Modul 4: Architektur (2-3 Unterrichtsstunden)
**Ziel:** Schüler verstehen, wie die Software funktioniert

**Aktivitäten:**
1. Lese Grammar.abnf
2. Untersuche Tokenizer.cs
3. Verstehe Parser.cs
4. Erkunde Commands.cs

**Fragen zum Nachdenken:**
- Warum brauchen wir einen Tokenizer?
- Was macht ein Parser?
- Warum sind Befehle Objekte?
- Wie speichert das Programm den Zustand?

### Modul 5: Erweiterung (3+ Unterrichtsstunden)
**Ziel:** Schüler implementieren neue Features

**Ideen:**
1. Neuer Befehl: `REPEAT` (Synonym für FOR)
2. Neuer Befehl: `PENUP` / `PENDOWN` (nicht zeichnen)
3. Neuer Befehl: `SETPOS x y` (Position setzen)
4. Neuer Befehl: `HOME` (zurück zum Start)
5. Variablen: `SET x 100`

## Differenzierung

### Anfänger-Gruppe
- Arbeite mit vordefinierten Code-Snippets
- Modifiziere nur Zahlen und Farben
- Fokus: Was machen die Befehle?

### Fortgeschrittene-Gruppe
- Schreibe eigene Code-Sequenzen
- Verstehe FOR-Schleifen
- Fokus: Wie funktionieren Schleifen?

### Expert-Gruppe
- Untersuche den Source-Code
- Implementiere neue Befehle
- Fokus: Wie funktioniert der Interpreter?

## Häufig gestellte Fragen

**F: Warum ist das besser als ein visueller Blocksprache-Editor?**
A: Weil Schüler echten Code schreiben und lesen - nicht nur ziehen und ablegen.
Dies bereitet sie auf echte Programmierung vor.

**F: Ab welchem Alter ist das geeignet?**
A: Grundsätzlich ab 10 Jahren (5. Klasse). Jüngere können mit Hilfe arbeiten.

**F: Wie lange dauert ein Unterricht?**
A: 1 Unterrichtsstunde (45 min) = 1 Modul.
Total ca. 8-10 Stunden für vollständiges Verständnis.

**F: Brauchen Schüler C#-Kenntnisse?**
A: Nein! Sie führen die Painter-Sprache aus, nicht C#.
C#-Kenntnisse helfen beim Modul 4 (Architektur).

**F: Wie bewerte ich den Erfolg?**
A: 
- Modul 1: Schüler können Quadrate zeichnen
- Modul 2: Schüler können Sterne zeichnen
- Modul 3: Schüler können Fehler erkennen und beheben
- Modul 4: Schüler können den Code erklären
- Modul 5: Schüler haben neue Features implementiert

## Unterrichts-Szenarien

### Szenario 1: Kunstprojekt (Zeichnen)
**Zielgruppe:** Alle Altersgruppen

**Ablauf:**
1. Schüler zeichnen eigene Designs
2. Jedes Design ist ein Kunstwerk
3. Ausstellung im Klassenzimmer

**Code-Fokus:** TURN, DRAW, COLOR

### Szenario 2: Mathematik-Projekt (Geometrie)
**Zielgruppe:** Mathe-Unterricht, 7-9 Klasse

**Ablauf:**
1. Zeichne verschiedene Polygone
2. Berechne Winkel
3. Untersuche Symmetrie

**Code-Fokus:** Winkel, FOR, Wiederholungen

### Szenario 3: Informatik-Projekt (Compilerbau)
**Zielgruppe:** Informatik-Unterricht, 10+ Klasse

**Ablauf:**
1. Verstehe Grammatik (ABNF)
2. Untersuche Tokenizer
3. Verfolge Parser-Ausführung
4. Implementiere neue Befehle

**Code-Fokus:** Architektur, Design Patterns

### Szenario 4: Programmier-Kurs
**Zielgruppe:** Anfänger in Programmierung

**Ablauf:**
1. Woche 1-2: Befehle lernen
2. Woche 3: Schleifen
3. Woche 4: Fehlerbehandlung
4. Woche 5+: Architektur

**Code-Fokus:** Schrittweise aufbauend

## Häufige Missverständnisse

❌ **Fehler:** "Das ist nur zum Zeichnen"
✅ **Richtig:** Das Zeichnen ist Visualisierung. Die Hauptlektion ist: Wie funktioniert ein Interpreter?

❌ **Fehler:** "Schüler müssen C# können"
✅ **Richtig:** Schüler schreiben Painter-Code, nicht C#. C# ist nur für die Architektur.

❌ **Fehler:** "Das ist zu schwer für Anfänger"
✅ **Richtig:** Die Painter-Sprache ist sehr einfach. Die Tiefe kommt später.

❌ **Fehler:** "Das sollte grafisch sein"
✅ **Richtig:** Text ist lernfreundlicher. Grafische Blöcke sind später möglich.

## Bewertungs-Kriterien

### Modul 1-3 (Praktisches Programmieren)

**Anfänger (Note 4)**
- Schüler kann DRAW und TURN verwenden
- Fehlerausgabe lesen und verstehen
- Einfache Formen zeichnen

**Fortgeschritten (Note 2-3)**
- Schüler kann FOR-Schleifen verwenden
- Schüler kann Fehler eigenständig beheben
- Schüler kann komplexe Muster zeichnen

**Experte (Note 1)**
- Schüler kann verschachtelte Schleifen schreiben
- Schüler kann Fehler antizipieren
- Schüler kann eigene Design-Sprache entwickeln

### Modul 4 (Verständnis Architektur)

**Anfänger (Note 4)**
- Schüler kann Tokenizer erklären
- Schüler kann Parser erklären
- Schüler weiß, was Commands sind

**Fortgeschritten (Note 2-3)**
- Schüler kann Code-Ausführung verfolgen
- Schüler versteht DrawingContext
- Schüler kann neue Commands schreiben

**Experte (Note 1)**
- Schüler kann Design Patterns erklären
- Schüler kann Compiler-Konzepte erklären
- Schüler kann bestehenden Code optimieren

### Modul 5 (Kreativität)

- Implementierung neuer Features
- Code-Qualität
- Dokumentation
- Innovation

## Ressourcen für Lehrende

1. **DOKUMENTATION.md** - Für Lehrer zum vollständigen Verständnis
2. **KOMPONENTEN_ERKLÄRUNG.md** - Zur Vorbereitung von Modul 4
3. **TESTBEISPIELE.md** - Code-Beispiele für Unterricht
4. **Quellcode mit Kommentaren** - Für Analysen

## Tipps für erfolgreichen Unterricht

1. **Beginne mit Visualisierung** - Zeichnungen motivieren
2. **Fehler sind Lernmaterial** - Debugging lehren
3. **Schrittweise Komplexität** - Nicht alles auf einmal
4. **Peer-Learning** - Schüler helfen einander
5. **Eigene Projekte** - Kreativität fördern
6. **Code-Review** - Mit anderen besprechen

## Häusliche Aufgaben / Hausaufgaben

**Leicht:**
- Zeichne dein Lieblingstier
- Erstelle ein Mosaik-Muster
- Experimentiere mit Farben

**Mittel:**
- Zeichne ein Haus mit Garten
- Erstelle ein regelmäßiges Polygon
- Fehlerhafter Code: Finde und behebe den Fehler

**Schwer:**
- Schreibe einen Parser für eine neue Grammatik
- Implementiere einen neuen Befehl
- Optimiere den Zeichnungs-Algorithmus

---

Viel Erfolg im Unterricht! 🎓
