@echo off

REM Set paths
set storagedir=..\..\azure-storage-net
set sampledir=..\Storage

REM Build Release binaries
msbuild %storagedir%\Lib\Unity\Microsoft.WindowsAzure.Storage.Unity.csproj /t:Rebuild /p:Configuration=Release
msbuild %storagedir%\Lib\WindowsRuntime\Microsoft.WindowsAzure.Storage.RT.csproj /t:Rebuild /p:Configuration=Release

REM Remove any old artifacts
rmdir /s /q Packaging

REM Create the directory structure
mkdir Packaging\Assets\Plugins\WSA
mkdir Packaging\Assets\Sample
mkdir Packaging\Assets\StreamingAssets

REM Copy the binaries and sample
copy /y %storagedir%\Lib\Unity\bin\Release\*.dll Packaging\Assets\Plugins
copy /y %storagedir%\Lib\WindowsRuntime\bin\Release\*.dll Packaging\Assets\Plugins\WSA
copy /y %sampledir%\Assets\Sample\*.* Packaging\Assets\Sample
copy /y %sampledir%\Assets\StreamingAssets\*.* Packaging\Assets\StreamingAssets

REM Package
"C:\Program Files\Unity2017.1\Editor\Unity.exe" -batchmode -projectPath %cd%\Packaging -exportPackage Assets %cd%\azure-storage-unity.unitypackage -quit

echo "Packaging complete."