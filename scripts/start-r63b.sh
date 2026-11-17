#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

echo "=============================================================================="
echo "           Habbo 2012 r63b (Atom / ButterStorm) Launcher"
echo "=============================================================================="
echo ""

cd "${ROOT_DIR}"

if [ ! -f .env ]; then
    echo "[*] Creating .env from .env.example..."
    cp .env.example .env
fi

echo "[*] Starting Habbo 2012 (r63b) via Docker Compose..."
docker compose --profile r63b up -d

echo ""
echo "[+] Retro 2012 r63b is online!"
echo "    Web / Client URL: http://localhost:8062"
echo "    Emulator Game Port: 30100"
echo "    MUS Port: 30101"
echo "    To view logs: docker compose logs -f r63b-emulator"
