@echo off

REM Set vars
set sdk=Storage
set version=9.1.1
set jsonversion=11.0.2
set packagename=azure-storage-unity

call _pre.bat UWP

REM install the package
nuget install WindowsAzure.Storage -Version %version% -OutputDirectory %nugetdir%
nuget install Newtonsoft.Json -Version %jsonversion% -OutputDirectory %nugetdir%
nuget install Newtonsoft.Json -Version 10.0.2 -OutputDirectory %nugetdir%
nuget install System.Runtime.Serialization.Primitives -Version 4.3.0 -OutputDirectory %nugetdir%

REM copy the proper DLLs to the package directory
copy /y "%nugetdir%\WindowsAzure.Storage.%version%\lib\netstandard1.3\Microsoft.WindowsAzure.Storage.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Newtonsoft.Json.%jsonversion%\lib\netstandard2.0\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\%sdk%

copy /y "%nugetdir%\WindowsAzure.Storage.%version%\lib\win8\Microsoft.WindowsAzure.Storage.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\Newtonsoft.Json.10.0.2\lib\portable-net45+win8+wpa81+wp8\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\%sdk%\WSA

call _post.bat
