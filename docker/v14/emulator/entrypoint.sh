#!/usr/bin/env bash
set -e

echo "[*] Kepler (v14) Emulator starting..."

DB_HOST="${DB_HOST:-v14-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-v14}"

BIND_IP="${BIND_IP:-0.0.0.0}"
GAME_PORT="${PORT_V14_GAME:-${GAME_PORT:-12321}}"
MUS_BIND="${MUS_BIND:-0.0.0.0}"
MUS_PORT="${PORT_V14_MUS:-${MUS_PORT:-12322}}"
RCON_BIND="${RCON_BIND:-0.0.0.0}"
RCON_PORT="${PORT_V14_RCON:-${RCON_PORT:-12309}}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
while ! nc -z "${DB_HOST}" "${DB_PORT}"; do
  sleep 1
done
echo "[+] Database is ready!"

# Generate server.ini dynamically
cat <<EOF > /app/server.ini
[Server]
server.bind=${BIND_IP}
server.port=${GAME_PORT}

[Rcon]
rcon.bind=${RCON_BIND}
rcon.port=${RCON_PORT}

[Mus]
mus.bind=${MUS_BIND}
mus.port=${MUS_PORT}

[Database]
mysql.hostname=${DB_HOST}
mysql.port=${DB_PORT}
mysql.user=${DB_USER}
mysql.password=${DB_PASS}
mysql.database=${DB_NAME}

[Logging]
log.received.packets=false
log.sent.packets=false

[Console]
debug=false
EOF

echo "[*] Configuration generated:"
cat /app/server.ini
echo "[*] Starting Kepler..."
exec java -classpath "kepler.jar:lib/*" org.alexdev.kepler.Kepler
