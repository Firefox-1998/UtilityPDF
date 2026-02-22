#pragma once
#include <windows.h>
#include <commctrl.h>
#include <string>

#pragma comment(lib, "comctl32.lib")

/// <summary>
/// Dialog nativo Win32 per visualizzare progresso aggiornamento
/// </summary>
class ProgressDialog
{
public:
    ProgressDialog();
    ~ProgressDialog();

    /// <summary>
    /// Crea e mostra il dialog
    /// </summary>
    bool Create(HINSTANCE hInstance);

    /// <summary>
    /// Aggiorna testo stato
    /// </summary>
    void SetStatus(const std::wstring& status);

    /// <summary>
    /// Aggiorna versione (da X.Y.Z a A.B.C)
    /// </summary>
    void SetVersionInfo(const std::wstring& currentVersion, const std::wstring& newVersion);

    /// <summary>
    /// Imposta progresso (0-100)
    /// </summary>
    void SetProgress(int percentage);

    /// <summary>
    /// Modalita indeterminata (marquee)
    /// </summary>
    void SetIndeterminate(bool enabled);

    /// <summary>
    /// Mostra successo
    /// </summary>
    void ShowSuccess(const std::wstring& message);

    /// <summary>
    /// Mostra errore
    /// </summary>
    void ShowError(const std::wstring& message);

    /// <summary>
    /// Chiude il dialog
    /// </summary>
    void Close();

    /// <summary>
    /// Processa messaggi Windows (chiamare periodicamente)
    /// </summary>
    void ProcessMessages();

private:
    HWND m_hwnd;
    HWND m_hwndStatus;
    HWND m_hwndProgress;
    HWND m_hwndVersion;
    HINSTANCE m_hInstance;

    static INT_PTR CALLBACK DialogProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam);
};
