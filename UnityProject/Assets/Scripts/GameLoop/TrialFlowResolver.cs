namespace DontLetHerIn.GameLoop
{
    /// <summary>What should happen after a trial result is applied (Phase 7B.2 / 7B.3).</summary>
    public enum TrialResolution
    {
        /// <summary>The creature reached the elevator (distance &lt;= 0): show SHE GOT IN.</summary>
        Lost,

        /// <summary>Alive with trials left on this floor: advance to the next trial.</summary>
        NextTrialSameFloor,

        /// <summary>Alive, last trial of a non-final floor, Door Seal met: play the transition.</summary>
        FloorCleared,

        /// <summary>Alive, last trial of the final floor, Door Seal met: show YOU ESCAPED.</summary>
        Escaped,

        /// <summary>Alive, last trial done, but Door Seal too low: the doors would not close.</summary>
        SealFailed
    }

    /// <summary>
    /// Pure rule (Phase 7B.3): every trial result consumes the current trial. Death overrides
    /// everything. With trials left, the next trial of the same floor starts. On the last
    /// trial of a floor, the floor clears only if the Door Seal threshold was reached
    /// (otherwise the doors would not close and the run is lost); clearing the final floor
    /// is the escape. No Unity dependency, so the rule stays fully testable in EditMode.
    /// </summary>
    public static class TrialFlowResolver
    {
        public static TrialResolution Resolve(
            bool isDead, bool isFinalTrialInFloor, bool isFinalFloor, bool doorSealReached)
        {
            // Death overrides everything: she reached the elevator.
            if (isDead) return TrialResolution.Lost;

            // Still trials to survive on this floor: move to the next one.
            if (!isFinalTrialInFloor) return TrialResolution.NextTrialSameFloor;

            // Last trial of the floor: the doors close only if enough Door Seal was built.
            if (!doorSealReached) return TrialResolution.SealFailed;

            return isFinalFloor ? TrialResolution.Escaped : TrialResolution.FloorCleared;
        }
    }
}
