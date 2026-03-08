# =============================================================================
# Sign release archive (.7z) for UtilityPDF auto-update
#
# Generates:
#   - {archive}.sha256  : SHA-256 hash (lowercase hex, no filename suffix)
#   - {archive}.sig     : RSA-4096 PKCS#1 SHA-256 signature (raw bytes)
#
# The .sha256 and .sig files must be uploaded as GitHub Release assets
# alongside the .7z archive. UpdaterBootstrap downloads all three and
# verifies hash + signature before extracting.
#
# Usage:
#   .\sign-archive.ps1 -ArchivePath "UtilityPDF-v1.8.0.7z" -PrivateKeyPath "update-signing-key.pem"
#
# Requires: OpenSSL (available in Git for Windows, or install separately)
# =============================================================================

param(
    [Parameter(Mandatory=$true, HelpMessage="Path to the .7z archive to sign")]
    [ValidateScript({ Test-Path $_ -PathType Leaf })]
    [string]$ArchivePath,

    [Parameter(Mandatory=$true, HelpMessage="Path to RSA-4096 private key (.pem)")]
    [ValidateScript({ Test-Path $_ -PathType Leaf })]
    [string]$PrivateKeyPath,

    [Parameter(Mandatory=$false, HelpMessage="Also sign the UpdaterBootstrap.exe binary")]
    [string]$UpdaterBinaryPath
)

$ErrorActionPreference = "Stop"

# Check OpenSSL availability
$openssl = Get-Command "openssl" -ErrorAction SilentlyContinue
if (-not $openssl) {
    # Try Git for Windows bundled OpenSSL
    $gitOpenssl = "C:\Program Files\Git\usr\bin\openssl.exe"
    if (Test-Path $gitOpenssl) {
        $openssl = $gitOpenssl
        Write-Host "[INFO] Using Git for Windows OpenSSL: $gitOpenssl" -ForegroundColor Cyan
    } else {
        Write-Error "OpenSSL not found. Install OpenSSL or Git for Windows."
        exit 1
    }
} else {
    $openssl = $openssl.Source
}

function Sign-File {
    param(
        [string]$FilePath,
        [string]$KeyPath,
        [string]$Label
    )

    Write-Host ""
    Write-Host "================================================================" -ForegroundColor Cyan
    Write-Host "  Signing: $Label" -ForegroundColor Cyan
    Write-Host "  File:    $FilePath" -ForegroundColor Cyan
    Write-Host "================================================================" -ForegroundColor Cyan

    $hashFile = "$FilePath.sha256"
    $sigFile = "$FilePath.sig"
    $sigRawFile = "$FilePath.sig.raw"

    # --- Step 1: SHA-256 hash ---
    Write-Host "[1/3] Computing SHA-256 hash..." -ForegroundColor Yellow
    $hash = (Get-FileHash -Path $FilePath -Algorithm SHA256).Hash.ToLower()
    $hash | Out-File -FilePath $hashFile -Encoding ASCII -NoNewline

    $fileSize = (Get-Item $FilePath).Length
    $fileSizeMB = [math]::Round($fileSize / 1MB, 2)

    Write-Host "  Hash: $hash" -ForegroundColor Green
    Write-Host "  Size: $fileSizeMB MB ($fileSize bytes)" -ForegroundColor Green
    Write-Host "  Written to: $hashFile" -ForegroundColor Green

    # --- Step 2: RSA-4096 PKCS#1 SHA-256 signature ---
    Write-Host "[2/3] Signing with RSA-4096 private key..." -ForegroundColor Yellow
    & $openssl dgst -sha256 -sign $KeyPath -out $sigRawFile $FilePath

    if ($LASTEXITCODE -ne 0) {
        Write-Error "OpenSSL signing failed (exit code: $LASTEXITCODE)"
        exit 1
    }

    # Keep signature as raw bytes (UpdaterBootstrap reads raw bytes via BCrypt)
    Copy-Item $sigRawFile $sigFile -Force
    Remove-Item $sigRawFile -Force

    $sigSize = (Get-Item $sigFile).Length
    Write-Host "  Signature size: $sigSize bytes (expected: 512 for RSA-4096)" -ForegroundColor Green
    Write-Host "  Written to: $sigFile" -ForegroundColor Green

    if ($sigSize -ne 512) {
        Write-Warning "Signature size is $sigSize bytes, expected 512 for RSA-4096. Verify the private key."
    }

    # --- Step 3: Verify signature ---
    Write-Host "[3/3] Verifying signature..." -ForegroundColor Yellow

    $pubKeyPath = [System.IO.Path]::ChangeExtension($KeyPath, ".pub.pem")
    if (-not (Test-Path $pubKeyPath)) {
        # Extract public key from private key
        & $openssl rsa -in $KeyPath -pubout -out $pubKeyPath 2>$null
    }

    if (Test-Path $pubKeyPath) {
        $verifyOutput = & $openssl dgst -sha256 -verify $pubKeyPath -signature $sigFile $FilePath 2>&1
        if ($verifyOutput -match "Verified OK") {
            Write-Host "  Verification: PASSED" -ForegroundColor Green
        } else {
            Write-Error "Signature verification FAILED: $verifyOutput"
            exit 1
        }
    } else {
        Write-Warning "Cannot verify — public key not found at: $pubKeyPath"
    }

    Write-Host ""
    return @{
        Hash = $hash
        HashFile = $hashFile
        SigFile = $sigFile
        FileSize = $fileSize
    }
}

# === Sign the archive ===
$archiveResult = Sign-File -FilePath $ArchivePath -KeyPath $PrivateKeyPath -Label "Release Archive"

# === Optionally sign the UpdaterBootstrap binary ===
if ($UpdaterBinaryPath -and (Test-Path $UpdaterBinaryPath)) {
    $binaryResult = Sign-File -FilePath $UpdaterBinaryPath -KeyPath $PrivateKeyPath -Label "UpdaterBootstrap Binary"
}

# === Summary ===
Write-Host ""
Write-Host "================================================================" -ForegroundColor Green
Write-Host "  SIGNING COMPLETE" -ForegroundColor Green
Write-Host "================================================================" -ForegroundColor Green
Write-Host ""
Write-Host "Files to upload as GitHub Release assets:" -ForegroundColor White
Write-Host "  1. $(Split-Path $ArchivePath -Leaf)" -ForegroundColor White
Write-Host "  2. $(Split-Path $ArchivePath -Leaf).sha256" -ForegroundColor White
Write-Host "  3. $(Split-Path $ArchivePath -Leaf).sig" -ForegroundColor White

if ($UpdaterBinaryPath -and (Test-Path $UpdaterBinaryPath)) {
    Write-Host "  4. $(Split-Path $UpdaterBinaryPath -Leaf)" -ForegroundColor White
    Write-Host "  5. $(Split-Path $UpdaterBinaryPath -Leaf).sha256" -ForegroundColor White
    Write-Host "  6. $(Split-Path $UpdaterBinaryPath -Leaf).sig" -ForegroundColor White
}

Write-Host ""
Write-Host "Verification command:" -ForegroundColor Cyan
Write-Host "  openssl dgst -sha256 -verify update-signing-key.pub.pem -signature `"$($archiveResult.SigFile)`" `"$ArchivePath`"" -ForegroundColor DarkGray
Write-Host ""