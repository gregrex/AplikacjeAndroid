param(
    [string]$OutputPath = (Join-Path $HOME "AppFactory-Secrets\appfactory-upload.keystore"),
    [string]$Alias = "appfactory-upload",
    [int]$ValidityDays = 10000
)

$ErrorActionPreference = "Stop"

if (-not (Get-Command keytool -ErrorAction SilentlyContinue)) {
    throw "keytool was not found. Install a supported JDK and add its bin directory to PATH."
}

$directory = Split-Path $OutputPath -Parent
New-Item -ItemType Directory -Force -Path $directory | Out-Null

if (Test-Path $OutputPath) {
    throw "Keystore already exists: $OutputPath. Do not overwrite an upload key that may already be registered in Google Play."
}

Write-Host "Creating AppFactory upload keystore outside the repository."
Write-Host "You will be asked for passwords and certificate identity by keytool."
Write-Host "Do not paste passwords into source files, issue comments or CI logs."

& keytool `
    -genkeypair `
    -v `
    -keystore $OutputPath `
    -alias $Alias `
    -keyalg RSA `
    -keysize 4096 `
    -validity $ValidityDays

if ($LASTEXITCODE -ne 0 -or -not (Test-Path $OutputPath)) {
    throw "keytool did not create the keystore."
}

Write-Host ""
Write-Host "Keystore created: $OutputPath"
Write-Host "Alias: $Alias"
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Create two encrypted backups in separate locations."
Write-Host "2. Never commit the keystore or password files."
Write-Host "3. Use this file only as the Google Play upload key."
