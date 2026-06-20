using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using DontLetHerIn.Creature;
using DontLetHerIn.GameLoop;
using DontLetHerIn.Questions;

namespace DontLetHerIn.UI
{
    /// <summary>
    /// Builds and drives a minimal mobile-portrait gameplay UI at runtime (legacy uGUI).
    /// This is a deliberately simple, code-built prototype HUD — not final UI design.
    /// Building it in code (instead of hand-authored Canvas YAML) keeps the scene robust
    /// and lets every element be created deterministically without the Unity Editor.
    ///
    /// Responsibilities: own the view references, raise input events and display state.
    /// All game logic lives in <see cref="PlayableRunFlowController"/> and the pure systems.
    /// </summary>
    public sealed class GameplayUIController : MonoBehaviour
    {
        public const int AnswerButtonCount = 4;

        // Portrait reference resolution (iPhone-like tall canvas).
        private static readonly Vector2 ReferenceResolution = new Vector2(1080f, 1920f);

        private static readonly Color PanelColor = new Color(0f, 0f, 0f, 0.72f);
        private static readonly Color DimColor = new Color(0f, 0f, 0f, 0.88f);
        private static readonly Color ButtonColor = new Color(0.16f, 0.17f, 0.20f, 0.95f);
        private static readonly Color ButtonDisabledColor = new Color(0.10f, 0.10f, 0.12f, 0.6f);
        private static readonly Color AccentColor = new Color(0.65f, 0.12f, 0.14f, 1f);
        private static readonly Color TextColor = new Color(0.92f, 0.92f, 0.9f, 1f);
        private static readonly Color GoodColor = new Color(0.45f, 0.85f, 0.4f, 1f);
        private static readonly Color BadColor = new Color(0.92f, 0.32f, 0.3f, 1f);
        private static readonly Color WarnColor = new Color(0.95f, 0.72f, 0.25f, 1f);

        // Compact translucent panels so the corridor stays visible behind the HUD.
        private static readonly Color HudPanelColor = new Color(0f, 0f, 0f, 0.45f);
        private static readonly Color CuePanelColor = new Color(0.05f, 0.02f, 0.02f, 0.6f);
        private static readonly Color CueColor = new Color(0.95f, 0.30f, 0.27f, 1f);
        private static readonly Color CueDimColor = new Color(0.78f, 0.78f, 0.74f, 1f);

        // Phase 6 horror feedback colours (short flashes + near-death overlay).
        private static readonly Color FlashGood = new Color(0.40f, 0.90f, 0.45f, 1f);
        private static readonly Color FlashBad = new Color(0.85f, 0.10f, 0.10f, 1f);
        private static readonly Color FlashDark = new Color(0.02f, 0f, 0f, 1f);
        private static readonly Color DangerOverlayColor = new Color(0.45f, 0f, 0f, 1f);
        private static readonly Color LossPanelColor = new Color(0.18f, 0.01f, 0.01f, 0.90f);

        /// <summary>Raised when the player presses Start.</summary>
        public event Action StartClicked;

        /// <summary>Raised when the player presses Restart.</summary>
        public event Action RestartClicked;

        /// <summary>Raised with the answer index the player tapped.</summary>
        public event Action<int> AnswerSelected;

        private Font _font;

        private GameObject _startPanel;
        private GameObject _gameplayRoot;
        private GameObject _resultPanel;

        private Text _floorText;
        private Text _threatText;
        private RectTransform _timerFill;
        private Image _timerFillImage;
        private Text _timerText;
        private GameObject _cueZone;
        private Text _cueLabel;
        private Text _cueLines;
        private GameObject _clueBoardZone;
        private Text _clueBoardText;
        private GameObject _questionPanel;
        private Text _questionText;
        private Text _statusText;
        private Text _proximityText;

        private GameObject _floorTransitionPanel;
        private Text _floorTransitionTitle;
        private Text _floorTransitionSubtitle;

        private GameObject _observationPanel;
        private Text _observationTitle;
        private Text _observationSubtitle;

        private Text _startTitleText;
        private Text _introBodyText;
        private Text _startButtonLabel;
        private Text _restartButtonLabel;
        private Text _resultText;
        private Text _resultSubtitleText;

        private Image _flashOverlay;
        private Image _dangerOverlay;
        private Image _resultPanelImage;
        private Coroutine _flashRoutine;
        private Coroutine _pulseRoutine;

