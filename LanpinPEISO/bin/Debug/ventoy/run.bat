@echo off
setlocal enabledelayedexpansion

:: 检查必要文件
if not exist "panfu.txt" (
    echo panfu.txt 文件不存在！
    pause
    exit /b 1
)

if not exist "ISO.iso" (
    echo ISO.iso 文件不存在！
    pause
    exit /b 1
)

:: 读取并验证盘符
set /p drive_letter=<panfu.txt
set "drive_letter=!drive_letter:"=!"
set "drive_letter=!drive_letter: =!"

if not "!drive_letter:~1,1!"==":" (
    echo 盘符格式不正确，应为类似 F:\ 的格式
    pause
    exit /b 1
)

set "drive=!drive_letter:~0,1!"

:: 执行Ventoy2Disk
echo 正在执行 Ventoy2Disk 命令，目标盘符为 !drive!:
Ventoy2Disk.exe VTOYCLI /I /Drive:!drive!: /GPT /NOUSBCheck

if errorlevel 1 (
    echo 执行过程中出现错误！
    pause
    exit /b 1
)

:: 准备文件拷贝
echo.
echo Ventoy2Disk 已完成，开始拷贝 ISO 文件...
echo.

set "source_file=ISO.iso"
set "dest_file=!drive!:\ISO.iso"

:: 获取文件大小
for %%F in ("%source_file%") do (
    set "file_size=%%~zF"
    set "file_name=%%~nxF"
)
set /a file_size_mb=file_size/1048576

:: 方法1：直接使用copy命令（最可靠）
echo 正在拷贝 !file_name! (!file_size_mb! MB) 到 !drive!: 盘...
copy /y "%source_file%" "!dest_file!" >nul

if errorlevel 1 (
    echo 错误: 文件拷贝失败！
    pause
    exit /b 1
)

:: 方法2：替代方案 - 使用xcopy（如果需要进度显示）
:: echo 正在拷贝 !file_name! (!file_size_mb! MB) 到 !drive!: 盘...
:: xcopy "%source_file%" "!dest_file!" /Y /V /C /H /K >nul
:: if errorlevel 1 (
::     echo 错误: 文件拷贝失败！
::     pause
::     exit /b 1
:: )

:: 校验文件
echo 正在校验文件完整性...
fc /b "%source_file%" "!dest_file!" >nul
if errorlevel 1 (
    echo 错误: 文件校验失败！
    pause
    exit /b 1
)

:: 完成显示
echo.
echo [▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓] 100%%
echo.
echo 文件拷贝完成且校验通过！
echo.
echo 已完成 您可以关闭烧写窗口！
echo.

pause