using NUnit.Framework;
using DontLetHerIn.Threat;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ThreatManagerTests
    {
        [Test]
        public void InitialDistance_IsSet()
        {
            var threat = new ThreatManager();
            Assert.AreEqual(ThreatManager.DefaultInitialDistance, threat.Distance);
        }

        [Test]
        public void InitialStress_IsSet()
        {
            var threat = new ThreatManager();
            Assert.AreEqual(ThreatManager.DefaultInitialStress, threat.StressLevel);
        }

        [Test]
        public void Distance_IsClampedAtZero()
        {
            var threat = new ThreatManager(initialDistance: 10);

            // -30 timeout from 10 would be -20; must clamp to 0.
            threat.ApplyTimeout();

            Assert.AreEqual(ThreatManager.MinDistance, threat.Distance);
        }

        [Test]
        public void Distance_IsClampedAtOneHundred()
        {
            var threat = new ThreatManager(initialDistance: 95);

            // +18 fast from 95 would be 113; must clamp to 100.
            threat.ApplyCorrectFast();

            Assert.AreEqual(ThreatManager.MaxDistance, threat.Distance);
        }

        [Test]
        public void CorrectFast_IncreasesDistance()
        {
            var threat = new ThreatManager(initialDistance: 50);

            threat.ApplyCorrectFast();

            Assert.AreEqual(50 + ThreatManager.CorrectFastDistance, threat.Distance);
        }

        [Test]
        public void CorrectFast_ReducesStress()
        {
            var threat = new ThreatManager(initialDistance: 50, initialStress: 2);

            threat.ApplyCorrectFast();

            Assert.AreEqual(2 + ThreatManager.CorrectFastStress, threat.StressLevel);
        }

        [Test]
        public void CorrectNormal_IncreasesDistance()
        {
            var threat = new ThreatManager(initialDistance: 50);

            threat.ApplyCorrectNormal();

            Assert.AreEqual(50 + ThreatManager.CorrectNormalDistance, threat.Distance);
        }

        [Test]
        public void CorrectSlow_IncreasesDistanceSlightly()
        {
            var threat = new ThreatManager(initialDistance: 50);

            threat.ApplyCorrectSlow();

            Assert.AreEqual(50 + ThreatManager.CorrectSlowDistance, threat.Distance);
            // Slow reward is the smallest positive distance change.
            Assert.Less(ThreatManager.CorrectSlowDistance, ThreatManager.CorrectNormalDistance);
        }

        [Test]
        public void WrongAnswer_DecreasesDistance()
        {
            var threat = new ThreatManager(initialDistance: 50);

            threat.ApplyWrongAnswer();

            Assert.AreEqual(50 + ThreatManager.WrongAnswerDistance, threat.Distance);
        }

        [Test]
        public void WrongAnswer_IncreasesStress()
        {
            var threat = new ThreatManager(initialDistance: 50, initialStress: 0);

            threat.ApplyWrongAnswer();

            Assert.AreEqual(ThreatManager.WrongAnswerStress, threat.StressLevel);
        }

        [Test]
        public void Timeout_DecreasesDistanceMoreThanWrongAnswer()
        {
            var afterWrong = new ThreatManager(initialDistance: 60);
            afterWrong.ApplyWrongAnswer();

            var afterTimeout = new ThreatManager(initialDistance: 60);
            afterTimeout.ApplyTimeout();

            Assert.Less(afterTimeout.Distance, afterWrong.Distance);
        }

        [Test]
        public void Timeout_IncreasesStressMoreThanWrongAnswer()
        {
            var afterWrong = new ThreatManager(initialDistance: 60, initialStress: 0);
            afterWrong.ApplyWrongAnswer();

            var afterTimeout = new ThreatManager(initialDistance: 60, initialStress: 0);
            afterTimeout.ApplyTimeout();

            Assert.Greater(afterTimeout.StressLevel, afterWrong.StressLevel);
        }

        [Test]
        public void Death_IsTriggeredWhenDistanceReachesZero()
        {
            var threat = new ThreatManager(initialDistance: 20);

            threat.ApplyTimeout(); // 20 - 30 -> clamp 0

            Assert.IsTrue(threat.IsDead);
            Assert.IsTrue(threat.CurrentState.IsDead);
        }

        [Test]
        public void Death_IsNotTriggeredWhenDistanceIsAboveZero()
        {
            var threat = new ThreatManager(initialDistance: 50);

            threat.ApplyWrongAnswer(); // 50 - 20 -> 30

            Assert.IsFalse(threat.IsDead);
        }

        [Test]
        public void Reset_RestoresInitialState()
        {
            var threat = new ThreatManager(initialDistance: 70, initialStress: 1);
            threat.ApplyTimeout();
            threat.ApplyWrongAnswer();

            threat.Reset();

            Assert.AreEqual(70, threat.Distance);
            Assert.AreEqual(1, threat.StressLevel);
            Assert.AreEqual(0, threat.LastDistanceDelta);
            Assert.AreEqual(0, threat.LastStressDelta);
        }

        [Test]
        public void Stress_IsClampedAtMax()
        {
            var threat = new ThreatManager(initialDistance: 100, initialStress: ThreatManager.MaxStress);

            threat.ApplyTimeout(); // +2 stress, already at max

            Assert.AreEqual(ThreatManager.MaxStress, threat.StressLevel);
        }
    }
}
