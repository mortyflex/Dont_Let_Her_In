namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Distance-driven danger level used to drive horror feedback (Phase 6).
    /// Derived only from the threat distance, mirroring the design thresholds.
    /// </summary>
    public enum ThreatProximityLevel
    {
        Calm,          // distance > 80
        Observed,      // distance <= 80
        VisibleDanger, // distance <= 60
        DangerClose,   // distance <= 40
        NearDoor,      // distance <= 25 (near-death warning)
        Panic          // distance <= 10 (panic warning)
    }

    /// <summary>
    /// Pure presentation helper: maps a threat distance to a proximity level, a short
    /// warning message and a near-death overlay intensity. No Unity dependency, so it
    /// stays fully testable in EditMode. It reads only the existing distance value and
    /// never changes gameplay state (ThreatManager remains the single source of truth).
    /// </summary>
    public static class ThreatProximityFeedback
    {
        /// <summary>Distance at or below which the near-death warning/overlay starts.</summary>
        public const int NearDeathDistance = 25;

        /// <summary>Distance at or below which the panic warning starts.</summary>
        public const int PanicDistance = 10;

        /// <summary>Maximum alpha of the near-death dark/red overlay.</summary>
        public const float MaxOverlayAlpha = 0.4f;

        /// <summary>Map a distance (100 far .. 0 death) to its proximity level.</summary>
        public static ThreatProximityLevel GetLevel(int distance)
        {
            if (distance > 80) return ThreatProximityLevel.Calm;
            if (distance > 60) return ThreatProximityLevel.Observed;
            if (distance > 40) return ThreatProximityLevel.VisibleDanger;
            if (distance > NearDeathDistance) return ThreatProximityLevel.DangerClose;
            if (distance > PanicDistance) return ThreatProximityLevel.NearDoor;
            return ThreatProximityLevel.Panic;
        }

        /// <summary>Short proximity message for the level. Calm returns an empty string.</summary>
        public static string GetMessage(int distance)
        {
            switch (GetLevel(distance))
            {
                case ThreatProximityLevel.Observed: return "SHE IS WATCHING";
                case ThreatProximityLevel.VisibleDanger: return "SHE IS IN THE HALL";
                case ThreatProximityLevel.DangerClose: return "SHE IS CLOSE";
                case ThreatProximityLevel.NearDoor: return "SHE IS AT THE DOOR";
                case ThreatProximityLevel.Panic: return "DO NOT LET HER IN";
                default: return string.Empty; // Calm
            }
        }

        /// <summary>True at or below the near-death distance while still alive.</summary>
        public static bool IsNearDeath(int distance) => distance > 0 && distance <= NearDeathDistance;

        /// <summary>True at or below the panic distance while still alive.</summary>
        public static bool IsPanic(int distance) => distance > 0 && distance <= PanicDistance;

        /// <summary>
        /// Alpha (0..<see cref="MaxOverlayAlpha"/>) of the near-death overlay: zero until
        /// the near-death distance, then ramping up to the maximum as distance reaches 0.
        /// </summary>
        public static float GetOverlayAlpha(int distance)
        {
            if (distance >= NearDeathDistance) return 0f;
            int clamped = distance < 0 ? 0 : distance;
            float t = (NearDeathDistance - clamped) / (float)NearDeathDistance;
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;
            return t * MaxOverlayAlpha;
        }
    }
}
