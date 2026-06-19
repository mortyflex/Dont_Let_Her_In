using System;

namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure Door Seal scoring (Phase 7B.3). A correct answer builds "Door Seal" toward
    /// closing the elevator doors; wrong answers and timeouts build none. Faster answers
    /// score more, and answering while the threat is closer is worth more (clutch reward).
    /// No Unity dependency, so the formula stays fully testable in EditMode.
    /// The full formula is intentionally not surfaced to the player.
    /// </summary>
    public static class DoorSealScoring
    {
        public const float CorrectFastBase = 100f;
        public const float CorrectNormalBase = 70f;
        public const float CorrectSlowBase = 40f;

        /// <summary>Base score for an outcome before the proximity multiplier (0 for wrong/timeout).</summary>
        public static float BaseScore(AnswerOutcome outcome)
        {
            switch (outcome)
            {
                case AnswerOutcome.CorrectFast: return CorrectFastBase;
                case AnswerOutcome.CorrectNormal: return CorrectNormalBase;
                case AnswerOutcome.CorrectSlow: return CorrectSlowBase;
                default: return 0f; // Wrong, Timeout
            }
        }

        /// <summary>
        /// Threat-proximity multiplier from the distance BEFORE the answer is resolved:
        /// closer threat (smaller distance) multiplies the score more.
        /// </summary>
        public static float ProximityMultiplier(int distanceBeforeAnswer)
        {
            if (distanceBeforeAnswer >= 80) return 1.00f;
            if (distanceBeforeAnswer >= 50) return 1.15f;
            if (distanceBeforeAnswer >= 25) return 1.35f;
            return 1.60f; // distance < 25 (and the death case, which scores 0 anyway)
        }

        /// <summary>
        /// Door Seal points awarded for this trial. Wrong/timeout always award 0; correct
        /// answers award base * proximity multiplier.
        /// </summary>
        public static float Score(AnswerOutcome outcome, int distanceBeforeAnswer)
        {
            float baseScore = BaseScore(outcome);
            if (baseScore <= 0f) return 0f;
            return baseScore * ProximityMultiplier(distanceBeforeAnswer);
        }
    }

    /// <summary>
    /// Pure running Door Seal total for the current floor (Phase 7B.3). Resets at each new
    /// floor with that floor's required threshold; correct answers add points; the doors
    /// can close only once <see cref="IsSealed"/> is true. No Unity dependency.
    /// </summary>
    public sealed class DoorSealScore
    {
        /// <summary>Accumulated seal for the current floor.</summary>
        public float Current { get; private set; }

        /// <summary>Seal required to close the doors on the current floor.</summary>
        public int Required { get; private set; }

        /// <summary>Begin a new floor: clear the seal and set the required threshold.</summary>
        public void StartFloor(int required)
        {
            Required = required < 0 ? 0 : required;
            Current = 0f;
        }

        /// <summary>Add seal points. Non-positive values (wrong/timeout) are ignored.</summary>
        public void Add(float points)
        {
            if (points > 0f) Current += points;
        }

        /// <summary>True when enough seal has been built to close the doors.</summary>
        public bool IsSealed => Current >= Required;

        /// <summary>Integer seal for HUD display.</summary>
        public int CurrentRounded => (int)Math.Round(Current, MidpointRounding.AwayFromZero);
    }
}
