[CmdletBinding()]
param()
$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path (Split-Path $PSScriptRoot)
$pluginPath = 'plugins/visentio-codex-usage'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) ('CodexUsageCheckoutCheck-' + [guid]::NewGuid().ToString('N'))
$snapshot = Join-Path $testRoot 'snapshot'

function Invoke-Git([string[]]$Arguments) {
    $output = & git @Arguments 2>&1
    if ($LASTEXITCODE -ne 0) { throw ($output -join "`n") }
    return $output
}

# Use a disposable repository/index: include uncommitted fixes without touching
# the maintainer's index, making a commit, or changing the current checkout.
$null = Invoke-Git @('init', '--quiet', $snapshot)
$gitDirectory = Join-Path $snapshot '.git'
$emptyAttributes = Join-Path $testRoot 'empty-attributes'
[IO.File]::WriteAllText($emptyAttributes, '')
$common = @('--git-dir', $gitDirectory, '-c', "core.attributesFile=$emptyAttributes", '-c', 'core.safecrlf=false')
$null = Invoke-Git ($common + @('--work-tree', $repositoryRoot, '-c', 'core.autocrlf=true', 'add', '--force', '--', '.gitattributes', $pluginPath))
$tree = Invoke-Git ($common + @('write-tree'))

foreach ($autoCrlf in @('false', 'true')) {
    $checkout = Join-Path $testRoot ('checkout-' + $autoCrlf)
    New-Item -ItemType Directory -Path $checkout | Out-Null
    $null = Invoke-Git ($common + @('--work-tree', $checkout, 'read-tree', [string]$tree))
    $null = Invoke-Git ($common + @('--work-tree', $checkout, '-c', "core.autocrlf=$autoCrlf", '-c', 'core.eol=lf', 'checkout-index', '--all', '--force'))
    $plugin = Join-Path $checkout $pluginPath
    $bundle = Join-Path $plugin 'skills/setup/bundle'
    $manifest = Get-Content -LiteralPath (Join-Path $bundle 'bundle.json') -Raw | ConvertFrom-Json
    $metadata = Get-Content -LiteralPath (Join-Path $plugin 'plugin.json') -Raw | ConvertFrom-Json
    if ($manifest.version -ne $metadata.version) { throw 'Checked-out app and plugin versions differ' }
    foreach ($file in $manifest.files) {
        $payloadPath = Join-Path $bundle ('app/' + $file.path)
        if ((Get-FileHash -LiteralPath $payloadPath -Algorithm SHA256).Hash -ne $file.sha256) {
            throw "Git checkout (core.autocrlf=$autoCrlf) changed payload bytes: $($file.path)"
        }
    }
    # Exercise the actual checked-out setup workflow, not a freshly rebuilt app.
    $archive = Join-Path $testRoot ("checkout-$autoCrlf.zip")
    Compress-Archive -LiteralPath $plugin -DestinationPath $archive
    & (Join-Path $PSScriptRoot 'Verify-TeamBundle.ps1') -Archive $archive -WorkspacePlugin
    Write-Output "Passed: clean Git checkout, all payload SHA-256 hashes and setup workflow (core.autocrlf=$autoCrlf)"
}
Write-Output "Isolated Git checkout test directory: $testRoot"
