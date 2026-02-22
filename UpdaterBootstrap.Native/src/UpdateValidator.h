#pragma once
#include <string>
#include <optional>

/// <summary>
/// 🔒 Validazioni di sicurezza per aggiornamenti
/// </summary>
class UpdateValidator
{
public:
    /// <summary>
    /// 🔒 Valida directory applicazione
    /// </summary>
    static bool ValidateApplicationDirectory(const std::wstring& appDir);

    /// <summary>
    /// 🔒 Valida directory aggiornamenti (supporta network share)
    /// </summary>
    static bool ValidateUpdateDirectory(const std::wstring& updateDir);

    /// <summary>
    /// 🔒 Verifica integrità post-aggiornamento
    /// </summary>
    static bool VerifyUpdateIntegrity(const std::wstring& appDir);

    /// <summary>
    /// 💿 Verifica spazio disco disponibile
    /// </summary>
    static bool CheckDiskSpace(const std::wstring& path, uintmax_t requiredBytes);

private:
    /// <summary>
    /// 🔒 Verifica se directory è protetta di sistema
    /// </summary>
    static bool IsSystemDirectory(const std::wstring& path);

    /// <summary>
    /// 🔒 Verifica permessi lettura/scrittura
    /// </summary>
    static bool HasReadWriteAccess(const std::wstring& path);

    /// <summary>
    /// 🔒 Verifica path traversal attacks
    /// </summary>
    static bool ContainsSuspiciousCharacters(const std::wstring& path);
};
