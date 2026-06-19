using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// One floor of the run: an ordered list of trials the player must survive.
    /// Pure data container (no Unity dependency). The floor is cleared only after its
    /// last trial is completed while the player is still alive (Phase 7B.2).
    /// </summary>
    public sealed class FloorDefinition
    {
        /// <summary>1-based floor number (Floor 1 is the first).</summary>
        public int FloorIndex { get; }

        /// <summary>Short human label (e.g. "Introduction"). Not required by the HUD.</summary>
        public string Label { get; }

        /// <summary>Ordered trials for this floor (trial 1 first).</summary>
        public IReadOnlyList<FloorTrial> Trials { get; }

        public FloorDefinition(int floorIndex, string label, IReadOnlyList<FloorTrial> trials)
        {
            FloorIndex = floorIndex;
            Label = label;
            Trials = trials ?? new List<FloorTrial>();
        }

        /// <summary>Number of trials on this floor.</summary>
        public int TrialCount => Trials.Count;
    }
}
