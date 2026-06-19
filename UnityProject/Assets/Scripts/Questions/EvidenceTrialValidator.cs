using System.Collections.Generic;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Questions
{
    /// <summary>
    /// Validates evidence floors/trials against the Phase 7E rules. Pure logic, no Unity
    /// dependency, fully testable in EditMode. Enforces the central principle from
    /// Docs/CORRIDOR_OBSERVATION_DESIGN.md: no trial without a clue, and no correct answer
    /// without observable evidence.
    ///
    /// The data containers stay permissive; all structural rules live here so they have a
    /// single source of truth and can be reported as a list of typed issues.
    /// </summary>
    public static class EvidenceTrialValidator
    {
        /// <summary>Expected number of answer options per trial (matches the 4-button HUD).</summary>
        public const int RequiredAnswerCount = 4;

        /// <summary>Minimum number of trials a floor must define.</summary>
        public const int MinTrialsPerFloor = 5;

        /// <summary>Validate a single floor's evidence content.</summary>
        public static EvidenceValidationResult Validate(FloorObservationSet floor)
        {
            var result = new EvidenceValidationResult();
            if (floor == null) return result;

            ValidateClues(floor, result);
            ValidateTrials(floor, result);
            return result;
        }

        /// <summary>Validate several floors, aggregating every issue into one result.</summary>
        public static EvidenceValidationResult ValidateAll(IReadOnlyList<FloorObservationSet> floors)
        {
            var result = new EvidenceValidationResult();
            if (floors == null) return result;

            for (int i = 0; i < floors.Count; i++)
            {
                EvidenceValidationResult floorResult = Validate(floors[i]);
                foreach (EvidenceValidationIssue issue in floorResult.Issues)
                {
                    result.Add(issue.Error, issue.Message);
                }
            }
            return result;
        }

        private static void ValidateClues(FloorObservationSet floor, EvidenceValidationResult result)
        {
            var seenIds = new HashSet<string>();
            for (int i = 0; i < floor.Clues.Count; i++)
            {
                CorridorClue clue = floor.Clues[i];
                if (clue == null) continue;

                if (string.IsNullOrWhiteSpace(clue.Id))
                {
                    result.Add(EvidenceValidationError.EmptyClueId,
                        $"Floor {floor.FloorDisplayNumber}: a clue has an empty id.");
                }
                else if (!seenIds.Add(clue.Id))
                {
                    result.Add(EvidenceValidationError.DuplicateClueId,
                        $"Floor {floor.FloorDisplayNumber}: duplicate clue id '{clue.Id}'.");
                }

                if (string.IsNullOrWhiteSpace(clue.EvidenceValue))
                {
                    result.Add(EvidenceValidationError.ClueEmptyEvidenceValue,
                        $"Clue '{clue.Id}' has an empty evidence value.");
                }
            }
        }

        private static void ValidateTrials(FloorObservationSet floor, EvidenceValidationResult result)
        {
            if (floor.Trials.Count < MinTrialsPerFloor)
            {
                result.Add(EvidenceValidationError.FloorFewerThanFiveTrials,
                    $"Floor {floor.FloorDisplayNumber} has {floor.Trials.Count} trials (needs >= {MinTrialsPerFloor}).");
            }

            var seenIds = new HashSet<string>();
            for (int i = 0; i < floor.Trials.Count; i++)
            {
                EvidenceTrial trial = floor.Trials[i];
                if (trial == null) continue;

                if (string.IsNullOrWhiteSpace(trial.Id))
                {
                    result.Add(EvidenceValidationError.EmptyTrialId,
                        $"Floor {floor.FloorDisplayNumber}: a trial has an empty id.");
                }
                else if (!seenIds.Add(trial.Id))
                {
                    result.Add(EvidenceValidationError.DuplicateTrialId,
                        $"Floor {floor.FloorDisplayNumber}: duplicate trial id '{trial.Id}'.");
                }

                if (string.IsNullOrWhiteSpace(trial.ClueId) || !floor.HasClue(trial.ClueId))
                {
                    result.Add(EvidenceValidationError.TrialReferencesMissingClue,
                        $"Trial '{trial.Id}' references missing clue '{trial.ClueId}'.");
                }

                if (trial.AnswerCount != RequiredAnswerCount)
                {
                    result.Add(EvidenceValidationError.TrialAnswerCountNotFour,
                        $"Trial '{trial.Id}' has {trial.AnswerCount} answers (needs {RequiredAnswerCount}).");
                }

                if (trial.CorrectAnswerCount != 1)
                {
                    result.Add(EvidenceValidationError.TrialNotExactlyOneCorrectAnswer,
                        $"Trial '{trial.Id}' has {trial.CorrectAnswerCount} correct answers (needs exactly 1).");
                }

                if (trial.TimeLimitSeconds <= 0f)
                {
                    result.Add(EvidenceValidationError.TrialInvalidTimeLimit,
                        $"Trial '{trial.Id}' has an invalid time limit ({trial.TimeLimitSeconds}).");
                }

                if (trial.Difficulty < 1)
                {
                    result.Add(EvidenceValidationError.TrialInvalidDifficulty,
                        $"Trial '{trial.Id}' has an invalid difficulty ({trial.Difficulty}).");
                }

                if (IsEnglishMissing(trial.Prompt))
                {
                    result.Add(EvidenceValidationError.LocalizedPromptMissingEnglish,
                        $"Trial '{trial.Id}' prompt is missing English text.");
                }

                ValidateAnswerLocalization(trial, result);
            }
        }

        private static void ValidateAnswerLocalization(EvidenceTrial trial, EvidenceValidationResult result)
        {
            for (int i = 0; i < trial.Answers.Count; i++)
            {
                EvidenceAnswerOption answer = trial.Answers[i];
                if (answer == null) continue;
                if (IsEnglishMissing(answer.Text))
                {
                    result.Add(EvidenceValidationError.LocalizedAnswerMissingEnglish,
                        $"Trial '{trial.Id}' answer '{answer.Id}' is missing English text.");
                }
            }
        }

        /// <summary>A localized entry is "missing English" when null or blank in English.</summary>
        private static bool IsEnglishMissing(LocalizedText text)
        {
            return text == null || string.IsNullOrWhiteSpace(text.English);
        }
    }
}
