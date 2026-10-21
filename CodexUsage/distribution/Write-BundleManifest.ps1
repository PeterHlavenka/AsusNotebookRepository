[CmdletBinding()]
param([Parameter(Mandatory)][string]$BundleDirectory, [Parameter(Mandatory)][string]$Version, [switch]$SelfContained)
$ErrorActionPreference = 'Stop'
if ($Version -notmatch '^\d+\.\d+\.\d+$') { throw 'Invalid version' }
$app = [IO.Path]::GetFullPath((Join-Path $BundleDirectory 'app'))
# Hash the final distributed bytes, not an earlier publish/staging directory.
$files = @(Get-ChildItem -LiteralPath $app -File -Recurse | Sort-Object FullName)
if (!$files.Count) { throw 'Empty application payload' }
$manifest = [ordered]@{
    version = $Version
    architecture = 'win-x64'
    selfContained = $SelfContained.IsPresent
    files = @($files | ForEach-Object {
        [ordered]@{ path = $_.FullName.Substring($app.Length + 1).Replace('\','/'); sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
    })
}
# Stable ordering and UTF-8/LF are independent of the PowerShell host.
$json = ($manifest | ConvertTo-Json -Depth 6).Replace("`r`n", "`n") + "`n"
[IO.File]::WriteAllText((Join-Path $BundleDirectory 'bundle.json'), $json, (New-Object Text.UTF8Encoding($false)))
