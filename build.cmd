@echo off
del /s *.trx 2>NUL
REM Tools
dotnet tool update -g GitVersion.Tool
dotnet tool update -g dotnet-reportgenerator-globaltool
dotnet tool update -g dotnet-coverage
dotnet tool update -g docfx

REM Versioning
dotnet-gitversion -updateprojectfiles -output json -showvariable SemVer

REM Build and Test
dotnet test --coverage --report-trx --report-trx-filename "output.trx"
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet build -c Release
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet-coverage merge **\*.coverage --remove-input-files --output-format xml --output output/output.xml
reportgenerator -reports:"output/output.xml" -targetdir:"output" -reporttypes:"MarkdownSummaryGithub"
if %errorlevel% neq 0 exit /b %errorlevel%

REM Documentation
echo Generating Documentation
docfx docs/docfx.json --logLevel warning
REM if %errorlevel% neq 0 exit /b %errorlevel%
del docs\_site\*.ico
powershell -command compress-archive -Path src\AlphaVantageAPI\bin\release\*.nupkg -Destination output/package.zip -Force

REM Show Test Results
echo Test Results
set reg=executed=\"(\d+)\" passed=\"(\d+)\" failed=\"(\d+)\"
set test=":purple_circle: Tests: "
set pass=" :green_circle: Passed: "
set fail=" :red_circle: Failed: "
powershell -command "Get-ChildItem -Recurse -Filter *.trx | ForEach-Object {get-content $_.FullName | select-string '%reg%' | ForEach-Object { '%test%' + $_.Matches[0].Groups[1].Value + '%pass%' + $_.Matches[0].Groups[2].Value + '%fail%' + $_.Matches[0].Groups[3].Value}}"

REM Show Code Coverage
echo Code Coverage
powershell -command "get-content output\SummaryGithub.md | select-string '<summary>(.+%)</summary>' | ForEach-Object {$_.Matches[0].Groups[1].Value}"

echo See 'output/' folder for test results, code coverage, and package
echo done.