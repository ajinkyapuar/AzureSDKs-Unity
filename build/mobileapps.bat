@echo off

pushd .

REM Set paths
set sampledir=..\MobileApps
set unityexe="C:\Program Files\Unity\Editor\Unity.exe"
set version=4.0.2
set nugetdir=nugetmobileapps

REM create output dir
rmdir /s /q %nugetdir%
mkdir %nugetdir%

REM install the package
nuget install Microsoft.Azure.Mobile.Client -Version %version% -OutputDirectory %nugetdir%

REM copy the proper DLLs to the package directory
copy /y "%nugetdir%\Microsoft.Azure.Mobile.Client.%version%\lib\net45\Microsoft.Azure.Mobile.Client.dll" %sampledir%\Assets\Plugins\MobileApps
copy /y "%nugetdir%\Microsoft.Azure.Mobile.Client.%version%\lib\uap10.0\Microsoft.Azure.Mobile.Client.dll" %sampledir%\Assets\Plugins\MobileApps\WSA
copy /y "%nugetdir%\Newtonsoft.Json.9.0.1\lib\portable-net40+sl5+wp80+win8+wpa81\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\MobileApps
copy /y "%nugetdir%\Newtonsoft.Json.9.0.1\lib\netstandard1.0\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\MobileApps\WSA

%unityexe% -batchmode -projectPath %cd%\%sampledir% -exportPackage Assets %cd%\..\UnityPackages\azure-mobile-client-unity-%version%.unitypackage -quit

REM remove working directories
rmdir /s /q %nugetdir%

echo "Packaging complete."

popd
