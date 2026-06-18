namespace DontLetHerIn.Questions
{
    /// <summary>
    /// How quickly the player answered, classified from response time vs. time limit.
    /// Drives the threat reward later (fast pushes the creature back hard, slow barely helps).
    /// </summary>
    public enum AnswerSpeed
    {
        Fast,
        Normal,
        Slow,
        Timeout
    }
}
