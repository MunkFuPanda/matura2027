## ⚙️ **Aufgabe 5 – Hybride Verschlüsselung (AES + RSA)**

**1. Nachricht erstellen**

```bash
echo "Das ist eine geheime Nachricht über AES." > hybrid.txt
```

**2. Symmetrisch mit AES (passwortgeschützt) verschlüsseln**

```bash
openssl enc -aes-256-cbc -salt -pbkdf2 -iter 100000 -in hybrid.txt -out hybrid.aes -pass pass:secret
```

**3. AES-Datei mit Public Key des Empfängers verschlüsseln**

```bash
openssl pkeyutl -encrypt -inkey max_pubkey.pem -pubin -in hybrid.aes -out hybrid.enc
```

**→ Datei `hybrid.enc` an Empfänger senden**

---

**Empfänger entschlüsselt mit seinem Private Key:**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in hybrid.enc -out hybrid_decrypted.aes
```

**Dann entschlüsselt er mit AES (Passwort: `secret`):**

```bash
openssl enc -d -aes-256-cbc -pbkdf2 -iter 100000 -in spizza/hybrid.aes -out spizza/hybrid_dec.txt -pass pass:secret

```