using System.Collections.Generic;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// An evidence-based trial (Phase 7E data model): a localized prompt plus answer options,
    /// grounded in a corridor clue referenced by <see cref="ClueId"/>. Pure data (no Unity
    /// dependency) so trial content stays testable in EditMode. This generalizes the current
    /// <see cref="FloorTrial"/> (question + cue) toward the observation/evidence layer.
    ///
    /// Structural rules (enforced by <see cref="EvidenceTrialValidator"/>, not in this
    /// container so it stays a permissive pure holder): exactly 4 answers, exactly 1 correct,
    /// a non-empty clue reference, a positive time limit and a valid difficulty.
    /// </summary>
    public sealed class EvidenceTrial
    {
        /// <summary>Stable English id (unique within its floor).</summary>
        public string Id { get; }

        /// <summary>Id of the <see cref="CorridorClue"/> that provides the evidence for this trial.</summary>
        public string ClueId { get; }

        /// <summary>Localized question text.</summary>
        public LocalizedText Prompt { get; }

        /// <summary>Ordered answer options (the prototype uses exactly 4).</summary>
        public IReadOnlyList<EvidenceAnswerOption> Answers { get; }

        /// <summary>Seconds allowed to answer (must be positive).</summary>
        public float TimeLimitSeconds { get; }

        /// <summary>Difficulty tier (must be >= 1).</summary>
        public int Difficulty { get; }

        public EvidenceTrial(
            string id,
            string clueId,
            LocalizedText prompt,
            IReadOnlyList<EvidenceAnswerOption> answers,
            float timeLimitSeconds,
            int difficulty = 1)
        {
            Id = id;
            ClueId = clueId;
            Prompt = prompt;
            Answers = answers ?? new List<EvidenceAnswerOption>();
            TimeLimitSeconds = timeLimitSeconds;
            Difficulty = difficulty;
        }

        /// <summary>Number of answer options.</summary>
        public int AnswerCount => Answers.Count;

        /// <summary>Number of options flagged as correct (should be exactly 1).</summary>
        public int CorrectAnswerCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Answers.Count; i++)
                {
                    if (Answers[i] != null && Answers[i].IsCorrect) count++;
                }
                return count;
            }
        }

        /// <summary>The single correct option, or null when not exactly one is flagged.</summary>
        public EvidenceAnswerOption CorrectAnswer
        {
            get
            {
                EvidenceAnswerOption found = null;
                for (int i = 0; i < Answers.Count; i++)
                {
                    if (Answers[i] == null || !Answers[i].IsCorrect) continue;
                    if (found != null) return null; // more than one correct
                    found = Answers[i];
                }
                return found;
            }
        }
    }
}
