# Übungsbeschreibung — Brute-Forcing & Entschlüsseln mit **bruteforcer**

---

## Allgemeines

* **Ziel:** Für drei verschlüsselte Dateien im Ordner `messages` das Passwort knacken und die Datei mit OpenSSL entschlüsseln, um die Lösung zu erhalten.
* **Benötigte Software:**

  * Das mitgelieferte Binary **`bruteforcer`**.
  * **OpenSSL** (Windows: am besten unter WSL).
* **Wichtig:** **Prüfe die Verschlüsselungsparameter** (Cipher, KDF, Iterationen). Diese Übung basiert auf **OpenSSL 3.6.0 (1 Oct 2025)** — Optionen/Defaults können anders sein. **Iterationen müssen beim Entschlüsseln exakt gesetzt werden.**
* **Hinweis:** Verwende beim Entschlüsseln exakt dieselben Optionen wie beim Verschlüsseln (`-pbkdf2`, digest: `sha256`, algorithmus: `aes-256-cbc`, iterations: `10000`).

---

## Vorgehensweise

1. Lies **MANUAL.pdf**, um `bruteforcer`-Optionen zu verstehen.
2. Passe `bruteforcer` an (Zeichensatz, Länge, Wordlists, Threads), um Suchräume klein zu halten.
3. Starte `bruteforcer` gegen die jeweilige `.enc`-Datei bis Passwort gefunden.
4. Entschlüssele mit OpenSSL, z. B.:

```bash
openssl enc -d -aes-256-cbc -pbkdf2 -in messages/<datei>.enc -out <datei>.txt -pass pass:<GEFUNDENES_PASSWORT>
```

5. Lies die entschlüsselte Datei — damit ist die Aufgabe gelöst.

---

## Aufgaben (Kurzbeschreibung)

* **Aufgabe 1:** Einstieg; keine konkreten Hinweise, Passwort ist 3-stellig.
* **Aufgabe 2:** Passwort besteht **nur aus Zahlen** → `bruteforcer` mit numerischem Zeichensatz (0–9) konfigurieren.
* **Aufgabe 3:** Standardpasswort → **Wörterbuchangriff** empfohlen (z. B. [passende Wordlist](https://github.com/CTzatzakis/Wordlists/blob/master/Words.list) verwenden).

## Laufzeiten (ungefähr)

Die Laufzeiten hängen stark von der Hardware und den Einstellungen ab.<br>
Hier einige grobe Richtwerte, basierend auf gut getuntem `bruteforcer` ohne zusätzliche Informationen:

* **Aufgabe 1:** ~30 Sekunden
* **Aufgabe 2:** ~6 Minuten 30 Sekunden
* **Aufgabe 3:** ~30 Sekunden (je nach Wordlist)

---

## Hinweise zur Feinabstimmung

* Je mehr Vorwissen (Zeichensatz, Länge, Wordlists) du einbringst, desto kürzer die Laufzeit.
* Vermeide unnötig große Schlüsselräume.
* Nutze Parallelisierung nur wenn sauber unterstützt.
* Setze **Stop-on-first** wenn verfügbar.

---

## Ablauf für die Nutzer

1. Mit `bruteforcer` das passende Passwort ermitteln.
2. `.enc`-Datei mit OpenSSL entschlüsseln (achten auf Cipher, `-pbkdf2`, **-iter**).
3. Entschlüsselte Nachricht prüfen und Lösung einreichen.

---

**Kurzwarnung:** Prüfe unbedingt Cipher-Namen und KDF-Parameter bevor du entschlüsselst.<br>
Falsche `-pbkdf2`/`-iter`/Cipher = Entschlüsselung schlägt fehl.<br>
Die Parameter -K -i -v 1 und -1 sind hilfreich/notwendig.

Viel Erfolg! 🚀

