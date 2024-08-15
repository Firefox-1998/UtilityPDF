using Ghostscript.NET;
using System.IO;
using System.Reflection;
using System;
using UtilityPDF.Properties;

namespace UtilityPDF
{
    internal class SettingsString
    {
        //Get, to settings, string message.
        public static readonly string WarnSelectOutDirTXT = Settings.Default.WarnSelectOutDirTXT;
        public static readonly string WarnAbortedExtraction = Settings.Default.WarnAbortedExtraction;
        public static readonly string InfoCompleteExtraction = Settings.Default.InfoCompleteExtraction;
        public static readonly string WarnSelectOutDirMERGE = Settings.Default.WarnSelectOutDirMERGE;
        public static readonly string WarnSelectOutDirCOMP = Settings.Default.WarnSelectOutDirCOMP;
        public static readonly string WarnSelectOutDirDOCX = Settings.Default.WarnSelectOutDirDOCX;
        public static readonly string WarnConfirmAbort = Settings.Default.WarnConfirmAbort;
        public static readonly string CompressLvl_0 = Settings.Default.CompressLvl_0;
        public static readonly string CompressLvl_1 = Settings.Default.CompressLvl_1;
        public static readonly string CompressLvl_2 = Settings.Default.CompressLvl_2;
        public static readonly string CompressLvl_3 = Settings.Default.CompressLvl_3;
        public static readonly string LblMsgInputPDF_Extr = Settings.Default.LblMsgInputPDF_Extr;
        public static readonly string LblMsgOutputDIR_Extr = Settings.Default.LblMsgOutputDIR_Extr;
        public static readonly string LblMsgOutputDIR_Merge = Settings.Default.LblMsgOutputDIR_Merge;
        public static readonly string LblMsgSelLang = Settings.Default.LblMsgSelLang;
        public static readonly string LblCompressionLvl = Settings.Default.LblCompressionLvl;
        public static readonly string LblPanelExtract = Settings.Default.LblPanelExtract;
        public static readonly string LblPanelCompress = Settings.Default.LblPanelCompress;
        public static readonly string LblPanelMerge = Settings.Default.LblPanelMerge;
        public static readonly string lbl_ConvDOCX = Settings.Default.LblPanelConvDOCX;
        public static readonly string TxtSelectPDFBtn = Settings.Default.TxtSelectPDFBtn;
        public static readonly string TxtResetBtn = Settings.Default.TxtResetBtn;
        public static readonly string TxtOutputDirBtn = Settings.Default.TxtOutputDirBtn;
        public static readonly string TxtAbortBtn = Settings.Default.TxtAbortBtn;
        public static readonly string TxtCompressBtn = Settings.Default.TxtCompressBtn;
        public static readonly string TxtExtractBtn = Settings.Default.TxtExtractBtn;
        public static readonly string TxtMergetBtn = Settings.Default.TxtMergetBtn;
        public static readonly string TxtConvertBtn = Settings.Default.TxtConvertBtn;
        public static readonly string TxtExitBtn = Settings.Default.TxtExitBtn;
        public static readonly string GenericMessageError = Settings.Default.GenericMessageError;
        public static readonly string SpecificMessageErrorIO = Settings.Default.SpecificMessageErrorIO;
        public static readonly string CompressCompleted = Settings.Default.CompressCompleted;
        public static readonly string MergeCompleted = Settings.Default.MergeCompleted;
        public static readonly string ConvertCompleted = Settings.Default.ConvertCompleted;
        public static readonly string LblCompressInProgress = Settings.Default.LblCompressInProgress;
        public static readonly string LblMergeInProgress = Settings.Default.LblMergeInProgress;
        public static readonly string LblConvertInProgress = Settings.Default.LblConvertInProgress;
        public static readonly string trainerDataFolder = Settings.Default.trainerDataFolder;
        public static readonly string csvLangFilename = Settings.Default.csvLangFilename;

        // Set Ghostscript dll path
        private static readonly string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");
        public static readonly GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(gsDllPath);
    }
}
