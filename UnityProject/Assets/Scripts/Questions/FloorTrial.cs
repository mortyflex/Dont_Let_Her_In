namespace DontLetHerIn.Questions
{
    /// <summary>
    /// One trial inside a floor: a question paired with its in-world cue.
    /// Pure data container (no Unity dependency) so floor/trial content stays testable.
    /// A floor is survived by clearing all of its trials while still alive (Phase 7B.2).
    /// </summary>
    public sealed class FloorTrial
    {
        /// <summary>The question shown for this trial.</summary>
        public QuestionData Question { get; }

        /// <summary>The cue that justifies the question (may be null if none).</summary>
        public QuestionCue Cue { get; }

        public FloorTrial(QuestionData question, QuestionCue cue)
        {
            Question = question;
            Cue = cue;
        }
    }
}
