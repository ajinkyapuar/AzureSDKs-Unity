@echo off

pushd .

REM Set paths
set sampledir=..\EventHubs
set unityexe="C:\Program Files\Unity\Editor\Unity.exe"
set version=1.0.3
set nugetdir=nugeteventhubs

REM create output dir
rmdir /s /q %nugetdir%
mkdir %nugetdir%

REM install the package
nuget install Microsoft.Azure.EventHubs -Version %version% -OutputDirectory %nugetdir%

REM copy the proper DLLs to the package directory
copy /y "%nugetdir%\Microsoft.Azure.EventHubs.%version%\lib\net451\Microsoft.Azure.EventHubs.dll" %sampledir%\Assets\Plugins\EventHubs
copy /y "%nugetdir%\Microsoft.Azure.EventHubs.%version%\lib\uap10.0\Microsoft.Azure.EventHubs.dll" %sampledir%\Assets\Plugins\EventHubs\WSA
copy /y "%nugetdir%\Microsoft.Azure.Amqp.2.1.1\lib\net45\Microsoft.Azure.Amqp.dll" %sampledir%\Assets\Plugins\EventHubs
copy /y "%nugetdir%\Microsoft.Azure.Amqp.2.1.1\lib\uap10.0\Microsoft.Azure.Amqp.dll" %sampledir%\Assets\Plugins\EventHubs\WSA

%unityexe% -batchmode -projectPath %cd%\%sampledir% -exportPackage Assets %cd%\..\UnityPackages\azure-event-hubs-%version%.unitypackage -quit

REM remove working directories
rmdir /s /q %nugetdir%

echo "Packaging complete."

popd