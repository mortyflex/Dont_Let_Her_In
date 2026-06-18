namespace DontLetHerIn.Creature
{
    /// <summary>
    /// Pure logic that maps a threat distance to a <see cref="CreaturePhase"/>.
    /// No Unity scene dependency so the mapping stays testable in EditMode.
    /// The distance itself is owned by ThreatManager; this class only reads it.
    ///
    /// Canonical mapping (see Docs/GAME_DESIGN.md and the Phase 3 agent prompt):
    /// <code>
    /// distance > 80 -> Far
    /// distance > 60 -> Visible
    /// distance > 40 -> MidCorridor
    /// distance > 25 -> NearDoor
    /// distance >  0 -> AtDoor
    /// distance <= 0 -> Attack
    /// </code>
    /// </summary>
    public static class CreatureDistanceMapper
    {
        /// <summary>Map a distance to a phase using the prototype default thresholds.</summary>
        public static CreaturePhase GetPhase(float distance)
        {
            return GetPhase(
                distance,
                CreatureData.DefaultFarThreshold,
                CreatureData.DefaultVisibleThreshold,
                CreatureData.DefaultMidCorridorThreshold,
                CreatureData.DefaultNearDoorThreshold,
                CreatureData.DefaultAtDoorThreshold);
        }

        /// <summary>
        /// Map a distance to a phase using the thresholds from <paramref name="creatureData"/>.
        /// A null <paramref name="creatureData"/> safely falls back to the default thresholds,
        /// so missing references in an unassembled scene never throw.
        /// </summary>
        public static CreaturePhase GetPhase(float distance, CreatureData creatureData)
        {
            if (creatureData == null)
            {
                return GetPhase(distance);
            }

            return GetPhase(
                distance,
                creatureData.FarThreshold,
                creatureData.VisibleThreshold,
                creatureData.MidCorridorThreshold,
                creatureData.NearDoorThreshold,
                creatureData.AtDoorThreshold);
        }

        /// <summary>
        /// Core mapping against explicit thresholds. Each threshold is an exclusive
        /// lower bound: a distance strictly greater than it belongs to that phase.
        /// Invalid input (NaN) is treated as <see cref="CreaturePhase.Far"/> so bad data
        /// never triggers a false death. Over-maximum and negative values map naturally.
        /// </summary>
        public static CreaturePhase GetPhase(
            float distance,
            float farThreshold,
            float visibleThreshold,
            float midCorridorThreshold,
            float nearDoorThreshold,
            float atDoorThreshold)
        {
            if (float.IsNaN(distance))
            {
                return CreaturePhase.Far;
            }

            if (distance > farThreshold) return CreaturePhase.Far;
            if (distance > visibleThreshold) return CreaturePhase.Visible;
            if (distance > midCorridorThreshold) return CreaturePhase.MidCorridor;
            if (distance > nearDoorThreshold) return CreaturePhase.NearDoor;
            if (distance > atDoorThreshold) return CreaturePhase.AtDoor;
            return CreaturePhase.Attack;
        }
    }
}
