FROM eclipse-temurin:17-jre-jammy

RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy Kepler jar, libraries and log config
COPY Retros-Portable/2007-v14/2007-v14/Server/www/kepler.jar /app/
COPY Retros-Portable/2007-v14/2007-v14/Server/www/lib/ /app/lib/
COPY Retros-Portable/2007-v14/2007-v14/Server/www/log4j.properties /app/

COPY docker/v14/emulator/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
