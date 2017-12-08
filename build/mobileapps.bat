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
nuget install System.Security.Cryptography.Algorithms -Version 4.2.0 -OutputDirectory %nugetdir%
nuget install System.Security.Cryptography.Encoding -Version 4.0.0 -OutputDirectory %nugetdir%
nuget install System.Security.Cryptography.Primitives -Version 4.0.0 -OutputDirectory %nugetdir%

REM copy the proper DLLs to the package directory
copy /y "%nugetdir%\Microsoft.Azure.Mobile.Client.%version%\lib\net45\Microsoft.Azure.Mobile.Client.dll" %sampledir%\Assets\Plugins\MobileApps
copy /y "%nugetdir%\Microsoft.Azure.Mobile.Client.%version%\lib\uap10.0\Microsoft.Azure.Mobile.Client.dll" %sampledir%\Assets\Plugins\MobileApps\WSA
copy /y "%nugetdir%\Newtonsoft.Json.9.0.1\lib\portable-net40+sl5+wp80+win8+wpa81\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\MobileApps
copy /y "%nugetdir%\Newtonsoft.Json.9.0.1\lib\netstandard1.0\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\MobileApps\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Algorithms.4.2.0\runtimes\win\lib\netcore50\System.Security.Cryptography.Algorithms.dll" %sampledir%\Assets\Plugins\MobileApps\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Encoding.4.0.0\runtimes\win\lib\netstandard1.3\System.Security.Cryptography.Encoding.dll" %sampledir%\Assets\Plugins\MobileApps\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Primitives.4.0.0\lib\netstandard1.3\System.Security.Cryptography.Primitives.dll" %sampledir%\Assets\Plugins\MobileApps\WSA

%unityexe% -batchmode -projectPath %cd%\%sampledir% -exportPackage Assets %cd%\..\UnityPackages\azure-mobile-client-unity-%version%.unitypackage -quit

REM remove working directories
rmdir /s /q %nugetdir%

echo "Packaging complete."

popd
