param(
    [Parameter(Mandatory = $true)][string]$KeystorePath,
    [string]$OutputPath = (Join-Path $HOME "AppFactory-Secrets\android-keystore-base64.txt")
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $KeystorePath)) {
    throw "Keystore not found: $KeystorePath"
}

$inputInfo = Get-Item $KeystorePath
if ($inputInfo.Length -le 0) {
    throw "Keystore is empty."
}

$directory = Split-Path $OutputPath -Parent
New-Item -ItemType Directory -Force -Path $directory | Out-Null

$bytes = [System.IO.File]::ReadAllBytes($KeystorePath)
$base64 = [Convert]::ToBase64String($bytes)
Set-Content -Path $OutputPath -Value $base64 -Encoding ASCII -NoNewline

Write-Host "Base64 keystore saved outside the repository: $OutputPath"
Write-Host "GitHub secret name: ANDROID_KEYSTORE_BASE64"
Write-Host ""
Write-Host "Also configure:"
Write-Host "- ANDROID_KEYSTORE_PASSWORD"
Write-Host "- ANDROID_KEY_PASSWORD"
Write-Host "- ANDROID_KEY_ALIAS"
Write-Host ""
Write-Host "After setting the secret, securely delete the temporary base64 text file."
Write-Host "Do not paste its contents into issues, commits, chat logs or workflow output."
