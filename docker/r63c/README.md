# 2015+2016+2017-r63c Retro (PlusEMU + RevCMS)

## Overview
- **Emulator**: PlusEMU (.NET Framework 4.6, running headless on Linux via Mono 6.8). Includes Linux-safe `Program.cs` patch for Console window handling.
- **Web / CMS**: RevCMS (PHP 5.6 + Apache), with dynamic server IP, ports, and SWF configuration.
- **Database**: MariaDB 10.5 seeded automatically from `docker/r63c/sql/r63c.sql`.
- **Client**: Flash PRODUCTION-201701242205 with dynamic external variables.

## Ports
- **Web CMS**: `8064` (default) or `${PORT_R63C_WEB:-8064}`
- **Game TCP**: `30300` (default) or `${PORT_R63C_GAME:-30300}`
- **MUS / RCON TCP**: `30301` (default) or `${PORT_R63C_MUS:-30301}`
- **Database**: `33066` (default) or `${PORT_R63C_DB:-33066}`

## Running Standalone
From repository root:
```bash
docker compose -f docker-compose.r63c.yml up --build -d
```
Or from `docker/r63c/`:
```bash
docker compose up --build -d
```

Access Web CMS at `http://localhost:8064` (or `http://<YOUR_IP>:8064`).
