using System.Collections.Generic;

namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure tracker of where the player is in the multi-trial run (Phase 7B.2):
    /// which floor (0-based <see cref="CurrentFloorIndex"/>) and which trial inside it
    /// (0-based <see cref="CurrentTrialIndex"/>). Driven by the per-floor trial counts,
    /// so it works for any floor/trial layout. No Unity dependency: fully testable.
    /// It owns indexing only; the threat/win state stays in RunController/ThreatManager.
    /// </summary>
    public sealed class RunTrialProgress
    {
        private readonly IReadOnlyList<int> _trialsPerFloor;

        /// <summary>
        /// <paramref name="trialsPerFloor"/> lists the trial count of each floor in order.
        /// A null/empty list falls back to a single one-trial floor so callers never throw.
        /// </summary>
        public RunTrialProgress(IReadOnlyList<int> trialsPerFloor)
        {
            _trialsPerFloor = (trialsPerFloor != null && trialsPerFloor.Count > 0)
                ? trialsPerFloor
                : new List<int> { 1 };
            Reset();
        }

        /// <summary>Total number of floors in the run.</summary>
        public int FloorCount => _trialsPerFloor.Count;

        /// <summary>Current floor, 0-based.</summary>
        public int CurrentFloorIndex { get; private set; }

        /// <summary>Current trial inside the floor, 0-based.</summary>
        public int CurrentTrialIndex { get; private set; }

        /// <summary>Current floor as a 1-based player-facing number.</summary>
        public int CurrentFloorNumber => CurrentFloorIndex + 1;

        /// <summary>Current trial as a 1-based player-facing number.</summary>
        public int CurrentTrialNumber => CurrentTrialIndex + 1;

        /// <summary>Trial count of the current floor.</summary>
        public int TrialsInCurrentFloor => _trialsPerFloor[CurrentFloorIndex];

        /// <summary>True when the current floor is the last one.</summary>
        public bool IsFinalFloor => CurrentFloorIndex >= FloorCount - 1;

        /// <summary>True when the current trial is the last one of the current floor.</summary>
        public bool IsFinalTrialInFloor => CurrentTrialIndex >= TrialsInCurrentFloor - 1;

        /// <summary>Back to the very first trial of the first floor.</summary>
        public void Reset()
        {
            CurrentFloorIndex = 0;
            CurrentTrialIndex = 0;
        }

        /// <summary>Advance to the next trial inside the current floor (no-op past the last).</summary>
        public void AdvanceTrial()
        {
            if (!IsFinalTrialInFloor)
            {
                CurrentTrialIndex++;
            }
        }

        /// <summary>Advance to the first trial of the next floor (no-op past the last floor).</summary>
        public void AdvanceFloor()
        {
            if (!IsFinalFloor)
            {
                CurrentFloorIndex++;
                CurrentTrialIndex = 0;
            }
        }
    }
}
