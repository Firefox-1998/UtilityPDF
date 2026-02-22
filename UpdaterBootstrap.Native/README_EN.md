# 🚀 UpdaterBootstrap.Native

**100% Native C++ Updater** - Completely independent from .NET Runtime.

## 🎯 Features

✅ **Zero .NET dependencies** - Standalone native C++ executable  
✅ **Independent JSON reading** - nlohmann-json (header-only)  
✅ **Complete update logic** in C++  
✅ **Automatic backup** before update  
✅ **Automatic rollback** on error  
✅ **Security validations** (system directories, path traversal)  
✅ **Network share support** and cloud sync  
✅ **Case-insensitive JSON** (PascalCase + camelCase)  
✅ **Structured file logging**  

## 📦 Build Requirements

- **Visual Studio 2022** (v17.8+)
- **Windows SDK 10.0**
- **C++20 Standard**
- **vcpkg** (for nlohmann-json)

## 🔧 Build Instructions

### Option 1: Visual Studio 2022

1. Open `UpdaterBootstrap.Native.sln`
2. Select configuration: **Release | x64**
3. Build → **Build Solution** (Ctrl+Shift+B)
4. Output: `bin\x64\Release\UpdaterBootstrap.exe`

### Option 2: MSBuild Command Line

```bash
msbuild UpdaterBootstrap.Native.sln /p:Configuration=Release /p:Platform=x64
```

### Option 3: vcpkg

```bash
# 1. Install vcpkg dependencies
vcpkg install nlohmann-json:x64-windows
vcpkg integrate install

# 2. Build Release
msbuild UpdaterBootstrap.Native.sln /p:Configuration=Release /p:Platform=x64

# 3. Output
bin\x64\Release\UpdaterBootstrap.exe (~200 KB)
```

## 📂 Project Structure

```
UtilityPDF/
├── UpdaterBootstrap.Native/
│   ├── src/
│   │   ├── main.cpp                  # Entry point + orchestrator
│   │   ├── ConfigReader.h/cpp        # JSON configuration reading
│   │   ├── Logger.h/cpp              # Native logging
│   │   ├── ProcessManager.h/cpp      # Process management
│   │   ├── FileOperations.h/cpp      # File operations + config merge
│   │   ├── BackupService.h/cpp       # Automatic backup/restore
│   │   └── UpdateValidator.h/cpp     # Security validations
│   ├── vcpkg.json                    # Dependencies (nlohmann-json)
│   ├── UpdaterBootstrap.Native.vcxproj
│   └── README.md
└── UpdaterBootstrap.Native.sln
```

## 🔑 Key Features

### 1. **Independent JSON Reading**

```cpp
ConfigReader reader(appDirectory);
auto config = reader.LoadConfiguration();
if (config.has_value()) {
    std::wcout << L"UpdateDirectory: " << config->updateDirectory << std::endl;
    std::wcout << L"LogDirectory: " << config->logDirectory << std::endl;
}
```

### 2. **Case-Insensitive Configuration**

Supports both `PascalCase` and `camelCase`:

```json
{
  "application": {     // or "Application"
    "updateDirectory"  // or "UpdateDirectory"
  }
}
```

### 3. **Structured Logging**

```cpp
Logger::Initialize(logDirectory);
Logger::LogInfo(L"BOOTSTRAP_START", L"UpdaterBootstrap started");
Logger::LogError(L"CONFIG_NOT_FOUND", L"File appsettings.json missing");
```

### 4. **Complete Update Logic**

```cpp
// Automatic backup
auto backupResult = BackupService::CreateBackup(appDir, backupDir);

// Apply update
auto updateResult = FileOperations::ApplyCompleteUpdate(appDir, updateDir, updateInfo);

// Rollback on error
if (!updateResult.success) {
    BackupService::RestoreBackup(backupResult.backupPath, appDir);
}

// Verify integrity
UpdateValidator::VerifyUpdateIntegrity(appDir);
```

## 🚀 Deployment

### Final Structure

```
UtilityPDF/
├── UpdaterBootstrap.exe           [Native C++ - 200 KB]
├── UtilityPDF.exe             	   [Main application]
├── config/
│   └── appsettings.json          [Configuration]
└── logs/
    └── update_native_20260130.log [Native log]
```

## ⚙️ Required Configuration

```json
{
  "application": {
    "updateDirectory": "Z:\\Updates\\UtilityPDF.exe"
  },
  "logging": {
    "logDirectory": "Z:\\Logs"
  }
}
```

## 🔄 Update Flow

```
1. Read appsettings.json (C++)
2. Validate update directory
3. Wait for GestioneEffetti.exe to close
4. Check for available update
5. Create automatic application backup
6. Apply file updates
7. Perform intelligent configuration merge
8. Verify post-update integrity
9. Automatic rollback if error occurs
10. Restart application
```

## 📊 Comparison

| Aspect | Native C++ | .NET Only |
|---------|-----------|-----------|
| **Size** | ~200 KB | ~2 MB |
| **Startup** | Instant | ~500ms |
| **Dependencies** | Zero | .NET 9 Runtime |
| **Complete Logic** | ✅ Yes | ✅ Yes |
| **Backup/Restore** | ✅ Yes | ✅ Yes |

## 🐛 Troubleshooting

### nlohmann-json not found

Install via vcpkg:

```bash
vcpkg install nlohmann-json:x64-windows
vcpkg integrate install
```

### Linker Error LNK2019

Ensure vcpkg integration is enabled:

```bash
vcpkg integrate install
```

Then reopen Visual Studio.

### Compilation Errors

Verify that C++20 standard is enabled in project properties:
- Configuration Properties → C/C++ → Language → C++ Language Standard: **ISO C++20 Standard (/std:c++20)**

## 🎉 Result

**UpdaterBootstrap.Native is now 100% autonomous** - no longer depends on `UpdaterBootstrap.Core.exe` (.NET 9)!

## 📝 License

MIT License - See main project [LICENSE](../LICENSE)

## 🙏 Credits

- **nlohmann-json**: https://github.com/nlohmann/json
- **Visual Studio 2022**: Microsoft
- **vcpkg**: Microsoft Package Manager

---

**Built with ❤️ for UtilityPDF**
