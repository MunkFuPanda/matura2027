# **OpenSSL Notizen**

## **1. RSA-Keypair erzeugen**

**Privaten Schlüssel (4096 Bit, AES-geschützt) generieren:**

```bash
openssl genpkey -algorithm RSA -out mykey.pem -aes256 -pkeyopt rsa_keygen_bits:4096
```

→ Passwort z. B. `secret`

**Öffentlichen Schlüssel exportieren:**

```bash
openssl rsa -pubout -in mykey.pem -out markus_pubkey.pem
```

**Interne Werte anzeigen und speichern:**

```bash
openssl rsa -in mykey.pem -text -noout > numbers.txt
```

---

## **2. Asymmetrische Verschlüsselung**

Nachricht verschlüsseln und mit privatem Schlüssel des Empfängers wieder entschlüsseln.

**Nachricht erstellen:**

```bash
echo "Hallo Lennard, treffen wir uns um 14:00 im Labor." > message.txt
```

**Verschlüsseln (mit Lennards Public Key):**

```bash
openssl pkeyutl -encrypt -inkey lennard_pubkey.pem -pubin -in message.txt -out message.enc
```

**Entschlüsseln (mit eigenem Private Key):**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in message.enc -out message_decrypted.txt
```

**Antwort verschlüsseln:**

```bash
echo "Ja passt, bis später!" > reply.txt
openssl pkeyutl -encrypt -inkey markus_pubkey.pem -pubin -in reply.txt -out reply.enc
```

**Antwort entschlüsseln:**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in reply.enc -out reply_decrypted.txt
```

---

## **3. Digitale Signatur**

**Textdatei erstellen:**

```bash
echo "Treffen mit Lennard um 14:00 im Labor." > meeting.txt
```

**Hash berechnen (SHA256):**

```bash
openssl dgst -sha256 meeting.txt > meeting.hash
```

**Signieren:**

```bash
openssl dgst -sha256 -sign mykey.pem -out meeting.sig meeting.txt
```

**Verifizieren:**

```bash
openssl dgst -sha256 -verify markus_pubkey.pem -signature meeting.sig meeting.txt
```

→ Ausgabe bei korrekter Signatur: `Verified OK`

---

## **4. Digitale Signatur + Verschlüsselung**

**Nachricht verschlüsseln (zuerst):**

```bash
openssl pkeyutl -encrypt -inkey max_pubkey.pem -pubin -in meeting.txt -out meeting.enc
```

**Dann signieren:**

```bash
openssl dgst -sha256 -sign mykey.pem -out meeting.enc.sig meeting.enc
```

**Verifikation durch Empfänger:**

```bash
openssl dgst -sha256 -verify markus_pubkey.pem -signature meeting.enc.sig meeting.enc
```

**Danach entschlüsseln:**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in meeting.enc -out meeting_decrypted.txt
```

---

## **5. Hybride Verschlüsselung (AES + RSA)**

**Nachricht erstellen:**

```bash
echo "Das ist eine geheime Nachricht über AES." > hybrid.txt
```

**Symmetrisch mit AES verschlüsseln (passwortgeschützt):**

```bash
openssl enc -aes-256-cbc -salt -pbkdf2 -iter 100000 -in hybrid.txt -out hybrid.aes -pass pass:secret
```

**AES-Datei mit Public Key des Empfängers verschlüsseln:**

```bash
openssl pkeyutl -encrypt -inkey max_pubkey.pem -pubin -in hybrid.aes -out hybrid.enc
```

**Empfänger entschlüsselt:**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in hybrid.enc -out hybrid_decrypted.aes
```

**Danach AES-Datei entschlüsseln (Passwort `secret`):**

```bash
openssl enc -d -aes-256-cbc -pbkdf2 -iter 100000 -in hybrid_decrypted.aes -out hybrid_dec.txt -pass pass:secret
```