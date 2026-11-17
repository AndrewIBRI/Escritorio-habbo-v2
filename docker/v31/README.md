# Habbo 2009 (v31) - Havana Flash & Shockwave Retro

Self-contained, production-ready containerization for **Habbo 2009 v31** using the **Havana** Java emulator and built-in Java Web server (by Quackster), MariaDB database, and dual Flash / Shockwave client support.

---

## Architecture

| Service | Container Name | Base Image | Description | Ports |
|---------|---------------|------------|-------------|-------|
| `v31-db` | `habbo-v31-db` | `mariadb:11` | MariaDB 11 initialized from `extracted_sql/v31.sql` | `33062:3306` |
| `v31-emulator` | `habbo-v31-emulator` | `eclipse-temurin:17-jre-jammy` | Havana Java game server running on Java 17 | `12321:12321` (Shockwave Game), `12322:12322` (MUS), `12309:12309` (RCON) |
| `v31-web` | `habbo-v31-web` | `eclipse-temurin:17-jre-jammy` | Havana Java web server (CMS, templates, Shockwave & Flash assets) | `8031:80` |

---

## Quick Start

### 1. Launch with Standalone Docker Compose
```bash
# Start the v31 stack
docker compose -f docker-compose.v31.yml up -d

# Check service logs
docker compose -f docker-compose.v31.yml logs -f v31-emulator
docker compose -f docker-compose.v31.yml logs -f v31-web

# Stop the stack
docker compose -f docker-compose.v31.yml down
```

### 2. Launch with Unified Docker Compose Profile
```bash
docker compose --profile v31 up -d
```

### 3. One-Click Scripts
- **Windows**: Run `scripts\start-v31.bat`
- **Linux / macOS**: Run `./scripts/start-v31.sh`

---

## Features

- **Built-in Java Web Server**: No PHP or Apache required! `Havana-Web.jar` serves the website, CMS, user registration, community pages, and hotel client.
- **Multi-Language Templates**: Includes English (`default-en`), Spanish (`es`), and Portuguese (`pt`).
- **Dual Client Support**:
  - Shockwave Client: `http://localhost:8031/client`
  - Flash Client: `http://localhost:8031/flash`
- **Dynamic Configuration**: Automatically seeds the MariaDB `settings` table and templates `server.ini` and `webserver-config.ini` with `HOST_IP` and custom ports.

---

## Configuration & Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `HOST_IP` | `127.0.0.1` | Public IP or domain name for client connections |
| `PORT_V31_WEB` | `8031` | Host port mapped to Havana Web server |
| `PORT_V31_GAME` | `12321` | Host port mapped to Havana Shockwave game socket |
| `PORT_V31_MUS` | `12322` | Host port mapped to MUS socket |
| `PORT_V31_RCON` | `12309` | Host port mapped to RCON socket |
| `PORT_V31_DB` | `33062` | Host port mapped to MariaDB |
| `MYSQL_ROOT_PASSWORD` | `habboroot` | Database root password |
| `DB_NAME_V31` | `v31` | Database name |
