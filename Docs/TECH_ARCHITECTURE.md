# Technical Architecture — Don’t Let Her In

## 1. Technical Summary

**Project name:** Don’t Let Her In  
**Engine:** Unity 6  
**Rendering:** URP  
**Language:** C#  
**Initial platform:** iOS mobile portrait  
**Future platforms:** Android, then possible VR/XR  
**Current milestone:** Prototype v0.1 — First Fear Loop

The prototype is a mobile-first horror game with a fixed first-person camera inside an elevator. The player answers short survival challenges while a creature approaches from a corridor.

The technical goal is to create a small, modular, testable Unity project that proves the core gameplay loop before investing in final art, audio, monetization or platform expansion.

Core loop (descent, Phase 7B.4):

```txt
Floor starts (threat reset to this floor's start distance)
Trial starts (1 of 5)
Timer starts
Player answers -> trial consumed
Wrong/timeout move the threat closer; correct does not move it back
Survive all 5 trials -> doors close -> descend; reach Ground Floor or get caught
```

---

## 1B. Current Implemented Systems (Phase 7B.4)

> Sections 7+ below describe the original recommended architecture (GameManager,
> FloorDirector, ScriptableObject data assets, etc.). The prototype as committed in
> Phase 7B.4 uses a leaner, code-authored set of systems. This section is the
> authoritative map of what actually exists and is tested.

Implemented systems (all under `UnityProject/Assets/Scripts/`):

```txt
GameLoop/PlayableRunFlowController.cs  - MonoBehaviour orchestrator: wires the pure
                                         systems to the UI and the creature, runs the
                                         descent flow (begin floor, run trials, transitions,
                                         result). Owns no game rules itself.
GameLoop/RunController.cs              - Pure run state: floor progression, answer stats,
                                         win/loss. RecordCorrectSealed() = correct answer
                                         with NO threat movement. ResetThreatForFloor()
                                         resets the threat per floor.
GameLoop/RunTrialProgress.cs           - Pure per-floor trial cursor (current floor/trial,
                                         final-trial / final-floor checks).
GameLoop/TrialFlowResolver.cs          - Pure rule: maps (isDead, isFinalTrial, isFinalFloor)
                                         to Lost / NextTrialSameFloor / FloorCleared / Escaped.
GameLoop/DescentFloorProfile.cs        - Pure descent tuning: displayed floor number counts
                                         down; per-floor start distance (Floor 5=85 .. Floor 1=65).
GameLoop/AnswerOutcome.cs +            - Classify an AnswerResult into
  AnswerOutcomeResolver.cs               CorrectFast/Normal/Slow / Wrong / Timeout.
GameLoop/InterQuestionPacing.cs        - Pure pacing helper (hold seconds per outcome).
GameLoop/ObservationPassTiming.cs      - Pure Phase 7H timing (observe hold / camera move /
                                         camera return seconds; clamps, total, all-positive).
GameLoop/ObservationPassState.cs       - Pure Phase 7H state guard: gates answers/timer while
                                         observing, blocks duplicate Begin, re-arms on restart.
GameLoop/ElevatorTransitionTiming.cs   - Pure Phase 7I timing (floor-cleared hold / door close /
                                         descent hold / door open; total, all-positive).
GameLoop/ElevatorTransitionState.cs    - Pure Phase 7I state guard: door/descent phase; gates
                                         answers/timer/clue-board/creature while the transition runs.
GameLoop/PrototypeLocalization.cs      - Central EN/FR string registry + current language.
GameLoop/LocalizedText.cs              - Small (english, french) pair with Get(language).
GameLoop/GameLanguage.cs               - enum { English, French }.
GameLoop/ThreatProximityFeedback.cs    - Pure near-death overlay alpha + warning messages.
Threat/ThreatManager.cs                - Distance/stress rules, clamps, death, ResetTo().
Threat/ThreatState.cs                  - Immutable threat snapshot.
Questions/QuestionManager.cs           - Question/timer flow, answer speed, AnswerResult.
Questions/QuestionData.cs, QuestionCue.cs, FloorDefinition.cs, FloorTrial.cs - data containers.
Questions/PrototypeFloorSet.cs         - Code-authored content: 5 floors x 5 trials (25 total),
                                         English-only. TrialCounts() drives RunTrialProgress.
Creature/CreatureController.cs +       - Distance-driven creature visual phase
  CreatureDistanceMapper.cs              (Far..Attack). No AI.
UI/GameplayUIController.cs             - Code-built mobile-portrait HUD: intro, trial,
                                         cue, timer, floor transition, result. Reads
                                         localized strings; owns no game rules.
```

