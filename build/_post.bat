REM Package it up
%unityexe% -batchmode -projectPath %cd%\%sampledir% -exportPackage Assets %cd%\..\UnityPackages\%packagename%-%version%.unitypackage -quit

REM remove working directories
rmdir /s /q %nugetdir%

echo.
echo -- Packaging complete --
echo.

popd
