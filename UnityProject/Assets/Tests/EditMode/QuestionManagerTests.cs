using NUnit.Framework;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class QuestionManagerTests
    {
        private static QuestionData MakeQuestion(float timeLimitSeconds = 10f, int correctAnswerIndex = 1)
        {
            return QuestionData.Create(
                id: "q-test",
                type: QuestionType.Observation,
                prompt: "Which symbol was in the center?",
                answers: new[] { "Eye", "Key", "Hand", "Door" },
                correctAnswerIndex: correctAnswerIndex,
                timeLimitSeconds: timeLimitSeconds);
        }

        [Test]
        public void StartQuestion_SetsQuestionActive()
        {
            var manager = new QuestionManager();

            manager.StartQuestion(MakeQuestion());

            Assert.IsTrue(manager.IsQuestionActive);
            Assert.IsNotNull(manager.ActiveQuestion);
        }

        [Test]
        public void StartQuestion_ResetsElapsedTime()
        {
            var manager = new QuestionManager();
            manager.StartQuestion(MakeQuestion());
            manager.Tick(2f);

            // Starting a fresh question must clear accumulated time and any previous result.
            manager.StartQuestion(MakeQuestion());

            Assert.AreEqual(0f, manager.ElapsedTime);
            Assert.IsNull(manager.LastResult);
        }

        [Test]
        public void SubmitAnswer_ProducesAnswerResult()
        {
            var manager = new QuestionManager();
            manager.StartQuestion(MakeQuestion(correctAnswerIndex: 1));
            manager.Tick(2f);

            AnswerResult? result = manager.SubmitAnswer(1);

            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(result.Value.IsCorrect);
            Assert.IsTrue(manager.LastResult.HasValue);
            Assert.AreEqual(AnswerSpeed.Fast, manager.LastResult.Value.Speed);
        }

        [Test]
        public void SubmitAnswer_EndsQuestion()
        {
            var manager = new QuestionManager();
            manager.StartQuestion(MakeQuestion());

            manager.SubmitAnswer(1);

            Assert.IsFalse(manager.IsQuestionActive);
        }

        [Test]
        public void Timeout_ProducesTimeoutResult()
        {
            var manager = new QuestionManager();
            manager.StartQuestion(MakeQuestion());

            AnswerResult? result = manager.ResolveTimeout();

            Assert.IsTrue(result.HasValue);
            Assert.IsTrue(result.Value.IsTimeout);
            Assert.IsFalse(result.Value.IsCorrect);
            Assert.AreEqual(AnswerSpeed.Timeout, result.Value.Speed);
        }

        [Test]
        public void Timeout_EndsQuestion()
        {
            var manager = new QuestionManager();
            manager.StartQuestion(MakeQuestion());

            manager.ResolveTimeout();

            Assert.IsFalse(manager.IsQuestionActive);
        }

        [Test]
        public void Tick_PastTimeLimit_AutoResolvesTimeout()
        {
            var manager = new QuestionManager();
            manager.StartQuestion(MakeQuestion(timeLimitSeconds: 5f));

            manager.Tick(6f);

            Assert.IsFalse(manager.IsQuestionActive);
            Assert.IsTrue(manager.LastResult.HasValue);
            Assert.IsTrue(manager.LastResult.Value.IsTimeout);
        }

        [Test]
        public void Reset_ClearsCurrentQuestion()
        {
            var manager = new QuestionManager();
            manager.StartQuestion(MakeQuestion());
            manager.Tick(2f);

            manager.Reset();

            Assert.IsFalse(manager.IsQuestionActive);
            Assert.IsNull(manager.ActiveQuestion);
            Assert.AreEqual(0f, manager.ElapsedTime);
            Assert.IsNull(manager.LastResult);
        }

        [Test]
        public void CannotSubmitAnswer_WhenNoQuestionActive()
        {
            var manager = new QuestionManager();

            // No StartQuestion call: submitting must not throw and must produce nothing.
            AnswerResult? result = manager.SubmitAnswer(0);

            Assert.IsFalse(result.HasValue);
            Assert.IsNull(manager.LastResult);
        }
    }
}
