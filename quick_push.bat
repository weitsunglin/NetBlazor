@echo off

cd /d "%~dp0log-service"
rd /s /q bin
rd /s /q obj

echo bin/ > .gitignore
echo obj/ >> .gitignore

git add -A
git commit -m "commit all files"
git push -u origin main

cd /d "%~dp0services-gui"
rd /s /q bin
rd /s /q obj

echo bin/ > .gitignore
echo obj/ >> .gitignore

git add -A
git commit -m "commit all files"
git push -u origin main

echo Repositories configured and pushed successfully.
pause