namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Pure answer-evaluation logic (no Unity scene dependency) so the rules stay testable.
    /// Classifies answer speed, evaluates the selected answer, detects timeout and
    /// produces an <see cref="AnswerResult"/>. Speed rules come from Docs/GAME_DESIGN.md
    /// section 7 and the Phase 2 agent prompt.
    /// </summary>
    public static class QuestionEvaluator
    {
        /// <summary>Upper bound (inclusive) of the Fast window, as a fraction of the timer.</summary>
        public const float FastThreshold = 0.35f;

        /// <summary>Upper bound (inclusive) of the Normal window, as a fraction of the timer.</summary>
        public const float NormalThreshold = 0.70f;

        /// <summary>
        /// Classify how fast the player answered.
        /// Boundaries are inclusive on the upper edge: exactly 35% is Fast, exactly 70% is Normal.
        /// Reaching (or exceeding) the time limit is a Timeout.
        /// </summary>
        public static AnswerSpeed ClassifyAnswerSpeed(float responseTimeSeconds, float timeLimitSeconds, bool timedOut)
        {
            if (timedOut) return AnswerSpeed.Timeout;
            if (timeLimitSeconds <= 0f) return AnswerSpeed.Timeout;
            if (responseTimeSeconds < 0f) responseTimeSeconds = 0f;
            if (responseTimeSeconds >= timeLimitSeconds) return AnswerSpeed.Timeout;

            float ratio = responseTimeSeconds / timeLimitSeconds;
            if (ratio <= FastThreshold) return AnswerSpeed.Fast;
            if (ratio <= NormalThreshold) return AnswerSpeed.Normal;
            return AnswerSpeed.Slow;
        }

        /// <summary>
        /// Evaluate a submitted answer. An out-of-range index never throws: it is preserved
        /// in the result and treated as incorrect. Answering at or past the time limit
        /// is treated as a timeout (and therefore not correct).
        /// </summary>
        public static AnswerResult Evaluate(QuestionData question, int selectedAnswerIndex, float responseTimeSeconds)
        {
            if (question == null)
            {
                return new AnswerResult(
                    string.Empty, false, AnswerSpeed.Timeout, selectedAnswerIndex, -1, responseTimeSeconds, 0f, true);
            }

            if (responseTimeSeconds < 0f) responseTimeSeconds = 0f;

            float timeLimit = question.TimeLimitSeconds;
            bool timedOut = timeLimit <= 0f || responseTimeSeconds >= timeLimit;
            AnswerSpeed speed = ClassifyAnswerSpeed(responseTimeSeconds, timeLimit, timedOut);

            bool isCorrect =
                !timedOut &&
                question.IsAnswerIndexInRange(selectedAnswerIndex) &&
                selectedAnswerIndex == question.CorrectAnswerIndex;

            return new AnswerResult(
                question.Id,
                isCorrect,
                speed,
                selectedAnswerIndex,
                question.CorrectAnswerIndex,
                responseTimeSeconds,
                timeLimit,
                timedOut);
        }

        /// <summary>
        /// Produce the result for a question the player never answered in time.
        /// Always incorrect, always a timeout, with no selected answer.
        /// </summary>
        public static AnswerResult EvaluateTimeout(QuestionData question)
        {
            string id = question != null ? question.Id : string.Empty;
            int correctIndex = question != null ? question.CorrectAnswerIndex : -1;
            float timeLimit = question != null ? question.TimeLimitSeconds : 0f;

            return new AnswerResult(
                id,
                isCorrect: false,
                speed: AnswerSpeed.Timeout,
                selectedAnswerIndex: -1,
                correctAnswerIndex: correctIndex,
                responseTimeSeconds: timeLimit,
                timeLimitSeconds: timeLimit,
                isTimeout: true);
        }
    }
}