        private readonly Button[] _answerButtons = new Button[AnswerButtonCount];
        private readonly Text[] _answerLabels = new Text[AnswerButtonCount];

        private bool _built;

        private void Awake()
        {
            Build();
        }

        /// <summary>Build the entire HUD once. Safe to call multiple times.</summary>
        public void Build()
        {
            if (_built) return;
            _built = true;

            _font = LoadBuiltinFont();
            EnsureEventSystem();

            RectTransform canvasRoot = CreateCanvas();
            BuildGameplay(canvasRoot);
            BuildStartPanel(canvasRoot);
            BuildResultPanel(canvasRoot);

            // Full-screen flash overlay, created last so it sits on top of everything.
            // It is transparent and never receives input, so it never blocks the buttons.
            _flashOverlay = CreatePanel("FlashOverlay", canvasRoot, new Color(0f, 0f, 0f, 0f),
                new Vector2(0f, 0f), new Vector2(1f, 1f)).GetComponent<Image>();
            _flashOverlay.raycastTarget = false;

            ShowStartPanel();
        }

        // ---- Public view API ----------------------------------------------

        public void ShowStartPanel()
        {
            // Pull localized intro text at show-time so a code/test language switch reflects.
            if (_startTitleText != null) _startTitleText.text = PrototypeLocalization.Current(PrototypeLocalization.Title);
            if (_introBodyText != null) _introBodyText.text = PrototypeLocalization.Current(PrototypeLocalization.Intro);
            if (_startButtonLabel != null) _startButtonLabel.text = PrototypeLocalization.Current(PrototypeLocalization.BeginDescent);

            _startPanel.SetActive(true);
            _gameplayRoot.SetActive(false);
            _resultPanel.SetActive(false);
            HideFloorTransition();
        }

        public void ShowGameplay()
        {
            _startPanel.SetActive(false);
            _gameplayRoot.SetActive(true);
            _resultPanel.SetActive(false);
            HideFloorTransition();
            SetStatus(string.Empty, TextColor);
            ResetFeedback();
        }

        /// <summary>Clear any lingering flash, overlay and warning state (e.g. on restart).</summary>
        private void ResetFeedback()
        {
            if (_flashRoutine != null) { StopCoroutine(_flashRoutine); _flashRoutine = null; }
            if (_pulseRoutine != null) { StopCoroutine(_pulseRoutine); _pulseRoutine = null; }
            if (_flashOverlay != null) _flashOverlay.color = new Color(0f, 0f, 0f, 0f);
            if (_dangerOverlay != null) _dangerOverlay.color = new Color(DangerOverlayColor.r, DangerOverlayColor.g, DangerOverlayColor.b, 0f);
            if (_statusText != null) _statusText.rectTransform.localScale = Vector3.one;
            if (_proximityText != null) _proximityText.text = string.Empty;
        }

        public void ShowResult(bool won, string detail)
        {
            _gameplayRoot.SetActive(true); // keep HUD visible behind the result overlay
            HideFloorTransition();
            _resultPanel.SetActive(true);
            if (_restartButtonLabel != null) _restartButtonLabel.text = PrototypeLocalization.Current(PrototypeLocalization.Restart);

            // Dark/red overlay on loss, neutral dark on win.
            if (_resultPanelImage != null)
            {
                _resultPanelImage.color = won ? DimColor : LossPanelColor;
            }

            // On win the player reaches the ground floor; on loss the creature got in.
            _resultText.text = won
                ? PrototypeLocalization.Current(PrototypeLocalization.GroundFloor) + " — " +
                  PrototypeLocalization.Current(PrototypeLocalization.YouEscaped)
                : PrototypeLocalization.Current(PrototypeLocalization.SheGotIn);
            _resultText.color = won ? GoodColor : BadColor;

            if (_resultSubtitleText != null)
            {
                string subtitle = won
                    ? PrototypeLocalization.Current(PrototypeLocalization.WinSubtitle)
                    : PrototypeLocalization.Current(PrototypeLocalization.LossSubtitleCaught);
                _resultSubtitleText.text = subtitle + "\n\n" + detail;
                _resultSubtitleText.color = TextColor;
            }

            // A final flash punctuates the outcome over the result overlay.
            Flash(won ? FlashGood : FlashBad, won ? 0.25f : 0.5f, won ? 0.35f : 0.5f);
        }

