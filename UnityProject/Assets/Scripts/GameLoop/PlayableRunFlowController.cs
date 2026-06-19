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

        private RunController _run;
        private QuestionManager _questions;
        private IReadOnlyList<QuestionData> _questionSet;
        private IReadOnlyDictionary<string, QuestionCue> _cues;
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

            _questionSet = PrototypeQuestionSet.BuildAll();
            _cues = PrototypeQuestionCueSet.BuildById();
            _run = new RunController(_questionSet.Count);
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
            if (ui != null) ui.ShowGameplay();
            UpdateCreature();
            RefreshThreatHud();
            StartCurrentQuestion();
        }

        private void StartCurrentQuestion()
        {
            int index = _run.CurrentFloor - 1;
            if (index < 0 || index >= _questionSet.Count) return;

            QuestionData question = _questionSet[index];
            if (ui != null)
            {
                ui.UpdateFloor(_run.CurrentFloor, _run.TotalFloors);
                ui.ShowQuestion(question);
                ShowCueForQuestion(question);
                ui.SetAnswersInteractable(true);
                ui.UpdateTimer(question.TimeLimitSeconds, question.TimeLimitSeconds);
            }
            _questions.StartQuestion(question);
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

            if (_run.HasLost)
            {
                ShowResult(won: false);
                return;
            }

            _run.CompleteFloor(); // one question per floor in the prototype
            if (_run.HasWon)
            {
                ShowResult(won: true);
                return;
            }

            float hold = InterQuestionPacing.GetHoldSeconds(outcome, statusHoldSeconds, dangerHoldExtraSeconds);
            _advanceRoutine = StartCoroutine(NextQuestionAfterDelay(hold));
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

        private IEnumerator NextQuestionAfterDelay(float holdSeconds)
        {
            yield return new WaitForSeconds(holdSeconds);
            _advanceRoutine = null;
            if (_run.IsRunning)
            {
                StartCurrentQuestion();
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

        private void ShowCueForQuestion(QuestionData question)
        {
            if (ui == null) return;
            if (_cues != null && question != null && _cues.TryGetValue(question.Id, out QuestionCue cue))
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
