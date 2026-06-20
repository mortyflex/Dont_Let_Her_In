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

        // Phase 7H.1 correction: while true, the visual root is force-hidden regardless of phase
        // (used to keep the creature invisible during the observation travelling). Pure visual
        // masking only — distance, phase and threat rules are untouched.
        private bool _observationHidden;

        // Phase 7I correction: set once this object is being destroyed. While true, no SetActive
        // is issued on the visual root, so tearing down Play Mode never throws "GameObjects can
        // not be made active when they are being destroyed".
        private bool _isDestroying;

        private void OnDestroy()
        {
            _isDestroying = true;
        }

        /// <summary>Raised when <see cref="CurrentPhase"/> changes.</summary>
        public event Action<CreaturePhase> PhaseChanged;

        /// <summary>True when the optional visual root is currently shown (false if hidden or unassigned).</summary>
        public bool IsVisualVisible => visualRoot != null && visualRoot.activeSelf;

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
            // Never touch SetActive while this object is being destroyed (teardown safety).
            if (visualRoot == null || _isDestroying)
            {
                return;
            }

            // During observation the creature is force-hidden; otherwise normal phase rules apply.
            bool visible = !_observationHidden && !(hideWhenFar && phase == CreaturePhase.Far);
            if (visualRoot.activeSelf != visible)
            {
                visualRoot.SetActive(visible);
            }
        }

        /// <summary>
        /// Phase 7H.1 correction: hide or restore the creature's visual root for the observation
        /// travelling. When <paramref name="hidden"/> is true the creature is never visible until
        /// restored; when false, normal phase-based visibility resumes for the current phase.
        /// This is a pure visual mask: it does not change distance, phase, stress or threat rules.
        /// Safe when no visual root is assigned (scene not fully built).
        /// </summary>
        public void SetObservationHidden(bool hidden)
        {
            _observationHidden = hidden;
            UpdateVisibility(CurrentPhase);
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
