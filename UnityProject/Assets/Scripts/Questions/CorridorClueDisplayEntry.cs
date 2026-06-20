using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// One line of the static corridor clue board (Phase 7G). Pure data, no Unity
    /// dependency, so the clue display mapping stays testable in EditMode. It wraps a
    /// <see cref="CorridorClue"/>'s localized label and its evidence value into a short,
    /// readable line that ties the visible corridor detail to the playable trials.
    /// </summary>
    public sealed class CorridorClueDisplayEntry
    {
        /// <summary>Source clue id (English, stable).</summary>
        public string ClueId { get; }

        /// <summary>Clue category (door number, wall message, symbol, ...).</summary>
        public CorridorClueType Type { get; }

        /// <summary>Localized clue source label (e.g. "ROOM DISPLAY" / "NUMÉRO DE PORTE").</summary>
        public LocalizedText Label { get; }

        /// <summary>The observed value the clue establishes (e.g. "104", "Up"). Mostly language-neutral.</summary>
        public string EvidenceValue { get; }

        public CorridorClueDisplayEntry(string clueId, CorridorClueType type, LocalizedText label, string evidenceValue)
        {
            ClueId = clueId;
            Type = type;
            Label = label;
            EvidenceValue = evidenceValue ?? string.Empty;
        }

        /// <summary>Localized label in the requested language (never null).</summary>
        public string GetLabel(GameLanguage language) => Label != null ? Label.Get(language) : string.Empty;

        /// <summary>
        /// One readable board line in the requested language, e.g. "ROOM DISPLAY: 104".
        /// Falls back gracefully when the label or evidence value is missing. Never null.
        /// </summary>
        public string GetLine(GameLanguage language)
        {
            string label = GetLabel(language);
            if (string.IsNullOrEmpty(EvidenceValue)) return label;
            return string.IsNullOrEmpty(label) ? EvidenceValue : $"{label}: {EvidenceValue}";
        }
    }
}