Localization approach (Phase 7B.4):

```txt
Lightweight code-based localization (PrototypeLocalization + LocalizedText + GameLanguage).
English is the default; French available for UI / status / intro / transition / result.
Language is switchable from code/tests; there is no settings UI yet.
No Unity Localization package and no localization asset pipeline.
Phase 7F: the live playable trial content (PrototypeFloorSet) is now localized EN/FR too —
  QuestionData and QuestionCue carry optional French fields (promptFrench, answersFrench,
  labelFrench, linesFrench). Their player-facing getters (Prompt/Answers/Label/Lines) resolve
  to PrototypeLocalization.Language (English fallback). Gameplay stays index-based, so the
  correct answer, answer count and floor/trial structure are unchanged across languages.
  GameplayUIController needed no change (it already reads those getters).
```

Removed / obsolete (do not document as current):

```txt
DoorSealScoring / DoorSealScore   - Door Seal scoring experiment, removed in Phase 7B.4.
FloorThreatProfile                - replaced by DescentFloorProfile (per-floor start distance).
FloorTransitionText               - transition strings now live in PrototypeLocalization.
Score-based floor clear           - floors are cleared by surviving 5 trials, not by score.
```

Note: the original `GameManager` / `FloorDirector` / `ElevatorController` / `AudioDirector`
and the ScriptableObject data assets (`FloorData`, `CreatureData`, etc.) described in the
sections below are NOT all implemented yet. `PlayableRunFlowController` plays the role of the
high-level orchestrator, and prototype content is code-authored rather than stored in
ScriptableObject assets. The architecture stays compatible with adding them later.

---

## 1C. Corridor Observation & Evidence Trials — Data Model (Phase 7E) + planned runtime

> **Data model implemented in Phase 7E (DATA_MODEL_ONLY); runtime/visual systems still
> planned.** Full design is in `Docs/CORRIDOR_OBSERVATION_DESIGN.md`. Goal: evolve trials
> into evidence-based corridor observation puzzles (observe -> remember -> return -> answer)
> without changing the threat or descent rules.

Implemented pure-data types (Phase 7E, EditMode-tested, no Unity dependency), under
`UnityProject/Assets/Scripts/Questions/`, evolving today's `QuestionData` / `QuestionCue` /
`FloorTrial` / `FloorDefinition`:

```txt
CorridorClueType (enum) - DoorNumber, WallMessage, Symbol, LightState, ObjectPlacement,
                          Anomaly, ColorCue, AudioProxy, ShadowOrSilhouette,
                          DirectionInstruction, ScratchedCode, DoorState
CorridorClue            - Id, Type, FloorDisplayNumber, Label (LocalizedText),
                          Description (LocalizedText), VisualAnchor, EvidenceValue,
                          DifficultyWeight, IsRequiredForTrial
                          (generalizes QuestionCue with an in-world visual anchor + evidence)
EvidenceAnswerOption    - Id, Text (LocalizedText), IsCorrect
EvidenceTrial           - Id, ClueId (REQUIRED), Prompt (LocalizedText),
                          Answers (IReadOnlyList<EvidenceAnswerOption>), TimeLimitSeconds,
                          Difficulty (generalizes FloorTrial)
FloorObservationSet     - FloorDisplayNumber, Clues, Trials (+ clue lookup helpers)
EvidenceTrialValidator  - pure validator -> EvidenceValidationResult (typed issues):
                          no empty/duplicate ids, every trial references an existing clue,
                          exactly 4 answers, exactly 1 correct, positive time, difficulty >= 1,
                          non-empty clue evidence, >= 5 trials/floor, English present for
                          prompts/answers.
PrototypeEvidenceFloorSet - 5 floors x 5 evidence trials (25), EN/FR, validatable; DATA ONLY.
```

