namespace DontLetHerIn.GameLoop
{
    /// <summary>What should happen after a trial result is applied (Phase 7B.4 descent).</summary>
    public enum TrialResolution
    {
        /// <summary>The creature reached the elevator (distance &lt;= 0): show SHE GOT IN.</summary>
        Lost,

        /// <summary>Alive with trials left on this floor: advance to the next trial.</summary>
        NextTrialSameFloor,

        /// <summary>Alive, last trial of a non-final floor: doors close, the elevator descends.</summary>
        FloorCleared,

        /// <summary>Alive, last trial of the final floor: reach the ground floor (YOU ESCAPED).</summary>
        Escaped
    }

    /// <summary>
    /// Pure rule (Phase 7B.4): every trial result consumes the current trial. There is no
    /// score/Door Seal requirement — surviving all 5 trials of a floor clears it. Death
    /// overrides everything. With trials left, the next trial of the same floor starts. On
    /// the last trial of a non-final floor the elevator descends; clearing the final floor
    /// reaches the ground floor (escape). No Unity dependency: fully testable in EditMode.
    /// </summary>
    public static class TrialFlowResolver
    {
        public static TrialResolution Resolve(bool isDead, bool isFinalTrialInFloor, bool isFinalFloor)
        {
            // Death overrides everything: she reached the elevator.
            if (isDead) return TrialResolution.Lost;

            // Still trials to survive on this floor: move to the next one.
            if (!isFinalTrialInFloor) return TrialResolution.NextTrialSameFloor;

            // Last trial of the floor survived (no score needed): descend, or escape if final.
            return isFinalFloor ? TrialResolution.Escaped : TrialResolution.FloorCleared;
        }
    }
}
