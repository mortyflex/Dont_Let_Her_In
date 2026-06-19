using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7F: the live playable trial content (PrototypeFloorSet) localizes EN/FR for
    /// prompts, answers and cues, while preserving gameplay structure (5 floors, 5 trials,
    /// 4 answers, same correct answer index, same threat/floor progression data).
    /// </summary>
    public sealed class PlayableContentLocalizationTests
    {
        [SetUp]
        public void SetEnglishDefault()
        {
            PrototypeLocalization.Language = GameLanguage.English;
        }

        [TearDown]
        public void RestoreDefaultLanguage()
        {
            // Language is global static state; reset so other tests see the English default.
            PrototypeLocalization.Language = PrototypeLocalization.DefaultLanguage;
        }

        private static IEnumerable<FloorTrial> AllTrials()
        {
            foreach (FloorDefinition floor in PrototypeFloorSet.BuildAll())
            {
                foreach (FloorTrial trial in floor.Trials)
                {
                    yield return trial;
                }
            }
        }

        [Test]
        public void DefaultLanguage_IsEnglish()
        {
            Assert.AreEqual(GameLanguage.English, PrototypeLocalization.DefaultLanguage);
        }

        [Test]
        public void EnglishLanguage_PromptAnswersCue_DisplayEnglish()
        {
            PrototypeLocalization.Language = GameLanguage.English;
            FloorTrial trial = PrototypeFloorSet.BuildAll()[0].Trials[0]; // floor 1, trial 1

            Assert.AreEqual("Which room number blinked?", trial.Question.Prompt);
            CollectionAssert.AreEqual(new[] { "101", "104", "108", "102" }, trial.Question.Answers);
            Assert.AreEqual("ROOM DISPLAY", trial.Cue.Label);
            CollectionAssert.AreEqual(new[] { "104" }, new List<string>(trial.Cue.Lines));
        }

        [Test]
        public void FrenchLanguage_PromptAnswersCue_DisplayFrench()
        {
            PrototypeLocalization.Language = GameLanguage.French;
            FloorTrial trial = PrototypeFloorSet.BuildAll()[0].Trials[0]; // floor 1, trial 1

            Assert.AreEqual("Quel numéro de chambre a clignoté ?", trial.Question.Prompt);
            CollectionAssert.AreEqual(new[] { "101", "104", "108", "102" }, trial.Question.Answers);
            Assert.AreEqual("AFFICHAGE", trial.Cue.Label);
            CollectionAssert.AreEqual(new[] { "104" }, new List<string>(trial.Cue.Lines));
        }

        [Test]
        public void FrenchLanguage_TranslatesWordAnswers()
        {
            PrototypeLocalization.Language = GameLanguage.French;
            // Floor 2, trial 1 (symbols): "Eye/Key/Hand/Door" -> "Œil/Clé/Main/Porte".
            FloorTrial trial = PrototypeFloorSet.BuildAll()[1].Trials[0];

            Assert.AreEqual("Quel symbole était au centre ?", trial.Question.Prompt);
            CollectionAssert.AreEqual(new[] { "Œil", "Clé", "Main", "Porte" }, trial.Question.Answers);
            Assert.AreEqual("SYMBOLES", trial.Cue.Label);
        }

        [Test]
        public void EveryTrial_HasEnglishPromptAnswersAndCue()
        {
            PrototypeLocalization.Language = GameLanguage.English;
            foreach (FloorTrial trial in AllTrials())
            {
                QuestionData q = trial.Question;
                Assert.IsFalse(string.IsNullOrWhiteSpace(q.PromptEnglish), $"{q.Id} missing English prompt.");
                Assert.AreEqual(4, q.AnswersEnglish.Length, $"{q.Id} should have 4 English answers.");
                foreach (string a in q.AnswersEnglish)
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(a), $"{q.Id} has an empty English answer.");
                }
                Assert.IsNotNull(trial.Cue, $"{q.Id} should have a cue.");
                Assert.IsFalse(string.IsNullOrWhiteSpace(trial.Cue.LabelEnglish), $"{q.Id} missing English cue label.");
                Assert.Greater(trial.Cue.LinesEnglish.Count, 0, $"{q.Id} missing English cue lines.");
            }
        }

        [Test]
        public void EveryTrial_HasFrenchPromptAnswersAndCue()
        {
            foreach (FloorTrial trial in AllTrials())
            {
                QuestionData q = trial.Question;
                Assert.IsTrue(q.HasFrench, $"{q.Id} should have full French prompt + answers.");
                Assert.IsFalse(string.IsNullOrWhiteSpace(q.PromptFrench), $"{q.Id} missing French prompt.");
                Assert.AreEqual(4, q.AnswersFrench.Length, $"{q.Id} should have 4 French answers.");
                foreach (string a in q.AnswersFrench)
                {
                    Assert.IsFalse(string.IsNullOrWhiteSpace(a), $"{q.Id} has an empty French answer.");
                }
                Assert.IsNotNull(trial.Cue, $"{q.Id} should have a cue.");
                Assert.IsTrue(trial.Cue.HasFrench, $"{q.Id} cue should have French label + lines.");
            }
        }

        [Test]
        public void FrenchContent_KeepsExactlyFourAnswersPerTrial()
        {
            PrototypeLocalization.Language = GameLanguage.French;
            foreach (FloorTrial trial in AllTrials())
            {
                Assert.AreEqual(4, trial.Question.Answers.Length, $"{trial.Question.Id} should still have 4 answers in French.");
            }
        }

        [Test]
        public void FrenchContent_KeepsSameCorrectAnswerIndexAndCount()
        {
            // Capture the correct index under English, then confirm it is unchanged in French.
            var correctById = new Dictionary<string, int>();
            PrototypeLocalization.Language = GameLanguage.English;
            foreach (FloorTrial trial in AllTrials())
            {
                correctById[trial.Question.Id] = trial.Question.CorrectAnswerIndex;
            }

            PrototypeLocalization.Language = GameLanguage.French;
            foreach (FloorTrial trial in AllTrials())
            {
                QuestionData q = trial.Question;
                Assert.AreEqual(correctById[q.Id], q.CorrectAnswerIndex, $"{q.Id} correct index changed with language.");
                Assert.IsTrue(q.IsAnswerIndexInRange(q.CorrectAnswerIndex), $"{q.Id} correct index out of range.");
                Assert.AreEqual(q.AnswersEnglish.Length, q.AnswersFrench.Length, $"{q.Id} EN/FR answer counts differ.");
            }
        }

        [Test]
        public void SwitchingLanguage_DoesNotChangeFloorOrTrialCount()
        {
            PrototypeLocalization.Language = GameLanguage.English;
            IReadOnlyList<FloorDefinition> en = PrototypeFloorSet.BuildAll();

            PrototypeLocalization.Language = GameLanguage.French;
            IReadOnlyList<FloorDefinition> fr = PrototypeFloorSet.BuildAll();

            Assert.AreEqual(en.Count, fr.Count);
            Assert.AreEqual(5, fr.Count);
            for (int i = 0; i < en.Count; i++)
            {
                Assert.AreEqual(en[i].TrialCount, fr[i].TrialCount);
                Assert.AreEqual(5, fr[i].TrialCount);
            }
            CollectionAssert.AreEqual(PrototypeFloorSet.TrialCounts(), new[] { 5, 5, 5, 5, 5 });
        }

        [Test]
        public void SwitchingLanguage_DoesNotChangeThreatFloorProgressionData()
        {
            // DescentFloorProfile start distances are language-independent gameplay data.
            int[] englishStarts = new int[5];
            PrototypeLocalization.Language = GameLanguage.English;
            for (int floor = 5; floor >= 1; floor--)
            {
                englishStarts[5 - floor] = DescentFloorProfile.StartDistance(floor);
            }

            PrototypeLocalization.Language = GameLanguage.French;
            for (int floor = 5; floor >= 1; floor--)
            {
                Assert.AreEqual(englishStarts[5 - floor], DescentFloorProfile.StartDistance(floor),
                    $"Floor {floor} start distance changed with language.");
            }

            // Sanity: known descent tuning preserved (Floor 5 = 85 ... Floor 1 = 65).
            CollectionAssert.AreEqual(new[] { 85, 80, 75, 70, 65 }, englishStarts);
        }
    }
}