These types are not yet used at runtime for the trials themselves: `PlayableRunFlowController`
still drives trials from `PrototypeFloorSet`. Reuses `LocalizedText` / `GameLanguage`.

Static corridor clue board (Phase 7G — first evidence bridge, display only):

```txt
CorridorClueDisplayEntry      - pure: one board line (clue id, type, localized Label,
                                EvidenceValue) with GetLine(GameLanguage).
CorridorClueDisplayFormatter  - pure: BuildEntries(floorDisplayNumber) reads
                                PrototypeEvidenceFloorSet; BuildBoardText(floor, language)
                                returns the localized "OBSERVED CLUES" board (never null;
                                empty floor -> header only). Header from
                                PrototypeLocalization.ObservedClues (EN/FR).
GameplayUIController.UpdateClues(int) - runtime HUD: a translucent left-mid "OBSERVED CLUES"
                                panel built in code (no Game.unity edit), updated per floor.
PlayableRunFlowController.BeginFloor  - calls ui.UpdateClues(displayFloor) on run start and
                                each descent. Display only; trials still from PrototypeFloorSet.
```

The clue board reads the evidence model for content while the playable questions stay on
`PrototypeFloorSet`; the two are theme-aligned per displayed floor. Per-anchor in-world clues
remain a future phase.

Observation pass (Phase 7H — DONE, first version): `PlayableRunFlowController` now runs a short
observation pass once per floor (run start, after each descent, after restart), AFTER
`ui.UpdateClues(displayFloor)` and BEFORE the first trial. It shows a localized
`OBSERVE THE CORRIDOR` overlay (`PrototypeLocalization.ObserveTitle` / `ObserveSubtitle`) and
eases the existing Main Camera toward the corridor/red light and back (HYBRID; overlay-only
fallback if no camera). During the pass no question is active, so the timer, threat and trial
count cannot advance, and answers/question are hidden. Pure timing/state live in
`ObservationPassTiming` / `ObservationPassState`. This is in the orchestrator coroutine, not yet
the dedicated scene MonoBehaviours below. No Cinemachine, no new package, no `Game.unity` edit.

Phase 7H.1 tuning: the pass is slower and travels farther (`PlayableRunFlowController` field
initializers — move 1.2s / hold 2.5s / return 0.7s ≈ 4.4s; forward 1.5m / height 0.1m; the scene
does not serialize these, so no `Game.unity` edit). The static clue board is now OBSERVATION-ONLY:
`ui.UpdateClues(displayFloor)` builds+shows it for the pass, and `ui.HideClues()` (called at
`StartCurrentTrial`) hides it when the first question starts, so the player answers from memory.
The rule is expressed purely as `ObservationPassState.CluesVisible` (visible only while observing);
`GameplayUIController.AreCluesVisible` exposes the live state for inspection.

Phase 7I — elevator descent transition: between floors (after a NON-final floor clears), the
`ClearFloorThenAdvance` coroutine plays a prototype elevator descent: `HideTrialHudForTransition()`
+ `HideClues()`, the creature is masked (`SetObservationHidden(true)`), then two opaque UI doors
(`ShowElevatorDoors`/`SetElevatorDoorProgress` 0 open..1 closed) close, DESCENDING plays with a
subtle vertical `PlayDescentCue` while the floor indicator updates, then the doors open and only
THEN `BeginObservationThenTrial` runs. The clue board reveal moved from `BeginFloor` to the start of
the observation pass (so it stays hidden during the transition). Timing is plain serialized fields
(`floorClearedHoldSeconds`/`doorCloseSeconds`/`descentHoldSeconds`/`doorOpenSeconds`) plus pure
`ElevatorTransitionTiming`; gating is pure `ElevatorTransitionState`. The transition only runs on
`TrialResolution.FloorCleared` (non-final), never on `Escaped`. UI-only prototype: no door models,
no Cinemachine, no new package, no `Game.unity` edit.

