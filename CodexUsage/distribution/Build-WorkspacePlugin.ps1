[CmdletBinding()]
param([string]$OutputDirectory = (Join-Path $PSScriptRoot '..\dist'))
$ErrorActionPreference = 'Stop'
$projectRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$repositoryRoot = Split-Path $projectRoot
$pluginRoot = Join-Path $repositoryRoot 'plugins\visentio-codex-usage'
$manifest = Get-Content -LiteralPath (Join-Path $pluginRoot 'plugin.json') -Raw | ConvertFrom-Json
[xml]$project = Get-Content -LiteralPath (Join-Path $projectRoot 'source\CodexUsage.csproj')
if ($manifest.version -ne [string]$project.Project.PropertyGroup.Version) { throw 'Plugin and app versions must match' }
$output = [IO.Path]::GetFullPath($OutputDirectory)
$staging = Join-Path $output ('plugin-build-' + [guid]::NewGuid().ToString('N'))
$app = Join-Path $staging 'app'
New-Item -ItemType Directory -Path $app -Force | Out-Null
dotnet publish (Join-Path $projectRoot 'source\CodexUsage.csproj') -c Release -r win-x64 --self-contained false -o $app -p:DebugType=None -p:DebugSymbols=false -p:IncludeSourceRevisionInInformationalVersion=false --nologo
if ($LASTEXITCODE -ne 0) { throw 'Plugin app publish failed' }
$allowed = @('CodexUsage.exe','CodexUsage.dll','CodexUsage.deps.json','CodexUsage.runtimeconfig.json')
$files = @(Get-ChildItem -LiteralPath $app -File)
if ($files.Count -ne $allowed.Count -or @(Get-ChildItem -LiteralPath $app -Directory).Count -gt 0 -or @($files | Where-Object { $_.Name -notin $allowed }).Count -gt 0) {
    throw 'Unexpected Windows payload; do not bundle configuration or credentials'
}
$bundle = Join-Path $pluginRoot 'skills\setup\bundle'
$targetApp = Join-Path $bundle 'app'
New-Item -ItemType Directory -Path $targetApp -Force | Out-Null
if (@(Get-ChildItem -LiteralPath $targetApp -File | Where-Object { $_.Name -notin $allowed }).Count -gt 0) {
    throw 'Remove unexpected files from the generated payload before rebuilding'
}
foreach ($file in $files) { Copy-Item -LiteralPath $file.FullName -Destination $targetApp -Force }
$bundleManifest = @{ version = $manifest.version; architecture = 'win-x64'; selfContained = $false; files = @($files | ForEach-Object {
    @{ path = $_.Name; sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
}) }
$bundleManifest | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $bundle 'bundle.json') -Encoding UTF8
foreach ($file in @('Install.ps1','Uninstall.ps1')) { Copy-Item -LiteralPath (Join-Path $PSScriptRoot $file) -Destination $bundle -Force }
$zip = Join-Path $output ("Visentio-Codex-Usage-Workspace-Plugin-$($manifest.version).zip")
Compress-Archive -LiteralPath $pluginRoot -DestinationPath $zip -Force
Write-Output $zip
