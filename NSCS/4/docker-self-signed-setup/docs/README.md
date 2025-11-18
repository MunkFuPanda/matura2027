# Self-Signed Certificate mit OpenSSL und Docker

Diese Implementierung der OpenSSL Public-Key Übung verwendet Docker Container für eine plattformunabhängige Lösung.

## Struktur

- `certs/` - Generierte Zertifikate und Keys
- `config/` - OpenSSL Konfigurationsdateien
- `scripts/` - Automatisierungsskripte
- `web/` - WebServer Content
- `docker-compose.yml` - Container Orchestrierung

## Verwendung

### 1. Root-CA und Server-Zertifikat erstellen

```bash
./scripts/generate-certificates.sh
```

Dieses Script:
- Erstellt eine Root-CA mit privatem Key (Passwort: Labor4)
- Generiert ein Server-Zertifikat
- Signiert das Server-Zertifikat mit der Root-CA

### 2. WebServer starten

```bash
docker-compose up -d
```

Der WebServer läuft auf:
- HTTPS: https://localhost
- HTTPS: https://myexample.com (hosts-Eintrag erforderlich)

### 3. Zertifikate inspizieren

```bash
# Root-Zertifikat anzeigen
openssl x509 -in certs/rootCA.crt -text -noout

# Server-Zertifikat anzeigen
openssl x509 -in certs/server.crt -text -noout
```

### 4. Root-Zertifikat in Browser importieren

Um die SSL-Warnung im Browser zu vermeiden:

1. Öffne `certs/rootCA.crt`
2. Importiere es in deinen Browser als vertrauenswürdige Root-CA
   - **Chrome/Edge**: Einstellungen → Datenschutz → Zertifikate verwalten → Vertrauenswürdige Stammzertifizierungsstellen
   - **Firefox**: Einstellungen → Datenschutz → Zertifikate → Zertifikate anzeigen → Zertifizierungsstellen

### 5. Hosts-Datei anpassen (Optional)

Füge folgende Einträge zu `/etc/hosts` (Linux/Mac) oder `C:\Windows\System32\drivers\etc\hosts` (Windows) hinzu:

```
127.0.0.1 myexample.com
127.0.0.1 sub.myexample.com
127.0.0.1 myexample1.com
```

## Aufräumen

```bash
docker-compose down
./scripts/clean.sh
```
