namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Immutable summary of a finished (or in-progress) run.
    /// Consumed later by the result screen. No Unity dependency.
    /// </summary>
    public readonly struct RunResult
    {
        public bool Won { get; }
        public bool Lost { get; }
        public int FloorsCompleted { get; }
        public int CorrectAnswers { get; }
        public int WrongAnswers { get; }
        public int Timeouts { get; }
        public int FinalDistance { get; }
        public int FinalStress { get; }

        public RunResult(
            bool won,
            bool lost,
            int floorsCompleted,
            int correctAnswers,
            int wrongAnswers,
            int timeouts,
            int finalDistance,
            int finalStress)
        {
            Won = won;
            Lost = lost;
            FloorsCompleted = floorsCompleted;
            CorrectAnswers = correctAnswers;
            WrongAnswers = wrongAnswers;
            Timeouts = timeouts;
            FinalDistance = finalDistance;
            FinalStress = finalStress;
        }
    }
}
