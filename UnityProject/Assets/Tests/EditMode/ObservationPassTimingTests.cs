using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ObservationPassTimingTests
    {
        [Test]
        public void Default_UsesRecommendedPhase7HValues()
        {
            var timing = ObservationPassTiming.Default;
            Assert.AreEqual(2.0f, timing.ObservationHoldSeconds, 0.0001f);
            Assert.AreEqual(0.6f, timing.CameraMoveSeconds, 0.0001f);
            Assert.AreEqual(0.4f, timing.CameraReturnSeconds, 0.0001f);
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
