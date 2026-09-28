@echo off
chcp 65001 >nul
cd /d "%~dp0"
start "" "%~dp0..\client\server\launcher\Nanaimo.Launcher.exe" --stop-server
