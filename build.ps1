$msbuild = join-path -path (Get-ItemProperty "HKLM:\software\Microsoft\MSBuild\ToolsVersions\14.0")."MSBuildToolsPath" -childpath "msbuild.exe"
&$msbuild StartupHelper\StartupHelper.csproj /t:Build /p:Configuration="Release.Net2"
&$msbuild StartupHelper\StartupHelper.csproj /t:Build /p:Configuration="Release.Net3.5"
&$msbuild StartupHelper\StartupHelper.csproj /t:Build /t:Package /t:Publish /p:Configuration="Release.Net4"