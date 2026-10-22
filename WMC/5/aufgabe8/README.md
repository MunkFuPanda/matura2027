# Aufgabe 8 - Docker Compose (Flask + Redis)

Vorlage: https://docs.docker.com/compose/gettingstarted/

Ordner: `app.py`, `requirements.txt`, `Dockerfile`, `compose.8a|8b|8c.yaml`
(`compose.yaml` = Endstand 8c, wird ohne `-f` automatisch verwendet).

## 8a - Internes Netzwerk + DNS
    docker compose -f compose.8a.yaml up --build
    curl localhost:8000        # Zaehler steigt bei jedem Aufruf
    docker compose -f compose.8a.yaml down

## 8b - depends_on
    docker compose -f compose.8b.yaml up --build
    # Redis wird zuerst "healthy" (redis-cli ping), erst dann startet web
    docker compose -f compose.8b.yaml down

## 8c - Volumes
    docker compose up --build -d          # = compose.8c.yaml
    curl localhost:8000; curl localhost:8000
    docker compose down                    # Container weg, Volume bleibt
    docker compose up -d; curl localhost:8000   # Zaehler zaehlt weiter
    docker volume ls                       # redis-data (Projektname-Praefix)
    docker compose down -v                 # loescht auch das Volume

## Nuetzlich
    docker compose ps
    docker compose logs -f
    docker compose exec web ping redis     # DNS-Name = Servicename
