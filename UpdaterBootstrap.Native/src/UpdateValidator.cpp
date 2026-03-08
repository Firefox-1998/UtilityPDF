#include "UpdateValidator.h"
#include "Logger.h"
#include <filesystem>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <algorithm>
#include <vector>
#include <shlobj.h>
#include <windows.h>
#include <bcrypt.h>

#pragma comment(lib, "bcrypt.lib")

namespace fs = std::filesystem;

// ============================================================================
// RSA-4096 Public Key (DER-encoded BCRYPT_RSAPUBLIC_BLOB)
// Generated once with: openssl genrsa -out update-signing-key.pem 4096
// Exported with: openssl rsa -in update-signing-key.pem -pubout -outform DER -out update-signing-key.der
// Then converted to byte array.
// KEEP THE PRIVATE KEY SECRET (CI/CD secrets only).
// ============================================================================
static const uint8_t kPublicKeyDer[] = {
    // TODO: Replace with your actual RSA-4096 public key DER bytes
    // Use the provided PowerShell script to generate this array
    0x30, 0x82, 0x02, 0x22 // placeholder — replace entirely
};
static const size_t kPublicKeyDerSize = sizeof(kPublicKeyDer);

// ============================================================================
// SHA-256 hash computation using Windows BCrypt
// ============================================================================
std::wstring UpdateValidator::ComputeFileSha256(const std::wstring& filePath)
{
    BCRYPT_ALG_HANDLE hAlgorithm = nullptr;
    BCRYPT_HASH_HANDLE hHash = nullptr;
    std::wstring result;

    try
    {
        if (!fs::exists(filePath))
        {
            Logger::LogError(L"SHA256_FILE_NOT_FOUND", L"File not found: " + filePath);
            return L"";
        }

        NTSTATUS status = BCryptOpenAlgorithmProvider(&hAlgorithm, BCRYPT_SHA256_ALGORITHM, nullptr, 0);
        if (!BCRYPT_SUCCESS(status))
        {
            Logger::LogError(L"SHA256_OPEN_ALG_FAILED", L"BCryptOpenAlgorithmProvider failed");
            return L"";
        }

        DWORD hashObjectSize = 0;
        DWORD dataSize = 0;
        status = BCryptGetProperty(hAlgorithm, BCRYPT_OBJECT_LENGTH,
            reinterpret_cast<PUCHAR>(&hashObjectSize), sizeof(DWORD), &dataSize, 0);
        if (!BCRYPT_SUCCESS(status))
        {
            BCryptCloseAlgorithmProvider(hAlgorithm, 0);
            return L"";
        }

        DWORD hashLength = 0;
        status = BCryptGetProperty(hAlgorithm, BCRYPT_HASH_LENGTH,
            reinterpret_cast<PUCHAR>(&hashLength), sizeof(DWORD), &dataSize, 0);
        if (!BCRYPT_SUCCESS(status))
        {
            BCryptCloseAlgorithmProvider(hAlgorithm, 0);
            return L"";
        }

        std::vector<uint8_t> hashObject(hashObjectSize);
        std::vector<uint8_t> hashValue(hashLength);

        status = BCryptCreateHash(hAlgorithm, &hHash, hashObject.data(), hashObjectSize, nullptr, 0, 0);
        if (!BCRYPT_SUCCESS(status))
        {
            BCryptCloseAlgorithmProvider(hAlgorithm, 0);
            return L"";
        }

        // Read file in 64KB chunks
        std::ifstream file(filePath, std::ios::binary);
        if (!file.is_open())
        {
            BCryptDestroyHash(hHash);
            BCryptCloseAlgorithmProvider(hAlgorithm, 0);
            Logger::LogError(L"SHA256_FILE_OPEN_FAILED", L"Cannot open file: " + filePath);
            return L"";
        }

        constexpr size_t bufferSize = 64 * 1024;
        std::vector<uint8_t> buffer(bufferSize);

        while (file.good())
        {
            file.read(reinterpret_cast<char*>(buffer.data()), bufferSize);
            std::streamsize bytesRead = file.gcount();
            if (bytesRead > 0)
            {
                status = BCryptHashData(hHash, buffer.data(), static_cast<ULONG>(bytesRead), 0);
                if (!BCRYPT_SUCCESS(status))
                {
                    BCryptDestroyHash(hHash);
                    BCryptCloseAlgorithmProvider(hAlgorithm, 0);
                    return L"";
                }
            }
        }

        status = BCryptFinishHash(hHash, hashValue.data(), hashLength, 0);
        if (!BCRYPT_SUCCESS(status))
        {
            BCryptDestroyHash(hHash);
            BCryptCloseAlgorithmProvider(hAlgorithm, 0);
            return L"";
        }

        // Convert to hex string
        std::wostringstream oss;
        oss << std::hex << std::setfill(L'0');
        for (DWORD i = 0; i < hashLength; i++)
        {
            oss << std::setw(2) << static_cast<int>(hashValue[i]);
        }
        result = oss.str();

        BCryptDestroyHash(hHash);
        BCryptCloseAlgorithmProvider(hAlgorithm, 0);
    }
    catch (const std::exception& ex)
    {
        if (hHash) { BCryptDestroyHash(hHash); }
        if (hAlgorithm) { BCryptCloseAlgorithmProvider(hAlgorithm, 0); }

        std::string errorMsg = ex.what();
        Logger::LogError(L"SHA256_EXCEPTION",
            L"Exception computing SHA-256: " + std::wstring(errorMsg.begin(), errorMsg.end()));
        return L"";
    }

    return result;
}

