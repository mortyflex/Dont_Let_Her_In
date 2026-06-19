namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Descent tuning (Phase 7B.4). The run starts at the top floor and descends to the
    /// ground floor. The internal play order goes from the first floor index (0) to the
    /// last, but the displayed floor number counts DOWN (top floor first). Each floor
    /// resets the threat to a floor-specific starting distance: the deeper the descent,
    /// the less safety. Pure static config (no Unity dependency), clamped for safety.
    /// </summary>
    public static class DescentFloorProfile
    {
        /// <summary>
        /// Displayed (player-facing) floor number for a 0-based play index. The top floor
        /// is shown first: index 0 -> highest floor number, last index -> Floor 1.
        /// Example with 5 floors: index 0 -> 5, 1 -> 4, ... 4 -> 1.
        /// </summary>
        public static int DisplayFloorNumber(int progressIndex, int floorCount)
        {
            int n = floorCount - progressIndex;
            if (n < 1) n = 1;
            return n;
        }

        /// <summary>
        /// Starting threat distance for a displayed floor number. Floor 5 starts at 85 and
        /// each floor down starts 5 closer, so Floor 1 starts at 65 (least safety).
        /// </summary>
        public static int StartDistance(int displayFloorNumber)
        {
            int f = displayFloorNumber;
            if (f < 1) f = 1;
            if (f > 5) f = 5;
            return 60 + f * 5; // Floor 1 -> 65 ... Floor 5 -> 85
        }
    }
}
