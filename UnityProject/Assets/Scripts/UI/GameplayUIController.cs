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
        private Text _questionText;
        private Text _statusText;
        private Text _resultText;

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

            ShowStartPanel();
        }

        // ---- Public view API ----------------------------------------------

        public void ShowStartPanel()
        {
            _startPanel.SetActive(true);
            _gameplayRoot.SetActive(false);
            _resultPanel.SetActive(false);
        }

        public void ShowGameplay()
        {
            _startPanel.SetActive(false);
            _gameplayRoot.SetActive(true);
            _resultPanel.SetActive(false);
            SetStatus(string.Empty, TextColor);
        }

        public void ShowResult(bool won, string detail)
        {
            _gameplayRoot.SetActive(true); // keep HUD visible behind the result overlay
            _resultPanel.SetActive(true);
            _resultText.text = (won ? "YOU ESCAPED" : "SHE GOT IN") + "\n\n" + detail;
            _resultText.color = won ? GoodColor : BadColor;
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

        public void UpdateFloor(int floor, int totalFloors)
        {
            _floorText.text = $"FLOOR {floor} / {totalFloors}";
        }

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

        public void ShowOutcomeStatus(AnswerOutcome outcome)
        {
            switch (outcome)
            {
                case AnswerOutcome.CorrectFast:
                    SetStatus("CORRECT — she recoils", GoodColor);
                    break;
                case AnswerOutcome.CorrectNormal:
                    SetStatus("Correct", GoodColor);
                    break;
                case AnswerOutcome.CorrectSlow:
                    SetStatus("Correct, but too slow", WarnColor);
                    break;
                case AnswerOutcome.Wrong:
                    SetStatus("WRONG — she steps closer", BadColor);
                    break;
                case AnswerOutcome.Timeout:
                    SetStatus("TOO LATE — she lunges", BadColor);
                    break;
            }
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

            // ---- TOP HUD (compact translucent band; corridor stays visible) ----
            _floorText = CreateText("FloorText", root, "FLOOR 1 / 5", 42, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.952f), new Vector2(0.95f, 0.995f));
            _floorText.fontStyle = FontStyle.Bold;

            _threatText = CreateText("ThreatText", root, "DIST 70   STRESS 0   FAR", 40,
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

            // ---- BOTTOM: feedback + compact question + answers ----
            _statusText = CreateText("StatusText", root, string.Empty, 38, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.405f), new Vector2(0.95f, 0.455f));
            _statusText.fontStyle = FontStyle.Bold;

            RectTransform qPanel = CreatePanel("QuestionPanel", root, HudPanelColor,
                new Vector2(0.04f, 0.335f), new Vector2(0.96f, 0.400f));
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
        }

        private void BuildStartPanel(RectTransform parent)
        {
            _startPanel = CreatePanel("StartPanel", parent, DimColor,
                new Vector2(0f, 0f), new Vector2(1f, 1f)).gameObject;
            var root = (RectTransform)_startPanel.transform;

            CreateText("Title", root, "DON'T LET HER IN", 64, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.62f), new Vector2(0.95f, 0.74f));
            CreateText("Tagline", root, "Every second of hesitation brings her closer.", 30,
                TextAnchor.MiddleCenter, new Vector2(0.08f, 0.54f), new Vector2(0.92f, 0.61f));

            Button start = CreateButton("StartButton", root, out Text startLabel,
                new Vector2(0.2f, 0.36f), new Vector2(0.8f, 0.46f));
            startLabel.text = "START";
            startLabel.fontSize = 44;
            start.onClick.AddListener(() => StartClicked?.Invoke());
        }

        private void BuildResultPanel(RectTransform parent)
        {
            _resultPanel = CreatePanel("ResultPanel", parent, DimColor,
                new Vector2(0f, 0f), new Vector2(1f, 1f)).gameObject;
            var root = (RectTransform)_resultPanel.transform;

            _resultText = CreateText("ResultText", root, string.Empty, 52, TextAnchor.MiddleCenter,
                new Vector2(0.05f, 0.45f), new Vector2(0.95f, 0.8f));

            Button restart = CreateButton("RestartButton", root, out Text restartLabel,
                new Vector2(0.2f, 0.28f), new Vector2(0.8f, 0.38f));
            restartLabel.text = "RESTART";
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
