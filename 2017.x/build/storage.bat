@echo off

REM Set paths
set storagedir=..\..\azure-storage-net
set sampledir=..\Storage
set unityexe="C:\Program Files\Unity\Editor\Unity.exe"
set version=8.6.0

REM Build Release binaries
msbuild %storagedir%\Lib\Unity\Microsoft.WindowsAzure.Storage.Unity.csproj /t:Rebuild /p:Configuration=Release
msbuild %storagedir%\Lib\WindowsRuntime\Microsoft.WindowsAzure.Storage.RT.csproj /t:Rebuild /p:Configuration=Release

REM Copy binaries to sample
copy /y %storagedir%\Lib\Unity\bin\Release\*.dll %sampledir%\Assets\Plugins\Storage
copy /y %storagedir%\Lib\WindowsRuntime\bin\Release\*.dll %sampledir%\Assets\Plugins\Storage\WSA

REM Package
%unityexe% -batchmode -projectPath %cd%\%sampledir% -exportPackage Assets %cd%\..\UnityPackages\azure-storage-unity-%version%.unitypackage -quit

echo "Packaging complete."
