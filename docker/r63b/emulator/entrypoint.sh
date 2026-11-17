#!/usr/bin/env bash
set -e

EMU_TYPE="${EMU_TYPE:-atom}"
echo "[*] 2012-r63b Emulator (${EMU_TYPE}) starting..."

DB_HOST="${DB_HOST:-r63b-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63b}"

GAME_PORT="${PORT_R63B_GAME:-${GAME_PORT:-30100}}"
MUS_PORT="${PORT_R63B_MUS:-${MUS_PORT:-30101}}"
HOST_IP="${HOST_IP:-127.0.0.1}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
while ! (timeout 1 bash -c "cat < /dev/null > /dev/tcp/${DB_HOST}/${DB_PORT}") 2>/dev/null && ! nc -z "${DB_HOST}" "${DB_PORT}" 2>/dev/null; do
    sleep 1
done
echo "[+] Database is reachable!"

if [ "${EMU_TYPE}" = "butterfly" ]; then
    WORK_DIR="/app/butterstorm"
    EXE_NAME="Butterfly Emulator.exe"
else
    WORK_DIR="/app/atom"
    EXE_NAME="Atom Emulator.exe"
fi

cd "${WORK_DIR}"
mkdir -p Settings

cat <<EOF > Settings/configuration.ini
## uberEmulator System Configuration File
## Must be edited for the server to work

## MySQL Configuration
db.hostname=${DB_HOST}
db.port=${DB_PORT}
db.username=${DB_USER}
db.password=${DB_PASS}
db.name=${DB_NAME}

## MySQL pooling setup (controls amount of connections)
db.pool.minsize=5
db.pool.maxsize=100

## Game TCP/IP Configuration
game.tcp.bindip=0.0.0.0
game.tcp.port=${GAME_PORT}
game.tcp.conlimit=11000
game.tcp.conperip=100
game.tcp.enablenagles=False

## MUS TCP/IP Configuration
mus.tcp.bindip=0.0.0.0
mus.tcp.port=${MUS_PORT}
mus.tcp.allowedaddr=0.0.0.0/0;127.0.0.1;${DB_HOST}

## Client configuration
client.ping.enabled=1
client.ping.interval=20000
client.maxrequests=300

#IRC
irc.enabled=false

group.enabled=true
auth.ssodisabled=false

game.pixel.enabled=true
game.pixel.amount=100
game.pixel.time=600

game.credits.enabled=true
game.credits.amount=1000
game.credits.time=600
game.chatlog=1

game.login.credits.receiveamount=100
game.login.pixel.receiveamount=100

SeparatedTasksInMainLoops.enabled=true
SeparatedTasksInGameClientManager.enabled=false
spambans.enabled=false
spambans.limit=20
EOF

echo "[*] Generated ${WORK_DIR}/Settings/configuration.ini successfully."
echo "[*] Launching ${EXE_NAME} via Mono..."
exec mono "${EXE_NAME}"
