namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Pure data/format helper for the Phase 7I elevator cabin prototype frame (the in-cabin
    /// dressing drawn around the central corridor aperture: side panels, a button column and a
    /// floor plate). No Unity dependency, so the cabin's text/values stay testable in EditMode.
    /// The cabin is purely visual: it owns no gameplay/threat rules.
    /// </summary>
    public static class ElevatorCabin
    {
        /// <summary>Floors shown on the cabin button column, top to bottom (5 down to 1).</summary>
        public static readonly int[] ButtonFloors = { 5, 4, 3, 2, 1 };

        /// <summary>Label of the ground-floor button under the numbered floors.</summary>
        public const string GroundButtonLabel = "G";

        /// <summary>Bare floor-plate readout for a displayed floor number (language-neutral digits).</summary>
        public static string FloorPlateText(int floorDisplayNumber)
        {
            int f = floorDisplayNumber < 0 ? 0 : floorDisplayNumber;
            return f.ToString();
        }
    }
}
