namespace OpenFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>Implements data persistence for application languages</summary>
    public static class LanguagePersistence
    {
        /// <summary>Repository of application languages</summary>
        private static List<Language> languages;

        /// <summary>Gets all languages stored in data persistence</summary>
        public static ReadOnlyCollection<Language> LanguagesAll
        {
            get
            {
                PreventNullLanguages();
                return new ReadOnlyCollection<Language>(languages);
            }
        }

        /// <summary>Load application languages</summary>
        public static void LoadLanguages()
        {
            PreventNullLanguages();
            foreach (Language language in Language.All)
            {
                languages.Add(language);
            }
        }

        /// <summary>
        /// Retrieve a language searched by identifier
        /// </summary>
        /// <param name="languageId">identifier value to search</param>
        /// <returns>Application language</returns>
        public static Language LanguageById(long languageId)
        {
            if (languages == null)
            {
                return Language.Empty;
            }

            if (languages.Any(l => l.Id == languageId))
            {
                return languages.Where(l => l.Id == languageId).First();
            }

            return Language.Empty;
        }

        /// <summary>
        /// Retrieve a language searched by ISO
        /// </summary>
        /// <param name="iso">ISO value to search</param>
        /// <returns>Application language</returns>
        public static Language LanguageByISO(string iso)
        {
            if (languages == null)
            {
                return Language.Empty;
            }

            if (languages.Any(l => l.ISO.Equals(iso, StringComparison.OrdinalIgnoreCase)))
            {
                return languages.Where(l => l.ISO.Equals(iso, StringComparison.OrdinalIgnoreCase)).First();
            }

            return Language.Empty;
        }

        /// <summary>
        /// Prevents null exception accessing data persistence
        /// </summary>
        private static void PreventNullLanguages()
        {
            if (languages == null)
            {
                languages = new List<Language>();
            }
        }
    }
}