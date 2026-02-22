#include "ProgressDialog.h"
#include <commctrl.h>

// Inizializza Common Controls
#pragma comment(linker, "\"/manifestdependency:type='win32' \
name='Microsoft.Windows.Common-Controls' version='6.0.0.0' \
processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")

ProgressDialog::ProgressDialog()
    : m_hwnd(nullptr)
    , m_hwndStatus(nullptr)
    , m_hwndProgress(nullptr)
    , m_hwndVersion(nullptr)
    , m_hInstance(nullptr)
{
}

ProgressDialog::~ProgressDialog()
{
    Close();
}

bool ProgressDialog::Create(HINSTANCE hInstance)
{
    m_hInstance = hInstance;

    // Inizializza Common Controls
    INITCOMMONCONTROLSEX icex;
    icex.dwSize = sizeof(INITCOMMONCONTROLSEX);
    icex.dwICC = ICC_PROGRESS_CLASS;
    InitCommonControlsEx(&icex);

    // Crea finestra principale
    WNDCLASSEXW wc = { 0 };
    wc.cbSize = sizeof(WNDCLASSEXW);
    wc.lpfnWndProc = DialogProc;
    wc.hInstance = hInstance;
    wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wc.lpszClassName = L"UpdaterProgressDialog";
    wc.hCursor = LoadCursor(nullptr, IDC_ARROW);

    RegisterClassExW(&wc);

    // ✅ Dimensioni dialog: larghezza ridotta, altezza originale
    int width = 550;   // Ridotto da 600 a 550
    int height = 220;  // Ridotto da 250 a 220 (più compatto)
    int screenWidth = GetSystemMetrics(SM_CXSCREEN);
    int screenHeight = GetSystemMetrics(SM_CYSCREEN);
    int x = (screenWidth - width) / 2;
    int y = (screenHeight - height) / 2;

    m_hwnd = CreateWindowExW(
        WS_EX_TOPMOST | WS_EX_DLGMODALFRAME,
        L"UpdaterProgressDialog",
        L"Aggiornamento in corso...",
        WS_POPUP | WS_CAPTION | WS_SYSMENU,
        x, y, width, height,
        nullptr, nullptr, hInstance, this
    );

    if (!m_hwnd)
        return false;

    // ✅ Margini laterali per centrare i controlli
    int leftMargin = 30;    // Margine sinistro
    int controlWidth = 490; // Larghezza controlli (550 - 60 margini totali)

    // ✅ Label versione con altezza aumentata per 2 righe
    m_hwndVersion = CreateWindowExW(
        0, L"STATIC", L"",
        WS_CHILD | WS_VISIBLE | SS_CENTER,
        leftMargin, 15, controlWidth, 45,  // Altezza 45 per contenere 2 righe
        m_hwnd, nullptr, hInstance, nullptr
    );

    // ✅ Label stato centrata con altezza aumentata
    m_hwndStatus = CreateWindowExW(
        0, L"STATIC", L"Inizializzazione...",
        WS_CHILD | WS_VISIBLE | SS_CENTER,
        leftMargin, 70, controlWidth, 40,  // Top 70, altezza 40
        m_hwnd, nullptr, hInstance, nullptr
    );

    // ✅ Progress bar centrata con spaziatura omogenea
    m_hwndProgress = CreateWindowExW(
        0, PROGRESS_CLASSW, nullptr,
        WS_CHILD | WS_VISIBLE | PBS_SMOOTH,
        leftMargin, 120, controlWidth, 35,  // Top 120, altezza 35
        m_hwnd, nullptr, hInstance, nullptr
    );

    // Imposta range progress bar
    SendMessage(m_hwndProgress, PBM_SETRANGE, 0, MAKELPARAM(0, 100));
    SendMessage(m_hwndProgress, PBM_SETSTEP, 1, 0);

    // Font più grande
    HFONT hFont = CreateFontW(
        18, 0, 0, 0, FW_NORMAL, FALSE, FALSE, FALSE,
        DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS,
        CLEARTYPE_QUALITY, DEFAULT_PITCH | FF_DONTCARE,
        L"Segoe UI"
    );

    SendMessage(m_hwndVersion, WM_SETFONT, (WPARAM)hFont, TRUE);
    SendMessage(m_hwndStatus, WM_SETFONT, (WPARAM)hFont, TRUE);

    ShowWindow(m_hwnd, SW_SHOW);
    UpdateWindow(m_hwnd);

    return true;
}

