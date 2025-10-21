## 🔏 **Aufgabe 4 – Digitale Signatur + Verschlüsselung**

**1. Nachricht verschlüsseln (zuerst):**

```bash
openssl pkeyutl -encrypt -inkey max_pubkey.pem -pubin -in meeting.txt -out meeting.enc
```

**2. Verschlüsselte Nachricht signieren:**

```bash
openssl dgst -sha256 -sign mykey.pem -out meeting.enc.sig meeting.enc
```

**→ Dateien senden:**

* `meeting.enc`
* `meeting.enc.sig`

---

**Empfänger prüft zuerst Signatur:**

```bash
openssl dgst -sha256 -verify markus_pubkey.pem -signature meeting.enc.sig meeting.enc
```

**Dann entschlüsselt er die Datei:**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in meeting.enc -out meeting_decrypted.txt
```