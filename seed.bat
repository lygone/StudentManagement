@echo off
chcp 65001 >nul
echo ========================================
echo   StudentManagement - Seed Data
echo ========================================
echo.

set CSC=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist "%CSC%" set CSC=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe

echo [1/2] Compiling SeedData.cs...
"%CSC%" /nologo /out:SeedData.exe /r:packages\LiteDB.5.0.21\lib\net45\LiteDB.dll SeedData.cs
if %ERRORLEVEL% NEQ 0 (
    echo [FAIL] Compilation failed!
    pause
    exit /b 1
)
echo [OK] Compiled

echo [2/2] Generating test data...
SeedData.exe
echo.
echo Copying database to project output...
if not exist "StudentManagement\bin\Debug" mkdir "StudentManagement\bin\Debug"
copy /Y Students.db "StudentManagement\bin\Debug\Students.db" >nul
echo [OK] Done!
echo.
pause
