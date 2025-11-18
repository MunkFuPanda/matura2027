#!/bin/bash

# Farben für Output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

CERT_DIR="."
CONFIG_DIR="/config"
PASSWORD="Labor4"

echo -e "${GREEN}=== OpenSSL Certificate Generation ===${NC}\n"

# Task 1: Root-CA erstellen
echo -e "${YELLOW}Task 1: Erstelle Root-CA...${NC}"

# a) Private Key für Root-CA erstellen
echo "  → Erstelle Private Key für Root-CA (rootCA.key)..."
openssl genrsa -des3 -passout pass:$PASSWORD -out "$CERT_DIR/rootCA.key" 2048

if [ $? -eq 0 ]; then
    echo -e "  ${GREEN}✓ Private Key erstellt${NC}"
else
    echo "  ✗ Fehler beim Erstellen des Private Keys"
    exit 1
fi

# b) Root-Zertifikat erstellen
echo "  → Erstelle Root-Zertifikat (rootCA.crt)..."
openssl req -x509 -new -nodes -key "$CERT_DIR/rootCA.key" -sha256 -days 1825 \
    -passin pass:$PASSWORD \
    -out "$CERT_DIR/rootCA.crt" \
    -config "$CONFIG_DIR/rootCA.cnf"

if [ $? -eq 0 ]; then
    echo -e "  ${GREEN}✓ Root-Zertifikat erstellt${NC}"
else
    echo "  ✗ Fehler beim Erstellen des Root-Zertifikats"
    exit 1
fi

echo ""

# Task 2: Server-Zertifikat erstellen
echo -e "${YELLOW}Task 2: Erstelle Server-Zertifikat...${NC}"

# c) Private Key für Server erstellen
echo "  → Erstelle Private Key für Server (server.key)..."
openssl genrsa -out "$CERT_DIR/server.key" 2048

if [ $? -eq 0 ]; then
    echo -e "  ${GREEN}✓ Private Key erstellt${NC}"
else
    echo "  ✗ Fehler beim Erstellen des Private Keys"
    exit 1
fi

# d) Certificate Signing Request (CSR) erstellen
echo "  → Erstelle Certificate Signing Request (server.csr)..."
openssl req -new -key "$CERT_DIR/server.key" \
    -out "$CERT_DIR/server.csr" \
    -config "$CONFIG_DIR/server.csr.cnf"

if [ $? -eq 0 ]; then
    echo -e "  ${GREEN}✓ CSR erstellt${NC}"
else
    echo "  ✗ Fehler beim Erstellen des CSR"
    exit 1
fi

# e) Server-Zertifikat mit Root-CA signieren
echo "  → Signiere Server-Zertifikat mit Root-CA (server.crt)..."
openssl x509 -req -in "$CERT_DIR/server.csr" \
    -CA "$CERT_DIR/rootCA.crt" \
    -CAkey "$CERT_DIR/rootCA.key" \
    -CAcreateserial \
    -passin pass:$PASSWORD \
    -out "$CERT_DIR/server.crt" \
    -days 825 -sha256 \
    -extfile "$CONFIG_DIR/v3.ext"

if [ $? -eq 0 ]; then
    echo -e "  ${GREEN}✓ Server-Zertifikat signiert${NC}"
else
    echo "  ✗ Fehler beim Signieren des Zertifikats"
    exit 1
fi

echo ""
echo -e "${GREEN}=== Zertifikate erfolgreich erstellt! ===${NC}\n"

# Informationen anzeigen
echo "Generierte Dateien in '$CERT_DIR/':"
echo "  - rootCA.key     (Root-CA Private Key, Passwort: $PASSWORD)"
echo "  - rootCA.crt     (Root-CA Zertifikat)"
echo "  - server.key     (Server Private Key)"
echo "  - server.csr     (Certificate Signing Request)"
echo "  - server.crt     (Signiertes Server-Zertifikat)"
echo ""

# Zertifikat-Details anzeigen
echo -e "${YELLOW}Root-CA Zertifikat Details:${NC}"
openssl x509 -in "$CERT_DIR/rootCA.crt" -noout -subject -issuer -dates
echo ""

echo -e "${YELLOW}Server-Zertifikat Details:${NC}"
openssl x509 -in "$CERT_DIR/server.crt" -noout -subject -issuer -dates
echo ""

# SAN (Subject Alternative Names) anzeigen
echo -e "${YELLOW}Subject Alternative Names (SANs):${NC}"
openssl x509 -in "$CERT_DIR/server.crt" -noout -ext subjectAltName
echo ""

echo "Das Root-Zertifikat (rootCA.crt) kann jetzt in den Browser importiert werden."
echo "Danach kann der WebServer mit 'docker-compose up -d' gestartet werden."
