import nbformat as nbf

nb = nbf.v4.new_notebook()
cells = []


def md(text, tag=None):
    c = nbf.v4.new_markdown_cell(text.strip("\n"))
    if tag:
        c.metadata["tags"] = [tag]
    cells.append(c)


def code(text):
    cells.append(nbf.v4.new_code_cell(text.strip("\n")))


md("""
# Miniprojekt Klassifikation: Erweiterung um PCA (95 % Varianz)

**Autor:** Spitzer (Datenbasis: Miniprojekt Klassifikation, Team Spitzer und Strobl)
**Datensatz:** MAGIC Gamma Telescope (Kaggle), binäre Klassifikation `g` (Gamma, Signal) gegen `h` (Hadron, Hintergrund)

## Aufgabenstellung

1. PCA mit so vielen Komponenten, dass **95 % der Varianz** abgedeckt sind, auf das Miniprojekt der 4. Klasse anwenden, und zwar für das **"beste" Random Forest (RF)** und das **"beste" k-NN**.
2. **Auswirkung auf Performance (Zeit messen!) und Modellqualität** diskutieren.
3. **Abbildung 8-8** (Explained Variance als Funktion der Dimensionen, *Hands-On Machine Learning*, Kap. 8) für unseren Fall nachzeichnen.

## Vorgehen

| Schritt | Inhalt |
|---|---|
| 1 | Daten und Split exakt wie im Miniprojekt laden (bereinigte `korr.csv`, 80/20 stratifiziert, `random_state=42`) |
| 2 | "Beste" Modelle mit den **Hyperparametern aus dem Miniprojekt** neu aufbauen und Ergebnis gegen das Miniprojekt prüfen (Sanity-Check) |
| 3 | PCA: Standardisieren, dann `PCA(n_components=0.95)`, **nur auf den Trainingsdaten gefittet** |
| 4 | Abbildung 8-8 für unseren Datensatz |
| 5 | Vergleich ohne/mit PCA: Qualität (Validierungsdaten und 5-fach-CV) und Zeit (mehrfach gemessen, Median) |
| 6 | Zusatz: Wie hängen Qualität und Zeit von der Komponentenzahl ab, und ändert Re-Tuning etwas? |
| 7 | Diskussion und Fazit |
""")

md("## 1. Setup und Daten")
code("""
import time
import warnings
warnings.filterwarnings("ignore")

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

from sklearn.model_selection import train_test_split, GridSearchCV, StratifiedKFold, cross_val_score
from sklearn.preprocessing import StandardScaler
from sklearn.decomposition import PCA
from sklearn.pipeline import Pipeline
from sklearn.ensemble import RandomForestClassifier
from sklearn.neighbors import KNeighborsClassifier
from sklearn.metrics import (accuracy_score, precision_score, recall_score, f1_score,
                             matthews_corrcoef, roc_auc_score)

RANDOM_STATE = 42
np.random.seed(RANDOM_STATE)
sns.set_theme(style="whitegrid")
pd.set_option("display.max_columns", None)
pd.set_option("display.width", 200)
print("Setup OK | numpy", np.__version__, "| pandas", pd.__version__)
""")
md("""
Wir laden die **bereinigte** Datei `korr.csv` aus dem Miniprojekt (Duplikate entfernt, 18 905 Zeilen, Spalte `target`: g=1, h=0). Der Split ist derselbe wie dort: 80 % Training, 20 % Validierung, stratifiziert, `random_state=42`. Damit sind die Ergebnisse direkt mit dem Miniprojekt vergleichbar.
""")
code("""
FEATURES = ['fLength', 'fWidth', 'fSize', 'fConc', 'fConc1', 'fAsym',
            'fM3Long', 'fM3Trans', 'fAlpha', 'fDist']

df = pd.read_csv("korr.csv")
X = df[FEATURES].copy()
y = df["target"].copy()

X_train, X_val, y_train, y_val = train_test_split(
    X, y, test_size=0.20, stratify=y, random_state=RANDOM_STATE)

print("Gesamt:", df.shape, "| Train:", X_train.shape, "| Validation:", X_val.shape)
print("Anteil Klasse g (1): Train %.3f | Val %.3f" % (y_train.mean(), y_val.mean()))
""")

