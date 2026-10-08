[CmdletBinding()]
param([Parameter(Mandatory)][string]$Archive)
$ErrorActionPreference = 'Stop'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) ('CodexUsageDistributionCheck-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testRoot | Out-Null
Expand-Archive -LiteralPath $Archive -DestinationPath $testRoot
$bundle = Join-Path $testRoot 'Visentio-Codex-Usage'
$profile = Join-Path $testRoot 'profile'
$configDirectory = Join-Path $profile 'CodexUsage'
New-Item -ItemType Directory -Path $configDirectory -Force | Out-Null
$sentinel = Join-Path $configDirectory 'preserve.txt'
Set-Content -LiteralPath $sentinel -Value 'preserve-account-settings'
function Invoke-IsolatedScript([string]$script,[string]$arguments = '') {
    $start = New-Object Diagnostics.ProcessStartInfo
    $start.FileName = 'powershell.exe'
    $start.Arguments = '-NoProfile -ExecutionPolicy Bypass -File "' + $script + '" ' + $arguments
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.EnvironmentVariables['LOCALAPPDATA'] = $profile
    $start.EnvironmentVariables.Remove('PSModulePath')
    $process = [Diagnostics.Process]::Start($start)
    $stdout = $process.StandardOutput.ReadToEndAsync()
    $stderr = $process.StandardError.ReadToEndAsync()
    if (!$process.WaitForExit(30000)) { $process.Kill(); throw 'Installer test timed out' }
    $result = @{ exitCode = $process.ExitCode; output = $stdout.Result + $stderr.Result }
    $process.Dispose()
    return $result
}
$installer = Join-Path $bundle 'Install.ps1'
$flags = '-NoLaunch -NoShortcuts'
$first = Invoke-IsolatedScript $installer $flags
if ($first.exitCode -ne 0) { throw $first.output }
$manifestPath = Join-Path $bundle 'bundle.json'
$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
$root = Join-Path $profile 'Programs\VisentioCodexUsage'
$firstVersion = $manifest.version
if (!(Test-Path -LiteralPath (Join-Path $root ('versions\' + $firstVersion + '\CodexUsage.exe')))) { throw 'Initial installation missing' }
$repeat = Invoke-IsolatedScript $installer $flags
if ($repeat.exitCode -ne 0) { throw $repeat.output }
$versionParts = $firstVersion.Split('.')
$manifest.version = $versionParts[0] + '.' + $versionParts[1] + '.' + ([int]$versionParts[2] + 1)
$manifest | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $manifestPath -Encoding UTF8
$update = Invoke-IsolatedScript $installer $flags
if ($update.exitCode -ne 0) { throw $update.output }
if (!(Test-Path -LiteralPath (Join-Path $root ('versions\' + $manifest.version + '\CodexUsage.exe')))) { throw 'Updated installation missing' }
if (!(Test-Path -LiteralPath (Join-Path $root ('versions\' + $firstVersion)))) { throw 'Previous version lost' }
if ((Get-Content -LiteralPath $sentinel).Trim() -ne 'preserve-account-settings') { throw 'Configuration modified' }
$originalHash = $manifest.files[0].sha256
$manifest.files[0].sha256 = '0' * 64
$manifest | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $manifestPath -Encoding UTF8
$corrupt = Invoke-IsolatedScript $installer $flags
if ($corrupt.exitCode -eq 0) { throw 'Corrupt bundle was accepted' }
$manifest.files[0].sha256 = $originalHash
$manifest.files[0].path = '../outside.exe'
$manifest | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $manifestPath -Encoding UTF8
$traversal = Invoke-IsolatedScript $installer $flags
if ($traversal.exitCode -eq 0) { throw 'Unsafe path was accepted' }
$uninstall = Invoke-IsolatedScript (Join-Path $root 'Uninstall.ps1')
if ($uninstall.exitCode -ne 0) { throw $uninstall.output }
if (Test-Path -LiteralPath $root) { throw 'Uninstall left application installed' }
if (!(Test-Path -LiteralPath $sentinel)) { throw 'Uninstall removed configuration' }
Write-Output 'Passed: install, repeated install, version update, preserved settings, corrupt-file rejection, unsafe-path rejection, uninstall'
Write-Output "Isolated test directory: $testRoot"
