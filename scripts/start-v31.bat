@echo off
setlocal enabledelayedexpansion
title Habbo 2009 v31 (Havana) Launcher

echo ==============================================================================
echo                 Habbo 2009 v31 (Havana Flash/Shockwave Retro)
echo ==============================================================================
echo.
echo Select execution mode:
echo   [1] Docker Compose (Isolated Linux container stack - Recommended)
echo   [2] Windows Native Portable (Runs bundled MariaDB, Havana-Server & Web)
echo   [3] Stop Docker stack
echo.
set /p MODE="Enter choice [1-3] (Default: 1): "
if "%MODE%"=="" set MODE=1

if "%MODE%"=="1" (
    echo.
    echo [*] Starting Habbo 2009 (v31) via Docker Compose...
    cd /d "%~dp0.."
    if not exist .env (
        echo [*] Creating .env from .env.example...
        copy .env.example .env >nul
    )
    docker compose --profile v31 up -d
    echo.
    echo [+] Retro 2009 v31 is online!
    echo     Web / Client URL: http://localhost:8031
    echo     Emulator Game Port: 12331
    echo     To view logs: docker compose logs -f v31-emulator
    goto END
)

if "%MODE%"=="2" (
    echo.
    echo [*] Starting Native Portable mode...
    cd /d "%~dp0..\Retros-Portable\2009-v31\2009-v31"
    if not exist MariaDB (
        echo [!] Nested folder not found, checking fallback path...
        cd /d "%~dp0..\Retros-Portable\2009-v31"
    )
    echo [*] Launching MariaDB...
    start "MariaDB 2009-v31" cmd /c "1-Database.bat"
    timeout /t 3 /nobreak >nul
    echo [*] Launching Havana Server & Web...
    start "Havana 2009-v31" cmd /c "2-Server & Emulator.bat"
    echo.
    echo [+] All native components launched! Check opened command windows.
    goto END
)

if "%MODE%"=="3" (
    echo.
    echo [*] Stopping Habbo 2009 v31 Docker containers...
    cd /d "%~dp0.."
    docker compose --profile v31 down
    goto END
)

:END
echo.
pause
