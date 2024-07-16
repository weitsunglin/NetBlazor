@echo off

REM Navigate to gameservice directory
cd /d "%~dp0gameservice"

REM Create .gitignore file and configure it to ignore bin and obj directories
echo bin/ > .gitignore
echo obj/ >> .gitignore

REM Add .gitignore, commit changes, and push to the remote repository
git add .gitignore
git commit -m "Add .gitignore to ignore bin and obj directories"
git add -A
git commit -m "Add all files and initial commit"
git push -u origin main

REM Navigate to services-gui directory
cd /d "%~dp0services-gui"

REM Create .gitignore file and configure it to ignore bin and obj directories
echo bin/ > .gitignore
echo obj/ >> .gitignore

REM Add .gitignore, commit changes, and push to the remote repository
git add .gitignore
git commit -m "Add .gitignore to ignore bin and obj directories"
git add -A
git commit -m "Add all files and initial commit"
git push -u origin main

echo Repositories configured and pushed successfully.
pause
