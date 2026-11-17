@echo off
setlocal enabledelayedexpansion
title Habbo HTML5 Nitro (Arcturus Morningstar) Launcher

echo ==============================================================================
echo              Habbo HTML5 Nitro + Arcturus Morningstar (No Flash)
echo ==============================================================================
echo.
echo Select execution mode:
echo   [1] Start Nitro HTML5 stack (docker-compose.nitro.yml)
echo   [2] View Emulator Logs
echo   [3] View Web Logs
echo   [4] Stop Nitro HTML5 stack
echo.
set /p MODE="Enter choice [1-4] (Default: 1): "
if "%MODE%"=="" set MODE=1

cd /d "%~dp0.."

if "%MODE%"=="1" (
    echo.
    echo [*] Starting Habbo HTML5 Nitro via Docker Compose...
    docker compose -f docker-compose.nitro.yml up -d --build
    echo.
    echo [+] Nitro HTML5 Hotel is online!
    echo     Web Portal / Client URL: http://localhost:8064
    echo     WebSocket Port: 2096 (proxied via /ws)
    echo     Emulator Game Port: 3000
    echo     RCON Port: 3001
    goto END
)

if "%MODE%"=="2" (
    docker compose -f docker-compose.nitro.yml logs -f nitro-emulator
    goto END
)

if "%MODE%"=="3" (
    docker compose -f docker-compose.nitro.yml logs -f nitro-web
    goto END
)

if "%MODE%"=="4" (
    docker compose -f docker-compose.nitro.yml down
    goto END
)

:END
echo.
pause
