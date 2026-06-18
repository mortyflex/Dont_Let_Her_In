using UnityEngine;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Data-driven definition of one survival question.
    /// Authored as a ScriptableObject asset so content stays out of gameplay scripts
    /// (see Docs/TECH_ARCHITECTURE.md section 10 and the CLAUDE.md data rules).
    /// Runtime threat rewards/penalties are owned by ThreatManager in this phase,
    /// so they are intentionally not stored here yet.
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

        // Optional clue hooks consumed by later phases (visual/audio clue systems).
        [SerializeField] private string optionalVisualClueId;
        [SerializeField] private string optionalAudioClueId;

        public string Id => id;
        public QuestionType Type => type;
        public string Prompt => prompt;
        public string[] Answers => answers;
        public int CorrectAnswerIndex => correctAnswerIndex;
        public float TimeLimitSeconds => timeLimitSeconds;
        public int Difficulty => difficulty;
        public string[] Tags => tags;
        public string OptionalVisualClueId => optionalVisualClueId;
        public string OptionalAudioClueId => optionalAudioClueId;

        public int AnswerCount => answers?.Length ?? 0;

        /// <summary>True when <paramref name="index"/> points to a real answer slot.</summary>
        public bool IsAnswerIndexInRange(int index) => index >= 0 && index < AnswerCount;

        /// <summary>
        /// Minimal authoring validation. A question is usable only when it has a prompt,
        /// at least two answers, a correct index inside that range and a positive timer.
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
        /// generation) without authoring a ScriptableObject asset on disk.
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
            string optionalAudioClueId = null)
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
            return question;
        }
    }
}
