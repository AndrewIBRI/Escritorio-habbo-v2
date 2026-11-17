#!/usr/bin/env bash
set -e

echo "[*] Gold Tree Emulator (2011-r63a) starting..."

DB_HOST="${DB_HOST:-r63a-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63a}"

GAME_PORT="${PORT_R63A_GAME:-${GAME_PORT:-30000}}"
MUS_PORT="${PORT_R63A_MUS:-${MUS_PORT:-30001}}"
HOST_IP="${HOST_IP:-127.0.0.1}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
while ! (timeout 1 bash -c "cat < /dev/null > /dev/tcp/${DB_HOST}/${DB_PORT}") 2>/dev/null && ! nc -z "${DB_HOST}" "${DB_PORT}" 2>/dev/null; do
    sleep 1
done
echo "[+] Database is reachable!"

cat <<EOF > /app/config.conf
## Gold Tree Emulator 3.0 System Configuration File

## MySQL Configuration
db.hostname=${DB_HOST}
db.port=${DB_PORT}
db.username=${DB_USER}
db.password=${DB_PASS}
db.name=${DB_NAME}

## MySQL pooling setup (controls amount of connections)
db.pool.minsize=5
db.pool.maxsize=55

## Game TCP/IP Configuration
game.tcp.bindip=0.0.0.0
game.tcp.port=${GAME_PORT}
game.tcp.conlimit=100
game.tcp.proxyip=${HOST_IP}

## Client configuration
client.ping.enabled=1
client.ping.interval=30000

## MUS TCP/IP Configuration
mus.tcp.bindip=0.0.0.0
mus.tcp.port=${MUS_PORT}
mus.tcp.allowedaddr=0.0.0.0/0;127.0.0.1;${DB_HOST}

## Console Configuration
emu.messages.connections=1
emu.messages.roommgr=1

## Automatic shutdown
shutdown-server=
shutdown-server-player-limit=0
shutdown-warning-alert=!!!Hotel shutdown!!!

## Automatic update
gte.update.noticy.disable=1
gte.update.autoupdate=0

## Anti Advertising Configuration 
anti.ads.enable=0
anti.ads.rank=1
anti.ads.msg=Advertising will lead you to be banned
ads.disable=1
ads.allowrdonlyrandom
ads=1
debug=0
EOF

echo "[*] Generated config.conf successfully."
echo "[*] Launching Gold Tree Emulator via Mono..."
exec mono "Gold Tree Emulator 3.0.exe"
