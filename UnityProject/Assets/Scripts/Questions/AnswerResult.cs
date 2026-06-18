namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Immutable outcome of evaluating one answer attempt.
    /// Produced by <see cref="QuestionEvaluator"/>. Pure data, no Unity dependency.
    /// Intentionally not coupled to the threat system in this phase: ThreatManager
    /// can map a result to distance/stress later.
    /// </summary>
    public readonly struct AnswerResult
    {
        /// <summary>Id of the evaluated question (may be empty).</summary>
        public string QuestionId { get; }

        /// <summary>True only when a valid, correct answer was selected before timeout.</summary>
        public bool IsCorrect { get; }

        /// <summary>Speed classification (Fast/Normal/Slow/Timeout).</summary>
        public AnswerSpeed Speed { get; }

        /// <summary>Index the player selected. Preserved even when out of range; -1 on timeout.</summary>
        public int SelectedAnswerIndex { get; }

        /// <summary>The question's correct answer index.</summary>
        public int CorrectAnswerIndex { get; }

        /// <summary>Time the player took to answer, in seconds.</summary>
        public float ResponseTimeSeconds { get; }

        /// <summary>The question's time limit, in seconds.</summary>
        public float TimeLimitSeconds { get; }

        /// <summary>True when the player ran out of time instead of answering.</summary>
        public bool IsTimeout { get; }

        public AnswerResult(
            string questionId,
            bool isCorrect,
            AnswerSpeed speed,
            int selectedAnswerIndex,
            int correctAnswerIndex,
            float responseTimeSeconds,
            float timeLimitSeconds,
            bool isTimeout)
        {
            QuestionId = questionId;
            IsCorrect = isCorrect;
            Speed = speed;
            SelectedAnswerIndex = selectedAnswerIndex;
            CorrectAnswerIndex = correctAnswerIndex;
            ResponseTimeSeconds = responseTimeSeconds;
            TimeLimitSeconds = timeLimitSeconds;
            IsTimeout = isTimeout;
        }
    }
}