        public void ShowQuestion(QuestionData question)
        {
            _questionText.text = question != null ? question.Prompt : string.Empty;
            SetStatus(string.Empty, TextColor);

            for (int i = 0; i < AnswerButtonCount; i++)
            {
                bool hasAnswer = question != null && i < question.AnswerCount;
                _answerButtons[i].gameObject.SetActive(hasAnswer);
                _answerButtons[i].interactable = hasAnswer;
                _answerLabels[i].text = hasAnswer ? question.Answers[i] : string.Empty;
            }
        }

        /// <summary>
        /// Show the clue that justifies the current question in the cue zone above the
        /// corridor. A null cue hides the zone. The highlighted line (e.g. the centered
        /// symbol) is wrapped in markers so it reads as the intended answer source.
        /// </summary>
        public void ShowCue(QuestionCue cue)
        {
            if (cue == null)
            {
                HideCue();
                return;
            }

            _cueLabel.text = cue.Label ?? string.Empty;
            _cueLines.text = FormatCueLines(cue);
            _cueZone.SetActive(true);
        }

        /// <summary>Hide the clue zone (no clue for this question or run not active).</summary>
        public void HideCue()
        {
            if (_cueZone != null) _cueZone.SetActive(false);
        }

        /// <summary>
        /// Enter the inter-floor transition (Phase 7B): hide the question, cue, answers and
        /// status so the lower screen reads as a safe "doors closing / ascending" moment,
        /// while the corridor and top HUD stay visible. Use <see cref="ShowFloorTransition"/>
        /// to set the messages and <see cref="HideFloorTransition"/> to leave it.
        /// </summary>
        public void BeginFloorTransition()
        {
            if (_questionPanel != null) _questionPanel.SetActive(false);
            HideCue();
            for (int i = 0; i < AnswerButtonCount; i++)
            {
                if (_answerButtons[i] != null) _answerButtons[i].gameObject.SetActive(false);
            }
            SetStatus(string.Empty, TextColor);
            if (_proximityText != null) _proximityText.text = string.Empty;
            if (_floorTransitionPanel != null) _floorTransitionPanel.SetActive(true);
        }

        /// <summary>Set the floor-transition title/subtitle (panel shown if not already).</summary>
        public void ShowFloorTransition(string title, string subtitle)
        {
            if (_floorTransitionPanel != null) _floorTransitionPanel.SetActive(true);
            if (_floorTransitionTitle != null) _floorTransitionTitle.text = title ?? string.Empty;
            if (_floorTransitionSubtitle != null) _floorTransitionSubtitle.text = subtitle ?? string.Empty;
        }

        /// <summary>Leave the inter-floor transition. The next question rebuilds its own UI.</summary>
        public void HideFloorTransition()
        {
            if (_floorTransitionPanel != null) _floorTransitionPanel.SetActive(false);
            if (_questionPanel != null) _questionPanel.SetActive(true);
        }

        /// <summary>
        /// Enter the Phase 7H observation pass: hide the question, cue, answers and status so
        /// the player cannot answer while observing, but keep the corridor, top HUD and the
        /// static clue board visible. Use <see cref="ShowObservationHint"/> for the overlay text
        /// and <see cref="HideObservationHint"/> to leave it. The clue board is updated/shown by
        /// <see cref="UpdateClues"/> before the pass, so it stays visible underneath.
        /// </summary>
        public void PrepareObservation()
        {
            if (_questionPanel != null) _questionPanel.SetActive(false);
            HideCue();
            for (int i = 0; i < AnswerButtonCount; i++)
            {
                if (_answerButtons[i] != null) _answerButtons[i].gameObject.SetActive(false);
            }
            SetStatus(string.Empty, TextColor);
            if (_proximityText != null) _proximityText.text = string.Empty;
        }

        /// <summary>Show the localized OBSERVE THE CORRIDOR overlay (text pulled at show-time).</summary>
        public void ShowObservationHint()
        {
            if (_observationTitle != null)
                _observationTitle.text = PrototypeLocalization.Current(PrototypeLocalization.ObserveTitle);
            if (_observationSubtitle != null)
                _observationSubtitle.text = PrototypeLocalization.Current(PrototypeLocalization.ObserveSubtitle);
            if (_observationPanel != null) _observationPanel.SetActive(true);
        }

        /// <summary>Hide the observation overlay and restore the question panel for the first trial.</summary>
        public void HideObservationHint()
        {
            if (_observationPanel != null) _observationPanel.SetActive(false);
            if (_questionPanel != null) _questionPanel.SetActive(true);
        }

