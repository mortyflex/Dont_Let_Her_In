using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ElevatorTransitionTimingTests
    {
        [Test]
        public void Default_UsesRecommendedPhase7IValues()
        {
            var timing = ElevatorTransitionTiming.Default;
            Assert.AreEqual(0.8f, timing.FloorClearedHoldSeconds, 0.0001f);
            Assert.AreEqual(0.8f, timing.DoorCloseSeconds, 0.0001f);
            Assert.AreEqual(1.4f, timing.DescentHoldSeconds, 0.0001f);
            Assert.AreEqual(0.8f, timing.DoorOpenSeconds, 0.0001f);
        }

        [Test]
        public void Default_AllValuesArePositive()
        {
            var timing = ElevatorTransitionTiming.Default;
            Assert.Greater(timing.FloorClearedHoldSeconds, 0f);
            Assert.Greater(timing.DoorCloseSeconds, 0f);
            Assert.Greater(timing.DescentHoldSeconds, 0f);
            Assert.Greater(timing.DoorOpenSeconds, 0f);
            Assert.IsTrue(timing.AreValuesPositive);
        }

        [Test]
        public void TotalSeconds_IsSumOfPhases()
        {
            var timing = new ElevatorTransitionTiming(0.8f, 0.8f, 1.4f, 0.8f);
            Assert.AreEqual(3.8f, timing.TotalSeconds, 0.0001f);
        }

        [Test]
        public void Default_Total_IsBounded_AndShorterThanObservation()
        {
            float transitionTotal = ElevatorTransitionTiming.Default.TotalSeconds;
            // Readable but bounded (within ~3.5..4.5s, comfortably under 5s).
            Assert.LessOrEqual(transitionTotal, 5f);
            // The transition must be shorter than the (long) observation pass.
            Assert.Less(transitionTotal, ObservationPassTiming.Default.TotalSeconds);
        }

        [Test]
        public void NegativeInputs_AreClampedToZero_AndNotPositive()
        {
            var timing = new ElevatorTransitionTiming(-1f, -2f, -3f, -4f);
            Assert.AreEqual(0f, timing.FloorClearedHoldSeconds);
            Assert.AreEqual(0f, timing.DoorCloseSeconds);
            Assert.AreEqual(0f, timing.DescentHoldSeconds);
            Assert.AreEqual(0f, timing.DoorOpenSeconds);
            Assert.IsFalse(timing.AreValuesPositive);
        }
    }
}
