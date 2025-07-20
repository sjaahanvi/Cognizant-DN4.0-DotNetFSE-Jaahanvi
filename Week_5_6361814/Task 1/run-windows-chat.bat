@echo off
echo Starting Windows Forms Chat Application...
echo.
echo This application provides a GUI for chatting via Kafka.
echo Make sure Kafka is running before proceeding.
echo You can run multiple instances for different users.
echo.
cd "WindowsFormsChat\ChatApp"
dotnet run
cd ..\..
pause
