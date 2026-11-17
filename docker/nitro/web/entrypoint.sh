#!/bin/bash
set -e

DB_HOST="${DB_HOST:-nitro-db}"
DB_PORT="${DB_PORT:-3306}"

echo "[*] FlowHotel Nitro Web starting..."
echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."

while ! nc -z "$DB_HOST" "$DB_PORT"; do
  sleep 1
done

echo "[+] Database connected!"

# Configure client json files with domain and websocket URL
php /app/configure_client.php || true

# Fix permissions
chown -R www-data:www-data /var/www/html

echo "[*] Starting Apache..."
exec apache2 -D FOREGROUND
