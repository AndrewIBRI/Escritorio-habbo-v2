#!/usr/bin/env bash
set -e

echo "[*] Gold Tree (2011-r63a) Web CMS starting..."

DB_HOST="${DB_HOST:-r63a-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63a}"

HOST_IP="${HOST_IP:-127.0.0.1}"
WEB_PORT="${PORT_R63A_WEB:-${WEB_PORT:-8061}}"
GAME_PORT="${PORT_R63A_GAME:-${GAME_PORT:-30000}}"
MUS_PORT="${PORT_R63A_MUS:-${MUS_PORT:-30001}}"

echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."
php -r "
\$host = '${DB_HOST}';
\$port = (int)'${DB_PORT}';
for (\$i = 0; \$i < 60; \$i++) {
    \$fp = @fsockopen(\$host, \$port, \$errno, \$errstr, 1);
    if (\$fp) {
        fclose(\$fp);
        echo \"[+] Database connected!\n\";
        exit(0);
    }
    sleep(1);
}
echo \"[-] Timed out waiting for Database.\n\";
exit(1);
"

# Easypanel & HTTPS Support
if [ -n "${SITE_URL}" ]; then
  HOTEL_URL="${SITE_URL}"
elif [ -n "${PRIMARY_DOMAIN}" ]; then
  HOTEL_URL="https://${PRIMARY_DOMAIN}"
elif [ "${WEB_PORT}" = "80" ] || [ "${WEB_PORT}" = "443" ]; then
  HOTEL_URL="http://${HOST_IP}"
else
  HOTEL_URL="http://${HOST_IP}:${WEB_PORT}"
fi

CLIENT_HOST="${CLIENT_HOST:-${HOST_IP:-${PRIMARY_DOMAIN}}}"
CLIENT_HOST="${CLIENT_HOST#http://}"
CLIENT_HOST="${CLIENT_HOST#https://}"
CLIENT_HOST="${CLIENT_HOST%%/*}"
CLIENT_HOST="${CLIENT_HOST%%:*}"

# Generate CMS database config
CONFIG_FILE="/var/www/html/data_classes/server-data.php_data_classes-config.php.php"
cat <<EOF > "${CONFIG_FILE}"
<?php
\$Hotelname = "Habbo";
\$MySQLhostname = "${DB_HOST}";
\$MySQLusername = "${DB_USER}";
\$MySQLpassword = "${DB_PASS}";
\$MySQLdb = "${DB_NAME}";
\$hotel_url = "${HOTEL_URL}";
\$avatar = "http://www.habbo.fr/habbo-imaging/avatarimage?figure=";
\$cimages_url = "${HOTEL_URL}/images/c_images/";
?>
EOF

# Update cms_settings in MySQL database
php -r "
\$link = @mysql_connect('${DB_HOST}:${DB_PORT}', '${DB_USER}', '${DB_PASS}');
if (\$link && mysql_select_db('${DB_NAME}', \$link)) {
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('client_ip', '${CLIENT_HOST}')\", \$link);
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('client_port', '${GAME_PORT}')\", \$link);
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('client_mus', '${MUS_PORT}')\", \$link);
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('cms_url', '${HOTEL_URL}')\", \$link);
    echo \"[+] CMS database settings updated.\n\";
    mysql_close(\$link);
}
"

# Dynamic host injection in client.php and vari.php
sed -i "s|connection.info.host\" : \"localhost\"|connection.info.host\" : \"${CLIENT_HOST}\"|g" /var/www/html/client.php || true
sed -i "s|connection.info.port\" : \"30000\"|connection.info.port\" : \"${GAME_PORT}\"|g" /var/www/html/client.php || true
sed -i "s|http://localhost/|${HOTEL_URL}/|g" /var/www/html/client.php || true
sed -i "s|http://localhost/|${HOTEL_URL}/|g" /var/www/html/phx/vari.php || true

echo "[*] Starting Apache..."
exec apache2-foreground
