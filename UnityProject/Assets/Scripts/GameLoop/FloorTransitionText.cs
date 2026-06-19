namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure player-facing text for the inter-floor transition (Phase 7B).
    /// Clearing a non-final floor is survival, not escape: the doors close, the elevator
    /// ascends and danger returns stronger one floor up. This helper owns the wording so
    /// the flow controller and UI stay free of hardcoded strings, and so the ramp framing
    /// is unit-testable without Unity.
    ///
    /// Transition sequence (non-final floor clear):
    /// <code>
    /// FLOOR CLEARED  -> DOORS CLOSING -> ASCENDING -> next floor
    /// </code>
    /// Only the final floor clear shows the real escape (handled by the run/result flow,
    /// not here).
    /// </summary>
    public static class FloorTransitionText
    {
        public const string ClearedTitle = "FLOOR CLEARED";
        public const string DoorsClosingTitle = "DOORS CLOSING";
        public const string AscendingTitle = "ASCENDING";

        /// <summary>Relief line shown with FLOOR CLEARED.</summary>
        public static string GetClearedSubtitle() => "You survived this floor.";

        /// <summary>Safety line shown with DOORS CLOSING (creature locked out for now).</summary>
        public static string GetDoorsClosingSubtitle() => "The doors close just in time.";

        /// <summary>
        /// Ramp-framing line shown with ASCENDING, escalating toward the final floor.
        /// <paramref name="nextFloor"/> is the floor the elevator is climbing to (1-based).
        /// When the next floor is the last one, it reads as the final-floor warning.
        /// </summary>
        public static string GetAscendingSubtitle(int nextFloor, int totalFloors)
        {
            if (totalFloors > 0 && nextFloor >= totalFloors)
            {
                return "Last floor. Do not let her in.";
            }

            switch (nextFloor)
            {
                case 2: return "The elevator climbs.";
                case 3: return "The lights flicker.";
                case 4: return "It is waiting above.";
                default: return "The next floor feels wrong.";
            }
        }
    }
}
