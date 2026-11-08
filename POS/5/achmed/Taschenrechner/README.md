# Taschenrechner (Qt + Math Expression Parser)

Mit Qt Creator öffnen: `Taschenrechner.pro` → Kit wählen → Build & Run.

## Umsetzung
- Buttons: Ziffern 0–9, Dezimaltrennzeichen `.`, Operatoren `+ - * /`, Klammern `( )`, `=`, plus `C` (Löschen) und `←` (Rücktaste).
- Anzeige in einem read-only `QLineEdit`, Anordnung mit `QGridLayout` (passender Layoutmanager).
- Auswertung mit der **Math Expression Parser Bibliothek** (`exprtk.hpp`, liegt im Projektordner).
- Auto-Connect: `on_pushButton_clear_clicked()`, `on_pushButton_back_clicked()`, `on_pushButton_eq_clicked()`; die restlichen Buttons werden per Lambda an `appendText()` angebunden.

## Anpassungen an die exprtk-Syntax (in `adaptFormula()`)
1. `,` → `.` (Dezimaltrennzeichen)
2. Implizite Multiplikation wird eingefügt: `3(4+5)` → `3*(4+5)`, `(1+2)(3+4)` → `(1+2)*(3+4)`, `(2)3` → `(2)*3`
3. Fehler der Bibliothek (`parser.error()`) werden in einer Message-Box angezeigt.

## Angaben-Hinweise (Windows)
Bereits in der `.pro` umgesetzt:
- MSVC: `QMAKE_CXXFLAGS += -bigobj`
- MinGW: `QMAKE_CXXFLAGS += -m64 -Wa,-mbig-obj`
- Im Code steht **vor** dem Include: `#define exprtk_disable_enhanced_features`

## Testfälle
| Eingabe | Ergebnis |
|---|---|
| `2+3*4` | 14 |
| `(2+3)*4` | 20 |
| `3(4+5)` | 27 (implizite Multiplikation) |
| `10/4` | 2.5 |
| `7/0` | Fehlermeldung (Message-Box) |
| `2+*3` | Fehlermeldung der Bibliothek |
| `1,5+2` | 3.5 |
