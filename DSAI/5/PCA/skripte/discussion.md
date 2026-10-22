## 7. Diskussion

### 7.1 Ergebnisse auf einen Blick

PCA mit 95 % Varianz reduziert die 10 Features auf **7 Hauptkomponenten** (96,0 % Varianz). Alle Zahlen stammen aus den Läufen oben (Validierungsdaten, derselbe Split wie im Miniprojekt).

| | F1 | MCC | ROC-AUC | Fit [s] | Predict [s] (3 781 Zeilen) | Grid-Search [s] |
|---|---|---|---|---|---|---|
| **RF ohne PCA** | 0,9056 | 0,726 | 0,934 | 3,83 | 0,065 | 126,6 |
| **RF mit PCA** | 0,8497 | 0,571 | 0,882 | 2,61 | 0,075 | 98,4 |
| **kNN ohne PCA** | 0,8907 | 0,655 | 0,898 | 0,017 | 0,377 | 4,8 |
| **kNN mit PCA** | 0,8617 | 0,559 | 0,863 | 0,014 | 0,155 | 2,6 |

=====
### 7.2 Auswirkung auf die Modellqualität: deutlich schlechter

* **Random Forest:** F1 sinkt um 5,6 Prozentpunkte, der MCC sogar um 15,4 Prozentpunkte (0,726 auf 0,571). Die 5-fach-Kreuzvalidierung bestätigt das: F1 0,9046 auf 0,8582 bei einer Streuung von nur etwa 0,003 zwischen den Folds.
* **k-NN:** F1 sinkt um 2,9 Prozentpunkte, der MCC um 9,6 Prozentpunkte. Kreuzvalidiert: 0,8860 auf 0,8654.
* **Das ist kein Zufall.** Wie stark der Random Forest allein durch Zufall schwankt, haben wir in Abschnitt 2 mit 5 Seeds gemessen (Std 0,0006). Die Verschlechterung ist rund 90-mal so groß. Ein kleiner Hinweis zur Ehrlichkeit: Unser Neuaufbau liefert für den RF ohne PCA F1 0,9051 ± 0,0006, das Miniprojekt nennt 0,9106. Das liegt vermutlich an anderen Softwareversionen und ändert nichts an der Aussage, weil beide Varianten hier unter identischen Bedingungen verglichen werden.
* **Der MCC reagiert am stärksten.** Er berücksichtigt alle vier Felder der Konfusionsmatrix und ist bei unserer Klassenimbalance (65 % `g`) das ehrlichste Maß. Dass F1 weniger fällt als der MCC, liegt daran, dass F1 die Mehrheitsklasse `g` bevorzugt.

### 7.3 Warum? Varianz ist nicht gleich Klasseninformation

Die 95 %-Regel behält die Richtungen mit der meisten **Varianz**, ohne das Label zu kennen. Abschnitt 6.3 zeigt, dass das hier schiefgeht:

* **PC7**, die letzte behaltene Komponente, hat einen ANOVA-F-Wert von nur **0,04**, sie trennt `g` und `h` praktisch gar nicht.
* **PC8** (F = 164) und **PC9** (F = 108) trennen die Klassen dagegen deutlich, wurden aber verworfen, weil sie nur 2,2 % bzw. 1,6 % der Varianz tragen. Laut den Ladungen sind das Kontraste zwischen Formmerkmalen (`fLength` gegen `fWidth`, `fSize` gegen `fWidth`), also Informationen über die Form des Schauers, die für die Unterscheidung Gamma/Hadron physikalisch Sinn ergeben.
* **Die Komponentenzahl-Kurve in 6.1 bestätigt das:** Von 7 auf 9 Komponenten springt F1 beim RF von 0,850 auf 0,890 und beim kNN von 0,862 auf 0,890, obwohl nur 3,8 % Varianz dazukommen. Bei 9 Komponenten (99,8 % Varianz) ist das kNN auf Niveau von "ohne PCA" (0,8896 gegen 0,8907).
* **Der Random Forest leidet zusätzlich unter der Rotation selbst.** Mit allen 10 Komponenten (also ganz ohne Informationsverlust, nur gedrehte Achsen) erreicht der RF F1 0,893 statt 0,9056, das sind 1,3 Prozentpunkte Verlust. Das kNN verliert dabei nichts (0,8919 gegen 0,8907). Plausible Erklärung: Bäume schneiden achsenparallel. Auf den Original-Features, etwa `fAlpha`, genügt ein einziger Schnitt. Nach der PCA sind die Features zu Mischungen verschmolzen, und derselbe Zusammenhang braucht mehrere Schnitte. Distanzbasierte Verfahren wie kNN ändert eine reine Drehung dagegen nicht. Das ist eine Interpretation, die wir nicht eigens gegengeprüft haben.
* **Re-Tuning hilft nicht** (Abschnitt 6.2): Der RF behält dieselben besten Parameter, CV-F1 0,8627. Das kNN wählt mit PCA ein größeres k (31) und Euklid-Distanz, kommt aber nur auf 0,8689 (Validierung) gegen 0,8907 ohne PCA. Der Verlust ist also kein Tuning-Artefakt, sondern Informationsverlust.

