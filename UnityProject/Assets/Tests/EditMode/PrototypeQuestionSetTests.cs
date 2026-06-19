using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class PrototypeQuestionSetTests
    {
        [Test]
        public void BuildAll_ReturnsFiveQuestions()
        {
            IReadOnlyList<QuestionData> questions = PrototypeQuestionSet.BuildAll();
            Assert.AreEqual(PrototypeQuestionSet.Count, questions.Count);
            Assert.AreEqual(5, questions.Count);
        }

        [Test]
        public void AllQuestions_AreValid()
        {
            foreach (QuestionData question in PrototypeQuestionSet.BuildAll())
            {
                Assert.IsTrue(question.IsValid(), $"Question '{question.Id}' should be valid.");
                Assert.AreEqual(4, question.AnswerCount, $"Question '{question.Id}' should have 4 answers.");
            }
        }

        [Test]
        public void CorrectAnswers_MatchDesignSpec()
        {
            IReadOnlyList<QuestionData> q = PrototypeQuestionSet.BuildAll();

            Assert.AreEqual("104", q[0].Answers[q[0].CorrectAnswerIndex]);
            Assert.AreEqual("Key", q[1].Answers[q[1].CorrectAnswerIndex]);
            Assert.AreEqual("Do not look left", q[2].Answers[q[2].CorrectAnswerIndex]);
            Assert.AreEqual("272", q[3].Answers[q[3].CorrectAnswerIndex]);
            Assert.AreEqual("Wait", q[4].Answers[q[4].CorrectAnswerIndex]);
        }

        [Test]
        public void TimeLimits_DecreasePerFloor()
        {
            IReadOnlyList<QuestionData> q = PrototypeQuestionSet.BuildAll();
            Assert.AreEqual(8f, q[0].TimeLimitSeconds);
            Assert.AreEqual(7f, q[1].TimeLimitSeconds);
            Assert.AreEqual(6f, q[2].TimeLimitSeconds);
            Assert.AreEqual(5f, q[3].TimeLimitSeconds);
            Assert.AreEqual(4f, q[4].TimeLimitSeconds);
        }
    }
}
