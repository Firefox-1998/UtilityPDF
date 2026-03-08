# 🚀 UpdaterBootstrap.Native

**Updater 100% Nativo in C++** — Completamente indipendente dal .NET Runtime.

## 🎯 Caratteristiche

✅ **Zero dipendenze .NET** — Eseguibile standalone C++ nativo  
✅ **Zero DLL esterne** — Tutte le librerie compilate staticamente nel binario  
✅ **Estrazione .7z nativa** — 7-Zip LZMA SDK (puro C) per le release da GitHub  
✅ **Compressione backup .zip** — minizip + zlib (puro C) per i backup pre-update  
✅ **Download HTTPS** — `URLDownloadToFile` (urlmon.lib)  
✅ **Verifica crittografica** — SHA-256 + RSA-4096 via Windows BCrypt API  
✅ **Backup selettivo** — Solo i file che verranno sovrascritti (basato sul contenuto del .7z)  
✅ **Rollback automatico** — Ripristino da backup .zip in caso di errore  
✅ **Lettura JSON indipendente** — nlohmann-json (header-only)  
✅ **Case-insensitive JSON** — Supporta PascalCase + camelCase  
✅ **Logging strutturato** — File di log con codici evento  
✅ **Progress dialog nativa** — Finestra di avanzamento Win32  

---

## 📦 Requisiti di Build

- **Visual Studio 2022** (v17.8+)
- **Windows SDK 10.0**
- **Standard C++20**
- **NuGet** (nlohmann-json 3.12.0)
- **OpenSSL** (per la firma — disponibile in Git for Windows)

### Librerie incluse nel progetto (sorgenti compilati staticamente)

| Libreria | Versione | Scopo | Licenza |
|----------|----------|-------|---------|
| **nlohmann-json** | 3.12.0 | Parsing JSON configurazione | MIT |
| **7-Zip LZMA SDK** | 24.07 | Estrazione archivi .7z | Public Domain |
| **zlib** | 1.3.1 | Compressione deflate | zlib License |
| **minizip** | (da zlib) | Creazione/estrazione .zip | zlib License |

---

## 📂 Struttura del Progetto

```
UtilityPDF/
├── UpdaterBootstrap.Native/
│   ├── src/
│   │   ├── main.cpp                  # Entry point + orchestratore aggiornamento
│   │   ├── ArchiveManager.h/cpp      # Estrazione .7z (LZMA SDK) + compressione .zip (minizip)
│   │   ├── BackupService.h/cpp       # Backup selettivo + restore da .zip
│   │   ├── ConfigReader.h/cpp        # Lettura configurazione JSON
│   │   ├── ConfigMerger.h/cpp        # Merge intelligente configurazione
│   │   ├── FileOperations.h/cpp      # Operazioni file + copia update
│   │   ├── Logger.h/cpp              # Logging strutturato in file
│   │   ├── ProcessManager.h/cpp      # Gestione processi (attesa chiusura, avvio)
│   │   ├── ProgressDialog.h/cpp      # Finestra progresso Win32 nativa
│   │   └── UpdateValidator.h/cpp     # Validazioni sicurezza + verifica SHA-256/RSA
│   ├── lib/
│   │   ├── 7zSDK/                    # 7-Zip LZMA SDK (solo decodifica)
│   │   ├── zlib/                     # zlib 1.3.1
│   │   └── minizip/                  # minizip (da zlib/contrib/minizip)
│   ├── resource.h
│   ├── UpdaterBootstrap.Native.rc
│   ├── updaterbootstrap.ico
│   ├── packages.config
│   └── UpdaterBootstrap.Native.vcxproj
├── scripts/
│   ├── generate-signing-keys.ps1     # Generazione chiavi RSA-4096 (una tantum)
│   ├── sign-archive.ps1             # Firma archivio .7z release (+ opzionale .exe)
│   └── sign-release.ps1             # Firma singolo binario (legacy)
├── .github/
│   └── workflows/
│       └── release.yml               # CI/CD: build + firma + pubblicazione release
└── keys/                             # Directory chiavi (generata da script)
    ├── update-signing-key.pem        # ⛔ SEGRETO — MAI committare!
    ├── update-signing-key.pub.pem    # ✅ Chiave pubblica PEM
    ├── update-signing-key.pub.der    # ✅ Chiave pubblica DER (per C++ BCrypt)
    ├── update-signing-key.pub.xml    # ✅ Chiave pubblica XML (per C# RSA)
    └── PublicKeyBytes.h              # ✅ Header C++ con bytes DER incorporati
```

---

## 🔧 Istruzioni di Build

### Opzione 1: Visual Studio 2022

1. Apri la soluzione contenente `UpdaterBootstrap.Native.vcxproj`
2. Seleziona configurazione: **Release | x64**
3. Build → **Build Solution** (Ctrl+Shift+B)
4. Output: `bin\x64\Release\UpdaterBootstrap.exe`

### Opzione 2: Riga di Comando MSBuild

```bash
nuget restore
msbuild UpdaterBootstrap.Native\UpdaterBootstrap.Native.vcxproj /p:Configuration=Release /p:Platform=x64
```

