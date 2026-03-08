# 🚀 UpdaterBootstrap.Native

[🇮🇹 Italiano](README_IT.md) | [🇬🇧 English](README_EN.md)

---

## 🎯 What is it? | Cos'è?

**EN:** A 100% native C++ updater with cryptographic signing — completely independent from .NET Runtime.  
**IT:** Un updater 100% nativo in C++ con firma crittografica — completamente indipendente dal .NET Runtime.

---

## ✨ Key Features | Caratteristiche Principali

✅ **Zero .NET dependencies** | Zero dipendenze .NET  
✅ **Zero external DLLs** | Zero DLL esterne  
✅ **Native .7z extraction** | Estrazione .7z nativa (LZMA SDK)  
✅ **Native .zip backup** | Backup .zip nativo (minizip + zlib)  
✅ **SHA-256 + RSA-4096 verification** | Verifica SHA-256 + RSA-4096 (BCrypt)  
✅ **Selective backup** | Backup selettivo (solo file da aggiornare)  
✅ **Automatic rollback** | Rollback automatico  
✅ **HTTPS download** | Download HTTPS (urlmon)  
✅ **Structured logging** | Logging strutturato  
✅ **Native progress dialog** | Progress dialog Win32 nativa  

---

## 🔐 Security | Sicurezza

Every release is protected by | Ogni release è protetta da:
- **SHA-256** hash (integrity | integrità)
- **RSA-4096** signature (authenticity | autenticità)

Scripts:
- `scripts/generate-signing-keys.ps1` — Key generation | Generazione chiavi (one-time | una tantum)
- `scripts/sign-archive.ps1` — Sign .7z + .exe | Firma .7z + .exe
- `.github/workflows/release.yml` — Automated CI/CD | CI/CD automatico

---

## 📦 Build Requirements | Requisiti di Build

- **Visual Studio 2022** (v17.8+)
- **Windows SDK 10.0**
- **C++20 Standard**
- **NuGet** (nlohmann-json)
- **7-Zip LZMA SDK** + **zlib** + **minizip** (sources in `lib/`)

---

## 🔧 Quick Build | Build Rapida

```bash
nuget restore
msbuild UpdaterBootstrap.Native.vcxproj /p:Configuration=Release /p:Platform=x64

# Output
bin\x64\Release\UpdaterBootstrap.exe (~250 KB)
```

---

## 🔄 Update Flow | Flusso Aggiornamento

```
Download .7z + .sha256 + .sig from GitHub Release
  → Verify SHA-256 hash (integrity)
  → Verify RSA-4096 signature (authenticity)
  → Extract .7z (LZMA SDK native)
  → Selective backup of overwritten files → .zip (minizip native)
  → Apply update files
  → On error: automatic rollback from .zip
  → Restart application
```

---

## 📊 Comparison | Confronto

| Aspect | Native C++ | .NET Only |
|--------|-----------|-----------|
| **Size | Dimensione** | ~250 KB | ~2 MB |
| **Startup | Avvio** | Instant | ~500ms |
| **Dependencies | Dipendenze** | Zero | .NET Runtime |
| **7z Extraction | Estrazione 7z** | ✅ LZMA SDK | ❌ |
| **Zip Backup** | ✅ minizip | ✅ System.IO.Compression |
| **RSA Verification | Verifica RSA** | ✅ BCrypt | ✅ RSACryptoServiceProvider |
| **External DLLs | DLL esterne** | Zero | Many |

---

## 📚 Documentation | Documentazione

- **🇮🇹 Documentazione completa (IT)**: [README_IT.md](README_IT.md)
- **🇬🇧 Full documentation (EN)**: [README_EN.md](README_EN.md)

Includes | Include:
- Detailed signing guide | Guida dettagliata alla firma
- Library setup | Setup librerie
- Troubleshooting | Risoluzione problemi

---

## 📝 License | Licenza

MIT License — See | Vedi [LICENSE](../LICENSE)

---

**Built with ❤️ for UtilityPDF**

