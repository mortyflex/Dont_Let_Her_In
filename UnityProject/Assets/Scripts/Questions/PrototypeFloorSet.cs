using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Code-authored prototype content for the Phase 7B.2 multi-trial flow:
    /// 5 floors, 2 trials each, 10 questions/cues total.
    ///
    /// The first trial of each floor reuses the validated Phase 5/5B content
    /// (<see cref="PrototypeQuestionSet"/> + <see cref="PrototypeQuestionCueSet"/>),
    /// and a second simple trial/cue is added per floor (mirrors the Phase 7B.2 prompt).
    /// This is content, not gameplay logic, and carries only pure data so it stays
    /// fully testable in EditMode without authoring ScriptableObject assets.
    /// </summary>
    public static class PrototypeFloorSet
    {
        /// <summary>Number of floors in the prototype.</summary>
        public const int FloorCount = 5;

        /// <summary>Trials per floor in the prototype.</summary>
        public const int TrialsPerFloor = 2;

        /// <summary>Build the ordered list of floors (floor 1 first), each with its trials.</summary>
        public static IReadOnlyList<FloorDefinition> BuildAll()
        {
            IReadOnlyList<QuestionData> firstTrials = PrototypeQuestionSet.BuildAll();
            IReadOnlyDictionary<string, QuestionCue> firstCues = PrototypeQuestionCueSet.BuildById();

            return new List<FloorDefinition>
            {
                new FloorDefinition(1, "Introduction", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[0], firstCues),
                    new FloorTrial(
                        QuestionData.Create(
                            id: "floor-1-trial-2",
                            type: QuestionType.Observation,
                            prompt: "Which arrow was lit?",
                            answers: new[] { "Up", "Down", "Left", "Right" },
                            correctAnswerIndex: 0,
                            timeLimitSeconds: 8f,
                            difficulty: 1,
                            tags: new[] { "observation" }),
                        new QuestionCue("floor-1-trial-2", "ELEVATOR PANEL", new[] { "UP ARROW" }, 0)),
                }),

                new FloorDefinition(2, "Memory", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[1], firstCues),
                    new FloorTrial(
                        QuestionData.Create(
                            id: "floor-2-trial-2",
                            type: QuestionType.ShortMemory,
                            prompt: "Which word appeared twice?",
                            answers: new[] { "Wait", "Open", "Run", "Hide" },
                            correctAnswerIndex: 0,
                            timeLimitSeconds: 7f,
                            difficulty: 1,
                            tags: new[] { "memory" }),
                        new QuestionCue("floor-2-trial-2", "WALL WORDS", new[] { "WAIT", "OPEN", "WAIT" }, -1)),
                }),

                new FloorDefinition(3, "Instructions", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[2], firstCues),
                    new FloorTrial(
                        QuestionData.Create(
                            id: "floor-3-trial-2",
                            type: QuestionType.EnvironmentalInstruction,
                            prompt: "Which button should you avoid?",
                            answers: new[] { "Alarm", "Door Open", "Floor 3", "Light" },
                            correctAnswerIndex: 1,
                            timeLimitSeconds: 6f,
                            difficulty: 2,
                            tags: new[] { "instruction" }),
                        new QuestionCue("floor-3-trial-2", "PANEL WARNING", new[] { "DO NOT OPEN" }, 0)),
                }),

                new FloorDefinition(4, "Audio Proxy / Codes", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[3], firstCues),
                    new FloorTrial(
                        QuestionData.Create(
                            id: "floor-4-trial-2",
                            type: QuestionType.Observation,
                            prompt: "Which code was scratched into the wall?",
                            answers: new[] { "914", "941", "491", "149" },
                            correctAnswerIndex: 0,
                            timeLimitSeconds: 5f,
                            difficulty: 2,
                            tags: new[] { "observation", "code" }),
                        new QuestionCue("floor-4-trial-2", "SCRATCHED CODE", new[] { "914" }, 0)),
                }),

                new FloorDefinition(5, "Final Panic", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[4], firstCues),
                    new FloorTrial(
                        QuestionData.Create(
                            id: "floor-5-trial-2",
                            type: QuestionType.SangFroid,
                            prompt: "She is at the door. What should you do?",
                            answers: new[] { "Hold the door", "Answer calmly", "Open it", "Look closer" },
                            correctAnswerIndex: 1,
                            timeLimitSeconds: 4f,
                            difficulty: 3,
                            tags: new[] { "sang-froid" }),
                        new QuestionCue("floor-5-trial-2", "FINAL WARNING", new[] { "DO NOT OPEN", "ANSWER CALMLY" }, 1)),
                }),
            };
        }

        /// <summary>Trial counts per floor, in floor order. Drives <c>RunTrialProgress</c>.</summary>
        public static IReadOnlyList<int> TrialCounts()
        {
            var counts = new List<int>();
            foreach (FloorDefinition floor in BuildAll())
            {
                counts.Add(floor.TrialCount);
            }
            return counts;
        }

        private static FloorTrial FirstTrial(QuestionData question, IReadOnlyDictionary<string, QuestionCue> cues)
        {
            cues.TryGetValue(question.Id, out QuestionCue cue);
            return new FloorTrial(question, cue);
        }
    }
}
