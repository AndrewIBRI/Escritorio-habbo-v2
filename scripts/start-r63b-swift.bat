@echo off
setlocal enabledelayedexpansion
title Habbo 2013-2014 r63b (Swift Emulator) Launcher

echo ==============================================================================
echo                 Habbo 2013+2014 r63b (SwiftEmulator Retro)
echo ==============================================================================
echo.
echo Select execution mode:
echo   [1] Docker Compose (Isolated Linux container stack - Recommended)
echo   [2] Windows Native Portable (Runs bundled XAMPP and Swift Emulator)
echo   [3] Stop Docker stack
echo.
set /p MODE="Enter choice [1-3] (Default: 1): "
if "%MODE%"=="" set MODE=1

if "%MODE%"=="1" (
    echo.
    echo [*] Starting Habbo 2013+2014 (r63b-swift) via Docker Compose...
    cd /d "%~dp0.."
    if not exist .env (
        echo [*] Creating .env from .env.example...
        copy .env.example .env >nul
    )
    docker compose --profile r63b-swift up -d
    echo.
    echo [+] Retro 2013+2014 r63b-swift is online!
    echo     Web / Client URL: http://localhost:8063
    echo     Emulator Game Port: 30200
    echo     MUS Port: 30201
    echo     To view logs: docker compose logs -f r63b-swift-emulator
    goto END
)

if "%MODE%"=="2" (
    echo.
    echo [*] Starting Native Portable mode...
    cd /d "%~dp0..\Retros-Portable\2013+2014-r63b\2013+2014-r63b"
    if not exist Server (
        echo [!] Nested folder not found, checking fallback path...
        cd /d "%~dp0..\Retros-Portable\2013+2014-r63b"
    )
    echo [*] Launching XAMPP Server...
    start "XAMPP 2013+2014" cmd /c "1-Server.bat"
    timeout /t 3 /nobreak >nul
    echo [*] Launching Swift Emulator...
    start "Swift Emulator" cmd /c "2-Emulator.bat"
    echo.
    echo [+] All native components launched! Check opened command windows.
    goto END
)

if "%MODE%"=="3" (
    echo.
    echo [*] Stopping Habbo 2013+2014 r63b-swift Docker containers...
    cd /d "%~dp0.."
    docker compose --profile r63b-swift down
    goto END
)

:END
echo.
pause
