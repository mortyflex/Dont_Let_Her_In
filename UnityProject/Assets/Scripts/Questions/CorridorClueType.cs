namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Categories of in-world corridor clue an evidence-based trial can be built on
    /// (Phase 7E data model). See Docs/CORRIDOR_OBSERVATION_DESIGN.md. Pure enum, no Unity
    /// dependency, so evidence content stays testable in EditMode. Code identifiers are
    /// English; player-facing text lives in the clue's LocalizedText fields.
    /// </summary>
    public enum CorridorClueType
    {
        DoorNumber,
        WallMessage,
        Symbol,
        LightState,
        ObjectPlacement,
        Anomaly,
        ColorCue,
        AudioProxy,
        ShadowOrSilhouette,
        DirectionInstruction,
        ScratchedCode,
        DoorState
    }
}