void ProgressDialog::SetStatus(const std::wstring& status)
{
    if (m_hwndStatus)
    {
        SetWindowTextW(m_hwndStatus, status.c_str());
        ProcessMessages();
    }
}

void ProgressDialog::SetVersionInfo(const std::wstring& currentVersion, const std::wstring& newVersion)
{
    if (m_hwndVersion)
    {
        std::wstring text = L"Aggiornamento: " + currentVersion + L" -> " + newVersion;
        SetWindowTextW(m_hwndVersion, text.c_str());
        ProcessMessages();
    }
}

void ProgressDialog::SetProgress(int percentage)
{
    if (m_hwndProgress)
    {
        // Disabilita marquee se era attivo
        LONG style = GetWindowLong(m_hwndProgress, GWL_STYLE);
        if (style & PBS_MARQUEE)
        {
            SetWindowLong(m_hwndProgress, GWL_STYLE, style & ~PBS_MARQUEE);
        }

        SendMessage(m_hwndProgress, PBM_SETPOS, percentage, 0);
        ProcessMessages();
    }
}

void ProgressDialog::SetIndeterminate(bool enabled)
{
    if (m_hwndProgress)
    {
        LONG style = GetWindowLong(m_hwndProgress, GWL_STYLE);
        
        if (enabled)
        {
            SetWindowLong(m_hwndProgress, GWL_STYLE, style | PBS_MARQUEE);
            SendMessage(m_hwndProgress, PBM_SETMARQUEE, TRUE, 50);
        }
        else
        {
            SendMessage(m_hwndProgress, PBM_SETMARQUEE, FALSE, 0);
            SetWindowLong(m_hwndProgress, GWL_STYLE, style & ~PBS_MARQUEE);
        }
        
        ProcessMessages();
    }
}

void ProgressDialog::ShowSuccess(const std::wstring& message)
{
    if (m_hwndStatus)
    {
        SetWindowTextW(m_hwndStatus, message.c_str());
        SetProgress(100);
        
        // Cambia colore progressbar in verde (se possibile)
        SendMessage(m_hwndProgress, PBM_SETBARCOLOR, 0, RGB(0, 200, 0));
        
        ProcessMessages();
    }
}

void ProgressDialog::ShowError(const std::wstring& message)
{
    if (m_hwndStatus)
    {
        SetWindowTextW(m_hwndStatus, message.c_str());
        
        // Cambia colore progressbar in rosso
        SendMessage(m_hwndProgress, PBM_SETBARCOLOR, 0, RGB(200, 0, 0));
        
        ProcessMessages();
    }
}

void ProgressDialog::Close()
{
    if (m_hwnd)
    {
        DestroyWindow(m_hwnd);
        m_hwnd = nullptr;
    }
}

void ProgressDialog::ProcessMessages()
{
    MSG msg;
    while (PeekMessage(&msg, nullptr, 0, 0, PM_REMOVE))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }
}

INT_PTR CALLBACK ProgressDialog::DialogProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    ProgressDialog* pThis = nullptr;

    if (msg == WM_CREATE)
    {
        CREATESTRUCT* pCreate = reinterpret_cast<CREATESTRUCT*>(lParam);
        pThis = reinterpret_cast<ProgressDialog*>(pCreate->lpCreateParams);
        SetWindowLongPtr(hwnd, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(pThis));
    }
    else
    {
        pThis = reinterpret_cast<ProgressDialog*>(GetWindowLongPtr(hwnd, GWLP_USERDATA));
    }

    switch (msg)
    {
    case WM_CLOSE:
        // Impedisci chiusura manuale durante aggiornamento
        return TRUE;

    case WM_DESTROY:
        PostQuitMessage(0);
        return TRUE;
    }

    return DefWindowProc(hwnd, msg, wParam, lParam);
}
