# POS – 5CHIF Vorbereitung (achmed)

Vorbereitungs-Projekte für die praktische Informatik-POS (Qt auf Windows).
Alle Projekte sind **Qt-Creator/qmake-Projekte** (`.pro`) und laufen mit Qt 5.15 und Qt 6.x
(build-geprüft mit Qt 6.8 / MinGW/GCC; Windows-Hinweise stehen in den jeweiligen READMEs).

| Projekt | Aufgabe | Besonderheit |
|---|---|---|
| [`TicTacToe/`](TicTacToe/) | Tic Tac Toe mit ComboBoxen (Mensch/PC), Start-/Gewinner-Dialog | Grafiken statt Buchstaben, PC-Gegner als Erweiterung |
| [`UPN-Rechner/`](UPN-Rechner/) | Umgekehrt polnische Notation | Stack = QT-Container `QStack<double>`, Enter & Button, Fehler per Message-Box |
| [`Taschenrechner/`](Taschenrechner/) | Rechner mit Buttons + exprtk | `exprtk.hpp` liegt im Projekt; Syntax-Anpassung (Komma → Punkt, implizite `*`); bigobj-Flags für MSVC/MinGW in der `.pro` |
| [`XML-Explorer/`](XML-Explorer/) | TinyXml2 als **dynamische** Bibliothek, TreeView mit Attribut-Anzeige | Erweiterungen: Attribut-Editor (ComboBox + Speichern) und Kopie mit **statischer** Bibliothek ([`XML-Explorer-static/`](XML-Explorer-static/)) |
| [`Formen/`](Formen/) | Factory-Pattern mit Plugin-Projekten | 11 eigene Formen inkl. Klassenhierarchie + [UML-Diagramm](Formen/uml/Formen.puml) |

## Arbeiten mit den Projekten
1. Qt Creator öffnen → *Open Project* → die `.pro` (bei Subdirs-Projekten die oberste `.pro` nehmen: `XML-Explorer/XML-Explorer.pro` bzw. `Formen/Formen.pro`).
2. Kit wählen (Desktop MSVC oder MinGW) → Build → Run.
3. Bei **XML-Explorer**: zuerst die Bibliothek, dann die App bauen (macht `CONFIG += ordered` automatisch).
4. Bei **Formen**: App läuft nur, wenn die Plugins im Ordner `plugins/` *neben* dem Build-Ordner liegen (passiert beim Bauen des Subdirs-Projekts von selbst).

## Hinweis für die Prüfung
`exprtk.hpp` und `tinyxml2.*` sind vendoriert (MIT/zlib-Lizenzen, liegen im Projekt) –
es muss nichts heruntergeladen werden.
