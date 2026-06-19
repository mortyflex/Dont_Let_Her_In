using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Code-authored sample question content for the Phase 5 playable prototype.
    /// This is a content provider, not gameplay logic: it keeps the five prototype
    /// questions out of the flow/controller scripts while avoiding hand-authored
    /// ScriptableObject assets that cannot be verified without the Unity Editor.
    /// Content mirrors Docs and the Phase 5 agent prompt (5 floors, one question each).
    ///
    /// Each call to <see cref="BuildAll"/> returns fresh <see cref="QuestionData"/>
    /// instances built through <see cref="QuestionData.Create"/>.
    /// </summary>
    public static class PrototypeQuestionSet
    {
        /// <summary>Number of prototype floors/questions.</summary>
        public const int Count = 5;

        /// <summary>Build the ordered list of prototype questions (floor 1 first).</summary>
        public static IReadOnlyList<QuestionData> BuildAll()
        {
            return new List<QuestionData>
            {
                QuestionData.Create(
                    id: "floor-1-observation",
                    type: QuestionType.Observation,
                    prompt: "Which room number blinked?",
                    answers: new[] { "101", "104", "108", "102" },
                    correctAnswerIndex: 1,
                    timeLimitSeconds: 8f,
                    difficulty: 1,
                    tags: new[] { "observation" }),

                QuestionData.Create(
                    id: "floor-2-short-memory",
                    type: QuestionType.ShortMemory,
                    prompt: "Which symbol was in the center?",
                    answers: new[] { "Eye", "Key", "Hand", "Door" },
                    correctAnswerIndex: 1,
                    timeLimitSeconds: 7f,
                    difficulty: 1,
                    tags: new[] { "memory" }),

                QuestionData.Create(
                    id: "floor-3-environmental",
                    type: QuestionType.EnvironmentalInstruction,
                    prompt: "What did the wall say?",
                    answers: new[] { "Do not run", "Do not look left", "Do not answer", "Do not lie" },
                    correctAnswerIndex: 1,
                    timeLimitSeconds: 6f,
                    difficulty: 2,
                    tags: new[] { "instruction" }),

                QuestionData.Create(
                    id: "floor-4-audio-clue",
                    type: QuestionType.AudioClue,
                    prompt: "Which sequence did the voice repeat?",
                    answers: new[] { "272", "227", "722", "277" },
                    correctAnswerIndex: 0,
                    timeLimitSeconds: 5f,
                    difficulty: 2,
                    tags: new[] { "audio", "placeholder" },
                    optionalAudioClueId: "voice-272"),

                QuestionData.Create(
                    id: "floor-5-sang-froid",
                    type: QuestionType.SangFroid,
                    prompt: "The elevator says PRESS EXIT NOW, the wall says WAIT. What do you do?",
                    answers: new[] { "Press exit", "Wait", "Open doors", "Look away" },
                    correctAnswerIndex: 1,
                    timeLimitSeconds: 4f,
                    difficulty: 3,
                    tags: new[] { "sang-froid" }),
            };
        }
    }
}
