#!/usr/bin/env bash
set -e

echo "[*] ProjectHabbo CMS (2013+2014-r63b Swift) Web starting..."

DB_HOST="${DB_HOST:-r63b-swift-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63b}"

HOST_IP="${HOST_IP:-127.0.0.1}"
WEB_PORT="${PORT_R63B_SWIFT_WEB:-${WEB_PORT:-8063}}"
GAME_PORT="${PORT_R63B_SWIFT_GAME:-${GAME_PORT:-30200}}"
MUS_PORT="${PORT_R63B_SWIFT_MUS:-${MUS_PORT:-30201}}"

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

# Generate CMS database config
CONFIG_FILE="/var/www/html/data_classes/server-data.php_data_classes-config.php.php"
cat <<EOF > "${CONFIG_FILE}"
<?php
\$Hotelname = "Habbo";
\$hotel_url = "http://${HOST_IP}:${WEB_PORT}";
\$MySQLhostname = "${DB_HOST}"; 
\$MySQLusername = "${DB_USER}";
\$MySQLpassword = "${DB_PASS}";
\$MySQLdb = "${DB_NAME}";
\$Facebook_id = "ProjectHabbo";
\$Twitter_id = "ProjectHabbo";
\$Email_admin = "contato@projecthabbo.com.br";
\$avatar = "http://www.habbo.fr/habbo-imaging/avatarimage?figure=";
\$cimages_url = "http://${HOST_IP}:${WEB_PORT}/swfs/c_images";
\$server_ip = "${HOST_IP}";
\$server_port = "${GAME_PORT}";
\$external_var = "http://${HOST_IP}:${WEB_PORT}/swfs/gamedata/external_variables/22e1a7119e60eb26d87e3d3000592bd389fc2d3d.txt";
\$external_text = "http://${HOST_IP}:${WEB_PORT}/swfs/gamedata/external_flash_texts/a18b0baa742c6a31de77b0f5e6ed3d2eca51dcde.txt";
?>
EOF

# Update cms_settings in MySQL database
php -r "
\$link = @mysql_connect('${DB_HOST}:${DB_PORT}', '${DB_USER}', '${DB_PASS}');
if (\$link && mysql_select_db('${DB_NAME}', \$link)) {
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('client_ip', '${HOST_IP}')\", \$link);
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('client_port', '${GAME_PORT}')\", \$link);
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('client_mus', '${MUS_PORT}')\", \$link);
    mysql_query(\"REPLACE INTO cms_settings (variable, value) VALUES ('cms_url', 'http://${HOST_IP}:${WEB_PORT}')\", \$link);
    echo \"[+] CMS database settings updated.\n\";
    mysql_close(\$link);
}
"

# Dynamic host injection in client.php and external_variables
sed -i "s|connection.info.host\" : \"localhost\"|connection.info.host\" : \"${HOST_IP}\"|g" /var/www/html/client.php || true
sed -i "s|connection.info.port\" : \"300\"|connection.info.port\" : \"${GAME_PORT}\"|g" /var/www/html/client.php || true
sed -i "s|http://localhost/|http://${HOST_IP}:${WEB_PORT}/|g" /var/www/html/client.php || true
sed -i "s|http://localhost|http://${HOST_IP}:${WEB_PORT}|g" /var/www/html/client.php || true

find /var/www/html/swfs/gamedata/ -type f -name "*.txt" -exec sed -i "s|//localhost/|//${HOST_IP}:${WEB_PORT}/|g" {} + || true
find /var/www/html/swfs/gamedata/ -type f -name "*.txt" -exec sed -i "s|http://localhost/|http://${HOST_IP}:${WEB_PORT}/|g" {} + || true

echo "[*] Starting Apache..."
exec apache2-foreground
