using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UtilityPDF
{
    internal class DataLangLoader
    {
        public IReadOnlyList<LangData> LoadData(string csvFilePath)
        {
            var dataList = new List<LangData>();

            // Leggi il file CSV e popola la collection
            using (var reader = new StreamReader(csvFilePath))
            {
                // Leggi la prima riga (nomi dei parametri)
                var headerLine = reader.ReadLine();
                var headers = headerLine.Split(';');

                // Leggi le righe successive (dati)
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    var data = new LangData
                    {
                        LangParam1 = values[0],
                        LangParam2 = values[1],
                        LangParam3 = values[2]
                    };

                    dataList.Add(data);
                }
            }

            // Converti la lista in una collection di sola lettura
            return dataList.AsReadOnly();
        }

        public IEnumerable<string> GetFiles(string exeDirectory, string trainerDataFolder, string csvFilename)
        {
            // Ottieni tutti i file nella cartella tessdata (escludendo il file CSV)
            return Directory.GetFiles(Path.Combine(exeDirectory, trainerDataFolder))
                            .Where(file => !file.EndsWith(csvFilename));
        }

        public LangData FindByParam3(IReadOnlyList<LangData> dataList, string param3)
        {
            return dataList.FirstOrDefault(data => data.LangParam3 == param3);
        }
    }
}
