.PHONY: help certs up down logs clean inspect-root inspect-server test

help: ## Zeige diese Hilfe an
	@echo "Verfügbare Befehle:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-20s\033[0m %s\n", $$1, $$2}'

certs: ## Erstelle SSL-Zertifikate
	@echo "Erstelle SSL-Zertifikate..."
	@chmod +x scripts/generate-certificates.sh
	@docker-compose run --rm cert-generator
	@echo "Zertifikate erstellt!"

up: ## Starte WebServer (erstellt Zertifikate falls nötig)
	@if [ ! -f certs/server.crt ]; then \
		echo "Zertifikate nicht gefunden. Erstelle zuerst Zertifikate..."; \
		$(MAKE) certs; \
	fi
	@echo "Starte WebServer..."
	@docker-compose up -d webserver
	@echo ""
	@echo "✓ WebServer läuft!"
	@echo ""
	@echo "  HTTPS: https://localhost"
	@echo "  HTTP:  http://localhost (wird zu HTTPS weitergeleitet)"
	@echo ""
	@echo "Importiere certs/rootCA.crt in deinen Browser, um SSL-Warnungen zu vermeiden."

down: ## Stoppe WebServer
	@docker-compose down

logs: ## Zeige WebServer Logs
	@docker-compose logs -f webserver

clean: ## Lösche generierte Zertifikate
	@chmod +x scripts/clean.sh
	@./scripts/clean.sh

inspect-root: ## Zeige Root-CA Zertifikat Details
	@openssl x509 -in certs/rootCA.crt -text -noout

inspect-server: ## Zeige Server-Zertifikat Details
	@openssl x509 -in certs/server.crt -text -noout

test: ## Teste HTTPS-Verbindung
	@echo "Teste HTTPS-Verbindung zu localhost..."
	@curl -k -I https://localhost || echo "Fehler: WebServer läuft nicht. Starte mit 'make up'"

info: ## Zeige Projekt-Informationen
	@echo "=== SSL Certificate Project Info ==="
	@echo ""
	@echo "Dateien:"
	@ls -lh certs/ 2>/dev/null || echo "  Keine Zertifikate gefunden (erstelle mit 'make certs')"
	@echo ""
	@echo "Container Status:"
	@docker-compose ps
