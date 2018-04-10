pushd .

REM Set common vars
set sampledir=..\%sdk%
set unityexe="C:\Program Files\Unity\Editor\Unity.exe"
set nugetdir=nuget%sdk%

REM create output dir
rmdir /s /q %nugetdir%
mkdir %nugetdir%
mkdir %sampledir%\..\UnityPackages
mkdir %sampledir%\Assets\Plugins\%sdk%

if "%1"=="UWP" mkdir %sampledir%\Assets\Plugins\%sdk%\WSA
