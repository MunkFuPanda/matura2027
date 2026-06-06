# Quick Start Guide

## Installation & Start

1. **Projekt öffnen** in Visual Studio
2. **Bauen** (Ctrl+Shift+B) - sollte erfolgreich sein
3. **Starten** (F5) - MainWindow öffnet sich

## Erste Schritte

### Schritt 1: Das Beispiel ausführen
Der Code-Editor hat bereits ein Beispiel (Stern). Klick einfach auf "Ausführen"!

### Schritt 2: Ein einfaches Quadrat zeichnen
Ersetze den Code mit:
```
COLOR Blue
FOR 4 {
    DRAW 100
    TURN RIGHT 90
}
```

Klick "Ausführen" → Du siehst ein blaues Quadrat!

### Schritt 3: Mit Fehlern experimentieren
Versuche einen Fehler absichtlich:
```
COLOR        ← Farbe fehlt!
DRAW 100
```

Klick "Ausführen" → Du siehst die Fehlermeldung mit Zeilennummer!

## Die Befehle

```
TURN LEFT <winkel>      Dreht nach links
TURN RIGHT <winkel>     Dreht nach rechts
DRAW <länge>            Zeichnet eine Linie
COLOR <farbenname>      Setzt die Farbe
FOR <anzahl> { ... }    Wiederholt
```

Farben: Red, Green, Blue, Yellow, White, Black, Cyan, Magenta, Gray

## Beispiele zum Nachprogrammieren

### Dreieck
```
COLOR Red
DRAW 100
TURN LEFT 120
DRAW 100
TURN LEFT 120
DRAW 100
```

### Hexagon (Sechseck)
```
COLOR Green
FOR 6 {
    DRAW 80
    TURN RIGHT 60
}
```

### Stern-Spitze
```
FOR 5 {
    DRAW 100
    TURN RIGHT 144
}
```

### Spirale
```
COLOR Red
FOR 36 {
    DRAW 50
    TURN RIGHT 10
    COLOR Green
    DRAW 50
    TURN RIGHT 10
    COLOR Blue
}
```

## Fehler verstehen

Wenn du einen Fehler machst, zeigt das Programm:
```
Fehler in Zeile 2, Spalte 6: Nach COLOR muss eine Farbe folgen (...)
```

Das bedeutet:
- **Zeile 2** - Der Fehler ist in der 2. Zeile
- **Spalte 6** - Er beginnt bei der 6. Spalte
- **Fehlermeldung** - Was ist falsch

## Häufige Fehler

❌ **Fehler:** Fehlende Klammer
```
FOR 4 {
    DRAW 100
← Fehler: } erwartet
```

❌ **Fehler:** Fehlende Zahl
```
DRAW
← Fehler: Eine Zahl erwartet
```

❌ **Fehler:** Ungültige Farbe
```
COLOR Purple
← Fehler: Unbekannte Farbe
```

✅ **Richtig:**
```
FOR 4 {
    DRAW 100
    TURN RIGHT 90
}
```

## Tipps & Tricks

1. **Code-Struktur beachten:**
   - Jeder Befehl ist eine neue Zeile
   - Blöcke `{ }` gehören zusammen
   - Leerzeilen sind okay

2. **Winkel verstehen:**
   - 0° = rechts
   - 90° = unten
   - 180° = links
   - 270° = oben

3. **Mit Löschen beginnen:**
   - Klick "Löschen" für neuen Code

4. **Schrittweise aufbauen:**
   - Schreib einen Befehl
   - Führe aus
   - Schreib nächsten Befehl

## So lernst du damit

### Anfänger (Tag 1)
- Verwende nur TURN RIGHT, DRAW, COLOR
- Verstehe, wie Winkel funktionieren
- Zeichne einfache Formen

### Fortgeschrittene (Tag 2-3)
- Verwende FOR-Schleifen
- Kombiniere mehrere Befehle
- Experimientiere mit Farben

### Experte (Tag 4+)
- Verstehe die Architektur
- Lese den Parser-Code
- Ergänze neue Befehle

## Dateien zum Lernen

1. **README.md** - Übersicht
2. **DOKUMENTATION.md** - Ausführliche Erklärung
3. **KOMPONENTEN_ERKLÄRUNG.md** - Wie es intern funktioniert
4. **TESTBEISPIELE.md** - Mehr Beispiele
5. **CHECKLISTE.md** - Was wurde implementiert

## Gut zu wissen

- 🎨 Das Programm zeichnet von oben-links nach unten-rechts
- 📏 Längen und Winkel sind in Pixel bzw. Grad
- 🔄 FOR kann verschachtelt sein
- 🎯 Der Stift startet in der Mitte
- 💾 Die Zeichnung wird automatisch zentriert

## Nächste Schritte

Nachdem du die Basics verstanden hast:

1. **Code lesen** - Schau dir `Tokenizer.cs` an
2. **Parser verstehen** - Lese `Parser.cs`
3. **Commands erforschen** - Schau `Commands.cs`
4. **Neue Befehle hinzufügen** - Modifiziere den Code!

## Hilfe bei Problemen

**Problem:** "Fehler in Zeile 1, Spalte 1: Unerwartetes Token..."
→ Der Befehl wird nicht erkannt. Schreib ihn in GROSSBUCHSTABEN.

**Problem:** "Nichts wird gezeichnet"
→ Verwende DRAW! TURN allein zeichnet nicht.

**Problem:** "Fehler: Eine Zahl erwartet"
→ Nach TURN und DRAW muss eine Zahl folgen, z.B. `DRAW 100`.

**Problem:** "Fehler: Nach COLOR muss eine Farbe folgen"
→ Schreib eine gültige Farbe: Red, Green, Blue, etc.

---

**Viel Spaß beim Programmieren! 🚀**

Falls du fragen zur Architektur hast, lies KOMPONENTEN_ERKLÄRUNG.md
Falls du fragen zum Code hast, schau in die Kommentare in den .cs Dateien
