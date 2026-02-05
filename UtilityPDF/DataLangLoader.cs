using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace UtilityPDF
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

                return new ReadOnlyCollection<DataLang>(dataList);
            }
            catch (Exception ex)
            {
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
                return Array.Empty<string>();
            }

            try
            {
                string[] allFiles = Directory.GetFiles(tessDataPath, "*.traineddata");
                return allFiles;
            }
            catch (Exception ex)
            {
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
