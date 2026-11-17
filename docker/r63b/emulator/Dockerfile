FROM mono:6.8-slim

# Optional networking tool
RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd || true && rm -rf /var/lib/apt/lists/* || true

WORKDIR /app

# Copy AtomEmulator Debug binaries and libraries
COPY Retros-Portable/2012-r63b/2012-r63b/Emulator(AtomEmulator)/bin/Debug/ /app/atom/
COPY Retros-Portable/2012-r63b/2012-r63b/Emulator(ButterStorm)/Butterfly\ Emulator/bin/Debug/ /app/butterstorm/

COPY docker/r63b/emulator/entrypoint.sh /app/entrypoint.sh
RUN sed -i 's/\r$//' /app/entrypoint.sh && chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
