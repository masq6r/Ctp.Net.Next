param(
    [string]$CtpSdkRoot = $env:CTP_SDK_ROOT,
    [string]$CtpSdkVersion = $env:CTP_SDK_VERSION
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$buildDir = Join-Path $scriptDir "build"

if ([string]::IsNullOrWhiteSpace($CtpSdkRoot)) {
    $CtpSdkRoot = Join-Path $scriptDir "ctp-sdk"
}

$configureArgs = @(
    '-S', $scriptDir,
    '-B', $buildDir,
    "-DCTP_SDK_ROOT=$CtpSdkRoot",
    "-DCTP_SDK_VERSION=$CtpSdkVersion"
)

& cmake @configureArgs
& cmake --build $buildDir --config Release
