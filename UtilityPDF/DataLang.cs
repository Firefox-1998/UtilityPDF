using System;

namespace UtilityPDF
{
    /// <summary>
    /// Represents OCR language data
    /// </summary>
    internal sealed class DataLang : IEquatable<DataLang>
    {
        /// <summary>
        /// ISO language code (e.g., "eng", "ita", "fra")
        /// </summary>
        public string LangParam1 { get; set; }

        /// <summary>
        /// Full language name (e.g., "English", "Italiano", "Français")
        /// </summary>
        public string LangParam2 { get; set; }

        /// <summary>
        /// Traineddata filename (e.g., "eng.traineddata")
        /// </summary>
        public string LangParam3 { get; set; }

        /// <summary>
        /// Initializes a new instance of the DataLang class
        /// </summary>
        public DataLang()
        {
            LangParam1 = string.Empty;
            LangParam2 = string.Empty;
            LangParam3 = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the DataLang class with parameters
        /// </summary>
        public DataLang(string langParam1, string langParam2, string langParam3)
        {
            LangParam1 = langParam1 ?? string.Empty;
            LangParam2 = langParam2 ?? string.Empty;
            LangParam3 = langParam3 ?? string.Empty;
        }

        /// <summary>
        /// Returns the full language name for display purposes
        /// </summary>
        public override string ToString()
        {
            return LangParam2;
        }

        /// <summary>
        /// Determines whether the specified DataLang is equal to the current DataLang
        /// </summary>
        public bool Equals(DataLang other)
        {
            if (other == null)
            {
                return false;
            }

            return string.Equals(LangParam3, other.LangParam3, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current DataLang
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as DataLang);
        }

        /// <summary>
        /// Returns a hash code for this instance
        /// </summary>
        public override int GetHashCode()
        {
            return LangParam3?.ToUpperInvariant().GetHashCode() ?? 0;
        }
    }
}
