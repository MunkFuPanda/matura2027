# Uebungs-Angabe: Mosaik-Werkstatt (PA-Simulation)

Eigenstaendige Angabe im Stil deines Lehrers - verschmilzt Roboter (Cursor auf
Gitter) und Painter (Farben, Tokenizer, Interpreter) und haengt die
Waldwunder-Variante (EF Core + LINQ) an. Bewusst **ohne** Custom-Control-DLL,
damit du alles mit purem WPF (UniformGrid) durchprogrammieren kannst.

## Szenario
Fuer Volksschulkinder soll ein Prototyp entstehen, mit dem sich ueber eine simple
textbasierte Sprache Muster auf einem Gitter malen lassen. Ein Cursor bewegt sich
ueber ein quadratisches Feld und faerbt Zellen ein.

## Die Sprache "MosaikScript"
- Cursor um einen Schritt nach UP/DOWN/LEFT/RIGHT bewegen (optional mehrere).
- Aktuelle Farbe wechseln.
- Pruefen, ob ein Schritt in eine Richtung noch im Feld liegt (CAN).
- Elemente wiederholen (feste Anzahl: REPEAT; bedingt: WHILE).
- Bedingt ausfuehren (IF).
- Mehrere Elemente zu einem Block zusammenfassen mit `{ }`.

## Die 7 Teilaufgaben (je ~10 Punkte)
1. **ABNF-Grammatik** fuer MosaikScript erstellen (-> `docs/01`).
2. **Klassen nach Interpreter-Pattern** planen (AST), optional als UML (-> `docs/03`).
3. **WPF-GUI**: skalierendes Gitter, Textfeld fuer Code, Buttons, Datei laden via
   OpenFileDialog (-> `docs/05`).
4. **Tokenizer** mit regulaeren Ausdruecken (-> `docs/02`).
5. **Parser** (rekursiver Abstieg), der den AST aufbaut (-> `docs/04`).
6. **Ausfuehrung + Visualisierung** mit 1 Sekunde Pause zwischen den Schritten
   (-> `docs/05`).
7. **Fehlerbehandlung** mit aussagekraeftigen Meldungen inkl. Zeilennummer
   (-> `docs/06`).
   **Variante (statt 6 oder 7):** Programme via EF Core in SQLite speichern und
   per LINQ suchen (-> `docs/07`).

## Beispiel 1 (handgetraced)  -> `Examples/beispiel1.mosaik`
```
SET COLOR Red
REPEAT 4 { MOVE RIGHT }
REPEAT 4 { MOVE DOWN }
REPEAT 4 { MOVE LEFT }
REPEAT 4 { MOVE UP }
```
**Soll-Ausgabe** (9x9-Feld, `R`=rot, `.`=leer, Cursor endet bei (0,0), 17 Schritte):
```
RRRRR....
R...R....
R...R....
R...R....
RRRRR....
.........
.........
.........
.........
```
Zeichnet dein Interpreter dieses Quadrat-Umriss, stimmt deine
`SET/MOVE/REPEAT`-Kette.

## Beispiel 2 (handgetraced)  -> `Examples/beispiel2.mosaik`
```
SET COLOR Blue
WHILE CAN DOWN { MOVE DOWN MOVE RIGHT }
SET COLOR Red
IF CAN UP { MOVE UP }
```
**Soll-Ausgabe** (`B`=blau, `R`=rot, Cursor endet bei (7,8), 19 Schritte):
```
B........
BB.......
.BB......
..BB.....
...BB....
....BB...
.....BB..
......BBR
.......BR
```
Blaue Treppe (Diagonale) mit rotem Endpunkt -> deine `WHILE/IF`-Kette stimmt.

## Fehler 1 (Token-/Wertfehler)  -> `Examples/fehler1.mosaik`
```
SET COLOR Red
MOVE RIGHT 2
MOVE DIAGONAL 3
```
**Soll-Meldung:** `Zeile 3, Spalte 6: Ungueltige Richtung 'DIAGONAL'`

## Fehler 2 (Strukturfehler, Klammern)  -> `Examples/fehler2.mosaik`
```
SET COLOR Red
REPEAT 3 { MOVE RIGHT
MOVE DOWN
```
**Soll-Meldung:** `Zeile 4, Spalte 1: Erwartet '}', aber Programmende erreicht`

## So uebst du
1. Versuch erst, das selbst zu bauen (Vorlage in `MosaikWerkstatt/` zur Not als
   Spickzettel danebenlegen).
2. Lade die 4 Beispieldateien und vergleiche mit den Soll-Ausgaben oben.
3. Stimmen Quadrat, Treppe und beide Fehlermeldungen -> du beherrschst alle 7
   Teile. Genau das ist das Muskelgedaechtnis fuer Montag.