md("""
## 2. Die "besten" Modelle aus dem Miniprojekt

Im Miniprojekt wurde per `GridSearchCV` (cv=5, Scoring F1) optimiert. Die Gewinner-Parameter übernehmen wir **unverändert**:

| Modell | Beste Parameter (aus dem Miniprojekt) | Pipeline im Miniprojekt |
|---|---|---|
| **Random Forest** | `n_estimators=400`, `max_depth=None`, `min_samples_leaf=1`, `class_weight='balanced'` | ohne Skalierung (Bäume sind skaleninvariant) |
| **k-NN** | `n_neighbors=11`, `p=1` (Manhattan), `weights='distance'` | `StandardScaler` + k-NN |

Der Random Forest war außerdem das insgesamt beste der fünf Modelle (F1 = 0,9106 auf der Validierung). Diese Werte dienen als Sanity-Check, dass der Neuaufbau mit dem Miniprojekt übereinstimmt.
""")
code("""
def make_rf(n_jobs=-1, seed=RANDOM_STATE):
    return RandomForestClassifier(n_estimators=400, max_depth=None, min_samples_leaf=1,
                                  class_weight="balanced", random_state=seed, n_jobs=n_jobs)

def make_knn():
    return KNeighborsClassifier(n_neighbors=11, p=1, weights="distance")

# Basis-Pipelines (ohne PCA) wie im Miniprojekt
def rf_base(n_jobs=-1):
    return Pipeline([("clf", make_rf(n_jobs))])

def knn_base():
    return Pipeline([("scaler", StandardScaler()), ("clf", make_knn())])

# Pipelines MIT PCA: Standardisieren -> PCA(95 % Varianz) -> Modell
def rf_pca(n_jobs=-1):
    return Pipeline([("scaler", StandardScaler()),
                     ("pca", PCA(n_components=0.95, random_state=RANDOM_STATE)),
                     ("clf", make_rf(n_jobs))])

def knn_pca():
    return Pipeline([("scaler", StandardScaler()),
                     ("pca", PCA(n_components=0.95, random_state=RANDOM_STATE)),
                     ("clf", make_knn())])

def quality(pipe, Xv, yv):
    \"\"\"Alle Gütemaße auf den Validierungsdaten.\"\"\"
    pred = pipe.predict(Xv)
    proba = pipe.predict_proba(Xv)[:, 1]
    return {"Accuracy": accuracy_score(yv, pred), "Precision": precision_score(yv, pred),
            "Recall": recall_score(yv, pred), "F1": f1_score(yv, pred),
            "MCC": matthews_corrcoef(yv, pred), "ROC-AUC": roc_auc_score(yv, proba)}

# Sanity-Check gegen das Miniprojekt (RF dort: Accuracy 0.8791, F1(g) 0.9106)
chk = rf_base().fit(X_train, y_train)
q = quality(chk, X_val, y_val)
print("RF ohne PCA   : Accuracy %.4f | F1 %.4f   (Miniprojekt: 0.8791 | 0.9106)" % (q["Accuracy"], q["F1"]))
chk = knn_base().fit(X_train, y_train)
q = quality(chk, X_val, y_val)
print("kNN ohne PCA  : Accuracy %.4f | F1 %.4f" % (q["Accuracy"], q["F1"]))
""")
md("""
**Hinweis zum Sanity-Check:** Der Neuaufbau trifft den Wert aus dem Miniprojekt nicht unbedingt auf die vierte Stelle. Ein Random Forest ist zufallsabhängig (Bootstrap, Feature-Auswahl), und die Zufallsströme können sich zwischen scikit-learn-Versionen unterscheiden. Wie groß dieses Rauschen ist, messen wir mit 5 verschiedenen Seeds. Das ist die Messlatte, an der wir den PCA-Effekt später beurteilen müssen: Ein Unterschied unterhalb dieser Streuung wäre kein belastbarer Befund.
""")
code("""
seed_f1 = []
for sd in [0, 1, 2, 3, 42]:
    m = Pipeline([("clf", make_rf(seed=sd))]).fit(X_train, y_train)
    seed_f1.append(f1_score(y_val, m.predict(X_val)))
print("RF ohne PCA, F1 auf Validierung für 5 Seeds:", np.round(seed_f1, 4))
print("Mittel %.4f | Std %.4f | Min %.4f | Max %.4f   (Miniprojekt: 0.9106)" %
      (np.mean(seed_f1), np.std(seed_f1), np.min(seed_f1), np.max(seed_f1)))
""")

