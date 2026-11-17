# Stage 1: Build Arcturus Morningstar
FROM maven:3.9-eclipse-temurin-17-alpine AS builder
WORKDIR /build

# Copy source code and pom
COPY docker/nitro/emulator/source/pom.xml .
COPY docker/nitro/emulator/source/src ./src

# Build the fat executable JAR
RUN mvn package -DskipTests -B && \
    (cp target/Habbo-*-jar-with-dependencies.jar target/Habbo.jar || cp target/*.jar target/Habbo.jar)

# Stage 2: Minimal Production Runtime
FROM eclipse-temurin:17-jre-alpine AS runtime

RUN apk add --no-cache bash netcat-openbsd curl

WORKDIR /app

# Copy compiled JAR
COPY --from=builder /build/target/Habbo.jar /app/Habbo.jar

# Copy configuration template and plugins
COPY docker/nitro/emulator/config.ini.template /app/config.ini.template
COPY docker/nitro/emulator/plugins/ /app/prebuilt-plugins/

# Copy entrypoint script
COPY docker/nitro/emulator/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

EXPOSE 3000 3001 2096

ENTRYPOINT ["/bin/bash", "/app/entrypoint.sh"]
