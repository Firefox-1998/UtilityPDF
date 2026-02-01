using System.Globalization;
using System.Threading;

namespace UtilityPDF
{
    internal static class LocalizationManager
    {
        private static CultureInfo currentCulture;

        /// <summary>
        /// Imposta la cultura dell'applicazione
        /// </summary>
        /// <param name="cultureName">Nome della cultura (es. "it-IT", "en-US")</param>
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
                // Fallback all'inglese se la cultura non è supportata
                SetCulture("en-US");
            }
        }

        /// <summary>
        /// Ottiene la cultura corrente dell'applicazione
        /// </summary>
        public static CultureInfo GetCurrentCulture()
        {
            return currentCulture ?? CultureInfo.CurrentUICulture;
        }

        /// <summary>
        /// Ottiene l'array delle lingue supportate
        /// </summary>
        public static string[] GetSupportedLanguages()
        {
            return new string[]
            {
                "en-US", // Inglese (default)
                "it-IT", // Italiano
                "fr-FR", // Francese
                "de-DE", // Tedesco
                "es-ES", // Spagnolo
                "pt-PT", // Portoghese
                "el-GR"  // Greco
            };
        }

        /// <summary>
        /// Ottiene il nome visualizzato della lingua
        /// </summary>
        public static string GetLanguageDisplayName(string cultureName)
        {
            return cultureName switch
            {
                "en-US" => "English",
                "it-IT" => "Italiano",
                "fr-FR" => "Français",
                "de-DE" => "Deutsch",
                "es-ES" => "Español",
                "pt-PT" => "Português",
                "el-GR" => "Ελληνικά",
                _ => cultureName,
            };
        }

        /// <summary>
        /// Ottiene il codice emoji della bandiera per la lingua
        /// </summary>
        public static string GetLanguageFlag(string cultureName)
        {
            return cultureName switch
            {
                "en-US" => "🇺🇸",
                "it-IT" => "🇮🇹",
                "fr-FR" => "🇫🇷",
                "de-DE" => "🇩🇪",
                "es-ES" => "🇪🇸",
                "pt-PT" => "🇵🇹",
                "el-GR" => "🇬🇷",
                _ => "🌐",
            };
        }

        /// <summary>
        /// Ottiene il codice della lingua dalla culture corrente
        /// </summary>
        public static string GetCurrentLanguageCode()
        {
            return GetCurrentCulture().Name;
        }
    }
}