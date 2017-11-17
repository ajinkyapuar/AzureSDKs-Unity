@echo off

REM Set paths
set storagedir=..\..\azure-storage-net
set sampledir=..\Storage
set version=8.6.0

REM Build Release binaries
msbuild %storagedir%\Lib\Unity\Microsoft.WindowsAzure.Storage.Unity.csproj /t:Rebuild /p:Configuration=Release
msbuild %storagedir%\Lib\WindowsRuntime\Microsoft.WindowsAzure.Storage.RT.csproj /t:Rebuild /p:Configuration=Release

REM Copy binaries to sample
copy /y %storagedir%\Lib\Unity\bin\Release\*.dll %sampledir%\Assets\Plugins
copy /y %storagedir%\Lib\WindowsRuntime\bin\Release\*.dll %sampledir%\Assets\Plugins\WSA

REM Package
"C:\Program Files\Unity2017.1\Editor\Unity.exe" -batchmode -projectPath %cd%\%sampledir% -exportPackage Assets %cd%\azure-storage-unity-%version%.unitypackage -quit

echo "Packaging complete."