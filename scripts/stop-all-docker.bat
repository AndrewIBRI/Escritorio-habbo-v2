@echo off
setlocal
title Stop ALL Habbo 6-in-1 Retros

echo [*] Stopping all Habbo Docker containers and profiles...
cd /d "%~dp0.."
docker compose --profile all down
echo [+] All containers stopped.
pause
