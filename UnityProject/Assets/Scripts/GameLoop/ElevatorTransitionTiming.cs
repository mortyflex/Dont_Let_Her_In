namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure timing config for the Phase 7I elevator descent transition (played once between
    /// two floors, after a non-final floor is cleared and before the next floor's observation
    /// pass). Holds the four phase durations only — no Unity dependency, so it stays fully
    /// testable in EditMode. Negative inputs are clamped to zero. The transition is intentionally
    /// shorter than the observation pass.
    ///
    /// Design targets (Phase 7I):
    /// <code>
    /// floorClearedHoldSeconds = 0.8
    /// doorCloseSeconds        = 0.8
    /// descentHoldSeconds      = 1.4
    /// doorOpenSeconds         = 0.8
    /// total                   ≈ 3.8s (readable, shorter than the observation pass)
    /// </code>
    /// </summary>
    public sealed class ElevatorTransitionTiming
    {
        public const float DefaultFloorClearedHoldSeconds = 0.8f;
        public const float DefaultDoorCloseSeconds = 0.8f;
        public const float DefaultDescentHoldSeconds = 1.4f;
        public const float DefaultDoorOpenSeconds = 0.8f;

        /// <summary>How long the FLOOR CLEARED beat holds before the doors start closing.</summary>
        public float FloorClearedHoldSeconds { get; }

        /// <summary>How long the doors take to close.</summary>
        public float DoorCloseSeconds { get; }

        /// <summary>How long the DESCENDING beat (with the descent cue) holds, doors closed.</summary>
        public float DescentHoldSeconds { get; }

        /// <summary>How long the doors take to open onto the next floor.</summary>
        public float DoorOpenSeconds { get; }

        public ElevatorTransitionTiming(
            float floorClearedHoldSeconds, float doorCloseSeconds, float descentHoldSeconds, float doorOpenSeconds)
        {
            FloorClearedHoldSeconds = Clamp(floorClearedHoldSeconds);
            DoorCloseSeconds = Clamp(doorCloseSeconds);
            DescentHoldSeconds = Clamp(descentHoldSeconds);
            DoorOpenSeconds = Clamp(doorOpenSeconds);
        }

        private static float Clamp(float value) => value < 0f ? 0f : value;

        /// <summary>The recommended Phase 7I timing (~3.8s total).</summary>
        public static ElevatorTransitionTiming Default => new ElevatorTransitionTiming(
            DefaultFloorClearedHoldSeconds, DefaultDoorCloseSeconds, DefaultDescentHoldSeconds, DefaultDoorOpenSeconds);

        /// <summary>Total seconds of the whole transition (cleared hold + close + descent + open).</summary>
        public float TotalSeconds =>
            FloorClearedHoldSeconds + DoorCloseSeconds + DescentHoldSeconds + DoorOpenSeconds;

        /// <summary>True only when every phase has a strictly positive duration.</summary>
        public bool AreValuesPositive =>
            FloorClearedHoldSeconds > 0f && DoorCloseSeconds > 0f && DescentHoldSeconds > 0f && DoorOpenSeconds > 0f;
    }
}
