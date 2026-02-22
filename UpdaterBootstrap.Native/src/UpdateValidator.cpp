#include "UpdateValidator.h"
#include "Logger.h"
#include <filesystem>
#include <shlobj.h>

namespace fs = std::filesystem;

bool UpdateValidator::ValidateApplicationDirectory(const std::wstring& appDir)
{
    try
    {
        // 1️⃣ Verifica esistenza directory
        if (appDir.empty() || !fs::exists(appDir))
        {
            Logger::LogError(L"VALIDATE_APP_DIR_NOT_EXISTS", 
                L"Directory applicazione non esiste: " + appDir);
            return false;
        }

        // 2️⃣ Verifica che non sia directory di sistema
        if (IsSystemDirectory(appDir))
        {
            Logger::LogError(L"SECURITY_APP_SYSTEM_DIR", 
                L"Tentativo di aggiornamento in directory di sistema: " + appDir);
            return false;
        }

        // 3️⃣ Verifica permessi
        if (!HasReadWriteAccess(appDir))
        {
            Logger::LogError(L"SECURITY_APP_NO_ACCESS", 
                L"Permessi insufficienti sulla directory applicazione: " + appDir);
            return false;
        }

        Logger::LogInfo(L"VALIDATE_APP_DIR_SUCCESS", 
            L"Directory applicazione validata: " + appDir);
        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"VALIDATE_APP_DIR_ERROR", 
            L"Errore validazione directory applicazione: " + 
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return false;
    }
}

bool UpdateValidator::ValidateUpdateDirectory(const std::wstring& updateDir)
{
    try
    {
        // 1️⃣ Verifica non vuota
        if (updateDir.empty())
        {
            Logger::LogError(L"UPDATE_DIR_EMPTY", L"Directory aggiornamento non specificata");
            return false;
        }

        // 2️⃣ Verifica esistenza directory
        if (!fs::exists(updateDir))
        {
            Logger::LogError(L"UPDATE_DIR_NOT_EXISTS", 
                L"Directory aggiornamento non esiste: " + updateDir);
            return false;
        }

        std::wstring fullPath = fs::absolute(updateDir).wstring();

        // 3️⃣ SICUREZZA: Blocca directory di sistema
        if (IsSystemDirectory(fullPath))
        {
            Logger::LogError(L"SECURITY_UPDATE_SYSTEM_DIR", 
                L"Directory aggiornamento in percorso protetto di sistema: " + fullPath);
            return false;
        }

        // 4️⃣ SICUREZZA: Verifica permessi lettura
        if (!HasReadWriteAccess(fullPath))
        {
            Logger::LogError(L"SECURITY_UPDATE_NO_READ", 
                L"Permessi di lettura insufficienti su directory aggiornamento: " + fullPath);
            return false;
        }

        // 5️⃣ SICUREZZA: Blocca path traversal
        if (ContainsSuspiciousCharacters(fullPath))
        {
            Logger::LogError(L"SECURITY_UPDATE_SUSPICIOUS", 
                L"Directory aggiornamento contiene caratteri sospetti: " + fullPath);
            return false;
        }

        // 6️⃣ Log tipo percorso (network/local)
        bool isNetworkPath = fullPath.starts_with(L"\\\\");
        Logger::LogInfo(L"UPDATE_DIR_VALIDATED", 
            L"Directory aggiornamento validata: " + fullPath + 
            (isNetworkPath ? L" (Network Share)" : L" (Local Path)"));

        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"VALIDATE_UPDATE_DIR_ERROR", 
            L"Errore validazione directory aggiornamento: " + 
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return false;
    }
}

bool UpdateValidator::VerifyUpdateIntegrity(const std::wstring& appDir)
{
    try
    {
        Logger::LogInfo(L"INTEGRITY_CHECK_START", L"Inizio verifica integrità");

        // 1️⃣ Verifica eseguibile principale
        fs::path exePath = fs::path(appDir) / L"GestioneEffetti.exe";
        if (!fs::exists(exePath))
        {
            Logger::LogError(L"INTEGRITY_EXE_MISSING", 
                L"Eseguibile principale mancante: " + exePath.wstring());
            return false;
        }

        // 2️⃣ Verifica dimensione minima eseguibile (1 KB)
        uintmax_t exeSize = fs::file_size(exePath);
        if (exeSize < 1024)
        {
            Logger::LogError(L"INTEGRITY_EXE_TOO_SMALL", 
                L"Eseguibile troppo piccolo: " + std::to_wstring(exeSize) + L" bytes");
            return false;
        }

        // 3️⃣ Verifica file configurazione
        fs::path configPath = fs::path(appDir) / L"config" / L"appsettings.json";
        if (!fs::exists(configPath))
        {
            Logger::LogError(L"INTEGRITY_CONFIG_MISSING", 
                L"File configurazione mancante: " + configPath.wstring());
            return false;
        }

        Logger::LogInfo(L"INTEGRITY_CHECK_PASSED", 
            L"✅ Verifica integrità completata con successo");
        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"INTEGRITY_CHECK_ERROR", 
            L"Errore verifica integrità: " + 
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return false;
    }
}

bool UpdateValidator::CheckDiskSpace(const std::wstring& path, uintmax_t requiredBytes)
{
    try
    {
        // ⚠️ LIMITAZIONE: spazio disco verificabile solo su percorsi locali
        if (path.starts_with(L"\\\\"))
        {
            Logger::LogWarning(L"DISK_SPACE_CHECK_SKIPPED", 
                L"Controllo spazio disco saltato (percorso UNC): " + path);
            return true; // Assumiamo OK per network share
        }

        fs::space_info space = fs::space(path);

        if (space.available < requiredBytes)
        {
            Logger::LogError(L"DISK_SPACE_INSUFFICIENT", 
                L"Spazio insufficiente - Richiesto: " + std::to_wstring(requiredBytes / 1024) + 
                L" KB, Disponibile: " + std::to_wstring(space.available / 1024) + L" KB");
            return false;
        }

        Logger::LogInfo(L"DISK_SPACE_OK", 
            L"Spazio disco sufficiente: " + std::to_wstring(space.available / 1024 / 1024) + L" MB");
        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogWarning(L"DISK_SPACE_CHECK_ERROR", 
            L"Errore verifica spazio disco: " + 
            std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return true; // Fallback: assumiamo OK se non verificabile
    }
}

bool UpdateValidator::IsSystemDirectory(const std::wstring& path)
{
    // ❌ Directory di sistema da bloccare
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
        // Test lettura: enumera file
        for (const auto& entry : fs::directory_iterator(path))
        {
            // Se arriviamo qui, abbiamo permessi lettura
            break;
        }

        // Test scrittura: crea file temporaneo
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
    // ❌ Blocca path traversal e caratteri sospetti
    if (path.find(L"..") != std::wstring::npos) return true;  // Path traversal
    if (path.find(L"~") != std::wstring::npos) return true;   // Home directory
    if (path.ends_with(L"\\") && path.length() == 3) return true;  // Drive root (es. C:\)

    return false;
}
