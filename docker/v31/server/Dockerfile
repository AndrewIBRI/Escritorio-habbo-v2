FROM eclipse-temurin:17-jre-jammy

RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy Havana game server jar, configs, and assets
COPY Retros-Portable/2009-v31/2009-v31/Havana/Havana-Server.jar /app/
COPY Retros-Portable/2009-v31/2009-v31/Havana/figuredata.xml /app/
COPY Retros-Portable/2009-v31/2009-v31/Havana/log4j.properties /app/

COPY docker/v31/server/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
