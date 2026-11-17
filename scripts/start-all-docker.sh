#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

echo "=============================================================================="
echo "          Habbo 6-in-1 Complete Retro Suite (All Profiles)"
echo "=============================================================================="
echo ""

cd "${ROOT_DIR}"

if [ ! -f .env ]; then
    echo "[*] Creating .env from .env.example..."
    cp .env.example .env
fi

echo "[*] Starting all 6 retros via Docker Compose profiles..."
docker compose --profile all up -d

echo ""
echo "=============================================================================="
echo "[+] ALL RETROS STARTED SUCCESSFULLY!"
echo "=============================================================================="
echo "  - 2007 v14 (Kepler):          http://localhost:8014  (Game TCP: 12321)"
echo "  - 2009 v31 (Havana):          http://localhost:8031  (Game TCP: 12331)"
echo "  - 2011 r63a (Gold Tree):      http://localhost:8061  (Game TCP: 30000)"
echo "  - 2012 r63b (Atom/Butter):    http://localhost:8062  (Game TCP: 30100)"
echo "  - 2013-14 r63b (Swift):       http://localhost:8063  (Game TCP: 30200)"
echo "  - 2015-17 r63c (PlusEMU):     http://localhost:8064  (Game TCP: 30300)"
echo "=============================================================================="