md("""
## 3. PCA auf 95 % erklärte Varianz

**Warum zuerst standardisieren?** PCA sucht die Richtungen größter Varianz. Unsere Features haben völlig unterschiedliche Einheiten und Größenordnungen (`fLength` in mm mit Werten bis 334, `fConc` ein Verhältnis zwischen 0 und 1). Ohne Skalierung würden die Features mit großen Zahlen (`fLength`, `fAsym`, `fM3Long`, `fDist`) die Hauptkomponenten allein bestimmen. Deshalb gilt: `StandardScaler`, dann PCA.

**Warum nur auf den Trainingsdaten fitten?** Skalierer und PCA lernen aus den Daten (Mittelwert, Standardabweichung, Achsen). Würden wir sie auf den Validierungsdaten mitfitten, würde Information aus der Validierung ins Training fließen (Data Leakage). Die Validierungsdaten werden nur mit `transform` abgebildet. In den Pipelines ist das automatisch richtig, auch innerhalb der Kreuzvalidierung.

`PCA(n_components=0.95)` wählt automatisch die kleinste Komponentenzahl, deren kumulierte erklärte Varianz mindestens 95 % beträgt.
""")
code("""
# Volle PCA (alle 10 Komponenten) auf standardisierten Trainingsdaten
scaler = StandardScaler().fit(X_train)
X_train_s = scaler.transform(X_train)
pca_full = PCA(random_state=RANDOM_STATE).fit(X_train_s)

evr = pca_full.explained_variance_ratio_
cumsum = np.cumsum(evr)
d = int(np.argmax(cumsum >= 0.95) + 1)          # kleinste Dimensionszahl mit >= 95 %

# Gegenprobe mit der sklearn-Kurzform n_components=0.95
pca95 = PCA(n_components=0.95, random_state=RANDOM_STATE).fit(X_train_s)
assert pca95.n_components_ == d

tab = pd.DataFrame({"Komponente": [f"PC{i+1}" for i in range(len(evr))],
                    "Erklärte Varianz": evr, "Kumuliert": cumsum})
display(tab.style.format({"Erklärte Varianz": "{:.4f}", "Kumuliert": "{:.4f}"})
        .background_gradient(cmap="Blues", subset=["Kumuliert"]))
print(f"-> Für 95 % der Varianz werden d = {d} von {X.shape[1]} Komponenten benötigt "
      f"(kumuliert {cumsum[d-1]:.4f}). Reduktion der Dimension um {(1 - d/X.shape[1]):.0%}.")
""")

