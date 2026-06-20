using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.UI;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ElevatorTransitionTimingTests
    {
        // Initial Phase 7I values, kept as a reference for the pacing adjustment.
        private const float InitialDoorCloseSeconds = 0.8f;
        private const float InitialDescentHoldSeconds = 1.4f;
        private const float InitialDoorOpenSeconds = 0.8f;

        [Test]
        public void Default_UsesAdjustedPhase7IValues()
        {
            var timing = ElevatorTransitionTiming.Default;
            Assert.AreEqual(0.8f, timing.FloorClearedHoldSeconds, 0.0001f);
            Assert.AreEqual(1.5f, timing.DoorCloseSeconds, 0.0001f);
            Assert.AreEqual(3.0f, timing.DescentHoldSeconds, 0.0001f);
            Assert.AreEqual(1.5f, timing.DoorOpenSeconds, 0.0001f);
        }

        [Test]
        public void Default_DoorClose_IsSlowerThanInitialPhase7I()
        {
            Assert.Greater(ElevatorTransitionTiming.Default.DoorCloseSeconds, InitialDoorCloseSeconds);
        }

        [Test]
        public void Default_DoorOpen_IsSlowerThanInitialPhase7I()
        {
            Assert.Greater(ElevatorTransitionTiming.Default.DoorOpenSeconds, InitialDoorOpenSeconds);
        }

        [Test]
        public void Default_Descent_IsSlowerThanInitialPhase7I()
        {
            Assert.Greater(ElevatorTransitionTiming.Default.DescentHoldSeconds, InitialDescentHoldSeconds);
        }

        [Test]
        public void DoorApertureWidthRatio_IsNotFullScreen_AndCentred()
        {
            // Doors must not cover the whole screen (side cabin stays visible).
            Assert.Less(GameplayUIController.DoorApertureWidthRatio, 1.0f);
            Assert.GreaterOrEqual(GameplayUIController.DoorApertureWidthRatio, 0.55f);
            Assert.LessOrEqual(GameplayUIController.DoorApertureWidthRatio, 0.8f);
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
            // Heavier but still bounded (~6.8s, comfortably under 8s).
            Assert.LessOrEqual(transitionTotal, 8f);
            // The transition must still be shorter than the (long) observation pass.
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
