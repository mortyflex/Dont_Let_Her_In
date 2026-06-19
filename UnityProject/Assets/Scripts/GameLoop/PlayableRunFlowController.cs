using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DontLetHerIn.Creature;
using DontLetHerIn.Questions;
using DontLetHerIn.UI;

namespace DontLetHerIn.GameLoop
{
    /// <summary>
    /// Phase 5 orchestrator: wires the pure systems (RunController, QuestionManager,
    /// ThreatManager) to the scene view (GameplayUIController) and the distance-driven
    /// CreatureController, producing the first playable run loop.
    ///
    /// It owns the run/question logic but delegates all rules to the existing systems:
    /// answer evaluation to QuestionManager/QuestionEvaluator, distance/stress to
    /// ThreatManager (through RunController), and visual phase to CreatureController.
    /// No game rules are duplicated here.
    /// </summary>
    public sealed class PlayableRunFlowController : MonoBehaviour
    {
        [Tooltip("Optional. Falls back to a GameplayUIController on this GameObject.")]
        [SerializeField] private GameplayUIController ui;

        [Tooltip("Optional. Falls back to the first CreatureController found in the scene.")]
        [SerializeField] private CreatureController creature;

        [Tooltip("Seconds the result feedback is shown after a correct answer before the next question starts.")]
        [SerializeField] private float statusHoldSeconds = 1.2f;

        [Tooltip("Extra seconds added after a wrong answer or timeout so the danger has time to register.")]
        [SerializeField] private float dangerHoldExtraSeconds = 0.3f;

        [Header("Inter-floor transition (Phase 7B)")]
        [Tooltip("Seconds the FLOOR CLEARED message holds when a non-final floor is cleared.")]
        [SerializeField] private float floorClearedHoldSeconds = 0.7f;

        [Tooltip("Seconds the DOORS CLOSING message holds during the inter-floor transition.")]
        [SerializeField] private float doorsClosingHoldSeconds = 0.7f;

        [Tooltip("Seconds the ASCENDING message holds before the next floor starts.")]
        [SerializeField] private float ascendingHoldSeconds = 0.8f;

        private RunController _run;
        private QuestionManager _questions;
        private IReadOnlyList<FloorDefinition> _floors;
        private RunTrialProgress _progress;
        private Coroutine _advanceRoutine;
        private bool _eventsBound;

        private void Awake()
        {
            if (ui == null)
            {
                ui = GetComponent<GameplayUIController>();
            }

            if (creature == null)
            {
#if UNITY_2023_1_OR_NEWER
                creature = Object.FindFirstObjectByType<CreatureController>();
#else
                creature = Object.FindObjectOfType<CreatureController>();
#endif
            }

            _floors = PrototypeFloorSet.BuildAll();
            _progress = new RunTrialProgress(PrototypeFloorSet.TrialCounts());
            _run = new RunController(_floors.Count);
            _questions = new QuestionManager();
            _questions.AnswerResolved += HandleAnswerResolved;
        }

        private void Start()
        {
            if (ui != null)
            {
                ui.Build();
                BindUiEvents();
                ui.ShowStartPanel();
            }

            UpdateCreature();
            RefreshThreatHud();
        }

        private void OnDestroy()
        {
            if (_questions != null)
            {
                _questions.AnswerResolved -= HandleAnswerResolved;
            }
            UnbindUiEvents();
        }

        private void Update()
        {
            if (_run == null || !_run.IsRunning) return;
            if (!_questions.IsQuestionActive) return;

            QuestionData active = _questions.ActiveQuestion;
            _questions.Tick(Time.deltaTime); // may auto-resolve a timeout

            if (_questions.IsQuestionActive && ui != null)
            {
                ui.UpdateTimer(_questions.RemainingTime, active.TimeLimitSeconds);
            }
        }

        // ---- UI event wiring ----------------------------------------------

        private void BindUiEvents()
        {
            if (_eventsBound || ui == null) return;
            ui.StartClicked += HandleStartClicked;
            ui.RestartClicked += HandleRestartClicked;
            ui.AnswerSelected += HandleAnswerSelected;
            _eventsBound = true;
        }

