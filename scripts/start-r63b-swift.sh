#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

echo "=============================================================================="
echo "           Habbo 2013+2014 r63b (SwiftEmulator) Launcher"
echo "=============================================================================="
echo ""

cd "${ROOT_DIR}"

if [ ! -f .env ]; then
    echo "[*] Creating .env from .env.example..."
    cp .env.example .env
fi

echo "[*] Starting Habbo 2013+2014 (r63b-swift) via Docker Compose..."
docker compose --profile r63b-swift up -d

echo ""
echo "[+] Retro 2013+2014 r63b-swift is online!"
echo "    Web / Client URL: http://localhost:8063"
echo "    Emulator Game Port: 30200"
echo "    MUS Port: 30201"
echo "    To view logs: docker compose logs -f r63b-swift-emulator"
