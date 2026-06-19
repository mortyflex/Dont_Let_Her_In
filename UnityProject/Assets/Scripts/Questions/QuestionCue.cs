using System.Collections.Generic;
using DontLetHerIn.GameLoop;

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
    ///
    /// Phase 7F adds optional French label/lines. The player-facing <see cref="Label"/> and
    /// <see cref="Lines"/> resolve to the current <see cref="PrototypeLocalization.Language"/>
    /// (English is the default and the fallback). The line count and highlight index never
    /// change with language, so the cue layout stays identical EN/FR.
    /// </summary>
    public sealed class QuestionCue
    {
        private readonly string _label;
        private readonly string _labelFrench;
        private readonly IReadOnlyList<string> _lines;
        private readonly IReadOnlyList<string> _linesFrench;

        /// <summary>Matches the owning <see cref="QuestionData.Id"/>.</summary>
        public string QuestionId { get; }

        /// <summary>
        /// Index of the line that should be emphasized (e.g. the centered symbol),
        /// or -1 when no single line is highlighted.
        /// </summary>
        public int HighlightLineIndex { get; }

        public QuestionCue(
            string questionId,
            string label,
            IReadOnlyList<string> lines,
            int highlightLineIndex = -1,
            string labelFrench = null,
            IReadOnlyList<string> linesFrench = null)
        {
            QuestionId = questionId;
            _label = label;
            _labelFrench = labelFrench;
            _lines = lines ?? new List<string>();
            _linesFrench = linesFrench;
            HighlightLineIndex = highlightLineIndex;
        }

        /// <summary>Short header naming the clue source, in the current language (English fallback).</summary>
        public string Label => ResolveString(_label, _labelFrench);

        /// <summary>Clue lines in display order, in the current language (English fallback, same count).</summary>
        public IReadOnlyList<string> Lines => ResolveLines();

        // Explicit per-language accessors (used by tests; no global state needed).
        public string LabelEnglish => _label ?? string.Empty;
        public string LabelFrench => _labelFrench ?? string.Empty;
        public IReadOnlyList<string> LinesEnglish => _lines;
        public IReadOnlyList<string> LinesFrench => _linesFrench ?? new List<string>();

        /// <summary>True when this cue carries a French label and matching-count French lines.</summary>
        public bool HasFrench
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_labelFrench)) return false;
                if (_linesFrench == null || _linesFrench.Count != _lines.Count) return false;
                for (int i = 0; i < _linesFrench.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(_linesFrench[i])) return false;
                }
                return true;
            }
        }

        /// <summary>True when <paramref name="index"/> points at a real clue line.</summary>
        public bool IsLineIndexInRange(int index) => index >= 0 && index < Lines.Count;

        /// <summary>True when this cue has a single emphasized line.</summary>
        public bool HasHighlight => IsLineIndexInRange(HighlightLineIndex);

        private static string ResolveString(string english, string french)
        {
            if (PrototypeLocalization.Language == GameLanguage.French && !string.IsNullOrEmpty(french))
            {
                return french;
            }
            return english ?? string.Empty;
        }

        private IReadOnlyList<string> ResolveLines()
        {
            // English (or no usable French): return the authored lines as-is (no allocation).
            if (PrototypeLocalization.Language != GameLanguage.French) return _lines;
            if (_linesFrench == null || _linesFrench.Count != _lines.Count) return _lines;

            // French: per-element fallback so a missing entry still shows English.
            var result = new List<string>(_lines.Count);
            for (int i = 0; i < _lines.Count; i++)
            {
                string fr = _linesFrench[i];
                result.Add(string.IsNullOrEmpty(fr) ? _lines[i] : fr);
            }
            return result;
        }
    }
}
