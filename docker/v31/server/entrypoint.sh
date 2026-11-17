#!/usr/bin/env bash
set -e

echo "[*] Havana (v31) Server starting..."

DB_HOST="${DB_HOST:-v31-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-v31}"

GAME_PORT="${PORT_V31_GAME:-${GAME_PORT:-12321}}"
MUS_PORT="${PORT_V31_MUS:-${MUS_PORT:-12322}}"
RCON_PORT="${PORT_V31_RCON:-${RCON_PORT:-12309}}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
while ! nc -z "${DB_HOST}" "${DB_PORT}"; do
  sleep 1
done
echo "[+] Database is ready!"

# Generate server.ini
cat <<EOF > /app/server.ini
[Global]
server.bind=0.0.0.0
server.port=${GAME_PORT}

[Server]
server.port=${GAME_PORT}
server.limit.bandwidth=false
server.limit.bandwidth.amount=40960

[Rcon]
rcon.bind=0.0.0.0
rcon.port=${RCON_PORT}

[Mus]
mus.bind=0.0.0.0
mus.port=${MUS_PORT}

[Database]
mysql.hostname=${DB_HOST}
mysql.port=${DB_PORT}
mysql.username=${DB_USER}
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
echo "[*] Starting Havana-Server..."
exec java -Djava.net.preferIPv4Stack=true -Djava.net.preferIPv4Addresses=true -jar Havana-Server.jar
