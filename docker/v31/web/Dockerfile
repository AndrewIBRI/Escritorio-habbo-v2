FROM eclipse-temurin:17-jre-jammy

RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd default-mysql-client && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy Havana Web server jar, figuredata, configs and assets
COPY Retros-Portable/2009-v31/2009-v31/Havana/Havana-Web.jar /app/
COPY Retros-Portable/2009-v31/2009-v31/Havana/figuredata.xml /app/
COPY Retros-Portable/2009-v31/2009-v31/Havana/log4j.web.properties /app/
COPY Retros-Portable/2009-v31/2009-v31/Havana/tools /app/tools

COPY docker/v31/web/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

EXPOSE 80
ENTRYPOINT ["/app/entrypoint.sh"]
