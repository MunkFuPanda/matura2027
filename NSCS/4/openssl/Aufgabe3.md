## ✍️ **Aufgabe 3 – Digitale Signatur**

**1. Textdatei erstellen**

```bash
echo "Treffen mit Lennard um 14:00 im Labor." > meeting.txt
```

**2. Hashwert erzeugen (z. B. SHA256):**

```bash
openssl dgst -sha256 meeting.txt > meeting.hash
```

**3. Datei signieren:**

```bash
openssl dgst -sha256 -sign mykey.pem -out meeting.sig meeting.txt
```

**→ Dateien an Mitschüler senden:**

* `meeting.txt`
* `meeting.sig`

---

**Empfänger verifiziert die Signatur:**

```bash
openssl dgst -sha256 -verify markus_pubkey.pem -signature meeting.sig meeting.txt
```

> Wenn alles korrekt ist, bekommst du:
> `Verified OK`