using System;
using UnityEngine;

namespace DontLetHerIn.Creature
{
    /// <summary>
    /// Minimal Unity-facing creature. It is distance-driven, not AI-driven: it receives
    /// a threat distance, maps it to a <see cref="CreaturePhase"/> through
    /// <see cref="CreatureDistanceMapper"/> and optionally snaps to a matching anchor.
    ///
    /// The scene is not assembled yet in Phase 3, so every reference (data, anchors,
    /// visual root) is optional and missing references are handled without throwing.
    /// No animation, pathfinding, AI or jumpscare logic lives here.
    /// </summary>
    public sealed class CreatureController : MonoBehaviour
    {
        [SerializeField] private CreatureData creatureData;

        [Header("Optional anchors (one per phase)")]
        [SerializeField] private Transform farAnchor;
        [SerializeField] private Transform visibleAnchor;
        [SerializeField] private Transform midCorridorAnchor;
        [SerializeField] private Transform nearDoorAnchor;
        [SerializeField] private Transform atDoorAnchor;
        [SerializeField] private Transform attackAnchor;

        [Header("Optional visual root toggled with visibility")]
        [SerializeField] private GameObject visualRoot;

        // When true, the visual root is hidden while the creature is in the Far phase.
        [SerializeField] private bool hideWhenFar = true;

        /// <summary>Raised when <see cref="CurrentPhase"/> changes.</summary>
        public event Action<CreaturePhase> PhaseChanged;

        /// <summary>Most recent distance fed into <see cref="ApplyDistance"/>.</summary>
        public float CurrentDistance { get; private set; }

        /// <summary>Current distance-derived phase. Defaults to Far before any update.</summary>
        public CreaturePhase CurrentPhase { get; private set; } = CreaturePhase.Far;

        /// <summary>
        /// Feed a new threat distance. Maps it to a phase, snaps to the matching anchor
        /// if one is assigned and updates visibility. Safe to call before the scene is
        /// fully assembled: missing anchors keep the current transform position.
        /// </summary>
        public CreaturePhase ApplyDistance(float distance)
        {
            CurrentDistance = distance;
            CreaturePhase phase = CreatureDistanceMapper.GetPhase(distance, creatureData);
            SetPhase(phase);
            return phase;
        }

        private void SetPhase(CreaturePhase phase)
        {
            bool changed = phase != CurrentPhase;
            CurrentPhase = phase;

            MoveToAnchor(phase);
            UpdateVisibility(phase);

            if (changed)
            {
                PhaseChanged?.Invoke(phase);
            }
        }

        private void MoveToAnchor(CreaturePhase phase)
        {
            Transform anchor = GetAnchor(phase);
            if (anchor == null)
            {
                // Scene not assembled yet (or anchor intentionally omitted): keep position.
                return;
            }

            transform.SetPositionAndRotation(anchor.position, anchor.rotation);
        }

        private void UpdateVisibility(CreaturePhase phase)
        {
            if (visualRoot == null)
            {
                return;
            }

            bool visible = !(hideWhenFar && phase == CreaturePhase.Far);
            if (visualRoot.activeSelf != visible)
            {
                visualRoot.SetActive(visible);
            }
        }

        /// <summary>Return the assigned anchor for a phase, or null when none is set.</summary>
        public Transform GetAnchor(CreaturePhase phase)
        {
            switch (phase)
            {
                case CreaturePhase.Far: return farAnchor;
                case CreaturePhase.Visible: return visibleAnchor;
                case CreaturePhase.MidCorridor: return midCorridorAnchor;
                case CreaturePhase.NearDoor: return nearDoorAnchor;
                case CreaturePhase.AtDoor: return atDoorAnchor;
                case CreaturePhase.Attack: return attackAnchor;
                default: return null;
            }
        }
    }
}
