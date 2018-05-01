@echo off

REM Set vars
set sdk=Microsoft.Azure.EventHubs
set version=2.0.0
set appauthversion=1.1.0-preview
set jsonversion=11.0.2
set packagename=azure-event-hubs

call _pre.bat UWP

REM install the packages
nuget install Microsoft.Azure.EventHubs -Version %version% -OutputDirectory %nugetdir%
nuget install Microsoft.Azure.Services.AppAuthentication -Version %appauthversion% -OutputDirectory %nugetdir%
nuget install Newtonsoft.Json -Version %jsonversion% -OutputDirectory %nugetdir%

REM copy the proper DLLs to the package directory
copy /y "%nugetdir%\Microsoft.Azure.EventHubs.%version%\lib\netstandard2.0\Microsoft.Azure.EventHubs.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Microsoft.Azure.Amqp.2.2.0\lib\netstandard1.3\Microsoft.Azure.Amqp.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\System.Diagnostics.DiagnosticSource.4.4.1\lib\netstandard1.3\System.Diagnostics.DiagnosticSource.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Microsoft.IdentityModel.Clients.ActiveDirectory.3.19.1\lib\netstandard1.3\Microsoft.IdentityModel.Clients.ActiveDirectory.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Microsoft.IdentityModel.Clients.ActiveDirectory.3.19.1\lib\netstandard1.3\Microsoft.IdentityModel.Clients.ActiveDirectory.Platform.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Microsoft.Azure.Services.AppAuthentication.%appauthversion%\lib\netstandard1.4\Microsoft.Azure.Services.AppAuthentication.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\System.IdentityModel.Tokens.Jwt.5.2.1\lib\netstandard1.4\System.IdentityModel.Tokens.Jwt.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Microsoft.IdentityModel.Tokens.5.2.1\lib\netstandard1.4\Microsoft.IdentityModel.Tokens.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Microsoft.IdentityModel.Logging.5.2.1\lib\netstandard1.4\Microsoft.IdentityModel.Logging.dll" %sampledir%\Assets\Plugins\%sdk%
copy /y "%nugetdir%\Newtonsoft.Json.%jsonversion%\lib\portable-net40+sl5+win8+wp8+wpa81\Newtonsoft.Json.dll" %sampledir%\Assets\Plugins\%sdk%

copy /y "%nugetdir%\Microsoft.Azure.EventHubs.%version%\lib\uap10.0\Microsoft.Azure.EventHubs.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\Microsoft.Azure.Amqp.2.2.0\lib\uap10.0\Microsoft.Azure.Amqp.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\System.Diagnostics.DiagnosticSource.4.4.1\lib\netstandard1.3\System.Diagnostics.DiagnosticSource.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Algorithms.4.3.0\runtimes\win\lib\netcore50\System.Security.Cryptography.Algorithms.dll" %sampledir%\Assets\Plugins\%sdk%\WSA
copy /y "%nugetdir%\System.Security.Cryptography.Primitives.4.3.0\lib\netstandard1.3\System.Security.Cryptography.Primitives.dll" %sampledir%\Assets\Plugins\%sdk%\WSA

call _post.bat
