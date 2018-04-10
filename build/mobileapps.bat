@echo off

REM Set vars
set sdk=MobileApps
set version=4.0.2
set jsonversion=11.0.2
set packagename=azure-mobile-client-unity

call _pre.bat UWP

REM install the packages
nuget install Microsoft.Azure.Mobile.Client -Version %version% -OutputDirectory %nugetdir%
nuget install Newtonsoft.Json -Version %jsonversion% -OutputDirectory %nugetdir%
nuget install System.Security.Cryptography.Algorithms -Version 4.2.0 -OutputDirectory %nugetdir%

REM copy the proper DLLs to the package directory
copy /y "%nugetdir%\Microsoft.Azure.Mobile.Client.%version%\lib\netstandard1.4\Microsoft.Azure.Mobile.Client.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Newtonsoft.Json.%jsonversion%\lib\netstandard2.0\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\%sdk%

copy /y "%nugetdir%\Microsoft.Azure.Mobile.Client.%version%\lib\uap10.0\Microsoft.Azure.Mobile.Client.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\Newtonsoft.Json.9.0.1\lib\netstandard1.0\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Algorithms.4.2.0\runtimes\win\lib\netcore50\System.Security.Cryptography.Algorithms.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Encoding.4.0.0\runtimes\win\lib\netstandard1.3\System.Security.Cryptography.Encoding.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Primitives.4.0.0\lib\netstandard1.3\System.Security.Cryptography.Primitives.dll" %sampledir%\Assets\Plugins\%sdk%\WSA

call _post.bat
