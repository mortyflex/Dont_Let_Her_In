using NUnit.Framework;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class QuestionEvaluatorTests
    {
        // 10s timer keeps the speed boundaries easy to read: Fast <= 3.5s, Normal <= 7.0s.
        private static QuestionData MakeQuestion(float timeLimitSeconds = 10f, int correctAnswerIndex = 1)
        {
            return QuestionData.Create(
                id: "q-test",
                type: QuestionType.Observation,
                prompt: "Which room number blinked?",
                answers: new[] { "101", "104", "140", "401" },
                correctAnswerIndex: correctAnswerIndex,
                timeLimitSeconds: timeLimitSeconds);
        }

        // ---- Answer evaluation ------------------------------------------------

        [Test]
        public void CorrectAnswer_ReturnsIsCorrectTrue()
        {
            var question = MakeQuestion(correctAnswerIndex: 1);

            AnswerResult result = QuestionEvaluator.Evaluate(question, selectedAnswerIndex: 1, responseTimeSeconds: 2f);

            Assert.IsTrue(result.IsCorrect);
            Assert.IsFalse(result.IsTimeout);
            Assert.AreEqual("q-test", result.QuestionId);
        }

        [Test]
        public void WrongAnswer_ReturnsIsCorrectFalse()
        {
            var question = MakeQuestion(correctAnswerIndex: 1);

            AnswerResult result = QuestionEvaluator.Evaluate(question, selectedAnswerIndex: 0, responseTimeSeconds: 2f);

            Assert.IsFalse(result.IsCorrect);
            Assert.AreEqual(0, result.SelectedAnswerIndex);
        }

        [Test]
        public void Timeout_ReturnsIsTimeoutTrue()
        {
            var question = MakeQuestion();

            AnswerResult result = QuestionEvaluator.EvaluateTimeout(question);

            Assert.IsTrue(result.IsTimeout);
            Assert.IsFalse(result.IsCorrect);
            Assert.AreEqual(AnswerSpeed.Timeout, result.Speed);
            Assert.AreEqual(-1, result.SelectedAnswerIndex);
        }

        [Test]
        public void InvalidAnswerIndex_IsHandledSafely()
        {
            var question = MakeQuestion();

            // Index 99 is out of range; must not throw and must be incorrect.
            AnswerResult result = QuestionEvaluator.Evaluate(question, selectedAnswerIndex: 99, responseTimeSeconds: 2f);

            Assert.IsFalse(result.IsCorrect);
            Assert.AreEqual(99, result.SelectedAnswerIndex, "Invalid index is preserved for diagnostics.");
        }

        [Test]
        public void NegativeAnswerIndex_IsHandledSafely()
        {
            var question = MakeQuestion();

            AnswerResult result = QuestionEvaluator.Evaluate(question, selectedAnswerIndex: -5, responseTimeSeconds: 2f);

            Assert.IsFalse(result.IsCorrect);
            Assert.AreEqual(-5, result.SelectedAnswerIndex);
        }

        // ---- Speed classification --------------------------------------------

        [Test]
        public void FastAnswer_IsClassifiedAsFast()
        {
            // 2s of 10s = 20% -> Fast.
            Assert.AreEqual(
                AnswerSpeed.Fast,
                QuestionEvaluator.ClassifyAnswerSpeed(2f, 10f, timedOut: false));
        }

        [Test]
        public void NormalAnswer_IsClassifiedAsNormal()
        {
            // 5s of 10s = 50% -> Normal.
            Assert.AreEqual(
                AnswerSpeed.Normal,
                QuestionEvaluator.ClassifyAnswerSpeed(5f, 10f, timedOut: false));
        }

        [Test]
        public void SlowAnswer_IsClassifiedAsSlow()
        {
            // 8s of 10s = 80% -> Slow.
            Assert.AreEqual(
                AnswerSpeed.Slow,
                QuestionEvaluator.ClassifyAnswerSpeed(8f, 10f, timedOut: false));
        }

        [Test]
        public void TimeoutAnswer_IsClassifiedAsTimeout()
        {
            // Reaching the limit is a timeout even when the timedOut flag is not set.
            Assert.AreEqual(
                AnswerSpeed.Timeout,
                QuestionEvaluator.ClassifyAnswerSpeed(10f, 10f, timedOut: false));
        }

        [Test]
        public void FastBoundary_IsHandledConsistently()
        {
            // Exactly 35% (3.5s of 10s) is the inclusive upper edge of Fast.
            Assert.AreEqual(
                AnswerSpeed.Fast,
                QuestionEvaluator.ClassifyAnswerSpeed(3.5f, 10f, timedOut: false));
        }

        [Test]
        public void NormalBoundary_IsHandledConsistently()
        {
            // Exactly 70% (7.0s of 10s) is the inclusive upper edge of Normal.
            Assert.AreEqual(
                AnswerSpeed.Normal,
                QuestionEvaluator.ClassifyAnswerSpeed(7f, 10f, timedOut: false));
        }

        // ---- QuestionData validation -----------------------------------------

        [Test]
        public void QuestionData_WithPrompt_IsValid()
        {
            Assert.IsTrue(MakeQuestion().IsValid());
        }

        [Test]
        public void QuestionData_WithoutPrompt_IsInvalid()
        {
            var question = QuestionData.Create(
                "q", QuestionType.Observation, prompt: "   ",
                answers: new[] { "a", "b" }, correctAnswerIndex: 0, timeLimitSeconds: 5f);

            Assert.IsFalse(question.IsValid());
        }

        [Test]
        public void QuestionData_WithAtLeastTwoAnswers_IsValid()
        {
            var question = QuestionData.Create(
                "q", QuestionType.SimpleLogic, prompt: "2 / 4 / 8 / ?",
                answers: new[] { "12", "16" }, correctAnswerIndex: 1, timeLimitSeconds: 6f);

            Assert.IsTrue(question.IsValid());
        }

        [Test]
        public void QuestionData_WithLessThanTwoAnswers_IsInvalid()
        {
            var question = QuestionData.Create(
                "q", QuestionType.SimpleLogic, prompt: "Only one option",
                answers: new[] { "16" }, correctAnswerIndex: 0, timeLimitSeconds: 6f);

            Assert.IsFalse(question.IsValid());
        }

        [Test]
        public void QuestionData_WithCorrectIndexInRange_IsValid()
        {
            var question = QuestionData.Create(
                "q", QuestionType.Observation, prompt: "Pick",
                answers: new[] { "a", "b", "c" }, correctAnswerIndex: 2, timeLimitSeconds: 5f);

            Assert.IsTrue(question.IsValid());
        }

        [Test]
        public void QuestionData_WithCorrectIndexOutOfRange_IsInvalid()
        {
            var question = QuestionData.Create(
                "q", QuestionType.Observation, prompt: "Pick",
                answers: new[] { "a", "b" }, correctAnswerIndex: 5, timeLimitSeconds: 5f);

            Assert.IsFalse(question.IsValid());
        }

        [Test]
        public void QuestionData_WithPositiveTimer_IsValid()
        {
            var question = QuestionData.Create(
                "q", QuestionType.Observation, prompt: "Pick",
                answers: new[] { "a", "b" }, correctAnswerIndex: 0, timeLimitSeconds: 0.5f);

            Assert.IsTrue(question.IsValid());
        }

        [Test]
        public void QuestionData_WithZeroTimer_IsInvalid()
        {
            var question = QuestionData.Create(
                "q", QuestionType.Observation, prompt: "Pick",
                answers: new[] { "a", "b" }, correctAnswerIndex: 0, timeLimitSeconds: 0f);

            Assert.IsFalse(question.IsValid());
        }
    }
}
