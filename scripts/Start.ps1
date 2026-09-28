param([switch]$Build, [switch]$Check)
$ErrorActionPreference = 'Stop'
$taskRoot = Split-Path $PSScriptRoot -Parent
if (![Environment]::Is64BitOperatingSystem) { throw 'This application requires 64-bit Windows.' }
$crtDirectories = @(Get-ChildItem -LiteralPath (Join-Path $env:windir 'WinSxS') -Directory -Filter 'amd64_microsoft.vc90.crt_*' -ErrorAction SilentlyContinue)
if ($crtDirectories.Count -eq 0) {
    throw 'Install Microsoft Visual C++ 2008 SP1 Redistributable (x64) once, then start again: https://www.microsoft.com/en-us/download/details.aspx?id=26368'
}
if ($Build) {
    & (Join-Path $PSScriptRoot 'Build.ps1')
    $directory = & (Join-Path $PSScriptRoot 'Prepare-Run.ps1') -BinaryDirectory (Join-Path $taskRoot 'artifacts\development') -OutputDirectory (Join-Path $taskRoot 'artifacts\development')
} else {
    $directory = & (Join-Path $PSScriptRoot 'Prepare-Run.ps1')
}
$executable = Join-Path $directory 'Teigha_Learn_Cad.exe'
if ($Check) {
    $report = Join-Path $directory 'startup-check.txt'
    if (Test-Path -LiteralPath $report) { Remove-Item -LiteralPath $report }
    $process = Start-Process -FilePath $executable -ArgumentList '--startup-check' -WorkingDirectory $directory -WindowStyle Hidden -PassThru
    $null = $process.Handle
    if (!$process.WaitForExit(45000)) {
        Stop-Process -Id $process.Id -ErrorAction SilentlyContinue
        throw 'Startup check timed out. A missing native dependency or an unexpected dialog may be blocking startup.'
    }
    $process.Refresh()
    if ($process.ExitCode -ne 0 -or !(Test-Path -LiteralPath $report)) { throw "Startup check failed. See $report and Windows Application event logs." }
    Get-Content -LiteralPath $report -Encoding UTF8
} else {
    Start-Process -FilePath $executable -WorkingDirectory $directory
}
