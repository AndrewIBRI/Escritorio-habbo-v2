# 2013+2014-r63b Retro (SwiftEmulator + ProjectHabbo CMS)

## Overview
- **Emulator**: SwiftEmulator / Butterfly Rev 5 Alpha 3 (.NET Framework 4.0, running headless on Linux via Mono 6.8).
- **Web / CMS**: ProjectHabbo CMS (PHP 5.6 + Apache), with dynamic server IP and port injection.
- **Database**: MariaDB 10.5 seeded automatically from `docker/r63b-swift/sql/r63b_swift.sql`.
- **Client**: Flash RELEASE63-201302211227 with dynamic external_variables.

## Ports
- **Web CMS**: `8063` (default) or `${PORT_R63B_SWIFT_WEB:-8063}`
- **Game TCP**: `30200` (default) or `${PORT_R63B_SWIFT_GAME:-30200}`
- **MUS TCP**: `30201` (default) or `${PORT_R63B_SWIFT_MUS:-30201}`
- **Database**: `33065` (default) or `${PORT_R63B_SWIFT_DB:-33065}`

## Running Standalone
From repository root:
```bash
docker compose -f docker-compose.swift.yml up --build -d
```
Or from `docker/r63b-swift/`:
```bash
docker compose up --build -d
```

Access Web CMS at `http://localhost:8063` (or `http://<YOUR_IP>:8063`).
