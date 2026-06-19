namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Per-floor tuning (Phase 7B.3): the threat distance each floor starts at, and the
    /// Door Seal threshold required to close its doors. Higher floors start with less
    /// safety and demand more seal. Pure static config (no Unity dependency), 1-based
    /// floor numbers, clamped for out-of-range queries so callers never throw.
    /// </summary>
    public static class FloorThreatProfile
    {
        // Floor 1..5 starting threat distances (each floor is a fresh danger cycle).
        private static readonly int[] StartDistances = { 85, 80, 75, 70, 65 };

        // Floor 1..5 Door Seal thresholds (rising tension across the run).
        private static readonly int[] DoorSealThresholds = { 180, 220, 260, 300, 340 };

        /// <summary>Number of configured floors.</summary>
        public static int FloorCount => StartDistances.Length;

        /// <summary>Starting threat distance for a 1-based floor number.</summary>
        public static int StartDistance(int floorNumber) => Lookup(StartDistances, floorNumber);

        /// <summary>Door Seal threshold required to clear a 1-based floor number.</summary>
        public static int DoorSealThreshold(int floorNumber) => Lookup(DoorSealThresholds, floorNumber);

        private static int Lookup(int[] table, int floorNumber)
        {
            int i = floorNumber - 1;
            if (i < 0) i = 0;
            if (i >= table.Length) i = table.Length - 1;
            return table[i];
        }
    }
}
