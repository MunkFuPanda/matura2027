# Quick Start Guide

## Schnelleinstieg in Roboter Steuerung

### 1. Programm starten

Das Programm `Robotersteuerung.exe` starten. Die GUI öffnet sich mit folgenden Bereichen:

- **Links**: Großes Spielfeld mit Custom Control
- **Rechts**: Steuerungselemente

### 2. Spielfeld laden

1. Beispiel-Pfad eingeben oder durchsuchen:
   ```
   Examples/field1.xml
   ```

2. **"Laden"** Button klicken
   - Das Spielfeld wird im Custom Control angezeigt
   - Der Roboter steht auf der Startposition

### 3. Programm eingeben

Option A: **Text eingeben**
```
MOVE RIGHT
MOVE DOWN
COLLECT
```

Option B: **Aus Datei laden**
```
Examples/program1.txt
```

### 4. Programm analysieren

1. **"Programm analysieren"** klicken
2. Erwartete Ausgabe: `✓ Programm erfolgreich analysiert!`
3. Falls Fehler: Fehlermeldung mit Zeile/Spalte wird angezeigt

### 5. Programm ausführen

1. **"Programm ausführen"** klicken
2. Der Roboter bewegt sich schrittweise:
   - 1 Sekunde Pause nach jedem Schritt
   - Das Ausführungsprotokoll wird links aufgelistet
   - Gesammelte Buchstaben werden oben rechts angezeigt

### 6. Zurücksetzen

**"Zurücksetzen"** klicken um die Anzeige zu löschen und neu zu starten.

---

## Beispiel-Szenarien

### Szenario 1: Einfache Bewegung

**Datei**: `Examples/field1.xml`
**Programm**:
```
REPEAT 2 {
    MOVE RIGHT
}
MOVE DOWN
COLLECT
```

**Erwartung**: Roboter bewegt sich nach rechts, dann nach unten und sammelt Buchstabe.

### Szenario 2: Schleifen und Bedingungen

**Datei**: `Examples/field2.xml`
**Programm**:
```
UNTIL RIGHT IS-A OBSTACLE {
    MOVE RIGHT
}
COLLECT
```

**Erwartung**: Roboter bewegt sich nach rechts bis zur Grenze und sammelt dann.

### Szenario 3: Fehlerbehandlung

**Programm**:
```
REPEAT ABC {
    MOVE RIGHT
}
```

**Erwartung**: Fehler wird angezeigt: "Zahl erwartet nach REPEAT" mit Zeile/Spalte.

---

## Befehlsreferenz

### Befehle

| Befehl | Syntax | Beispiel |
|--------|--------|----------|
| Bewegung | `MOVE <Richtung>` | `MOVE UP` |
| Sammeln | `COLLECT` | `COLLECT` |
| Wiederholung | `REPEAT n { ... }` | `REPEAT 3 { MOVE DOWN }` |
| Bedingung | `IF <Bed> { ... }` | `IF UP IS-A A { COLLECT }` |
| Schleife | `UNTIL <Bed> { ... }` | `UNTIL RIGHT IS-A OBSTACLE { MOVE RIGHT }` |

### Richtungen

- `UP` - Nach oben
- `DOWN` - Nach unten
- `LEFT` - Nach links
- `RIGHT` - Nach rechts

### Bedingungen

- `<Richtung> IS-A OBSTACLE` - Ist dort ein Hindernis oder die Spielfeldgrenze?
- `<Richtung> IS-A <Buchstabe>` - Ist dort dieser Buchstabe?

---

## Fehlerbehebung

### Problem: "Bitte geben Sie einen Pfad zur XML-Datei ein"

**Lösung**: Sicherstellen dass der Pfad in das Feld eingegeben wurde, z.B.:
```
C:\Users\Markus\matura2027\POS\4\Robotersteuerung\Examples\field1.xml
```

### Problem: Unerwartetes Token in Zeile X

**Lösung**: 
- Überprüfe die Syntax des Programms
- Verwende nur gültige Befehle: MOVE, COLLECT, REPEAT, IF, UNTIL
- Klammern müssen korrekt geschlossen sein

### Problem: Custom Control wird nicht angezeigt

**Lösung**:
- Sicherstellen dass `Lib/AbcRobotCore.dll` existiert
- Projekt neu bauen: `Ctrl+Shift+B`
- Visual Studio neustarten falls nötig

### Problem: Roboter bewegt sich nicht

**Lösung**:
1. Prüfe ob das Programm erfolgreich analysiert wurde
2. Prüfe ob die Richtung gültig ist (UP/DOWN/LEFT/RIGHT)
3. Prüfe ob der Roboter nicht an der Grenze ist

---

## Wichtige Dateien

```
Robotersteuerung/
├── Examples/
│   ├── field1.xml          ← Einfaches Spielfeld
│   ├── field2.xml          ← Komplexes Spielfeld
│   ├── program1.txt        ← Beispiel-Programm 1
│   ├── program2.txt        ← Beispiel-Programm 2
│   ├── error1.txt          ← Fehlerhaftes Programm 1
│   └── error2.txt          ← Fehlerhaftes Programm 2
├── Lib/
│   └── AbcRobotCore.dll    ← Custom Control
├── MainWindow.xaml         ← GUI Layout
├── README.md               ← Dokumentation
└── CUSTOM_CONTROL_INTEGRATION.md ← Technical Details
```

---

## Tipps & Tricks

✓ **Verschachtelte Schleifen**: Funktioniert problemlos
```
REPEAT 2 {
    REPEAT 3 {
        MOVE DOWN
    }
    MOVE RIGHT
}
```

✓ **Komplexe Bedingungen**: UNTIL mit mehreren Bedingungen
```
REPEAT 4 {
    UNTIL DOWN IS-A OBSTACLE {
        MOVE DOWN
    }
    MOVE RIGHT
}
```

✓ **Debugging**: Das Ausführungsprotokoll zeigt alle Schritte an

✓ **Größere Programme**: Können aus Dateien geladen werden

---

## Beispiel-Ausgabe

Nach dem Ausführen von `program1.txt`:

```
→ MOVE RIGHT
→ MOVE RIGHT
→ MOVE DOWN
→ MOVE DOWN
→ MOVE DOWN
→ MOVE DOWN
→ MOVE DOWN
→ MOVE DOWN
→ MOVE LEFT
→ MOVE LEFT
→ COLLECT
→ MOVE RIGHT
→ MOVE RIGHT
→ MOVE RIGHT
→ MOVE RIGHT
→ MOVE DOWN
→ COLLECT
→ MOVE RIGHT
→ MOVE UP
→ MOVE UP
→ MOVE UP
→ MOVE UP
→ MOVE LEFT
→ COLLECT

✓ Programm erfolgreich ausgeführt!

Gesammelte Buchstaben: A, B, C
```

---

## Nächste Schritte

1. Mit den Beispielen experimentieren
2. Eigene Programme schreiben
3. Unterschiedliche Spielfelder testen
4. Die Fehler-Testdateien verwenden um Fehlerbehandlung zu testen

Viel Spaß beim Programmieren! 🚀
