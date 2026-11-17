FROM mono:6.8-slim

# Optional networking tool
RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd || true && rm -rf /var/lib/apt/lists/* || true

WORKDIR /app

# Copy SwiftEmulator Release binaries and libraries (using wildcard for brackets in folder name)
COPY Retros-Portable/2013+2014-r63b/2013+2014-r63b/Emulator(SwiftEmulator)*/bin/Release/ /app/

COPY docker/r63b-swift/emulator/entrypoint.sh /app/entrypoint.sh
RUN sed -i 's/\r$//' /app/entrypoint.sh && chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
