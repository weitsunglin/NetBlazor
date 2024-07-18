@echo off

cd /d "%~dp0log-service"
rd /s /q bin
rd /s /q obj
echo bin/ > .gitignore
echo obj/ >> .gitignore
git add -A
git commit -m "commit log-service"
git push origin main


cd /d "%~dp0services-gui"
rd /s /q bin
rd /s /q obj
echo bin/ > .gitignore
echo obj/ >> .gitignore
git add -A
git commit -m "commit services-gui"
git push origin main


cd /d "%~dp0test-project\tcp-test"
rd /s /q bin
rd /s /q obj
echo bin/ > .gitignore
echo obj/ >> .gitignore
git add -A
git commit -m "commit test-project"
git push origin main

echo Repositories configured and pushed successfully.
pause