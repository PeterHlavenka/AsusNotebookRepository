@echo off
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Install.ps1"
if errorlevel 1 (
  echo Installation failed. Please share the error message with the maintainer.
  pause
  exit /b 1
)
echo Installation complete.
pause