md("""
## 4. Abbildung 8-8 für unseren Fall

Nachgebaut nach dem Code aus dem Notebook zu *Hands-On Machine Learning* (Kap. 8, Abbildung 8-8: *Explained variance as a function of the number of dimensions*). Angepasst wurde nur, was der Datensatz erfordert: Der Originalcode verwendet MNIST mit bis zu 784 Dimensionen (`axis([0, 400, 0, 1])`, "Elbow" bei x≈65). Bei uns gibt es 10 Dimensionen, deshalb sind Achsen und Beschriftungsposition angepasst. Die x-Achse beginnt bei 1 (Anzahl Komponenten), die Markierung für 95 % wird aus den Daten berechnet statt von Hand gesetzt.

Den "Elbow" (Knick der Kurve) bestimmen wir objektiv als den Punkt mit dem größten Abstand zur Sehne zwischen erstem und letztem Punkt der Kurve.
""")
code("""
# Elbow = Punkt der Kurve mit größtem Abstand zur Sehne (erster -> letzter Punkt)
xs = np.arange(1, len(cumsum) + 1)
p1, p2 = np.array([xs[0], cumsum[0]]), np.array([xs[-1], cumsum[-1]])
vec = (p2 - p1) / np.linalg.norm(p2 - p1)
pts = np.column_stack([xs, cumsum]) - p1
dist = np.abs(pts[:, 0] * vec[1] - pts[:, 1] * vec[0])
elbow = int(xs[np.argmax(dist)])

# --- Abbildung 8-8 (Code nach ageron/handson-ml2, angepasst auf 10 Dimensionen) ---
plt.figure(figsize=(6, 4))
plt.plot(xs, cumsum, linewidth=3)
plt.axis([1, len(cumsum), 0, 1.02])
plt.xlabel("Dimensions")
plt.ylabel("Explained Variance")
plt.plot([d, d], [0, 0.95], "k:")
plt.plot([1, d], [0.95, 0.95], "k:")
plt.plot(d, 0.95, "ko")
plt.annotate("Elbow", xy=(elbow, cumsum[elbow - 1]), xytext=(elbow + 2, cumsum[elbow - 1] - 0.25),
             arrowprops=dict(arrowstyle="->"), fontsize=16)
plt.annotate(f"95 % bei d = {d}", xy=(d, 0.95), xytext=(d + 0.3, 0.55), fontsize=11)
plt.xticks(xs)
plt.grid(True)
plt.title("Abbildung 8-8 für MAGIC Gamma Telescope", fontsize=11)
plt.tight_layout()
plt.savefig("abb_8_8_explained_variance.png", dpi=150)
plt.show()
print(f"Elbow bei {elbow} Komponenten (kumuliert {cumsum[elbow-1]:.3f}); 95 %-Schwelle bei d = {d}.")
""")
md("""
Zusätzlich zur geforderten Abbildung zeigen wir, **woraus** die Hauptkomponenten bestehen (Ladungen der Features) und wie sich die Varianz pro Komponente verteilt (Scree-Plot). Das hilft bei der Interpretation der Diskussion weiter unten.
""")
code("""
fig, axes = plt.subplots(1, 2, figsize=(15, 4.8), gridspec_kw={"width_ratios": [1, 1.5]})

axes[0].bar(xs, evr, color="tab:blue")
axes[0].plot(xs, evr, "ko-")
axes[0].axhline(1 / len(evr), color="tab:red", ls="--", label="Kaiser: 1/10 der Gesamtvarianz")
axes[0].set_xlabel("Komponente"); axes[0].set_ylabel("Erklärte Varianz (Anteil)")
axes[0].set_title("Scree-Plot"); axes[0].set_xticks(xs); axes[0].legend()

loadings = pd.DataFrame(pca_full.components_[:d].T, index=FEATURES,
                        columns=[f"PC{i+1}" for i in range(d)])
sns.heatmap(loadings, annot=True, fmt=".2f", cmap="coolwarm", center=0, ax=axes[1],
            cbar_kws={"shrink": .8})
axes[1].set_title(f"Ladungen der ersten {d} Hauptkomponenten (Features -> Komponenten)")
plt.tight_layout(); plt.show()
""")

md("""
## 5. Vergleich ohne und mit PCA

### 5.1 Modellqualität auf den Validierungsdaten

Alle vier Pipelines werden auf den Trainingsdaten gefittet und auf den (nie gesehenen) Validierungsdaten bewertet. Neben Accuracy zählen wegen der Klassenimbalance (65 % `g` zu 35 % `h`) vor allem **F1, MCC und ROC-AUC**. Der Einzelsplit ist eine Momentaufnahme, deshalb folgt in 5.2 zusätzlich eine 5-fach-Kreuzvalidierung.
""")
code("""
pipes = {"RF ohne PCA": rf_base, "RF mit PCA": rf_pca,
         "kNN ohne PCA": knn_base, "kNN mit PCA": knn_pca}
MET = ["Accuracy", "Precision", "Recall", "F1", "MCC", "ROC-AUC"]

fitted, rows = {}, []
for name, mk in pipes.items():
    p = mk().fit(X_train, y_train)
    fitted[name] = p
    r = quality(p, X_val, y_val)
    r["Dimensionen"] = p.named_steps["pca"].n_components_ if "pca" in p.named_steps else X.shape[1]
    rows.append(pd.Series(r, name=name))
qual = pd.DataFrame(rows)

display(qual.style.format({c: "{:.4f}" for c in MET})
        .format({"Dimensionen": "{:.0f}"})
        .background_gradient(cmap="Greens", subset=["F1", "MCC", "ROC-AUC"]))

# Änderung durch PCA (in Prozentpunkten)
delta = pd.DataFrame({
    "RF":  (qual.loc["RF mit PCA", MET] - qual.loc["RF ohne PCA", MET]) * 100,
    "kNN": (qual.loc["kNN mit PCA", MET] - qual.loc["kNN ohne PCA", MET]) * 100})
print("\\nÄnderung durch PCA in Prozentpunkten (mit minus ohne):")
display(delta.style.format("{:+.2f}"))
""")

