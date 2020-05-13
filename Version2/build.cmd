@echo off
rem  *********************************************************************************
rem  *  This script is intended for rebuilding a project.                            *
rem  *********************************************************************************

REM =================================================

SET VS90IDEDIR=%VS90COMNTOOLS%..\IDE\
SET BuildType=Rebuild
SET BUILD_CONFIGURATION=RELEASE
SET SLN_PATH=TestTool\
SET SLN_FILE=TestTool.sln
SET SETUP_PROJECT=TestTool.Setup\TestTool.Setup.vdproj
SET LIVE555_PATCH_PATH=External
SET LIVE555_PATCH_FILE=live_applypatch.bat
SET LIVE555_PROJECT=live\build\live.vcproj

SET CUR_DIR_BEFORE_BUILD=%CD%

ECHO.

REM ============ Build help file ==================
REM TODO

REM ============== Patch live555 ==================
CHDIR /D %LIVE555_PATCH_PATH%
ECHO Patching live555 (%TIME% %DATE%)...
CALL "%LIVE555_PATCH_FILE%"
ECHO Completed (%TIME% %DATE%)

REM ============= Build live555 ===================
ECHO Building live555 (%TIME% %DATE%)...
IF EXIST ..\TestTool\Lib\live.lib del ..\TestTool\Lib\live.lib /q /f
"%VS90IDEDIR%"devenv.exe %LIVE555_PROJECT% /Build "%BUILD_CONFIGURATION%" /out "%CUR_DIR_BEFORE_BUILD%\buildlog.txt"
IF %ERRORLEVEL% NEQ 0 GOTO VS_ERROR
ECHO Completed (%TIME% %DATE%)
CHDIR /D %CUR_DIR_BEFORE_BUILD%

REM ============= Build projects ==================
CHDIR /D %SLN_PATH%
ECHO %BuildType%ing %SLN_FILE% solution (%TIME% %DATE%)...
"%VS90IDEDIR%"devenv.exe /Build %BUILD_CONFIGURATION% %SLN_FILE% /out "%CUR_DIR_BEFORE_BUILD%\buildlog.txt"
IF %ERRORLEVEL% NEQ 0 GOTO VS_ERROR
ECHO Completed (%TIME% %DATE%)

REM ============ Build setup project ==============
ECHO Building %SETUP_PROJECT% setup project (%TIME% %DATE%)...
"%VS90IDEDIR%"devenv.exe %SLN_FILE% /project %SETUP_PROJECT% /Build "%BUILD_CONFIGURATION%" /out "%CUR_DIR_BEFORE_BUILD%\buildlog.txt"
IF %ERRORLEVEL% NEQ 0 GOTO VS_ERROR
ECHO Completed (%TIME% %DATE%)

GOTO BUILD_EXIT

:VS_ERROR

echo.
echo Build error occured. Devenv.exe exited with code [%ERRORLEVEL%]
echo See buildlog.txt for details
echo.


:BUILD_EXIT

SET VS90IDEDIR=
SET BuildType=
SET BUILD_CONFIGURATION=
SET SLN_PATH=
SET SLN_FILE=
SET SETUP_PROJECT=
SET LIVE555_PATCH=
SET LIVE555_PROJECT=