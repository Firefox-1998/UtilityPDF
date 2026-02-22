# 🚀 UpdaterBootstrap.Native

**Updater 100% Nativo in C++** - Completamente indipendente dal .NET Runtime.

## 🎯 Caratteristiche

✅ **Zero dipendenze .NET** - Eseguibile standalone C++ nativo  
✅ **Lettura JSON indipendente** - nlohmann-json (header-only)  
✅ **Logica aggiornamento completa** in C++  
✅ **Backup automatico** pre-aggiornamento  
✅ **Rollback automatico** in caso di errore  
✅ **Validazioni di sicurezza** (directory sistema, path traversal)  
✅ **Supporto network share** e cloud sync  
✅ **Case-insensitive JSON** (PascalCase + camelCase)  
✅ **Logging strutturato** in file  

## 📦 Requisiti di Build

- **Visual Studio 2022** (v17.8+)
- **Windows SDK 10.0**
- **Standard C++20**
- **vcpkg** (per nlohmann-json)

## 🔧 Istruzioni di Build

### Opzione 1: Visual Studio 2022

1. Apri `UpdaterBootstrap.Native.sln`
2. Seleziona configurazione: **Release | x64**
3. Build → **Build Solution** (Ctrl+Shift+B)
4. Output: `bin\x64\Release\UpdaterBootstrap.exe`

### Opzione 2: Riga di Comando MSBuild

```bash
msbuild UpdaterBootstrap.Native.sln /p:Configuration=Release /p:Platform=x64
```

### Opzione 3: vcpkg

```bash
# 1. Installa dipendenze vcpkg
vcpkg install nlohmann-json:x64-windows
vcpkg integrate install

# 2. Build Release
msbuild UpdaterBootstrap.Native.sln /p:Configuration=Release /p:Platform=x64

# 3. Output
bin\x64\Release\UpdaterBootstrap.exe (~200 KB)
```

## 📂 Struttura del Progetto

```
UtilityPDF/
├── UpdaterBootstrap.Native/
│   ├── src/
│   │   ├── main.cpp                  # Entry point + orchestratore
│   │   ├── ConfigReader.h/cpp        # Lettura configurazione JSON
│   │   ├── Logger.h/cpp              # Logging nativo
│   │   ├── ProcessManager.h/cpp      # Gestione processi
│   │   ├── FileOperations.h/cpp      # Operazioni file + merge config
│   │   ├── BackupService.h/cpp       # Backup/restore automatico
│   │   └── UpdateValidator.h/cpp     # Validazioni sicurezza
│   ├── vcpkg.json                    # Dipendenze (nlohmann-json)
│   ├── UpdaterBootstrap.Native.vcxproj
│   └── README.md
└── UpdaterBootstrap.Native.sln
```

## 🔑 Funzionalità Principali

### 1. **Lettura JSON Indipendente**

```cpp
ConfigReader reader(appDirectory);
auto config = reader.LoadConfiguration();
if (config.has_value()) {
    std::wcout << L"UpdateDirectory: " << config->updateDirectory << std::endl;
    std::wcout << L"LogDirectory: " << config->logDirectory << std::endl;
}
```

### 2. **Configurazione Case-Insensitive**

Supporta sia `PascalCase` che `camelCase`:

```json
{
  "application": {     // oppure "Application"
    "updateDirectory"  // oppure "UpdateDirectory"
  }
}
```

### 3. **Logging Strutturato**

```cpp
Logger::Initialize(logDirectory);
Logger::LogInfo(L"BOOTSTRAP_START", L"UpdaterBootstrap avviato");
Logger::LogError(L"CONFIG_NOT_FOUND", L"File appsettings.json mancante");
```

### 4. **Logica Aggiornamento Completa**

```cpp
// Backup automatico
auto backupResult = BackupService::CreateBackup(appDir, backupDir);

// Applica aggiornamento
auto updateResult = FileOperations::ApplyCompleteUpdate(appDir, updateDir, updateInfo);

// Rollback se errore
if (!updateResult.success) {
    BackupService::RestoreBackup(backupResult.backupPath, appDir);
}

// Verifica integrità
UpdateValidator::VerifyUpdateIntegrity(appDir);
```

## 🚀 Deployment

### Struttura Finale

```
UtilityPDF/
├── UpdaterBootstrap.exe           [C++ Nativo - 200 KB]
├── UtilityPDF.exe            [Applicazione principale]
├── config/
│   └── appsettings.json          [Configurazione]
└── logs/
    └── update_native_20260130.log [Log nativo]
```

## ⚙️ Configurazione Richiesta

```json
{
  "application": {
    "updateDirectory": "Z:\\Updates\UtilityPDF.exe" 
  },
  "logging": {
    "logDirectory": "Z:\\Logs"
  }
}
```

## 🔄 Flusso di Aggiornamento

```
1. Legge appsettings.json (C++)
2. Valida directory aggiornamenti
3. Attende chiusura GestioneEffetti.exe
4. Verifica disponibilità aggiornamento
5. Crea backup automatico applicazione
6. Applica aggiornamento file
7. Esegue merge intelligente configurazione
8. Verifica integrità post-aggiornamento
9. Esegue rollback automatico se errore
10. Riavvia applicazione
```

## 📊 Confronto

| Aspetto | C++ Nativo | Solo .NET |
|---------|-----------|-----------|
| **Dimensione** | ~200 KB | ~2 MB |
| **Avvio** | Istantaneo | ~500ms |
| **Dipendenze** | Zero | .NET 9 Runtime |
| **Logica Completa** | ✅ Sì | ✅ Sì |
| **Backup/Restore** | ✅ Sì | ✅ Sì |

## 🐛 Risoluzione Problemi

### nlohmann-json non trovato

Installa tramite vcpkg:

```bash
vcpkg install nlohmann-json:x64-windows
vcpkg integrate install
```

### Errore Linker LNK2019

Assicurati che l'integrazione vcpkg sia abilitata:

```bash
vcpkg integrate install
```

Poi riapri Visual Studio.

### Errori di Compilazione

Verifica che lo standard C++20 sia abilitato nelle proprietà del progetto:
- Proprietà Configurazione → C/C++ → Linguaggio → Standard Linguaggio C++: **Standard ISO C++20 (/std:c++20)**

## 🎉 Risultato

**UpdaterBootstrap.Native è ora 100% autonomo** - non dipende più da `UpdaterBootstrap.Core.exe` (.NET 9)!

## 📝 Licenza

Licenza MIT - Vedi [LICENSE](../LICENSE) del progetto principale

## 🙏 Ringraziamenti

- **nlohmann-json**: https://github.com/nlohmann/json
- **Visual Studio 2022**: Microsoft
- **vcpkg**: Microsoft Package Manager

---

**Sviluppato con ❤️ per UtilityPDF**
