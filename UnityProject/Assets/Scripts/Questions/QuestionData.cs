using UnityEngine;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Data-driven definition of one survival question.
    /// Authored as a ScriptableObject asset so content stays out of gameplay scripts
    /// (see Docs/TECH_ARCHITECTURE.md section 10 and the CLAUDE.md data rules).
    /// Runtime threat rewards/penalties are owned by ThreatManager in this phase,
    /// so they are intentionally not stored here yet.
    ///
    /// Phase 7F adds optional French content for the prompt and answers. The
    /// player-facing getters (<see cref="Prompt"/>, <see cref="Answers"/>) resolve to the
    /// current <see cref="PrototypeLocalization.Language"/> (English is the default and the
    /// fallback). Gameplay stays index-based: <see cref="CorrectAnswerIndex"/> and answer
    /// count never change with language, so the correct answer identity is preserved EN/FR.
    /// </summary>
    [CreateAssetMenu(fileName = "QuestionData", menuName = "DontLetHerIn/Question", order = 0)]
    public sealed class QuestionData : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private QuestionType type = QuestionType.Observation;

        [TextArea]
        [SerializeField] private string prompt;

        [SerializeField] private string[] answers = new string[0];
        [SerializeField] private int correctAnswerIndex;
        [SerializeField] private float timeLimitSeconds = 8f;
        [SerializeField] private int difficulty = 1;
        [SerializeField] private string[] tags = new string[0];

        // Optional French content (Phase 7F). Empty/null means "fall back to English".
        [TextArea]
        [SerializeField] private string promptFrench;
        [SerializeField] private string[] answersFrench = new string[0];

        // Optional clue hooks consumed by later phases (visual/audio clue systems).
        [SerializeField] private string optionalVisualClueId;
        [SerializeField] private string optionalAudioClueId;

        public string Id => id;
        public QuestionType Type => type;

        /// <summary>Prompt in the current language (English fallback).</summary>
        public string Prompt => ResolveString(prompt, promptFrench);

        /// <summary>Answer labels in the current language (English fallback, same length).</summary>
        public string[] Answers => ResolveAnswers();

        public int CorrectAnswerIndex => correctAnswerIndex;
        public float TimeLimitSeconds => timeLimitSeconds;
        public int Difficulty => difficulty;
        public string[] Tags => tags;
        public string OptionalVisualClueId => optionalVisualClueId;
        public string OptionalAudioClueId => optionalAudioClueId;

        // Explicit per-language accessors (used by tests; no global state needed).
        public string PromptEnglish => prompt ?? string.Empty;
        public string PromptFrench => promptFrench ?? string.Empty;
        public string[] AnswersEnglish => answers ?? new string[0];
        public string[] AnswersFrench => answersFrench ?? new string[0];

        /// <summary>Answer count (language-independent: French must match the English length).</summary>
        public int AnswerCount => answers?.Length ?? 0;

        /// <summary>True when this question carries French text for the prompt and every answer.</summary>
        public bool HasFrench
        {
            get
            {
                if (string.IsNullOrWhiteSpace(promptFrench)) return false;
                if (answersFrench == null || answersFrench.Length != AnswerCount) return false;
                for (int i = 0; i < answersFrench.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(answersFrench[i])) return false;
                }
                return true;
            }
        }

        /// <summary>True when <paramref name="index"/> points to a real answer slot.</summary>
        public bool IsAnswerIndexInRange(int index) => index >= 0 && index < AnswerCount;

        /// <summary>
        /// Minimal authoring validation. A question is usable only when it has a prompt,
        /// at least two answers, a correct index inside that range and a positive timer.
        /// Validation uses the English (authoring) prompt so it is language-independent.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(prompt)) return false;
            if (AnswerCount < 2) return false;
            if (!IsAnswerIndexInRange(correctAnswerIndex)) return false;
            if (timeLimitSeconds <= 0f) return false;
            return true;
        }

        /// <summary>
        /// Build a configured instance in code. Used by tests (and possible runtime
        /// generation) without authoring a ScriptableObject asset on disk. French content is
        /// optional; when omitted the prompt/answers fall back to English.
        /// </summary>
        public static QuestionData Create(
            string id,
            QuestionType type,
            string prompt,
            string[] answers,
            int correctAnswerIndex,
            float timeLimitSeconds,
            int difficulty = 1,
            string[] tags = null,
            string optionalVisualClueId = null,
            string optionalAudioClueId = null,
            string promptFrench = null,
            string[] answersFrench = null)
        {
            var question = CreateInstance<QuestionData>();
            question.id = id;
            question.type = type;
            question.prompt = prompt;
            question.answers = answers ?? new string[0];
            question.correctAnswerIndex = correctAnswerIndex;
            question.timeLimitSeconds = timeLimitSeconds;
            question.difficulty = difficulty;
            question.tags = tags ?? new string[0];
            question.optionalVisualClueId = optionalVisualClueId;
            question.optionalAudioClueId = optionalAudioClueId;
            question.promptFrench = promptFrench;
            question.answersFrench = answersFrench ?? new string[0];
            return question;
        }

        private static string ResolveString(string english, string french)
        {
            if (PrototypeLocalization.Language == GameLanguage.French && !string.IsNullOrEmpty(french))
            {
                return french;
            }
            return english ?? string.Empty;
        }

        private string[] ResolveAnswers()
        {
            string[] en = answers ?? new string[0];

            // English (or no usable French): return the authored array as-is (no allocation).
            if (PrototypeLocalization.Language != GameLanguage.French) return en;
            if (answersFrench == null || answersFrench.Length != en.Length) return en;

            // French: per-element fallback so a missing entry still shows English.
            var result = new string[en.Length];
            for (int i = 0; i < en.Length; i++)
            {
                result[i] = string.IsNullOrEmpty(answersFrench[i]) ? en[i] : answersFrench[i];
            }
            return result;
        }
    }
}
