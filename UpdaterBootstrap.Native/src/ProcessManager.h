#pragma once
#include <windows.h>
#include <string>
#include <optional>

/// <summary>
/// Gestione processi Windows
/// </summary>
class ProcessManager
{
public:
    /// <summary>
    /// Verifica se un processo è in esecuzione
    /// </summary>
    static bool IsProcessRunning(const std::wstring& processName);

    /// <summary>
    /// Attende terminazione processo
    /// </summary>
    static void WaitForProcessTermination(const std::wstring& processName);

    /// <summary>
    /// Avvia un processo
    /// </summary>
    static bool LaunchProcess(const std::wstring& exePath);

    /// <summary>
    /// 🔒 Verifica che il processo parent sia GestioneEffetti.exe
    /// </summary>
    static bool IsLaunchedByMainApp();

    /// <summary>
    /// 🔒 Ottiene il nome del processo parent
    /// </summary>
    static std::optional<std::wstring> GetParentProcessName();

private:
    /// <summary>
    /// Ottiene PID del processo parent
    /// </summary>
    static DWORD GetParentProcessId();
};
