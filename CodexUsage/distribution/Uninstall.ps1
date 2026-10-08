$ErrorActionPreference = 'Stop'
$expectedRoot = [IO.Path]::GetFullPath((Join-Path $env:LOCALAPPDATA 'Programs\VisentioCodexUsage'))
if (![string]::Equals([IO.Path]::GetFullPath($PSScriptRoot), $expectedRoot, [StringComparison]::OrdinalIgnoreCase)) {
    throw 'Run the uninstall script from the installed application folder'
}
$running = @(Get-CimInstance Win32_Process -Filter "Name='CodexUsage.exe'" | Where-Object {
    $_.ExecutablePath -and $_.ExecutablePath.StartsWith($expectedRoot + '\', [StringComparison]::OrdinalIgnoreCase)
})
foreach ($info in $running) {
    $process = Get-Process -Id $info.ProcessId -ErrorAction SilentlyContinue
    if ($process -and (!$process.CloseMainWindow() -or !$process.WaitForExit(10000))) { throw 'Close Codex Usage before uninstalling' }
}
foreach ($folder in @('Programs','DesktopDirectory','Startup')) {
    $link = Join-Path ([Environment]::GetFolderPath($folder)) 'Visentio Codex Usage.lnk'
    if (Test-Path -LiteralPath $link) {
        $shell = New-Object -ComObject WScript.Shell
        $shortcut = $shell.CreateShortcut($link)
        if ($shortcut.TargetPath.StartsWith($expectedRoot + '\', [StringComparison]::OrdinalIgnoreCase)) { Remove-Item -LiteralPath $link }
    }
}
Remove-Item -LiteralPath $expectedRoot -Recurse -Force
Write-Host 'Application removed. Your account configuration and encrypted sign-ins were preserved.'
