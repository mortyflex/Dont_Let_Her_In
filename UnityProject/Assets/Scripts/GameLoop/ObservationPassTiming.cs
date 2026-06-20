namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure timing config for the Phase 7H observation pass (the short "observe the corridor"
    /// moment played once at the start of each floor, before its trials). Holds the three
    /// durations only — no Unity dependency, so it stays fully testable in EditMode. Negative
    /// inputs are clamped to zero so the pass never goes backwards.
    ///
    /// Design targets (Phase 7H):
    /// <code>
    /// observationHoldSeconds = 2.0
    /// cameraMoveSeconds      = 0.6
    /// cameraReturnSeconds    = 0.4
    /// </code>
    /// </summary>
    public sealed class ObservationPassTiming
    {
        public const float DefaultObservationHoldSeconds = 2.0f;
        public const float DefaultCameraMoveSeconds = 0.6f;
        public const float DefaultCameraReturnSeconds = 0.4f;

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
