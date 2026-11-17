# 2011-r63a Retro (Gold Tree Emulator 3.0 / Phoenix + HobbaCMS)

## Overview
- **Emulator**: Gold Tree Emulator 3.0 (.NET Framework 4.5, running headless on Linux via Mono 6.8).
- **Web / CMS**: HobbaCMS (PHP 5.6 + Apache), with dynamic IP/port injection.
- **Database**: MariaDB 10.5 seeded automatically from `docker/r63a/sql/r63a.sql`.
- **Client**: Flash r63a (`phx/Habbo.swf`) with dynamic configuration via `phx/vari.php`.

## Ports
- **Web CMS**: `8061` (default) or `${PORT_R63A_WEB:-8061}`
- **Game TCP**: `30000` (default) or `${PORT_R63A_GAME:-30000}`
- **MUS TCP**: `30001` (default) or `${PORT_R63A_MUS:-30001}`
- **Database**: `33063` (default) or `${PORT_R63A_DB:-33063}`

## Running Standalone
From repository root:
```bash
docker compose -f docker-compose.r63a.yml up --build -d
```
Or from `docker/r63a/`:
```bash
docker compose up --build -d
```

Access Web CMS at `http://localhost:8061` (or `http://<YOUR_IP>:8061`).