        private void UnbindUiEvents()
        {
            if (!_eventsBound || ui == null) return;
            ui.StartClicked -= HandleStartClicked;
            ui.RestartClicked -= HandleRestartClicked;
            ui.AnswerSelected -= HandleAnswerSelected;
            _eventsBound = false;
        }

        private void HandleStartClicked() => BeginRun();

        private void HandleRestartClicked()
        {
            StopAdvanceRoutine();
            _questions.Reset();
            BeginRun();
        }

        private void HandleAnswerSelected(int index)
        {
            if (!_questions.IsQuestionActive) return;
            if (ui != null) ui.SetAnswersInteractable(false);
            _questions.SubmitAnswer(index); // fires HandleAnswerResolved
        }

        // ---- Run flow ------------------------------------------------------

        private void BeginRun()
        {
            StopAdvanceRoutine();
            _run.StartRun();
            _progress.Reset();
            if (ui != null) ui.ShowGameplay();
            UpdateCreature();
            RefreshThreatHud();
            StartCurrentTrial();
        }

        private void StartCurrentTrial()
        {
            FloorTrial trial = CurrentTrial();
            if (trial == null) return;

            QuestionData question = trial.Question;
            if (ui != null)
            {
                ui.UpdateProgress(_progress.CurrentFloorNumber, _progress.FloorCount,
                    _progress.CurrentTrialNumber, _progress.TrialsInCurrentFloor);
                ui.ShowQuestion(question);
                ShowCue(trial.Cue);
                ui.SetAnswersInteractable(true);
                ui.UpdateTimer(question.TimeLimitSeconds, question.TimeLimitSeconds);
            }
            _questions.StartQuestion(question);
        }

        /// <summary>The trial the player is currently on, or null if indices are out of range.</summary>
        private FloorTrial CurrentTrial()
        {
            int f = _progress.CurrentFloorIndex;
            if (_floors == null || f < 0 || f >= _floors.Count) return null;

            IReadOnlyList<FloorTrial> trials = _floors[f].Trials;
            int t = _progress.CurrentTrialIndex;
            if (t < 0 || t >= trials.Count) return null;

            return trials[t];
        }

        private void HandleAnswerResolved(AnswerResult result)
        {
            AnswerOutcome outcome = AnswerOutcomeResolver.Resolve(result);
            ApplyOutcome(outcome); // updates threat via RunController; may mark loss

            UpdateCreature();
            RefreshThreatHud();
            if (ui != null)
            {
                ui.ShowOutcomeStatus(outcome);
                ui.SetAnswersInteractable(false);
            }

            // Every trial result (correct/wrong/timeout) consumes the current trial
            // (Phase 7B.2). Death overrides everything; otherwise we either advance to the
            // next trial of the same floor, clear the floor, or escape on the final floor.
            TrialResolution resolution = TrialFlowResolver.Resolve(
                _run.HasLost, _progress.IsFinalTrialInFloor, _progress.IsFinalFloor);
            float hold = InterQuestionPacing.GetHoldSeconds(outcome, statusHoldSeconds, dangerHoldExtraSeconds);

            switch (resolution)
            {
                case TrialResolution.Lost:
                    ShowResult(won: false);
                    return;

                case TrialResolution.Escaped:
                    _run.CompleteFloor(); // final floor fully cleared -> marks the run won
                    ShowResult(won: true);
                    return;

                case TrialResolution.NextTrialSameFloor:
                    // Floor not finished: move to the next trial of the SAME floor after the
                    // hold (no transition, no floor advance). Not a retry of the same question.
                    _progress.AdvanceTrial();
                    _advanceRoutine = StartCoroutine(NextTrialAfterDelay(hold));
                    return;

                case TrialResolution.FloorCleared:
                    // Last trial of a non-final floor survived: advance both the run (floor
                    // stats/threat owner) and the trial progress, then play the transition.
                    _run.CompleteFloor();
                    _progress.AdvanceFloor();
                    _advanceRoutine = StartCoroutine(ClearFloorThenAdvance(hold));
                    return;
            }
        }

