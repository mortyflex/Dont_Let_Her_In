using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ObservationPassTimingTests
    {
        // Phase 7H baseline timing, kept here as a reference point for the 7H.1 tuning.
        private const float Phase7HMoveSeconds = 0.6f;
        private const float Phase7HHoldSeconds = 2.0f;
        private const float Phase7HReturnSeconds = 0.4f;
        private const float Phase7HTotalSeconds =
            Phase7HMoveSeconds + Phase7HHoldSeconds + Phase7HReturnSeconds; // 3.0s

        [Test]
        public void Default_UsesRecommendedPhase7H1Values()
        {
            var timing = ObservationPassTiming.Default;
            Assert.AreEqual(2.5f, timing.ObservationHoldSeconds, 0.0001f);
            Assert.AreEqual(1.2f, timing.CameraMoveSeconds, 0.0001f);
            Assert.AreEqual(0.7f, timing.CameraReturnSeconds, 0.0001f);
        }

        [Test]
        public void Default_AllTimingValuesArePositive()
        {
            var timing = ObservationPassTiming.Default;
            Assert.Greater(timing.ObservationHoldSeconds, 0f);
            Assert.Greater(timing.CameraMoveSeconds, 0f);
            Assert.Greater(timing.CameraReturnSeconds, 0f);
            Assert.IsTrue(timing.AreValuesPositive);
        }

        [Test]
        public void Default_TotalDuration_IsLongerThanPhase7H()
        {
            // 7H.1 tuning makes the observation slower/more readable than the 3.0s 7H pass.
            Assert.Greater(ObservationPassTiming.Default.TotalSeconds, Phase7HTotalSeconds);
        }

        [Test]
        public void Default_CameraMove_IsPositive_AndSlowerThanPhase7H()
        {
            var timing = ObservationPassTiming.Default;
            Assert.Greater(timing.CameraMoveSeconds, 0f);
            Assert.Greater(timing.CameraMoveSeconds, Phase7HMoveSeconds);
        }

        [Test]
        public void Default_CameraReturn_IsPositive_AndSlowerThanPhase7H()
        {
            var timing = ObservationPassTiming.Default;
            Assert.Greater(timing.CameraReturnSeconds, 0f);
            Assert.Greater(timing.CameraReturnSeconds, Phase7HReturnSeconds);
        }

        [Test]
        public void Default_Total_IsBounded_AndNotExcessive()
        {
            // Short but readable: comfortably under ~6s so the pass never feels painfully long.
            Assert.LessOrEqual(ObservationPassTiming.Default.TotalSeconds, 6f);
        }

        [Test]
        public void TotalSeconds_IsSumOfPhases()
        {
            var timing = new ObservationPassTiming(2.0f, 0.6f, 0.4f);
            Assert.AreEqual(3.0f, timing.TotalSeconds, 0.0001f);
        }

        [Test]
        public void NegativeInputs_AreClampedToZero_AndNotPositive()
        {
            var timing = new ObservationPassTiming(-1f, -2f, -3f);
            Assert.AreEqual(0f, timing.ObservationHoldSeconds);
            Assert.AreEqual(0f, timing.CameraMoveSeconds);
            Assert.AreEqual(0f, timing.CameraReturnSeconds);
            Assert.IsFalse(timing.AreValuesPositive);
        }
    }
}
