# 2012-r63b Retro (AtomEmulator & ButterStorm + HabboCMS)

## Overview
- **Emulator**: AtomEmulator (default) or ButterStorm (.NET Framework 4.0, running headless on Linux via Mono 6.8). Set `EMU_TYPE=atom` or `EMU_TYPE=butterfly`.
- **Web / CMS**: HabboCMS (PHP 5.6 + Apache), with dynamic database and client settings.
- **Database**: MariaDB 10.5 seeded automatically from `docker/r63b/sql/r63b.sql`.
- **Client**: Flash r63b (`images/Habbo.swf`) with dynamic asset paths.

## Ports
- **Web CMS**: `8062` (default) or `${PORT_R63B_WEB:-8062}`
- **Game TCP**: `30100` (default) or `${PORT_R63B_GAME:-30100}`
- **MUS TCP**: `30101` (default) or `${PORT_R63B_MUS:-30101}`
- **Database**: `33064` (default) or `${PORT_R63B_DB:-33064}`

## Running Standalone
From repository root:
```bash
docker compose -f docker-compose.r63b.yml up --build -d
```
Or from `docker/r63b/`:
```bash
docker compose up --build -d
```

Access Web CMS at `http://localhost:8062` (or `http://<YOUR_IP>:8062`).
