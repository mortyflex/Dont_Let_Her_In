using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Threat;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class RunControllerTests
    {
        private static RunController NewStartedRun(int totalFloors = 5, ThreatManager threat = null)
        {
            var run = new RunController(totalFloors, threat);
            run.StartRun();
            return run;
        }

        [Test]
        public void Run_StartsAtFirstFloor()
        {
            var run = NewStartedRun();
            Assert.AreEqual(1, run.CurrentFloor);
            Assert.IsTrue(run.IsRunning);
        }

        [Test]
        public void Run_TracksCurrentFloor()
        {
            var run = NewStartedRun(totalFloors: 5);

            run.CompleteFloor();
            Assert.AreEqual(2, run.CurrentFloor);

            run.CompleteFloor();
            Assert.AreEqual(3, run.CurrentFloor);
        }

        [Test]
        public void Run_AdvancesAfterFloorCompleted()
        {
            var run = NewStartedRun(totalFloors: 5);
            int before = run.CurrentFloor;

            run.CompleteFloor();

            Assert.AreEqual(before + 1, run.CurrentFloor);
            Assert.AreEqual(1, run.FloorsCompleted);
        }

        [Test]
        public void Run_WinsAfterFinalFloor()
        {
            var run = NewStartedRun(totalFloors: 3);

            run.CompleteFloor(); // floor 1 -> 2
            run.CompleteFloor(); // floor 2 -> 3
            Assert.IsFalse(run.HasWon);

            run.CompleteFloor(); // floor 3 (final) -> win

            Assert.IsTrue(run.HasWon);
            Assert.IsFalse(run.IsRunning);
            Assert.AreEqual(3, run.FloorsCompleted);
        }

        [Test]
        public void Run_LosesWhenThreatDeathOccurs()
        {
            // Start the threat low so a single timeout reaches the elevator.
            var threat = new ThreatManager(initialDistance: 20);
            var run = NewStartedRun(totalFloors: 5, threat: threat);

            run.RecordTimeout(); // 20 - 30 -> clamp 0 -> death

            Assert.IsTrue(run.HasLost);
            Assert.IsFalse(run.IsRunning);
            Assert.IsTrue(run.Threat.IsDead);
        }

        [Test]
        public void Run_DoesNotAdvanceAfterDeath()
        {
            var threat = new ThreatManager(initialDistance: 20);
            var run = NewStartedRun(totalFloors: 5, threat: threat);

            run.RecordTimeout(); // death
            int floorAtDeath = run.CurrentFloor;

            run.CompleteFloor(); // should be ignored

            Assert.AreEqual(floorAtDeath, run.CurrentFloor);
            Assert.IsFalse(run.HasWon);
        }

        [Test]
        public void Run_RestartResetsState()
        {
            var run = NewStartedRun(totalFloors: 5);
            run.RecordWrongAnswer();
            run.RecordTimeout();
            run.RecordCorrectFast();
            run.CompleteFloor();

            run.RestartRun();

            Assert.AreEqual(1, run.CurrentFloor);
            Assert.AreEqual(0, run.FloorsCompleted);
            Assert.AreEqual(0, run.CorrectAnswers);
            Assert.AreEqual(0, run.WrongAnswers);
            Assert.AreEqual(0, run.Timeouts);
            Assert.IsTrue(run.IsRunning);
            Assert.IsFalse(run.HasWon);
            Assert.IsFalse(run.HasLost);
            Assert.AreEqual(ThreatManager.DefaultInitialDistance, run.Threat.Distance);
        }

        [Test]
        public void Run_TracksCorrectAnswers()
        {
            var run = NewStartedRun();

            run.RecordCorrectFast();
            run.RecordCorrectNormal();
            run.RecordCorrectSlow();

            Assert.AreEqual(3, run.CorrectAnswers);
        }

        [Test]
        public void Run_TracksWrongAnswers()
        {
            var run = NewStartedRun();

            run.RecordWrongAnswer();
            run.RecordWrongAnswer();

            Assert.AreEqual(2, run.WrongAnswers);
        }

        [Test]
        public void Run_TracksTimeouts()
        {
            // High starting distance so timeouts do not end the run early.
            var threat = new ThreatManager(initialDistance: 100);
            var run = NewStartedRun(totalFloors: 5, threat: threat);

            run.RecordTimeout();
            run.RecordTimeout();

            Assert.AreEqual(2, run.Timeouts);
            Assert.IsTrue(run.IsRunning);
        }

        [Test]
        public void RunResult_ReflectsCurrentRunState()
        {
            var threat = new ThreatManager(initialDistance: 100);
            var run = NewStartedRun(totalFloors: 3, threat: threat);

            run.RecordCorrectFast();   // correct +1
            run.RecordWrongAnswer();   // wrong +1
            run.RecordTimeout();       // timeout +1
            run.CompleteFloor();       // floorsCompleted 1

            RunResult result = run.BuildResult();

            Assert.AreEqual(1, result.CorrectAnswers);
            Assert.AreEqual(1, result.WrongAnswers);
            Assert.AreEqual(1, result.Timeouts);
            Assert.AreEqual(1, result.FloorsCompleted);
            Assert.AreEqual(run.Threat.Distance, result.FinalDistance);
            Assert.AreEqual(run.Threat.StressLevel, result.FinalStress);
            Assert.IsFalse(result.Won);
            Assert.IsFalse(result.Lost);
        }

        [Test]
        public void RunResult_ReflectsVictory()
        {
            var run = NewStartedRun(totalFloors: 1);

            run.CompleteFloor(); // single floor -> win

            RunResult result = run.BuildResult();
            Assert.IsTrue(result.Won);
            Assert.IsFalse(result.Lost);
            Assert.AreEqual(1, result.FloorsCompleted);
        }
    }
}
