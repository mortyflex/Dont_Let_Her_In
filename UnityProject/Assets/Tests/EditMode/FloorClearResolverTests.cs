using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class FloorClearResolverTests
    {
        [Test]
        public void IsCorrect_TrueForAllCorrectSpeeds()
        {
            Assert.IsTrue(FloorClearResolver.IsCorrect(AnswerOutcome.CorrectFast));
            Assert.IsTrue(FloorClearResolver.IsCorrect(AnswerOutcome.CorrectNormal));
            Assert.IsTrue(FloorClearResolver.IsCorrect(AnswerOutcome.CorrectSlow));
        }

        [Test]
        public void IsCorrect_FalseForWrongAndTimeout()
        {
            Assert.IsFalse(FloorClearResolver.IsCorrect(AnswerOutcome.Wrong));
            Assert.IsFalse(FloorClearResolver.IsCorrect(AnswerOutcome.Timeout));
        }

        [Test]
        public void CorrectAnswer_OnNonFinalFloor_ClearsFloor()
        {
            Assert.AreEqual(FloorResolution.FloorCleared,
                FloorClearResolver.Resolve(AnswerOutcome.CorrectFast, isDead: false, isFinalFloor: false));
            Assert.AreEqual(FloorResolution.FloorCleared,
                FloorClearResolver.Resolve(AnswerOutcome.CorrectNormal, isDead: false, isFinalFloor: false));
            Assert.AreEqual(FloorResolution.FloorCleared,
                FloorClearResolver.Resolve(AnswerOutcome.CorrectSlow, isDead: false, isFinalFloor: false));
        }

        [Test]
        public void CorrectAnswer_OnFinalFloor_Escapes()
        {
            Assert.AreEqual(FloorResolution.Escaped,
                FloorClearResolver.Resolve(AnswerOutcome.CorrectFast, isDead: false, isFinalFloor: true));
            Assert.AreEqual(FloorResolution.Escaped,
                FloorClearResolver.Resolve(AnswerOutcome.CorrectSlow, isDead: false, isFinalFloor: true));
        }

        [Test]
        public void WrongAnswer_AliveDoesNotClearFloor_RetriesSameFloor()
        {
            Assert.AreEqual(FloorResolution.RetrySameFloor,
                FloorClearResolver.Resolve(AnswerOutcome.Wrong, isDead: false, isFinalFloor: false));
            // Even on the final floor, a wrong answer never escapes.
            Assert.AreEqual(FloorResolution.RetrySameFloor,
                FloorClearResolver.Resolve(AnswerOutcome.Wrong, isDead: false, isFinalFloor: true));
        }

        [Test]
        public void Timeout_AliveDoesNotClearFloor_RetriesSameFloor()
        {
            Assert.AreEqual(FloorResolution.RetrySameFloor,
                FloorClearResolver.Resolve(AnswerOutcome.Timeout, isDead: false, isFinalFloor: false));
            Assert.AreEqual(FloorResolution.RetrySameFloor,
                FloorClearResolver.Resolve(AnswerOutcome.Timeout, isDead: false, isFinalFloor: true));
        }

        [Test]
        public void Death_OverridesEverything_Lost()
        {
            // Loss wins over retry (wrong/timeout that killed).
            Assert.AreEqual(FloorResolution.Lost,
                FloorClearResolver.Resolve(AnswerOutcome.Wrong, isDead: true, isFinalFloor: false));
            Assert.AreEqual(FloorResolution.Lost,
                FloorClearResolver.Resolve(AnswerOutcome.Timeout, isDead: true, isFinalFloor: false));
            // Loss wins even over a (theoretical) correct outcome and even on the final floor.
            Assert.AreEqual(FloorResolution.Lost,
                FloorClearResolver.Resolve(AnswerOutcome.CorrectFast, isDead: true, isFinalFloor: true));
        }
    }
}
