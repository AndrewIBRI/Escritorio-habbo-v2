#!/usr/bin/env bash
set -e

echo "[*] RevCMS (2015-2017-r63c PlusEMU) Web starting..."

DB_HOST="${DB_HOST:-r63c-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-root}"
DB_PASS="${DB_PASS:-${MYSQL_ROOT_PASSWORD:-habboroot}}"
DB_NAME="${DB_NAME:-r63c}"

HOST_IP="${HOST_IP:-127.0.0.1}"
WEB_PORT="${PORT_R63C_WEB:-${WEB_PORT:-8064}}"
GAME_PORT="${PORT_R63C_GAME:-${GAME_PORT:-30300}}"
MUS_PORT="${PORT_R63C_MUS:-${MUS_PORT:-30301}}"

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

# Generate RevCMS config.php
CONFIG_FILE="/var/www/html/app/management/config.php"
cat <<EOF > "${CONFIG_FILE}"
<?php
if(!defined('IN_INDEX')) { die('Sorry, you cannot access this file.'); }

\$_CONFIG['mysql']['connection_type'] = 'connect';
\$_CONFIG['mysql']['hostname'] = '${DB_HOST}';
\$_CONFIG['mysql']['username'] = '${DB_USER}';
\$_CONFIG['mysql']['password'] = '${DB_PASS}';
\$_CONFIG['mysql']['database'] = '${DB_NAME}';
\$_CONFIG['mysql']['port'] = '${DB_PORT}';

\$_CONFIG['hotel']['server_ip'] = '${CLIENT_HOST}';
\$_CONFIG['hotel']['url'] = '${HOTEL_URL}';
\$_CONFIG['hotel']['name'] = 'Habbo';
\$_CONFIG['hotel']['desc'] = 'Retro Portable';
\$_CONFIG['hotel']['email'] = 'Support@hotmail.com';
\$_CONFIG['hotel']['in_maint'] = False;
\$_CONFIG['hotel']['motto'] = 'I <3 Habbo!';
\$_CONFIG['hotel']['credits'] = 3000000000;
\$_CONFIG['hotel']['pixels'] = 150000000;
\$_CONFIG['hotel']['figure'] = '-';
\$_CONFIG['hotel']['web_build'] = '63_1dc60c6d6ea6e089c6893ab4e0541ee0/527';
\$_CONFIG['hotel']['external_vars'] = '${HOTEL_URL}/swf/gamedata/external_variables.txt';
\$_CONFIG['hotel']['external_texts'] = '${HOTEL_URL}/swf/gamedata/external_flash_texts.txt';
\$_CONFIG['hotel']['product_data'] = '${HOTEL_URL}/swf/gamedata/productdata.txt';
\$_CONFIG['hotel']['furni_data'] = '${HOTEL_URL}/swf/gamedata/furnidata.xml';
\$_CONFIG['hotel']['swf_folder'] = '${HOTEL_URL}/swf/gordon/PRODUCTION-201701242205-837386173';

\$_CONFIG['template']['style'] = 'habbologic';
\$_CONFIG['thehabbos']['username'] = '';
\$_CONFIG['retro_top']['user'] = '';
\$_CONFIG['recaptcha']['priv_key'] = '';
\$_CONFIG['recaptcha']['pub_key'] = '';
\$_CONFIG['social']['twitter'] = '';
\$_CONFIG['social']['facebook'] = '';

error_reporting(0);
?>
EOF

# Update housekeeping DB configs
for hk in /var/www/html/app/tpl/skins/*/databaseconfig.php /var/www/html/app/tpl/skins/*/*/databaseconfig.php /var/www/html/app/tpl/skins/*/*/*/databaseconfig.php; do
    if [ -f "\$hk" ]; then
        cat <<EOF > "\$hk"
<?php
\$host = "${DB_HOST}";
\$user = "${DB_USER}";
\$password = "${DB_PASS}";
\$database = "${DB_NAME}";
mysql_connect(\$host, \$user, \$password) or die(mysql_error());
mysql_select_db(\$database) or die(mysql_error());
?>
EOF
    fi
done

# Dynamic host injection in external_variables
find /var/www/html/swf/gamedata/ -type f -name "*.txt" -exec sed -i "s|localhost/swf|${CLIENT_HOST}:${WEB_PORT}/swf|g" {} + || true
find /var/www/html/swf/gamedata/ -type f -name "*.txt" -exec sed -i "s|http://localhost/|${HOTEL_URL}/|g" {} + || true

# Ensure Apache listens on both port 80 and 8064 for Easypanel routing compatibility
if ! grep -q "Listen 8064" /etc/apache2/ports.conf 2>/dev/null; then
    echo "Listen 8064" >> /etc/apache2/ports.conf
fi

echo "[*] Starting Apache..."
exec apache2-foreground
