@echo off
setlocal enabledelayedexpansion
title Habbo 2015-2017 r63c (PlusEMU) Launcher

echo ==============================================================================
echo                 Habbo 2015-2017 r63c (PlusEMU Retro)
echo ==============================================================================
echo.
echo Select execution mode:
echo   [1] Docker Compose (Isolated Linux container stack - Recommended)
echo   [2] Windows Native Portable (Runs bundled XAMPP and PlusEMU)
echo   [3] Stop Docker stack
echo.
set /p MODE="Enter choice [1-3] (Default: 1): "
if "%MODE%"=="" set MODE=1

if "%MODE%"=="1" (
    echo.
    echo [*] Starting Habbo 2015-2017 (r63c) via Docker Compose...
    cd /d "%~dp0.."
    if not exist .env (
        echo [*] Creating .env from .env.example...
        copy .env.example .env >nul
    )
    docker compose --profile r63c up -d
    echo.
    echo [+] Retro 2015-2017 r63c is online!
    echo     Web / Client URL: http://localhost:8064
    echo     Emulator Game Port: 30300
    echo     MUS / RCON Port: 30301
    echo     To view logs: docker compose logs -f r63c-emulator
    goto END
)

if "%MODE%"=="2" (
    echo.
    echo [*] Starting Native Portable mode...
    cd /d "%~dp0..\Retros-Portable\2015+2016+2017-r63c\2015+2016+2017-r63c"
    if not exist Server (
        echo [!] Nested folder not found, checking fallback path...
        cd /d "%~dp0..\Retros-Portable\2015+2016+2017-r63c"
    )
    echo [*] Launching XAMPP Server...
    start "XAMPP 2015-2017" cmd /c "1-Server.bat"
    timeout /t 3 /nobreak >nul
    echo [*] Launching PlusEMU Emulator...
    start "PlusEMU Emulator" cmd /c "2-Emulator.bat"
    echo.
    echo [+] All native components launched! Check opened command windows.
    goto END
)

if "%MODE%"=="3" (
    echo.
    echo [*] Stopping Habbo 2015-2017 r63c Docker containers...
    cd /d "%~dp0.."
    docker compose --profile r63c down
    goto END
)

:END
echo.
pause
