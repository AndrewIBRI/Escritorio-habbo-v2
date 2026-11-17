FROM mono:6.8-slim

# Optional networking tool
RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd || true && rm -rf /var/lib/apt/lists/* || true

WORKDIR /app

# Copy Gold Tree Emulator Debug binaries and libraries
COPY Retros-Portable/2011-r63a/2011-r63a/Emulator/Gold\ Tree\ Emulator\ 3.0/bin/Debug/ /app/

COPY docker/r63a/emulator/entrypoint.sh /app/entrypoint.sh
RUN sed -i 's/\r$//' /app/entrypoint.sh && chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
