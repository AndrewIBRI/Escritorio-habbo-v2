#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

echo "=============================================================================="
echo "                 Habbo 2011 r63a (Gold Tree / Phoenix) Launcher"
echo "=============================================================================="
echo ""

cd "${ROOT_DIR}"

if [ ! -f .env ]; then
    echo "[*] Creating .env from .env.example..."
    cp .env.example .env
fi

echo "[*] Starting Habbo 2011 (r63a) via Docker Compose..."
docker compose --profile r63a up -d

echo ""
echo "[+] Retro 2011 r63a is online!"
echo "    Web / Client URL: http://localhost:8061"
echo "    Emulator Game Port: 30000"
echo "    MUS Port: 30001"
echo "    To view logs: docker compose logs -f r63a-emulator"
