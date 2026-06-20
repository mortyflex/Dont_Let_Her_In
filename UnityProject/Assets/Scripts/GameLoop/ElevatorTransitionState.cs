namespace DontLetHerIn.GameLoop
{
    /// <summary>Phases of a single elevator descent transition (Phase 7I).</summary>
    public enum ElevatorTransitionPhase
    {
        /// <summary>No transition running; the elevator doors are open for gameplay.</summary>
        Inactive,

        /// <summary>The doors are closing after the floor was cleared.</summary>
        DoorsClosing,

        /// <summary>Doors closed; the descent cue plays and the floor indicator updates.</summary>
        Descending,

        /// <summary>The doors are opening onto the next floor.</summary>
        DoorsOpening,

        /// <summary>The transition finished; the next floor's observation pass may start.</summary>
        Completed
    }

    /// <summary>
    /// Pure state guard for the Phase 7I elevator descent transition. It tracks the door/descent
    /// phase and gates answers/timer/clue-board/creature while the transition runs, so the playable
    /// trial flow stays paused between floors. Owns no Unity references and is fully testable.
    ///
    /// Rules while the transition is active (closing/descending/opening):
    /// - answers and the timer are NOT allowed,
    /// - the clue board is NOT visible (clues are observation-only),
    /// - the creature is hidden,
    /// - <see cref="Begin"/> refuses to re-enter an already-running transition (no duplicate).
    /// The transition only runs after a NON-final floor is cleared; the final Floor 1 escape
    /// shows the result instead and never starts a transition (enforced by the caller).
    /// </summary>
    public sealed class ElevatorTransitionState
    {
        public ElevatorTransitionPhase Phase { get; private set; } = ElevatorTransitionPhase.Inactive;

        /// <summary>True while the doors are closing, the elevator is descending, or doors opening.</summary>
        public bool IsActive =>
            Phase == ElevatorTransitionPhase.DoorsClosing
            || Phase == ElevatorTransitionPhase.Descending
            || Phase == ElevatorTransitionPhase.DoorsOpening;

        /// <summary>Doors are open (gameplay/observation) when inactive or once completed.</summary>
        public bool DoorsOpen => Phase == ElevatorTransitionPhase.Inactive || Phase == ElevatorTransitionPhase.Completed;

        /// <summary>Answers may be tapped only when no transition is running.</summary>
        public bool AnswersAllowed => !IsActive;

        /// <summary>The trial timer may count down only when no transition is running.</summary>
        public bool TimerAllowed => !IsActive;

        /// <summary>The clue board is never visible during a transition (it is observation-only).</summary>
        public bool ClueBoardVisible => false;

        /// <summary>The creature is hidden for the whole transition.</summary>
        public bool CreatureHidden => IsActive;

        /// <summary>
        /// Start the transition by closing the doors. Returns false (and changes nothing) if a
        /// transition is already running, so callers cannot start a duplicate.
        /// </summary>
        public bool Begin()
        {
            if (IsActive) return false;
            Phase = ElevatorTransitionPhase.DoorsClosing;
            return true;
        }

        /// <summary>Doors are closed: enter the descending phase (descent cue + floor update).</summary>
        public void EnterDescending() => Phase = ElevatorTransitionPhase.Descending;

        /// <summary>Begin opening the doors onto the next floor.</summary>
        public void BeginOpening() => Phase = ElevatorTransitionPhase.DoorsOpening;

        /// <summary>Mark the transition finished; the next observation pass may now start.</summary>
        public void Complete() => Phase = ElevatorTransitionPhase.Completed;

        /// <summary>Return to the inactive phase (e.g. when a transition is interrupted by restart).</summary>
        public void Reset() => Phase = ElevatorTransitionPhase.Inactive;
    }
}
