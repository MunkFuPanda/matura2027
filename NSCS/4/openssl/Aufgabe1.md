## **Aufgabe 1 – Keypair erzeugen**

**1. RSA-Schlüsselpaar (4096 Bit) mit Passwort erzeugen**

```bash
openssl genpkey -algorithm RSA -out mykey.pem -aes256 -pkeyopt rsa_keygen_bits:4096
```

> → Es wird nach einem Passwort gefragt (z. B. `secret`).

**2. Öffentlichen Schlüssel extrahieren**

```bash
openssl rsa -pubout -in mykey.pem -out markus_pubkey.pem
```

**3. Interne Zahlen (Primzahlen, Exponenten, usw.) ausgeben und speichern**

```bash
openssl rsa -in mykey.pem -text -noout > numbers.txt
```
