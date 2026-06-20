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

        [Header("Elevator descent transition (Phase 7I)")]
        [Tooltip("Seconds the FLOOR CLEARED beat holds before the doors start closing.")]
        [SerializeField] private float floorClearedHoldSeconds = 0.8f;

        [Tooltip("Seconds the elevator doors take to close.")]
        [SerializeField] private float doorCloseSeconds = 0.8f;

        [Tooltip("Seconds the DESCENDING beat (with descent cue) holds, doors closed.")]
        [SerializeField] private float descentHoldSeconds = 1.4f;

        [Tooltip("Seconds the elevator doors take to open onto the next floor.")]
        [SerializeField] private float doorOpenSeconds = 0.8f;

        [Header("Observation pass (Phase 7H + 7H.1 tuning)")]
        [Tooltip("Optional. Falls back to Camera.main, then the first Camera found in the scene.")]
        [SerializeField] private Camera observationCamera;

        [Tooltip("Seconds the camera pauses at the deep point of the travel (0..0.5).")]
        [SerializeField] private float observationHoldSeconds = 0.5f;

        [Tooltip("Seconds of the slow travel toward the corridor / red light (~8s).")]
        [SerializeField] private float cameraMoveSeconds = 8.0f;

        [Tooltip("Seconds of the slow travel back to the gameplay pose before the trial (~8s).")]
        [SerializeField] private float cameraReturnSeconds = 8.0f;

        [Tooltip("How far the camera travels toward the corridor / red light during observation (metres).")]
        [SerializeField] private float observationForwardOffset = 7.0f;

        [Tooltip("How much the camera rises during observation (metres). Keep small.")]
        [SerializeField] private float observationHeightOffset = 0.18f;

        private RunController _run;
        private QuestionManager _questions;
        private IReadOnlyList<FloorDefinition> _floors;
        private RunTrialProgress _progress;
        private Coroutine _advanceRoutine;
        private bool _eventsBound;

        // Phase 7H observation pass state.
        private ObservationPassTiming _observationTiming;
        private readonly ObservationPassState _observation = new ObservationPassState();
        private Coroutine _observationRoutine;
        private bool _cameraPoseCaptured;
        private Vector3 _cameraHomePosition;
        private Quaternion _cameraHomeRotation;

        // Phase 7I elevator descent transition state.
        private ElevatorTransitionTiming _transitionTiming;
        private readonly ElevatorTransitionState _transition = new ElevatorTransitionState();

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

            _observationTiming = new ObservationPassTiming(
                observationHoldSeconds, cameraMoveSeconds, cameraReturnSeconds);
            _transitionTiming = new ElevatorTransitionTiming(
                floorClearedHoldSeconds, doorCloseSeconds, descentHoldSeconds, doorOpenSeconds);
        }

        private void Start()
        {
            if (ui != null)
            {
                ui.Build();
                BindUiEvents();
                ui.ShowStartPanel();
            }

            CaptureCameraHomePose();
            UpdateCreature();
            RefreshThreatHud();
        }

        /// <summary>
        /// Resolve the camera and store its gameplay pose once, so the observation pass can ease
        /// toward the corridor and reliably settle back. If no camera is available the pass runs
        /// as an overlay-only fallback (no physical movement).
        /// </summary>
        private void CaptureCameraHomePose()
        {
            if (observationCamera == null)
            {
                observationCamera = Camera.main;
            }
            if (observationCamera == null)
            {
#if UNITY_2023_1_OR_NEWER
                observationCamera = Object.FindFirstObjectByType<Camera>();
#else
                observationCamera = Object.FindObjectOfType<Camera>();
#endif
            }

            if (observationCamera != null)
            {
                Transform t = observationCamera.transform;
                _cameraHomePosition = t.localPosition;
                _cameraHomeRotation = t.localRotation;
                _cameraPoseCaptured = true;
            }
        }

        private void OnDestroy()
        {
            if (_questions != null)
            {
                _questions.AnswerResolved -= HandleAnswerResolved;
            }
            StopObservationRoutine();
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
            StopObservationRoutine();
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
            StopObservationRoutine();
            ResetTransitionVisuals(); // Phase 7I: never start a run mid-transition (doors/creature)
            _run.StartRun();
            _progress.Reset();
            if (ui != null) ui.ShowGameplay();
            BeginFloor(); // top floor threat reset, creature + HUD refresh
            BeginObservationThenTrial(); // Phase 7H: observe before the first trial
        }

        /// <summary>
        /// Start a floor as a fresh danger cycle (Phase 7B.4 descent): reset the threat to
        /// this floor's configured starting distance (stress cleared), then refresh the
        /// creature and HUD. The deeper the descent, the lower the starting distance.
        /// </summary>
        private void BeginFloor()
        {
            int displayFloor = DescentFloorProfile.DisplayFloorNumber(_progress.CurrentFloorIndex, _progress.FloorCount);
            _run.ResetThreatForFloor(DescentFloorProfile.StartDistance(displayFloor));
            UpdateCreature();
            RefreshThreatHud();

            // Phase 7I: the clue board is no longer revealed here — it is revealed at the start
            // of the observation pass (see ObserveThenStartTrial) so it stays hidden during the
            // elevator descent transition and during questions (observation-only clue board).
        }

        private void StartCurrentTrial()
        {
            FloorTrial trial = CurrentTrial();
            if (trial == null) return;

            QuestionData question = trial.Question;
            if (ui != null)
            {
                int displayFloor = DescentFloorProfile.DisplayFloorNumber(_progress.CurrentFloorIndex, _progress.FloorCount);
                ui.UpdateProgress(displayFloor, _progress.CurrentTrialNumber, _progress.TrialsInCurrentFloor);
                // Phase 7H.1: clues are observation-only. As soon as a question starts the clue
                // board is hidden so the player answers from memory (ObservationPassState.CluesVisible).
                ui.HideClues();
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

            ApplyOutcome(outcome); // wrong/timeout advance threat (+ death); correct never recedes

            UpdateCreature();
            RefreshThreatHud();
            if (ui != null)
            {
                ui.ShowOutcomeStatus(outcome);
                ui.SetAnswersInteractable(false);
            }

            // Every trial result consumes the current trial. Surviving all 5 trials clears
            // the floor (no score needed); the last floor's clear reaches the ground floor.
            // Death (distance <= 0) overrides everything.
            TrialResolution resolution = TrialFlowResolver.Resolve(
                _run.HasLost, _progress.IsFinalTrialInFloor, _progress.IsFinalFloor);
            float hold = InterQuestionPacing.GetHoldSeconds(outcome, statusHoldSeconds, dangerHoldExtraSeconds);

            switch (resolution)
            {
                case TrialResolution.Lost:
                    ShowResult(won: false);
                    return;

                case TrialResolution.Escaped:
                    _run.CompleteFloor(); // final floor (Floor 1) cleared -> reach ground floor
                    ShowResult(won: true);
                    return;

                case TrialResolution.NextTrialSameFloor:
                    // Floor not finished: move to the next trial of the SAME floor after the
                    // hold (no transition, no floor advance). Not a retry of the same question.
                    _progress.AdvanceTrial();
                    _advanceRoutine = StartCoroutine(NextTrialAfterDelay(hold));
                    return;

                case TrialResolution.FloorCleared:
                    // Survived all trials of a non-final floor: advance both the run (floor
                    // stats) and the trial progress, then play the descent transition.
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
                // Correct answers never push the creature back during a floor (non-receding
                // threat); they only consume the trial.
                case AnswerOutcome.CorrectFast:
                case AnswerOutcome.CorrectNormal:
                case AnswerOutcome.CorrectSlow:
                    _run.RecordCorrectSealed();
                    break;
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
        /// After a NON-final floor is fully cleared (Phase 7I): hold on the answer outcome
        /// (Phase 7 pacing), then play the prototype elevator descent transition — FLOOR CLEARED,
        /// doors close, DESCENDING (with a subtle descent cue while the floor indicator updates),
        /// doors open — and only then start the next lower floor's observation pass. Progress
        /// already points at the next floor. During the whole transition the creature is hidden,
        /// the clue board stays hidden, and no threat/timer/answer activity happens. The final
        /// Floor 1 escape never reaches here (it shows the result instead), so no transition runs
        /// after the escape.
        /// </summary>
        private IEnumerator ClearFloorThenAdvance(float outcomeHold)
        {
            yield return new WaitForSeconds(outcomeHold);
            if (!_run.IsRunning) { _advanceRoutine = null; yield break; }

            _transition.Begin(); // DoorsClosing

            // Hide the trial HUD and the clue board; keep the creature hidden for the whole transition.
            if (ui != null)
            {
                ui.HideTrialHudForTransition();
                ui.HideClues();
            }
            if (creature != null) creature.SetObservationHidden(true);

            // FLOOR CLEARED beat, doors still open.
            if (ui != null)
            {
                ui.ShowElevatorDoors(true);
                ui.SetElevatorDoorProgress(0f); // open
                ui.SetDescentMessage(PrototypeLocalization.Current(PrototypeLocalization.FloorCleared), string.Empty);
            }
            yield return new WaitForSeconds(_transitionTiming.FloorClearedHoldSeconds);

            // DOORS CLOSING: animate the doors from open (0) to closed (1).
            if (ui != null) ui.SetDescentMessage(PrototypeLocalization.Current(PrototypeLocalization.DoorsClosing), string.Empty);
            yield return AnimateDoors(0f, 1f, _transitionTiming.DoorCloseSeconds);

            // New floor = fresh danger cycle: reset threat to this lower floor's start distance
            // (creature stays hidden via the mask, clue board not revealed here). Update the floor
            // indicator while the doors are closed.
            _transition.EnterDescending();
            BeginFloor();
            int displayFloor = DescentFloorProfile.DisplayFloorNumber(_progress.CurrentFloorIndex, _progress.FloorCount);
            if (ui != null)
            {
                ui.UpdateProgress(displayFloor, _progress.CurrentTrialNumber, _progress.TrialsInCurrentFloor);
                ui.SetDescentMessage(PrototypeLocalization.Current(PrototypeLocalization.Descending),
                    PrototypeLocalization.FloorLabel(displayFloor));
                ui.PlayDescentCue(_transitionTiming.DescentHoldSeconds); // subtle vertical cue, doors closed
            }
            yield return new WaitForSeconds(_transitionTiming.DescentHoldSeconds);

            // DOORS OPENING: animate the doors from closed (1) back to open (0).
            _transition.BeginOpening();
            if (ui != null) ui.SetDescentMessage(PrototypeLocalization.Current(PrototypeLocalization.DoorsOpening),
                PrototypeLocalization.FloorLabel(displayFloor));
            yield return AnimateDoors(1f, 0f, _transitionTiming.DoorOpenSeconds);

            if (ui != null) ui.HideElevatorDoors();
            _transition.Complete();

            _advanceRoutine = null;
            if (_run.IsRunning)
            {
                // Observation starts ONLY after the doors have opened.
                BeginObservationThenTrial();
            }
        }

        /// <summary>Animate the elevator doors between two progress values (0 open, 1 closed).</summary>
        private IEnumerator AnimateDoors(float from, float to, float seconds)
        {
            if (ui == null || seconds <= 0f)
            {
                if (ui != null) ui.SetElevatorDoorProgress(to);
                if (seconds > 0f) yield return new WaitForSeconds(seconds);
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < seconds)
            {
                elapsed += Time.deltaTime;
                float k = Mathf.Clamp01(elapsed / seconds);
                k = k * k * (3f - 2f * k); // smoothstep ease
                ui.SetElevatorDoorProgress(Mathf.Lerp(from, to, k));
                yield return null;
            }
            ui.SetElevatorDoorProgress(to);
        }

        private void StopAdvanceRoutine()
        {
            if (_advanceRoutine != null)
            {
                StopCoroutine(_advanceRoutine);
                _advanceRoutine = null;
            }
        }

        // ---- Observation pass (Phase 7H) -----------------------------------

        /// <summary>
        /// Run the observation pass for the current floor, then start its first trial. Called
        /// once per floor (run start and after each descent), after the clue board is updated.
        /// During the pass no question is active, so the timer, threat and trial count cannot
        /// advance and answers stay hidden. A duplicate pass cannot start while one is running.
        /// </summary>
        private void BeginObservationThenTrial()
        {
            StopObservationRoutine();
            _observationRoutine = StartCoroutine(ObserveThenStartTrial());
        }

        private IEnumerator ObserveThenStartTrial()
        {
            _observation.Begin();
            if (ui != null)
            {
                // Phase 7I: reveal this floor's clue board now (observation-only). It was kept
                // hidden during the elevator descent transition; StartCurrentTrial hides it again.
                int displayFloor = DescentFloorProfile.DisplayFloorNumber(_progress.CurrentFloorIndex, _progress.FloorCount);
                ui.UpdateClues(displayFloor);
                ui.PrepareObservation();   // hide question/answers/cue/status, keep clue board
                ui.ShowObservationHint();  // localized OBSERVE THE CORRIDOR overlay
            }
            // Phase 7H.1 correction: the creature is never visible during the observation travel.
            if (creature != null) creature.SetObservationHidden(true);

            Vector3 home = _cameraHomePosition;
            Vector3 observe = ObservePosition();

            // Slow camera travel toward the corridor / red light, brief hold, then travel back.
            // With no camera the moves still wait so the overlay-only fallback keeps the pacing.
            yield return MoveCameraTo(home, observe, _observationTiming.CameraMoveSeconds);
            yield return new WaitForSeconds(_observationTiming.ObservationHoldSeconds);
            yield return MoveCameraTo(observe, home, _observationTiming.CameraReturnSeconds);

            if (ui != null) ui.HideObservationHint();
            // Restore normal phase-based creature visibility so she can appear during the answer
            // phase according to the current threat state (not during the travel).
            if (creature != null) creature.SetObservationHidden(false);
            _observation.Complete();
            _observationRoutine = null;

            if (_run.IsRunning)
            {
                StartCurrentTrial(); // resumes normal trial flow (question/answers/timer)
            }
        }

        /// <summary>The "look into the corridor" pose: travel forward toward the red light, raised.</summary>
        private Vector3 ObservePosition()
        {
            if (!_cameraPoseCaptured) return _cameraHomePosition;
            Vector3 forward = _cameraHomeRotation * Vector3.forward;
            return _cameraHomePosition
                   + forward * observationForwardOffset
                   + Vector3.up * observationHeightOffset;
        }

        private IEnumerator MoveCameraTo(Vector3 from, Vector3 to, float seconds)
        {
            // No camera (overlay-only fallback): just consume the time so pacing is unchanged.
            if (observationCamera == null || !_cameraPoseCaptured)
            {
                if (seconds > 0f) yield return new WaitForSeconds(seconds);
                yield break;
            }

            Transform t = observationCamera.transform;
            if (seconds <= 0f)
            {
                t.localPosition = to;
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < seconds)
            {
                elapsed += Time.deltaTime;
                float k = Mathf.Clamp01(elapsed / seconds);
                k = k * k * (3f - 2f * k); // smoothstep ease
                t.localPosition = Vector3.Lerp(from, to, k);
                yield return null;
            }
            t.localPosition = to;
        }

        /// <summary>
        /// Stop any running observation pass and restore the camera/overlay if it was
        /// interrupted mid-pass (e.g. restart). Safe to call when no pass is running.
        /// </summary>
        private void StopObservationRoutine()
        {
            if (_observationRoutine != null)
            {
                StopCoroutine(_observationRoutine);
                _observationRoutine = null;
            }

            if (_observation.IsObserving)
            {
                if (_cameraPoseCaptured && observationCamera != null)
                {
                    Transform t = observationCamera.transform;
                    t.localPosition = _cameraHomePosition;
                    t.localRotation = _cameraHomeRotation;
                }
                if (ui != null) ui.HideObservationHint();
                // Restore creature visibility so an interrupted pass never leaves her stuck hidden.
                if (creature != null) creature.SetObservationHidden(false);
                _observation.Reset();
            }
        }

        /// <summary>
        /// Phase 7I: clear any in-progress elevator transition visuals (doors overlay + state)
        /// and restore creature visibility, so a restart/new run never starts mid-transition.
        /// Safe to call when no transition is running.
        /// </summary>
        private void ResetTransitionVisuals()
        {
            if (_transition.IsActive && creature != null) creature.SetObservationHidden(false);
            _transition.Reset();
            if (ui != null) ui.HideElevatorDoors();
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
            StopObservationRoutine();
            ResetTransitionVisuals(); // Phase 7I: clear any door overlay before the result screen
            if (ui == null) return;

            var result = _run.BuildResult();
            string detail =
                $"Floors cleared: {result.FloorsCompleted}/{_run.TotalFloors}\n" +
                $"Correct: {result.CorrectAnswers}   Wrong: {result.WrongAnswers}   Timeouts: {result.Timeouts}\n" +
                $"Distance: {result.FinalDistance}";
            ui.ShowResult(won, detail);
        }
    }
}
