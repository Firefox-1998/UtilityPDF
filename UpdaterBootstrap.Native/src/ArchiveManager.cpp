#include "ArchiveManager.h"
#include "Logger.h"
#include <algorithm>
#include <fstream>
#include <mutex>

// 7-Zip LZMA SDK (pure C) — for .7z extraction
extern "C" {
#include "7z.h"
#include "7zAlloc.h"
#include "7zBuf.h"
#include "7zCrc.h"
#include "7zFile.h"
#include "7zTypes.h"
}

// minizip (from zlib) — for .zip creation and extraction
#include "zip.h"
#include "unzip.h"
#include "iowin32.h"

static const ISzAlloc g_Alloc = { SzAlloc, SzFree };
static const ISzAlloc g_AllocTemp = { SzAllocTemp, SzFreeTemp };
static std::once_flag g_CrcInitFlag;

void ArchiveManager::InitCrc()
{
    std::call_once(g_CrcInitFlag, []() { CrcGenerateTable(); });
}

bool ArchiveManager::EnsureDirectoryExists(const std::wstring& filePath)
{
    try
    {
        fs::path parentDir = fs::path(filePath).parent_path();
        if (!parentDir.empty() && !fs::exists(parentDir))
        {
            fs::create_directories(parentDir);
        }
        return true;
    }
    catch (...) { return false; }
}

bool ArchiveManager::IsSupportedArchive(const std::wstring& filePath)
{
    std::wstring ext = fs::path(filePath).extension().wstring();
    std::transform(ext.begin(), ext.end(), ext.begin(), ::towlower);
    return ext == L".7z" || ext == L".zip";
}

