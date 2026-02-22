#include "ProcessManager.h"
#include "Logger.h"
#include <windows.h>
#include <vector>
#include <tlhelp32.h>
#include <algorithm>

bool ProcessManager::IsProcessRunning(const std::wstring& processName) {
    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (snapshot == INVALID_HANDLE_VALUE) {
        return false;
    }

    PROCESSENTRY32W entry = { sizeof(entry) };
    
    if (Process32FirstW(snapshot, &entry)) {
        do {
            if (processName == entry.szExeFile) {
                CloseHandle(snapshot);
                return true;
            }
        } while (Process32NextW(snapshot, &entry));
    }
    
    CloseHandle(snapshot);
    return false;
}

void ProcessManager::WaitForProcessTermination(const std::wstring& processName) {
    Logger::LogInfo(L"WAIT_PROCESS", L"Attendo terminazione: " + processName);
    
    while (IsProcessRunning(processName)) {
        Sleep(500);
    }
    
    Logger::LogInfo(L"PROCESS_TERMINATED", processName + L" terminato");
}

bool ProcessManager::LaunchProcess(const std::wstring& exePath) {
    STARTUPINFOW si = { sizeof(si) };
    PROCESS_INFORMATION pi;
    
    std::wstring cmdLine = L"\"" + exePath + L"\"";
    std::vector<wchar_t> cmdLineBuf(cmdLine.begin(), cmdLine.end());
    cmdLineBuf.push_back(0);
    
    if (CreateProcessW(nullptr, cmdLineBuf.data(), nullptr, nullptr, 
                      FALSE, 0, nullptr, nullptr, &si, &pi)) {
        Logger::LogInfo(L"PROCESS_LAUNCHED", L"Avviato: " + exePath);
        CloseHandle(pi.hProcess);
        CloseHandle(pi.hThread);
        return true;
    }
    
    Logger::LogError(L"PROCESS_LAUNCH_FAILED", L"Impossibile avviare: " + exePath);
    return false;
}

// ✅ NUOVO: Ottiene PID del processo parent
DWORD ProcessManager::GetParentProcessId()
{
    DWORD currentPid = GetCurrentProcessId();
    DWORD parentPid = 0;
    
    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (snapshot == INVALID_HANDLE_VALUE)
    {
        Logger::LogError(L"PARENT_PID_ERROR", L"Impossibile creare snapshot processi");
        return 0;
    }
    
    PROCESSENTRY32W pe32;
    pe32.dwSize = sizeof(PROCESSENTRY32W);
    
    if (Process32FirstW(snapshot, &pe32))
    {
        do {
            if (pe32.th32ProcessID == currentPid)
            {
                parentPid = pe32.th32ParentProcessID;
                break;
            }
        } while (Process32NextW(snapshot, &pe32));
    }
    
    CloseHandle(snapshot);
    
    Logger::LogInfo(L"PARENT_PID_FOUND", 
        L"PID corrente: " + std::to_wstring(currentPid) + 
        L", PID parent: " + std::to_wstring(parentPid));
    
    return parentPid;
}

// ✅ NUOVO: Ottiene nome processo parent
std::optional<std::wstring> ProcessManager::GetParentProcessName()
{
    DWORD parentPid = GetParentProcessId();
    if (parentPid == 0)
    {
        Logger::LogWarning(L"PARENT_PROCESS_NOT_FOUND", L"PID parent non trovato");
        return std::nullopt;
    }
    
    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (snapshot == INVALID_HANDLE_VALUE)
    {
        Logger::LogError(L"PARENT_NAME_ERROR", L"Impossibile creare snapshot processi");
        return std::nullopt;
    }
    
    PROCESSENTRY32W pe32;
    pe32.dwSize = sizeof(PROCESSENTRY32W);
    std::optional<std::wstring> parentName;
    
    if (Process32FirstW(snapshot, &pe32))
    {
        do {
            if (pe32.th32ProcessID == parentPid)
            {
                parentName = pe32.szExeFile;
                break;
            }
        } while (Process32NextW(snapshot, &pe32));
    }
    
    CloseHandle(snapshot);
    
    if (parentName.has_value())
    {
        Logger::LogInfo(L"PARENT_PROCESS_NAME", L"Processo parent: " + parentName.value());
    }
    else
    {
        Logger::LogWarning(L"PARENT_PROCESS_NAME_NOT_FOUND", 
            L"Nome processo parent non trovato per PID: " + std::to_wstring(parentPid));
    }
    
    return parentName;
}

// ✅ NUOVO: Verifica che parent sia GestioneEffetti.exe
bool ProcessManager::IsLaunchedByMainApp()
{
    auto parentNameOpt = GetParentProcessName();
    
    if (!parentNameOpt.has_value())
    {
        Logger::LogError(L"PARENT_CHECK_FAILED", L"Impossibile ottenere nome processo parent");
        return false;
    }
    
    std::wstring parentName = parentNameOpt.value();
    
    // Converti in minuscolo per confronto case-insensitive
    std::transform(parentName.begin(), parentName.end(), 
                   parentName.begin(), ::tolower);
    
    bool isValid = (parentName == L"gestioneeffetti.exe");
    
    if (isValid)
    {
        Logger::LogInfo(L"PARENT_VALIDATION_SUCCESS", 
            L"Processo avviato correttamente da GestioneEffetti.exe");
    }
    else
    {
        Logger::LogWarning(L"PARENT_VALIDATION_FAILED", 
            L"Processo avviato da processo non autorizzato: " + parentNameOpt.value());
    }
    
    return isValid;
}
