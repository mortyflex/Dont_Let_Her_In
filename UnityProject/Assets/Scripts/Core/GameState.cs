namespace DontLetHerIn.Core
{
    /// <summary>
    /// High-level game states for the prototype run flow.
    /// See Docs/TECH_ARCHITECTURE.md section 8 for the state machine.
    /// </summary>
    public enum GameState
    {
        Boot,
        MainMenu,
        RunStart,
        ElevatorIdle,
        QuestionActive,
        ResolvingAnswer,
        FloorTransition,
        CreatureAttack,
        RunWon,
        RunLost,
        Results
    }
}