Phase 7I playtest correction (door framing/timing adjustment): the transition is slower/heavier
(doorClose 1.5s / descent 3.0s / doorOpen 1.5s, ~6.8s total, bounded <= 8s, still shorter than the
observation pass), and the doors only cover the central corridor aperture instead of the full
screen — `GameplayUIController.DoorApertureWidthRatio` (0.68) drives `SetElevatorDoorProgress`, so
each leaf grows from its aperture edge to the centre when closing and collapses to zero width at the
edge when open; the side cabin (buttons/walls) stays visible. The ratio is a public const, so the
"doors are not full-screen" rule is unit-testable.

Proposed MonoBehaviours (still planned — own NO trial/threat rules):

```txt
ObservationPhaseController    - plays forward/backward camera travel, exposes the floor's
                                clues, raises an "observation complete" event for handoff.
CorridorObservationController - binds a FloorObservationSet to corridor visual anchors,
                                swaps per-floor clue visuals while keeping the corridor
                                structurally consistent.
```

Planned integration: in a future phase, `PlayableRunFlowController` requests an observation
pass when a floor begins (via `ObservationPhaseController`) and starts the trial sequence
only after the handoff. The current trial flow, `ThreatManager` rules, `DescentFloorProfile`
tuning and descent transitions remain unchanged. Localization reuses the existing
`PrototypeLocalization` / `LocalizedText` / `GameLanguage` approach.

Phasing: 7E (evidence data model — DONE) -> 7F (question content localization EN/FR) ->
7G (static corridor clues) -> 7H (observation camera pass) -> 7I (evidence-based floor playtest).

---

## 2. Architecture Principles

The project must follow these principles:

```txt
Simple before complex
Prototype before polish
Placeholders before final assets
Data-driven before hardcoded content
Testable logic before scene-only logic
Mobile-first before cinematic excess
Small tasks before large refactors
```

The architecture should allow later replacement of:

- placeholder art
- placeholder audio
- temporary UI
- simple creature movement
- basic questions
- simple floor data

without rewriting the entire gameplay loop.

---

## 3. Unity Project Location

The Unity project must live inside:

```txt
UnityProject/
```

Repository root contains:

```txt
AGENTS.md
README.md
Docs/
Skills/
UnityProject/
```

Unity-specific files must remain inside `UnityProject/`.

---

## 4. Recommended Unity Folder Structure

Inside `UnityProject/Assets/`:

```txt
Assets/
  Art/
    Characters/
    Elevator/
    Corridor/
    Props/
    Materials/
    VFX/
  Audio/
    Ambience/
    SFX/
    Voices/
    Music/
  Prefabs/
    Elevator/
    Corridor/
    Creature/
    UI/
    Systems/
  Scenes/
    Game.unity
  Scripts/
    Core/
    GameLoop/
    Questions/
    Threat/
    Creature/
    Elevator/
    UI/
    Audio/
    Save/
    Tools/
  ScriptableObjects/
    Questions/
    Floors/
    Creatures/
    Difficulty/
    Audio/
    HorrorEvents/
  Tests/
    EditMode/
    PlayMode/
```

Do not create random top-level folders without a reason.

---

## 5. Scene Strategy

For prototype v0.1, use one main scene:

```txt
UnityProject/Assets/Scenes/Game.unity
```

Later scenes may be added:

```txt
Boot.unity
MainMenu.unity
Results.unity
```

But v0.1 can use only `Game.unity` if it contains:

- start flow
- gameplay flow
- result flow
- restart flow

---

## 6. Recommended Scene Hierarchy

`Game.unity` should use this hierarchy:

```txt
SceneRoot
  GameSystems
  Elevator
  Corridor
  Creature
  Lighting
  UI
  Audio
```

Recommended `GameSystems` children:

```txt
GameSystems
  GameManager
  RunController
  QuestionManager
  ThreatManager
  FloorDirector
  AudioDirector
```