md("### 5.2 Robustheit: 5-fach-Kreuzvalidierung (PCA liegt in der Pipeline, kein Leakage)")
code("""
skf = StratifiedKFold(n_splits=5, shuffle=True, random_state=RANDOM_STATE)
cv_rows = []
for name, mk in pipes.items():
    t0 = time.perf_counter()
    s = cross_val_score(mk(), X_train, y_train, cv=skf, scoring="f1")
    cv_rows.append({"Modell": name, "F1 (CV Mittel)": s.mean(), "F1 (CV Std)": s.std(),
                    "Min": s.min(), "Max": s.max(), "CV-Dauer [s]": time.perf_counter() - t0})
cv_df = pd.DataFrame(cv_rows).set_index("Modell")
display(cv_df.style.format({"F1 (CV Mittel)": "{:.4f}", "F1 (CV Std)": "{:.4f}", "Min": "{:.4f}",
                            "Max": "{:.4f}", "CV-Dauer [s]": "{:.1f}"}))
""")

md("""
### 5.3 Zeitmessung

Wir messen mit `time.perf_counter()`, **jede Messung 7-mal wiederholt** und bewerten den **Median** (robust gegen Ausreißer durch andere Prozesse auf der Maschine). Gemessen wird die **gesamte Pipeline**, also bei den PCA-Varianten inklusive Standardisieren und PCA-Fit bzw. -Transform. Alles andere wäre unfair, denn die PCA kostet selbst Zeit.

* **Fit-Zeit:** `pipe.fit(X_train, y_train)` (15 124 Zeilen)
* **Predict-Zeit:** `pipe.predict(X_val)` (3 781 Zeilen)

Beim **k-NN** ist die Fit-Zeit fast null (er speichert nur die Daten, "lazy learner"), die Arbeit steckt im Vorhersagen. Beim **Random Forest** ist es umgekehrt.
""")
code("""
N_REP = 7

def timed(fn):
    t0 = time.perf_counter(); out = fn(); return time.perf_counter() - t0, out

time_rows = []
for name, mk in pipes.items():
    fit_t, pred_t = [], []
    for _ in range(N_REP):
        p = mk()
        t, _ = timed(lambda: p.fit(X_train, y_train)); fit_t.append(t)
        t, _ = timed(lambda: p.predict(X_val));        pred_t.append(t)
    time_rows.append({"Modell": name,
                      "Fit Median [s]": np.median(fit_t), "Fit Std [s]": np.std(fit_t),
                      "Predict Median [s]": np.median(pred_t), "Predict Std [s]": np.std(pred_t),
                      "Predict je 1000 Zeilen [ms]": np.median(pred_t) / len(X_val) * 1e6})
tm = pd.DataFrame(time_rows).set_index("Modell")
tm["Fit+Predict [s]"] = tm["Fit Median [s]"] + tm["Predict Median [s]"]
display(tm.style.format("{:.4f}"))

print("\\nVeränderung durch PCA (Faktor; < 1 heißt schneller mit PCA):")
for m in ["RF", "kNN"]:
    a, b = tm.loc[f"{m} ohne PCA"], tm.loc[f"{m} mit PCA"]
    print(f"  {m:3s}: Fit x{b['Fit Median [s]']/a['Fit Median [s]']:.2f} | "
          f"Predict x{b['Predict Median [s]']/a['Predict Median [s]']:.2f} | "
          f"Fit+Predict x{b['Fit+Predict [s]']/a['Fit+Predict [s]']:.2f}")
""")
md("Aufschlüsselung: Was kostet die PCA selbst, und was spart das nachfolgende Modell?")
code("""
# Kosten der Vorverarbeitung allein (Scaler + PCA)
t_fit, t_tr = [], []
for _ in range(N_REP):
    pre = Pipeline([("scaler", StandardScaler()),
                    ("pca", PCA(n_components=0.95, random_state=RANDOM_STATE))])
    t, _ = timed(lambda: pre.fit(X_train)); t_fit.append(t)
    t, _ = timed(lambda: pre.transform(X_val)); t_tr.append(t)
print(f"Scaler + PCA fit      : {np.median(t_fit)*1000:7.2f} ms (Median)")
print(f"Scaler + PCA transform: {np.median(t_tr)*1000:7.2f} ms (Median, {len(X_val)} Zeilen)")

# Reine Modellzeit (ohne Vorverarbeitung) auf bereits transformierten Daten
Xtr_p, Xva_p = pre.transform(X_train), pre.transform(X_val)
Xtr_s, Xva_s = scaler.transform(X_train), scaler.transform(X_val)
def clf_times(mk, Xa, Xb):
    f, p = [], []
    for _ in range(N_REP):
        c = mk()
        t, _ = timed(lambda: c.fit(Xa, y_train)); f.append(t)
        t, _ = timed(lambda: c.predict(Xb));      p.append(t)
    return np.median(f), np.median(p)

res = {"RF  (Rohdaten, 10 Dim.)": clf_times(make_rf, X_train.values, X_val.values),
       f"RF  (PCA, {d} Dim.)": clf_times(make_rf, Xtr_p, Xva_p),
       "kNN (standardisiert, 10 Dim.)": clf_times(make_knn, Xtr_s, Xva_s),
       f"kNN (PCA, {d} Dim.)": clf_times(make_knn, Xtr_p, Xva_p)}
core = pd.DataFrame(res, index=["Fit [s]", "Predict [s]"]).T
print("\\nReine Klassifikator-Zeit ohne Vorverarbeitung:")
display(core.style.format("{:.4f}"))
""")