        private void ApplyOutcome(AnswerOutcome outcome)
        {
            switch (outcome)
            {
                case AnswerOutcome.CorrectFast: _run.RecordCorrectFast(); break;
                case AnswerOutcome.CorrectNormal: _run.RecordCorrectNormal(); break;
                case AnswerOutcome.CorrectSlow: _run.RecordCorrectSlow(); break;
                case AnswerOutcome.Wrong: _run.RecordWrongAnswer(); break;
                case AnswerOutcome.Timeout: _run.RecordTimeout(); break;
            }
        }

        /// <summary>
        /// Trial consumed but the floor is not finished: after the feedback hold, start the
        /// NEXT trial of the same floor (progress was already advanced). Cue and timer reset
        /// via <see cref="StartCurrentTrial"/>. No floor transition is shown.
        /// </summary>
        private IEnumerator NextTrialAfterDelay(float holdSeconds)
        {
            yield return new WaitForSeconds(holdSeconds);
            _advanceRoutine = null;
            if (_run.IsRunning)
            {
                StartCurrentTrial();
            }
        }

        /// <summary>
        /// After a non-final floor is fully cleared: hold on the answer outcome (Phase 7
        /// pacing), then play a short UI-only elevator transition (FLOOR CLEARED -> DOORS
        /// CLOSING -> ASCENDING) framing temporary safety, then start the next floor's first
        /// trial. Progress already points at the next floor. Danger is paused during the
        /// transition: no threat/creature change is applied here.
        /// </summary>
        private IEnumerator ClearFloorThenAdvance(float outcomeHold)
        {
            yield return new WaitForSeconds(outcomeHold);
            if (!_run.IsRunning) { _advanceRoutine = null; yield break; }

            if (ui != null)
            {
                ui.BeginFloorTransition();
                ui.ShowFloorTransition(FloorTransitionText.ClearedTitle, FloorTransitionText.GetClearedSubtitle());
            }
            yield return new WaitForSeconds(floorClearedHoldSeconds);

            if (ui != null)
            {
                ui.ShowFloorTransition(FloorTransitionText.DoorsClosingTitle, FloorTransitionText.GetDoorsClosingSubtitle());
            }
            yield return new WaitForSeconds(doorsClosingHoldSeconds);

            if (ui != null)
            {
                // Reveal the floor we are climbing to (and its first trial) during the ascent.
                ui.UpdateProgress(_progress.CurrentFloorNumber, _progress.FloorCount,
                    _progress.CurrentTrialNumber, _progress.TrialsInCurrentFloor);
                ui.ShowFloorTransition(FloorTransitionText.AscendingTitle,
                    FloorTransitionText.GetAscendingSubtitle(_progress.CurrentFloorNumber, _progress.FloorCount));
            }
            yield return new WaitForSeconds(ascendingHoldSeconds);

            if (ui != null) ui.HideFloorTransition();

            _advanceRoutine = null;
            if (_run.IsRunning)
            {
                StartCurrentTrial();
            }
        }

        private void StopAdvanceRoutine()
        {
            if (_advanceRoutine != null)
            {
                StopCoroutine(_advanceRoutine);
                _advanceRoutine = null;
            }
        }

        // ---- View helpers --------------------------------------------------

        private void ShowCue(QuestionCue cue)
        {
            if (ui == null) return;
            if (cue != null)
            {
                ui.ShowCue(cue);
            }
            else
            {
                ui.HideCue();
            }
        }

        private void UpdateCreature()
        {
            if (creature != null)
            {
                creature.ApplyDistance(_run.Threat.Distance);
            }
        }

        private void RefreshThreatHud()
        {
            if (ui == null) return;
            int distance = _run.Threat.Distance;
            CreaturePhase phase = creature != null
                ? creature.CurrentPhase
                : CreatureDistanceMapper.GetPhase(distance);
            ui.UpdateThreat(distance, _run.Threat.StressLevel, phase);
            ui.UpdateProximity(distance);
        }

        private void ShowResult(bool won)
        {
            StopAdvanceRoutine();
            if (ui == null) return;

            var result = _run.BuildResult();
            string detail =
                $"Floors cleared: {result.FloorsCompleted}/{_run.TotalFloors}\n" +
                $"Correct: {result.CorrectAnswers}   Wrong: {result.WrongAnswers}   Timeouts: {result.Timeouts}\n" +
                $"Final distance: {result.FinalDistance}";
            ui.ShowResult(won, detail);
        }
    }
}
