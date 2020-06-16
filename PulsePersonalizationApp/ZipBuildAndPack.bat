@echo off
cls

set destFolder="e:\NuGet"

echo Creating the module zip file..
powershell -command "Compress-Archive -Path .\Views\, .\ClientResources\, .\module.config -DestinationPath .\Modules\PulsePersonalizationApp\PulsePersonalizationApp -CompressionLevel Optimal -Force"
echo.

echo Creating NuGet package..
echo.
nuget pack PulsePersonalizationApp.nuspec -symbols
rem nuget pack PulsePersonalizationApp.csproj -Build

xcopy /s /y "PulsePersonalizationApp.*.nupkg" %destFolder%

echo.
echo.
echo Finished
