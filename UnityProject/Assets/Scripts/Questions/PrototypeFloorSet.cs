using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Code-authored prototype content for the Phase 7B.3 multi-trial flow:
    /// 5 floors, 5 trials each, 25 questions/cues total.
    ///
    /// Trials 1-2 of each floor reuse the validated Phase 5/5B/7B.2 content
    /// (<see cref="PrototypeQuestionSet"/> + <see cref="PrototypeQuestionCueSet"/> for trial 1,
    /// an authored trial 2). Trials 3-5 are simple, short, prototype-quality but playable
    /// additions (clearly labelled below). This is content, not gameplay logic, and carries
    /// only pure data so it stays fully testable in EditMode without ScriptableObject assets.
    /// </summary>
    public static class PrototypeFloorSet
    {
        /// <summary>Number of floors in the prototype.</summary>
        public const int FloorCount = 5;

        /// <summary>Trials per floor in the prototype.</summary>
        public const int TrialsPerFloor = 5;

        /// <summary>Build the ordered list of floors (floor 1 first), each with its 5 trials.</summary>
        public static IReadOnlyList<FloorDefinition> BuildAll()
        {
            IReadOnlyList<QuestionData> firstTrials = PrototypeQuestionSet.BuildAll();
            IReadOnlyDictionary<string, QuestionCue> firstCues = PrototypeQuestionCueSet.BuildById();

            return new List<FloorDefinition>
            {
                new FloorDefinition(1, "Introduction", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[0], firstCues),
                    Trial("floor-1-trial-2", QuestionType.Observation, 8f,
                        "Which arrow was lit?", new[] { "Up", "Down", "Left", "Right" }, 0,
                        "ELEVATOR PANEL", new[] { "UP ARROW" }, 0),
                    // Prototype trials 3-5 (simple placeholder-but-playable).
                    Trial("floor-1-trial-3", QuestionType.Observation, 8f,
                        "Which number was shown?", new[] { "7", "3", "9", "5" }, 0,
                        "NUMBER PAD", new[] { "7" }, 0),
                    Trial("floor-1-trial-4", QuestionType.Observation, 8f,
                        "Which light stayed on?", new[] { "Red", "Green", "Blue", "White" }, 1,
                        "PANEL LIGHT", new[] { "GREEN" }, 0),
                    Trial("floor-1-trial-5", QuestionType.Anomaly, 8f,
                        "Which floor number glitched?", new[] { "2", "4", "6", "8" }, 1,
                        "FLOOR DISPLAY", new[] { "4" }, 0),
                }),

                new FloorDefinition(2, "Memory", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[1], firstCues),
                    Trial("floor-2-trial-2", QuestionType.ShortMemory, 7f,
                        "Which word appeared twice?", new[] { "Wait", "Open", "Run", "Hide" }, 0,
                        "WALL WORDS", new[] { "WAIT", "OPEN", "WAIT" }, -1),
                    Trial("floor-2-trial-3", QuestionType.ShortMemory, 7f,
                        "Which symbol moved?", new[] { "Circle", "Square", "Triangle", "Cross" }, 2,
                        "SYMBOLS", new[] { "TRIANGLE" }, 0),
                    Trial("floor-2-trial-4", QuestionType.ShortMemory, 7f,
                        "Which name was whispered?", new[] { "Anna", "Mara", "Lena", "Sara" }, 1,
                        "WHISPER", new[] { "MARA" }, 0),
                    Trial("floor-2-trial-5", QuestionType.Observation, 7f,
                        "Which door was open?", new[] { "Left", "Right", "Center", "None" }, 2,
                        "HALL", new[] { "CENTER DOOR" }, 0),
                }),

                new FloorDefinition(3, "Instructions", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[2], firstCues),
                    Trial("floor-3-trial-2", QuestionType.EnvironmentalInstruction, 6f,
                        "Which button should you avoid?", new[] { "Alarm", "Door Open", "Floor 3", "Light" }, 1,
                        "PANEL WARNING", new[] { "DO NOT OPEN" }, 0),
                    Trial("floor-3-trial-3", QuestionType.EnvironmentalInstruction, 6f,
                        "Which instruction was safe?", new[] { "Run", "Stay still", "Scream", "Knock" }, 1,
                        "NOTE", new[] { "STAY STILL" }, 0),
                    Trial("floor-3-trial-4", QuestionType.EnvironmentalInstruction, 6f,
                        "Which warning should you obey?", new[] { "Keep quiet", "Look back", "Open up", "Step out" }, 0,
                        "SIGN", new[] { "KEEP QUIET" }, 0),
                    Trial("floor-3-trial-5", QuestionType.SangFroid, 6f,
                        "What must you not do?", new[] { "Breathe", "Blink", "Look at her", "Wait" }, 2,
                        "WALL", new[] { "DO NOT LOOK AT HER" }, 0),
                }),

                new FloorDefinition(4, "Audio Proxy / Codes", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[3], firstCues),
                    Trial("floor-4-trial-2", QuestionType.Observation, 5f,
                        "Which code was scratched into the wall?", new[] { "914", "941", "491", "149" }, 0,
                        "SCRATCHED CODE", new[] { "914" }, 0),
                    Trial("floor-4-trial-3", QuestionType.Observation, 5f,
                        "Which code appeared?", new[] { "358", "385", "538", "583" }, 0,
                        "DISPLAY CODE", new[] { "358" }, 0),
                    Trial("floor-4-trial-4", QuestionType.AudioClue, 5f,
                        "Which tone repeated?", new[] { "Low", "High", "Mid", "None" }, 0,
                        "SPEAKER", new[] { "LOW TONE" }, 0),
                    Trial("floor-4-trial-5", QuestionType.Anomaly, 5f,
                        "Which digits flashed red?", new[] { "60", "06", "66", "00" }, 1,
                        "RED DIGITS", new[] { "06" }, 0),
                }),

                new FloorDefinition(5, "Final Panic", new List<FloorTrial>
                {
                    FirstTrial(firstTrials[4], firstCues),
                    Trial("floor-5-trial-2", QuestionType.SangFroid, 4f,
                        "She is at the door. What should you do?",
                        new[] { "Hold the door", "Answer calmly", "Open it", "Look closer" }, 1,
                        "FINAL WARNING", new[] { "DO NOT OPEN", "ANSWER CALMLY" }, 1),
                    Trial("floor-5-trial-3", QuestionType.SangFroid, 4f,
                        "The lights die. What do you do?",
                        new[] { "Scream", "Stay silent", "Run out", "Knock back" }, 1,
                        "DARK", new[] { "STAY SILENT" }, 0),
                    Trial("floor-5-trial-4", QuestionType.SangFroid, 4f,
                        "She whispers your name. What do you do?",
                        new[] { "Answer", "Ignore it", "Open door", "Look" }, 1,
                        "WHISPER", new[] { "DO NOT ANSWER" }, 0),
                    Trial("floor-5-trial-5", QuestionType.SangFroid, 4f,
                        "Last second. The doors must seal. What do you do?",
                        new[] { "Hold breath", "Panic", "Force doors", "Scream" }, 0,
                        "FINAL", new[] { "HOLD BREATH" }, 0),
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

        private static FloorTrial Trial(
            string id, QuestionType type, float timeLimitSeconds,
            string prompt, string[] answers, int correctAnswerIndex,
            string cueLabel, string[] cueLines, int highlightLineIndex)
        {
            QuestionData question = QuestionData.Create(
                id: id,
                type: type,
                prompt: prompt,
                answers: answers,
                correctAnswerIndex: correctAnswerIndex,
                timeLimitSeconds: timeLimitSeconds,
                difficulty: 1,
                tags: new[] { "prototype" });
            var cue = new QuestionCue(id, cueLabel, cueLines, highlightLineIndex);
            return new FloorTrial(question, cue);
        }
    }
}
