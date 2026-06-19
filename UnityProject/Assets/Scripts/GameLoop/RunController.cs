using System;
using DontLetHerIn.Threat;

namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Owns a single run: floor progression, answer stats, win/loss detection.
    /// Pure C# so the run logic is testable without UI or a scene.
    /// Delegates all distance/stress rules to <see cref="ThreatManager"/>.
    /// </summary>
    public sealed class RunController
    {
        public const int DefaultTotalFloors = 5;

        private readonly ThreatManager _threat;

        public RunController(int totalFloors = DefaultTotalFloors, ThreatManager threat = null)
        {
            TotalFloors = totalFloors < 1 ? 1 : totalFloors;
            _threat = threat ?? new ThreatManager();
        }

        /// <summary>Threat state owner for this run.</summary>
        public ThreatManager Threat => _threat;

        public int TotalFloors { get; }

        /// <summary>Current floor, 1-based. Zero before a run starts.</summary>
        public int CurrentFloor { get; private set; }

        public int FloorsCompleted { get; private set; }
        public int CorrectAnswers { get; private set; }
        public int WrongAnswers { get; private set; }
        public int Timeouts { get; private set; }

        public bool IsRunning { get; private set; }
        public bool HasWon { get; private set; }
        public bool HasLost { get; private set; }

        /// <summary>Raised when the run ends in victory.</summary>
        public event Action OnRunWon;

        /// <summary>Raised when the run ends in defeat.</summary>
        public event Action OnRunLost;

        /// <summary>Begin a fresh run from the first floor with reset stats and threat.</summary>
        public void StartRun()
        {
            CurrentFloor = 1;
            FloorsCompleted = 0;
            CorrectAnswers = 0;
            WrongAnswers = 0;
            Timeouts = 0;
            HasWon = false;
            HasLost = false;
            IsRunning = true;
            _threat.Reset();
        }

        /// <summary>Reset everything and start over. Same effect as <see cref="StartRun"/>.</summary>
        public void RestartRun() => StartRun();

        /// <summary>Record a fast correct answer and push the creature back.</summary>
        public void RecordCorrectFast()
        {
            if (!IsRunning) return;
            CorrectAnswers++;
            _threat.ApplyCorrectFast();
        }

        /// <summary>Record a normal-speed correct answer.</summary>
        public void RecordCorrectNormal()
        {
            if (!IsRunning) return;
            CorrectAnswers++;
            _threat.ApplyCorrectNormal();
        }

        /// <summary>Record a slow correct answer.</summary>
        public void RecordCorrectSlow()
        {
            if (!IsRunning) return;
            CorrectAnswers++;
            _threat.ApplyCorrectSlow();
        }

        /// <summary>
        /// Record a correct trial that builds Door Seal WITHOUT pushing the creature back
        /// (Phase 7B.3 non-receding threat). Counts toward correct-answer stats only; the
        /// Door Seal score itself is owned by the flow. The threat distance is unchanged.
        /// </summary>
        public void RecordCorrectSealed()
        {
            if (!IsRunning) return;
            CorrectAnswers++;
        }

        /// <summary>
        /// Reset the threat to a floor-specific starting distance with stress cleared
        /// (Phase 7B.3 per-floor reset). The previous floor's threat is blocked by the
        /// closed doors, so the next floor starts as a fresh danger cycle.
        /// </summary>
        public void ResetThreatForFloor(int startDistance)
        {
            _threat.ResetTo(startDistance, 0);
        }

        /// <summary>
        /// End the run as a loss while the player is still technically alive (e.g. Door Seal
        /// too low at the end of a floor — the doors would not close).
        /// </summary>
        public void FailRun() => MarkLost();

        /// <summary>Record a wrong answer; may trigger loss if the creature reaches the elevator.</summary>
        public void RecordWrongAnswer()
        {
            if (!IsRunning) return;
            WrongAnswers++;
            _threat.ApplyWrongAnswer();
            CheckThreatDeath();
        }

        /// <summary>Record a timeout; harsher than a wrong answer and may trigger loss.</summary>
        public void RecordTimeout()
        {
            if (!IsRunning) return;
            Timeouts++;
            _threat.ApplyTimeout();
            CheckThreatDeath();
        }

        /// <summary>
        /// Mark the current floor as completed. Advances to the next floor,
        /// or wins the run if the final floor was just completed.
        /// </summary>
        public void CompleteFloor()
        {
            if (!IsRunning) return;

            FloorsCompleted++;
            if (CurrentFloor >= TotalFloors)
            {
                MarkWon();
            }
            else
            {
                CurrentFloor++;
            }
        }

        /// <summary>Snapshot of the current run state.</summary>
        public RunResult BuildResult()
        {
            return new RunResult(
                HasWon,
                HasLost,
                FloorsCompleted,
                CorrectAnswers,
                WrongAnswers,
                Timeouts,
                _threat.Distance,
                _threat.StressLevel);
        }

        private void CheckThreatDeath()
        {
            if (_threat.IsDead)
            {
                MarkLost();
            }
        }

        private void MarkWon()
        {
            if (!IsRunning) return;
            IsRunning = false;
            HasWon = true;
            OnRunWon?.Invoke();
        }

        private void MarkLost()
        {
            if (!IsRunning) return;
            IsRunning = false;
            HasLost = true;
            OnRunLost?.Invoke();
        }
    }
}
