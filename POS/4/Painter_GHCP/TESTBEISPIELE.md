# Painter-Programmiersprache - Testbeispiele

## Gültiges Beispiel: Einfaches Quadrat

```
COLOR Blue
DRAW 100
TURN RIGHT 90
DRAW 100
TURN RIGHT 90
DRAW 100
TURN RIGHT 90
DRAW 100
```

Ergebnis: Ein blaues Quadrat


## Gültiges Beispiel: Stern (aus der Aufgabe)

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

Ergebnis: Ein weißer Strich mit einem roten/blauen Muster und einem grünen Kreis-ähnlichen Pattern


## Gültiges Beispiel: Verschachtelter Blöcke

```
FOR 3 {
COLOR Red
FOR 4 {
DRAW 50
TURN RIGHT 90
}
TURN RIGHT 120
}
```

Ergebnis: 3 rote Quadrate, rotiert um 120°


## Fehlerhaftes Beispiel 1: Fehlender Farbname

```
TURN RIGHT 45
COLOR
DRAW 250
```

Fehler in Zeile 2, Spalte 6: Nach COLOR muss eine Farbe folgen (Red, Green, Blue, Yellow, White, Black, Cyan, Magenta, Gray)


## Fehlerhaftes Beispiel 2: Ungültiges Token

```
TURN RIGHT 45
COLOR White
DRAW 250
123
FOR 6 {
COLOR Red
}
```

Fehler in Zeile 4, Spalte 1: Unerwartetes Token: '123'. Erwartet: TURN, DRAW, COLOR, FOR oder {


## Fehlerhaftes Beispiel 3: Ungültige Zahl

```
FOR 6 {
COLOR Red
TURN LEFT 150 ABC
DRAW 150
}
```

Fehler in Zeile 3, Spalte 24: Eine Zahl erwartet


## Fehlerhaftes Beispiel 4: Fehlender Block

```
FOR 6
TURN RIGHT 90
DRAW 40
```

Fehler in Zeile 1, Spalte 7: Nach FOR <zahl> muss ein Block { ... } folgen


## Fehlerhaftes Beispiel 5: Fehlende schließende Klammer

```
FOR 4 {
DRAW 100
TURN RIGHT 90
```

Fehler in Zeile 3, Spalte ...: Schließende Klammer } erwartet


## Fehlerhaftes Beispiel 6: Ungültige Farbe

```
COLOR Orange
DRAW 100
```

Fehler in Zeile 1, Spalte 7: Unbekannte Farbe: orange


## Komplexeres Beispiel: Blume

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

Ergebnis: Eine grüne Stamm mit 6 roten "Blütenblättern"


## Test aller Farben

```
COLOR Red
DRAW 50

COLOR Green
DRAW 50

COLOR Blue
DRAW 50

COLOR Yellow
DRAW 50

COLOR White
DRAW 50

COLOR Black
DRAW 50

COLOR Cyan
DRAW 50

COLOR Magenta
DRAW 50

COLOR Gray
DRAW 50
```

Ergebnis: Eine Spirale mit allen Farben