md("### 5.4 Zeit gegen Qualität auf einen Blick")
code("""
fig, axes = plt.subplots(1, 3, figsize=(17, 4.6))
names = list(pipes.keys())
colors = ["tab:blue", "tab:orange", "tab:blue", "tab:orange"]

axes[0].bar(names, qual["F1"], color=colors)
axes[0].set_ylim(qual["F1"].min() - 0.02, qual["F1"].max() + 0.01)
axes[0].set_title("F1 (Validierung)"); axes[0].tick_params(axis="x", rotation=20)
for i, v in enumerate(qual["F1"]): axes[0].text(i, v + 0.0005, f"{v:.4f}", ha="center", fontsize=9)

axes[1].bar(names, tm["Fit Median [s]"], color=colors)
axes[1].set_title("Fit-Zeit (Median) [s]"); axes[1].tick_params(axis="x", rotation=20)
for i, v in enumerate(tm["Fit Median [s]"]): axes[1].text(i, v, f"{v:.2f}", ha="center", va="bottom", fontsize=9)

axes[2].bar(names, tm["Predict Median [s]"], color=colors)
axes[2].set_title("Predict-Zeit (Median) [s]"); axes[2].tick_params(axis="x", rotation=20)
for i, v in enumerate(tm["Predict Median [s]"]): axes[2].text(i, v, f"{v:.3f}", ha="center", va="bottom", fontsize=9)

fig.suptitle("Blau = ohne PCA, orange = mit PCA (95 % Varianz)", fontsize=12)
plt.tight_layout(); plt.show()
""")