        private static string FormatCueLines(QuestionCue cue)
        {
            if (cue.Lines.Count == 0) return string.Empty;

            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < cue.Lines.Count; i++)
            {
                if (i > 0) sb.Append('\n');
                bool highlight = cue.HasHighlight && i == cue.HighlightLineIndex && cue.Lines.Count > 1;
                if (highlight)
                {
                    sb.Append("» ").Append(cue.Lines[i]).Append(" «");
                }
                else
                {
                    sb.Append(cue.Lines[i]);
                }
            }
            return sb.ToString();
        }

        public void SetAnswersInteractable(bool interactable)
        {
            for (int i = 0; i < AnswerButtonCount; i++)
            {
                _answerButtons[i].interactable = interactable && _answerButtons[i].gameObject.activeSelf;
            }
        }

        public void UpdateTimer(float remaining, float limit)
        {
            float ratio = limit > 0f ? Mathf.Clamp01(remaining / limit) : 0f;
            _timerFill.anchorMin = new Vector2(0f, 0f);
            _timerFill.anchorMax = new Vector2(ratio, 1f);
            _timerFill.offsetMin = Vector2.zero;
            _timerFill.offsetMax = Vector2.zero;
            _timerText.text = remaining.ToString("0.0") + "s";

            // Timer pressure: bar colour shifts and a warning rises as time runs out.
            Color pressure = ratio > 0.5f ? GoodColor : (ratio > 0.25f ? WarnColor : BadColor);
            if (_timerFillImage != null) _timerFillImage.color = pressure;

            if (ratio <= 0.25f)
            {
                SetStatus("SHE IS CLOSER", BadColor);
            }
            else if (ratio <= 0.5f)
            {
                SetStatus("Hurry — she stirs", WarnColor);
            }
            else
            {
                SetStatus(string.Empty, TextColor);
            }
        }

        /// <summary>
        /// Show floor and trial progression together (Phase 7B.2). The player is climbing
        /// floors and surviving trials, so the HUD reads e.g. "FLOOR 1 / 5 — TRIAL 1 / 2".
        /// </summary>
        /// <summary>
        /// Show descent floor + trial progress (Phase 7B.4), localized, e.g.
        /// "FLOOR 5   —   TRIAL 1 / 5". The floor number counts DOWN toward the ground floor.
        /// </summary>
        public void UpdateProgress(int floor, int trial, int totalTrials)
        {
            _floorText.text = PrototypeLocalization.FloorAndTrial(floor, trial, totalTrials);
        }

        /// <summary>
        /// Refresh the static corridor clue board for the given displayed floor (Phase 7G).
        /// Reads the evidence data via <see cref="CorridorClueDisplayFormatter"/> and the
        /// current <see cref="PrototypeLocalization.Language"/>. Hides the board when the
        /// floor has no clues (safe fallback). This is a prototype evidence bridge, not final UI.
        ///
        /// Phase 7H.1: this builds the per-floor content AND shows the board for the observation
        /// pass. The board is hidden again by <see cref="HideClues"/> once the first trial starts.
        /// </summary>
        public void UpdateClues(int floorDisplayNumber)
        {
            if (_clueBoardText == null || _clueBoardZone == null) return;

            var entries = CorridorClueDisplayFormatter.BuildEntries(floorDisplayNumber);
            if (entries.Count == 0)
            {
                _clueBoardText.text = string.Empty;
                _clueBoardZone.SetActive(false);
                return;
            }

            _clueBoardText.text =
                CorridorClueDisplayFormatter.BuildBoardText(floorDisplayNumber, PrototypeLocalization.Language);
            _clueBoardZone.SetActive(true);
        }

        /// <summary>
        /// Phase 7H.1: hide the corridor clue board. Called when a trial's question starts, so
        /// clues are observation-only and the player answers from memory. Safe to call repeatedly.
        /// </summary>
        public void HideClues()
        {
            if (_clueBoardZone != null) _clueBoardZone.SetActive(false);
        }

        /// <summary>True when the corridor clue board is currently shown (Phase 7H.1 readability check).</summary>
        public bool AreCluesVisible => _clueBoardZone != null && _clueBoardZone.activeSelf;

        public void UpdateThreat(int distance, int stress, CreaturePhase phase)
        {
            _threatText.text = $"DIST {distance}   STRESS {stress}   {PhaseLabel(phase)}";
            _threatText.color = ThreatColor(phase);
        }

