@echo off
setlocal enabledelayedexpansion

:: ����Ҫ�ļ�
if not exist "panfu.txt" (
    echo panfu.txt �ļ������ڣ�
    pause
    exit /b 1
)

if not exist "ISO.iso" (
    echo ISO.iso �ļ������ڣ�
    pause
    exit /b 1
)

:: ��ȡ����֤�̷�
set /p drive_letter=<panfu.txt
set "drive_letter=!drive_letter:"=!"
set "drive_letter=!drive_letter: =!"

if not "!drive_letter:~1,1!"==":" (
    echo �̷���ʽ����ȷ��ӦΪ���� F:\ �ĸ�ʽ
    pause
    exit /b 1
)

set "drive=!drive_letter:~0,1!"

:: ִ��Ventoy2Disk
echo ����ִ�� Ventoy2Disk ���Ŀ���̷�Ϊ !drive!:
Ventoy2Disk.exe VTOYCLI /I /Drive:!drive!: /GPT /NOUSBCheck

if errorlevel 1 (
    echo ִ�й����г��ִ���
    pause
    exit /b 1
)

:: ׼���ļ�����
echo.
echo Ventoy2Disk ����ɣ���ʼ���� ISO �ļ�...
echo.

set "source_file=ISO.iso"
set "dest_file=!drive!:\ISO.iso"

:: ��ȡ�ļ���С
for %%F in ("%source_file%") do (
    set "file_size=%%~zF"
    set "file_name=%%~nxF"
)
set /a file_size_mb=file_size/1048576

:: ����1��ֱ��ʹ��copy�����ɿ���
echo ���ڿ��� !file_name! (!file_size_mb! MB) �� !drive!: ��...
copy /y "%source_file%" "!dest_file!" >nul

if errorlevel 1 (
    echo ����: �ļ�����ʧ�ܣ�
    pause
    exit /b 1
)

:: ����2��������� - ʹ��xcopy�������Ҫ������ʾ��
:: echo ���ڿ��� !file_name! (!file_size_mb! MB) �� !drive!: ��...
:: xcopy "%source_file%" "!dest_file!" /Y /V /C /H /K >nul
:: if errorlevel 1 (
::     echo ����: �ļ�����ʧ�ܣ�
::     pause
::     exit /b 1
:: )

:: У���ļ�
echo ����У���ļ�������...
fc /b "%source_file%" "!dest_file!" >nul
if errorlevel 1 (
    echo ����: �ļ�У��ʧ�ܣ�
    pause
    exit /b 1
)

:: �����ʾ
echo.
echo [����������������������������������������] 100%%
echo.
echo �ļ����������У��ͨ����
echo.
echo ����� �����Թر���д���ڣ�
echo.

pause