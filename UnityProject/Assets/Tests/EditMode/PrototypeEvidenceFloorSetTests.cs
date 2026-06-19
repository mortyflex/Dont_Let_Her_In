using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7E: the code-authored evidence prototype is well-formed — 5 floors, 5 trials
    /// each (25 total), every trial grounded in an existing clue, exactly 4 answers with one
    /// correct option matching the clue's evidence, and EN/FR coverage for prompts/answers.
    /// </summary>
    public sealed class PrototypeEvidenceFloorSetTests
    {
        [Test]
        public void BuildAll_HasFiveFloors()
        {
            IReadOnlyList<FloorObservationSet> floors = PrototypeEvidenceFloorSet.BuildAll();
            Assert.AreEqual(5, floors.Count);
            Assert.AreEqual(PrototypeEvidenceFloorSet.FloorCount, floors.Count);
        }

        [Test]
        public void EachFloor_HasFiveTrials_TwentyFiveTotal()
        {
            IReadOnlyList<FloorObservationSet> floors = PrototypeEvidenceFloorSet.BuildAll();
            int total = 0;
            foreach (FloorObservationSet floor in floors)
            {
                Assert.AreEqual(5, floor.TrialCount, $"Floor {floor.FloorDisplayNumber} should have 5 trials.");
                Assert.AreEqual(PrototypeEvidenceFloorSet.TrialsPerFloor, floor.TrialCount);
                total += floor.TrialCount;
            }
            Assert.AreEqual(25, total);
        }

        [Test]
        public void FloorsAreNumbered_FiveDownToOne_DescentOrder()
        {
            IReadOnlyList<FloorObservationSet> floors = PrototypeEvidenceFloorSet.BuildAll();
            int[] expected = { 5, 4, 3, 2, 1 };
            for (int i = 0; i < floors.Count; i++)
            {
                Assert.AreEqual(expected[i], floors[i].FloorDisplayNumber);
            }
        }

        [Test]
        public void EveryTrial_ReferencesAnExistingClueOnItsFloor()
        {
            foreach (FloorObservationSet floor in PrototypeEvidenceFloorSet.BuildAll())
            {
                foreach (EvidenceTrial trial in floor.Trials)
                {
                    Assert.IsTrue(floor.HasClue(trial.ClueId),
                        $"Trial '{trial.Id}' references missing clue '{trial.ClueId}'.");
                }
            }
        }

        [Test]
        public void EveryTrial_HasFourAnswers_AndExactlyOneCorrect()
        {
            foreach (FloorObservationSet floor in PrototypeEvidenceFloorSet.BuildAll())
            {
                foreach (EvidenceTrial trial in floor.Trials)
                {
                    Assert.AreEqual(4, trial.AnswerCount, $"Trial '{trial.Id}' should have 4 answers.");
                    Assert.AreEqual(1, trial.CorrectAnswerCount, $"Trial '{trial.Id}' should have exactly 1 correct answer.");
                }
            }
        }

        [Test]
        public void EveryCorrectAnswer_MatchesItsClueEvidenceValue()
        {
            foreach (FloorObservationSet floor in PrototypeEvidenceFloorSet.BuildAll())
            {
                foreach (EvidenceTrial trial in floor.Trials)
                {
                    CorridorClue clue = floor.FindClue(trial.ClueId);
                    Assert.IsNotNull(clue, $"Trial '{trial.Id}' clue missing.");
                    Assert.IsNotNull(trial.CorrectAnswer, $"Trial '{trial.Id}' has no single correct answer.");
                    Assert.AreEqual(clue.EvidenceValue, trial.CorrectAnswer.Text.Get(GameLanguage.English),
                        $"Trial '{trial.Id}' correct answer must match clue evidence '{clue.EvidenceValue}'.");
                }
            }
        }

        [Test]
        public void EveryPrompt_HasEnglishAndFrench()
        {
            foreach (FloorObservationSet floor in PrototypeEvidenceFloorSet.BuildAll())
            {
                foreach (EvidenceTrial trial in floor.Trials)
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(trial.Prompt.Get(GameLanguage.English)),
                        $"Trial '{trial.Id}' missing English prompt.");
                    Assert.IsFalse(string.IsNullOrWhiteSpace(trial.Prompt.Get(GameLanguage.French)),
                        $"Trial '{trial.Id}' missing French prompt.");
                }
            }
        }

        [Test]
        public void EveryAnswer_HasEnglishAndFrench()
        {
            foreach (FloorObservationSet floor in PrototypeEvidenceFloorSet.BuildAll())
            {
                foreach (EvidenceTrial trial in floor.Trials)
                {
                    foreach (EvidenceAnswerOption answer in trial.Answers)
                    {
                        Assert.IsFalse(string.IsNullOrWhiteSpace(answer.Text.Get(GameLanguage.English)),
                            $"Answer '{answer.Id}' missing English text.");
                        Assert.IsFalse(string.IsNullOrWhiteSpace(answer.Text.Get(GameLanguage.French)),
                            $"Answer '{answer.Id}' missing French text.");
                    }
                }
            }
        }

        [Test]
        public void AllTrialIds_AreUnique_AcrossPrototype()
        {
            var ids = new HashSet<string>();
            foreach (FloorObservationSet floor in PrototypeEvidenceFloorSet.BuildAll())
            {
                foreach (EvidenceTrial trial in floor.Trials)
                {
                    Assert.IsTrue(ids.Add(trial.Id), $"Duplicate trial id '{trial.Id}'.");
                }
            }
            Assert.AreEqual(25, ids.Count);
        }

        [Test]
        public void Prototype_PassesEvidenceValidation()
        {
            EvidenceValidationResult result =
                EvidenceTrialValidator.ValidateAll(PrototypeEvidenceFloorSet.BuildAll());
            Assert.IsTrue(result.IsValid, string.Join("; ", result.Messages));
        }
    }
}
