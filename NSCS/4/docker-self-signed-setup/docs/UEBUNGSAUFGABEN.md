# Übungsaufgaben - Self-Signed Certificate

Diese Datei entspricht den Übungsanforderungen aus dem PDF.

## ✅ Task 1: Eigene (lokale) Root-CA erstellen

### a) Lokale Domänennamen im hosts-file anlegen
**Status:** Optional - kann manuell durchgeführt werden

Füge folgende Zeilen zu deiner `/etc/hosts` (Linux/Mac) bzw. `C:\Windows\System32\drivers\etc\hosts` (Windows) hinzu:
```
127.0.0.1 myexample.com
127.0.0.1 sub.myexample.com
127.0.0.1 myexample1.com
```

### b) Private Key für die Root-CA erstellen (rootCA.key)
**Status:** ✅ Erledigt

```bash
openssl genrsa -des3 -passout pass:Labor4 -out certs/rootCA.key 2048
```

**Resultat:**
- Datei: `certs/rootCA.key`
- Passwort: `Labor4`
- Verschlüsselung: DES3
- Key-Länge: 2048 Bit

### c) Ein Root-Zertifikat erstellen
**Status:** ✅ Erledigt

```bash
openssl req -x509 -new -nodes -key certs/rootCA.key -sha256 -days 1825 \
    -passin pass:Labor4 \
    -out certs/rootCA.crt \
    -config config/rootCA.cnf
```

**Resultat:**
- Datei: `certs/rootCA.crt`
- Hash-Algorithmus: SHA-256
- Gültigkeit: 5 Jahre (1825 Tage)
- Common Name: "Markus's Root Certificate"

**Konfiguration (config/rootCA.cnf):**
```ini
[req]
default_bits = 2048
prompt = no
default_md = sha256
distinguished_name = dn

[dn]
C=AT
ST=Niederoesterreich
L=Wiener Neustadt
O=HTLWRN
OU=Informatik
emailAddress=admin@example.com
CN = Markus's Root Certificate
```

### d) Zertifikat in den Zertifikatspeicher von Windows importieren
**Status:** Manuell mit Docker-Alternative

Da wir Docker verwenden (plattformunabhängig), erfolgt der Import direkt in den Browser.
Siehe detaillierte Anleitung in: [`BROWSER_IMPORT.md`](BROWSER_IMPORT.md)

### e) Zertifikatspeicher öffnen und Zertifikat anzeigen
**Status:** Alternative mit OpenSSL

```bash
# Vollständige Zertifikat-Details anzeigen
openssl x509 -in certs/rootCA.crt -text -noout

# Nur wichtige Informationen
openssl x509 -in certs/rootCA.crt -noout -subject -issuer -dates
```

**Optional - DER-Format Export:**
```bash
# Exportiere als DER (binäres Format)
openssl x509 -in certs/rootCA.crt -outform der -out certs/rootCA.der

# Vergleiche beide Formate
ls -lh certs/rootCA.crt certs/rootCA.der
```

### f) Root-Zertifikat in Browser importieren
**Status:** Erforderlich für SSL-Warnung zu vermeiden

Siehe ausführliche Anleitung: [`BROWSER_IMPORT.md`](BROWSER_IMPORT.md)

**Kurzanleitung:**
- **Firefox:** Einstellungen → Zertifikate → Zertifizierungsstellen → Importieren
- **Chrome/Edge:** Einstellungen → Sicherheit → Zertifikate verwalten → Zertifizierungsstellen

---

## ✅ Task 2: SSL-Zertifikat für lokalen WebServer erstellen

### g) Konfig-File für das Zertifikat anlegen (server.csr.cnf)
**Status:** ✅ Erledigt

**Datei: `config/server.csr.cnf`**
```ini
[req]
default_bits = 2048
prompt = no
default_md = sha256
distinguished_name = dn

[dn]
C=AT
ST=Niederoesterreich
L=Wiener Neustadt
O=HTLWRN
OU=Informatik
emailAddress=markus@example.com
CN = Markus
```

**Wichtig:** Der Schülername "Markus" erscheint sowohl im CN (Common Name) als auch in der Email-Adresse!

### h) Extra-File für die Domänennamen erstellen (v3.ext)
**Status:** ✅ Erledigt

**Datei: `config/v3.ext`**
```ini
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names

[alt_names]
DNS.1=myexample.com
DNS.2=sub.myexample.com
DNS.3=myexample1.com
DNS.4=localhost
IP.1=127.0.0.1
```

**Zusatz:** IP.1=127.0.0.1 wurde hinzugefügt für direkten Zugriff via IP

### i) Zertifikatsanfrage erstellen (CSR)
**Status:** ✅ Erledigt

**Schritt 1: Private Key für Server erstellen**
```bash
openssl genrsa -out certs/server.key 2048
```

**Resultat:**
- Datei: `certs/server.key`
- Key-Länge: 2048 Bit
- Kein Passwort (für einfache Verwendung im WebServer)

**Schritt 2: Certificate Signing Request erstellen**
```bash
openssl req -new -key certs/server.key \
    -out certs/server.csr \
    -config config/server.csr.cnf
```

**Resultat:**
- Datei: `certs/server.csr`
- Daten aus config/server.csr.cnf geladen

### j) CSR mit Root-Zertifikat signieren
**Status:** ✅ Erledigt

