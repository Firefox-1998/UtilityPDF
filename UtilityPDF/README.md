# UtilityPDF

This software was created, totally free, to facilitate and collect in one place, some operations that are normally done on PDF files.

## Features

Currently the possible operations are:

* **PDF Compression** - Compress PDF files by adjusting the compression factor with 4 different quality levels (prepress, printer, ebook, screen);
* **Text Extraction** - Extract text from PDF via OCR with support for multiple languages using Tesseract;
* **PDF Merging** - Merge multiple PDF files following the sequence of adding files, creating a new combined PDF file;
* **PDF Conversion** - Convert PDF files to editable DOCX and/or RTF formats. >>> No Office installation required <<< to convert.
  If PDF contains only images, output DOCX and/or RTF files will have only images. Use text extraction (with OCR) to convert text inside images.

## Modern UI Interface 🎨

The application features a **completely redesigned modern interface** with:

### Custom Controls
- **ModernCard** - Stylish card containers with colored headers and rounded corners for each operation section
- **ModernButton** - Smooth gradient buttons with hover effects and rounded borders
- **ModernProgressBar** - Enhanced progress bar with percentage display and smooth animations
- **LoadingSpinner** - Animated circular spinner for visual feedback during long operations

### Visual Enhancements
- **Color-coded sections**: Each operation has its own distinctive color theme:
  - 🔵 Text Extraction (OCR): Blue (`#3498DB`)
  - 🟣 PDF Merging: Purple (`#9B59B6`)
  - 🟠 PDF Compression: Orange (`#E67E22`)
  - 🟢 PDF Conversion: Teal (`#1ABC9C`)
- **Emoji icons** throughout the interface for better visual recognition
- **Responsive layout** with centered, organized panels
- **Smooth animations** for loading states and transitions
- **Professional color palette** with subtle shadows and modern styling

## Multilingual Interface 🌍

The application now supports **7 languages** with real-time interface switching:
- 🇺🇸 English (default)
- 🇮🇹 Italiano
- 🇫🇷 Français
- 🇩🇪 Deutsch
- 🇪🇸 Español
- 🇵🇹 Português
- 🇬🇷 Ελληνικά

Users can change the interface language at runtime using the language selector without restarting the application.

## Advanced Configuration System ⚙️

The application includes a robust configuration management system:

### Features
- **JSON-based configuration** - Settings stored in `AppSettings.json` for easy customization
- **Persistent user preferences** - Language selection and other settings are automatically saved
- **Flexible settings structure** - Organized configuration with Application, Paths, and Processing sections
- **Automatic configuration loading** - Settings are loaded at startup and applied throughout the application

### Configuration Structure
- `ApplicationSettings` - General settings for the application
  - `DefaultLanguage` - Sets the default interface language on startup
  - `Theme` - Specifies the color theme (light or dark mode)
- `PathsSettings` - File path configurations
  - `InputDirectory` - Default directory for input files
  - `OutputDirectory` - Default directory for saving output files
- `ProcessingSettings` - Options related to file processing
  - `OCRLanguage` - Language selection for OCR text extraction
  - `CompressionLevel` - Default compression level for PDF compression

### LocalizationManager
- **Culture-aware localization** - Automatically applies the selected language to all UI elements
- **Resource-based translations** - Uses .NET resource files for efficient multilingual support
- **Dynamic language switching** - Changes take effect immediately without application restart

## Auto-Update System 🔄

The application includes a **secure automatic update system** powered by a native C++ updater:

### How it works

1. **UtilityPDF.exe** checks for new versions via the GitHub Releases API
2. If an update is available, it creates an `update_in_progress.json` marker and launches `UpdaterBootstrap.exe`
3. **UpdaterBootstrap.exe** (native C++, ~250 KB, zero .NET dependencies) handles the entire update process:

```
Download .7z + .sha256 + .sig from GitHub Release
  → Verify SHA-256 hash (integrity)
  → Verify RSA-4096 signature (authenticity)
  → Extract .7z archive (7-Zip LZMA SDK, native)
  → Selective backup of only files being overwritten → .zip (minizip, native)
  → Apply update files to application directory
  → On error: automatic rollback from .zip backup
  → Restart UtilityPDF.exe
```

