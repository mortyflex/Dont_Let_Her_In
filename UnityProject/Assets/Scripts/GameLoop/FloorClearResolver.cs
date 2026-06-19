namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// What should happen to the run after an answer outcome has been applied to the
    /// threat/run state. Pure decision result, no Unity dependency.
    /// </summary>
    public enum FloorResolution
    {
        /// <summary>The creature reached the elevator: show SHE GOT IN.</summary>
        Lost,

        /// <summary>Wrong answer or timeout while still alive: same floor stays active.</summary>
        RetrySameFloor,

        /// <summary>Correct answer on a non-final floor: play transition, advance.</summary>
        FloorCleared,

        /// <summary>Correct answer on the final floor: show YOU ESCAPED.</summary>
        Escaped
    }

    /// <summary>
    /// Pure rule (Phase 7B.1): only a correct answer clears the current floor.
    /// Wrong answers and timeouts are danger, not progress — if they do not kill the
    /// player, the same floor remains active and must be retried. Death overrides
    /// everything. No Unity dependency so the rule stays fully testable in EditMode.
    /// </summary>
    public static class FloorClearResolver
    {
        /// <summary>True for the three correct-answer outcomes (fast/normal/slow).</summary>
        public static bool IsCorrect(AnswerOutcome outcome)
        {
            return outcome == AnswerOutcome.CorrectFast
                || outcome == AnswerOutcome.CorrectNormal
                || outcome == AnswerOutcome.CorrectSlow;
        }

        /// <summary>
        /// Decide the run resolution from the applied outcome and current state.
        /// <paramref name="isDead"/> reflects the post-outcome threat distance (&lt;= 0).
        /// <paramref name="isFinalFloor"/> is true when the active floor is the last one.
        /// </summary>
        public static FloorResolution Resolve(AnswerOutcome outcome, bool isDead, bool isFinalFloor)
        {
            // Death overrides everything: she reached the elevator.
            if (isDead) return FloorResolution.Lost;

            // Only a correct answer clears the current floor.
            if (!IsCorrect(outcome)) return FloorResolution.RetrySameFloor;

            return isFinalFloor ? FloorResolution.Escaped : FloorResolution.FloorCleared;
        }
    }
}
