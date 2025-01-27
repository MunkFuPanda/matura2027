### Aufgabe

Es soll ein Programm erstellt werden mit dem schnell eine Einkaufsliste erzeugt werden kann. Dazu steht eine Auswahl an Produkten in einer Datei [(de)](https://www.dropbox.com/s/55d9yhc2kx6l6ha/Produkte.csv?dl=0) [(en)](https://www.dropbox.com/s/3kz2liqcg6gs2tq/Products.csv?dl=0) zur Verfügung.

Erstellen Sie für die Aufgabe ein Projekt und fügen Sie die beiden CSV-Dateien zum Projekt hinzu.

Die Datei soll eingelesen werden und der Benutzer soll in einer Combobox die verschiedenen Produktgruppen auswählen können. Die Produkte der jeweils ausgewählten Gruppe sollen in einer zweiten Combobox zur Auswahl stehen. Jedes mal wenn die Produktgruppe gewechselt wird sollen auch die Produkte in der zweiten Combobox wechseln. Speichern Sie dazu die Produktgruppen mit ihren Produkten in einer geeigneten Datenstruktur.

Für Produkte die nicht in der Liste sind soll ein normales Textfeld zur Verfügung stehen.

Zusätzlich kann der Benutzer noch die Anzahl angeben (z.B. Slider, Textbox,...)

Mit dem Button "Hinzufügen" kann der Benutzer ein neues Produkt der Einkaufsliste hinzufügen. Dazu wird die Anzahl genommen und entweder der Text aus dem Textfeld für eigene Produkte oder das in der Combobox ausgewählte Produkt.

Die Einkaufsliste selbst soll in einem Table dargestellt werden. Es soll jede Spalte eine geeignete Überschrift haben.

Neben dem "Hinzufügen" Buttons soll es noch folgende weitere Buttons geben:

-   Löschen - Löscht alle ausgewählten Einträge aus der Einkaufsliste

Außerdem sollen über ein Menü noch folgende Funktionen zur Verfügung stehen:

-   Neu - Beginnt eine neue Einkaufsliste
-   Speichern - Speichert die Einkaufsliste in einer XML-Datei (File-Chooser-Dialog verwenden)
-   Laden - Lädt eine Einkaufsliste aus einer XML-Datei (File-Chooser-Dialog verwenden)

Die Texte (Menü, Buttons, Titel,...) des Einkaufslistengenerators sollen in Deutsch oder Englisch angezeigt werden, abhängig von der Systemsprache.

### Erweiterung

Es soll das Menü noch um folgende Punkte erweitert werden:

-   Drucken - Druckt die Einkaufsliste
-   Zusammenfassen - Fasst gleiche Elemente auf der Einkaufsliste zusammen (addiert Anzahlen)
-   Sortieren - Sortiert die Einkaufsliste