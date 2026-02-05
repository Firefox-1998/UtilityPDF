using Ghostscript.NET;
using System;
using System.IO;
using System.Reflection;

namespace UtilityPDF
{
    /// <summary>
    /// Application configuration settings and paths
    /// </summary>
    internal static class SettingsString
    {
        /// <summary>
        /// Tessdata folder name for OCR trained data
        /// </summary>
        public const string TrainerDataFolder = "tessdata";

        /// <summary>
        /// CSV filename containing language data
        /// </summary>
        public const string CsvLangFilename = "lang.csv";

        /// <summary>
        /// Application binary path (lazy initialized)
        /// </summary>
        private static readonly Lazy<string> lazyBinPath = new Lazy<string>(() =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        /// <summary>
        /// Ghostscript DLL path (lazy initialized)
        /// </summary>
        private static readonly Lazy<string> lazyGsDllPath = new Lazy<string>(() =>
            Path.Combine(BinPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll"));

        /// <summary>
        /// Ghostscript version info (lazy initialized)
        /// </summary>
        private static readonly Lazy<GhostscriptVersionInfo> lazyGvi = new Lazy<GhostscriptVersionInfo>(() =>
            new GhostscriptVersionInfo(GsDllPath));

        /// <summary>
        /// Gets the application binary path
        /// </summary>
        public static string BinPath => lazyBinPath.Value;

        /// <summary>
        /// Gets the Ghostscript DLL path
        /// </summary>
        public static string GsDllPath => lazyGsDllPath.Value;

        /// <summary>
        /// Gets the Ghostscript version info
        /// </summary>
        public static GhostscriptVersionInfo Gvi => lazyGvi.Value;

        /// <summary>
        /// Gets the full path to tessdata folder
        /// </summary>
        public static string TessDataPath => Path.Combine(BinPath, TrainerDataFolder);

        /// <summary>
        /// Gets the full path to language CSV file
        /// </summary>
        public static string LangCsvPath => Path.Combine(TessDataPath, CsvLangFilename);
    }
}
