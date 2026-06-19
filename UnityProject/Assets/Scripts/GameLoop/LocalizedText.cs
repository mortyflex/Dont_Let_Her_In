namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// A single piece of visible text in every supported language (Phase 7B.4).
    /// Pure data, no Unity dependency, so localized content stays testable in EditMode.
    /// </summary>
    public sealed class LocalizedText
    {
        public string English { get; }
        public string French { get; }

        public LocalizedText(string english, string french)
        {
            English = english ?? string.Empty;
            French = french ?? string.Empty;
        }

        /// <summary>The string for the requested language (English is the fallback).</summary>
        public string Get(GameLanguage language)
        {
            return language == GameLanguage.French ? French : English;
        }
    }
}
