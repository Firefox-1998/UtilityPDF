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

```

### Backup: cosa viene salvato?

Il backup è **selettivo**: vengono salvati solo i file presenti nell'archivio `.7z` scaricato da GitHub. Se il `.7z` contiene 5 file, solo quei 5 file dalla directory corrente dell'applicazione vengono salvati (se esistono). I file nuovi (presenti nel `.7z` ma non nell'app corrente) vengono registrati nel log come "nuovo file" e non vengono backuppati — non c'è nulla da ripristinare.

Il backup viene salvato come `.zip` nella sottocartella `backups\`:

```
C:\App\UtilityPDF\
├── backups\
│   ├── pre-update_v1.7.0.zip     ← backup prima dell'aggiornamento a v1.7.1
│   └── pre-update_v1.7.5.zip     ← backup prima dell'aggiornamento a v1.8.0
```

I backup più vecchi di 30 giorni vengono eliminati automaticamente.

---

## 🔐 Sistema di Firma Crittografica

### Panoramica

Ogni release di UtilityPDF è protetta da una **doppia verifica crittografica**:

1. **Hash SHA-256** — Verifica di integrità (il file non è stato corrotto durante il download)
2. **Firma RSA-4096** — Verifica di autenticità (il file è stato creato da noi, non da un attaccante)

La chiave privata RSA-4096 è conservata **esclusivamente** nei GitHub Secrets (`SIGNING_PRIVATE_KEY`). La chiave pubblica corrispondente è incorporata nel binario `UpdaterBootstrap.exe` (come array di byte DER) e in `UtilityPDF.exe` (come XML).

### File generati per ogni release

Per ogni release su GitHub, vengono caricati **3 asset**:

| File | Contenuto | Formato |
|------|-----------|---------|
| `UtilityPDF-v1.8.0.7z` | Archivio con tutti i file dell'applicazione | 7z LZMA |
| `UtilityPDF-v1.8.0.7z.sha256` | Hash SHA-256 dell'archivio | Testo ASCII, hex minuscolo |
| `UtilityPDF-v1.8.0.7z.sig` | Firma RSA-4096 dell'archivio | Binario grezzo (512 byte) |

### Come funziona la verifica (in UpdaterBootstrap.exe)

```
1. Scarica .7z, .sha256, .sig
2. Calcola SHA-256(.7z) usando Windows BCrypt
3. Confronta con il contenuto di .sha256 (confronto a tempo costante)
4. Se hash non corrisponde → BLOCCO (file corrotto o manomesso)
5. Importa la chiave pubblica DER incorporata nel binario
6. Verifica firma: BCryptVerifySignature(chiavePubblica, SHA-256(.7z), .sig)
7. Se firma non valida → BLOCCO (file non firmato da noi)
8. Solo se ENTRAMBE le verifiche passano → procede con l'estrazione
```

---

## 🔑 Guida Operativa alla Firma — Passo per Passo

### Prerequisiti

- **OpenSSL** installato (o disponibile tramite **Git for Windows** — già incluso in `C:\Program Files\Git\usr\bin\openssl.exe`)
- **PowerShell 5.1+** (incluso in Windows 10/11)

### PASSO 1 — Generazione Chiavi (una tantum)

Questo script si esegue **UNA SOLA VOLTA** durante la configurazione iniziale del progetto, oppure se la chiave privata viene compromessa e deve essere rigenerata.

```powershell
cd "Z:\Visual Studio 2022\MyProjects\UtilityPDF"
.\scripts\generate-signing-keys.ps1 -OutputDir keys
```

**Parametri:**

| Parametro | Default | Descrizione |
|-----------|---------|-------------|
| `-OutputDir` | `keys` | Directory dove salvare le chiavi |
| `-KeySize` | `4096` | Dimensione chiave RSA (non modificare) |

**File generati:**

```
keys/
├── update-signing-key.pem       ⛔ CHIAVE PRIVATA — MAI committare!
├── update-signing-key.pub.pem   ✅ Chiave pubblica PEM (verifica manuale con OpenSSL)
├── update-signing-key.pub.der   ✅ Chiave pubblica DER (per C++ BCrypt)
├── update-signing-key.pub.xml   ✅ Chiave pubblica XML (per C# RSACryptoServiceProvider)
└── PublicKeyBytes.h             ✅ Header C++ con byte DER come array
```

**Dopo la generazione, è necessario fare 4 cose:**

#### 1a. Incorporare la chiave pubblica in UpdaterBootstrap (C++)

Aprire `keys\PublicKeyBytes.h` e copiare il contenuto dell'array `kPublicKeyDer[]` in `UpdaterBootstrap.Native\src\UpdateValidator.cpp`, sostituendo il placeholder:

```cpp
// In UpdateValidator.cpp — sostituire il placeholder con i byte reali
static const unsigned char kPublicKeyDer[] = {
    0x30, 0x82, 0x02, 0x22, 0x30, 0x0D, ...  // byte generati dallo script
};
```

#### 1b. Incorporare la chiave pubblica in UtilityPDF (C#)

Aprire `keys\update-signing-key.pub.xml` e copiare il contenuto XML in `UtilityPDF\Security\BinaryIntegrityVerifier.cs`, sostituendo il placeholder:

```csharp
// In BinaryIntegrityVerifier.cs — sostituire il placeholder con l'XML reale
private const string PublicKeyXml =
    "<RSAKeyValue>" +
    "<Modulus>ABC123...valore_reale...</Modulus>" +
    "<Exponent>AQAB</Exponent>" +
    "</RSAKeyValue>";
```

#### 1c. Salvare la chiave privata nei GitHub Secrets

1. Andare su GitHub → Repository → **Settings** → **Secrets and variables** → **Actions**
2. Cliccare **New repository secret**
3. Nome: `SIGNING_PRIVATE_KEY`
4. Valore: incollare l'**intero contenuto** di `keys\update-signing-key.pem` (incluse le righe `-----BEGIN RSA PRIVATE KEY-----` e `-----END RSA PRIVATE KEY-----`)
5. Cliccare **Add secret**

#### 1d. Aggiungere al .gitignore

Verificare che `.gitignore` contenga:

```gitignore
# Chiavi private di firma — MAI committare
keys/update-signing-key.pem
signing-key.pem
*.pem
!*.pub.pem
```

---

### PASSO 2 — Firma dell'Archivio .7z (per ogni release)

Ci sono **due modalità**: automatica (CI/CD) e manuale (locale).

#### Modalità A: Automatica via GitHub Actions (consigliata)

Il workflow `.github/workflows/release.yml` gestisce tutto automaticamente.

**Come usare:**

```bash
# 1. Assicurarsi che tutto sia committato e pushato
git add .
git commit -m "Release v1.8.0"
git push origin feature/autoupdate

# 2. Creare un tag di versione
git tag v1.8.0

# 3. Pushare il tag — questo avvia il workflow automaticamente
git push origin v1.8.0
```

**Cosa fa il workflow `release.yml`:**

| Passo | Azione |
|-------|--------|
| 1 | Checkout del codice |
| 2 | Estrae la versione dal tag (es. `v1.8.0` → `1.8.0`) |
| 3 | Build di UtilityPDF.csproj (Release) |
| 4 | Build di UpdaterBootstrap.Native.vcxproj (Release x64) |
| 5 | Prepara tutti i file in `release-staging\` |
| 6 | Crea `UtilityPDF-v1.8.0.7z` con compressione LZMA livello 7 |
| 7 | Scrive la chiave privata dal secret `SIGNING_PRIVATE_KEY` su disco (temporaneamente) |
| 8 | Genera `UtilityPDF-v1.8.0.7z.sha256` (hash SHA-256) |
| 9 | Genera `UtilityPDF-v1.8.0.7z.sig` (firma RSA-4096, 512 byte grezzi) |
| 10 | Verifica la firma appena generata (auto-test) |
| 11 | Firma anche `UpdaterBootstrap.exe` (hash + sig) |
| 12 | **Elimina la chiave privata dal disco** (step `always`) |
| 13 | Crea la GitHub Release con i 3 asset (.7z, .sha256, .sig) |

**Risultato su GitHub Releases:**

```
UtilityPDF v1.8.0
├── UtilityPDF-v1.8.0.7z          (archivio release)
├── UtilityPDF-v1.8.0.7z.sha256   (hash per verifica integrità)
└── UtilityPDF-v1.8.0.7z.sig      (firma RSA per verifica autenticità)
```

#### Modalità B: Manuale (locale)

Se non si usa GitHub Actions, o per testare la firma in locale:

```powershell
cd "Z:\Visual Studio 2022\MyProjects\UtilityPDF"

# 1. Compilare il progetto (da Visual Studio o MSBuild)
# ...

# 2. Creare manualmente il .7z con 7-Zip
& "C:\Program Files\7-Zip\7z.exe" a -t7z -mx=7 "UtilityPDF-v1.8.0.7z" ".\release-staging\*"

# 3. Firmare l'archivio (genera .sha256 e .sig)
.\scripts\sign-archive.ps1 `
    -ArchivePath "UtilityPDF-v1.8.0.7z" `
    -PrivateKeyPath "keys\update-signing-key.pem"
```

**Parametri per `sign-archive.ps1`:**

| Parametro | Obbligatorio | Descrizione |
|-----------|:---:|-------------|
| `-ArchivePath` | ✅ | Percorso dell'archivio `.7z` da firmare |
| `-PrivateKeyPath` | ✅ | Percorso della chiave privata RSA-4096 `.pem` |
| `-UpdaterBinaryPath` | ❌ | Se specificato, firma anche questo binario |

**Esempio completo di firma archivio + UpdaterBootstrap.exe:**

```powershell
.\scripts\sign-archive.ps1 `
    -ArchivePath "UtilityPDF-v1.8.0.7z" `
    -PrivateKeyPath "keys\update-signing-key.pem" `
    -UpdaterBinaryPath "UpdaterBootstrap.Native\bin\x64\Release\UpdaterBootstrap.exe"
```

**Output generato:**

```
UtilityPDF-v1.8.0.7z.sha256      ← Hash SHA-256 (testo ASCII, hex minuscolo)
UtilityPDF-v1.8.0.7z.sig         ← Firma RSA-4096 (512 byte binari grezzi)
UpdaterBootstrap.exe.sha256       ← Hash SHA-256 del binario
UpdaterBootstrap.exe.sig          ← Firma RSA-4096 del binario
```

**Cosa fa lo script internamente:**

1. Calcola `SHA-256` del file con `Get-FileHash`
2. Scrive l'hash in formato hex minuscolo nel file `.sha256`
3. Esegue `openssl dgst -sha256 -sign {chiave} -out {file}.sig {file}` per generare la firma
4. Verifica che la firma sia di 512 byte (dimensione attesa per RSA-4096)
5. Estrae la chiave pubblica dalla chiave privata e **auto-verifica** la firma
6. Mostra un riepilogo dei file generati

---

### PASSO 3 — Firma di un Singolo Binario (legacy)

Lo script `sign-release.ps1` è la versione originale che firma **un singolo binario** e produce la firma in **Base64** (non byte grezzi).

> ⚠️ **Nota:** Per il flusso di auto-aggiornamento, usare `sign-archive.ps1` che produce firme in formato byte grezzi compatibile con la verifica BCrypt di UpdaterBootstrap. `sign-release.ps1` è mantenuto per compatibilità e per firme standalone dove è necessario il formato Base64.

```powershell
.\scripts\sign-release.ps1 `
    -BinaryPath "UpdaterBootstrap.Native\bin\x64\Release\UpdaterBootstrap.exe" `
    -PrivateKeyPath "keys\update-signing-key.pem"
```

**Parametri:**

| Parametro | Obbligatorio | Descrizione |
|-----------|:---:|-------------|
| `-BinaryPath` | ✅ | Percorso del binario da firmare |
| `-PrivateKeyPath` | ✅ | Percorso della chiave privata RSA-4096 `.pem` |

**Differenze tra `sign-archive.ps1` e `sign-release.ps1`:**

| Aspetto | `sign-archive.ps1` | `sign-release.ps1` |
|---------|:---:|:---:|
| **Formato firma (.sig)** | Byte grezzi (512 B) | Base64 (testo) |
| **Auto-verifica** | ✅ Sì | ❌ No |
| **Supporto multi-file** | ✅ Archivio + binario | ❌ File singolo |
| **Output dettagliato** | ✅ Dettagliato | Minimale |
| **Uso consigliato** | Release con auto-update | Firme standalone |

---

### Verifica Manuale della Firma

Per verificare manualmente una firma (utile per il debug):

```powershell
# Verificare la firma dell'archivio
openssl dgst -sha256 -verify keys\update-signing-key.pub.pem -signature "UtilityPDF-v1.8.0.7z.sig" "UtilityPDF-v1.8.0.7z"
# Output atteso: "Verified OK"

# Verificare l'hash
$expectedHash = Get-Content "UtilityPDF-v1.8.0.7z.sha256"
$actualHash = (Get-FileHash "UtilityPDF-v1.8.0.7z" -Algorithm SHA256).Hash.ToLower()
if ($expectedHash -eq $actualHash) { Write-Host "Hash OK" } else { Write-Host "HASH NON CORRISPONDENTE!" }
```

---

## 🗺️ Riepilogo: Chi Firma Cosa e Quando

```
┌─────────────────────────────────────────────────────────────────┐
│                    CICLO DI VITA DELLA FIRMA                     │
│                                                                 │
│  UNA TANTUM (setup progetto):                                   │
│  ┌──────────────────────────────────────────┐                   │
│  │  generate-signing-keys.ps1               │                   │
│  │  → Genera coppia chiavi RSA-4096         │                   │
│  │  → Chiave privata → GitHub Secrets       │                   │
│  │  → Chiave pubblica → codice sorgente     │                   │
│  │    (UpdateValidator.cpp + BinaryIntegrity │                   │
│  │     Verifier.cs)                         │                   │
│  └──────────────────────────────────────────┘                   │
│                                                                 │
│  PER OGNI RELEASE:                                              │
│  ┌──────────────────────────────────────────┐                   │
│  │  Automatica (CI/CD):                     │                   │
│  │    git tag v1.8.0 && git push origin v1.8.0                  │
│  │    → release.yml gestisce tutto          │                   │
│  │                                          │                   │
│  │  Manuale:                                │                   │
│  │    sign-archive.ps1                      │                   │
│  │    → Firma .7z → genera .sha256 + .sig   │                   │
│  │    → Upload manuale su GitHub Release    │                   │
│  └──────────────────────────────────────────┘                   │
│                                                                 │
│  A RUNTIME (automatico, in UpdaterBootstrap.exe):               │
│  ┌──────────────────────────────────────────┐                   │
│  │  Scarica .7z + .sha256 + .sig            │                   │
│  │  → Verifica SHA-256 (integrità)          │                   │
│  │  → Verifica RSA-4096 (autenticità)       │                   │
│  │  → Solo se OK → estrae e aggiorna        │                   │
│  └──────────────────────────────────────────┘                   │
└─────────────────────────────────────────────────────────────────┘
```

---

## ⚙️ Configurazione Necessaria

### appsettings.json

```json
{
  "application": {
    "updateDirectory": "Z:\\Updates\\UtilityPDF"
  },
  "logging": {
    "logDirectory": "Z:\\Logs"
  }
}
```

### update_in_progress.json (creato da UtilityPDF.exe)

```json
{
  "currentVersion": "1.7.5",
  "newVersion": "1.8.0",
  "archiveUrl": "https://github.com/Firefox-1998/UtilityPDF/releases/download/v1.8.0/UtilityPDF-v1.8.0.7z",
  "hashUrl": "https://github.com/Firefox-1998/UtilityPDF/releases/download/v1.8.0/UtilityPDF-v1.8.0.7z.sha256",
  "signatureUrl": "https://github.com/Firefox-1998/UtilityPDF/releases/download/v1.8.0/UtilityPDF-v1.8.0.7z.sig"
}
```

---

## 🚀 Deployment — Struttura Finale

```
UtilityPDF/
├── UpdaterBootstrap.exe           [C++ Nativo ~250 KB]
├── UtilityPDF.exe                 [Applicazione principale]
├── config/
│   └── appsettings.json           [Configurazione]
├── backups/
│   └── pre-update_v1.7.5.zip     [Backup selettivo pre-aggiornamento]
├── temp_update/                   [Temporanea — eliminata dopo l'aggiornamento]
│   ├── update_archive.7z
│   ├── update_archive.sha256
│   ├── update_archive.sig
│   └── extracted/                 [File estratti dal .7z]
└── logs/
    └── update_native_20260130.log [Log strutturato]
```

---

## 📊 Confronto con Approcci Alternativi

| Aspetto | UpdaterBootstrap (C++ Nativo) | Approccio .NET | PowerShell |
|---------|:---:|:---:|:---:|
| **Dimensione** | ~250 KB | ~2 MB | N/A |
| **Avvio** | Istantaneo | ~500ms | ~1s |
| **Dipendenze runtime** | Zero | .NET Runtime | Policy PS |
| **Estrazione 7z** | LZMA SDK nativo | Nessuna | Solo .zip |
| **Backup zip** | minizip nativo | System.IO.Compression | Compress-Archive |
| **Verifica RSA** | BCrypt API nativa | RSACryptoServiceProvider | OpenSSL esterno |
| **DLL esterne** | Zero | Molte | N/A |
| **Firma crittografica** | SHA-256 + RSA-4096 | SHA-256 + RSA-4096 | N/A |

---

## 🐛 Risoluzione Problemi

### nlohmann-json non trovato

Il pacchetto è gestito tramite NuGet:

```bash
nuget restore
```

### Errori di compilazione sui file 7z SDK / zlib

Verificare che:
1. I sorgenti siano presenti in `lib\7zSDK\`, `lib\zlib\`, `lib\minizip\`
2. I file `.c` siano configurati con `CompileAs: CompileAsC` nel `.vcxproj`
3. `_7ZIP_ST` sia definito nelle definizioni del preprocessore per i file 7z SDK

### Firma non valida a runtime

1. Verificare che la chiave pubblica nel codice sorgente corrisponda alla chiave privata usata per la firma
2. Verificare che il file `.sig` sia in formato **byte grezzi** (512 byte per RSA-4096), non Base64
3. Verificare che il file `.sha256` contenga l'hash in **hex minuscolo** senza spazi o newline finali

### Errore "OpenSSL not found" negli script

Installare Git for Windows (include OpenSSL) o aggiungere OpenSSL al PATH:

```powershell
# Verificare se OpenSSL è disponibile
openssl version

# Se non trovato, usare quello di Git for Windows
$env:PATH += ";C:\Program Files\Git\usr\bin"
openssl version
```

---

## 📝 Licenza

MIT License — Vedi [LICENSE](../LICENSE) del progetto principale

## Versione

Versione Corrente: **v2.0.0-preview.1**

## 🙏 Crediti

- **nlohmann-json**: https://github.com/nlohmann/json — Licenza MIT
- **7-Zip LZMA SDK**: https://www.7-zip.org/sdk.html — Pubblico Dominio
- **zlib**: https://zlib.net/ — Licenza zlib
- **minizip**: (da zlib/contrib) — Licenza zlib
- **Visual Studio 2022**: Microsoft
- **OpenSSL**: https://www.openssl.org/ — Licenza Apache 2.0

---

**UpdaterBootstrap — Aggiornamenti trasparenti, zero sforzo.**
