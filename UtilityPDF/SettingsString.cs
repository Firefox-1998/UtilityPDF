using Ghostscript.NET;
using System;
using System.IO;
using System.Reflection;

namespace UtilityPDF
{
    internal class SettingsString
    {
        // Set Ghostscript dll path
        private static readonly string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");
        public static readonly string trainerDataFolder = "tessdata";
        public static readonly string csvLangFilename = "lang.csv";
        public static readonly GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(gsDllPath);
    }
}
