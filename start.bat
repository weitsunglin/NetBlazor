@echo off

cd /d "%~dp0a001"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0ServerManager"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0test/tcp-test"
rd /s /q bin
rd /s /q obj
start cmd /k "dotnet run"

cd /d "%~dp0test"
start cmd /k "http-test.bat"

timeout /t 2 /nobreak
start http://localhost:5283
