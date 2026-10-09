# 📚 Lernzettel – NSCS 3CHIF (DNS, Security, Firewall)

---

## 🌐 DNS (Domain Name System)

### Was ist DNS?
- DNS = Domain Name System
- Übersetzt Domainnamen (z. B. `google.com`) in IP-Adressen (z. B. `142.250.186.68`)

### DNS-Server-Arten:
- Root-Server: kennen TLD-Server
- TLD-Server: verwalten `.com`, `.org`, etc.
- Autoritativer Server: hat endgültige Antwort
- Recursive Resolver: fragt Kette ab
- Forwarder: leitet Anfragen weiter

### DNS-Auflösung:
1. Client → Resolver
2. Resolver → Root → TLD → Autoritativ
3. Antwort wird gecacht und zurückgegeben

### DNS-Cache:
- Speichert frühere Anfragen (TTL)
- Vorteile: schnellere Antworten, weniger Traffic

### DNS-Spoofing:
- Falsche DNS-Antwort → Umleitung auf Fake-Seiten
- Schutz: DNSSEC, sichere Resolver, DoT/DoH

### DNS-Zonen:
- Enthalten DNS-Einträge einer Domain
- Primary, Secondary, Reverse Zonen

### DNSSEC:
- Signiert DNS-Daten
- Schützt vor Manipulation
- Kette des Vertrauens: Root → TLD → Domain

### DNS-Tools:
- `/etc/resolv.conf`, `/etc/hosts`, `nslookup`
- Öffentliche Server: Google (8.8.8.8), Cloudflare (1.1.1.1), Quad9 (9.9.9.9)

### Load Balancing:
- Round-Robin, GeoDNS, Failover
- Verbessert Verfügbarkeit & Skalierbarkeit

---

## 🔐 Security

### 1. Phishing
- Betrug durch gefälschte Nachrichten/Webseiten
- Formen: Spear, Whaling, Vishing, Smishing
- Schutz: 2FA, Schulung, Filter

### 2. Ransomware
- Verschlüsselt Daten, fordert Lösegeld
- Schutz: Backups, aktuelle Software, Rechte einschränken

### 3. DDoS
- Überlastung durch viele Anfragen
- Schutz: DDoS-Dienste, Rate-Limiting, Blacklisting

### 4. Man-in-the-Middle (MitM)
- Abhören/Manipulation von Kommunikation
- Schutz: HTTPS, VPN, Zertifikate prüfen

### 5. SQL-Injection
- Schädlicher SQL-Code über Eingabefelder
- Schutz: Prepared Statements, Validierung

### 6. Malware
- Schadsoftware (Virus, Wurm, Trojaner, Spyware)
- Schutz: Antivirenprogramm, Updates, Rechte

### 7. Social Engineering
- Manipulation von Menschen (z. B. Fake-Anrufe)
- Schutz: Schulung, Identitätsprüfung

### 8. Zero-Day-Exploits
- Angriffe auf unbekannte Schwachstellen
- Schutz: Verhaltensanalyse, schnelles Patchen

### 9. Insider-Bedrohung
- Schaden durch eigene Mitarbeiter
- Schutz: Logging, Rechte einschränken, 4-Augen-Prinzip

### 10. DNS-Spoofing
- Siehe DNS-Sektion

### 11. Brute-Force
- Automatisches Erraten von Passwörtern
- Schutz: 2FA, Sperrmechanismen, Captchas

### 12. XSS (Cross-Site-Scripting)
- Schadcode in Webseiten eingeschleust
- Schutz: Escaping, CSP, sichere Frameworks

---

## 🔥 Firewalls

### Arten:
- **Hardware-Firewall**: physisch, performant
- **Application-Firewall**: Layer 7, z. B. für HTTP
- **NGFW**: Kombination aus Firewall, DPI, IDS/IPS

### Regeln & Filter:
- Filterung nach IP, Port, Protokoll, Anwendung
- ACLs: Zugriffskontrolllisten
- SPI vs. DPI: Header vs. Dateninhalt

### IDS/IPS:
- IDS: erkennt Angriffe
- IPS: erkennt & blockiert Angriffe

### Netzwerktopologie & DMZ:
- DMZ: Zone für öffentlich zugängliche Dienste
- Interne Firewalls für Segmentierung

### VPNs:
- **Site-to-Site**: Standortverbindung
- **Remote Access**: für Mitarbeiter
- Kombination mit Firewalls: Zugriff absichern

### DPI vs. SPI:
- DPI analysiert Inhalte, erkennt Malware
- Datenschutz & Ressourcenverbrauch beachten

### DoS-Schutz:
- Firewalls können begrenzen, blockieren
- Cloudlösungen ergänzend nützlich

### MFA:
- Mehrstufige Authentifizierung (z. B. Passwort + Code)
- Absicherung von Admin-Zugängen

### Cloud-Firewalls:
- Skalierbar, API-gesteuert
- Integration in Cloud-Umgebungen (AWS, Azure)

### Firewall-Management:
- Dokumentation, Monitoring, Audits
- Konfig-Backups, Regelpflege

### Performance:
- Abhängig von Sessions, Durchsatz
- Hardware-Firewalls = schneller, Software = flexibler

---

## ✅ Tipps
- **"Allow by default" ist schlecht.** → Immer "deny by default"
- **Dokumentiere Regeln!**
- **Updates und Patches** retten dich.
- **Segmentierung ist King.**
