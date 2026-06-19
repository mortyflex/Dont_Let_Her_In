using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Threat;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7B.3: correct answers must never push the creature back during a floor, while
    /// wrong/timeout still advance it, and a new floor resets the threat distance and stress.
    /// </summary>
    public sealed class NonRecedingThreatTests
    {
        private static RunController StartedRun(int distance)
        {
            var run = new RunController(5, new ThreatManager(initialDistance: distance));
            run.StartRun();
            return run;
        }

        [Test]
        public void RecordCorrectSealed_DoesNotIncreaseThreatDistance()
        {
            var run = StartedRun(70);
            int before = run.Threat.Distance;

            run.RecordCorrectSealed();

            Assert.AreEqual(before, run.Threat.Distance, "Correct answers must not make the creature recede.");
            Assert.AreEqual(1, run.CorrectAnswers, "It still counts as a correct answer.");
        }

        [Test]
        public void WrongAnswer_StillAdvancesThreat()
        {
            var run = StartedRun(70);
            run.RecordWrongAnswer();
            Assert.AreEqual(70 + ThreatManager.WrongAnswerDistance, run.Threat.Distance);
        }

        [Test]
        public void Timeout_AdvancesThreatMoreStronglyThanWrong()
        {
            var afterWrong = StartedRun(70);
            afterWrong.RecordWrongAnswer();

            var afterTimeout = StartedRun(70);
            afterTimeout.RecordTimeout();

            Assert.Less(afterTimeout.Threat.Distance, afterWrong.Threat.Distance);
        }

        [Test]
        public void ResetThreatForFloor_ResetsDistanceAndClearsStress()
        {
            var run = StartedRun(60);
            run.RecordWrongAnswer(); // distance + stress changed
            run.RecordWrongAnswer();

            run.ResetThreatForFloor(80);

            Assert.AreEqual(80, run.Threat.Distance, "Threat resets to the floor start distance.");
            Assert.AreEqual(0, run.Threat.StressLevel, "Stress resets to 0 at the new floor.");
        }

        [Test]
        public void FailRun_MarksRunLost_WhileAlive()
        {
            var run = StartedRun(70); // alive
            Assert.IsFalse(run.Threat.IsDead);

            run.FailRun();

            Assert.IsTrue(run.HasLost);
            Assert.IsFalse(run.IsRunning);
        }

        [Test]
        public void ThreatManager_ResetTo_ClampsValues()
        {
            var threat = new ThreatManager(initialDistance: 50);
            threat.ResetTo(999, 99);
            Assert.AreEqual(ThreatManager.MaxDistance, threat.Distance);
            Assert.AreEqual(ThreatManager.MaxStress, threat.StressLevel);

            threat.ResetTo(-10, -10);
            Assert.AreEqual(ThreatManager.MinDistance, threat.Distance);
            Assert.AreEqual(ThreatManager.MinStress, threat.StressLevel);
        }
    }
}