```bash
openssl x509 -req -in certs/server.csr \
    -CA certs/rootCA.crt \
    -CAkey certs/rootCA.key \
    -CAcreateserial \
    -passin pass:Labor4 \
    -out certs/server.crt \
    -days 825 -sha256 \
    -extfile config/v3.ext
```

**Resultat:**
- Datei: `certs/server.crt`
- Signiert von: "Markus's Root Certificate"
- Gültigkeit: 825 Tage (~2 Jahre)
- Enthält Subject Alternative Names (SANs)

---

## 🌐 WebServer mit HTTPS aktivieren und testen

### Docker WebServer Setup
**Status:** ✅ Erledigt

Der WebServer läuft in einem Nginx Docker-Container mit folgender Konfiguration:

**Nginx Konfiguration (`nginx.conf`):**
- HTTPS auf Port 443 (TLS 1.2, TLS 1.3)
- HTTP auf Port 80 (automatischer Redirect zu HTTPS)
- SSL-Zertifikate: `server.crt` und `server.key`
- Unterstützte Domains: localhost, myexample.com, sub.myexample.com, myexample1.com

**Docker Compose Setup:**
```bash
docker-compose up -d webserver
```

### WebServer testen

**1. Mit curl (SSL-Verifikation deaktiviert):**
```bash
curl -k https://localhost
```

**2. Mit curl (mit Root-CA Verifikation):**
```bash
curl --cacert certs/rootCA.crt https://localhost
```

**3. Mit Browser:**
Öffne https://localhost

**4. Mit OpenSSL:**
```bash
openssl s_client -connect localhost:443 -CAfile certs/rootCA.crt
```

---

## 📊 Übersicht der generierten Dateien

| Datei | Beschreibung | Verwendung |
|-------|--------------|------------|
| `certs/rootCA.key` | Root-CA Private Key (Passwort: Labor4) | Zum Signieren von Zertifikaten |
| `certs/rootCA.crt` | Root-CA Zertifikat | Zum Importieren in Browser |
| `certs/server.key` | Server Private Key | WebServer SSL |
| `certs/server.csr` | Certificate Signing Request | Zwischenprodukt |
| `certs/server.crt` | Signiertes Server-Zertifikat | WebServer SSL |
| `certs/rootCA.srl` | Serial Number für signierte Zertifikate | Automatisch generiert |

---

## 🎯 Lernziele erreicht

- ✅ **Selbst eine Root-CA erstellen**
  - Private Key mit Passwort-Schutz (Labor4)
  - Root-Zertifikat mit X.509 Standard
  
- ✅ **Zertifikat für WebServer erstellen und signieren**
  - Schülername "Markus" im CN und Email
  - Von eigener Root-CA signiert
  
- ✅ **Root-CA in Zertifikatsspeicher legen**
  - Anleitung für alle Browser verfügbar
  - Import-Anleitung dokumentiert
  
- ✅ **Lokalen WebServer mit HTTPS aktivieren und testen**
  - Nginx Container mit SSL
  - Automatischer HTTP→HTTPS Redirect
  - Funktioniert mit allen konfigurierten Domains

---

## 🚀 Verwendung

### Alles auf einmal starten:
```bash
./quick-start.sh
```

### Oder Schritt für Schritt:

1. **Zertifikate erstellen:**
   ```bash
   ./scripts/generate-certificates.sh
   ```

2. **WebServer starten:**
   ```bash
   docker-compose up -d webserver
   ```

3. **Root-Zertifikat in Browser importieren:**
   Siehe [`BROWSER_IMPORT.md`](BROWSER_IMPORT.md)

4. **WebServer testen:**
   ```bash
   curl -k https://localhost
   ```
   Oder öffne https://localhost im Browser

### Logs ansehen:
```bash
docker-compose logs -f webserver
```

### WebServer stoppen:
```bash
docker-compose down
```

### Aufräumen:
```bash
./scripts/clean.sh
```

---

## 📚 Zusätzliche Ressourcen

- **[README.md](README.md)** - Projekt-Übersicht und Quick-Start
- **[BROWSER_IMPORT.md](BROWSER_IMPORT.md)** - Detaillierte Browser-Import Anleitung
- **[OPENSSL_COMMANDS.md](OPENSSL_COMMANDS.md)** - OpenSSL Befehls-Referenz
- **[Makefile](Makefile)** - Automatisierung mit Make

---

## 🔒 Sicherheitshinweise

**Wichtig für die Übung:**
- ✅ Passwort für Root-CA: `Labor4` (wie vorgegeben)
- ✅ Private Keys werden lokal gespeichert
- ✅ Zertifikate sind nur für lokale Tests

**Nicht für Produktionsumgebungen:**
- ❌ Selbst-signierte Zertifikate nicht öffentlich verwenden
- ❌ Root-CA Key sicher aufbewahren (nicht committen)
- ❌ Passwörter in Produktionsumgebungen komplexer wählen

---

## 🐳 Docker-Vorteile

Diese Implementierung mit Docker bietet mehrere Vorteile:

1. **Plattformunabhängig:** Funktioniert auf Windows, Linux und macOS
2. **Keine lokale OpenSSL-Installation nötig:** Alles läuft in Containern
3. **Reproduzierbar:** Identisches Setup auf allen Systemen
4. **Einfach zu resetten:** `docker-compose down && ./scripts/clean.sh`
5. **Isoliert:** Keine Änderungen am Host-System nötig