> **Nota:** I sorgenti di 7z SDK, zlib e minizip devono essere presenti nelle cartelle `lib\7zSDK\`, `lib\zlib\` e `lib\minizip\`. Vedere la sezione [Setup librerie](#-setup-librerie) per i dettagli.

---

## 📥 Setup Librerie

### 7-Zip LZMA SDK (estrazione .7z)

1. Scarica da: https://www.7-zip.org/sdk.html → `lzma2407.7z`
2. Estrai i file dalla cartella `C/` del SDK
3. Copia in `UpdaterBootstrap.Native\lib\7zSDK\`:

```
lib\7zSDK\
├── 7z.h / 7zAlloc.c/.h / 7zArcIn.c / 7zBuf.c/.h
├── 7zCrc.c/.h / 7zCrcOpt.c / 7zDec.c
├── 7zFile.c/.h / 7zStream.c / 7zTypes.h
├── Bcj2.c/.h / Bra.c/.h / CpuArch.c/.h
├── Compiler.h / Delta.c/.h / Precomp.h
├── LzmaDec.c/.h / Lzma2Dec.c/.h
└── Ppmd.h / Ppmd7.c/.h / Ppmd7Dec.c
```

### zlib (compressione deflate)

1. Scarica da: https://zlib.net/ → `zlib-1.3.1.tar.gz`
2. Copia in `UpdaterBootstrap.Native\lib\zlib\`:

```
lib\zlib\
├── adler32.c / compress.c / crc32.c/.h
├── deflate.c/.h / infback.c / inffast.c/.h
├── inflate.c/.h / inftrees.c/.h / trees.c/.h
├── uncompr.c / zconf.h / zlib.h / zutil.c/.h
```

### minizip (creazione/estrazione .zip)

1. Dalla stessa distribuzione zlib, cartella `contrib/minizip/`
2. Copia in `UpdaterBootstrap.Native\lib\minizip\`:

```
lib\minizip\
├── ioapi.c/.h / iowin32.c/.h
├── zip.c/.h / unzip.c/.h
```

> Tutti i file `.c` vengono compilati come C (`CompileAs: CompileAsC`) e con `_7ZIP_ST` definito per la modalità single-thread. Le configurazioni sono già nel `.vcxproj`.

---

## 🔄 Flusso di Aggiornamento

```
┌─────────────────────────────────────────────────────────┐
│                    UtilityPDF.exe                        │
│  1. Controlla aggiornamenti su GitHub API               │
│  2. Se disponibile: crea update_in_progress.json        │
│  3. Lancia UpdaterBootstrap.exe                         │
│  4. Si chiude                                           │
└────────────────────────┬────────────────────────────────┘
                         ▼
┌─────────────────────────────────────────────────────────┐
│                 UpdaterBootstrap.exe                     │
│                                                         │
│   SICUREZZA                                             │
│   ├── 5. Verifica marker file (update_in_progress.json) │
│   ├── 6. Verifica processo padre (UtilityPDF.exe)       │
│   └── 7. Carica e valida URL dal marker                 │
│                                                         │
│   ATTESA                                                │
│   └── 8. Attende chiusura UtilityPDF.exe                │
│                                                         │
│   CONFERMA                                              │
│   └── 9. Chiede conferma all'utente (Sì/No)            │
│                                                         │
│   DOWNLOAD (da GitHub Release)                          │
│   ├── 10. Scarica .7z  → temp_update\update_archive.7z │
│   ├── 11. Scarica .sha256 → temp_update\*.sha256       │
│   └── 12. Scarica .sig → temp_update\*.sig             │
│                                                         │
│   VERIFICA CRITTOGRAFICA                                │
│   ├── 13. Calcola SHA-256 del .7z scaricato             │
│   ├── 14. Confronta con hash atteso (.sha256)           │
│   └── 15. Verifica firma RSA-4096 (.sig) con BCrypt     │
│                                                         │
│   ESTRAZIONE                                            │
│   └── 16. Estrae .7z → temp_update\extracted\           │
│           (7-Zip LZMA SDK nativo, zero DLL)             │
│                                                         │
│   BACKUP SELETTIVO                                      │
│   ├── 17. Enumera file da temp_update\extracted\        │
│   ├── 18. Per ogni file: se esiste in appDir, lo copia  │
│   │       nella staging directory                       │
│   ├── 19. Comprime staging → backups\pre-update_v{X}.zip│
│   │       (minizip + zlib nativi, zero DLL)             │
│   └── 20. Elimina staging directory                     │
│                                                         │
│   APPLICAZIONE UPDATE                                   │
│   ├── 21. Copia file da extracted\ → appDir\            │
│   │       (salta file di servizio: .log, .sig, ecc.)    │
│   └── 22. Se errore → ROLLBACK da backup .zip           │
│                                                         │
│   POST-UPDATE                                           │
│   ├── 23. Verifica integrità post-aggiornamento         │
│   ├── 24. Pulizia temp_update\ e marker files           │
│   └── 25. Riavvia UtilityPDF.exe                        │
│                                                         │
└─────────────────────────────────────────────────────────┘
