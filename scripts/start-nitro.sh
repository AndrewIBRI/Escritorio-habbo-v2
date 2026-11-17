#!/bin/bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "${SCRIPT_DIR}/.." && pwd)"

cd "${ROOT_DIR}"

echo "=============================================================================="
echo "         Habbo HTML5 Nitro + Arcturus Morningstar (No Flash)"
echo "=============================================================================="
echo ""
echo "Select execution mode:"
echo "  [1] Start Nitro HTML5 stack (docker-compose.nitro.yml)"
echo "  [2] View Emulator Logs"
echo "  [3] View Web Logs"
echo "  [4] Stop Nitro HTML5 stack"
echo ""

read -rp "Enter choice [1-4] (Default: 1): " MODE
MODE="${MODE:-1}"

case "$MODE" in
  1)
    echo "[*] Building and starting Nitro HTML5 stack..."
    docker compose -f docker-compose.nitro.yml up -d --build
    echo ""
    echo "[+] Nitro HTML5 Hotel is online!"
    echo "    Web Portal / Client URL: http://localhost:8064"
    echo "    WebSocket Port: 2096 (proxied via /ws)"
    echo "    Emulator Game Port: 3000"
    ;;
  2)
    docker compose -f docker-compose.nitro.yml logs -f nitro-emulator
    ;;
  3)
    docker compose -f docker-compose.nitro.yml logs -f nitro-web
    ;;
  4)
    docker compose -f docker-compose.nitro.yml down
    ;;
  *)
    echo "[-] Invalid option."
    exit 1
    ;;
esac
