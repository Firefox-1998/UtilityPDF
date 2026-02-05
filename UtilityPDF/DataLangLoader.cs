using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace UtilityPDF
{
    /// <summary>
    /// Carica i dati delle lingue disponibili per l'OCR da un file CSV
    /// </summary>
    internal class DataLangLoader
    {
        /// <summary>
        /// Carica i dati delle lingue dal file CSV specificato
        /// </summary>
        public ReadOnlyCollection<DataLang> LoadData(string filePath)
        {
            List<DataLang> dataList = new List<DataLang>();

            if (!File.Exists(filePath))
            {
                return new ReadOnlyCollection<DataLang>(dataList);
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                int lineNumber = 0;
                foreach (string line in lines)
                {
                    lineNumber++;
                    
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    // Salta la prima riga (header)
                    if (lineNumber == 1)
                    {
                        continue;
                    }

                    // Usa TAB come separatore invece della virgola
                    string[] parts = line.Split(';');

                    if (parts.Length >= 3)
                    {
                        DataLang dataLang = new DataLang
                        {
                            LangParam1 = parts[0].Trim(),
                            LangParam2 = parts[1].Trim(),
                            LangParam3 = parts[2].Trim()
                        };

                        dataList.Add(dataLang);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError.ErrorGeneric(ex);
            }

            return new ReadOnlyCollection<DataLang>(dataList);
        }

        /// <summary>
        /// Ottiene tutti i file traineddata dalla cartella specificata
        /// </summary>
        public string[] GetFiles(string exeDirectory, string trainerDataFolder, string csvFilename)
        {
            string tessDataPath = Path.Combine(exeDirectory, trainerDataFolder);

            if (!Directory.Exists(tessDataPath))
            {
                return new string[0];
            }

            try
            {
                string[] allFiles = Directory.GetFiles(tessDataPath, "*.traineddata");                
                return allFiles;
            }
            catch (Exception ex)
            {
                DisplayError.ErrorGeneric(ex);
                return new string[0];
            }
        }

        /// <summary>
        /// Cerca un elemento DataLang per nome file traineddata
        /// </summary>
        public DataLang FindByParam3(ReadOnlyCollection<DataLang> dataList, string fileName)
        {
            if (dataList == null || string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            foreach (DataLang data in dataList)
            {                
                if (data.LangParam3.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return data;
                }
            }
            return null;
        }
    }
}
