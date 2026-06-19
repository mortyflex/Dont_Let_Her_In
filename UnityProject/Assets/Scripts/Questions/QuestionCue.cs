using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Pure, data-only description of the in-world clue that justifies a question.
    /// Phase 5B introduces this so the prototype questions no longer feel random:
    /// each question is paired with a short, readable cue (a blinking room number,
    /// a remembered symbol row, a wall message, an audio proxy, conflicting orders).
    ///
    /// This is presentation content, not gameplay logic. It carries no Unity types
    /// so it stays fully testable in EditMode. The runtime view
    /// (<c>GameplayUIController</c>) is responsible for rendering it.
    /// </summary>
    public sealed class QuestionCue
    {
        /// <summary>Matches the owning <see cref="QuestionData.Id"/>.</summary>
        public string QuestionId { get; }

        /// <summary>Short header naming the clue source (e.g. "ROOM DISPLAY", "WALL").</summary>
        public string Label { get; }

        /// <summary>One or more clue lines shown to the player, in display order.</summary>
        public IReadOnlyList<string> Lines { get; }

        /// <summary>
        /// Index of the line that should be emphasized (e.g. the centered symbol),
        /// or -1 when no single line is highlighted.
        /// </summary>
        public int HighlightLineIndex { get; }

        public QuestionCue(string questionId, string label, IReadOnlyList<string> lines, int highlightLineIndex = -1)
        {
            QuestionId = questionId;
            Label = label;
            Lines = lines ?? new List<string>();
            HighlightLineIndex = highlightLineIndex;
        }

        /// <summary>True when <paramref name="index"/> points at a real clue line.</summary>
        public bool IsLineIndexInRange(int index) => index >= 0 && index < Lines.Count;

        /// <summary>True when this cue has a single emphasized line.</summary>
        public bool HasHighlight => IsLineIndexInRange(HighlightLineIndex);
    }
}
