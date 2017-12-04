@echo on

pushd .

REM Set paths
set sampledir=..\AppInsights
set unityexe="C:\Program Files\Unity\Editor\Unity.exe"
set version=2.4.0

REM dependency in net46 on System.DiagnosticsSource
set diagnosticsSourceVersion=4.4.0

REM create output dir
rmdir /s /q nuget
mkdir nuget

REM install the package
nuget install Microsoft.ApplicationInsights -Version %version% -OutputDirectory nuget

REM copy the proper DLLs to the package directory
copy /y "nuget\Microsoft.ApplicationInsights.%version%\lib\net46\Microsoft.ApplicationInsights.dll" %sampledir%\Assets\Plugins\AppInsights\net46
copy /y "nuget\Microsoft.ApplicationInsights.%version%\lib\uap10.0\Microsoft.ApplicationInsights.dll" %sampledir%\Assets\Plugins\AppInsights\WSA
copy /y "nuget\System.Diagnostics.DiagnosticSource.%diagnosticsSourceVersion%\lib\net46\System.Diagnostics.DiagnosticSource.dll" %sampledir%\Assets\Plugins\AppInsights\net46


%unityexe% -batchmode -projectPath %cd%\%sampledir% -exportPackage Assets %cd%\azure-application-insights-%version%.unitypackage -quit

REM remove working directories
rmdir /s /q nuget
rmdir /s /q package
rmdir /s /q etc
popd