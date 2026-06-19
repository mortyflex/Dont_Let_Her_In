using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// All the evidence content for one floor (Phase 7E data model): the observable corridor
    /// clues plus the evidence-based trials that reference them. Pure data (no Unity
    /// dependency) so floor content stays fully testable in EditMode.
    ///
    /// Structural validity (no duplicate ids, every trial references an existing clue, at
    /// least 5 trials, etc.) is checked by <see cref="EvidenceTrialValidator"/>; this
    /// container only stores the authored content.
    /// </summary>
    public sealed class FloorObservationSet
    {
        /// <summary>Displayed floor number (5..1 in the prototype descent).</summary>
        public int FloorDisplayNumber { get; }

        /// <summary>Observable clues available on this floor.</summary>
        public IReadOnlyList<CorridorClue> Clues { get; }

        /// <summary>Evidence-based trials for this floor (each references a clue id).</summary>
        public IReadOnlyList<EvidenceTrial> Trials { get; }

        public FloorObservationSet(
            int floorDisplayNumber,
            IReadOnlyList<CorridorClue> clues,
            IReadOnlyList<EvidenceTrial> trials)
        {
            FloorDisplayNumber = floorDisplayNumber;
            Clues = clues ?? new List<CorridorClue>();
            Trials = trials ?? new List<EvidenceTrial>();
        }

        /// <summary>Number of clues on this floor.</summary>
        public int ClueCount => Clues.Count;

        /// <summary>Number of trials on this floor.</summary>
        public int TrialCount => Trials.Count;

        /// <summary>Find a clue by id, or null when absent.</summary>
        public CorridorClue FindClue(string clueId)
        {
            for (int i = 0; i < Clues.Count; i++)
            {
                if (Clues[i] != null && Clues[i].Id == clueId) return Clues[i];
            }
            return null;
        }

        /// <summary>True when a clue with the given id exists on this floor.</summary>
        public bool HasClue(string clueId) => FindClue(clueId) != null;
    }
}
