# Browser Root-CA Import Anleitung

Um SSL-Warnungen zu vermeiden, muss das Root-Zertifikat (`certs/rootCA.crt`) in deinen Browser importiert werden.

## 🦊 Firefox

1. Öffne Firefox Einstellungen
2. Suche nach "Zertifikate" oder navigiere zu:
   - **Datenschutz & Sicherheit** → **Zertifikate** → **Zertifikate anzeigen**
3. Klicke auf den Tab **"Zertifizierungsstellen"**
4. Klicke auf **"Importieren..."**
5. Wähle die Datei `certs/rootCA.crt` aus
6. Aktiviere **"Dieser CA vertrauen, um Websites zu identifizieren"**
7. Klicke auf **OK**

## 🌐 Chrome / Edge / Chromium-basierte Browser

### Windows

1. Doppelklick auf `certs/rootCA.crt`
2. Klicke auf **"Zertifikat installieren..."**
3. Wähle **"Lokaler Computer"** (Administrator-Rechte erforderlich)
4. Klicke auf **"Weiter"**
5. Wähle **"Alle Zertifikate in folgendem Speicher speichern"**
6. Klicke auf **"Durchsuchen..."**
7. Wähle **"Vertrauenswürdige Stammzertifizierungsstellen"**
8. Klicke auf **OK** und dann auf **"Weiter"**
9. Klicke auf **"Fertig stellen"**

### Linux

1. Öffne Chrome/Edge Einstellungen
2. Suche nach "Zertifikate" oder navigiere zu:
   - **Datenschutz und Sicherheit** → **Sicherheit** → **Zertifikate verwalten**
3. Klicke auf den Tab **"Zertifizierungsstellen"**
4. Klicke auf **"Importieren"**
5. Wähle die Datei `certs/rootCA.crt` aus
6. Aktiviere **"Websites damit vertrauen und identifizieren"**
7. Klicke auf **OK**

### macOS

1. Öffne die Datei `certs/rootCA.crt` mit **Schlüsselbundverwaltung** (Keychain Access)
2. Das Zertifikat wird automatisch zum Schlüsselbund hinzugefügt
3. Suche nach "Markus's Root Certificate" in der Schlüsselbundverwaltung
4. Doppelklick auf das Zertifikat
5. Erweitere **"Vertrauen"**
6. Bei **"Beim Verwenden dieses Zertifikats"** wähle **"Immer vertrauen"**
7. Schließe das Fenster (Administrator-Passwort erforderlich)

## 🧪 Verifizierung

Nachdem das Zertifikat importiert wurde:

1. Öffne https://localhost in deinem Browser
2. Du solltest **kein SSL-Warnung** mehr sehen
3. Das Schloss-Symbol in der Adressleiste sollte grün/sicher sein

## 🔍 Zertifikat-Details ansehen

Im Browser kannst du die Zertifikat-Details einsehen:

1. Klicke auf das **Schloss-Symbol** in der Adressleiste
2. Klicke auf **"Zertifikat"** oder **"Verbindung ist sicher"**
3. Unter **"Ausgestellt von"** sollte stehen: **"Markus's Root Certificate"**
4. Unter **"Ausgestellt für"** sollte stehen: **"Markus"**

## ⚠️ Sicherheitshinweis

**Wichtig:** Dieses Root-Zertifikat sollte nur für Testzwecke auf deinem lokalen Computer verwendet werden!

- ❌ Nicht in Produktionsumgebungen verwenden
- ❌ Nicht mit anderen teilen
- ❌ Nicht auf öffentlichen Computern installieren
- ✓ Nur für lokale Entwicklung und Tests

## 🗑️ Zertifikat wieder entfernen

### Firefox
Zertifikate → Zertifizierungsstellen → "Markus's Root Certificate" suchen → Löschen

### Chrome/Edge/Chromium
Einstellungen → Sicherheit → Zertifikate verwalten → Zertifizierungsstellen → "Markus's Root Certificate" suchen → Entfernen

### macOS Schlüsselbund
Schlüsselbundverwaltung öffnen → "Markus's Root Certificate" suchen → Rechtsklick → Löschen
