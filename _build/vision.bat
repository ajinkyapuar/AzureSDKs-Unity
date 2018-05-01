@echo off

REM Set vars
set sdk=Microsoft.ProjectOxford.Vision
set version=1.0.393
set jsonversion=11.0.2
set packagename=projectoxford-vision

call _pre.bat

REM install the packages
nuget install Microsoft.ProjectOxford.Vision -Version %version% -OutputDirectory %nugetdir%
nuget install Newtonsoft.Json -Version %jsonversion% -OutputDirectory %nugetdir%

REM copy the proper DLLs to the package directory
copy /y "%nugetdir%\Microsoft.ProjectOxford.Vision.%version%\lib\portable-net45+win+wpa81+wp80+MonoAndroid10+xamarinios10+MonoTouch10\Microsoft.ProjectOxford.Vision.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Newtonsoft.Json.%jsonversion%\lib\portable-net40+sl5+win8+wp8+wpa81\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\%sdk%

call _post.bat