Recommended `Elevator` children:

```txt
Elevator
  ElevatorInterior
  DoorLeft
  DoorRight
  ButtonPanel
  DigitalDisplay
  CameraAnchor
```

Recommended `Corridor` children:

```txt
Corridor
  Floor
  Walls
  Ceiling
  Doors
  Props
  ClueAnchors
  CreatureAnchors
```

Recommended `CreatureAnchors` children:

```txt
CreatureAnchors
  Far
  Visible
  MidCorridor
  NearDoor
  AtDoor
  Attack
```

Recommended `UI` children:

```txt
UI
  Canvas
    SafeArea
      QuestionPanel
      AnswerButtons
      TimerView
      FeedbackOverlay
      ResultPanel
```

Recommended `Audio` children:

```txt
Audio
  AmbienceSource
  ElevatorSource
  CreatureSource
  UISource
```

---

## 7. Core Systems

## 7.1 `GameManager`

Purpose:

```txt
Own global game state.
Coordinate high-level transitions.
```

Responsibilities:

- current game state
- start run
- end run
- route to result state
- expose game state events

Should not:

- calculate answer effects
- store question content
- move the creature directly
- handle all UI logic

Suggested location:

```txt
UnityProject/Assets/Scripts/Core/GameManager.cs
```

---

## 7.2 `RunController`

Purpose:

```txt
Own the current run.
Track floors, run stats, win/loss.
```

Responsibilities:

- current floor index
- total floors
- floors completed
- correct answers count
- wrong answers count
- timeout count
- average response time
- win condition
- loss condition
- restart/reset

Suggested location:

```txt
UnityProject/Assets/Scripts/GameLoop/RunController.cs
```

---

## 7.3 `QuestionManager`

Purpose:

```txt
Own question flow.
Start questions, handle timer, evaluate answers.
```

Responsibilities:

- load current question
- start timer
- expose remaining time
- receive selected answer
- detect timeout
- classify answer speed
- emit `AnswerResult`

Should not:

- decide creature visuals
- hardcode all questions
- own full run state

Suggested location:

```txt
UnityProject/Assets/Scripts/Questions/QuestionManager.cs
```

---

## 7.4 `ThreatManager`

Purpose:

```txt
Own creature distance and stress logic.
```

Responsibilities:

- current distance
- current stress
- apply correct fast result
- apply correct normal result
- apply correct slow result
- apply wrong answer result
- apply timeout result
- clamp distance
- clamp stress
- detect death

This is one of the most important test targets.

Suggested location:

```txt
UnityProject/Assets/Scripts/Threat/ThreatManager.cs
```

---

## 7.5 `CreatureController`

Purpose:

```txt
Represent threat state visually.
```

Responsibilities:

- read distance from `ThreatManager` or received state
- move creature to distance-based position
- update creature phase
- trigger wrong-answer movement
- trigger timeout movement
- trigger attack feedback

Should not:

- calculate answer correctness
- decide run outcome
- own question timer

Suggested location:

```txt
UnityProject/Assets/Scripts/Creature/CreatureController.cs
```

---

## 7.6 `ElevatorController`

Purpose:

```txt
Control elevator presentation.
```

Responsibilities:

- door state
- floor display
- door twitch/jam feedback
- elevator light feedback
- optional button panel feedback

Suggested location:

```txt
UnityProject/Assets/Scripts/Elevator/ElevatorController.cs
```

---

## 7.7 `FloorDirector`

Purpose:

```txt
Coordinate floor-level setup.
```

Responsibilities:

- select current floor data
- set initial creature distance
- select question
- apply lighting mood
- trigger optional floor horror event
- request transition to next floor

Suggested location:

```txt
UnityProject/Assets/Scripts/GameLoop/FloorDirector.cs
```

---

## 7.8 `AudioDirector`

Purpose:

```txt
Coordinate audio feedback.
```

Responsibilities:

- ambience loop
- wrong answer sound
- timeout sound
- correct answer sound
- creature proximity sound
- attack sound

