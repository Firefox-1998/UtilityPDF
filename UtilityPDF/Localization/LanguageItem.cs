using UtilityPDF.Localization;

namespace UtilityPDF
{
    /// <summary>
    /// Represents a language item for the ComboBox
    /// </summary>
    internal sealed class LanguageItem
    {
        /// <summary>
        /// Gets the culture code (e.g., "en-US", "it-IT")
        /// </summary>
        public string CultureCode { get; }

        /// <summary>
        /// Gets the flag emoji for this language
        /// </summary>
        public string Flag { get; }

        /// <summary>
        /// Gets the display name of the language
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Initializes a new instance of the LanguageItem class
        /// </summary>
        public LanguageItem(string cultureCode)
        {
            CultureCode = cultureCode;
            Flag = LocalizationManager.GetLanguageFlag(cultureCode);
            DisplayName = LocalizationManager.GetLanguageDisplayName(cultureCode);
        }

        /// <summary>
        /// Returns the string representation for display in ComboBox
        /// </summary>
        public override string ToString()
        {
            return $"{Flag} {DisplayName}";
        }
    }
}
