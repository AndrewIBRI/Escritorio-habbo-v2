#!/usr/bin/env bash
set -e

echo "[*] SwiftEmulator (2013+2014-r63b) starting..."

DB_HOST="${DB_HOST:-r63b-swift-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63b}"

GAME_PORT="${PORT_R63B_SWIFT_GAME:-${GAME_PORT:-30200}}"
MUS_PORT="${PORT_R63B_SWIFT_MUS:-${MUS_PORT:-30201}}"
HOST_IP="${HOST_IP:-127.0.0.1}"
WEB_PORT="${PORT_R63B_SWIFT_WEB:-${WEB_PORT:-8063}}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
while ! (timeout 1 bash -c "cat < /dev/null > /dev/tcp/${DB_HOST}/${DB_PORT}") 2>/dev/null && ! nc -z "${DB_HOST}" "${DB_PORT}" 2>/dev/null; do
    sleep 1
done
echo "[+] Database is reachable!"

mkdir -p /app/Settings

cat <<EOF > /app/Settings/configuration.ini
## uberEmulator System Configuration File
## Must be edited for the server to work

## MySQL Configuration
db.hostname=${DB_HOST}
db.port=${DB_PORT}
db.username=${DB_USER}
db.password=${DB_PASS}
db.name=${DB_NAME}

## MySQL pooling setup (controls amount of connections)
db.pool.minsize=1
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

## Maximum duckets
ducket.upper.limit=7000

## Stream Message
hotel.stream.message=N

## License
internal.license=

##Hotel Link
hotel.link=http://${HOST_IP}:${WEB_PORT}/

## Hotel Link; To Stream Images
guilds.link=http://${HOST_IP}:${WEB_PORT}/habbo-imaging/badge/
stream.link=http://${HOST_IP}:${WEB_PORT}/habbo-imaging/head/

## Client configuration
client.ping.enabled=1
client.ping.interval=200000
client.maxrequests=500
client.maxpossiblefriends=700
client.maxitemsininventary=0

#IRC
irc.enabled=false

allow.packetlog=false
group.enabled=true
auth.ssodisabled=false

#Says wether the users should receive pixels each X seconds, how much and when (When in seconds)
game.pixel.enabled=true
game.pixel.amount=500
game.pixel.time=400

#Says wether the users should receive pixels each X seconds, how much and when (When in seconds)
game.credits.enabled=true
game.credits.amount=1000
game.credits.time=400

game.login.credits.receiveamount=200
game.login.pixel.receiveamount=200

SeparatedTasksInMainLoops.enabled=true
SeparatedTasksInGameClientManager.enabled=true
spambans.enabled=true
spambans.limit=5
EOF

echo "[*] Generated /app/Settings/configuration.ini successfully."
echo "[*] Launching Butterfly Emulator (Swift) via Mono..."
exec mono "Butterfly Emulator.exe"
