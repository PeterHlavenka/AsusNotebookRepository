[CmdletBinding()]
param([switch]$NoStartup, [switch]$NoLaunch, [switch]$NoDesktopShortcut, [switch]$NoShortcuts)
$ErrorActionPreference = 'Stop'
$bundle = $PSScriptRoot
$manifest = Get-Content -LiteralPath (Join-Path $bundle 'bundle.json') -Raw | ConvertFrom-Json
if ($manifest.version -notmatch '^\d+\.\d+\.\d+$') { throw 'Invalid bundle version' }
if (!$manifest.selfContained) {
    $dotnet = Get-Command dotnet.exe -ErrorAction SilentlyContinue
    if (!$dotnet) { throw 'Install .NET 10 Desktop Runtime (x64), or use the standalone bundle' }
    $runtimeList = & $dotnet.Source --list-runtimes
    if ($LASTEXITCODE -ne 0 -or !($runtimeList -match '^Microsoft\.WindowsDesktop\.App 10\.')) {
        throw 'Install .NET 10 Desktop Runtime (x64), or use the standalone bundle'
    }
}
$appSource = Join-Path $bundle 'app'
$expected = @{}
foreach ($file in $manifest.files) {
    if ($file.path -notmatch '^[a-zA-Z0-9_.-]+(/[a-zA-Z0-9_.-]+)*$' -or $file.path.Split('/') -contains '..' -or $file.path.Split('/') -contains '.' -or $expected.ContainsKey($file.path)) { throw 'Invalid bundle path' }
    $path = Join-Path $appSource $file.path
    if (!(Test-Path -LiteralPath $path -PathType Leaf)) { throw "Missing file: $($file.path)" }
    if ((Get-Item -LiteralPath $path).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'Links are not supported' }
    if ((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash -ne $file.sha256) { throw "Damaged file: $($file.path)" }
    $expected[$file.path] = $true
}
if (@(Get-ChildItem -LiteralPath $appSource -Recurse | Where-Object { $_.Attributes -band [IO.FileAttributes]::ReparsePoint }).Count -gt 0) { throw 'Links are not supported' }
$actual = @(Get-ChildItem -LiteralPath $appSource -File -Recurse)
if ($actual.Count -ne $expected.Count -or @($actual | Where-Object { !$expected.ContainsKey($_.FullName.Substring($appSource.Length + 1).Replace('\','/')) }).Count -gt 0) { throw 'Unexpected bundle files' }
if (!(Test-Path -LiteralPath (Join-Path $appSource 'CodexUsage.exe'))) { throw 'Missing application' }
$root = Join-Path $env:LOCALAPPDATA 'Programs\VisentioCodexUsage'
$destination = Join-Path $root ('versions\' + $manifest.version)
$existing = @(Get-CimInstance Win32_Process -Filter "Name='CodexUsage.exe'" | Where-Object {
    $_.ExecutablePath -and $_.ExecutablePath.StartsWith($root + '\', [StringComparison]::OrdinalIgnoreCase)
})
foreach ($processInfo in $existing) {
    $process = Get-Process -Id $processInfo.ProcessId -ErrorAction SilentlyContinue
    if ($process -and !$process.CloseMainWindow()) { throw 'Close Codex Usage before installing' }
    if ($process -and !$process.WaitForExit(10000)) { throw 'Codex Usage did not close; close it and try again' }
}
if (Test-Path -LiteralPath $destination) {
    foreach ($file in $manifest.files) {
        $installed = Join-Path $destination $file.path
        if (!(Test-Path -LiteralPath $installed) -or (Get-FileHash -LiteralPath $installed -Algorithm SHA256).Hash -ne $file.sha256) {
            throw 'This version is already installed with different contents. Use a new version number'
        }
    }
} else {
    $staging = Join-Path $root ('staging-' + [guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $staging -Force | Out-Null
    foreach ($file in $manifest.files) {
        $target = Join-Path $staging $file.path
        New-Item -ItemType Directory -Path (Split-Path $target) -Force | Out-Null
        Copy-Item -LiteralPath (Join-Path $appSource $file.path) -Destination $target
    }
    New-Item -ItemType Directory -Path (Split-Path $destination) -Force | Out-Null
    Move-Item -LiteralPath $staging -Destination $destination
}
$exe = Join-Path $destination 'CodexUsage.exe'
if (!$NoShortcuts) { $shell = New-Object -ComObject WScript.Shell }
function Add-Shortcut([string]$folder) {
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
    $shortcut = $shell.CreateShortcut((Join-Path $folder 'Visentio Codex Usage.lnk'))
    $shortcut.TargetPath = $exe
    $shortcut.WorkingDirectory = $destination
    $shortcut.Description = 'Codex usage miniwindow'
    $shortcut.Save()
}
if (!$NoShortcuts) { Add-Shortcut ([Environment]::GetFolderPath('Programs')) }
if (!$NoShortcuts -and !$NoDesktopShortcut) { Add-Shortcut ([Environment]::GetFolderPath('DesktopDirectory')) }
if (!$NoShortcuts -and !$NoStartup) { Add-Shortcut ([Environment]::GetFolderPath('Startup')) }
Copy-Item -LiteralPath (Join-Path $bundle 'Uninstall.ps1') -Destination (Join-Path $root 'Uninstall.ps1') -Force
Set-Content -LiteralPath (Join-Path $root 'installed-version.txt') -Value $manifest.version -Encoding UTF8
Write-Host "Installed Visentio Codex Usage $($manifest.version)"
Write-Host 'Accounts and sign-ins remain in your own Windows profile.'
if (!$NoLaunch) { Start-Process -FilePath $exe -WorkingDirectory $destination }
