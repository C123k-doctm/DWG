param(
    [string]$BinaryDirectory = '',
    [string]$OutputDirectory = ''
)
$ErrorActionPreference = 'Stop'
$taskRoot = Split-Path $PSScriptRoot -Parent
if (!$BinaryDirectory) { $BinaryDirectory = Join-Path $taskRoot 'app' }
if (!$OutputDirectory) { $OutputDirectory = Join-Path $taskRoot 'artifacts\desktop' }

foreach ($manifestPath in @((Join-Path $taskRoot 'vendor\manifest.json'), (Join-Path $BinaryDirectory 'manifest.json'))) {
    if (!(Test-Path -LiteralPath $manifestPath)) {
        if ($manifestPath -eq (Join-Path $taskRoot 'vendor\manifest.json') -or $BinaryDirectory -eq (Join-Path $taskRoot 'app')) {
            throw "Missing manifest: $manifestPath. Clone the complete repository."
        }
        continue
    }
    $manifest = Get-Content -LiteralPath $manifestPath -Raw -Encoding UTF8 | ConvertFrom-Json
    foreach ($entry in $manifest.files) {
        $file = Join-Path (Split-Path $manifestPath -Parent) $entry.path
        if (!(Test-Path -LiteralPath $file) -or (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash -ne $entry.sha256) {
            throw "Missing or changed bundled file: $file. Restore the file from Git before starting."
        }
    }
}

New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
Copy-Item -LiteralPath (Get-ChildItem (Join-Path $taskRoot 'vendor\runtime') -File).FullName -Destination $OutputDirectory -Force
$templateDirectory = Join-Path $OutputDirectory 'Template'
New-Item -ItemType Directory -Path $templateDirectory -Force | Out-Null
Copy-Item -LiteralPath (Get-ChildItem (Join-Path $taskRoot 'vendor\templates') -File).FullName -Destination $templateDirectory -Force
foreach ($name in @('Teigha_Learn_Cad.exe', 'Teigha_Learn_Cad.exe.config', 'TeighaCad_Bll.dll', 'ClassLibrary1.dll')) {
    $source = Join-Path $BinaryDirectory $name
    if (!(Test-Path -LiteralPath $source)) { throw "Missing application file: $source. Build the project first." }
    if ([IO.Path]::GetFullPath($source) -ne [IO.Path]::GetFullPath((Join-Path $OutputDirectory $name))) {
        Copy-Item -LiteralPath $source -Destination $OutputDirectory -Force
    }
}
return [IO.Path]::GetFullPath($OutputDirectory)
