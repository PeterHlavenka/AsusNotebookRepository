[CmdletBinding()]
param([switch]$CheckOnly, [switch]$NoLaunch, [switch]$NoShortcuts)
$ErrorActionPreference = 'Stop'
if ([Environment]::OSVersion.Platform -ne [PlatformID]::Win32NT -or [Environment]::Is64BitOperatingSystem -eq $false) {
    throw 'Setup requires local Windows x64 execution'
}
$bundle = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\bundle'))
$manifestPath = Join-Path $bundle 'bundle.json'
if (!(Test-Path -LiteralPath $manifestPath)) { throw 'The plugin is missing its Windows bundle. Contact the maintainer' }
$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
$dotnetPath = Join-Path ([Environment]::GetFolderPath('ProgramFiles')) 'dotnet\dotnet.exe'
$runtimeAvailable = $false
if (Test-Path -LiteralPath $dotnetPath) {
    $runtimeAvailable = [bool]((& $dotnetPath --list-runtimes) -match '^Microsoft\.WindowsDesktop\.App 10\.')
}
$codexPath = $null
$codexDirectory = Join-Path $env:LOCALAPPDATA 'OpenAI\Codex\bin'
if (Test-Path -LiteralPath $codexDirectory) {
    $codexPath = Get-ChildItem -LiteralPath $codexDirectory -Directory |
        ForEach-Object { Join-Path $_.FullName 'codex.exe' } |
        Where-Object { Test-Path -LiteralPath $_ -PathType Leaf } |
        Sort-Object { (Get-Item -LiteralPath $_).LastWriteTimeUtc } -Descending |
        Select-Object -First 1
}
if (!$codexPath) {
    foreach ($directory in ($env:PATH -split [IO.Path]::PathSeparator)) {
        if ([string]::IsNullOrWhiteSpace($directory)) { continue }
        $candidate = Join-Path $directory.Trim('"') 'codex.exe'
        if (Test-Path -LiteralPath $candidate -PathType Leaf) { $codexPath = $candidate; break }
    }
}
$installationRoot = Join-Path $env:LOCALAPPDATA 'Programs\VisentioCodexUsage'
if ($CheckOnly) {
    [pscustomobject]@{
        version = $manifest.version
        windowsX64 = $true
        desktopRuntime10 = $runtimeAvailable
        codexAvailable = [bool]$codexPath
        bundledExecutable = Test-Path -LiteralPath (Join-Path $bundle 'app\CodexUsage.exe')
        installDirectory = $installationRoot
    } | ConvertTo-Json
    return
}
if (!$runtimeAvailable) { throw 'Install .NET 10 Desktop Runtime (x64) before running setup' }
if (!$codexPath) { throw 'Install Codex desktop and sign in before running setup' }
$otherCopies = @(Get-CimInstance Win32_Process -Filter "Name='CodexUsage.exe'" | Where-Object {
    $_.ExecutablePath -and !$_.ExecutablePath.StartsWith($installationRoot + '\', [StringComparison]::OrdinalIgnoreCase)
})
if (!$NoLaunch -and $otherCopies.Count -gt 0) {
    throw 'Codex Usage is already running from another installation. Close that miniwindow and run setup again'
}
$start = New-Object Diagnostics.ProcessStartInfo
$start.FileName = Join-Path $env:SystemRoot 'System32\WindowsPowerShell\v1.0\powershell.exe'
$start.Arguments = '-NoProfile -ExecutionPolicy Bypass -File "' + (Join-Path $bundle 'Install.ps1') + '"'
if ($NoLaunch) { $start.Arguments += ' -NoLaunch' }
if ($NoShortcuts) { $start.Arguments += ' -NoShortcuts' }
$start.UseShellExecute = $false
$start.CreateNoWindow = $true
$start.RedirectStandardOutput = $true
$start.RedirectStandardError = $true
$start.EnvironmentVariables.Remove('PSModulePath')
$process = [Diagnostics.Process]::Start($start)
$output = $process.StandardOutput.ReadToEndAsync()
$errors = $process.StandardError.ReadToEndAsync()
if (!$process.WaitForExit(60000)) { $process.Kill(); throw 'Installation timed out' }
if ($process.ExitCode -ne 0) { throw ($output.Result + $errors.Result) }
$process.Dispose()
$exe = Join-Path $installationRoot ('versions\' + $manifest.version + '\CodexUsage.exe')
if (!(Test-Path -LiteralPath $exe)) { throw 'Installed executable was not found' }
$running = @(Get-CimInstance Win32_Process -Filter "Name='CodexUsage.exe'" | Where-Object ExecutablePath -eq $exe)
[pscustomobject]@{
    installed = $true
    version = $manifest.version
    executable = $exe
    running = ($running.Count -gt 0)
    startupConfigured = !$NoShortcuts
} | ConvertTo-Json
