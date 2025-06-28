@echo off
setlocal
set "DB_NAME=GameServer"
set "DB_USER=gameserver"
set "DB_PASSWORD=YourStrong@Passw0rd"
set "DB_HOST=localhost"
set "DB_PORT=13306"
set "SQL_FILE=test_data.sql"
cd ..\sql
mysql -h %DB_HOST% -P %DB_PORT% -u %DB_USER% -p%DB_PASSWORD% %DB_NAME% < %SQL_FILE%
if %ERRORLEVEL% neq 0 (
    echo Failed to setup test data the database.
    exit /b %ERRORLEVEL%
)
echo Database setup test data successfully.
endlocal
