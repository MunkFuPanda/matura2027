# 🎯 START HIER - Deine Reise mit Painter

## 👋 Willkommen!

Du hast eine **vollständig implementierte Programmiersprache** vor dir - einen Interpreter, 
der Code in Zeichnungen verwandelt!

Diese Datei führt dich in die richtige Richtung.

---

## ⏱️ Du hast 5 Minuten?

1. Starte das Projekt (F5)
2. Klick "Ausführen"
3. Sehe das Ergebnis: Ein schöner Stern wird gezeichnet! 🌟
4. Lies **README.md**

✅ **Fertig!** Du weißt jetzt, was dieses Projekt tut.

---

## ⏱️ Du hast 30 Minuten?

1. Starte das Projekt (F5)
2. Experimentiere mit **QUICKSTART.md** Beispielen
3. Versuche eigene Formen zu zeichnen:
   ```
   COLOR Red
   FOR 4 {
       DRAW 100
       TURN RIGHT 90
   }
   ```
4. Erzeuge absichtlich Fehler und verstehe sie

✅ **Fertig!** Du verstehst die Sprache!

---

## ⏱️ Du hast 2 Stunden?

1. Arbeite **QUICKSTART.md** durch
2. Lese **DOKUMENTATION.md**
3. Studiere **Grammar.abnf** (die Grammatik)
4. Schreibe eigene Painter-Programme

✅ **Fertig!** Du verstehst das System!

---

## ⏱️ Du hast 4+ Stunden?

1. Mache alles von oben
2. Lese **KOMPONENTEN_ERKLÄRUNG.md**
3. Untersuche den Quellcode:
   - Tokenizer.cs (Regex)
   - Parser.cs (Parsing)
   - Commands.cs (Architektur)
4. Implementiere neue Features

✅ **Fertig!** Du bist ein Experte!

---

## 📚 Dokumentations-Übersicht

| Datei | Dauer | Für wen? | Was? |
|-------|-------|----------|------|
| README.md | 5 min | Alle | Was ist das? |
| QUICKSTART.md | 15 min | Anfänger | Wie fange ich an? |
| DOKUMENTATION.md | 30 min | Alle | Wie funktioniert's? |
| KOMPONENTEN_ERKLÄRUNG.md | 45 min | Fortgeschrittene | Wie ist es gebaut? |
| FÜR_LEHRENDE.md | 20 min | Lehrer | Wie unterrichte ich? |
| TESTBEISPIELE.md | 10 min | Alle | Welche Beispiele gibt es? |

---

## 🎮 Erste Aufgabe

Schreibe Painter-Code um folgende Formen zu zeichnen:

### Aufgabe 1: Quadrat (2 Min)
```
COLOR Blue
FOR 4 {
    DRAW 100
    TURN RIGHT 90
}
```
**Ergebnis:** Ein blaues Quadrat ✓

### Aufgabe 2: Dreieck (5 Min)
**Tipp:** 3 Seiten, Winkel = 120°
```
COLOR Red
FOR 3 {
    DRAW 100
    TURN RIGHT 120
}
```
**Ergebnis:** Ein rotes Dreieck ✓

### Aufgabe 3: Sechseck (5 Min)
**Tipp:** 6 Seiten, Winkel = 60°
```
COLOR Green
FOR 6 {
    DRAW 80
    TURN RIGHT 60
}
```
**Ergebnis:** Ein grünes Sechseck ✓

### Aufgabe 4: Stern (10 Min)
**Tipp:** 5 Punkte, externe Winkel nutzen
```
FOR 5 {
    DRAW 100
    TURN RIGHT 144
}
```
**Ergebnis:** Ein Stern ✓

---

## 🐛 Fehler absichtlich erzeugen

Versuche diese fehlerhaften Codes - die Fehlermeldungen sagen dir, was falsch ist!

### Fehler 1: Fehlende Farbe
```
COLOR
DRAW 100
```
**Fehler:** `Nach COLOR muss eine Farbe folgen`

### Fehler 2: Ungültige Zahl
```
DRAW abc
```
**Fehler:** `Eine Zahl erwartet`

### Fehler 3: Ungültige Struktur
```
FOR 4
DRAW 100
```
**Fehler:** `Nach FOR <zahl> muss ein Block { ... } folgen`

---

## 🔬 Den Code untersuchen

Sobald du die Basics verstehst, schau dir den Code an:

### 1. Tokenizer (Lexer/Tokenizer.cs)
**Frage:** Wie wird Text in Befehle zerlegt?
**Antwort:** Mit Regular Expressions!

### 2. Parser (Interpreter/Parser.cs)
**Frage:** Wie wird die Grammatik überprüft?
**Antwort:** Mit Recursive Descent Parsing!

### 3. Commands (Interpreter/Commands/Commands.cs)
**Frage:** Wie werden Befehle ausgeführt?
**Antwort:** Mit dem Command Pattern!

### 4. GUI (MainWindow.xaml.cs)
**Frage:** Wie wird die Zeichnung gezeigt?
**Antwort:** Mit einem benutzerdefinierten WPF-Control!

---

## 🎓 Lernziele (Ort der Erreichung)

### Ziel: Regular Expressions verstehen
**Datei:** Lexer/Tokenizer.cs
**Konzept:** Wie erkennt man Muster in Text?
```csharp
public static readonly string KEYWORD = @"^(TURN|LEFT|RIGHT|...)(?=[\s\{\}]|$)";
```

### Ziel: Parsing verstehen
**Datei:** Interpreter/Parser.cs
**Konzept:** Wie überprüft man Grammatik?
```csharp
private ICommand ParseTurnCommand() { ... }
```

