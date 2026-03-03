using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UtilityPDF.UI;
using UtilityPDF.Resources;

namespace UtilityPDF.Localization
{
    /// <summary>
    /// Loads available OCR language data from a CSV file
    /// </summary>
    internal class DataLangLoader
    {
        private static readonly ReadOnlyCollection<DataLang> EmptyCollection =
            new ReadOnlyCollection<DataLang>(Array.Empty<DataLang>());

        /// <summary>
        /// Loads language data from the specified CSV file
        /// </summary>
        public ReadOnlyCollection<DataLang> LoadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                DisplayError.LogWarning(string.Format(Strings.Log_CsvNotFound, filePath));
                return EmptyCollection;
            }

            try
            {
                List<DataLang> dataList = File.ReadLines(filePath)
                    .Skip(1) // Skip header row
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(';'))
                    .Where(parts => parts.Length >= 3)
                    .Select(parts => new DataLang
                    {
                        LangParam1 = parts[0].Trim(),
                        LangParam2 = parts[1].Trim(),
                        LangParam3 = parts[2].Trim()
                    })
                    .ToList();

                DisplayError.LogInfo(string.Format(Strings.LogNumLangCsv, dataList.Count));
                return new ReadOnlyCollection<DataLang>(dataList);
            }
            catch (Exception ex)
            {
                DisplayError.LogCustomError(string.Format(Strings.LogFailedLoadLang, ex.Message), new Dictionary<string, string>
                {
                    { Strings.Log_FilePath, filePath },
                    { Strings.Log_ErrorType, ex.GetType().Name }
                });
                DisplayError.ErrorGeneric(ex);
                return EmptyCollection;
            }
        }

        /// <summary>
        /// Gets all traineddata files from the specified folder
        /// </summary>
        public string[] GetFiles(string exeDirectory, string trainerDataFolder, string csvFilename)
        {
            string tessDataPath = Path.Combine(exeDirectory, trainerDataFolder);

            if (!Directory.Exists(tessDataPath))
            {
                DisplayError.LogWarning(string.Format(Strings.Log_TessDataDir, tessDataPath));
                return Array.Empty<string>();
            }

            try
            {
                string[] allFiles = Directory.GetFiles(tessDataPath, "*.traineddata");
                DisplayError.LogInfo(string.Format(Strings.Log_NumTessData, allFiles.Length, tessDataPath));
                return allFiles;
            }
            catch (Exception ex)
            {
                DisplayError.LogCustomError(string.Format(Strings.Log_FailedTessData, ex.Message), new Dictionary<string, string>
                {
                    { Strings.Log_Dir, tessDataPath },
                    { Strings.Log_ErrorType, ex.GetType().Name }
                });
                DisplayError.ErrorGeneric(ex);
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Searches for a DataLang element by traineddata filename
        /// </summary>
        public DataLang FindByParam3(ReadOnlyCollection<DataLang> dataList, string fileName)
        {
            if (dataList == null || string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            DataLang result = dataList.FirstOrDefault(data =>
                data.LangParam3.Equals(fileName, StringComparison.OrdinalIgnoreCase));

            return result;
        }
    }
}