        private static string PhaseLabel(CreaturePhase phase)
        {
            switch (phase)
            {
                case CreaturePhase.Far: return "FAR";
                case CreaturePhase.Visible: return "SEEN";
                case CreaturePhase.MidCorridor: return "MID";
                case CreaturePhase.NearDoor: return "NEAR";
                case CreaturePhase.AtDoor: return "AT DOOR";
                case CreaturePhase.Attack: return "ATTACK";
                default: return phase.ToString().ToUpperInvariant();
            }
        }

        private static Color ThreatColor(CreaturePhase phase)
        {
            switch (phase)
            {
                case CreaturePhase.Far:
                case CreaturePhase.Visible:
                    return GoodColor;
                case CreaturePhase.MidCorridor:
                    return WarnColor;
                default:
                    return BadColor;
            }
        }

        public void SetStatus(string message, Color color)
        {
            _statusText.text = message;
            _statusText.color = color;
        }

        /// <summary>
        /// Show outcome feedback: a status message plus a short, transparent flash and an
        /// optional pulse. Flashes fade quickly and never receive input, so the corridor,
        /// question and answer buttons stay readable.
        /// </summary>
        public void ShowOutcomeStatus(AnswerOutcome outcome)
        {
            // Messages are localized (Phase 7B.4); flashes/pulses (Phase 6) are unchanged.
            string message = PrototypeLocalization.OutcomeMessage(outcome);
            switch (outcome)
            {
                case AnswerOutcome.CorrectFast:
                    SetStatus(message, GoodColor);
                    Flash(FlashGood, 0.18f, 0.22f);
                    break;
                case AnswerOutcome.CorrectNormal:
                    SetStatus(message, GoodColor);
                    Flash(FlashGood, 0.10f, 0.18f);
                    break;
                case AnswerOutcome.CorrectSlow:
                    SetStatus(message, WarnColor);
                    Flash(WarnColor, 0.10f, 0.18f);
                    break;
                case AnswerOutcome.Wrong:
                    SetStatus(message, BadColor);
                    Flash(FlashBad, 0.30f, 0.30f);
                    Pulse(_statusText != null ? _statusText.rectTransform : null, 1.18f, 0.22f);
                    break;
                case AnswerOutcome.Timeout:
                    SetStatus(message, BadColor);
                    // Stronger, darker flash ("brief blackout") for the worst outcome.
                    Flash(FlashDark, 0.55f, 0.40f);
                    Pulse(_statusText != null ? _statusText.rectTransform : null, 1.22f, 0.26f);
                    break;
            }
        }

        /// <summary>
        /// Update threat-proximity feedback from the current distance: the near-death
        /// overlay alpha ramps in below distance 25 and a warning message appears for
        /// near-death (&lt;= 25) and panic (&lt;= 10). Above that, no overlay/warning.
        /// </summary>
        public void UpdateProximity(int distance)
        {
            if (_dangerOverlay != null)
            {
                float alpha = ThreatProximityFeedback.GetOverlayAlpha(distance);
                _dangerOverlay.color = new Color(DangerOverlayColor.r, DangerOverlayColor.g, DangerOverlayColor.b, alpha);
            }

            if (_proximityText != null)
            {
                _proximityText.text = ThreatProximityFeedback.IsNearDeath(distance)
                    ? ThreatProximityFeedback.GetMessage(distance)
                    : string.Empty;
            }
        }

        /// <summary>Play a short, fading full-screen flash of the given colour.</summary>
        public void Flash(Color color, float peakAlpha, float duration)
        {
            if (_flashOverlay == null) return;
            if (_flashRoutine != null) StopCoroutine(_flashRoutine);
            _flashRoutine = StartCoroutine(FlashRoutine(color, peakAlpha, duration));
        }

