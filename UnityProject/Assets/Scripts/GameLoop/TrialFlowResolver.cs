namespace DontLetHerIn.GameLoop
{
    /// <summary>What should happen after a trial result is applied (Phase 7B.2).</summary>
    public enum TrialResolution
    {
        /// <summary>The creature reached the elevator: show SHE GOT IN.</summary>
        Lost,

        /// <summary>Alive with trials left on this floor: advance to the next trial.</summary>
        NextTrialSameFloor,

        /// <summary>Alive, last trial of a non-final floor: play the floor transition.</summary>
        FloorCleared,

        /// <summary>Alive, last trial of the final floor: show YOU ESCAPED.</summary>
        Escaped
    }

    /// <summary>
    /// Pure rule (Phase 7B.2): every trial result — correct, wrong or timeout — consumes
    /// the current trial. Death overrides everything. If the player survives and trials
    /// remain on the floor, the next trial of the SAME floor starts (no retry of the same
    /// question). The floor is cleared only after its last trial; clearing the final floor
    /// is the escape. No Unity dependency, so the rule stays fully testable in EditMode.
    /// </summary>
    public static class TrialFlowResolver
    {
        public static TrialResolution Resolve(bool isDead, bool isFinalTrialInFloor, bool isFinalFloor)
        {
            // Death overrides everything: she reached the elevator.
            if (isDead) return TrialResolution.Lost;

            // Still trials to survive on this floor: move to the next one.
            if (!isFinalTrialInFloor) return TrialResolution.NextTrialSameFloor;

            // Last trial of the floor cleared while alive.
            return isFinalFloor ? TrialResolution.Escaped : TrialResolution.FloorCleared;
        }
    }
}
