namespace DontLetHerIn.Threat
{
    /// <summary>
    /// Immutable snapshot of the threat (creature proximity + player stress).
    /// Produced by <see cref="ThreatManager"/>. Pure data, no Unity dependency.
    /// </summary>
    public readonly struct ThreatState
    {
        /// <summary>Creature distance. 100 = far, 0 = death.</summary>
        public int Distance { get; }

        /// <summary>Player stress level, clamped between 0 and 4.</summary>
        public int StressLevel { get; }

        /// <summary>True when the creature has reached the elevator (distance &lt;= 0).</summary>
        public bool IsDead { get; }

        /// <summary>Distance change produced by the most recent applied answer.</summary>
        public int LastDistanceDelta { get; }

        /// <summary>Stress change produced by the most recent applied answer.</summary>
        public int LastStressDelta { get; }

        public ThreatState(int distance, int stressLevel, bool isDead, int lastDistanceDelta, int lastStressDelta)
        {
            Distance = distance;
            StressLevel = stressLevel;
            IsDead = isDead;
            LastDistanceDelta = lastDistanceDelta;
            LastStressDelta = lastStressDelta;
        }
    }
}