md("""
## 6. Zusatzanalysen

### 6.1 Wie hängen Qualität und Zeit von der Komponentenzahl ab?

Die 95 %-Regel ist eine Konvention. Um zu sehen, ob sie hier sinnvoll sitzt, trainieren wir beide Modelle mit 1 bis 10 Komponenten (bei 10 Komponenten ist es reine Rotation ohne Informationsverlust).
""")
code("""
sweep = []
for k in range(1, X.shape[1] + 1):
    for name, mk in [("RF", make_rf), ("kNN", make_knn)]:
        pp = Pipeline([("scaler", StandardScaler()), ("pca", PCA(n_components=k, random_state=RANDOM_STATE)),
                       ("clf", mk())])
        ft, _ = timed(lambda: pp.fit(X_train, y_train))
        pt, pred = timed(lambda: pp.predict(X_val))
        sweep.append({"k": k, "Modell": name, "F1": f1_score(y_val, pred),
                      "MCC": matthews_corrcoef(y_val, pred),
                      "Fit [s]": ft, "Predict [s]": pt, "Varianz": cumsum[k - 1]})
sw = pd.DataFrame(sweep)

piv = sw.pivot(index="k", columns="Modell", values="F1").rename(columns={"RF": "F1 RF", "kNN": "F1 kNN"})
piv.insert(0, "Kumulierte Varianz", cumsum[:len(piv)])
display(piv[["Kumulierte Varianz", "F1 RF", "F1 kNN"]].style.format("{:.4f}"))

fig, axes = plt.subplots(1, 3, figsize=(17, 4.6))
for name, col in [("RF", "tab:green"), ("kNN", "tab:purple")]:
    s = sw[sw["Modell"] == name]
    axes[0].plot(s["k"], s["F1"], "o-", color=col, label=name)
    axes[1].plot(s["k"], s["Fit [s]"], "o-", color=col, label=name)
    axes[2].plot(s["k"], s["Predict [s]"], "o-", color=col, label=name)
axes[0].axhline(qual.loc["RF ohne PCA", "F1"], color="tab:green", ls=":", label="RF ohne PCA")
axes[0].axhline(qual.loc["kNN ohne PCA", "F1"], color="tab:purple", ls=":", label="kNN ohne PCA")
for ax, t in zip(axes, ["F1 (Validierung)", "Fit-Zeit [s] (einzelne Messung)", "Predict-Zeit [s] (einzelne Messung)"]):
    ax.axvline(d, color="k", ls="--", alpha=.6); ax.set_xlabel("Anzahl Hauptkomponenten"); ax.set_title(t)
    ax.set_xticks(range(1, 11)); ax.legend()
plt.tight_layout(); plt.show()
""")

md("""
### 6.2 Ändert Re-Tuning etwas?

Die Hyperparameter wurden im Originalraum optimiert. Im PCA-Raum könnten andere Werte besser sein. Wir wiederholen daher dieselben `GridSearchCV`-Läufe (selbe Gitter wie im Miniprojekt, cv=5, Scoring F1) auf den PCA-Daten und vergleichen mit den übernommenen Parametern. Das ist auch eine Zeitmessung für den **gesamten Optimierungsaufwand**, der in der Praxis oft wichtiger ist als eine einzelne Fit-Zeit.

Für faire Zeiten parallelisiert nur die Grid-Search (`n_jobs=-1`), der Random Forest läuft darin mit `n_jobs=1`. Sonst würden sich beide Ebenen die CPU-Kerne gegenseitig wegnehmen.
""")
code("""
grids = {
    "RF":  (rf_pca,  {"clf__n_estimators": [200, 400], "clf__max_depth": [10, 20, None], "clf__min_samples_leaf": [1, 5]}),
    "kNN": (knn_pca, {"clf__n_neighbors": [5, 11, 21, 31], "clf__weights": ["uniform", "distance"], "clf__p": [1, 2]}),
}
grids_base = {
    "RF":  (rf_base,  grids["RF"][1]),
    "kNN": (knn_base, grids["kNN"][1]),
}
tune_rows, tuned = [], {}
for variant, gdict in [("ohne PCA", grids_base), ("mit PCA", grids)]:
    for m, (mk, pg) in gdict.items():
        t0 = time.perf_counter()
        est = mk(n_jobs=1) if m == "RF" else mk()
        gs = GridSearchCV(est, pg, cv=5, scoring="f1", n_jobs=-1).fit(X_train, y_train)
        el = time.perf_counter() - t0
        tuned[(m, variant)] = gs
        r = quality(gs.best_estimator_, X_val, y_val)
        tune_rows.append({"Modell": f"{m} {variant}", "Grid-Search [s]": el, "CV-F1": gs.best_score_,
                          "Val-F1": r["F1"], "Val-MCC": r["MCC"],
                          "Beste Parameter": str({k.replace("clf__", ""): v for k, v in gs.best_params_.items()})})
tune = pd.DataFrame(tune_rows).set_index("Modell")
pd.set_option("display.max_colwidth", 120)
display(tune.style.format({"Grid-Search [s]": "{:.1f}", "CV-F1": "{:.4f}", "Val-F1": "{:.4f}", "Val-MCC": "{:.4f}"}))
""")

md("(Diskussion folgt)", tag="DISC")

nb["cells"] = cells
nb["metadata"]["kernelspec"] = {"display_name": "Python 3", "language": "python", "name": "python3"}
nbf.write(nb, "Miniproject_PCA.ipynb")
print("geschrieben, Zellen:", len(cells))
