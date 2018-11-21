@echo off
set suppressConfirmation = "n"
set runbuild = ""

FOR %%A IN (%*) DO (
   FOR /f "tokens=1,2 delims=:" %%G IN ("%%A") DO set %%G=%%H
)

IF /i "%suppressConfirmation%"=="y" GOTO Confirm

SET /P basicyesno=You are about to publish CZ TEST version. Would you like to continue? [Y/N]:
IF /i "%basicyesno%"=="y" GOTO Confirm

GOTO End

:Confirm

SET language=CZ
@echo Language is: %language%
@echo.

SET environment=Test
@echo Environment is: %environment%
@echo.

SET assemblyName=MIR.Media.Importing2.Shell.%language%.%environment%
@echo AssemblyName is: %assemblyName%
@echo.

SET exeName=%assemblyName%.exe
@echo ExeName is: %exeName%
@echo.

SET appName=Importing2 %language% %environment%
@echo Application name is: %appName%
@echo.



SET toolsDir=..\..\..\tools\Deploy.Net.4.0
@echo Tools directory is: %toolsDir%
@echo.

SET binDir=.\MIR.Media.Importing2.Shell\bin\Debug
@echo Binary source directory is: %binDir%
@echo.

@echo determining application version...
FOR /F "tokens=1-3" %%i IN ('%toolsDir%\sigcheck.exe %binDir%\%exeName%') DO ( IF "%%i %%j"=="File version:" SET version=%%k)
@echo Application version is %version%



SET assemlyInfo=..\..\_shared\GlobalAssemblyInfo.cs
rem SET assemlyInfo=C:\Pool\AdIntel\src\_shared\Text.txt
@echo GlobalAsemlyInfo je zde: %assemlyInfo%

pause
@echo zjistuju verzi v assemlyInfo...

FOR /F "tokens=*" %%A IN (%assemlyInfo%) DO (@echo %%A)




GOTO End

:err
echo Nastala chyba...
IF /i "%suppressConfirmation%"=="y" exit /B 1
timeout 3
pause
exit /B 1

:end
IF /i "%suppressConfirmation%"=="y" exit /B 0
pause
timeout 3
exit /B 0