// ============================================================================
// Constant-time string comparison (prevents timing attacks)
// ============================================================================
bool UpdateValidator::ConstantTimeCompare(const std::wstring& a, const std::wstring& b)
{
    if (a.length() != b.length())
    {
        return false;
    }

    int diff = 0;
    for (size_t i = 0; i < a.length(); i++)
    {
        diff |= (a[i] ^ b[i]);
    }

    return diff == 0;
}

// ============================================================================
// Verify SHA-256 hash of downloaded archive
// ============================================================================
bool UpdateValidator::VerifyArchiveHash(const std::wstring& archivePath, const std::wstring& expectedHashHex)
{
    if (archivePath.empty() || expectedHashHex.empty())
    {
        Logger::LogError(L"VERIFY_HASH_INVALID_PARAMS", L"Invalid parameters for hash verification");
        return false;
    }

    std::wstring computedHash = ComputeFileSha256(archivePath);
    if (computedHash.empty())
    {
        Logger::LogError(L"VERIFY_HASH_COMPUTE_FAILED", L"Failed to compute SHA-256 hash");
        return false;
    }

    std::wstring expectedLower = expectedHashHex;
    std::transform(expectedLower.begin(), expectedLower.end(), expectedLower.begin(), ::towlower);

    bool isValid = ConstantTimeCompare(computedHash, expectedLower);

    if (isValid)
    {
        Logger::LogInfo(L"VERIFY_HASH_OK", L"SHA-256 hash verified successfully");
    }
    else
    {
        Logger::LogError(L"VERIFY_HASH_MISMATCH",
            L"SHA-256 mismatch — Expected: " + expectedLower + L", Computed: " + computedHash);
    }

    return isValid;
}

