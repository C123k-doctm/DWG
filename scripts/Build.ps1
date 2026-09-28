param(
    [ValidateSet('Debug','Release')][string]$Configuration = 'Debug',
    [switch]$Publish
)
$ErrorActionPreference = 'Stop'
$taskRoot = Split-Path $PSScriptRoot -Parent
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$msbuild = $null
if (Test-Path -LiteralPath $vswhere) {
    $msbuild = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
}
if (!$msbuild) {
    $command = Get-Command MSBuild.exe -ErrorAction SilentlyContinue
    if ($command) { $msbuild = $command.Source }
}
if (!$msbuild) { throw 'Install Visual Studio / Build Tools with .NET desktop build tools before building. The prebuilt launcher does not require Visual Studio.' }
$outputDirectory = Join-Path $taskRoot 'artifacts\development'
New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null
$project = Join-Path $taskRoot 'Teigha_Learn_Cad\Teigha_Learn_Cad\Teigha_Learn_Cad.csproj'
& $msbuild $project /t:Build "/p:Configuration=$Configuration" /p:Platform=AnyCPU "/p:OutputPath=$outputDirectory\" /nologo /v:minimal
if ($LASTEXITCODE -ne 0) { throw "Build failed with exit code $LASTEXITCODE. See DEPENDENCIES.md for prerequisites." }
if ($Publish) {
    $appDirectory = Join-Path $taskRoot 'app'
    foreach ($name in @('Teigha_Learn_Cad.exe', 'Teigha_Learn_Cad.exe.config', 'TeighaCad_Bll.dll', 'ClassLibrary1.dll')) {
        Copy-Item -LiteralPath (Join-Path $outputDirectory $name) -Destination $appDirectory -Force
    }
    $entries = foreach ($file in Get-ChildItem -LiteralPath $appDirectory -File | Where-Object Name -ne 'manifest.json') {
        [ordered]@{ path=$file.Name; size=$file.Length; sha256=(Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash }
    }
    [ordered]@{ configuration=$Configuration; files=@($entries) } | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $appDirectory 'manifest.json') -Encoding UTF8
    Write-Host 'Updated app snapshot. Run the startup check before committing it.'
}
Write-Host "Build output: $outputDirectory"
