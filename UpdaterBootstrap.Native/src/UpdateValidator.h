#pragma once
#include <string>
#include <optional>
#include <vector>
#include <cstdint>

/// <summary>
/// Security validations for updates.
/// Provides SHA-256 hash verification and RSA-4096 signature validation.
/// </summary>
class UpdateValidator
{
public:
    /// <summary>
    /// Result of archive signature verification
    /// </summary>
    struct SignatureVerificationResult
    {
        bool hashValid = false;
        bool signatureValid = false;
        std::wstring errorMessage;

        bool IsValid() const { return hashValid && signatureValid && errorMessage.empty(); }
    };

    /// <summary>
    /// Validates the application directory
    /// </summary>
    static bool ValidateApplicationDirectory(const std::wstring& appDir);

    /// <summary>
    /// Validates the update directory (supports network shares)
    /// </summary>
    static bool ValidateUpdateDirectory(const std::wstring& updateDir);

    /// <summary>
    /// Post-update integrity check
    /// </summary>
    static bool VerifyUpdateIntegrity(const std::wstring& appDir);

    /// <summary>
    /// Checks available disk space
    /// </summary>
    static bool CheckDiskSpace(const std::wstring& path, uintmax_t requiredBytes);

    /// <summary>
    /// Computes SHA-256 hash of a file and returns it as lowercase hex string
    /// </summary>
    static std::wstring ComputeFileSha256(const std::wstring& filePath);

    /// <summary>
    /// Verifies SHA-256 hash of a downloaded archive against the expected value.
    /// Uses constant-time comparison to prevent timing attacks.
    /// </summary>
    static bool VerifyArchiveHash(const std::wstring& archivePath, const std::wstring& expectedHashHex);

    /// <summary>
    /// Verifies RSA-4096 signature of a file using the embedded public key.
    /// The signature must be raw RSA-PKCS1-SHA256 bytes.
    /// </summary>
    static bool VerifyArchiveSignature(const std::wstring& archivePath, const std::vector<uint8_t>& signatureBytes);

    /// <summary>
    /// Full verification: SHA-256 hash + RSA signature.
    /// Call this on the downloaded archive BEFORE extraction.
    /// </summary>
    static SignatureVerificationResult VerifyArchiveFull(
        const std::wstring& archivePath,
        const std::wstring& expectedHashHex,
        const std::vector<uint8_t>& signatureBytes);

private:
    static bool IsSystemDirectory(const std::wstring& path);
    static bool HasReadWriteAccess(const std::wstring& path);
    static bool ContainsSuspiciousCharacters(const std::wstring& path);

    /// <summary>
    /// Constant-time comparison to prevent timing attacks on hash values
    /// </summary>
    static bool ConstantTimeCompare(const std::wstring& a, const std::wstring& b);
};
