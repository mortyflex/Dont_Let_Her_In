using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// A single observable corridor clue (Phase 7E data model). A clue is the visible
    /// evidence that justifies an <see cref="EvidenceTrial"/>: a door number, a wall
    /// message, a symbol, a light state, an anomaly, etc. Pure data (no Unity dependency)
    /// so evidence content stays fully testable in EditMode.
    ///
    /// Central rule (see Docs/CORRIDOR_OBSERVATION_DESIGN.md): no trial without a clue, and
    /// no correct answer without observable evidence. The <see cref="EvidenceValue"/> is the
    /// ground-truth value the clue establishes (e.g. "104", "Key"); a trial's correct answer
    /// is expected to match it.
    /// </summary>
    public sealed class CorridorClue
    {
        /// <summary>Stable English id referenced by trials via <see cref="EvidenceTrial.ClueId"/>.</summary>
        public string Id { get; }

        /// <summary>Clue category (drives how it is presented in a future visual phase).</summary>
        public CorridorClueType Type { get; }

        /// <summary>Displayed floor number this clue belongs to (5..1 in the prototype descent).</summary>
        public int FloorDisplayNumber { get; }

        /// <summary>Short localized header naming the clue source (e.g. "ROOM DISPLAY", "WALL").</summary>
        public LocalizedText Label { get; }

        /// <summary>Localized description of what is visible, usable as a recall aid.</summary>
        public LocalizedText Description { get; }

        /// <summary>Id of the corridor scene anchor where the clue appears (bound in a future phase).</summary>
        public string VisualAnchor { get; }

        /// <summary>Ground-truth value the clue establishes (language-independent where possible).</summary>
        public string EvidenceValue { get; }

        /// <summary>Relative difficulty contribution for pacing/selection.</summary>
        public int DifficultyWeight { get; }

        /// <summary>True when at least one trial is expected to reference this clue.</summary>
        public bool IsRequiredForTrial { get; }

        public CorridorClue(
            string id,
            CorridorClueType type,
            int floorDisplayNumber,
            LocalizedText label,
            LocalizedText description,
            string visualAnchor,
            string evidenceValue,
            int difficultyWeight = 1,
            bool isRequiredForTrial = true)
        {
            Id = id;
            Type = type;
            FloorDisplayNumber = floorDisplayNumber;
            Label = label;
            Description = description;
            VisualAnchor = visualAnchor;
            EvidenceValue = evidenceValue;
            DifficultyWeight = difficultyWeight;
            IsRequiredForTrial = isRequiredForTrial;
        }
    }
}
