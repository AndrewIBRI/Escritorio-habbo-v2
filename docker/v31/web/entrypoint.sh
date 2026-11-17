#!/usr/bin/env bash
set -e

echo "[*] Havana (v31) Web starting..."

HOST_IP="${HOST_IP:-127.0.0.1}"
PORT_V31_WEB="${PORT_V31_WEB:-8031}"
PORT_V31_GAME="${PORT_V31_GAME:-12321}"
PORT_V31_MUS="${PORT_V31_MUS:-12322}"
PORT_V31_RCON="${PORT_V31_RCON:-12309}"

DB_HOST="${DB_HOST:-v31-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-v31}"

# Easypanel & HTTPS Support
if [ -n "${SITE_URL}" ]; then
  :
elif [ -n "${PRIMARY_DOMAIN}" ]; then
  SITE_URL="https://${PRIMARY_DOMAIN}"
elif [ "${PORT_V31_WEB}" = "80" ] || [ "${PORT_V31_WEB}" = "443" ]; then
  SITE_URL="http://${HOST_IP}"
else
  SITE_URL="http://${HOST_IP}:${PORT_V31_WEB}"
fi

CLIENT_HOST="${CLIENT_HOST:-${HOST_IP:-${PRIMARY_DOMAIN}}}"
CLIENT_HOST="${CLIENT_HOST#http://}"
CLIENT_HOST="${CLIENT_HOST#https://}"
CLIENT_HOST="${CLIENT_HOST%%/*}"
CLIENT_HOST="${CLIENT_HOST%%:*}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
while ! nc -z "${DB_HOST}" "${DB_PORT}"; do
  sleep 1
done
echo "[+] Database is ready!"

# Generate webserver-config.ini
cat <<EOF > /app/webserver-config.ini
[Site]
site.directory=tools/www

[Global]
bind.ip=0.0.0.0
bind.port=80

[Rcon]
rcon.ip=127.0.0.1
rcon.port=${PORT_V31_RCON}

[Database]
mysql.hostname=${DB_HOST}
mysql.port=${DB_PORT}
mysql.username=${DB_USER}
mysql.password=${DB_PASS}
mysql.database=${DB_NAME}

[Template]
template.directory=tools/www-tpl
template.name=default-en

page.encoding=utf-8
EOF

echo "[*] Configuration generated:"
cat /app/webserver-config.ini

# Seed / update dynamic settings in MySQL settings table
echo "[*] Updating Havana settings table with dynamic host (${HOST_IP}) and ports..."
mysql -h"${DB_HOST}" -P"${DB_PORT}" -u"${DB_USER}" -p"${DB_PASS}" "${DB_NAME}" <<EOSQL || true
INSERT INTO settings (setting, value) VALUES
  ('site.name', 'Habbo'),
  ('site.path', '${SITE_URL}'),
  ('static.content.path', '${SITE_URL}'),
  ('loader.game.ip', '${CLIENT_HOST}'),
  ('loader.game.port', '${PORT_V31_GAME}'),
  ('loader.mus.ip', '${CLIENT_HOST}'),
  ('loader.mus.port', '${PORT_V31_MUS}'),
  ('loader.dcr', '${SITE_URL}/dcr/v31/habbo.dcr?'),
  ('loader.external.variables', '${SITE_URL}/dcr/v31/gamedata/external_variables.txt?'),
  ('loader.external.texts', '${SITE_URL}/dcr/v31/gamedata/external_texts.txt?'),
  ('loader.flash.base', '${SITE_URL}/gordon/RELEASE39-22643-22891-200911110035_07c3a2a30713fd5bea8a8caf07e33438/'),
  ('loader.flash.swf', '${SITE_URL}/gordon/RELEASE39-22643-22891-200911110035_07c3a2a30713fd5bea8a8caf07e33438/Habbo.swf'),
  ('loader.flash.external.texts', '${SITE_URL}/flash/gamedata/external_flash_texts.txt'),
  ('loader.flash.external.variables', '${SITE_URL}/flash/gamedata/external_variables.txt')
ON DUPLICATE KEY UPDATE value=VALUES(value);
EOSQL

# Dynamic replacement in static external_variables text files
if [ -f "/app/tools/www/flash/gamedata/external_variables.txt" ]; then
  if [ ! -f "/app/tools/www/flash/gamedata/external_variables.txt.orig" ]; then
    cp /app/tools/www/flash/gamedata/external_variables.txt /app/tools/www/flash/gamedata/external_variables.txt.orig
  fi
  sed "s|http://localhost|${SITE_URL}|g" /app/tools/www/flash/gamedata/external_variables.txt.orig > /app/tools/www/flash/gamedata/external_variables.txt
fi

if [ -f "/app/tools/www/dcr/v31/gamedata/external_variables.txt" ]; then
  if [ ! -f "/app/tools/www/dcr/v31/gamedata/external_variables.txt.orig" ]; then
    cp /app/tools/www/dcr/v31/gamedata/external_variables.txt /app/tools/www/dcr/v31/gamedata/external_variables.txt.orig
  fi
  sed "s|http://localhost|${SITE_URL}|g" /app/tools/www/dcr/v31/gamedata/external_variables.txt.orig > /app/tools/www/dcr/v31/gamedata/external_variables.txt
fi

echo "[*] Starting Havana-Web on port 80..."
exec java -Djava.net.preferIPv4Stack=true -Djava.net.preferIPv4Addresses=true -jar Havana-Web.jar