// ============================================================================
// 7z EXTRACTION (for GitHub release archives)
// ============================================================================
ArchiveManager::ArchiveResult ArchiveManager::Extract7z(
    const std::wstring& archivePath,
    const std::wstring& destDir,
    ProgressCallback progressCallback)
{
    ArchiveResult result;
    CFileInStream archiveStream;
    CLookToRead2 lookStream;
    CSzArEx db;

    UInt32 blockIndex = 0xFFFFFFFF;
    Byte* outBuffer = nullptr;
    size_t outBufferSize = 0;

    static const size_t kInputBufSize = (1 << 18); // 256 KB
    Byte* lookStreamBuf = nullptr;
    bool dbInitialized = false;
    bool fileOpened = false;

    try
    {
        if (!fs::exists(archivePath))
        {
            result.message = L"Archive file not found: " + archivePath;
            Logger::LogError(L"EXTRACT_FILE_NOT_FOUND", result.message);
            return result;
        }

        if (!fs::exists(destDir))
        {
            fs::create_directories(destDir);
        }

        Logger::LogInfo(L"EXTRACT_7Z_START",
            L"Extracting .7z: " + archivePath + L" -> " + destDir);

        InitCrc();

        WRes wres = InFile_OpenW(&archiveStream.file, archivePath.c_str());
        if (wres != 0)
        {
            result.message = L"Cannot open archive file: " + archivePath;
            Logger::LogError(L"EXTRACT_OPEN_FAILED", result.message);
            return result;
        }
        fileOpened = true;
        FileInStream_CreateVTable(&archiveStream);

        lookStreamBuf = static_cast<Byte*>(ISzAlloc_Alloc(&g_Alloc, kInputBufSize));
        if (!lookStreamBuf)
        {
            File_Close(&archiveStream.file);
            result.message = L"Memory allocation failed";
            Logger::LogError(L"EXTRACT_ALLOC_FAILED", result.message);
            return result;
        }

        LookToRead2_CreateVTable(&lookStream, False);
        lookStream.buf = lookStreamBuf;
        lookStream.bufSize = kInputBufSize;
        lookStream.realStream = &archiveStream.vt;
        LookToRead2_INIT(&lookStream)

        SzArEx_Init(&db);
        dbInitialized = true;

        SRes res = SzArEx_Open(&db, &lookStream.vt, &g_Alloc, &g_AllocTemp);
        if (res != SZ_OK)
        {
            result.message = L"Failed to open .7z archive (SRes: " + std::to_wstring(res) + L")";
            Logger::LogError(L"EXTRACT_OPEN_7Z_FAILED", result.message);
        }
        else
        {
            UInt32 numItems = db.NumFiles;
            int filesExtracted = 0;
            uintmax_t totalSize = 0;

            Logger::LogInfo(L"EXTRACT_ITEMS",
                L"Archive contains " + std::to_wstring(numItems) + L" items");

            for (UInt32 i = 0; i < numItems; i++)
            {
                if (progressCallback && numItems > 0)
                {
                    progressCallback(static_cast<int>((i * 100) / numItems));
                }

                // Get file name (UTF-16)
                size_t nameLen = SzArEx_GetFileNameUtf16(&db, i, nullptr);
                std::vector<UInt16> nameBuf(nameLen);
                SzArEx_GetFileNameUtf16(&db, i, nameBuf.data());

                std::wstring fileName(
                    reinterpret_cast<const wchar_t*>(nameBuf.data()),
                    nameLen > 0 ? nameLen - 1 : 0);

                fs::path outputPath = fs::path(destDir) / fileName;

                if (SzArEx_IsDir(&db, i))
                {
                    if (!fs::exists(outputPath))
                    {
                        fs::create_directories(outputPath);
                    }
                    continue;
                }

                EnsureDirectoryExists(outputPath.wstring());

                size_t offset = 0;
                size_t outSizeProcessed = 0;

                res = SzArEx_Extract(&db, &lookStream.vt, i,
                    &blockIndex, &outBuffer, &outBufferSize,
                    &offset, &outSizeProcessed,
                    &g_Alloc, &g_AllocTemp);

                if (res != SZ_OK)
                {
                    Logger::LogWarning(L"EXTRACT_FILE_ERROR",
                        L"Failed to extract: " + fileName);
                    continue;
                }

                std::ofstream outFile(outputPath, std::ios::binary);
                if (outFile.is_open())
                {
                    outFile.write(
                        reinterpret_cast<const char*>(outBuffer + offset),
                        static_cast<std::streamsize>(outSizeProcessed));
                    outFile.close();
                    filesExtracted++;
                    totalSize += outSizeProcessed;
                }

                if (SzBitWithVals_Check(&db.Attribs, i))
                {
                    SetFileAttributesW(outputPath.c_str(), db.Attribs.Vals[i]);
                }
            }

            if (progressCallback) { progressCallback(100); }

            result.success = true;
            result.filesProcessed = filesExtracted;
            result.totalSize = totalSize;
            result.message = L"Extraction completed successfully";

            Logger::LogInfo(L"EXTRACT_7Z_SUCCESS",
                L"Extracted " + std::to_wstring(filesExtracted) + L" files (" +
                std::to_wstring(totalSize / 1024) + L" KB)");
        }

        // Cleanup
        ISzAlloc_Free(&g_Alloc, outBuffer);
        if (dbInitialized) { SzArEx_Free(&db, &g_Alloc); }
        ISzAlloc_Free(&g_Alloc, lookStreamBuf);
        if (fileOpened) { File_Close(&archiveStream.file); }
    }
    catch (const std::exception& ex)
    {
        ISzAlloc_Free(&g_Alloc, outBuffer);
        if (dbInitialized) { SzArEx_Free(&db, &g_Alloc); }
        if (lookStreamBuf) { ISzAlloc_Free(&g_Alloc, lookStreamBuf); }
        if (fileOpened) { File_Close(&archiveStream.file); }

        std::string errorMsg = ex.what();
        result.message = L"Extraction exception: " +
            std::wstring(errorMsg.begin(), errorMsg.end());
        Logger::LogError(L"EXTRACT_7Z_EXCEPTION", result.message);
    }

    return result;
}

