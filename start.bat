@echo off

cd /d "%~dp0a001"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0services-gui"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0tcpClientExample"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"