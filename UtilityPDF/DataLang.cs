namespace UtilityPDF
{
    /// <summary>
    /// Rappresenta i dati di una lingua per l'OCR
    /// </summary>
    internal class DataLang
    {
        /// <summary>
        /// Codice lingua ISO (es. "eng", "ita", "fra")
        /// </summary>
        public string LangParam1 { get; set; }

        /// <summary>
        /// Nome completo della lingua (es. "English", "Italiano", "Français")
        /// </summary>
        public string LangParam2 { get; set; }

        /// <summary>
        /// Nome del file traineddata (es. "eng.traineddata")
        /// </summary>
        public string LangParam3 { get; set; }

        /// <summary>
        /// Costruttore della classe DataLang
        /// </summary>
        public DataLang()
        {
            LangParam1 = string.Empty;
            LangParam2 = string.Empty;
            LangParam3 = string.Empty;
        }

        /// <summary>
        /// Costruttore con parametri
        /// </summary>
        public DataLang(string langParam1, string langParam2, string langParam3)
        {
            LangParam1 = langParam1;
            LangParam2 = langParam2;
            LangParam3 = langParam3;
        }
    }
}