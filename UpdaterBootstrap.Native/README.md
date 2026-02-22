# 🚀 UpdaterBootstrap.Native

[🇮🇹 Italiano](README_IT.md) | [🇬🇧 English](README_EN.md)

---

## 🎯 What is it? | Cos'è?

**EN:** A 100% native C++ updater - completely independent from .NET Runtime.  
**IT:** Un updater 100% nativo in C++ - completamente indipendente dal .NET Runtime.

---

## ✨ Key Features | Caratteristiche Principali

✅ **Zero .NET dependencies** | Zero dipendenze .NET  
✅ **Independent JSON reading** | Lettura JSON indipendente  
✅ **Complete update logic in C++** | Logica aggiornamento completa in C++  
✅ **Automatic backup/rollback** | Backup/rollback automatico  
✅ **Security validations** | Validazioni di sicurezza  
✅ **Network share support** | Supporto network share  
✅ **Case-insensitive JSON** (PascalCase + camelCase)  
✅ **Structured file logging** | Logging strutturato  

---

## 📦 Build Requirements | Requisiti di Build

- **Visual Studio 2022** (v17.8+)
- **Windows SDK 10.0**
- **C++20 Standard**
- **vcpkg** (nlohmann-json)

---

## 🔧 Quick Build | Build Rapida

```bash
# Install dependencies | Installa dipendenze
vcpkg install nlohmann-json:x64-windows
vcpkg integrate install

# Build | Compila
msbuild UpdaterBootstrap.Native.sln /p:Configuration=Release /p:Platform=x64

# Output | Output
bin\x64\Release\UpdaterBootstrap.exe (~200 KB)
```

---

## 📚 Documentation | Documentazione

- **🇮🇹 Documentazione Italiana**: [README_IT.md](README_IT.md)
- **🇬🇧 English Documentation**: [README_EN.md](README_EN.md)

---

## 📊 Comparison | Confronto

| Aspect | Native C++ | .NET Only |
|--------|-----------|-----------|
| **Size | Dimensione** | ~200 KB | ~2 MB |
| **Startup | Avvio** | Instant | ~500ms |
| **Dependencies | Dipendenze** | Zero | .NET 9 Runtime |
| **Complete Logic | Logica Completa** | ✅ Yes/Sì | ✅ Yes/Sì |
| **Backup/Restore** | ✅ Yes/Sì | ✅ Yes/Sì |

---

## 🎉 Result | Risultato

**EN:** UpdaterBootstrap.Native is now 100% autonomous - no longer depends on .NET!  
**IT:** UpdaterBootstrap.Native è ora 100% autonomo - non dipende più da .NET!

---

## 📝 License | Licenza

MIT License - See | Vedi [LICENSE](../LICENSE)

---

**Built with ❤️ for UtilityPDF**

