@echo off
echo Starting Console Chat Consumer...
echo.
echo This application receives chat messages from Kafka.
echo Make sure Kafka is running before proceeding.
echo.
cd "ConsoleChat\ChatConsumer"
dotnet run
cd ..\..
pause
