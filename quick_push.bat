@echo off

REM Navigate to log-service directory
cd /d "%~dp0log-service"
rd /s /q bin
rd /s /q obj

REM Create .gitignore file and configure it to ignore bin and obj directories
echo bin/ > .gitignore
echo obj/ >> .gitignore

REM Add .gitignore, commit changes, and push to the remote repository
git add -A
git commit -m "commit all files"
git push -u origin main

REM Navigate to services-gui directory
cd /d "%~dp0services-gui"
rd /s /q bin
rd /s /q obj

REM Create .gitignore file and configure it to ignore bin and obj directories
echo bin/ > .gitignore
echo obj/ >> .gitignore

REM Add .gitignore, commit changes, and push to the remote repository
git add -A
git commit -m "commit all files"
git push -u origin main

echo Repositories configured and pushed successfully.
pause
