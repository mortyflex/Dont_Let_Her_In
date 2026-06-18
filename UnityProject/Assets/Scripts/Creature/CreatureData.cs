using UnityEngine;

namespace DontLetHerIn.Creature
{
    /// <summary>
    /// Data-driven creature configuration authored as a ScriptableObject asset.
    /// Holds the distance thresholds used to map a threat distance to a
    /// <see cref="CreaturePhase"/> (see Docs/GAME_DESIGN.md distance interpretation
    /// and the Phase 3 agent prompt). No AI or pathfinding data lives here in v0.1.
    /// Each threshold is the exclusive lower bound of its phase:
    /// a distance strictly greater than the threshold belongs to that phase.
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureData", menuName = "DontLetHerIn/Creature", order = 0)]
    public sealed class CreatureData : ScriptableObject
    {
        // Prototype default thresholds. These mirror the canonical mapping:
        //   distance > 80 -> Far, > 60 -> Visible, > 40 -> MidCorridor,
        //   distance > 25 -> NearDoor, > 0 -> AtDoor, <= 0 -> Attack.
        public const float DefaultFarThreshold = 80f;
        public const float DefaultVisibleThreshold = 60f;
        public const float DefaultMidCorridorThreshold = 40f;
        public const float DefaultNearDoorThreshold = 25f;
        public const float DefaultAtDoorThreshold = 0f;
        public const float DefaultAttackThreshold = 0f;
        public const float DefaultBaseAdvanceSpeed = 1f;

        [SerializeField] private string id = "creature-default";
        [SerializeField] private string displayName = "The Lady";

        [Header("Distance thresholds (exclusive lower bound per phase)")]
        [SerializeField] private float farThreshold = DefaultFarThreshold;
        [SerializeField] private float visibleThreshold = DefaultVisibleThreshold;
        [SerializeField] private float midCorridorThreshold = DefaultMidCorridorThreshold;
        [SerializeField] private float nearDoorThreshold = DefaultNearDoorThreshold;
        [SerializeField] private float atDoorThreshold = DefaultAtDoorThreshold;

        // Distance at or below this value means the creature reached the elevator.
        [SerializeField] private float attackThreshold = DefaultAttackThreshold;

        [Header("Movement")]
        [SerializeField] private float baseAdvanceSpeed = DefaultBaseAdvanceSpeed;

        public string Id => id;
        public string DisplayName => displayName;
        public float FarThreshold => farThreshold;
        public float VisibleThreshold => visibleThreshold;
        public float MidCorridorThreshold => midCorridorThreshold;
        public float NearDoorThreshold => nearDoorThreshold;
        public float AtDoorThreshold => atDoorThreshold;
        public float AttackThreshold => attackThreshold;
        public float BaseAdvanceSpeed => baseAdvanceSpeed;

        /// <summary>
        /// Build a configured instance in code. Used by tests (and possible runtime
        /// generation) without authoring a ScriptableObject asset on disk.
        /// </summary>
        public static CreatureData Create(
            string id = "creature-default",
            string displayName = "The Lady",
            float farThreshold = DefaultFarThreshold,
            float visibleThreshold = DefaultVisibleThreshold,
            float midCorridorThreshold = DefaultMidCorridorThreshold,
            float nearDoorThreshold = DefaultNearDoorThreshold,
            float atDoorThreshold = DefaultAtDoorThreshold,
            float attackThreshold = DefaultAttackThreshold,
            float baseAdvanceSpeed = DefaultBaseAdvanceSpeed)
        {
            var data = CreateInstance<CreatureData>();
            data.id = id;
            data.displayName = displayName;
            data.farThreshold = farThreshold;
            data.visibleThreshold = visibleThreshold;
            data.midCorridorThreshold = midCorridorThreshold;
            data.nearDoorThreshold = nearDoorThreshold;
            data.atDoorThreshold = atDoorThreshold;
            data.attackThreshold = attackThreshold;
            data.baseAdvanceSpeed = baseAdvanceSpeed;
            return data;
        }
    }
}
