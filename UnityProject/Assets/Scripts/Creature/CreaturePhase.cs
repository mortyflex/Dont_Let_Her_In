namespace DontLetHerIn.Creature
{
    /// <summary>
    /// Distance-driven visual state of the creature in the corridor.
    /// The creature has no AI in v0.1: its phase is derived only from the threat
    /// distance (see Docs/GAME_DESIGN.md and the Phase 3 agent prompt).
    /// </summary>
    public enum CreaturePhase
    {
        Far,
        Visible,
        MidCorridor,
        NearDoor,
        AtDoor,
        Attack
    }
}
