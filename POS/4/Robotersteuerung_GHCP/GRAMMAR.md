# Roboter-Steuerungssprache - ABNF Grammatik

## Grundstruktur

```
Program = 1*Command

Command = MoveCommand
        / CollectCommand
        / RepeatCommand
        / IfCommand
        / UntilCommand

MoveCommand = "MOVE" Direction

CollectCommand = "COLLECT"

RepeatCommand = "REPEAT" NUMBER "{" 1*Command "}"

IfCommand = "IF" Condition "{" 1*Command "}"

UntilCommand = "UNTIL" Condition "{" 1*Command "}"

Direction = "UP" / "DOWN" / "LEFT" / "RIGHT"

Condition = Direction "IS-A" ConditionCheck

ConditionCheck = "OBSTACLE" / LETTER

LETTER = 1*ALPHA
NUMBER = 1*DIGIT
```

## Beschreibung der Befehle

### MOVE
Bewegt den Roboter eine Zelle in die angegebene Richtung.

Beispiel:
```
MOVE UP
MOVE DOWN
MOVE LEFT
MOVE RIGHT
```

### COLLECT
Der Roboter sammelt den Buchstaben auf seinem aktuellen Feld ein.

Beispiel:
```
COLLECT
```

### REPEAT
Wiederholt die Befehle in den geschwungenen Klammern n-mal.

Beispiel:
```
REPEAT 5 {
    MOVE RIGHT
}
```

### IF
Führt die Befehle in den Klammern nur aus, wenn die Bedingung wahr ist.

Beispiel:
```
IF DOWN IS-A A {
    MOVE DOWN
    COLLECT
}
```

### UNTIL
Wiederholt die Befehle so lange, bis die Bedingung wahr wird.

Beispiel:
```
UNTIL RIGHT IS-A OBSTACLE {
    MOVE RIGHT
}
```

## Bedingungen

Bedingungen prüfen den Inhalt eines bestimmten Feldes neben dem Roboter:

```
DOWN IS-A OBSTACLE    // Ist unten ein Hindernis?
RIGHT IS-A A          // Ist rechts der Buchstabe A?
UP IS-A B             // Ist oben der Buchstabe B?
```

### Verfügbare Tests

- `IS-A OBSTACLE` - Prüft auf ein Hindernis (#) oder Spielfeldgrenze
- `IS-A <Buchstabe>` - Prüft auf einen bestimmten Buchstaben

## Symbole

- `R` - Roboter (Startposition)
- `#` - Hindernis
- ` ` (Leerzeichen) - Leeres Feld
- `A-Z` - Sammelbare Buchstaben
