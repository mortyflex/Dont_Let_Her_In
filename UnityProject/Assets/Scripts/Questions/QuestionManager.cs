using System;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Owns the flow of a single question: start, elapsed-time tracking, answer
    /// submission and timeout resolution. Pure C# (no Unity scene dependency) so the
    /// flow stays testable; UI integration and a real timer driver come in later phases.
    /// Evaluation is delegated to <see cref="QuestionEvaluator"/>.
    /// </summary>
    public sealed class QuestionManager
    {
        /// <summary>Question currently being answered, or null when idle.</summary>
        public QuestionData ActiveQuestion { get; private set; }

        /// <summary>True between <see cref="StartQuestion"/> and a submit/timeout resolution.</summary>
        public bool IsQuestionActive { get; private set; }

        /// <summary>Seconds accumulated since the active question started.</summary>
        public float ElapsedTime { get; private set; }

        /// <summary>Last produced result, or null before any resolution / after <see cref="Reset"/>.</summary>
        public AnswerResult? LastResult { get; private set; }

        /// <summary>Time left before the active question times out (0 when idle).</summary>
        public float RemainingTime
        {
            get
            {
                if (ActiveQuestion == null) return 0f;
                float remaining = ActiveQuestion.TimeLimitSeconds - ElapsedTime;
                return remaining > 0f ? remaining : 0f;
            }
        }

        /// <summary>Raised when a new question becomes active.</summary>
        public event Action<QuestionData> QuestionStarted;

        /// <summary>Raised when the active question is resolved (answer or timeout).</summary>
        public event Action<AnswerResult> AnswerResolved;

        /// <summary>Begin a new question, resetting elapsed time and the last result.</summary>
        public void StartQuestion(QuestionData question)
        {
            ActiveQuestion = question;
            IsQuestionActive = question != null;
            ElapsedTime = 0f;
            LastResult = null;

            if (IsQuestionActive)
            {
                QuestionStarted?.Invoke(question);
            }
        }

        /// <summary>
        /// Advance the question timer by <paramref name="deltaTime"/> seconds.
        /// Auto-resolves a timeout once the limit is reached. Ignored when idle.
        /// </summary>
        public void Tick(float deltaTime)
        {
            if (!IsQuestionActive || deltaTime <= 0f) return;

            ElapsedTime += deltaTime;

            if (ElapsedTime >= ActiveQuestion.TimeLimitSeconds)
            {
                ResolveTimeout();
            }
        }

        /// <summary>
        /// Submit the player's selected answer. Returns the produced result, or null when
        /// no question is active. The question ends after a submission.
        /// </summary>
        public AnswerResult? SubmitAnswer(int selectedAnswerIndex)
        {
            if (!IsQuestionActive || ActiveQuestion == null) return null;

            AnswerResult result = QuestionEvaluator.Evaluate(ActiveQuestion, selectedAnswerIndex, ElapsedTime);
            EndWith(result);
            return result;
        }

        /// <summary>
        /// Resolve the active question as a timeout. Returns the produced result, or null
        /// when no question is active. The question ends after resolution.
        /// </summary>
        public AnswerResult? ResolveTimeout()
        {
            if (!IsQuestionActive || ActiveQuestion == null) return null;

            AnswerResult result = QuestionEvaluator.EvaluateTimeout(ActiveQuestion);
            EndWith(result);
            return result;
        }

        /// <summary>Clear all current question state and return to idle.</summary>
        public void Reset()
        {
            ActiveQuestion = null;
            IsQuestionActive = false;
            ElapsedTime = 0f;
            LastResult = null;
        }

        private void EndWith(AnswerResult result)
        {
            LastResult = result;
            IsQuestionActive = false;
            AnswerResolved?.Invoke(result);
        }
    }
}
