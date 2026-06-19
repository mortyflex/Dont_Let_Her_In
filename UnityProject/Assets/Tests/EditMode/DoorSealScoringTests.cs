using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class DoorSealScoringTests
    {
        [Test]
        public void Wrong_ScoresZero()
        {
            Assert.AreEqual(0f, DoorSealScoring.Score(AnswerOutcome.Wrong, 85));
            Assert.AreEqual(0f, DoorSealScoring.Score(AnswerOutcome.Wrong, 10));
        }

        [Test]
        public void Timeout_ScoresZero()
        {
            Assert.AreEqual(0f, DoorSealScoring.Score(AnswerOutcome.Timeout, 85));
            Assert.AreEqual(0f, DoorSealScoring.Score(AnswerOutcome.Timeout, 10));
        }

        [Test]
        public void CorrectFast_ScoresMoreThanCorrectNormal_AtSameDistance()
        {
            float fast = DoorSealScoring.Score(AnswerOutcome.CorrectFast, 85);
            float normal = DoorSealScoring.Score(AnswerOutcome.CorrectNormal, 85);
            Assert.Greater(fast, normal);
        }

        [Test]
        public void CorrectNormal_ScoresMoreThanCorrectSlow_AtSameDistance()
        {
            float normal = DoorSealScoring.Score(AnswerOutcome.CorrectNormal, 85);
            float slow = DoorSealScoring.Score(AnswerOutcome.CorrectSlow, 85);
            Assert.Greater(normal, slow);
        }

        [Test]
        public void CloserThreat_IncreasesScore_ForSameOutcome()
        {
            float far = DoorSealScoring.Score(AnswerOutcome.CorrectFast, 90);
            float mid = DoorSealScoring.Score(AnswerOutcome.CorrectFast, 60);
            float close = DoorSealScoring.Score(AnswerOutcome.CorrectFast, 30);
            float veryClose = DoorSealScoring.Score(AnswerOutcome.CorrectFast, 10);

            Assert.Less(far, mid);
            Assert.Less(mid, close);
            Assert.Less(close, veryClose);
        }

        [Test]
        public void Score_MatchesSpecExamples()
        {
            Assert.AreEqual(100f, DoorSealScoring.Score(AnswerOutcome.CorrectFast, 90), 0.001f);
            Assert.AreEqual(160f, DoorSealScoring.Score(AnswerOutcome.CorrectFast, 20), 0.001f);
            Assert.AreEqual(94.5f, DoorSealScoring.Score(AnswerOutcome.CorrectNormal, 45), 0.001f);
            Assert.AreEqual(64f, DoorSealScoring.Score(AnswerOutcome.CorrectSlow, 20), 0.001f);
        }

        [Test]
        public void ProximityMultiplier_FollowsBands()
        {
            Assert.AreEqual(1.00f, DoorSealScoring.ProximityMultiplier(80), 0.001f);
            Assert.AreEqual(1.15f, DoorSealScoring.ProximityMultiplier(50), 0.001f);
            Assert.AreEqual(1.35f, DoorSealScoring.ProximityMultiplier(25), 0.001f);
            Assert.AreEqual(1.60f, DoorSealScoring.ProximityMultiplier(24), 0.001f);
        }

        // ---- DoorSealScore running total ----

        [Test]
        public void DoorSealScore_StartsAtZero_WithRequired()
        {
            var seal = new DoorSealScore();
            seal.StartFloor(220);
            Assert.AreEqual(0, seal.CurrentRounded);
            Assert.AreEqual(220, seal.Required);
            Assert.IsFalse(seal.IsSealed);
        }

        [Test]
        public void DoorSealScore_Add_IgnoresZeroAndNegative()
        {
            var seal = new DoorSealScore();
            seal.StartFloor(100);
            seal.Add(0f);
            seal.Add(-50f);
            Assert.AreEqual(0, seal.CurrentRounded);
        }

        [Test]
        public void DoorSealScore_IsSealed_WhenThresholdReached()
        {
            var seal = new DoorSealScore();
            seal.StartFloor(180);
            seal.Add(100f);
            Assert.IsFalse(seal.IsSealed);
            seal.Add(80f);
            Assert.IsTrue(seal.IsSealed);
        }

        [Test]
        public void DoorSealScore_StartFloor_ResetsCurrent()
        {
            var seal = new DoorSealScore();
            seal.StartFloor(180);
            seal.Add(200f);
            Assert.IsTrue(seal.IsSealed);

            seal.StartFloor(260); // new floor
            Assert.AreEqual(0, seal.CurrentRounded);
            Assert.AreEqual(260, seal.Required);
            Assert.IsFalse(seal.IsSealed);
        }
    }
}
