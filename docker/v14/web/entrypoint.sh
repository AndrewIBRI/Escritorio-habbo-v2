#!/usr/bin/env bash
set -e

echo "[*] Kepler (v14) Web server starting on port 80..."
exec apache2-foreground
