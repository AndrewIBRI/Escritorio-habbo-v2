@echo off
setlocal enabledelayedexpansion
title Habbo 2011 r63a (Gold Tree) Launcher

echo ==============================================================================
echo                 Habbo 2011 r63a (Gold Tree / Phoenix Retro)
echo ==============================================================================
echo.
echo Select execution mode:
echo   [1] Docker Compose (Isolated Linux container stack - Recommended)
echo   [2] Windows Native Portable (Runs bundled XAMPP and Gold Tree Emulator)
echo   [3] Stop Docker stack
echo.
set /p MODE="Enter choice [1-3] (Default: 1): "
if "%MODE%"=="" set MODE=1

if "%MODE%"=="1" (
    echo.
    echo [*] Starting Habbo 2011 (r63a) via Docker Compose...
    cd /d "%~dp0.."
    if not exist .env (
        echo [*] Creating .env from .env.example...
        copy .env.example .env >nul
    )
    docker compose --profile r63a up -d
    echo.
    echo [+] Retro 2011 r63a is online!
    echo     Web / Client URL: http://localhost:8061
    echo     Emulator Game Port: 30000
    echo     MUS Port: 30001
    echo     To view logs: docker compose logs -f r63a-emulator
    goto END
)

if "%MODE%"=="2" (
    echo.
    echo [*] Starting Native Portable mode...
    cd /d "%~dp0..\Retros-Portable\2011-r63a\2011-r63a"
    if not exist Server (
        echo [!] Nested folder not found, checking fallback path...
        cd /d "%~dp0..\Retros-Portable\2011-r63a"
    )
    echo [*] Launching XAMPP Server...
    start "XAMPP 2011-r63a" cmd /c "1-Server.bat"
    timeout /t 3 /nobreak >nul
    echo [*] Launching Gold Tree Emulator...
    start "Gold Tree Emulator" cmd /c "2-Emulator.bat"
    echo.
    echo [+] All native components launched! Check opened command windows.
    goto END
)

if "%MODE%"=="3" (
    echo.
    echo [*] Stopping Habbo 2011 r63a Docker containers...
    cd /d "%~dp0.."
    docker compose --profile r63a down
    goto END
)

:END
echo.
pause
