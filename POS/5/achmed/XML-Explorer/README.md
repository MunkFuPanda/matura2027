# XML-Explorer (Qt + TinyXml2 als dynamische Bibliothek)

Mit Qt Creator öffnen: `XML-Explorer.pro` (Subdirs-Projekt) → Kit → Build & Run.
Es wird zuerst die Bibliothek, dann die App gebaut (`CONFIG += ordered`).

## Struktur
- `tinyxml2lib/` – **dynamische Bibliothek** von TinyXml2 (`TEMPLATE = lib`,
  `DEFINES += TINYXML2_LIB`). Die fertige DLL/`.so` wird ins Build-Verzeichnis der
  App ausgegeben (`DESTDIR`), damit sie zur Laufzeit gefunden wird.
- `explorer/` – die App (`DEFINES += TINYXML2_IMPORT`, `LIBS += -L$$OUT_PWD -ltinyxml2`).
- `beispiel.xml` – Testdatei.
- `../XML-Explorer-static/` – Kopie des Projekts mit **statischer** Bibliothek
  (`CONFIG += staticlib`), TinyXml2 ist dann in die App einkompiliert.

## Umsetzung (Pflichtteil)
- Filedialog (`QFileDialog::getOpenFileName`) wählt eine XML-Datei, gelesen mit
  `tinyxml2::XMLDocument::LoadFile`; Parse-Fehler kommen als Message-Box.
- Die XML-Tags erscheinen als Baum im `QTreeWidget` (Element-Zeiger liegt im
  `Qt::UserRole` des Items).
- Bei Selektion (`itemSelectionChanged`) zeigt das Textfeld daneben
  **Attribute und Inhalt** des Elements.

## Erweiterungen (umgesetzt)
1. Kopie mit statischer Bibliothek → eigenes Projekt `XML-Explorer-static`.
2. `QComboBox` listet die Attribute des selektierten Elements; darunter ein
   Eingabefeld mit dem aktuellen Wert (Auswahlwechsel aktualisiert das Feld).
3. "Übernehmen" setzt das Attribut (`SetAttribute`), "Speichern" schreibt die
   Datei mit `SaveFile()` zurück.

## Testfall
`beispiel.xml` öffnen → `mitarbeiter id="1"` auswählen → Attribut `id` auf `10`
setzen → Übernehmen → Speichern → Datei im Editor nachprüfen.
