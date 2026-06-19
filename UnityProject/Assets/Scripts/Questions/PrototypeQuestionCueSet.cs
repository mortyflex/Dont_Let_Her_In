using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Code-authored clue content for the five Phase 5B prototype questions.
    /// Each cue is keyed by the question id from <see cref="PrototypeQuestionSet"/>
    /// so the flow controller can look up the matching clue when a floor starts.
    ///
    /// Content mirrors the Phase 5B agent prompt:
    /// floor 1 blinking "104", floor 2 "Eye / Key / Hand" with Key centered,
    /// floor 3 "DO NOT LOOK LEFT", floor 4 audio proxy "VOICE: 272",
    /// floor 5 the conflicting "PRESS EXIT NOW" / "WAIT" orders.
    /// This is presentation content, not gameplay logic.
    /// </summary>
    public static class PrototypeQuestionCueSet
    {
        /// <summary>Number of prototype cues (one per floor/question).</summary>
        public const int Count = 5;

        /// <summary>Build the ordered list of prototype cues (floor 1 first).</summary>
        public static IReadOnlyList<QuestionCue> BuildAll()
        {
            return new List<QuestionCue>
            {
                new QuestionCue(
                    questionId: "floor-1-observation",
                    label: "ROOM DISPLAY",
                    lines: new[] { "104" },
                    highlightLineIndex: 0),

                new QuestionCue(
                    questionId: "floor-2-short-memory",
                    label: "SYMBOLS",
                    lines: new[] { "Eye", "Key", "Hand" },
                    highlightLineIndex: 1),

                new QuestionCue(
                    questionId: "floor-3-environmental",
                    label: "WALL",
                    lines: new[] { "DO NOT LOOK LEFT" },
                    highlightLineIndex: 0),

                new QuestionCue(
                    questionId: "floor-4-audio-clue",
                    label: "VOICE",
                    lines: new[] { "VOICE: 272" },
                    highlightLineIndex: 0),

                new QuestionCue(
                    questionId: "floor-5-sang-froid",
                    label: "CONFLICT",
                    lines: new[] { "ELEVATOR: PRESS EXIT NOW", "WALL: WAIT" },
                    highlightLineIndex: -1),
            };
        }

        /// <summary>
        /// Build a lookup of cues keyed by question id. Returns an empty/duplicate-safe
        /// dictionary; the last cue wins if two share an id (none do in the prototype).
        /// </summary>
        public static IReadOnlyDictionary<string, QuestionCue> BuildById()
        {
            var map = new Dictionary<string, QuestionCue>();
            foreach (QuestionCue cue in BuildAll())
            {
                map[cue.QuestionId] = cue;
            }
            return map;
        }
    }
}
