#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

echo "=============================================================================="
echo "                 Habbo 2015-2017 r63c (PlusEMU) Launcher"
echo "=============================================================================="
echo ""

cd "${ROOT_DIR}"

if [ ! -f .env ]; then
    echo "[*] Creating .env from .env.example..."
    cp .env.example .env
fi

echo "[*] Starting Habbo 2015-2017 (r63c) via Docker Compose..."
docker compose --profile r63c up -d

echo ""
echo "[+] Retro 2015-2017 r63c is online!"
echo "    Web / Client URL: http://localhost:8064"
echo "    Emulator Game Port: 30300"
echo "    MUS / RCON Port: 30301"
echo "    To view logs: docker compose logs -f r63c-emulator"