### Ziel: Design Patterns verstehen
**Datei:** Interpreter/Commands/Commands.cs
**Konzept:** Wie modelliert man Befehle?
```csharp
public interface ICommand {
    void Execute(DrawingContext context);
}
```

### Ziel: WPF-Control erstellen
**Datei:** Controls/PainterControl.cs
**Konzept:** Wie zeichnet man mit WPF?
```csharp
protected override void OnRender(DrawingContext dc) { ... }
```

---

## 💡 Tipps für schnelleres Lernen

1. **Experimentieren** - Versuche Code auszuführen und zu verändern!
2. **Fehlermeldungen lesen** - Sie sagen genau, was falsch ist!
3. **Schrittweise vorgehen** - Lerne einen Befehl nach dem anderen!
4. **Fragen stellen** - Schau ins Dokumentation!
5. **Code-Kommentare lesen** - Jede Datei hat Erklärungen!

---

## 🚀 Aufgabe der nächsten Stufe

Wenn du die Basics verstanden hast:

### Mission 1: Neue Farbe hinzufügen
- Öffne `Lexer/Tokenizer.cs`
- Finde die VALID_COLORS Liste
- Füge eine neue Farbe hinzu (z.B. "Orange")

### Mission 2: Neuer Befehl
- Erstelle einen neuen Command (z.B. CIRCLE)
- Registriere ihn im Tokenizer
- Implementiere Parsing im Parser
- Implementiere Ausführung

### Mission 3: Fehlerbehandlung
- Erstelle absichtlich Fehler
- Verstehe die Fehlermeldungen
- Lese die ParseException in Parser.cs

---

## 🎯 Dein Lernpfad

```
TAG 1: GRUNDLAGEN
├─ Starte Projekt
├─ Lies README.md
├─ Probiere Beispiele aus
└─ Verstehe Befehle

TAG 2: VERTIEFUNG
├─ Lies DOKUMENTATION.md
├─ Schreibe eigene Programme
├─ Erzeuge Fehler
└─ Verstehe Fehlerbehandlung

TAG 3: ARCHITEKTUR
├─ Lies KOMPONENTEN_ERKLÄRUNG.md
├─ Untersuche Grammar.abnf
├─ Schau Tokenizer an
└─ Verstehe Parser

TAG 4: QUELLCODE
├─ Lese Commands.cs
├─ Lese DrawingContext.cs
├─ Lese PainterControl.cs
└─ Verstehe die Interaktion

TAG 5+: EXPANSION
├─ Implementiere neue Features
├─ Lese alles nochmal
├─ Optimiere den Code
└─ Lehre anderen!
```

---

## ❓ Häufige Fragen

**F: Brauche ich C#-Kenntnisse?**
A: Du brauchst C#-Kenntnisse nur um den Code zu lesen (Tag 3+).
   Die Painter-Sprache ist viel einfacher!

**F: Was ist der schwierigste Teil?**
A: Der Parser (recursive descent). Aber mit Kommentaren ist es verständlich!

**F: Kann ich neue Befehle hinzufügen?**
A: Ja! Das ist eine tolle Lernaktivität für Tag 5+

**F: Wie lange dauert es alles zu verstehen?**
A: Basis: 1-2 Tage. Architektur: 3-4 Tage. Tiefe: Woche+

**F: Wo sind die Tests?**
A: TESTBEISPIELE.md hat gültige und fehlerhafte Beispiele!

---

## ✅ Erfolgs-Kriterien

### Du hast verstanden, wenn:

✅ Du kannst ein Quadrat zeichnen
✅ Du kannst einen Stern zeichnen
✅ Du verstehst Fehler-Meldungen
✅ Du kannst die Grammatik (Grammar.abnf) lesen
✅ Du kannst Tokenizer.cs verstehen
✅ Du kannst Parser.cs verstehen
✅ Du kannst neue Befehle hinzufügen

---

## 🎁 Bonus: Code zum Experimentieren

### Projekt 1: Bunte Spirale
```
FOR 36 {
    COLOR Red
    DRAW 10
    TURN RIGHT 10
    COLOR Green
    DRAW 10
    TURN RIGHT 10
    COLOR Blue
    DRAW 10
    TURN RIGHT 10
}
```

### Projekt 2: Mandala
```
FOR 12 {
    FOR 4 {
        DRAW 60
        TURN RIGHT 90
    }
    TURN RIGHT 30
}
```

### Projekt 3: Blume
```
COLOR Green
DRAW 100

FOR 6 {
    TURN LEFT 60
    FOR 8 {
        COLOR Red
        DRAW 30
        TURN RIGHT 45
    }
    TURN RIGHT 60
}
```

---

## 📞 Du steckst fest?

1. **Fehler-Meldung?** → Lese sie genau, sie sagt dir die Zeilennummer!
2. **Code-Verständnis?** → Kommentare in der Datei lesen!
3. **Konzept unklar?** → Dokumentation (README, DOKUMENTATION, KOMPONENTEN_ERKLÄRUNG)
4. **Grammatik unklar?** → Grammar.abnf + DOKUMENTATION.md
5. **Architektur?** → KOMPONENTEN_ERKLÄRUNG.md

---

## 🎉 Los geht's!

**Nächster Schritt:**

1. Baue das Projekt (Ctrl+Shift+B)
2. Starte es (F5)
3. Klick "Ausführen"
4. Sehe den Stern! 🌟

**Danach:**

- Lese README.md
- Versuche QUICKSTART.md Beispiele
- Experimentiere mit eigenen Codes!

---

**Viel Spaß beim Lernen! Der beste Weg zu verstehen ist: EXPERIMENTIEREN! 🚀**

P.S. Wenn du Fragen hast, schau in die Kommentare der .cs Dateien - alles ist sehr gut erklärt! 📖
