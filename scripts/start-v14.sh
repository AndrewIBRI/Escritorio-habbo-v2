#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

echo "=============================================================================="
echo "                 Habbo 2007 v14 (Kepler Shockwave Retro) Launcher"
echo "=============================================================================="
echo ""

cd "${ROOT_DIR}"

if [ ! -f .env ]; then
    echo "[*] Creating .env from .env.example..."
    cp .env.example .env
fi

echo "[*] Starting Habbo 2007 (v14) via Docker Compose..."
docker compose --profile v14 up -d

echo ""
echo "[+] Retro 2007 v14 is online!"
echo "    Web / Client URL: http://localhost:8014"
echo "    Emulator Game Port: 12321"
echo "    To view logs: docker compose logs -f v14-emulator"
