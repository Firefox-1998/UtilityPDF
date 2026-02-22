#pragma once
#include <string>
#include <fstream>
#include <mutex>

class Logger {
public:
    static void Initialize(const std::wstring& logDirectory);
    static void LogInfo(const std::wstring& operation, const std::wstring& message);
    static void LogWarning(const std::wstring& operation, const std::wstring& message); // ✅ NUOVO
    static void LogError(const std::wstring& operation, const std::wstring& message);
    static void Shutdown(); // ✅ NUOVO: Chiude file log
    
private:
    static std::wofstream _logFile;
    static std::mutex _logMutex;
    static std::wstring GetTimestamp();
    static void WriteLog(const std::wstring& level, const std::wstring& operation, const std::wstring& message);
};
