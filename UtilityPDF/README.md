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

## Technical Details

**Target Framework:** .NET Framework 4.8

**Used Libraries:**

* FreeSpire.Doc v. 12.2.0 - ([Free Spire.Doc for .NET](https://www.e-iceblue.com/Introduce/free-doc-component.html))
  Free Spire.Doc for .NET is a Community Edition of the Spire.Doc for .NET, which is a totally free word API for commercial and personal use.
* Freeware.Pdf2Png v. 1.0.1 - MIT License
* Freeware.Pdf2Docx v. 1.1.0 - MIT License
* Ghostscript.NET v. 1.2.3.1 - AGPL (GNU Affero General Public License)
* PDFsharp v. 6.1.1 - MIT License	
* Tesseract v. 5.2.0 - Apache License

All library dependencies, mentioned above, are **MIT LICENSED**/**AGPL LICENSED**/**APACHE LICENSED**

## Tesseract OCR Setup

For **TESSERACT traineddata** (LSTM only - best quality), put the trained language files in the `tessdata` directory.

Download languages from: https://github.com/tesseract-ocr/tessdata_best

The application automatically detects available language files and populates the language selector for OCR operations.

## License

This software is released under the **MIT License**

Copyright © [2026] [G.L. Develop aka Firefox_1998]

## Version

Current Version: **1.7.0.0**

---

*UtilityPDF - Your complete toolkit for PDF operations*