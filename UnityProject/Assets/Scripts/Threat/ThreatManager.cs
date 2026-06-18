using System;

namespace DontLetHerIn.Threat
{
    /// <summary>
    /// Owns creature distance and player stress logic.
    /// Pure C# (no Unity scene dependency) so the core gameplay rules stay testable.
    /// Prototype values come from Docs/GAME_DESIGN.md and the Phase 1 agent prompt.
    /// </summary>
    public sealed class ThreatManager
    {
        // Clamp bounds.
        public const int MinDistance = 0;
        public const int MaxDistance = 100;
        public const int MinStress = 0;
        public const int MaxStress = 4;

        // Default starting values.
        public const int DefaultInitialDistance = 70;
        public const int DefaultInitialStress = 0;

        // Answer effects (distance delta, stress delta).
        public const int CorrectFastDistance = 18;
        public const int CorrectFastStress = -1;
        public const int CorrectNormalDistance = 10;
        public const int CorrectNormalStress = 0;
        public const int CorrectSlowDistance = 3;
        public const int CorrectSlowStress = 0;
        public const int WrongAnswerDistance = -20;
        public const int WrongAnswerStress = 1;
        public const int TimeoutDistance = -30;
        public const int TimeoutStress = 2;

        private readonly int _initialDistance;
        private readonly int _initialStress;

        private int _distance;
        private int _stress;
        private int _lastDistanceDelta;
        private int _lastStressDelta;

        /// <summary>Raised whenever distance or stress changes.</summary>
        public event Action<ThreatState> StateChanged;

        public ThreatManager(int initialDistance = DefaultInitialDistance, int initialStress = DefaultInitialStress)
        {
            _initialDistance = Clamp(initialDistance, MinDistance, MaxDistance);
            _initialStress = Clamp(initialStress, MinStress, MaxStress);
            ResetInternal();
        }

        public int Distance => _distance;
        public int StressLevel => _stress;
        public int LastDistanceDelta => _lastDistanceDelta;
        public int LastStressDelta => _lastStressDelta;

        /// <summary>True when the creature has reached the elevator.</summary>
        public bool IsDead => _distance <= MinDistance;

        public ThreatState CurrentState =>
            new ThreatState(_distance, _stress, IsDead, _lastDistanceDelta, _lastStressDelta);

        /// <summary>Fast correct answer: strong push back, slight relief.</summary>
        public ThreatState ApplyCorrectFast() => Apply(CorrectFastDistance, CorrectFastStress);

        /// <summary>Normal correct answer: moderate push back.</summary>
        public ThreatState ApplyCorrectNormal() => Apply(CorrectNormalDistance, CorrectNormalStress);

        /// <summary>Slow correct answer: barely recedes.</summary>
        public ThreatState ApplyCorrectSlow() => Apply(CorrectSlowDistance, CorrectSlowStress);

        /// <summary>Wrong answer: creature advances, stress rises.</summary>
        public ThreatState ApplyWrongAnswer() => Apply(WrongAnswerDistance, WrongAnswerStress);

        /// <summary>Timeout: stronger advance than a wrong answer, more stress.</summary>
        public ThreatState ApplyTimeout() => Apply(TimeoutDistance, TimeoutStress);

        /// <summary>Restore the configured initial distance and stress.</summary>
        public ThreatState Reset()
        {
            ResetInternal();
            StateChanged?.Invoke(CurrentState);
            return CurrentState;
        }

        private ThreatState Apply(int distanceDelta, int stressDelta)
        {
            int newDistance = Clamp(_distance + distanceDelta, MinDistance, MaxDistance);
            int newStress = Clamp(_stress + stressDelta, MinStress, MaxStress);

            // Record the actual applied delta after clamping.
            _lastDistanceDelta = newDistance - _distance;
            _lastStressDelta = newStress - _stress;
            _distance = newDistance;
            _stress = newStress;

            ThreatState state = CurrentState;
            StateChanged?.Invoke(state);
            return state;
        }

        private void ResetInternal()
        {
            _distance = _initialDistance;
            _stress = _initialStress;
            _lastDistanceDelta = 0;
            _lastStressDelta = 0;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
