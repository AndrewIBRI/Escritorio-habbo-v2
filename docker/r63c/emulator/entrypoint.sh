#!/usr/bin/env bash
set -e

echo "[*] PlusEMU (2015+2016+2017-r63c) starting..."

DB_HOST="${DB_HOST:-r63c-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63c}"

GAME_PORT="${PORT_R63C_GAME:-${GAME_PORT:-30300}}"
MUS_PORT="${PORT_R63C_MUS:-${MUS_PORT:-30301}}"
HOST_IP="${HOST_IP:-127.0.0.1}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
while ! (timeout 1 bash -c "cat < /dev/null > /dev/tcp/${DB_HOST}/${DB_PORT}") 2>/dev/null && ! nc -z "${DB_HOST}" "${DB_PORT}" 2>/dev/null; do
    sleep 1
done
echo "[+] Database is reachable!"

cat <<EOF > /app/bin/Debug/config.ini
## PlusEMU System Configuration File
## Must be edited for the server to work

## MySQL Configuration
db.hostname=${DB_HOST}
db.port=${DB_PORT}
db.username=${DB_USER}
db.password=${DB_PASS}
db.name=${DB_NAME}

## MySQL pooling setup (controls amount of connections)
db.pool.minsize=10
db.pool.maxsize=250

## Game TCP/IP Configuration
game.tcp.bindip=0.0.0.0
game.tcp.port=${GAME_PORT}
game.tcp.conlimit=100000
game.tcp.conperip=5
game.tcp.enablenagles=true

## RCON TCP/IP Configuration
rcon.tcp.bindip=0.0.0.0
rcon.tcp.port=${MUS_PORT}
rcon.tcp.allowedaddr=localhost;127.0.0.1;${DB_HOST}

## Client configuration
client.ping.enabled=1
client.ping.interval=20000
client.maxrequests=300
EOF

echo "[*] Generated config.ini successfully."
echo "[*] Launching Plus Emulator via Mono..."
exec mono "Plus Emulator.exe"
