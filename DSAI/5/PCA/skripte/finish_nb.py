import copy
import nbformat as nbf
from nbclient import NotebookClient

run = nbf.read("Miniproject_PCA_run.ipynb", 4)
src = nbf.read("Miniproject_PCA.ipynb", 4)

# --- zusätzliche Analyse-Zelle (Klassentrennung je Hauptkomponente) -------------
extra_md = nbf.v4.new_markdown_cell("""
### 6.3 Warum fällt die Qualität? Varianz ist nicht gleich Klasseninformation

PCA ist **unüberwacht**: Sie kennt das Label nicht und sortiert Komponenten nur nach Varianz. Ob eine Komponente die Klassen `g` und `h` trennt, steht auf einem anderen Blatt. Das prüfen wir mit dem ANOVA-F-Wert (derselbe wie im Miniprojekt für die Features) je Hauptkomponente auf den Trainingsdaten. Hoher F-Wert bedeutet starke Klassentrennung.
""".strip("\n"))
extra_code = nbf.v4.new_code_cell("""
from sklearn.feature_selection import f_classif

Z_train = pca_full.transform(X_train_s)             # alle 10 Komponenten
f_pc, _ = f_classif(Z_train, y_train)
pc_tab = pd.DataFrame({"Varianz [%]": evr * 100, "kumuliert [%]": cumsum * 100, "ANOVA-F (Klassentrennung)": f_pc,
                       "im 95 %-Modell behalten": ["ja" if i < d else "NEIN" for i in range(len(evr))]},
                      index=[f"PC{i+1}" for i in range(len(evr))])
display(pc_tab.style.format({"Varianz [%]": "{:.2f}", "kumuliert [%]": "{:.2f}", "ANOVA-F (Klassentrennung)": "{:.1f}"}))

fig, ax = plt.subplots(figsize=(9, 4))
ax.bar(range(1, 11), f_pc, color=["tab:blue" if i < d else "tab:red" for i in range(10)])
ax.set_xticks(range(1, 11)); ax.set_xlabel("Hauptkomponente"); ax.set_ylabel("ANOVA-F (höher = trennt Klassen besser)")
ax.set_title("Klassentrennung je Hauptkomponente (blau = im 95 %-Modell, rot = weggelassen)")
plt.tight_layout(); plt.show()

load = pd.DataFrame(pca_full.components_[7:].T, index=FEATURES, columns=["PC8", "PC9", "PC10"]).round(2)
print("Ladungen der weggelassenen Komponenten:")
display(load)
""")

# Zelle isoliert ausführen: Prelude (Setup, Daten, PCA) + neue Zelle
need = [c for c in src.cells if c.cell_type == "code" and any(
    k in c.source for k in ["import time", "FEATURES = ['fLength'", "scaler = StandardScaler().fit(X_train)"])]
tmp = nbf.v4.new_notebook(); tmp.metadata = src.metadata
tmp.cells = [copy.deepcopy(c) for c in need] + [copy.deepcopy(extra_code)]
NotebookClient(tmp, timeout=600, kernel_name="python3").execute()
extra_code.outputs = tmp.cells[-1].outputs
extra_code.execution_count = None

# --- Diskussion -----------------------------------------------------------------
disc = open("discussion.md", encoding="utf-8").read().split("\n=====\n")
disc_cells = [nbf.v4.new_markdown_cell(t.strip("\n")) for t in disc]

out = []
for c in run.cells:
    if "DISC" in c.metadata.get("tags", []):
        out += [extra_md, extra_code] + disc_cells
    else:
        out.append(c)
run.cells = out
nbf.write(run, "Miniproject_PCA_final.ipynb")
print("final geschrieben:", len(out), "Zellen")
