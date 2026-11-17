@echo off
setlocal enabledelayedexpansion
title Habbo 2007 v14 (Kepler) Launcher

echo ==============================================================================
echo                 Habbo 2007 v14 (Kepler Shockwave Retro)
echo ==============================================================================
echo.
echo Select execution mode:
echo   [1] Docker Compose (Isolated Linux container stack - Recommended)
echo   [2] Windows Native Portable (Runs bundled MariaDB, PHP server and Kepler)
echo   [3] Stop Docker stack
echo.
set /p MODE="Enter choice [1-3] (Default: 1): "
if "%MODE%"=="" set MODE=1

if "%MODE%"=="1" (
    echo.
    echo [*] Starting Habbo 2007 (v14) via Docker Compose...
    cd /d "%~dp0.."
    if not exist .env (
        echo [*] Creating .env from .env.example...
        copy .env.example .env >nul
    )
    docker compose --profile v14 up -d
    echo.
    echo [+] Retro 2007 v14 is online!
    echo     Web / Client URL: http://localhost:8014
    echo     Emulator Game Port: 12321
    echo     To view logs: docker compose logs -f v14-emulator
    goto END
)

if "%MODE%"=="2" (
    echo.
    echo [*] Starting Native Portable mode...
    cd /d "%~dp0..\Retros-Portable\2007-v14\2007-v14"
    if not exist MariaDB (
        echo [!] Nested folder not found, checking fallback path...
        cd /d "%~dp0..\Retros-Portable\2007-v14"
    )
    echo [*] Launching MariaDB...
    start "MariaDB 2007-v14" cmd /c "1-Database.bat"
    timeout /t 3 /nobreak >nul
    echo [*] Launching Web Server...
    start "Web Server 2007-v14" cmd /c "2-Server.bat"
    timeout /t 2 /nobreak >nul
    echo [*] Launching Kepler Emulator...
    start "Kepler Emulator 2007-v14" cmd /c "3-Emulator.bat"
    echo.
    echo [+] All native components launched! Check opened command windows.
    goto END
)

if "%MODE%"=="3" (
    echo.
    echo [*] Stopping Habbo 2007 v14 Docker containers...
    cd /d "%~dp0.."
    docker compose --profile v14 down
    goto END
)

:END
echo.
pause
