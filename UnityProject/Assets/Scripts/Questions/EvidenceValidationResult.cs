using System.Collections.Generic;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Typed reasons an evidence floor/trial can fail validation (Phase 7E).
    /// English code identifiers; messages carry the offending id for context.
    /// </summary>
    public enum EvidenceValidationError
    {
        EmptyClueId,
        EmptyTrialId,
        DuplicateClueId,
        DuplicateTrialId,
        TrialReferencesMissingClue,
        TrialAnswerCountNotFour,
        TrialNotExactlyOneCorrectAnswer,
        TrialInvalidTimeLimit,
        TrialInvalidDifficulty,
        ClueEmptyEvidenceValue,
        FloorFewerThanFiveTrials,
        LocalizedPromptMissingEnglish,
        LocalizedAnswerMissingEnglish
    }

    /// <summary>One validation problem: a typed code plus a human-readable message.</summary>
    public sealed class EvidenceValidationIssue
    {
        public EvidenceValidationError Error { get; }
        public string Message { get; }

        public EvidenceValidationIssue(EvidenceValidationError error, string message)
        {
            Error = error;
            Message = message ?? string.Empty;
        }

        public override string ToString() => $"{Error}: {Message}";
    }

    /// <summary>
    /// Result of validating evidence content (Phase 7E). Aggregates every issue found so a
    /// caller (or test) can assert both overall validity and specific error codes. Pure data,
    /// no Unity dependency.
    /// </summary>
    public sealed class EvidenceValidationResult
    {
        private readonly List<EvidenceValidationIssue> _issues;

        public EvidenceValidationResult(List<EvidenceValidationIssue> issues = null)
        {
            _issues = issues ?? new List<EvidenceValidationIssue>();
        }

        /// <summary>All issues found (empty when valid).</summary>
        public IReadOnlyList<EvidenceValidationIssue> Issues => _issues;

        /// <summary>True when no issues were found.</summary>
        public bool IsValid => _issues.Count == 0;

        /// <summary>Number of issues found.</summary>
        public int IssueCount => _issues.Count;

        /// <summary>Add an issue (used by the validator).</summary>
        public void Add(EvidenceValidationError error, string message) =>
            _issues.Add(new EvidenceValidationIssue(error, message));

        /// <summary>True when at least one issue of the given type was recorded.</summary>
        public bool HasError(EvidenceValidationError error)
        {
            for (int i = 0; i < _issues.Count; i++)
            {
                if (_issues[i].Error == error) return true;
            }
            return false;
        }

        /// <summary>Human-readable messages for all issues.</summary>
        public IReadOnlyList<string> Messages
        {
            get
            {
                var list = new List<string>(_issues.Count);
                for (int i = 0; i < _issues.Count; i++) list.Add(_issues[i].ToString());
                return list;
            }
        }
    }
}