// ============================================================================
// Verify RSA-4096 PKCS#1 v1.5 signature using Windows BCrypt
// ============================================================================
bool UpdateValidator::VerifyArchiveSignature(const std::wstring& archivePath, const std::vector<uint8_t>& signatureBytes)
{
    if (archivePath.empty() || signatureBytes.empty())
    {
        Logger::LogError(L"VERIFY_SIG_INVALID_PARAMS", L"Invalid parameters for signature verification");
        return false;
    }

    BCRYPT_ALG_HANDLE hRsaAlg = nullptr;
    BCRYPT_KEY_HANDLE hKey = nullptr;
    bool isValid = false;

    try
    {
        // 1. Compute SHA-256 hash of the file
        std::wstring hashHex = ComputeFileSha256(archivePath);
        if (hashHex.empty())
        {
            return false;
        }

        // Convert hex hash to bytes
        std::vector<uint8_t> hashBytes(32);
        for (size_t i = 0; i < 32; i++)
        {
            std::wstring byteStr = hashHex.substr(i * 2, 2);
            hashBytes[i] = static_cast<uint8_t>(std::stoul(byteStr, nullptr, 16));
        }

        // 2. Open RSA algorithm provider
        NTSTATUS status = BCryptOpenAlgorithmProvider(&hRsaAlg, BCRYPT_RSA_ALGORITHM, nullptr, 0);
        if (!BCRYPT_SUCCESS(status))
        {
            Logger::LogError(L"VERIFY_SIG_OPEN_ALG_FAILED", L"BCryptOpenAlgorithmProvider(RSA) failed");
            return false;
        }

        // 3. Import the public key
        status = BCryptImportKeyPair(hRsaAlg, nullptr, BCRYPT_RSAPUBLIC_BLOB,
            &hKey, const_cast<PUCHAR>(kPublicKeyDer), static_cast<ULONG>(kPublicKeyDerSize), 0);

        if (!BCRYPT_SUCCESS(status))
        {
            Logger::LogError(L"VERIFY_SIG_IMPORT_KEY_FAILED",
                L"BCryptImportKeyPair failed — Ensure public key is in correct format");
            BCryptCloseAlgorithmProvider(hRsaAlg, 0);
            return false;
        }

        // 4. Verify the signature (PKCS1 padding with SHA-256)
        BCRYPT_PKCS1_PADDING_INFO paddingInfo = {};
        paddingInfo.pszAlgId = BCRYPT_SHA256_ALGORITHM;

        status = BCryptVerifySignature(
            hKey,
            &paddingInfo,
            hashBytes.data(),
            static_cast<ULONG>(hashBytes.size()),
            const_cast<PUCHAR>(signatureBytes.data()),
            static_cast<ULONG>(signatureBytes.size()),
            BCRYPT_PAD_PKCS1);

        isValid = BCRYPT_SUCCESS(status);

        if (isValid)
        {
            Logger::LogInfo(L"VERIFY_SIG_OK", L"RSA signature verified successfully");
        }
        else
        {
            Logger::LogError(L"VERIFY_SIG_FAILED",
                L"RSA signature verification FAILED (NTSTATUS: " + std::to_wstring(status) + L")");
        }

        BCryptDestroyKey(hKey);
        BCryptCloseAlgorithmProvider(hRsaAlg, 0);
    }
    catch (const std::exception& ex)
    {
        if (hKey) { BCryptDestroyKey(hKey); }
        if (hRsaAlg) { BCryptCloseAlgorithmProvider(hRsaAlg, 0); }

        std::string errorMsg = ex.what();
        Logger::LogError(L"VERIFY_SIG_EXCEPTION",
            L"Exception during signature verification: " + std::wstring(errorMsg.begin(), errorMsg.end()));
        return false;
    }

    return isValid;
}

// ============================================================================
// Full verification: SHA-256 + RSA signature
// ============================================================================
UpdateValidator::SignatureVerificationResult UpdateValidator::VerifyArchiveFull(
    const std::wstring& archivePath,
    const std::wstring& expectedHashHex,
    const std::vector<uint8_t>& signatureBytes)
{
    SignatureVerificationResult result;

    Logger::LogInfo(L"VERIFY_FULL_START", L"Starting full archive verification: " + archivePath);

    // Step 1: Verify SHA-256 hash
    if (!VerifyArchiveHash(archivePath, expectedHashHex))
    {
        result.errorMessage = L"SHA-256 hash mismatch — archive may be corrupted or tampered";
        Logger::LogError(L"VERIFY_FULL_HASH_FAIL", result.errorMessage);
        return result;
    }
    result.hashValid = true;

    // Step 2: Verify RSA signature
    if (!VerifyArchiveSignature(archivePath, signatureBytes))
    {
        result.errorMessage = L"RSA signature verification failed — archive is not authentic";
        Logger::LogError(L"VERIFY_FULL_SIG_FAIL", result.errorMessage);
        return result;
    }
    result.signatureValid = true;

    Logger::LogInfo(L"VERIFY_FULL_SUCCESS", L"Archive verification passed (hash + signature OK)");
    return result;
}