// ============================================================================
// ZIP COMPRESSION (for backup: pre-update_v{version}.zip)
// ============================================================================
ArchiveManager::ArchiveResult ArchiveManager::CompressZip(
    const std::wstring& sourceDir,
    const std::wstring& zipPath,
    ProgressCallback progressCallback)
{
    ArchiveResult result;

    try
    {
        if (!fs::exists(sourceDir))
        {
            result.message = L"Source directory not found: " + sourceDir;
            Logger::LogError(L"ZIP_DIR_NOT_FOUND", result.message);
            return result;
        }

        EnsureDirectoryExists(zipPath);

        if (fs::exists(zipPath))
        {
            fs::remove(zipPath);
        }

        Logger::LogInfo(L"ZIP_COMPRESS_START",
            L"Creating zip: " + sourceDir + L" -> " + zipPath);

        // Collect files first for progress tracking
        std::vector<fs::path> files;
        for (const auto& entry : fs::recursive_directory_iterator(sourceDir))
        {
            if (entry.is_regular_file())
            {
                files.push_back(entry.path());
            }
        }

        if (files.empty())
        {
            result.message = L"No files to compress";
            Logger::LogWarning(L"ZIP_EMPTY", result.message);
            return result;
        }

        // Open zip file using minizip with Win32 IO for Unicode path support
        zlib_filefunc64_def ffunc;
        fill_win32_filefunc64W(&ffunc);

        zipFile zf = zipOpen2_64(zipPath.c_str(), APPEND_STATUS_CREATE, nullptr, &ffunc);
        if (!zf)
        {
            result.message = L"Cannot create zip file: " + zipPath;
            Logger::LogError(L"ZIP_CREATE_FAILED", result.message);
            return result;
        }

        int filesCompressed = 0;
        uintmax_t totalSize = 0;
        constexpr size_t readBufSize = 64 * 1024; // 64 KB read buffer
        std::vector<char> readBuffer(readBufSize);

        for (size_t idx = 0; idx < files.size(); idx++)
        {
            if (progressCallback)
            {
                progressCallback(static_cast<int>((idx * 100) / files.size()));
            }

            // Compute relative path (UTF-8 for zip standard)
            fs::path relativePath = fs::relative(files[idx], sourceDir);
            std::u8string relPathU8 = relativePath.u8string();
            std::string relPathUtf8(reinterpret_cast<const char*>(relPathU8.data()), relPathU8.size());

            // Normalize path separators to '/' (zip standard)
            std::replace(relPathUtf8.begin(), relPathUtf8.end(), '\\', '/');

            // Get file time for zip entry
            zip_fileinfo zi = {};
            WIN32_FILE_ATTRIBUTE_DATA fad = {};
            if (GetFileAttributesExW(files[idx].c_str(), GetFileExInfoStandard, &fad))
            {
                FILETIME ftLocal;
                SYSTEMTIME st;
                FileTimeToLocalFileTime(&fad.ftLastWriteTime, &ftLocal);
                FileTimeToSystemTime(&ftLocal, &st);

                zi.tmz_date.tm_sec = st.wSecond;
                zi.tmz_date.tm_min = st.wMinute;
                zi.tmz_date.tm_hour = st.wHour;
                zi.tmz_date.tm_mday = st.wDay;
                zi.tmz_date.tm_mon = st.wMonth - 1;
                zi.tmz_date.tm_year = st.wYear;
            }

            // Open new entry in zip (Deflate compression, level 6)
            int err = zipOpenNewFileInZip(zf, relPathUtf8.c_str(), &zi,
                nullptr, 0, nullptr, 0, nullptr,
                Z_DEFLATED, 6);

            if (err != ZIP_OK)
            {
                Logger::LogWarning(L"ZIP_ENTRY_FAILED",
                    L"Failed to add entry: " + relativePath.wstring());
                continue;
            }

            // Read and write file data in chunks
            std::ifstream inFile(files[idx], std::ios::binary);
            if (inFile.is_open())
            {
                while (inFile.good())
                {
                    inFile.read(readBuffer.data(), readBufSize);
                    std::streamsize bytesRead = inFile.gcount();
                    if (bytesRead > 0)
                    {
                        zipWriteInFileInZip(zf, readBuffer.data(), static_cast<unsigned>(bytesRead));
                    }
                }
            }

            zipCloseFileInZip(zf);
            filesCompressed++;
            totalSize += fs::file_size(files[idx]);
        }

        zipClose(zf, nullptr);

        if (progressCallback) { progressCallback(100); }

        uintmax_t zipSize = fs::file_size(zipPath);
        int ratio = (totalSize > 0)
            ? static_cast<int>((zipSize * 100) / totalSize) : 0;

        result.success = true;
        result.filesProcessed = filesCompressed;
        result.totalSize = zipSize;
        result.message = L"Zip compression completed";

        Logger::LogInfo(L"ZIP_COMPRESS_SUCCESS",
            L"Created: " + zipPath + L" (" +
            std::to_wstring(zipSize / 1024) + L" KB, " +
            std::to_wstring(ratio) + L"% of original, " +
            std::to_wstring(filesCompressed) + L" files)");
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        result.message = L"Zip compression error: " +
            std::wstring(errorMsg.begin(), errorMsg.end());
        Logger::LogError(L"ZIP_COMPRESS_EXCEPTION", result.message);
    }

    return result;
}

