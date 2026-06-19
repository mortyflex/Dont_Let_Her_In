using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class RunTrialProgressTests
    {
        private static RunTrialProgress TwoTrialsPerFiveFloors()
        {
            return new RunTrialProgress(new List<int> { 2, 2, 2, 2, 2 });
        }

        [Test]
        public void StartsAtFloorOne_TrialOne()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            Assert.AreEqual(0, p.CurrentFloorIndex);
            Assert.AreEqual(0, p.CurrentTrialIndex);
            Assert.AreEqual(1, p.CurrentFloorNumber);
            Assert.AreEqual(1, p.CurrentTrialNumber);
            Assert.AreEqual(5, p.FloorCount);
            Assert.AreEqual(2, p.TrialsInCurrentFloor);
        }

        [Test]
        public void IndicatorValues_AreOneBased()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            p.AdvanceTrial();
            Assert.AreEqual(1, p.CurrentFloorNumber);
            Assert.AreEqual(2, p.CurrentTrialNumber);
            p.AdvanceFloor();
            Assert.AreEqual(2, p.CurrentFloorNumber);
            Assert.AreEqual(1, p.CurrentTrialNumber);
        }

        [Test]
        public void FirstTrial_IsNotFinalTrial()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            Assert.IsFalse(p.IsFinalTrialInFloor);
        }

        [Test]
        public void AdvanceTrial_MovesToNextTrial_SameFloor()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            p.AdvanceTrial();
            Assert.AreEqual(0, p.CurrentFloorIndex, "Still on the same floor.");
            Assert.AreEqual(1, p.CurrentTrialIndex, "Moved to trial 2 (not retrying trial 1).");
            Assert.IsTrue(p.IsFinalTrialInFloor, "Trial 2 is the last trial of the floor.");
        }

        [Test]
        public void AdvanceTrial_PastLastTrial_DoesNotOverflow()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            p.AdvanceTrial(); // trial 2
            p.AdvanceTrial(); // no-op, still trial 2
            Assert.AreEqual(1, p.CurrentTrialIndex);
        }

        [Test]
        public void AdvanceFloor_ResetsTrialToOne()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            p.AdvanceTrial(); // trial 2
            p.AdvanceFloor(); // floor 2, trial 1
            Assert.AreEqual(1, p.CurrentFloorIndex);
            Assert.AreEqual(0, p.CurrentTrialIndex);
        }

        [Test]
        public void FinalFloor_IsDetected()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            Assert.IsFalse(p.IsFinalFloor);
            p.AdvanceFloor();
            p.AdvanceFloor();
            p.AdvanceFloor();
            Assert.IsFalse(p.IsFinalFloor); // floor 4
            p.AdvanceFloor();
            Assert.IsTrue(p.IsFinalFloor); // floor 5
        }

        [Test]
        public void AdvanceFloor_PastLastFloor_DoesNotOverflow()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            for (int i = 0; i < 10; i++) p.AdvanceFloor();
            Assert.AreEqual(4, p.CurrentFloorIndex);
            Assert.IsTrue(p.IsFinalFloor);
        }

        [Test]
        public void Reset_ReturnsToFirstFloorFirstTrial()
        {
            RunTrialProgress p = TwoTrialsPerFiveFloors();
            p.AdvanceTrial();
            p.AdvanceFloor();
            p.Reset();
            Assert.AreEqual(0, p.CurrentFloorIndex);
            Assert.AreEqual(0, p.CurrentTrialIndex);
        }

        [Test]
        public void NullOrEmptyCounts_FallBackToSingleFloorSingleTrial()
        {
            var p = new RunTrialProgress(null);
            Assert.AreEqual(1, p.FloorCount);
            Assert.AreEqual(1, p.TrialsInCurrentFloor);
            Assert.IsTrue(p.IsFinalFloor);
            Assert.IsTrue(p.IsFinalTrialInFloor);
        }
    }
}
