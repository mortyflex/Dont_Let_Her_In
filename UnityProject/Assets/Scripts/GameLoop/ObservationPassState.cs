namespace DontLetHerIn.GameLoop
{
    /// <summary>Lifecycle of a single floor's observation pass (Phase 7H).</summary>
    public enum ObservationPhase
    {
        /// <summary>No observation pass is running and none has completed yet for this floor.</summary>
        Inactive,

        /// <summary>The observation overlay is showing; trials/timer/answers are gated off.</summary>
        Observing,

        /// <summary>The observation pass finished; normal trial flow may resume.</summary>
        Completed
    }

    /// <summary>
    /// Pure state guard for the Phase 7H observation pass. It tracks whether the pass is
    /// currently running and gates answers/timer while observing, so the playable trial flow
    /// stays paused during observation. It owns no Unity references and is fully testable.
    ///
    /// Rules:
    /// - while <see cref="Observing"/>, answers and the timer are NOT allowed,
    /// - <see cref="Begin"/> refuses to re-enter an already-running pass (no duplicate),
    /// - after <see cref="Complete"/> a new run/restart can <see cref="Begin"/> again.
    /// </summary>
    public sealed class ObservationPassState
    {
        public ObservationPhase Phase { get; private set; } = ObservationPhase.Inactive;

        /// <summary>True only while the observation overlay is on screen.</summary>
        public bool IsObserving => Phase == ObservationPhase.Observing;

        /// <summary>Answers may be tapped only when not observing.</summary>
        public bool AnswersAllowed => Phase != ObservationPhase.Observing;

        /// <summary>The trial timer may count down only when not observing.</summary>
        public bool TimerAllowed => Phase != ObservationPhase.Observing;

        /// <summary>
        /// Enter the observing phase. Returns false (and changes nothing) if a pass is already
        /// running, so callers cannot start a duplicate observation.
        /// </summary>
        public bool Begin()
        {
            if (Phase == ObservationPhase.Observing) return false;
            Phase = ObservationPhase.Observing;
            return true;
        }

        /// <summary>Mark the pass finished. Answers and timer become allowed again.</summary>
        public void Complete()
        {
            Phase = ObservationPhase.Completed;
        }

        /// <summary>Return to the inactive phase (e.g. when a pass is interrupted by restart).</summary>
        public void Reset()
        {
            Phase = ObservationPhase.Inactive;
        }
    }
}
