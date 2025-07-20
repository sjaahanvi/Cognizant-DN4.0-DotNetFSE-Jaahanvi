@echo off
echo Starting Console Chat Producer...
echo.
echo This application sends chat messages to Kafka.
echo Make sure Kafka is running before proceeding.
echo.
cd "ConsoleChat\ChatProducer"
dotnet run
cd ..\..
pause
