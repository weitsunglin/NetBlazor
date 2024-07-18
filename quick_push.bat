@echo off

:: Define an array of project directories
set "projects=a001 CommonLibrary services-gui test-project\tcp-test"

:: Loop through each project directory
for %%p in (%projects%) do (
    cd /d "%~dp0%%p"
    rd /s /q bin
    rd /s /q obj
    echo bin/ > .gitignore
    echo obj/ >> .gitignore
    git add -A
    git commit -m "commit %%p"
    git push origin main
)

echo Repositories configured and pushed successfully.
pause
