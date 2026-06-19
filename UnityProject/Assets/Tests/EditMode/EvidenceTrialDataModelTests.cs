using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7E: the pure evidence data containers store what they are given and expose the
    /// expected helpers. Validation rules are covered by <see cref="EvidenceTrialValidatorTests"/>.
    /// </summary>
    public sealed class EvidenceTrialDataModelTests
    {
        private static LocalizedText L(string en, string fr) => new LocalizedText(en, fr);

        [Test]
        public void CorridorClue_StoresIdTypeFloorAndEvidenceValue()
        {
            var clue = new CorridorClue(
                "f5-clue-room", CorridorClueType.DoorNumber, 5,
                L("ROOM DISPLAY", "NUMÉRO DE PORTE"),
                L("Room 104 blinks.", "La chambre 104 clignote."),
                "Corridor.Door1Plate", "104", difficultyWeight: 2, isRequiredForTrial: true);

            Assert.AreEqual("f5-clue-room", clue.Id);
            Assert.AreEqual(CorridorClueType.DoorNumber, clue.Type);
            Assert.AreEqual(5, clue.FloorDisplayNumber);
            Assert.AreEqual("104", clue.EvidenceValue);
            Assert.AreEqual("Corridor.Door1Plate", clue.VisualAnchor);
            Assert.AreEqual(2, clue.DifficultyWeight);
            Assert.IsTrue(clue.IsRequiredForTrial);
            Assert.AreEqual("ROOM DISPLAY", clue.Label.Get(GameLanguage.English));
            Assert.AreEqual("NUMÉRO DE PORTE", clue.Label.Get(GameLanguage.French));
        }

        [Test]
        public void EvidenceAnswerOption_StoresIdTextAndCorrectFlag()
        {
            var option = new EvidenceAnswerOption("a1", L("Up", "Haut"), true);

            Assert.AreEqual("a1", option.Id);
            Assert.IsTrue(option.IsCorrect);
            Assert.AreEqual("Up", option.Text.Get(GameLanguage.English));
            Assert.AreEqual("Haut", option.Text.Get(GameLanguage.French));
        }

        [Test]
        public void EvidenceTrial_StoresClueIdPromptAnswersTimeLimit()
        {
            var answers = new List<EvidenceAnswerOption>
            {
                new EvidenceAnswerOption("a1", L("104", "104"), true),
                new EvidenceAnswerOption("a2", L("101", "101"), false),
                new EvidenceAnswerOption("a3", L("140", "140"), false),
                new EvidenceAnswerOption("a4", L("401", "401"), false),
            };
            var trial = new EvidenceTrial("t1", "f5-clue-room",
                L("Which room number blinked?", "Quel numéro de chambre clignotait ?"),
                answers, 8f, 1);

            Assert.AreEqual("t1", trial.Id);
            Assert.AreEqual("f5-clue-room", trial.ClueId);
            Assert.AreEqual("Which room number blinked?", trial.Prompt.Get(GameLanguage.English));
            Assert.AreEqual(4, trial.AnswerCount);
            Assert.AreEqual(8f, trial.TimeLimitSeconds);
            Assert.AreEqual(1, trial.Difficulty);
        }

        [Test]
        public void EvidenceTrial_CorrectAnswerCountAndCorrectAnswer_ReflectFlags()
        {
            var answers = new List<EvidenceAnswerOption>
            {
                new EvidenceAnswerOption("a1", L("Up", "Haut"), false),
                new EvidenceAnswerOption("a2", L("Down", "Bas"), true),
                new EvidenceAnswerOption("a3", L("Left", "Gauche"), false),
                new EvidenceAnswerOption("a4", L("Right", "Droite"), false),
            };
            var trial = new EvidenceTrial("t1", "clue", L("p", "p"), answers, 5f, 1);

            Assert.AreEqual(1, trial.CorrectAnswerCount);
            Assert.IsNotNull(trial.CorrectAnswer);
            Assert.AreEqual("a2", trial.CorrectAnswer.Id);
        }

        [Test]
        public void EvidenceTrial_WithTwoCorrect_HasNoSingleCorrectAnswer()
        {
            var answers = new List<EvidenceAnswerOption>
            {
                new EvidenceAnswerOption("a1", L("A", "A"), true),
                new EvidenceAnswerOption("a2", L("B", "B"), true),
            };
            var trial = new EvidenceTrial("t1", "clue", L("p", "p"), answers, 5f, 1);

            Assert.AreEqual(2, trial.CorrectAnswerCount);
            Assert.IsNull(trial.CorrectAnswer);
        }

        [Test]
        public void FloorObservationSet_StoresFloorAndExposesClueLookup()
        {
            var clues = new List<CorridorClue>
            {
                new CorridorClue("c1", CorridorClueType.DoorNumber, 5, L("L", "L"), L("D", "D"), "anchor", "104"),
            };
            var trials = new List<EvidenceTrial>();
            var floor = new FloorObservationSet(5, clues, trials);

            Assert.AreEqual(5, floor.FloorDisplayNumber);
            Assert.AreEqual(1, floor.ClueCount);
            Assert.AreEqual(0, floor.TrialCount);
            Assert.IsTrue(floor.HasClue("c1"));
            Assert.IsFalse(floor.HasClue("missing"));
            Assert.AreEqual("104", floor.FindClue("c1").EvidenceValue);
            Assert.IsNull(floor.FindClue("missing"));
        }

        [Test]
        public void NullCollections_DefaultToEmpty()
        {
            var trial = new EvidenceTrial("t", "c", L("p", "p"), null, 5f, 1);
            Assert.AreEqual(0, trial.AnswerCount);

            var floor = new FloorObservationSet(1, null, null);
            Assert.AreEqual(0, floor.ClueCount);
            Assert.AreEqual(0, floor.TrialCount);
        }
    }
}
