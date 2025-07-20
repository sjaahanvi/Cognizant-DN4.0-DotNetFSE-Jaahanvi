@echo off
echo ============================================
echo         Kafka Setup Instructions
echo ============================================
echo.
echo This script provides instructions for setting up Kafka.
echo You need to download and install Kafka separately.
echo.
echo STEP 1: Download Kafka
echo ------------------------
echo 1. Go to https://kafka.apache.org/downloads
echo 2. Download the latest binary version for Scala 2.13
echo 3. Extract to C:\kafka (or update paths below)
echo.
echo STEP 2: Install Java
echo ---------------------
echo 1. Download Java 8 or higher from Oracle or OpenJDK
echo 2. Install and set JAVA_HOME environment variable
echo.
echo STEP 3: Start Services (Run these in separate command prompts)
echo -------------------------------------------------------------
echo.
echo Command Prompt 1 - Start Zookeeper:
echo cd C:\kafka
echo .\bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties
echo.
echo Command Prompt 2 - Start Kafka Server:
echo cd C:\kafka
echo .\bin\windows\kafka-server-start.bat .\config\server.properties
echo.
echo Command Prompt 3 - Create Topic:
echo cd C:\kafka
echo .\bin\windows\kafka-topics.bat --create --topic chat-topic --bootstrap-server localhost:9092 --partitions 3 --replication-factor 1
echo.
echo STEP 4: Verify Setup
echo --------------------
echo After starting Kafka, you should see:
echo - Zookeeper running on port 2181
echo - Kafka broker running on port 9092
echo - Topic 'chat-topic' created successfully
echo.
echo ============================================
echo Once Kafka is running, use the following scripts:
echo - build-all.bat: Build all applications
echo - run-console-producer.bat: Run console message sender
echo - run-console-consumer.bat: Run console message receiver
echo - run-windows-chat.bat: Run Windows Forms chat app
echo ============================================
echo.
pause
