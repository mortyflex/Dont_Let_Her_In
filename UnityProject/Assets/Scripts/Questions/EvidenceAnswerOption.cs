using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// One answer option of an <see cref="EvidenceTrial"/> (Phase 7E data model).
    /// Pure data, localized text. Exactly one option per trial is expected to be correct,
    /// and the correct option's value should match the referenced clue's evidence.
    /// </summary>
    public sealed class EvidenceAnswerOption
    {
        /// <summary>Stable English id for this option (unique within its trial).</summary>
        public string Id { get; }

        /// <summary>Localized answer text (language-independent values like numbers may repeat EN/FR).</summary>
        public LocalizedText Text { get; }

        /// <summary>True when this option is the correct, evidence-backed answer.</summary>
        public bool IsCorrect { get; }

        public EvidenceAnswerOption(string id, LocalizedText text, bool isCorrect)
        {
            Id = id;
            Text = text;
            IsCorrect = isCorrect;
        }
    }
}