        private System.Collections.IEnumerator FlashRoutine(Color color, float peakAlpha, float duration)
        {
            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float a = Mathf.Lerp(peakAlpha, 0f, duration > 0f ? t / duration : 1f);
                _flashOverlay.color = new Color(color.r, color.g, color.b, a);
                yield return null;
            }
            _flashOverlay.color = new Color(color.r, color.g, color.b, 0f);
            _flashRoutine = null;
        }

        /// <summary>Briefly scale a UI element up then back to one (a safe danger "pulse").</summary>
        public void Pulse(RectTransform target, float peakScale, float duration)
        {
            if (target == null) return;
            if (_pulseRoutine != null) StopCoroutine(_pulseRoutine);
            _pulseRoutine = StartCoroutine(PulseRoutine(target, peakScale, duration));
        }

        private System.Collections.IEnumerator PulseRoutine(RectTransform target, float peakScale, float duration)
        {
            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                float p = duration > 0f ? t / duration : 1f;
                // Up then back down: triangular profile peaking at the midpoint.
                float tri = 1f - Mathf.Abs(0.5f - p) * 2f;
                float scale = Mathf.Lerp(1f, peakScale, tri);
                target.localScale = new Vector3(scale, scale, 1f);
                yield return null;
            }
            target.localScale = Vector3.one;
            _pulseRoutine = null;
        }

        // ---- Build helpers -------------------------------------------------

        private RectTransform CreateCanvas()
        {
            var go = new GameObject("RuntimeGameplayCanvas",
                typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(transform, false);

            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = ReferenceResolution;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            return go.GetComponent<RectTransform>();
        }

        private void BuildGameplay(RectTransform parent)
        {
            _gameplayRoot = CreateContainer("GameplayRoot", parent);
            var root = (RectTransform)_gameplayRoot.transform;

            // Near-death overlay: behind the HUD (first sibling) so text stays readable.
            // Starts fully transparent; its alpha is driven by threat distance.
            _dangerOverlay = CreatePanel("DangerOverlay", root,
                new Color(DangerOverlayColor.r, DangerOverlayColor.g, DangerOverlayColor.b, 0f),
                new Vector2(0f, 0f), new Vector2(1f, 1f)).GetComponent<Image>();
            _dangerOverlay.raycastTarget = false;

            // ---- TOP HUD (compact translucent band; corridor stays visible) ----
            _floorText = CreateText("FloorText", root, "FLOOR 1 / 5   —   TRIAL 1 / 5", 38, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.952f), new Vector2(0.95f, 0.995f));
            _floorText.fontStyle = FontStyle.Bold;

            _threatText = CreateText("ThreatText", root, "DIST 85   STRESS 0   FAR", 40,
                TextAnchor.MiddleCenter, new Vector2(0.04f, 0.902f), new Vector2(0.96f, 0.948f));
            _threatText.fontStyle = FontStyle.Bold;

            // Timer bar (background + fill) with the numeric label overlaid inside it.
            RectTransform timerBg = CreatePanel("TimerBar", root, new Color(0.08f, 0.08f, 0.08f, 0.92f),
                new Vector2(0.06f, 0.856f), new Vector2(0.94f, 0.896f));
            _timerFill = CreatePanel("TimerFill", timerBg, GoodColor, new Vector2(0f, 0f), new Vector2(1f, 1f));
            _timerFillImage = _timerFill.GetComponent<Image>();
            _timerText = CreateText("TimerText", timerBg, "0.0s", 30, TextAnchor.MiddleCenter,
                new Vector2(0f, 0f), new Vector2(1f, 1f));
            _timerText.fontStyle = FontStyle.Bold;

            // ---- CLUE ZONE (top of the corridor view) ----
            // Small translucent panel that names where the question's clue came from.
            RectTransform cuePanel = CreatePanel("CueZone", root, CuePanelColor,
                new Vector2(0.10f, 0.760f), new Vector2(0.90f, 0.846f));
            _cueZone = cuePanel.gameObject;
            _cueLabel = CreateText("CueLabel", cuePanel, string.Empty, 26, TextAnchor.UpperCenter,
                new Vector2(0.04f, 0.62f), new Vector2(0.96f, 0.98f));
            _cueLabel.color = CueDimColor;
            _cueLabel.fontStyle = FontStyle.Bold;
            _cueLines = CreateText("CueLines", cuePanel, string.Empty, 38, TextAnchor.UpperCenter,
                new Vector2(0.04f, 0.04f), new Vector2(0.96f, 0.60f));
            _cueLines.color = CueColor;
            _cueLines.fontStyle = FontStyle.Bold;
            _cueZone.SetActive(false);

            // ---- MIDDLE (0.42 - 0.76) intentionally left clear: corridor + creature ----
            // Proximity warning sits high over the corridor and only appears near death,
            // so it never blocks the cue, the question or the answer buttons.
            _proximityText = CreateText("ProximityText", root, string.Empty, 46, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.690f), new Vector2(0.95f, 0.752f));
            _proximityText.fontStyle = FontStyle.Bold;
            _proximityText.color = BadColor;

            // ---- CORRIDOR CLUE BOARD (Phase 7G) ----
            // Static "observed clues" board for the current floor. Sits on the left of the
            // mid-corridor area: translucent so the corridor stays visible, and clear of the
            // timer (top), the status/question/answers (bottom) and the proximity warning.
            RectTransform cluePanel = CreatePanel("ClueBoardZone", root, CuePanelColor,
                new Vector2(0.04f, 0.470f), new Vector2(0.62f, 0.682f));
            _clueBoardZone = cluePanel.gameObject;
            _clueBoardText = CreateText("ClueBoardText", cluePanel, string.Empty, 26, TextAnchor.UpperLeft,
                new Vector2(0.06f, 0.05f), new Vector2(0.96f, 0.95f));
            _clueBoardText.color = CueDimColor;
            _clueBoardZone.SetActive(false);

            // ---- BOTTOM: feedback + compact question + answers ----
            _statusText = CreateText("StatusText", root, string.Empty, 38, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.405f), new Vector2(0.95f, 0.455f));
            _statusText.fontStyle = FontStyle.Bold;

            RectTransform qPanel = CreatePanel("QuestionPanel", root, HudPanelColor,
                new Vector2(0.04f, 0.335f), new Vector2(0.96f, 0.400f));
            _questionPanel = qPanel.gameObject;
            _questionText = CreateText("QuestionText", qPanel, string.Empty, 38, TextAnchor.MiddleCenter,
                new Vector2(0.03f, 0.05f), new Vector2(0.97f, 0.95f));

            // Four answer buttons stacked vertically across the bottom (thumb-friendly).
            float top = 0.325f;
            float height = 0.068f;
            float gap = 0.008f;
            for (int i = 0; i < AnswerButtonCount; i++)
            {
                float yMax = top - i * (height + gap);
                float yMin = yMax - height;
                int index = i; // capture
                Button button = CreateButton($"AnswerButton{i}", root, out Text label,
                    new Vector2(0.08f, yMin), new Vector2(0.92f, yMax));
                label.fontSize = 40;
                button.onClick.AddListener(() => AnswerSelected?.Invoke(index));
                _answerButtons[i] = button;
                _answerLabels[i] = label;
            }

            // ---- FLOOR TRANSITION OVERLAY ----
            // Translucent band over the lower question/answer area only, so the corridor
            // and creature stay visible above it. Shown between floors (FLOOR CLEARED /
            // DOORS CLOSING / DESCENDING); never receives input and starts hidden.
            RectTransform transitionPanel = CreatePanel("FloorTransitionPanel", root, DimColor,
                new Vector2(0.04f, 0.02f), new Vector2(0.96f, 0.46f));
            _floorTransitionPanel = transitionPanel.gameObject;
            _floorTransitionTitle = CreateText("FloorTransitionTitle", transitionPanel, string.Empty, 56,
                TextAnchor.MiddleCenter, new Vector2(0.05f, 0.52f), new Vector2(0.95f, 0.92f));
            _floorTransitionTitle.fontStyle = FontStyle.Bold;
            _floorTransitionTitle.color = GoodColor;
            _floorTransitionSubtitle = CreateText("FloorTransitionSubtitle", transitionPanel, string.Empty, 34,
                TextAnchor.MiddleCenter, new Vector2(0.06f, 0.10f), new Vector2(0.94f, 0.48f));
            _floorTransitionSubtitle.color = TextColor;
            _floorTransitionPanel.SetActive(false);

            // ---- OBSERVATION OVERLAY (Phase 7H) ----
            // Translucent band over the lower question/answer area only (same footprint as the
            // floor transition), so the corridor, creature and the clue board above stay visible
            // while the player observes. Shown once per floor before the first trial; starts hidden.
            RectTransform observationPanel = CreatePanel("ObservationPanel", root, DimColor,
                new Vector2(0.04f, 0.02f), new Vector2(0.96f, 0.46f));
            _observationPanel = observationPanel.gameObject;
            _observationTitle = CreateText("ObservationTitle", observationPanel, string.Empty, 50,
                TextAnchor.MiddleCenter, new Vector2(0.05f, 0.52f), new Vector2(0.95f, 0.92f));
            _observationTitle.fontStyle = FontStyle.Bold;
            _observationTitle.color = WarnColor;
            _observationSubtitle = CreateText("ObservationSubtitle", observationPanel, string.Empty, 32,
                TextAnchor.MiddleCenter, new Vector2(0.06f, 0.10f), new Vector2(0.94f, 0.50f));
            _observationSubtitle.color = TextColor;
            _observationPanel.SetActive(false);
        }

        private void BuildStartPanel(RectTransform parent)
        {
            _startPanel = CreatePanel("StartPanel", parent, DimColor,
                new Vector2(0f, 0f), new Vector2(1f, 1f)).gameObject;
            var root = (RectTransform)_startPanel.transform;

            // Narrative intro (Phase 7B.4): title, the wake-up/descent context, then the
            // BEGIN DESCENT button. All localized text is set in ShowStartPanel.
            _startTitleText = CreateText("Title", root, string.Empty, 60, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.80f), new Vector2(0.95f, 0.90f));
            _startTitleText.fontStyle = FontStyle.Bold;

            _introBodyText = CreateText("IntroBody", root, string.Empty, 34, TextAnchor.UpperCenter,
                new Vector2(0.08f, 0.30f), new Vector2(0.92f, 0.76f));
            _introBodyText.color = TextColor;

            Button start = CreateButton("BeginDescentButton", root, out Text startLabel,
                new Vector2(0.15f, 0.16f), new Vector2(0.85f, 0.26f));
            _startButtonLabel = startLabel;
            startLabel.text = string.Empty;
            startLabel.fontSize = 40;
            start.onClick.AddListener(() => StartClicked?.Invoke());
        }

        private void BuildResultPanel(RectTransform parent)
        {
            RectTransform panel = CreatePanel("ResultPanel", parent, DimColor,
                new Vector2(0f, 0f), new Vector2(1f, 1f));
            _resultPanel = panel.gameObject;
            _resultPanelImage = panel.GetComponent<Image>();
            var root = panel;

            // Large outcome headline.
            _resultText = CreateText("ResultText", root, string.Empty, 84, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.60f), new Vector2(0.95f, 0.80f));
            _resultText.fontStyle = FontStyle.Bold;

            // Subtitle + run detail.
            _resultSubtitleText = CreateText("ResultSubtitle", root, string.Empty, 32, TextAnchor.UpperCenter,
                new Vector2(0.08f, 0.42f), new Vector2(0.92f, 0.58f));

            Button restart = CreateButton("RestartButton", root, out Text restartLabel,
                new Vector2(0.2f, 0.26f), new Vector2(0.8f, 0.36f));
            _restartButtonLabel = restartLabel;
            restartLabel.text = string.Empty; // localized in ShowResult
            restartLabel.fontSize = 44;
            restart.onClick.AddListener(() => RestartClicked?.Invoke());
        }

        private static GameObject CreateContainer(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            Stretch(rt, new Vector2(0f, 0f), new Vector2(1f, 1f));
            return go;
        }

        private RectTransform CreatePanel(string name, Transform parent, Color color, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            var rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            Stretch(rt, min, max);
            go.GetComponent<Image>().color = color;
            return rt;
        }

        private Text CreateText(string name, Transform parent, string content, int fontSize,
            TextAnchor anchor, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            var rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            Stretch(rt, min, max);

            var text = go.GetComponent<Text>();
            text.font = _font;
            text.text = content;
            text.fontSize = fontSize;
            text.alignment = anchor;
            text.color = TextColor;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }

        private Button CreateButton(string name, Transform parent, out Text label, Vector2 min, Vector2 max)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            var rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            Stretch(rt, min, max);

            var image = go.GetComponent<Image>();
            image.color = ButtonColor;

            var button = go.GetComponent<Button>();
            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(1.1f, 1.1f, 1.1f, 1f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            colors.disabledColor = ButtonDisabledColor;
            button.colors = colors;

            label = CreateText(name + "Label", rt, string.Empty, 38, TextAnchor.MiddleCenter,
                new Vector2(0.04f, 0f), new Vector2(0.96f, 1f));
            return button;
        }

        private static void Stretch(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax)
        {
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);
        }

        private static Font LoadBuiltinFont()
        {
            // Unity 6 ships the legacy dynamic font under this name.
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null)
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            return font;
        }

        private static void EnsureEventSystem()
        {
#if UNITY_2023_1_OR_NEWER
            if (UnityEngine.Object.FindFirstObjectByType<EventSystem>() != null) return;
#else
            if (UnityEngine.Object.FindObjectOfType<EventSystem>() != null) return;
#endif
            var go = new GameObject("EventSystem", typeof(EventSystem));
            // The project uses the Input System package (activeInputHandler = 1),
            // so the legacy StandaloneInputModule would throw. Use the new module.
            var module = go.AddComponent<InputSystemUIInputModule>();
            module.AssignDefaultActions();
        }
    }
}
