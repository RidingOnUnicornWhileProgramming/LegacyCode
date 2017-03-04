@echo off

rem remove all files, keep folders
del C:\devenv\*.* /q
del C:\devenv\config\*.* /q

copy ..\*.* C:\devenv
copy ..\config\*.* C:\devenv\config
