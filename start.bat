@echo off

:: 切换到 log-service 目录并删除 bin 和 obj 目录
cd /d "%~dp0log-service"
rd /s /q bin
rd /s /q obj

:: 启动 log-service
start cmd /k "dotnet run"

:: 切换到 services-gui 目录并删除 bin 和 obj 目录
cd /d "%~dp0services-gui"
rd /s /q bin
rd /s /q obj

:: 启动 services-gui
start cmd /k "dotnet run"
