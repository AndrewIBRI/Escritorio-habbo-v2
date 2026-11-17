# Habbo 2007 (v14) - Kepler Shockwave Retro

Self-contained, production-ready containerization for **Habbo 2007 v14** using the **Kepler** Java emulator (by Quackster), MariaDB database, and Apache/PHP web server for DCR assets and Shockwave client loader.

---

## Architecture

| Service | Container Name | Base Image | Description | Ports |
|---------|---------------|------------|-------------|-------|
| `v14-db` | `habbo-v14-db` | `mariadb:11` | MariaDB 11 initialized from `extracted_sql/v14.sql` | `33061:3306` |
| `v14-emulator` | `habbo-v14-emulator` | `eclipse-temurin:17-jre-jammy` | Kepler Java game server running on Java 17 | `12321:12321` (Game), `12322:12322` (MUS), `12309:12309` (RCON) |
| `v14-web` | `habbo-v14-web` | `php:8.2-apache` | Apache + PHP serving DCRs, CCTs, external variables, and client loader | `8014:80` |

---

## Quick Start

### 1. Launch with Standalone Docker Compose
```bash
# Start the v14 stack
docker compose -f docker-compose.v14.yml up -d

# Check service logs
docker compose -f docker-compose.v14.yml logs -f v14-emulator

# Stop the stack
docker compose -f docker-compose.v14.yml down
```

### 2. Launch with Unified Docker Compose Profile
```bash
docker compose --profile v14 up -d
```

### 3. One-Click Scripts
- **Windows**: Run `scripts\start-v14.bat`
- **Linux / macOS**: Run `./scripts/start-v14.sh`

---

## Playing the Game

1. Open your browser with Shockwave support (e.g. Pale Moon 32-bit or the included `Retros-Portable/.Files/BasiliskPortable`).
2. Navigate to:
   ```
   http://localhost:8014/
   ```
3. To play over LAN or VPS, set `HOST_IP` in `.env` to your server's IP address (e.g. `HOST_IP=192.168.1.100` or `HOST_IP=your-domain.com`). The client page and external variables will dynamically bind to that address automatically.

---

## Configuration & Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `HOST_IP` | `127.0.0.1` | Public IP or domain name for client connections |
| `PORT_V14_WEB` | `8014` | Host port mapped to Web HTTP service |
| `PORT_V14_GAME` | `12321` | Host port mapped to Kepler Game socket |
| `PORT_V14_MUS` | `12322` | Host port mapped to MUS socket |
| `PORT_V14_RCON` | `12309` | Host port mapped to RCON socket |
| `PORT_V14_DB` | `33061` | Host port mapped to MariaDB |
| `MYSQL_ROOT_PASSWORD` | `habboroot` | Database root password |
| `DB_NAME_V14` | `v14` | Database name |
