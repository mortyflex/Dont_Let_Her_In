namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure pacing helper: chooses how long the outcome feedback should stay on screen
    /// before the next question starts. Phase 7 balancing intent is that dangerous
    /// outcomes (wrong / timeout) hold slightly longer so the player can register the
    /// danger state, distance change and creature jump, while correct answers keep the
    /// run snappy. No Unity dependency, so it stays fully testable in EditMode.
    ///
    /// Design targets (Phase 7 — Inter-Question Pacing):
    /// <code>
    /// correct outcomes : about 0.6s to 1.2s
    /// wrong / timeout  : about 0.9s to 1.5s (base + danger extra)
    /// </code>
    /// </summary>
    public static class InterQuestionPacing
    {
        /// <summary>True for outcomes that should hold longer so the danger registers.</summary>
        public static bool IsDangerOutcome(AnswerOutcome outcome)
        {
            return outcome == AnswerOutcome.Wrong || outcome == AnswerOutcome.Timeout;
        }

        /// <summary>
        /// Seconds the outcome feedback should hold before the next question.
        /// Dangerous outcomes add <paramref name="dangerExtraSeconds"/> on top of the base
        /// hold. Negative inputs are treated as zero so pacing never goes backwards.
        /// </summary>
        public static float GetHoldSeconds(AnswerOutcome outcome, float baseHoldSeconds, float dangerExtraSeconds)
        {
            float baseHold = baseHoldSeconds < 0f ? 0f : baseHoldSeconds;
            if (!IsDangerOutcome(outcome))
            {
                return baseHold;
            }

            float extra = dangerExtraSeconds < 0f ? 0f : dangerExtraSeconds;
            return baseHold + extra;
        }
    }
}
