#!/bin/bash
set -e

DB_HOST="${DB_HOST:-nitro-db}"
DB_PORT="${DB_PORT:-3306}"
DB_USER="${DB_USER:-arcturus_user}"
DB_PASS="${DB_PASS:-HabboUser_0kh9qhJkhYI0Klk444U45VjA}"
DB_NAME="${DB_NAME:-arcturus}"

echo "[*] Arcturus Morningstar starting..."
echo "[*] Waiting for Database at ${DB_HOST}:${DB_PORT}..."

while ! nc -z "$DB_HOST" "$DB_PORT"; do
  sleep 1
done

echo "[+] Database is reachable!"

# Render config.ini from template
sed -e "s/\${DB_HOST}/$DB_HOST/g" \
    -e "s/\${DB_PORT}/$DB_PORT/g" \
    -e "s/\${DB_NAME}/$DB_NAME/g" \
    -e "s/\${DB_USER}/$DB_USER/g" \
    -e "s/\${DB_PASS}/$DB_PASS/g" \
    /app/config.ini.template > /app/config.ini

echo "[+] Generated /app/config.ini"

# Ensure plugins directory exists and NitroWebsockets is present
mkdir -p /app/plugins
if [ -f /app/prebuilt-plugins/NitroWebsockets-3.1.jar ]; then
  cp /app/prebuilt-plugins/NitroWebsockets-3.1.jar /app/plugins/
fi

JAR_FILE=$(ls /app/Habbo-*-jar-with-dependencies.jar 2>/dev/null | head -n 1 || ls /app/*.jar 2>/dev/null | head -n 1)

if [ -z "$JAR_FILE" ]; then
  echo "[-] ERROR: No Arcturus JAR found in /app!"
  exit 1
fi

echo "[*] Launching Arcturus Morningstar ($JAR_FILE)..."
exec java -Dfile.encoding=UTF-8 -Duser.country=US -Duser.language=en -jar "$JAR_FILE"
