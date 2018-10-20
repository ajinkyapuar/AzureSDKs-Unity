@echo off

pushd NuGet2Unity\src
dotnet build
dotnet run -- -n Microsoft.Azure.EventHubs -p ..\..\..\Microsoft.Azure.EventHubs -o ..\..\..\_UnityPackages -m
dotnet run -- -n Microsoft.Azure.Mobile.Client -p ..\..\..\Microsoft.Azure.Mobile.Client -o ..\..\..\_UnityPackages -m
dotnet run -- -n WindowsAzure.Storage -p ..\..\..\WindowsAzure.Storage -o ..\..\..\_UnityPackages -m
popd