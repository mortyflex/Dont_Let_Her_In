using NUnit.Framework;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class InterQuestionPacingTests
    {
        [Test]
        public void IsDangerOutcome_TrueForWrongAndTimeout()
        {
            Assert.IsTrue(InterQuestionPacing.IsDangerOutcome(AnswerOutcome.Wrong));
            Assert.IsTrue(InterQuestionPacing.IsDangerOutcome(AnswerOutcome.Timeout));
        }

        [Test]
        public void IsDangerOutcome_FalseForCorrectOutcomes()
        {
            Assert.IsFalse(InterQuestionPacing.IsDangerOutcome(AnswerOutcome.CorrectFast));
            Assert.IsFalse(InterQuestionPacing.IsDangerOutcome(AnswerOutcome.CorrectNormal));
            Assert.IsFalse(InterQuestionPacing.IsDangerOutcome(AnswerOutcome.CorrectSlow));
        }

        [Test]
        public void GetHoldSeconds_CorrectOutcomes_UseBaseHold()
        {
            Assert.AreEqual(1.2f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.CorrectFast, 1.2f, 0.3f));
            Assert.AreEqual(1.2f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.CorrectNormal, 1.2f, 0.3f));
            Assert.AreEqual(1.2f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.CorrectSlow, 1.2f, 0.3f));
        }

        [Test]
        public void GetHoldSeconds_DangerOutcomes_AddDangerExtra()
        {
            Assert.AreEqual(1.5f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.Wrong, 1.2f, 0.3f), 0.0001f);
            Assert.AreEqual(1.5f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.Timeout, 1.2f, 0.3f), 0.0001f);
        }

        [Test]
        public void GetHoldSeconds_DangerHoldIsNeverShorterThanCorrectHold()
        {
            float correct = InterQuestionPacing.GetHoldSeconds(AnswerOutcome.CorrectNormal, 1.2f, 0.3f);
            float wrong = InterQuestionPacing.GetHoldSeconds(AnswerOutcome.Wrong, 1.2f, 0.3f);
            float timeout = InterQuestionPacing.GetHoldSeconds(AnswerOutcome.Timeout, 1.2f, 0.3f);
            Assert.GreaterOrEqual(wrong, correct);
            Assert.GreaterOrEqual(timeout, correct);
        }

        [Test]
        public void GetHoldSeconds_NegativeInputs_AreClampedToZero()
        {
            Assert.AreEqual(0f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.CorrectFast, -1f, 0.3f));
            // Base clamps to 0, danger extra clamps to 0 -> total 0.
            Assert.AreEqual(0f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.Wrong, -1f, -1f));
            // Base 1.2, negative extra clamps to 0 -> base only.
            Assert.AreEqual(1.2f, InterQuestionPacing.GetHoldSeconds(AnswerOutcome.Timeout, 1.2f, -5f));
        }
    }
}
