using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class AnswerOutcomeResolverTests
    {
        private static AnswerResult Correct(AnswerSpeed speed)
        {
            // A correct, non-timeout answer at the given speed.
            return new AnswerResult(
                questionId: "q",
                isCorrect: true,
                speed: speed,
                selectedAnswerIndex: 1,
                correctAnswerIndex: 1,
                responseTimeSeconds: 1f,
                timeLimitSeconds: 8f,
                isTimeout: false);
        }

        [Test]
        public void CorrectFast_MapsToCorrectFast()
        {
            Assert.AreEqual(AnswerOutcome.CorrectFast, AnswerOutcomeResolver.Resolve(Correct(AnswerSpeed.Fast)));
        }

        [Test]
        public void CorrectNormal_MapsToCorrectNormal()
        {
            Assert.AreEqual(AnswerOutcome.CorrectNormal, AnswerOutcomeResolver.Resolve(Correct(AnswerSpeed.Normal)));
        }

        [Test]
        public void CorrectSlow_MapsToCorrectSlow()
        {
            Assert.AreEqual(AnswerOutcome.CorrectSlow, AnswerOutcomeResolver.Resolve(Correct(AnswerSpeed.Slow)));
        }

        [Test]
        public void WrongAnswer_MapsToWrong()
        {
            var wrong = new AnswerResult("q", false, AnswerSpeed.Fast, 0, 1, 1f, 8f, false);
            Assert.AreEqual(AnswerOutcome.Wrong, AnswerOutcomeResolver.Resolve(wrong));
        }

        [Test]
        public void Timeout_MapsToTimeout()
        {
            var timeout = new AnswerResult("q", false, AnswerSpeed.Timeout, -1, 1, 8f, 8f, true);
            Assert.AreEqual(AnswerOutcome.Timeout, AnswerOutcomeResolver.Resolve(timeout));
        }

        [Test]
        public void TimeoutTakesPriorityOverCorrectFlag()
        {
            // Defensive: an inconsistent "correct + timeout" result still resolves to Timeout.
            var weird = new AnswerResult("q", true, AnswerSpeed.Timeout, 1, 1, 8f, 8f, true);
            Assert.AreEqual(AnswerOutcome.Timeout, AnswerOutcomeResolver.Resolve(weird));
        }
    }
}
