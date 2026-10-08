[CmdletBinding()]
param([string]$OutputDirectory = (Join-Path $PSScriptRoot '..\dist'), [switch]$SelfContained)
$ErrorActionPreference = 'Stop'
$projectRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$output = [IO.Path]::GetFullPath($OutputDirectory)
[xml]$project = Get-Content -LiteralPath (Join-Path $projectRoot 'source\CodexUsage.csproj')
$version = [string]$project.Project.PropertyGroup.Version
if ($version -notmatch '^\d+\.\d+\.\d+$') { throw 'Invalid version' }
$work = Join-Path $output ('build-' + [guid]::NewGuid().ToString('N'))
$bundle = Join-Path $work 'Visentio-Codex-Usage'
$app = Join-Path $bundle 'app'
New-Item -ItemType Directory -Path $app -Force | Out-Null
dotnet publish (Join-Path $projectRoot 'source\CodexUsage.csproj') -c Release -r win-x64 --self-contained $SelfContained.IsPresent -o $app -p:DebugType=None -p:DebugSymbols=false --nologo
if ($LASTEXITCODE -ne 0) { throw 'Publish failed' }
$files = @(Get-ChildItem -LiteralPath $app -File -Recurse)
$manifest = @{ version = $version; architecture = 'win-x64'; selfContained = $SelfContained.IsPresent; files = @($files | ForEach-Object {
    @{ path = $_.FullName.Substring($app.Length + 1).Replace('\','/'); sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
}) }
$manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $bundle 'bundle.json') -Encoding UTF8
foreach ($name in @('Install.ps1','Install.cmd','Uninstall.ps1','START-HERE.txt')) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot $name) -Destination $bundle
}
Copy-Item -LiteralPath (Join-Path $projectRoot 'README.md') -Destination $bundle
$variant = if ($SelfContained) { 'standalone' } else { 'desktop-runtime' }
$zip = Join-Path $output ("Visentio-Codex-Usage-$version-win-x64-$variant.zip")
Compress-Archive -LiteralPath $bundle -DestinationPath $zip -Force
$hash = (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash.ToLowerInvariant()
"$hash  $([IO.Path]::GetFileName($zip))" | Set-Content -LiteralPath ($zip + '.sha256') -Encoding ASCII
Write-Output $zip
