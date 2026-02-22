#include <windows.h>
#include <string>
#include <filesystem>
#include "ConfigReader.h"
#include "Logger.h"
#include "ProcessManager.h"
#include "FileOperations.h"
#include "BackupService.h"
#include "UpdateValidator.h"
#include "ProgressDialog.h"

namespace fs = std::filesystem;

std::wstring GetApplicationDirectory() {
    wchar_t buffer[MAX_PATH];
    GetModuleFileNameW(nullptr, buffer, MAX_PATH);
    fs::path exePath(buffer);
    return exePath.parent_path().wstring();
}

void ShowErrorDialog(const std::wstring& message) {
    MessageBoxW(nullptr, message.c_str(), 
               L"UpdaterBootstrap - Errore", 
               MB_OK | MB_ICONERROR | MB_TOPMOST | MB_SETFOREGROUND);
}

void ShowInfoDialog(const std::wstring& message) {
    MessageBoxW(nullptr, message.c_str(), 
               L"UpdaterBootstrap", 
               MB_OK | MB_ICONINFORMATION | MB_TOPMOST | MB_SETFOREGROUND);
}

/// <summary>
/// ✅ NUOVO: Mostra MessageBox sempre in primo piano
/// </summary>
int ShowTopMostMessageBox(const wchar_t* message, const wchar_t* title, UINT type)
{
    // Crea finestra temporanea invisibile con WS_EX_TOPMOST
    HWND tempOwner = CreateWindowExW(
        WS_EX_TOPMOST | WS_EX_TOOLWINDOW,
        L"STATIC",
        L"",
        WS_POPUP,
        0, 0, 0, 0,
        nullptr,
        nullptr,
        GetModuleHandle(nullptr),
        nullptr
    );

    // Mostra MessageBox con finestra temporanea come owner
    int result = MessageBoxW(tempOwner, message, title, type | MB_TOPMOST | MB_SETFOREGROUND);

    // Distruggi finestra temporanea
    if (tempOwner)
    {
        DestroyWindow(tempOwner);
    }

    return result;
}

/// <summary>
/// 🧹 NUOVO: Pulisce file marker aggiornamento
/// </summary>
void CleanupUpdateMarkerFiles(const std::wstring& appDir)
{
    try
    {
        fs::path configDir = fs::path(appDir) / L"config";
        
        std::vector<std::wstring> markerFiles = {
            (configDir / L"update_in_progress.json").wstring(),
            (configDir / L"update_failed.json").wstring()
        };

        for (const auto& markerFile : markerFiles)
        {
            if (fs::exists(markerFile))
            {
                try
                {
                    fs::remove(markerFile);
                    Logger::LogInfo(L"MARKER_FILE_DELETED", 
                        L"File marker aggiornamento eliminato: " + fs::path(markerFile).filename().wstring());
                }
                catch (const std::exception& ex)
                {
                    std::string errorMsg = ex.what();
                    Logger::LogWarning(L"MARKER_FILE_DELETE_ERROR", 
                        L"Impossibile eliminare file marker: " + fs::path(markerFile).filename().wstring() +
                        L" - " + std::wstring(errorMsg.begin(), errorMsg.end()));
                }
            }
        }
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogWarning(L"CLEANUP_MARKERS_ERROR", 
            L"Errore durante pulizia file marker: " + std::wstring(errorMsg.begin(), errorMsg.end()));
    }
}

