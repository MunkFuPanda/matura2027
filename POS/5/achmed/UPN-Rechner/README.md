# UPN-Rechner (Qt)

Mit Qt Creator öffnen: `UPN-Rechner.pro` → Kit wählen → Build & Run.

## Umsetzung
- Eingabe in einem `QLineEdit`, Auswertung per **Enter** (`returnPressed`) oder **Berechnen**-Button.
- Als Stack wird der QT-Container `QStack<double>` verwendet.
- Elemente werden mit `QString::split(' ', Qt::SkipEmptyParts)` getrennt (Leerstellen trennen Operanden und Operatoren).
- Zahlen (`toDouble`) werden gepusht; bei `+ - * /` werden zwei Operanden gepoppt, gerechnet und das Ergebnis gepusht.
- Ergebnis erscheint im `QLabel`; alle fehlerhaften Eingaben erzeugen eine `QMessageBox`.

## Testfälle
| Eingabe (UPN) | Infix | Ergebnis |
|---|---|---|
| `3 4 5 * +` | 3 + 4·5 | 23 |
| `10 2 /` | 10 / 2 | 5 |
| `2 3 - 4 *` | (2−3)·4 | −4 |
| `5 0 /` | 5/0 | Fehlermeldung "Division durch 0" |
| `3 +` | — | Fehlermeldung "zu wenige Operanden" |
| `1 2 3` | — | Fehlermeldung "Ausdruck unvollständig" |
| `abc 2 +` | — | Fehlermeldung "weder Zahl noch Operator" |
| `` (leer) | — | Fehlermeldung "zuerst eingeben" |