Suggested location:

```txt
UnityProject/Assets/Scripts/Audio/AudioDirector.cs
```

---

## 8. Game State Machine

Use a clear state machine.

Prototype states:

```txt
Boot
MainMenu
RunStart
ElevatorIdle
QuestionActive
ResolvingAnswer
FloorTransition
CreatureAttack
RunWon
RunLost
Results
```

Suggested enum:

```csharp
public enum GameState
{
    Boot,
    MainMenu,
    RunStart,
    ElevatorIdle,
    QuestionActive,
    ResolvingAnswer,
    FloorTransition,
    CreatureAttack,
    RunWon,
    RunLost,
    Results
}
```

State transition rules:

```txt
Boot -> MainMenu
MainMenu -> RunStart
RunStart -> ElevatorIdle
ElevatorIdle -> QuestionActive
QuestionActive -> ResolvingAnswer
ResolvingAnswer -> FloorTransition
ResolvingAnswer -> CreatureAttack
FloorTransition -> ElevatorIdle
CreatureAttack -> RunLost
RunLost -> Results
RunWon -> Results
Results -> RunStart
```

For the first prototype, `MainMenu` can be a simple start panel inside `Game.unity`.

---

## 9. Data-Driven Design

Do not hardcode gameplay content directly into managers.

Use ScriptableObjects where possible.

Required or recommended data assets:

```txt
QuestionData
FloorData
CreatureData
DifficultyData
AudioCueData
HorrorEventData
```

---

## 10. `QuestionData`

Suggested location:

```txt
UnityProject/Assets/Scripts/Questions/QuestionData.cs
```

Suggested asset folder:

```txt
UnityProject/Assets/ScriptableObjects/Questions/
```

Suggested fields:

```txt
id
type
prompt
answers
correctAnswerIndex
timeLimitSeconds
difficulty
fastCorrectReward
normalCorrectReward
slowCorrectReward
wrongAnswerPenalty
timeoutPenalty
optionalVisualClueId
optionalAudioClueId
tags
```

Suggested enum:

```csharp
public enum QuestionType
{
    Observation,
    ShortMemory,
    AudioClue,
    EnvironmentalInstruction,
    SimpleLogic,
    SangFroid,
    Anomaly
}
```

---

## 11. `FloorData`

Suggested location:

```txt
UnityProject/Assets/Scripts/GameLoop/FloorData.cs
```

Suggested asset folder:

```txt
UnityProject/Assets/ScriptableObjects/Floors/
```

Suggested fields:

```txt
floorIndex
floorLabel
questions
initialCreatureDistance
creatureAdvanceSpeed
lightingMood
horrorEvent
```

---

## 12. `CreatureData`

Suggested location:

```txt
UnityProject/Assets/Scripts/Creature/CreatureData.cs
```

Suggested asset folder:

```txt
UnityProject/Assets/ScriptableObjects/Creatures/
```

Suggested fields:

```txt
id
displayName
farThreshold
visibleThreshold
midCorridorThreshold
nearDoorThreshold
atDoorThreshold
attackThreshold
baseAdvanceSpeed
wrongAnswerMoveStyle
timeoutMoveStyle
```

---

## 13. Runtime Models

## 13.1 `AnswerSpeed`

Suggested enum:

```csharp
public enum AnswerSpeed
{
    Fast,
    Normal,
    Slow,
    Timeout
}
```

## 13.2 `AnswerResult`

Suggested fields:

```txt
questionId
isCorrect
answerSpeed
selectedAnswerIndex
correctAnswerIndex
responseTimeSeconds
distanceDelta
stressDelta
isTimeout
```

Suggested file:

```txt
UnityProject/Assets/Scripts/Questions/AnswerResult.cs
```

## 13.3 `ThreatState`

Suggested fields:

```txt
distance
stressLevel
isDead
lastDistanceDelta
lastStressDelta
```

Suggested file:

```txt
UnityProject/Assets/Scripts/Threat/ThreatState.cs
```

## 13.4 `RunResult`

Suggested fields:

