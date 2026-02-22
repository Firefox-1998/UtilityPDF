#include "Logger.h"
#include <iostream>
#include <iomanip>
#include <sstream>
#include <chrono>
#include <filesystem>

namespace fs = std::filesystem;

std::wofstream Logger::_logFile;
std::mutex Logger::_logMutex;

void Logger::Initialize(const std::wstring& logDirectory) {
    std::lock_guard<std::mutex> lock(_logMutex);
    
    try {
        if (!fs::exists(logDirectory)) {
            fs::create_directories(logDirectory);
        }

        auto now = std::chrono::system_clock::now();
        auto time_t_now = std::chrono::system_clock::to_time_t(now);
        std::tm tm_now;
        localtime_s(&tm_now, &time_t_now);

        std::wostringstream filename;
        filename << logDirectory << L"\\update_native_"
                << std::put_time(&tm_now, L"%Y%m%d_%H%M%S") << L".log";

        _logFile.open(filename.str(), std::ios::out | std::ios::app);
        _logFile.imbue(std::locale(""));

        if (_logFile.is_open()) {
            _logFile << L"========================================" << std::endl;
            _logFile << L"UpdaterBootstrap.Native Log Started" << std::endl;
            _logFile << L"Timestamp: " << GetTimestamp() << std::endl;
            _logFile << L"========================================" << std::endl;
        }
    }
    catch (...) {
        // Fallback: output su console
        std::wcout << L"⚠️ Impossibile inizializzare log file" << std::endl;
    }
}

void Logger::LogInfo(const std::wstring& operation, const std::wstring& message) {
    WriteLog(L"INFO", operation, message);
}

void Logger::LogWarning(const std::wstring& operation, const std::wstring& message) {
    WriteLog(L"WARNING", operation, message);
}

void Logger::LogError(const std::wstring& operation, const std::wstring& message) {
    WriteLog(L"ERROR", operation, message);
}

void Logger::WriteLog(const std::wstring& level, const std::wstring& operation, const std::wstring& message) {
    std::lock_guard<std::mutex> lock(_logMutex);
    
    std::wstring timestamp = GetTimestamp();
    std::wstring logEntry = L"[" + timestamp + L"] [" + level + L"] [" + operation + L"] " + message;

    if (_logFile.is_open()) {
        _logFile << logEntry << std::endl;
        _logFile.flush();
    }

    // ✅ Output anche su console per debugging
    std::wcout << logEntry << std::endl;
}

void Logger::Shutdown() {
    std::lock_guard<std::mutex> lock(_logMutex);
    
    if (_logFile.is_open()) {
        _logFile << L"========================================" << std::endl;
        _logFile << L"UpdaterBootstrap.Native Log Ended" << std::endl;
        _logFile << L"Timestamp: " << GetTimestamp() << std::endl;
        _logFile << L"========================================" << std::endl;
        _logFile.close();
    }
}

std::wstring Logger::GetTimestamp() {
    auto now = std::chrono::system_clock::now();
    auto time_t_now = std::chrono::system_clock::to_time_t(now);
    std::tm tm_now;
    localtime_s(&tm_now, &time_t_now);

    std::wostringstream oss;
    oss << std::put_time(&tm_now, L"%Y-%m-%d %H:%M:%S");
    return oss.str();
}