@echo off
setlocal enabledelayedexpansion
title Habbo 2012 r63b (Atom / ButterStorm) Launcher

echo ==============================================================================
echo              Habbo 2012 r63b (AtomEmulator / ButterStorm Retro)
echo ==============================================================================
echo.
echo Select execution mode:
echo   [1] Docker Compose (Isolated Linux container stack - Recommended)
echo   [2] Windows Native Portable (Runs bundled XAMPP and Atom Emulator)
echo   [3] Windows Native Portable (Runs bundled XAMPP and ButterStorm Emulator)
echo   [4] Stop Docker stack
echo.
set /p MODE="Enter choice [1-4] (Default: 1): "
if "%MODE%"=="" set MODE=1

if "%MODE%"=="1" (
    echo.
    echo [*] Starting Habbo 2012 (r63b) via Docker Compose...
    cd /d "%~dp0.."
    if not exist .env (
        echo [*] Creating .env from .env.example...
        copy .env.example .env >nul
    )
    docker compose --profile r63b up -d
    echo.
    echo [+] Retro 2012 r63b is online!
    echo     Web / Client URL: http://localhost:8062
    echo     Emulator Game Port: 30100
    echo     MUS Port: 30101
    echo     To view logs: docker compose logs -f r63b-emulator
    goto END
)

if "%MODE%"=="2" (
    echo.
    echo [*] Starting Native Portable mode (Atom Emulator)...
    cd /d "%~dp0..\Retros-Portable\2012-r63b\2012-r63b"
    if not exist Server (
        echo [!] Nested folder not found, checking fallback path...
        cd /d "%~dp0..\Retros-Portable\2012-r63b"
    )
    echo [*] Launching XAMPP Server...
    start "XAMPP 2012-r63b" cmd /c "1-Server.bat"
    timeout /t 3 /nobreak >nul
    echo [*] Launching Atom Emulator...
    start "Atom Emulator" cmd /c "2-Emulator(Atom).bat"
    echo.
    echo [+] All native components launched! Check opened command windows.
    goto END
)

if "%MODE%"=="3" (
    echo.
    echo [*] Starting Native Portable mode (ButterStorm Emulator)...
    cd /d "%~dp0..\Retros-Portable\2012-r63b\2012-r63b"
    if not exist Server (
        echo [!] Nested folder not found, checking fallback path...
        cd /d "%~dp0..\Retros-Portable\2012-r63b"
    )
    echo [*] Launching XAMPP Server...
    start "XAMPP 2012-r63b" cmd /c "1-Server.bat"
    timeout /t 3 /nobreak >nul
    echo [*] Launching ButterStorm Emulator...
    start "ButterStorm Emulator" cmd /c "2-Emulator(ButterStorm).bat"
    echo.
    echo [+] All native components launched! Check opened command windows.
    goto END
)

if "%MODE%"=="4" (
    echo.
    echo [*] Stopping Habbo 2012 r63b Docker containers...
    cd /d "%~dp0.."
    docker compose --profile r63b down
    goto END
)

:END
echo.
pause
