# Tic Tac Toe (Qt)

Mit Qt Creator öffnen: `TicTacToe.pro` → Kit wählen → Build & Run.

## Umsetzung
- **Spielfeld:** 3×3 `QPushButton`s, die mit Grafiken belegt sind (`cross.png`, `star.png`, `empty.png` über `images.qrc`) – keine Buchstaben.
- **Spielerwahl:** zwei `QComboBox`en (Mensch / PC) für Spieler 1 (Stern) und Spieler 2 (Kreuz).
- **Start-Button:** brett leeren, Startspieler wird mit `QRandomGenerator` **zufällig** ermittelt und per `QMessageBox`-Dialog angezeigt.
- **Spielende:** Gewinner bzw. Unentschieden wird ebenfalls per Dialog gemeldet.
- Basisteil (Mensch gegen Mensch) und **Erweiterung PC-Gegner** sind in einem Projekt enthalten: beim Start "Mensch/Mensch" lassen, beide Combos auf *Mensch*.

## PC-Gegner (Erweiterung)
Zugreihenfolge: eigenen Gewinnzug suchen → Zug des Gegners blocken → Mitte → freie Ecke (zufällig) → beliebiges freies Feld. Der PC-Zug läuft zeitversetzt über `QTimer::singleShot`.

## Testfälle
| Eingabe | Erwartung |
|---|---|
| Start | Dialog "Stern/Kreuz beginnt!", Felder leer (leere Grafik) |
| Diagonale f1,f5,f9 für Stern | Dialog "Gewonnen hat: Stern!" |
| Brett voll, keine Linie | Dialog "Unentschieden!" |
| Spieler 2 = PC | Kreuz antwortet nach jedem Mensch-Zug selbst |
| Klick auf besetztes Feld | nichts passiert |
