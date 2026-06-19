using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class FloorThreatProfileTests
    {
        [Test]
        public void StartDistances_AreConfiguredForAllFiveFloors()
        {
            Assert.AreEqual(85, FloorThreatProfile.StartDistance(1));
            Assert.AreEqual(80, FloorThreatProfile.StartDistance(2));
            Assert.AreEqual(75, FloorThreatProfile.StartDistance(3));
            Assert.AreEqual(70, FloorThreatProfile.StartDistance(4));
            Assert.AreEqual(65, FloorThreatProfile.StartDistance(5));
        }

        [Test]
        public void DoorSealThresholds_AreConfiguredForAllFiveFloors()
        {
            Assert.AreEqual(180, FloorThreatProfile.DoorSealThreshold(1));
            Assert.AreEqual(220, FloorThreatProfile.DoorSealThreshold(2));
            Assert.AreEqual(260, FloorThreatProfile.DoorSealThreshold(3));
            Assert.AreEqual(300, FloorThreatProfile.DoorSealThreshold(4));
            Assert.AreEqual(340, FloorThreatProfile.DoorSealThreshold(5));
        }

        [Test]
        public void StartDistances_DecreasePerFloor()
        {
            for (int floor = 1; floor < 5; floor++)
            {
                Assert.Greater(FloorThreatProfile.StartDistance(floor),
                    FloorThreatProfile.StartDistance(floor + 1));
            }
        }

        [Test]
        public void DoorSealThresholds_IncreasePerFloor()
        {
            for (int floor = 1; floor < 5; floor++)
            {
                Assert.Less(FloorThreatProfile.DoorSealThreshold(floor),
                    FloorThreatProfile.DoorSealThreshold(floor + 1));
            }
        }

        [Test]
        public void OutOfRangeFloors_AreClamped()
        {
            Assert.AreEqual(FloorThreatProfile.StartDistance(1), FloorThreatProfile.StartDistance(0));
            Assert.AreEqual(FloorThreatProfile.StartDistance(5), FloorThreatProfile.StartDistance(99));
            Assert.AreEqual(5, FloorThreatProfile.FloorCount);
        }
    }
}
