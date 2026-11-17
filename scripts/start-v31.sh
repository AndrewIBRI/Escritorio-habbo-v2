#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

echo "=============================================================================="
echo "                 Habbo 2009 v31 (Havana Flash/Shockwave) Launcher"
echo "=============================================================================="
echo ""

cd "${ROOT_DIR}"

if [ ! -f .env ]; then
    echo "[*] Creating .env from .env.example..."
    cp .env.example .env
fi

echo "[*] Starting Habbo 2009 (v31) via Docker Compose..."
docker compose --profile v31 up -d

echo ""
echo "[+] Retro 2009 v31 is online!"
echo "    Web / Client URL: http://localhost:8031"
echo "    Emulator Game Port: 12331"
echo "    To view logs: docker compose logs -f v31-emulator"
