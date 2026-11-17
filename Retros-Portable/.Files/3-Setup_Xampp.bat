@echo off
setlocal

:: Go back one folder (from .Files to Retros-Portable)
cd /d "%~dp0.."

:: List of server folders
set SERVERS=2011-r63a 2012-r63b "2013+2014-r63b" "2015+2016+2017-r63c"

for %%S in (%SERVERS%) do (
    echo Updating setup_xampp.bat in %%S\Server...
    pushd "%%S\Server"
    if exist setup_xampp.bat (
        echo 1 | start /wait "" setup_xampp.bat
    ) else (
        echo setup_xampp.bat file not found in %%S\Server
    )
    popd
)

echo All folders processed!
pause
exit
