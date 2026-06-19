using System.Collections.Generic;
using NUnit.Framework;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.Tests.EditMode
{
    /// <summary>
    /// Phase 7E: the EvidenceTrialValidator enforces the evidence rules
    /// (no trial without a clue, exactly 4 answers, exactly 1 correct, etc.).
    /// Each test starts from a valid floor and mutates one aspect to trigger one error.
    /// </summary>
    public sealed class EvidenceTrialValidatorTests
    {
        private static LocalizedText L(string en, string fr = null) => new LocalizedText(en, fr ?? en);

        private static EvidenceAnswerOption[] FourAnswers(int correctCount = 1)
        {
            var arr = new EvidenceAnswerOption[4];
            for (int i = 0; i < 4; i++)
            {
                arr[i] = new EvidenceAnswerOption($"a{i}", L($"opt{i}"), i < correctCount);
            }
            return arr;
        }

        private static CorridorClue Clue(string id, string evidence = "ev") =>
            new CorridorClue(id, CorridorClueType.DoorNumber, 5, L("LABEL"), L("DESC"), "anchor", evidence);

        private static EvidenceTrial Trial(string id, string clueId, float time = 5f, int difficulty = 1) =>
            new EvidenceTrial(id, clueId, L("prompt"), FourAnswers(), time, difficulty);

        private static FloorObservationSet ValidFloor()
        {
            var clues = new List<CorridorClue>();
            var trials = new List<EvidenceTrial>();
            for (int i = 1; i <= 5; i++)
            {
                clues.Add(Clue($"c{i}"));
                trials.Add(Trial($"t{i}", $"c{i}"));
            }
            return new FloorObservationSet(5, clues, trials);
        }

        [Test]
        public void ValidFloor_PassesValidation()
        {
            EvidenceValidationResult result = EvidenceTrialValidator.Validate(ValidFloor());
            Assert.IsTrue(result.IsValid, string.Join("; ", result.Messages));
        }

        [Test]
        public void Rejects_EmptyClueId()
        {
            var clues = new List<CorridorClue> { Clue("c1"), Clue("c2"), Clue("c3"), Clue("c4"), Clue(" ") };
            var trials = MatchingTrials("c1", "c2", "c3", "c4", "c1");
            var result = EvidenceTrialValidator.Validate(new FloorObservationSet(5, clues, trials));
            Assert.IsTrue(result.HasError(EvidenceValidationError.EmptyClueId));
        }

        [Test]
        public void Rejects_EmptyTrialId()
        {
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 0, Trial(" ", "c1"));
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.EmptyTrialId));
        }

        [Test]
        public void Rejects_DuplicateClueIds()
        {
            var clues = new List<CorridorClue> { Clue("dup"), Clue("dup"), Clue("c3"), Clue("c4"), Clue("c5") };
            var trials = MatchingTrials("dup", "dup", "c3", "c4", "c5");
            var result = EvidenceTrialValidator.Validate(new FloorObservationSet(5, clues, trials));
            Assert.IsTrue(result.HasError(EvidenceValidationError.DuplicateClueId));
        }

        [Test]
        public void Rejects_DuplicateTrialIds()
        {
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 1, Trial("t1", "c2"));
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.DuplicateTrialId));
        }

        [Test]
        public void Rejects_TrialReferencingMissingClue()
        {
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 0, Trial("t1", "does-not-exist"));
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.TrialReferencesMissingClue));
        }

        [Test]
        public void Rejects_TrialWithoutExactlyFourAnswers()
        {
            var threeAnswers = new List<EvidenceAnswerOption>
            {
                new EvidenceAnswerOption("a0", L("a"), true),
                new EvidenceAnswerOption("a1", L("b"), false),
                new EvidenceAnswerOption("a2", L("c"), false),
            };
            var trial = new EvidenceTrial("t1", "c1", L("p"), threeAnswers, 5f, 1);
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 0, trial);
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.TrialAnswerCountNotFour));
        }

        [Test]
        public void Rejects_TrialWithoutExactlyOneCorrectAnswer()
        {
            var noCorrect = new EvidenceTrial("t1", "c1", L("p"), FourAnswers(correctCount: 0), 5f, 1);
            var twoCorrect = new EvidenceTrial("t1", "c1", L("p"), FourAnswers(correctCount: 2), 5f, 1);

            var r0 = EvidenceTrialValidator.Validate(ReplaceTrial(ValidFloor(), 0, noCorrect));
            var r2 = EvidenceTrialValidator.Validate(ReplaceTrial(ValidFloor(), 0, twoCorrect));

            Assert.IsTrue(r0.HasError(EvidenceValidationError.TrialNotExactlyOneCorrectAnswer));
            Assert.IsTrue(r2.HasError(EvidenceValidationError.TrialNotExactlyOneCorrectAnswer));
        }

        [Test]
        public void Rejects_InvalidTimeLimit()
        {
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 0, Trial("t1", "c1", time: 0f));
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.TrialInvalidTimeLimit));
        }

        [Test]
        public void Rejects_InvalidDifficulty()
        {
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 0, Trial("t1", "c1", difficulty: 0));
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.TrialInvalidDifficulty));
        }

        [Test]
        public void Rejects_ClueWithEmptyEvidenceValue()
        {
            var clues = new List<CorridorClue> { Clue("c1", evidence: ""), Clue("c2"), Clue("c3"), Clue("c4"), Clue("c5") };
            var trials = MatchingTrials("c1", "c2", "c3", "c4", "c5");
            var result = EvidenceTrialValidator.Validate(new FloorObservationSet(5, clues, trials));
            Assert.IsTrue(result.HasError(EvidenceValidationError.ClueEmptyEvidenceValue));
        }

        [Test]
        public void Rejects_FloorWithFewerThanFiveTrials()
        {
            var clues = new List<CorridorClue> { Clue("c1"), Clue("c2"), Clue("c3"), Clue("c4") };
            var trials = MatchingTrials("c1", "c2", "c3", "c4");
            var result = EvidenceTrialValidator.Validate(new FloorObservationSet(5, clues, trials));
            Assert.IsTrue(result.HasError(EvidenceValidationError.FloorFewerThanFiveTrials));
        }

        [Test]
        public void Rejects_MissingEnglishPrompt()
        {
            var trial = new EvidenceTrial("t1", "c1", new LocalizedText("", "Question en français"),
                FourAnswers(), 5f, 1);
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 0, trial);
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.LocalizedPromptMissingEnglish));
        }

        [Test]
        public void Rejects_MissingEnglishAnswer()
        {
            var answers = new List<EvidenceAnswerOption>
            {
                new EvidenceAnswerOption("a0", new LocalizedText("", "Réponse"), true),
                new EvidenceAnswerOption("a1", L("b"), false),
                new EvidenceAnswerOption("a2", L("c"), false),
                new EvidenceAnswerOption("a3", L("d"), false),
            };
            var trial = new EvidenceTrial("t1", "c1", L("p"), answers, 5f, 1);
            FloorObservationSet floor = ReplaceTrial(ValidFloor(), 0, trial);
            var result = EvidenceTrialValidator.Validate(floor);
            Assert.IsTrue(result.HasError(EvidenceValidationError.LocalizedAnswerMissingEnglish));
        }

        // ---- helpers ----

        private static List<EvidenceTrial> MatchingTrials(params string[] clueIds)
        {
            var trials = new List<EvidenceTrial>();
            for (int i = 0; i < clueIds.Length; i++)
            {
                trials.Add(Trial($"t{i + 1}", clueIds[i]));
            }
            return trials;
        }

        private static FloorObservationSet ReplaceTrial(FloorObservationSet floor, int index, EvidenceTrial replacement)
        {
            var trials = new List<EvidenceTrial>(floor.Trials);
            trials[index] = replacement;
            return new FloorObservationSet(floor.FloorDisplayNumber, new List<CorridorClue>(floor.Clues), trials);
        }
    }
}