### 7.4 Auswirkung auf die Performance (Zeit)

Gemessen wurde die gesamte Pipeline (Median aus 7 Läufen), die PCA ist also eingerechnet. Sie selbst kostet nur etwa 3,3 ms (Fit) und 1,2 ms (Transform für 3 781 Zeilen) und fällt nicht ins Gewicht.

* **Random Forest, Training:** 3,83 s auf 2,61 s, also Faktor 0,68 (rund 32 % schneller). Mögliche Ursache: Der RF betrachtet pro Split nur `sqrt(Anzahl Features)` Kandidaten, bei 7 statt 10 Features also weniger, und jeder Split ist billiger. Die gesamte Grid-Search wird von 127 s auf 98 s kürzer.
* **Random Forest, Vorhersage:** praktisch unverändert (0,065 s gegen 0,075 s). Die Differenz liegt in der Größenordnung der Messstreuung (0,01 bis 0,016 s) und ist nicht belastbar.
* **k-NN, Vorhersage:** 0,377 s auf 0,155 s, Faktor 0,41, also rund **2,4-mal schneller**. kNN rechnet bei jeder Vorhersage Abstände zu den Trainingspunkten, weniger Dimensionen bedeuten billigere Abstände und effizientere Suchstrukturen. Das Training ist bei kNN in beiden Fällen vernachlässigbar (nur Daten merken). Die Grid-Search halbiert sich fast (4,8 s auf 2,6 s).
* **Einordnung:** Das sind Sekundenbruchteile auf 15 000 Zeilen. In absoluten Zahlen spart PCA hier 1,2 s beim RF-Training und 0,22 s beim Vorhersagen von 3 781 Zeilen mit kNN. Die Zeitgewinne sind real, aber bei dieser Datenmenge klein. Sie würden erst bei sehr vielen Zeilen oder Features oder bei strengen Latenzvorgaben relevant. Die absoluten Zeiten gelten nur für diese Maschine (4 Kerne, virtuelle Maschine), die Verhältnisse sind aussagekräftiger als die Sekunden.

=====
### 7.5 Abbildung 8-8: Wie viel lässt sich überhaupt komprimieren?

Die Kurve in Abschnitt 4 steigt bei uns vergleichsweise gleichmäßig an: PC1 erklärt 42 %, danach folgen 16 %, 10 %, 10 %, 7 %, 6,5 % und 4 %. Einen scharfen Knick gibt es nicht wirklich, der objektiv bestimmte "Elbow" liegt bei 6 Komponenten (92 %), die 95 %-Schwelle bei 7. Die Dimension sinkt damit nur um 30 % (10 auf 7). Beim MNIST-Beispiel im Buch reichen für 95 % dagegen grob 150 von 784 Dimensionen, also etwa 80 % weniger. Der Grund: Unsere 10 Hillas-Parameter sind nur teilweise redundant (`fConc` und `fConc1` korrelieren mit etwa 0,97, `fLength`, `fWidth` und `fSize` hängen zusammen), die meisten tragen aber eigene Information. Wenig Redundanz heißt wenig Kompressionspotenzial, und hier kommt hinzu, dass die wenigen "überflüssigen" Richtungen ausgerechnet Klasseninformation enthalten.

### 7.6 Fazit

1. **PCA mit 95 % ist für dieses Problem nicht empfehlenswert.** Der RF verliert 5,6 Prozentpunkte F1 (MCC 15 Punkte) für 32 % kürzeres Training, das kNN verliert 2,9 Punkte F1 für 2,4-fach schnelleres Vorhersagen. Bei Datenmengen, die ohnehin in Sekundenbruchteilen laufen, ist das ein schlechter Tausch.
2. **Die Ursache ist grundsätzlich:** PCA wählt nach Varianz, nicht nach Nutzen für die Klassifikation. Hier liegt die trennende Information teils in kleinen Varianzanteilen.
3. **Wenn man PCA trotzdem will:** Die Schwelle hochsetzen (bei 99 % Varianz, also 9 Komponenten, geht beim kNN nichts verloren, beim RF etwa 1,6 Punkte), oder überwachte Verfahren verwenden (z. B. Feature-Auswahl nach F-Wert wie im Miniprojekt, oder LDA).
4. **Die besten Modelle bleiben die aus dem Miniprojekt:** RF und kNN auf den ursprünglichen, standardisierten bzw. unskalierten Features, ohne PCA.

**Grenzen dieser Untersuchung:** Die Aussagen beruhen auf einem einzelnen Train/Validation-Split (durch 5-fach-CV und 5-Seeds-Test abgesichert), es gibt keine formalen Signifikanztests, und die Zeitmessungen gelten nur für diese Maschine. Die Erklärungen zu Laufzeit-Ursachen und zur Rotation beim RF sind plausible Interpretationen, keine eigens bewiesenen Tatsachen.
