# Sign release binary using RSA-4096 private key
# Usage: .\sign-release.ps1 -BinaryPath "path\to\UpdaterBootstrap.exe" -PrivateKeyPath "update-signing-key.pem"

param(
    [Parameter(Mandatory=$true)]
    [string]$BinaryPath,

    [Parameter(Mandatory=$true)]
    [string]$PrivateKeyPath
)

# Generate SHA-256 hash
$hash = (Get-FileHash -Path $BinaryPath -Algorithm SHA256).Hash.ToLower()
$hash | Out-File -FilePath "$BinaryPath.sha256" -Encoding ASCII -NoNewline
Write-Host "SHA-256: $hash"

# Sign with RSA private key (requires OpenSSL)
& openssl dgst -sha256 -sign $PrivateKeyPath -out "$BinaryPath.sig.raw" $BinaryPath

# Convert to Base64 for easier embedding
$sigBytes = [System.IO.File]::ReadAllBytes("$BinaryPath.sig.raw")
$sigBase64 = [Convert]::ToBase64String($sigBytes)
$sigBase64 | Out-File -FilePath "$BinaryPath.sig" -Encoding ASCII -NoNewline

Remove-Item "$BinaryPath.sig.raw" -Force

Write-Host "Signature written to: $BinaryPath.sig"
Write-Host "Hash written to: $BinaryPath.sha256"
