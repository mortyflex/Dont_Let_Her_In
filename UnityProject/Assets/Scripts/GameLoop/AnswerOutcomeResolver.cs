using DontLetHerIn.Questions;

namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure mapping from an <see cref="AnswerResult"/> to the single
    /// <see cref="AnswerOutcome"/> that the run/threat systems should apply.
    /// No Unity scene dependency so the mapping stays testable in EditMode.
    ///
    /// Rules (Phase 5 agent prompt — Threat Application Rules):
    /// <code>
    /// timeout            -> Timeout
    /// correct + Fast     -> CorrectFast
    /// correct + Normal   -> CorrectNormal
    /// correct + Slow     -> CorrectSlow
    /// wrong (not timeout) -> Wrong
    /// </code>
    /// </summary>
    public static class AnswerOutcomeResolver
    {
        public static AnswerOutcome Resolve(AnswerResult result)
        {
            // A timeout is always the harshest outcome and is never correct.
            if (result.IsTimeout || result.Speed == AnswerSpeed.Timeout)
            {
                return AnswerOutcome.Timeout;
            }

            if (!result.IsCorrect)
            {
                return AnswerOutcome.Wrong;
            }

            switch (result.Speed)
            {
                case AnswerSpeed.Fast: return AnswerOutcome.CorrectFast;
                case AnswerSpeed.Slow: return AnswerOutcome.CorrectSlow;
                // Normal speed (and any unexpected value on a correct answer) maps to normal.
                default: return AnswerOutcome.CorrectNormal;
            }
        }
    }
}