### Security

Every release is cryptographically signed with **SHA-256 + RSA-4096**. The public key is embedded in both binaries. The private key is stored exclusively in GitHub Secrets and never leaves CI/CD.

### Backup & Rollback

- Before updating, a **selective backup** is created containing only the files that will be overwritten
- Backups are saved as `backups\pre-update_v{version}.zip`
- If the update fails, an **automatic rollback** restores the previous version

> For detailed documentation see [UpdaterBootstrap.Native/README_EN.md](../UpdaterBootstrap.Native/README_EN.md)

## Technical Details

**Target Framework:** .NET Framework 4.8

**Used Libraries (UtilityPDF — C#):**

* FreeSpire.Doc v. 12.2.0 - ([Free Spire.Doc for .NET](https://www.e-iceblue.com/Introduce/free-doc-component.html))
  Free Spire.Doc for .NET is a Community Edition of the Spire.Doc for .NET, which is a totally free word API for commercial and personal use.
* Freeware.Pdf2Png v. 1.0.1 - MIT License
* Freeware.Pdf2Docx v. 1.1.0 - MIT License
* Ghostscript.NET v. 1.3.2 - AGPL (GNU Affero General Public License)
* PDFsharp v. 6.2.4 - MIT License	
* Tesseract v. 5.2.0 - Apache License

**Used Libraries (UpdaterBootstrap — C++20):**

| Library | Purpose | License |
|---------|---------|---------|
| nlohmann-json 3.12.0 | JSON parsing | MIT |
| 7-Zip LZMA SDK 24.07 | .7z extraction | Public Domain |
| zlib 1.3.1 | Deflate compression | zlib License |
| minizip | .zip creation/extraction | zlib License |

All library dependencies are **MIT** / **AGPL** / **Apache** / **zlib** / **Public Domain** licensed.

## Tesseract OCR Setup

For **TESSERACT traineddata** (LSTM only - best quality), put the trained language files in the `tessdata` directory.

Download languages from: https://github.com/tesseract-ocr/tessdata_best

The application automatically detects available language files and populates the language selector for OCR operations.

## Code Architecture 🏗️

The application follows modern software engineering practices:

### Namespaces & Organization (UtilityPDF — C#)
- **UtilityPDF.Configuration** - Configuration management and settings persistence
- **UtilityPDF.Controls** - Custom modern UI controls (ModernCard, ModernButton, ModernProgressBar, LoadingSpinner)
- **UtilityPDF.Localization** - Multilingual support and culture management
- **UtilityPDF.Processing** - PDF processing operations (compression, merging, conversion, text extraction)
- **UtilityPDF.Resources** - Localized string resources for all supported languages
- **UtilityPDF.Security** - Binary integrity verification (SHA-256 + RSA-4096)
- **UtilityPDF.UI** - UI helper classes (ProgressHelper, GraphicsHelper, ControlTextImgAssigner)

### Modules (UpdaterBootstrap — C++20)
- **ArchiveManager** - .7z extraction (LZMA SDK) + .zip compression (minizip + zlib)
- **BackupService** - Selective pre-update backup + restore from .zip
- **ConfigReader / ConfigMerger** - JSON configuration reading and merge
- **UpdateValidator** - SHA-256 hash + RSA-4096 signature verification via BCrypt

### Design Patterns
- **Separation of Concerns** - Clear separation between UI, business logic, and configuration
- **Helper Classes** - Dedicated utility classes for progress management, graphics operations, and text assignments
- **Resource Management** - Proper disposal of resources and memory management
- **Async/Await Pattern** - Non-blocking operations for better user experience during long-running tasks

## License

This software is released under the **MIT License**

Copyright © [2026] [G.L. Develop aka Firefox_1998]

## Version

Current Version: **v2.0.0-preview.1**

---

*UtilityPDF - Your complete toolkit for PDF operations*
