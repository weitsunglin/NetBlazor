@echo off

cd /d "%~dp0log-service"
rd /s /q bin
rd /s /q obj

start cmd /k "dotnet run"

cd /d "%~dp0services-gui"
rd /s /q bin
rd /s /q obj

start cmd /k "dotnet run"