namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Gameplay-level classification of a resolved answer attempt.
    /// Maps the raw <see cref="DontLetHerIn.Questions.AnswerResult"/> to the single
    /// threat reaction that should be applied (see Phase 5 threat application rules).
    /// Pure data enum, no Unity dependency.
    /// </summary>
    public enum AnswerOutcome
    {
        CorrectFast,
        CorrectNormal,
        CorrectSlow,
        Wrong,
        Timeout
    }
}
