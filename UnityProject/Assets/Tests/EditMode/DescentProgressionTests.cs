using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7B.4: the run starts at Floor 5 and descends 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor.
    /// Display floor numbers and per-floor start distances are driven by DescentFloorProfile,
    /// while RunTrialProgress tracks the underlying play indices.
    /// </summary>
    public sealed class DescentProgressionTests
    {
        private static RunTrialProgress FivePlayableFloors()
        {
            return new RunTrialProgress(new List<int> { 5, 5, 5, 5, 5 });
        }

        [Test]
        public void RunStartsAtFloorFive()
        {
            RunTrialProgress p = FivePlayableFloors();
            int displayFloor = DescentFloorProfile.DisplayFloorNumber(p.CurrentFloorIndex, p.FloorCount);
            Assert.AreEqual(5, displayFloor);
            Assert.AreEqual(1, p.CurrentTrialNumber);
            Assert.AreEqual(5, p.TrialsInCurrentFloor);
        }

        [Test]
        public void ProgressionDescendsFiveToOne()
        {
            RunTrialProgress p = FivePlayableFloors();
            int[] expected = { 5, 4, 3, 2, 1 };
            for (int i = 0; i < 5; i++)
            {
                int displayFloor = DescentFloorProfile.DisplayFloorNumber(p.CurrentFloorIndex, p.FloorCount);
                Assert.AreEqual(expected[i], displayFloor, $"Play index {i} should display floor {expected[i]}.");
                p.AdvanceFloor();
            }
        }

        [Test]
        public void FloorOne_IsTheFinalFloorBeforeGround()
        {
            RunTrialProgress p = FivePlayableFloors();
            p.AdvanceFloor(); // floor 4
            p.AdvanceFloor(); // floor 3
            p.AdvanceFloor(); // floor 2
            p.AdvanceFloor(); // floor 1
            Assert.AreEqual(1, DescentFloorProfile.DisplayFloorNumber(p.CurrentFloorIndex, p.FloorCount));
            Assert.IsTrue(p.IsFinalFloor, "Floor 1 (last play index) is the final floor; clearing it reaches the ground floor.");
        }

        [Test]
        public void StartDistances_AreDeeperLowerDown()
        {
            Assert.AreEqual(85, DescentFloorProfile.StartDistance(5));
            Assert.AreEqual(80, DescentFloorProfile.StartDistance(4));
            Assert.AreEqual(75, DescentFloorProfile.StartDistance(3));
            Assert.AreEqual(70, DescentFloorProfile.StartDistance(2));
            Assert.AreEqual(65, DescentFloorProfile.StartDistance(1));
        }

        [Test]
        public void EachFloorStillHasFiveTrials()
        {
            RunTrialProgress p = FivePlayableFloors();
            for (int floor = 0; floor < 5; floor++)
            {
                Assert.AreEqual(5, p.TrialsInCurrentFloor);
                p.AdvanceFloor();
            }
        }

        [Test]
        public void DisplayFloorNumber_NeverBelowOne()
        {
            Assert.AreEqual(1, DescentFloorProfile.DisplayFloorNumber(10, 5));
        }
    }
}
