using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    public sealed class PrototypeFloorSetTests
    {
        [Test]
        public void BuildAll_HasFiveFloors_FiveTrialsEach_TwentyFiveTotal()
        {
            IReadOnlyList<FloorDefinition> floors = PrototypeFloorSet.BuildAll();
            Assert.AreEqual(5, floors.Count);
            Assert.AreEqual(PrototypeFloorSet.FloorCount, floors.Count);

            int totalTrials = 0;
            foreach (FloorDefinition floor in floors)
            {
                Assert.AreEqual(5, floor.TrialCount, $"Floor {floor.FloorIndex} should have 5 trials.");
                Assert.AreEqual(PrototypeFloorSet.TrialsPerFloor, floor.TrialCount);
                totalTrials += floor.TrialCount;
            }
            Assert.AreEqual(25, totalTrials, "Prototype should have 25 trials/cues total.");
        }

        [Test]
        public void FloorsAreNumbered_OneToFive_InOrder()
        {
            IReadOnlyList<FloorDefinition> floors = PrototypeFloorSet.BuildAll();
            for (int i = 0; i < floors.Count; i++)
            {
                Assert.AreEqual(i + 1, floors[i].FloorIndex);
            }
        }

        [Test]
        public void EveryTrial_HasValidQuestion_FourAnswers_OneCorrect_AndMatchingCue()
        {
            foreach (FloorDefinition floor in PrototypeFloorSet.BuildAll())
            {
                foreach (FloorTrial trial in floor.Trials)
                {
                    Assert.IsNotNull(trial.Question, $"Floor {floor.FloorIndex} trial has a question.");
                    Assert.IsTrue(trial.Question.IsValid(), $"Question '{trial.Question.Id}' should be valid.");
                    Assert.AreEqual(4, trial.Question.AnswerCount, $"Question '{trial.Question.Id}' should have 4 answers.");
                    Assert.IsTrue(trial.Question.IsAnswerIndexInRange(trial.Question.CorrectAnswerIndex),
                        $"Question '{trial.Question.Id}' should have exactly one in-range correct answer.");

                    Assert.IsNotNull(trial.Cue, $"Trial '{trial.Question.Id}' should have a cue.");
                    Assert.AreEqual(trial.Question.Id, trial.Cue.QuestionId,
                        "Cue must be paired with its own question id.");
                    Assert.Greater(trial.Cue.Lines.Count, 0, $"Cue '{trial.Cue.QuestionId}' should have lines.");
                }
            }
        }

        [Test]
        public void AllQuestionIds_AreUnique()
        {
            var ids = new HashSet<string>();
            foreach (FloorDefinition floor in PrototypeFloorSet.BuildAll())
            {
                foreach (FloorTrial trial in floor.Trials)
                {
                    Assert.IsTrue(ids.Add(trial.Question.Id), $"Duplicate question id '{trial.Question.Id}'.");
                }
            }
            Assert.AreEqual(25, ids.Count);
        }

        [Test]
        public void FirstTwoTrials_MatchKnownSpec()
        {
            IReadOnlyList<FloorDefinition> f = PrototypeFloorSet.BuildAll();

            // Trial 1 of each floor (reused validated content).
            Assert.AreEqual("104", CorrectAnswer(f, 0, 0));
            Assert.AreEqual("Key", CorrectAnswer(f, 1, 0));
            Assert.AreEqual("Do not look left", CorrectAnswer(f, 2, 0));
            Assert.AreEqual("272", CorrectAnswer(f, 3, 0));
            Assert.AreEqual("Wait", CorrectAnswer(f, 4, 0));

            // Trial 2 of each floor.
            Assert.AreEqual("Up", CorrectAnswer(f, 0, 1));
            Assert.AreEqual("Wait", CorrectAnswer(f, 1, 1));
            Assert.AreEqual("Door Open", CorrectAnswer(f, 2, 1));
            Assert.AreEqual("914", CorrectAnswer(f, 3, 1));
            Assert.AreEqual("Answer calmly", CorrectAnswer(f, 4, 1));
        }

        [Test]
        public void TimeLimits_ArePositive_AndFirstTrialDecreasesPerFloor()
        {
            IReadOnlyList<FloorDefinition> f = PrototypeFloorSet.BuildAll();
            float[] firstTrialTimers = { 8f, 7f, 6f, 5f, 4f };

            for (int floor = 0; floor < 5; floor++)
            {
                Assert.AreEqual(firstTrialTimers[floor], f[floor].Trials[0].Question.TimeLimitSeconds);
                foreach (FloorTrial trial in f[floor].Trials)
                {
                    Assert.Greater(trial.Question.TimeLimitSeconds, 0f);
                }
            }
        }

        [Test]
        public void TrialCounts_ReturnsFivePerFloor()
        {
            IReadOnlyList<int> counts = PrototypeFloorSet.TrialCounts();
            Assert.AreEqual(5, counts.Count);
            foreach (int c in counts) Assert.AreEqual(5, c);
        }

        private static string CorrectAnswer(IReadOnlyList<FloorDefinition> floors, int floor, int trial)
        {
            QuestionData q = floors[floor].Trials[trial].Question;
            return q.Answers[q.CorrectAnswerIndex];
        }
    }
}