```txt
won
lost
floorsCompleted
correctAnswers
wrongAnswers
timeouts
averageResponseTime
finalDistance
score
```

Suggested file:

```txt
UnityProject/Assets/Scripts/GameLoop/RunResult.cs
```

---

## 14. Event Strategy

Use simple C# events or UnityEvents for prototype.

Avoid overengineering a full event bus at v0.1 unless needed.

Possible events:

```txt
OnRunStarted
OnRunEnded
OnQuestionStarted
OnQuestionAnswered
OnQuestionTimedOut
OnThreatChanged
OnCreaturePhaseChanged
OnPlayerDied
OnFloorCompleted
OnResultShown
```

For pure logic, prefer C# events.

For Inspector-driven scene reactions, UnityEvents are acceptable.

---

## 15. UI Architecture

Recommended UI scripts:

```txt
QuestionView
AnswerButtonView
TimerView
FeedbackOverlayView
ResultView
GameplayUIView
```

Suggested folder:

```txt
UnityProject/Assets/Scripts/UI/
```

Responsibilities:

## 15.1 `QuestionView`

- display question prompt
- display answer choices
- expose answer selected event

## 15.2 `TimerView`

- display remaining time
- optionally animate urgency

## 15.3 `FeedbackOverlayView`

- show wrong-answer feedback
- show timeout feedback
- show correct feedback
- show blackout/glitch overlay if implemented

## 15.4 `ResultView`

- show survived/caught
- show run stats
- expose restart button

UI must not own core gameplay rules.

---

## 16. Creature Visual Architecture

`CreatureController` should map distance to phase.

Suggested phases:

```csharp
public enum CreaturePhase
{
    Far,
    Visible,
    MidCorridor,
    NearDoor,
    AtDoor,
    Attack
}
```

Suggested mapping:

```txt
distance > 80: Far
distance > 60: Visible
distance > 40: MidCorridor
distance > 25: NearDoor
distance > 0: AtDoor
distance <= 0: Attack
```

The controller may:

- snap to anchors
- interpolate to anchors
- hide the creature when far
- trigger attack on distance 0

Prototype can snap between anchors.

Polish can interpolate later.

---

## 17. Timer Architecture

Question timer belongs to `QuestionManager` or a small helper class.

Suggested behavior:

```txt
Start timer with question time limit
Track elapsed time
Expose remaining time
Classify answer speed
Trigger timeout event
Stop timer after answer
Stop timer on death
```

Avoid timer logic spread across UI scripts.

---

## 18. Save System

Save system is optional for v0.1.

If implemented, keep it minimal.

Possible saved data:

```txt
bestScore
bestFloor
settingsVolume
hasCompletedPrototype
```

Suggested file:

```txt
UnityProject/Assets/Scripts/Save/SaveManager.cs
```

Do not add cloud save in v0.1.

---

## 19. Audio Architecture

Use `AudioDirector` for high-level audio events.

Suggested audio events:

```txt
PlayCorrectFast
PlayCorrectNormal
PlayWrongAnswer
PlayTimeout
PlayCreatureNear
PlayAttack
SetThreatIntensity
```

Audio intensity can be based on distance:

```txt
far: low ambience
mid: footsteps/scrape audible
near: breathing/metal pressure
door: intense close threat
```

Do not add complex audio middleware in v0.1.

---

## 20. Mobile Architecture

Prototype target:

```txt
Android first
Portrait orientation
Touch input
Fixed camera
No joystick
No keyboard
No controller requirement
```

Mobile rules:

- large tap targets
- readable text
- no tiny UI
- no precision input
- no free movement
- simple scene
- fast restart

---

## 21. Performance Constraints

Target:

```txt
30 FPS minimum for prototype
No blocking console errors
Small scene
Limited lights
No heavy post-processing
No large asset packs without approval
```

Avoid:

- many real-time lights
- heavy shadows
- large textures
- complex shaders
- dense particles
- unnecessary physics
- heavy per-frame allocations
- repeated `FindObjectOfType` calls

---

## 22. Testing Architecture

