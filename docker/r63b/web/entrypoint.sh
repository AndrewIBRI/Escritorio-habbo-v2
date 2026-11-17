#!/usr/bin/env bash
set -e

echo "[*] HabboCMS (2012-r63b) Web starting..."

DB_HOST="${DB_HOST:-r63b-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63b}"

HOST_IP="${HOST_IP:-127.0.0.1}"
WEB_PORT="${PORT_R63B_WEB:-${WEB_PORT:-8062}}"
GAME_PORT="${PORT_R63B_GAME:-${GAME_PORT:-30100}}"
MUS_PORT="${PORT_R63B_MUS:-${MUS_PORT:-30101}}"

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
\$MySQLhostname = "${DB_HOST}";
\$MySQLusername = "${DB_USER}";
\$MySQLpassword = "${DB_PASS}";
\$MySQLdb = "${DB_NAME}";
\$cms_version = "2.0";
\$avatar = "http://www.habbo.co.uk/habbo-imaging/avatarimage?figure=";
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

# Dynamic host injection in client.php and external_variables.txt
sed -i "s|connection.info.host\" : \"localhost\"|connection.info.host\" : \"${HOST_IP}\"|g" /var/www/html/client.php || true
sed -i "s|http://localhost/|http://${HOST_IP}:${WEB_PORT}/|g" /var/www/html/client.php || true
sed -i "s|http://localhost/|http://${HOST_IP}:${WEB_PORT}/|g" /var/www/html/images/external_variables.txt || true

echo "[*] Starting Apache..."
exec apache2-foreground
