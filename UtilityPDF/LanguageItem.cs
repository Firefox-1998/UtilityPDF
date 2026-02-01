namespace UtilityPDF
{
    /// <summary>
    /// Rappresenta un elemento lingua per la ComboBox
    /// </summary>
    internal class LanguageItem
    {
        public string CultureCode { get; set; }
        public string Flag { get; set; }
        public string DisplayName { get; set; }

        public LanguageItem(string cultureCode)
        {
            CultureCode = cultureCode;
            Flag = LocalizationManager.GetLanguageFlag(cultureCode);
            DisplayName = LocalizationManager.GetLanguageDisplayName(cultureCode);
        }

        public override string ToString()
        {
            return $"{Flag} {DisplayName} ({CultureCode})";
        }
    }
}