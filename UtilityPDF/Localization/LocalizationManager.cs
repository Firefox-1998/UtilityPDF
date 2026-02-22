using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace UtilityPDF.Localization
{
    /// <summary>
    /// Manages application localization and culture settings
    /// </summary>
    internal static class LocalizationManager
    {
        private static CultureInfo currentCulture;

        private static readonly Dictionary<string, (string DisplayName, string Flag)> SupportedLanguages =
            new Dictionary<string, (string, string)>
            {
                { "en-US", ("English", "🇺🇸") },
                { "it-IT", ("Italiano", "🇮🇹") },
                { "fr-FR", ("Français", "🇫🇷") },
                { "de-DE", ("Deutsch", "🇩🇪") },
                { "es-ES", ("Español", "🇪🇸") },
                { "pt-PT", ("Português", "🇵🇹") },
                { "el-GR", ("Ελληνικά", "🇬🇷") }
            };

        private const string DefaultCulture = "en-US";

        /// <summary>
        /// Gets the current culture
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get
            {
                currentCulture ??= Thread.CurrentThread.CurrentUICulture;
                return currentCulture;
            }
        }

        /// <summary>
        /// Sets the application culture
        /// </summary>
        /// <param name="cultureName">Culture name (e.g., "it-IT", "en-US")</param>
        public static void SetCulture(string cultureName)
        {
            try
            {
                currentCulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentUICulture = currentCulture;
                Thread.CurrentThread.CurrentCulture = currentCulture;
                Properties.Resources.Culture = currentCulture;
            }
            catch (CultureNotFoundException)
            {
                // Fallback to English if culture is not supported
                if (cultureName != DefaultCulture)
                {
                    SetCulture(DefaultCulture);
                }
            }
        }

        /// <summary>
        /// Gets the current application culture
        /// </summary>
        public static CultureInfo GetCurrentCulture()
        {
            if (currentCulture == null)
            {
                return CultureInfo.CurrentUICulture;
            }
            return currentCulture;
        }

        /// <summary>
        /// Gets the current language code
        /// </summary>
        public static string GetCurrentLanguageCode()
        {
            return GetCurrentCulture().Name;
        }

        /// <summary>
        /// Gets the array of supported language codes
        /// </summary>
        public static string[] GetSupportedLanguages()
        {
            string[] languages = new string[SupportedLanguages.Count];
            SupportedLanguages.Keys.CopyTo(languages, 0);
            return languages;
        }

        /// <summary>
        /// Gets all supported languages with details
        /// </summary>
        public static IReadOnlyDictionary<string, (string DisplayName, string Flag)> GetSupportedLanguagesWithDetails()
        {
            return SupportedLanguages;
        }

        /// <summary>
        /// Gets the display name for a language
        /// </summary>
        public static string GetLanguageDisplayName(string cultureName)
        {
            if (SupportedLanguages.TryGetValue(cultureName, out (string DisplayName, string Flag) info))
            {
                return info.DisplayName;
            }

            return cultureName;
        }

        /// <summary>
        /// Gets the flag emoji for a language
        /// </summary>
        public static string GetLanguageFlag(string cultureName)
        {
            if (SupportedLanguages.TryGetValue(cultureName, out (string DisplayName, string Flag) info))
            {
                return info.Flag;
            }

            return "🌐";
        }

        /// <summary>
        /// Checks if a culture is supported
        /// </summary>
        public static bool IsCultureSupported(string cultureName)
        {
            return SupportedLanguages.ContainsKey(cultureName);
        }
    }
}
