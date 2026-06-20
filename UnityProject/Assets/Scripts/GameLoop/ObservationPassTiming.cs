namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure timing config for the Phase 7H observation pass (the short "observe the corridor"
    /// moment played once at the start of each floor, before its trials). Holds the three
    /// durations only — no Unity dependency, so it stays fully testable in EditMode. Negative
    /// inputs are clamped to zero so the pass never goes backwards.
    ///
    /// Design targets (Phase 7H.2 tuning — a real, readable travelling, not a subtle nudge):
    /// <code>
    /// cameraMoveSeconds      = 5.0  (slow 5s travel toward the corridor / red light)
    /// observationHoldSeconds = 0.5  (brief pause at the deep point; 0.0..0.5 max)
    /// cameraReturnSeconds    = 5.0  (slow 5s travel back to the gameplay pose)
    /// total                  ≈ 10.5s (bounded; stays under ~11s)
    /// </code>
    /// </summary>
    public sealed class ObservationPassTiming
    {
        public const float DefaultObservationHoldSeconds = 0.5f;
        public const float DefaultCameraMoveSeconds = 5.0f;
        public const float DefaultCameraReturnSeconds = 5.0f;

        /// <summary>How long the observation overlay holds while the player reads the corridor.</summary>
        public float ObservationHoldSeconds { get; }

        /// <summary>How long the subtle camera ease toward the corridor takes.</summary>
        public float CameraMoveSeconds { get; }

        /// <summary>How long the camera takes to settle back to the gameplay pose.</summary>
        public float CameraReturnSeconds { get; }

        public ObservationPassTiming(float observationHoldSeconds, float cameraMoveSeconds, float cameraReturnSeconds)
        {
            ObservationHoldSeconds = observationHoldSeconds < 0f ? 0f : observationHoldSeconds;
            CameraMoveSeconds = cameraMoveSeconds < 0f ? 0f : cameraMoveSeconds;
            CameraReturnSeconds = cameraReturnSeconds < 0f ? 0f : cameraReturnSeconds;
        }

        /// <summary>The recommended Phase 7H timing.</summary>
        public static ObservationPassTiming Default =>
            new ObservationPassTiming(DefaultObservationHoldSeconds, DefaultCameraMoveSeconds, DefaultCameraReturnSeconds);

        /// <summary>Total seconds the whole pass takes (camera out + hold + camera back).</summary>
        public float TotalSeconds => CameraMoveSeconds + ObservationHoldSeconds + CameraReturnSeconds;

        /// <summary>True only when every phase has a strictly positive duration.</summary>
        public bool AreValuesPositive =>
            ObservationHoldSeconds > 0f && CameraMoveSeconds > 0f && CameraReturnSeconds > 0f;
    }
}
