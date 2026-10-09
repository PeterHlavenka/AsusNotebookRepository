param([string]$SdkRoot=$env:ANDROID_HOME,[string]$JavaHome=$env:JAVA_HOME,[string]$Gradle='gradle')
$ErrorActionPreference='Stop'
if (!$SdkRoot -or !(Test-Path -LiteralPath $SdkRoot)) { throw 'Nastav ANDROID_HOME nebo -SdkRoot na Android SDK (platform 35)' }
if ($JavaHome) { $env:JAVA_HOME=$JavaHome }
$env:ANDROID_HOME=$SdkRoot
Push-Location $PSScriptRoot
try {
    & $Gradle --no-daemon testDebugUnitTest lintDebug assembleDebug
    if ($LASTEXITCODE -ne 0) { throw 'Android build nebo kontrola selhaly' }
    New-Item -ItemType Directory -Force -Path dist | Out-Null
    $metadata=Get-Content -LiteralPath 'app\build\outputs\apk\debug\output-metadata.json' -Raw | ConvertFrom-Json
    $apkPath=Join-Path 'dist' ('CodexUsage-' + $metadata.elements[0].versionName + '.apk')
    Copy-Item -LiteralPath 'app\build\outputs\apk\debug\app-debug.apk' -Destination $apkPath -Force
    Get-FileHash -Algorithm SHA256 -LiteralPath $apkPath
} finally { Pop-Location }
