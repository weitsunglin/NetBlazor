@echo off

cd /d "%~dp0a001"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0services-gui"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0test-project/tcp-test"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0test-project"
start cmd /k "http-test.bat"

start http://localhost:5283