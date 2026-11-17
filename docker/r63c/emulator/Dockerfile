FROM mono:6.8

# Optional networking tool
RUN apt-get update && apt-get install -y --no-install-recommends netcat-openbsd || true && rm -rf /var/lib/apt/lists/* || true

WORKDIR /app

# Copy PlusEMU source and precompiled binaries
COPY Retros-Portable/2015+2016+2017-r63c/2015+2016+2017-r63c/Emulator(PlusEMU)/ /app/

# Build project with msbuild to include Linux-safe Program.cs patch
RUN msbuild /p:Configuration=Debug /p:Platform="Any CPU" "Plus Emulator.csproj" || true

WORKDIR /app/bin/Debug

COPY docker/r63c/emulator/entrypoint.sh /app/bin/Debug/entrypoint.sh
RUN sed -i 's/\r$//' /app/bin/Debug/entrypoint.sh && chmod +x /app/bin/Debug/entrypoint.sh

ENTRYPOINT ["/app/bin/Debug/entrypoint.sh"]
