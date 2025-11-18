# OpenSSL Zertifikats-Befehle Referenz

Nützliche OpenSSL-Befehle zum Analysieren und Verwalten von Zertifikaten.

## 📋 Zertifikat-Informationen anzeigen

### Vollständige Zertifikat-Details
```bash
openssl x509 -in certs/rootCA.crt -text -noout
openssl x509 -in certs/server.crt -text -noout
```

### Nur Subject und Issuer
```bash
openssl x509 -in certs/server.crt -noout -subject -issuer
```

### Nur Gültigkeitsdaten
```bash
openssl x509 -in certs/server.crt -noout -dates
```

### Subject Alternative Names (SANs)
```bash
openssl x509 -in certs/server.crt -noout -ext subjectAltName
```

### Fingerprint/Hash
```bash
openssl x509 -in certs/server.crt -noout -fingerprint -sha256
```

## 🔑 Private Key Informationen

### Private Key prüfen
```bash
openssl rsa -in certs/server.key -check
openssl rsa -in certs/rootCA.key -check -passin pass:Labor4
```

### Public Key aus Private Key extrahieren
```bash
openssl rsa -in certs/server.key -pubout -out certs/server.pub
```

### Prüfen ob Private Key zum Zertifikat passt
```bash
# Die Moduli müssen identisch sein
openssl x509 -noout -modulus -in certs/server.crt | openssl md5
openssl rsa -noout -modulus -in certs/server.key | openssl md5
```

## 📝 CSR (Certificate Signing Request) Informationen

### CSR Details anzeigen
```bash
openssl req -text -noout -in certs/server.csr
```

### CSR Subject anzeigen
```bash
openssl req -noout -subject -in certs/server.csr
```

## 🔄 Format-Konvertierungen

### PEM zu DER
```bash
openssl x509 -in certs/server.crt -outform der -out certs/server.der
```

### DER zu PEM
```bash
openssl x509 -in certs/server.der -inform der -out certs/server.pem
```

### PEM zu PKCS12 (.pfx)
```bash
openssl pkcs12 -export -out certs/server.pfx \
  -inkey certs/server.key \
  -in certs/server.crt \
  -certfile certs/rootCA.crt
```

### PKCS12 zu PEM
```bash
openssl pkcs12 -in certs/server.pfx -out certs/server-bundle.pem -nodes
```

## 🧪 Verbindungs-Tests

### HTTPS-Verbindung testen mit eigenem Zertifikat
```bash
openssl s_client -connect localhost:443 -CAfile certs/rootCA.crt
```

### HTTPS-Verbindung testen (alle Zertifikate akzeptieren)
```bash
openssl s_client -connect localhost:443 -showcerts
```

### Zeige Zertifikatskette
```bash
openssl s_client -connect localhost:443 -showcerts 2>/dev/null | \
  grep -A 100 "BEGIN CERTIFICATE" | \
  openssl x509 -text -noout
```

### Nur Cipher Suite testen
```bash
openssl s_client -connect localhost:443 -cipher 'HIGH' -brief
```

## 🔍 Zertifikat-Verifikation

### Zertifikat gegen Root-CA verifizieren
```bash
openssl verify -CAfile certs/rootCA.crt certs/server.crt
```

### Zertifikatskette verifizieren
```bash
cat certs/server.crt certs/rootCA.crt > certs/chain.pem
openssl verify -CAfile certs/rootCA.crt certs/chain.pem
```

## 📊 Zertifikat vergleichen

### Zeige Unterschiede zwischen zwei Zertifikaten
```bash
diff <(openssl x509 -in certs/rootCA.crt -noout -text) \
     <(openssl x509 -in certs/server.crt -noout -text)
```

## 🗂️ Zertifikat-Bundle erstellen

### Full Chain (Server + Root CA)
```bash
cat certs/server.crt certs/rootCA.crt > certs/fullchain.pem
```

### Mit Private Key
```bash
cat certs/server.key certs/server.crt certs/rootCA.crt > certs/bundle.pem
```

## 🔐 Passwort-geschützte Keys

### Passwort von Private Key entfernen
```bash
openssl rsa -in certs/rootCA.key -passin pass:Labor4 -out certs/rootCA-nopass.key
```

### Passwort zu Private Key hinzufügen
```bash
openssl rsa -in certs/server.key -des3 -out certs/server-encrypted.key
```

### Passwort ändern
```bash
openssl rsa -in certs/rootCA.key -passin pass:Labor4 -des3 -passout pass:NewPassword -out certs/rootCA-new.key
```

## 📈 Erweiterte Analysen

### Alle Extensions anzeigen
```bash
openssl x509 -in certs/server.crt -noout -ext subjectAltName,keyUsage,basicConstraints
```

### Key Usage
```bash
openssl x509 -in certs/server.crt -noout -ext keyUsage
```

### Basic Constraints
```bash
openssl x509 -in certs/server.crt -noout -ext basicConstraints
```

### Authority Key Identifier
```bash
openssl x509 -in certs/server.crt -noout -ext authorityKeyIdentifier
```

## 🔬 Debugging

### OpenSSL Version
```bash
openssl version -a
```

### Alle verfügbaren Cipher Suites
```bash
openssl ciphers -v 'ALL'
```

### TLS 1.3 Cipher Suites
```bash
openssl ciphers -v 'TLSv1.3'
```

## 💾 Batch-Export

### Alle Zertifikat-Infos in Datei speichern
```bash
{
  echo "=== Root CA ==="
  openssl x509 -in certs/rootCA.crt -text -noout
  echo ""
  echo "=== Server Certificate ==="
  openssl x509 -in certs/server.crt -text -noout
} > certs/certificate-info.txt
```
