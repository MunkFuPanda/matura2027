# DSAI Jahr 5: PCA (95 % Varianz) für das Miniprojekt Klassifikation

Erweiterung von `DSAI/4/Miniproject_Classification` (MAGIC Gamma Telescope): PCA auf 95 % erklärte Varianz für das beste Random Forest und das beste kNN, Zeitmessung, Abbildung 8-8 (Hands-On ML, Kap. 8).

- `Miniproject_PCA.pdf`: **Abgabe** (PDF des Notebooks, A4 quer)
- `Miniproject_PCA.ipynb`: ausgeführtes Notebook mit allen Ausgaben
- `korr.csv`: bereinigte Daten aus dem Miniprojekt (Eingabe, Kopie)
- `abb_8_8_explained_variance.png`: Abbildung 8-8 für unseren Datensatz
- `build/`: `build_nb.py` erzeugt das Notebook, `finish_nb.py` fügt Zusatzanalyse 6.3 und `discussion.md` (Diskussion/Fazit) ein

Neu ausführen: `jupyter nbconvert --to notebook --execute Miniproject_PCA.ipynb` (ca. 10 Min., braucht scikit-learn, pandas, matplotlib, seaborn).
Ergebnis: 7 von 10 Komponenten für 95 %; F1 sinkt beim RF von 0,906 auf 0,850, beim kNN von 0,891 auf 0,862; RF-Training ca. 32 % schneller, kNN-Vorhersage ca. 2,4-mal schneller.
