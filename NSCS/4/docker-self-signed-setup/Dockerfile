FROM alpine:latest AS cert-builder

RUN apk add --no-cache openssl bash

WORKDIR /certs

COPY config/ /config/

COPY scripts/generate-certificates.sh /generate-certificates.sh
RUN chmod +x /generate-certificates.sh

RUN /generate-certificates.sh


# Stage 2: WebServer mit Nginx
FROM nginx:alpine

COPY nginx.conf /etc/nginx/nginx.conf

COPY --from=cert-builder /certs/*.crt /etc/nginx/certs/
COPY --from=cert-builder /certs/*.key /etc/nginx/certs/

EXPOSE 443 80

# Nginx starten
CMD ["nginx", "-g", "daemon off;"]
