## **Aufgabe 2 – Asymmetrische Verschlüsselung**

Angenommen, du möchtest eine Nachricht an **Lennard** senden.
Lennard hat seinen Public Key `lennard_pubkey.pem` bereitgestellt.

**1. Nachricht erstellen**

```bash
echo "Hallo Lennard, treffen wir uns um 14:00 im Labor." > message.txt
```

**2. Nachricht mit Lennard’ öffentlichem Schlüssel verschlüsseln**

```bash
openssl pkeyutl -encrypt -inkey lennard_pubkey.pem -pubin -in message.txt -out message.enc
```

**3. Datei `message.enc` an Lennard übermitteln**

---

**Empfänger (Lennard) entschlüsselt sie mit seinem privaten Schlüssel:**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in message.enc -out message_decrypted.txt
```

**4. Antwort verschlüsseln (umgekehrte Richtung):**

```bash
echo "Ja passt, bis später!" > reply.txt
openssl pkeyutl -encrypt -inkey markus_pubkey.pem -pubin -in reply.txt -out reply.enc
```

**5. Antwort entschlüsseln:**

```bash
openssl pkeyutl -decrypt -inkey mykey.pem -in reply.enc -out reply_decrypted.txt
```
