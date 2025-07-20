@echo off
echo ============================================
echo    Kafka Chat Applications - Build Script
echo ============================================
echo.

echo Building Console Chat Producer...
cd "ConsoleChat\ChatProducer"
dotnet build --configuration Release
if %ERRORLEVEL% neq 0 (
    echo Failed to build Chat Producer
    pause
    exit /b 1
)
cd ..\..

echo.
echo Building Console Chat Consumer...
cd "ConsoleChat\ChatConsumer"
dotnet build --configuration Release
if %ERRORLEVEL% neq 0 (
    echo Failed to build Chat Consumer
    pause
    exit /b 1
)
cd ..\..

echo.
echo Building Windows Forms Chat App...
cd "WindowsFormsChat\ChatApp"
dotnet build --configuration Release
if %ERRORLEVEL% neq 0 (
    echo Failed to build Windows Forms Chat App
    pause
    exit /b 1
)
cd ..\..

echo.
echo ============================================
echo All applications built successfully!
echo ============================================
echo.
echo Next steps:
echo 1. Make sure Kafka is running (see setup-kafka.bat)
echo 2. Run the applications using the run-*.bat files
echo.
pause