int WINAPI wWinMain(
    _In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPWSTR lpCmdLine,
    _In_ int nShowCmd)
{
    // 1. Ottieni directory applicazione
    std::wstring appDir = GetApplicationDirectory();
    
    // 2. Carica configurazione
    ConfigReader configReader(appDir);
    auto configOpt = configReader.LoadConfiguration();
    
    if (!configOpt.has_value()) {
        ShowErrorDialog(
            L"Directory aggiornamento non configurata o non valida.\n\n"
            L"Configura 'application.updateDirectory' in config\\appsettings.json"
        );
        
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }
    
    auto config = configOpt.value();
    
    // 3. Inizializza logger
    Logger::Initialize(config.logDirectory);
    Logger::LogInfo(L"BOOTSTRAP_START", L"UpdaterBootstrap.Native avviato");
    Logger::LogInfo(L"APP_DIRECTORY", appDir);
    Logger::LogInfo(L"UPDATE_DIRECTORY", config.updateDirectory);
    
    // 4. VALIDAZIONI
    if (!UpdateValidator::ValidateApplicationDirectory(appDir)) {
        ShowErrorDialog(L"Directory applicazione non valida.");
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }
    
    if (!UpdateValidator::ValidateUpdateDirectory(config.updateDirectory)) {
        ShowErrorDialog(L"Directory aggiornamento non valida o non sicura.");
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }
    
    // 5. VERIFICA SICUREZZA DOPPIA (Marker + Processo Parent)
    fs::path markerFile = fs::path(appDir) / L"config" / L"update_in_progress.json";
    bool markerExists = fs::exists(markerFile);
    bool validParent = ProcessManager::IsLaunchedByMainApp();
    
    if (!markerExists || !validParent)
    {
        std::wstring errorMsg = 
            L"UpdaterBootstrap pu\u00F2 essere avviato solo da GestioneEffetti.exe\n\n"
            L"Verifica sicurezza:\n";
        
        errorMsg += L"  File marker: ";
        errorMsg += markerExists ? L"OK" : L"MANCANTE";
        errorMsg += L"\n";
        
        errorMsg += L"  Processo parent: ";
        errorMsg += validParent ? L"OK" : L"NON AUTORIZZATO";
        
        ShowErrorDialog(errorMsg);
        
        Logger::LogError(L"SECURITY_CHECK_FAILED", 
            L"Verifica sicurezza fallita - Marker: " + 
            std::wstring(markerExists ? L"OK" : L"FAIL") + 
            L", Parent: " + 
            std::wstring(validParent ? L"OK" : L"FAIL"));
        
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }
    
    Logger::LogInfo(L"SECURITY_CHECK_PASSED", 
        L"Verifica sicurezza completata con successo");
    
    // 6. Attendi chiusura app principale
    std::wstring mainAppExe = L"GestioneEffetti.exe";
    if (ProcessManager::IsProcessRunning(mainAppExe)) {
        Logger::LogInfo(L"WAIT_APP_CLOSE", L"Attendo chiusura " + mainAppExe);
        
        ProcessManager::WaitForProcessTermination(mainAppExe);
    }

    // 7. Cerca file configurazione aggiornamento
    auto updateConfigPathOpt = FileOperations::FindUpdateConfigPath(config.updateDirectory);
    if (!updateConfigPathOpt.has_value()) {
        ShowErrorDialog(L"File updateappsettings.json non trovato");
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }
    
    std::wstring updateConfigPath = updateConfigPathOpt.value();
    
    // 8. Verifica disponibilità aggiornamento
    auto updateInfoOpt = FileOperations::CheckForUpdate(appDir, config.updateDirectory, updateConfigPath);
    if (!updateInfoOpt.has_value()) {
        ShowInfoDialog(L"Nessun aggiornamento disponibile");
        CleanupUpdateMarkerFiles(appDir);
        return 0;
    }
    
    auto updateInfo = updateInfoOpt.value();
    Logger::LogInfo(L"UPDATE_AVAILABLE", 
        L"Aggiornamento disponibile: " + updateInfo.currentVersion + L" -> " + updateInfo.newVersion);

    // ✅ 9: CONFERMA UTENTE (PRIMA della ProgressDialog)
    std::wstring confirmMessage = 
        L"Aggiornamento disponibile\n\n"
        L"Versione corrente: " + updateInfo.currentVersion + L"\n"
        L"Nuova versione: " + updateInfo.newVersion + L"\n\n"
        L"Procedere con l'aggiornamento?";
    
    int result = ShowTopMostMessageBox(confirmMessage.c_str(), 
                                       L"Conferma Aggiornamento", 
                                       MB_YESNO | MB_ICONQUESTION);
    
    if (result != IDYES) {
        Logger::LogInfo(L"UPDATE_CANCELLED", L"Aggiornamento annullato dall'utente");
        CleanupUpdateMarkerFiles(appDir);
        return 0;
    }

    // ✅ 10: CREA DIALOG PROGRESSO (DOPO la conferma)
    ProgressDialog progressDlg;
    if (!progressDlg.Create(hInstance))
    {
        ShowErrorDialog(L"Impossibile creare finestra di progresso");
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    progressDlg.SetVersionInfo(updateInfo.currentVersion, updateInfo.newVersion);
    progressDlg.SetStatus(L"Verifica prerequisiti...");
    progressDlg.SetProgress(5);
    
    // 11. BACKUP SELETTIVO
    progressDlg.SetStatus(L"Creazione backup selettivo...");
    progressDlg.SetProgress(25);
    
    std::wstring backupDir = appDir + L"\\backups";
    
    auto backupResult = BackupService::CreateSelectiveBackup(appDir, backupDir, config.updateDirectory);
    
    if (!backupResult.success) {
        progressDlg.ShowError(L"Errore creazione backup");
        ShowTopMostMessageBox((L"Errore creazione backup:\n" + backupResult.message).c_str(), 
                              L"Errore", 
                              MB_OK | MB_ICONERROR);
        Sleep(3000);
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }
    
    progressDlg.SetProgress(40);
    
    // 12. APPLICA AGGIORNAMENTO
    progressDlg.SetStatus(L"Applicazione aggiornamento...");
    progressDlg.SetProgress(50);
    
    auto updateResult = FileOperations::ApplyCompleteUpdate(appDir, config.updateDirectory, updateInfo);
    
    progressDlg.SetProgress(80);
    
    if (!updateResult.success) {
        Logger::LogError(L"UPDATE_FAILED", L"Aggiornamento fallito");
        progressDlg.ShowError(L"Aggiornamento fallito. Ripristino backup...");
        
        BackupService::RestoreBackup(backupResult.backupPath, appDir);
        CleanupUpdateMarkerFiles(appDir);
        
        Sleep(3000);
        return 1;
    }
    
    // 13. VERIFICA INTEGRITÀ
    progressDlg.SetStatus(L"Verifica integrit\u00E0...");
    progressDlg.SetProgress(90);
    
    if (!UpdateValidator::VerifyUpdateIntegrity(appDir)) {
        Logger::LogError(L"INTEGRITY_CHECK_FAILED", L"Verifica integrit\u00E0 fallita");
        progressDlg.ShowError(L"Verifica integrit\u00E0 fallita. Ripristino backup...");
        
        BackupService::RestoreBackup(backupResult.backupPath, appDir);
        CleanupUpdateMarkerFiles(appDir);
        Sleep(3000);
        return 1;
    }
    
    // 14. SUCCESSO
    progressDlg.SetProgress(100);
    progressDlg.ShowSuccess(L"Aggiornamento completato con successo!");
    
    Logger::LogInfo(L"UPDATE_SUCCESS", L"Aggiornamento completato");
    
    CleanupUpdateMarkerFiles(appDir);
    
    Sleep(2000);
    
    // ✅ 15: Mostra dialog completamento con caratteri accentati corretti
    std::wstring completionMessage =
        L"Aggiornamento completato!\n\n"
        L"Versione: " + updateInfo.newVersion + L"\n\n"
        L"L'applicazione verr\u00E0 riavviata.";

    ShowTopMostMessageBox(
        completionMessage.c_str(),
        L"Aggiornamento Completato",
        MB_OK | MB_ICONINFORMATION);
    
    // 16. Riavvia applicazione
    std::wstring mainAppPath = appDir + L"\\" + mainAppExe;
    
    if (fs::exists(mainAppPath)) {
        ProcessManager::LaunchProcess(mainAppPath);
    }
    
    progressDlg.Close();
    return 0;
}