Use Unity Test Framework.

Test folders:

```txt
UnityProject/Assets/Tests/EditMode/
UnityProject/Assets/Tests/PlayMode/
```

Prioritize EditMode tests for pure logic.

Minimum test files:

```txt
ThreatManagerTests.cs
QuestionEvaluatorTests.cs
RunControllerTests.cs
```

Possible PlayMode test file:

```txt
GameSceneFlowTests.cs
```

---

## 23. Required Tests for Core Loop

The prototype should include tests for:

```txt
Threat distance clamps between 0 and 100
Correct fast increases distance
Correct normal increases distance
Correct slow increases distance slightly
Wrong answer decreases distance
Timeout decreases distance more than wrong answer
Stress increases after wrong answer
Stress increases more after timeout
Correct fast can reduce stress
Death triggers when distance reaches 0
Answer speed classification works
Run can win after final floor
Run can reset after loss
```

If Unity Editor is unavailable, the agent must say tests were not run.

Never claim tests passed unless they were executed.

---

## 24. Git Architecture Rules

Never commit generated Unity folders.

Do not commit:

```txt
UnityProject/Library/
UnityProject/Temp/
UnityProject/Obj/
UnityProject/Build/
UnityProject/Builds/
UnityProject/Logs/
UnityProject/UserSettings/
UnityProject/MemoryCaptures/
UnityProject/Recordings/
```

Do not use:

```bash
git add .
```

Use targeted adds only.

Example:

```bash
git add UnityProject/Assets/Scripts/Threat/ThreatManager.cs UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs
git commit -m "🎮 feat(gameplay): add threat distance system"
```

---

## 25. Dependency Rules

Do not add third-party dependencies in v0.1 unless explicitly approved.

Allowed by default:

```txt
Unity built-in systems
URP
TextMeshPro
Unity Test Framework
Unity Input System if needed
```

Not allowed in v0.1 unless explicitly requested:

```txt
ads SDK
analytics SDK
IAP SDK
networking SDK
VR/XR SDK
large visual scripting framework
large third-party controller
advanced audio middleware
```

---

## 26. Build Rules

Initial build target:

```txt
Android
Portrait
Development build acceptable for testing
```

Builds should not be committed.

Build output folders should remain ignored.

Potential local output:

```txt
Builds/Android/
```

This folder is ignored.

---

## 27. Agent Implementation Rules

For each implementation task, the agent must:

1. Read relevant docs.
2. Restate scope.
3. Implement only requested changes.
4. Keep changes small.
5. Add tests when logic changes.
6. Run tests if possible.
7. Check Git status.
8. Report files changed.
9. Suggest targeted commit command.

The agent must not:

- refactor unrelated systems
- add final art
- import heavy assets
- add monetization
- add VR
- add procedural generation
- add online systems
- change product scope silently

---

## 28. Prototype v0.1 Technical Definition of Done

Prototype v0.1 is technically complete when:

- `Game.unity` opens without blocking errors
- player can start a run
- question appears
- timer starts
- answer buttons work
- answer speed is classified
- `ThreatManager` updates distance and stress
- creature visual reacts to distance
- wrong answer feedback plays
- timeout feedback plays
- death state works
- victory state works
- result screen appears
- restart works
- mobile portrait UI is readable
- core logic has EditMode tests
- generated Unity folders are ignored
- docs reflect current behavior

---

## 29. Future Technical Extensions

Do not implement these in v0.1, but keep architecture compatible:

```txt
better creature animation
better art assets
more floors
more question banks
daily challenge
infinite mode
save progression
settings menu
haptics
analytics
iOS build
VR/XR adaptation
localization
```

---

## 30. Current Technical Decisions

Current decisions:

```txt
Engine: Unity 6
Rendering: URP
Language: C#
Platform: iOS first
Orientation: portrait
Camera: fixed inside elevator
Movement: no free movement
Creature AI: none in v0.1
Creature logic: distance-driven
Data: ScriptableObjects where possible
Testing: Unity Test Framework
Build outputs: ignored
```
