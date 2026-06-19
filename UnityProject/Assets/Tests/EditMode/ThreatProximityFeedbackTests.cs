using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class ThreatProximityFeedbackTests
    {
        [Test]
        public void GetLevel_MapsThresholdsToExpectedLevels()
        {
            Assert.AreEqual(ThreatProximityLevel.Calm, ThreatProximityFeedback.GetLevel(100));
            Assert.AreEqual(ThreatProximityLevel.Calm, ThreatProximityFeedback.GetLevel(81));
            Assert.AreEqual(ThreatProximityLevel.Observed, ThreatProximityFeedback.GetLevel(80));
            Assert.AreEqual(ThreatProximityLevel.Observed, ThreatProximityFeedback.GetLevel(61));
            Assert.AreEqual(ThreatProximityLevel.VisibleDanger, ThreatProximityFeedback.GetLevel(60));
            Assert.AreEqual(ThreatProximityLevel.VisibleDanger, ThreatProximityFeedback.GetLevel(41));
            Assert.AreEqual(ThreatProximityLevel.DangerClose, ThreatProximityFeedback.GetLevel(40));
            Assert.AreEqual(ThreatProximityLevel.DangerClose, ThreatProximityFeedback.GetLevel(26));
            Assert.AreEqual(ThreatProximityLevel.NearDoor, ThreatProximityFeedback.GetLevel(25));
            Assert.AreEqual(ThreatProximityLevel.NearDoor, ThreatProximityFeedback.GetLevel(11));
            Assert.AreEqual(ThreatProximityLevel.Panic, ThreatProximityFeedback.GetLevel(10));
            Assert.AreEqual(ThreatProximityLevel.Panic, ThreatProximityFeedback.GetLevel(1));
            Assert.AreEqual(ThreatProximityLevel.Panic, ThreatProximityFeedback.GetLevel(0));
        }

        [Test]
        public void GetMessage_ReturnsExpectedText()
        {
            Assert.AreEqual(string.Empty, ThreatProximityFeedback.GetMessage(100));
            Assert.AreEqual("SHE IS WATCHING", ThreatProximityFeedback.GetMessage(80));
            Assert.AreEqual("SHE IS IN THE HALL", ThreatProximityFeedback.GetMessage(60));
            Assert.AreEqual("SHE IS CLOSE", ThreatProximityFeedback.GetMessage(40));
            Assert.AreEqual("SHE IS AT THE DOOR", ThreatProximityFeedback.GetMessage(25));
            Assert.AreEqual("DO NOT LET HER IN", ThreatProximityFeedback.GetMessage(10));
        }

        [Test]
        public void IsNearDeath_TrueAtOrBelow25AndAboveZero()
        {
            Assert.IsFalse(ThreatProximityFeedback.IsNearDeath(26));
            Assert.IsTrue(ThreatProximityFeedback.IsNearDeath(25));
            Assert.IsTrue(ThreatProximityFeedback.IsNearDeath(1));
            Assert.IsFalse(ThreatProximityFeedback.IsNearDeath(0));
        }

        [Test]
        public void IsPanic_TrueAtOrBelow10AndAboveZero()
        {
            Assert.IsFalse(ThreatProximityFeedback.IsPanic(11));
            Assert.IsTrue(ThreatProximityFeedback.IsPanic(10));
            Assert.IsTrue(ThreatProximityFeedback.IsPanic(1));
            Assert.IsFalse(ThreatProximityFeedback.IsPanic(0));
        }

        [Test]
        public void GetOverlayAlpha_ZeroAboveNearDeath_RampsToMaxAtZero()
        {
            Assert.AreEqual(0f, ThreatProximityFeedback.GetOverlayAlpha(100), 0.0001f);
            Assert.AreEqual(0f, ThreatProximityFeedback.GetOverlayAlpha(26), 0.0001f);
            Assert.AreEqual(0f, ThreatProximityFeedback.GetOverlayAlpha(25), 0.0001f);
            Assert.AreEqual(ThreatProximityFeedback.MaxOverlayAlpha,
                ThreatProximityFeedback.GetOverlayAlpha(0), 0.0001f);
        }

        [Test]
        public void GetOverlayAlpha_IncreasesAsDistanceDecreases()
        {
            float far = ThreatProximityFeedback.GetOverlayAlpha(20);
            float mid = ThreatProximityFeedback.GetOverlayAlpha(10);
            float near = ThreatProximityFeedback.GetOverlayAlpha(2);

            Assert.Greater(mid, far);
            Assert.Greater(near, mid);
            Assert.LessOrEqual(near, ThreatProximityFeedback.MaxOverlayAlpha);
            Assert.GreaterOrEqual(far, 0f);
        }
    }
}