// ============================================================================
// Existing validation methods
// ============================================================================
bool UpdateValidator::ValidateApplicationDirectory(const std::wstring& appDir)
{
    try
    {
        if (appDir.empty() || !fs::exists(appDir))
        {
            Logger::LogError(L"VALIDATE_APP_DIR_NOT_EXISTS",
                L"Application directory does not exist: " + appDir);
            return false;
        }

        if (IsSystemDirectory(appDir))
        {
            Logger::LogError(L"SECURITY_APP_SYSTEM_DIR",
                L"Attempted update in system directory: " + appDir);
            return false;
        }

        if (!HasReadWriteAccess(appDir))
        {
            Logger::LogError(L"SECURITY_APP_NO_ACCESS",
                L"Insufficient permissions on application directory: " + appDir);
            return false;
        }

        Logger::LogInfo(L"VALIDATE_APP_DIR_SUCCESS",
            L"Application directory validated: " + appDir);
        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"VALIDATE_APP_DIR_ERROR",
            L"Error validating application directory: " +
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return false;
    }
}

bool UpdateValidator::ValidateUpdateDirectory(const std::wstring& updateDir)
{
    try
    {
        if (updateDir.empty())
        {
            Logger::LogError(L"UPDATE_DIR_EMPTY", L"Update directory not specified");
            return false;
        }

        if (!fs::exists(updateDir))
        {
            Logger::LogError(L"UPDATE_DIR_NOT_EXISTS",
                L"Update directory does not exist: " + updateDir);
            return false;
        }

        std::wstring fullPath = fs::absolute(updateDir).wstring();

        if (IsSystemDirectory(fullPath))
        {
            Logger::LogError(L"SECURITY_UPDATE_SYSTEM_DIR",
                L"Update directory in protected system path: " + fullPath);
            return false;
        }

        if (!HasReadWriteAccess(fullPath))
        {
            Logger::LogError(L"SECURITY_UPDATE_NO_READ",
                L"Insufficient read permissions on update directory: " + fullPath);
            return false;
        }

        if (ContainsSuspiciousCharacters(fullPath))
        {
            Logger::LogError(L"SECURITY_UPDATE_SUSPICIOUS",
                L"Update directory contains suspicious characters: " + fullPath);
            return false;
        }

        bool isNetworkPath = fullPath.starts_with(L"\\\\");
        Logger::LogInfo(L"UPDATE_DIR_VALIDATED",
            L"Update directory validated: " + fullPath +
            (isNetworkPath ? L" (Network Share)" : L" (Local Path)"));

        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"VALIDATE_UPDATE_DIR_ERROR",
            L"Error validating update directory: " +
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return false;
    }
}