// ============================================================================
// ZIP EXTRACTION (for backup restore)
// ============================================================================
ArchiveManager::ArchiveResult ArchiveManager::ExtractZip(
    const std::wstring& zipPath,
    const std::wstring& destDir,
    ProgressCallback progressCallback)
{
    ArchiveResult result;

    try
    {
        if (!fs::exists(zipPath))
        {
            result.message = L"Zip file not found: " + zipPath;
            Logger::LogError(L"ZIP_NOT_FOUND", result.message);
            return result;
        }

        if (!fs::exists(destDir))
        {
            fs::create_directories(destDir);
        }

        Logger::LogInfo(L"ZIP_EXTRACT_START",
            L"Extracting zip: " + zipPath + L" -> " + destDir);

        // Open zip with Win32 IO for Unicode support
        zlib_filefunc64_def ffunc;
        fill_win32_filefunc64W(&ffunc);

        unzFile uf = unzOpen2_64(zipPath.c_str(), &ffunc);
        if (!uf)
        {
            result.message = L"Cannot open zip file: " + zipPath;
            Logger::LogError(L"ZIP_OPEN_FAILED", result.message);
            return result;
        }

        // Get total number of entries
        unz_global_info64 gi;
        unzGetGlobalInfo64(uf, &gi);
        ZPOS64_T totalEntries = gi.number_entry;

        int filesExtracted = 0;
        uintmax_t totalSize = 0;
        constexpr size_t bufSize = 64 * 1024;
        std::vector<char> buffer(bufSize);

        int err = unzGoToFirstFile(uf);

        while (err == UNZ_OK)
        {
            if (progressCallback && totalEntries > 0)
            {
                progressCallback(static_cast<int>((filesExtracted * 100) / totalEntries));
            }

            // Get current file info
            unz_file_info64 fileInfo;
            char fileNameBuf[512];
            err = unzGetCurrentFileInfo64(uf, &fileInfo,
                fileNameBuf, sizeof(fileNameBuf),
                nullptr, 0, nullptr, 0);

            if (err != UNZ_OK)
            {
                break;
            }

            std::string fileName(fileNameBuf);

            // Normalize separators
            std::replace(fileName.begin(), fileName.end(), '/', '\\');

            // Security: reject path traversal
            if (fileName.find("..") != std::string::npos)
            {
                Logger::LogWarning(L"ZIP_PATH_TRAVERSAL",
                    L"Skipping suspicious path: " + std::wstring(fileName.begin(), fileName.end()));
                err = unzGoToNextFile(uf);
                continue;
            }

            fs::path outputPath = fs::path(destDir) / fs::path(std::u8string(
                reinterpret_cast<const char8_t*>(fileName.data()), fileName.size()));

            // Check if directory entry (name ends with separator)
            if (fileName.back() == '\\' || fileName.back() == '/')
            {
                if (!fs::exists(outputPath))
                {
                    fs::create_directories(outputPath);
                }
                err = unzGoToNextFile(uf);
                continue;
            }

            // Ensure parent directory exists
            EnsureDirectoryExists(outputPath.wstring());

            // Extract file
            if (unzOpenCurrentFile(uf) != UNZ_OK)
            {
                err = unzGoToNextFile(uf);
                continue;
            }

            std::ofstream outFile(outputPath, std::ios::binary);
            if (outFile.is_open())
            {
                int bytesRead = 0;
                do
                {
                    bytesRead = unzReadCurrentFile(uf, buffer.data(), static_cast<unsigned>(bufSize));
                    if (bytesRead > 0)
                    {
                        outFile.write(buffer.data(), bytesRead);
                        totalSize += bytesRead;
                    }
                } while (bytesRead > 0);

                outFile.close();
                filesExtracted++;
            }

            unzCloseCurrentFile(uf);
            err = unzGoToNextFile(uf);
        }

        unzClose(uf);

        if (progressCallback) { progressCallback(100); }

        result.success = true;
        result.filesProcessed = filesExtracted;
        result.totalSize = totalSize;
        result.message = L"Zip extraction completed";

        Logger::LogInfo(L"ZIP_EXTRACT_SUCCESS",
            L"Extracted " + std::to_wstring(filesExtracted) + L" files (" +
            std::to_wstring(totalSize / 1024) + L" KB)");
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        result.message = L"Zip extraction error: " +
            std::wstring(errorMsg.begin(), errorMsg.end());
        Logger::LogError(L"ZIP_EXTRACT_EXCEPTION", result.message);
    }

    return result;
}