bool UpdateValidator::VerifyUpdateIntegrity(const std::wstring& appDir)
{
    try
    {
        Logger::LogInfo(L"INTEGRITY_CHECK_START", L"Starting integrity verification");

        // Verify main executable
        fs::path exePath = fs::path(appDir) / L"UtilityPDF.exe";
        if (!fs::exists(exePath))
        {
            Logger::LogError(L"INTEGRITY_EXE_MISSING",
                L"Main executable missing: " + exePath.wstring());
            return false;
        }

        uintmax_t exeSize = fs::file_size(exePath);
        if (exeSize < 1024)
        {
            Logger::LogError(L"INTEGRITY_EXE_TOO_SMALL",
                L"Executable too small: " + std::to_wstring(exeSize) + L" bytes");
            return false;
        }

        // Verify configuration file
        fs::path configPath = fs::path(appDir) / L"config" / L"appsettings.json";
        if (!fs::exists(configPath))
        {
            Logger::LogError(L"INTEGRITY_CONFIG_MISSING",
                L"Configuration file missing: " + configPath.wstring());
            return false;
        }

        Logger::LogInfo(L"INTEGRITY_CHECK_PASSED", L"Integrity verification passed");
        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"INTEGRITY_CHECK_ERROR",
            L"Integrity check error: " +
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return false;
    }
}

bool UpdateValidator::CheckDiskSpace(const std::wstring& path, uintmax_t requiredBytes)
{
    try
    {
        if (path.starts_with(L"\\\\"))
        {
            Logger::LogWarning(L"DISK_SPACE_CHECK_SKIPPED",
                L"Disk space check skipped (UNC path): " + path);
            return true;
        }

        fs::space_info space = fs::space(path);

        if (space.available < requiredBytes)
        {
            Logger::LogError(L"DISK_SPACE_INSUFFICIENT",
                L"Insufficient space — Required: " + std::to_wstring(requiredBytes / 1024) +
                L" KB, Available: " + std::to_wstring(space.available / 1024) + L" KB");
            return false;
        }

        Logger::LogInfo(L"DISK_SPACE_OK",
            L"Sufficient disk space: " + std::to_wstring(space.available / 1024 / 1024) + L" MB");
        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogWarning(L"DISK_SPACE_CHECK_ERROR",
            L"Disk space check error: " +
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return true;
    }
}

bool UpdateValidator::IsSystemDirectory(const std::wstring& path)
{
    wchar_t systemDir[MAX_PATH];
    wchar_t windowsDir[MAX_PATH];
    wchar_t programFilesDir[MAX_PATH];
    wchar_t programFilesX86Dir[MAX_PATH];

    SHGetFolderPathW(nullptr, CSIDL_SYSTEM, nullptr, 0, systemDir);
    SHGetFolderPathW(nullptr, CSIDL_WINDOWS, nullptr, 0, windowsDir);
    SHGetFolderPathW(nullptr, CSIDL_PROGRAM_FILES, nullptr, 0, programFilesDir);
    SHGetFolderPathW(nullptr, CSIDL_PROGRAM_FILESX86, nullptr, 0, programFilesX86Dir);

    std::wstring lowerPath = path;
    std::transform(lowerPath.begin(), lowerPath.end(), lowerPath.begin(), ::towlower);

    std::vector<std::wstring> systemPaths = {
        std::wstring(systemDir),
        std::wstring(windowsDir),
        std::wstring(programFilesDir),
        std::wstring(programFilesX86Dir)
    };

    for (auto& sysPath : systemPaths)
    {
        std::transform(sysPath.begin(), sysPath.end(), sysPath.begin(), ::towlower);
        if (lowerPath.starts_with(sysPath))
        {
            return true;
        }
    }

    return false;
}

bool UpdateValidator::HasReadWriteAccess(const std::wstring& path)
{
    try
    {
        for (const auto& entry : fs::directory_iterator(path))
        {
            break;
        }

        fs::path testFile = fs::path(path) / L".write_test";
        std::ofstream ofs(testFile);
        if (ofs.is_open())
        {
            ofs << "test";
            ofs.close();
            fs::remove(testFile);
            return true;
        }

        return false;
    }
    catch (...)
    {
        return false;
    }
}

bool UpdateValidator::ContainsSuspiciousCharacters(const std::wstring& path)
{
    if (path.find(L"..") != std::wstring::npos) { return true; }
    if (path.find(L"~") != std::wstring::npos) { return true; }
    if (path.ends_with(L"\\") && path.length() == 3) { return true; }

    return false;
}
