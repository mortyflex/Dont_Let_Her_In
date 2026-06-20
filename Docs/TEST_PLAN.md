# Test Plan — Don’t Let Her In

## 1. Test Plan Summary

This document defines the testing strategy for **Don’t Let Her In**.

The project is a Unity mobile-first horror prototype.

The current milestone is:

```txt
Prototype v0.1 — First Fear Loop
```

The prototype must prove this gameplay loop:

```txt
Question starts
Timer starts
Creature advances
Player answers
Answer is evaluated
Threat distance changes
Next floor or death
```

Testing must focus first on the core loop, not on final art.

---

## 2. Testing Goals

The test plan must verify that:

- the core gameplay rules work
- answer outcomes affect threat distance correctly
- timeout is more dangerous than wrong answer
- death triggers correctly
- victory triggers correctly
- restart works
- the UI is usable in mobile portrait
- the creature movement is readable
- wrong answer feedback is visible
- timeout feedback is stronger than wrong answer feedback
- no blocking Unity console errors exist
- generated Unity folders are not committed

---

## 3. Testing Philosophy

Test the systems that can break the prototype.

Prioritize:

```txt
ThreatManager
QuestionManager
Answer evaluation
Answer speed classification
RunController
Death condition
Victory condition
Restart flow
Mobile UI readability
Scene integration
```

Do not waste early effort testing final graphics, placeholder models or visual polish.

Prototype visuals can be tested manually.

Core logic must be tested with Unity Test Framework where possible.

---

## 4. Test Types

The project uses these test types:

```txt
EditMode tests
PlayMode tests
Manual Play Mode checks
iOS device checks
Git hygiene checks
Documentation checks
```

---

## 5. EditMode Tests

EditMode tests are used for pure gameplay logic.

They should not require the full Unity scene.

Use EditMode tests for:

- threat distance rules
- stress rules
- answer speed classification
- answer evaluation
- run progression
- score calculation if implemented
- data validation

Recommended folder:

```txt
UnityProject/Assets/Tests/EditMode/
```

Recommended files:

```txt
ThreatManagerTests.cs
QuestionManagerTests.cs
QuestionEvaluatorTests.cs
RunControllerTests.cs
```

---

## 6. PlayMode Tests

PlayMode tests are used for scene and integration behavior.

Use PlayMode tests for:

- scene loads
- game systems exist
- UI appears
- answer buttons work
- death flow works
- victory flow works
- restart flow works
- floor transition works

Recommended folder:

```txt
UnityProject/Assets/Tests/PlayMode/
```

Recommended files:

```txt
GameSceneFlowTests.cs
GameplayUIViewTests.cs
CreatureControllerPlayModeTests.cs
```

PlayMode tests are optional for the earliest logic-only phases, but they become important once the scene is playable.

---

## 7. ThreatManager Test Cases

`ThreatManager` is a critical test target.

Required tests:

```txt
InitialDistance_IsSetFromConfig
InitialStress_IsSetFromConfig
Distance_IsClampedAtZero
Distance_IsClampedAtOneHundred
CorrectFast_IncreasesDistance
CorrectFast_ReducesStress
CorrectNormal_IncreasesDistance
CorrectSlow_IncreasesDistanceSlightly
WrongAnswer_DecreasesDistance
WrongAnswer_IncreasesStress
Timeout_DecreasesDistanceMoreThanWrongAnswer
Timeout_IncreasesStressMoreThanWrongAnswer
Death_IsTriggeredWhenDistanceReachesZero
Death_IsNotTriggeredWhenDistanceIsAboveZero
```

Expected prototype values:

```txt
Correct fast: +18 distance, stress -1
Correct normal: +10 distance
Correct slow: +3 distance
Wrong answer: -20 distance, stress +1
Timeout: -30 distance, stress +2
Death: distance <= 0
Distance clamp: 0 to 100
Stress clamp: 0 to 4
```

---

## 8. Answer Evaluation Test Cases

Required tests:

```txt
CorrectAnswer_ReturnsIsCorrectTrue
WrongAnswer_ReturnsIsCorrectFalse
Timeout_ReturnsIsTimeoutTrue
InvalidAnswerIndex_IsHandledSafely
FastAnswer_IsClassifiedAsFast
NormalAnswer_IsClassifiedAsNormal
SlowAnswer_IsClassifiedAsSlow
TimeoutAnswer_IsClassifiedAsTimeout
```

Suggested answer speed rules:

```txt
Fast: answered in first 35% of timer
Normal: answered between 35% and 70% of timer
Slow: answered after 70% of timer
Timeout: no answer before timer reaches 0
```

Example with 10-second timer:

```txt
0.0s to 3.5s: Fast
3.5s to 7.0s: Normal
7.0s to 10.0s: Slow
10.0s+: Timeout
```

---

## 9. QuestionData Validation Tests

If validation helpers exist, test:

```txt
QuestionData_WithPrompt_IsValid
QuestionData_WithoutPrompt_IsInvalid
QuestionData_WithAtLeastTwoAnswers_IsValid
QuestionData_WithLessThanTwoAnswers_IsInvalid
QuestionData_WithCorrectIndexInRange_IsValid
QuestionData_WithCorrectIndexOutOfRange_IsInvalid
QuestionData_WithPositiveTimer_IsValid
QuestionData_WithZeroTimer_IsInvalid
```

Question content should not be hardcoded inside gameplay managers.

---

## 10. RunController Test Cases

Required tests:

```txt
Run_StartsAtFirstFloor
Run_TracksCurrentFloor
Run_AdvancesAfterResolvedQuestion
Run_DoesNotAdvanceAfterDeath
Run_WinsAfterFinalFloor
Run_LosesWhenThreatDeathOccurs
Run_RestartResetsState
Run_TracksCorrectAnswers
Run_TracksWrongAnswers
Run_TracksTimeouts
```

If scoring is implemented:

```txt
Score_IncreasesWithFloorsCompleted
Score_IncreasesWithCorrectAnswers
Score_DecreasesWithWrongAnswers
Score_DecreasesWithTimeouts
Score_UsesFinalDistance
```

Scoring is optional for v0.1. Do not block the playable loop for scoring.

---

## 11. CreatureController Test Cases

Creature visuals can be mostly checked manually in v0.1.

If logic is testable, add tests for:

```txt
Distance100_ReturnsFarPhase
Distance80_ReturnsFarOrVisiblePhase
Distance60_ReturnsVisiblePhase
Distance40_ReturnsMidCorridorPhase
Distance25_ReturnsNearDoorPhase
Distance10_ReturnsAtDoorPhase
Distance0_ReturnsAttackPhase
```

Suggested phase mapping:

```txt
distance > 80: Far
distance > 60: Visible
distance > 40: MidCorridor
distance > 25: NearDoor
distance > 0: AtDoor
distance <= 0: Attack
```

Manual verification is required:

```txt
Wrong answer visibly brings creature closer
Timeout visibly brings creature closer more strongly
Correct fast visibly pushes creature away
Death triggers attack phase
```

---

## 12. UI Test Cases

UI can be tested manually first.

Required manual checks:

```txt
Question text is readable
Answer buttons are readable
Answer buttons are large enough
Timer is visible
Wrong answer feedback appears
Timeout feedback appears
Result screen appears
Restart button works
UI does not block creature
UI works in portrait aspect
```

If PlayMode UI tests are implemented, test:

```txt
QuestionPanel_ShowsPrompt
AnswerButtons_DisplayAnswers
AnswerButton_ClickSendsAnswer
TimerView_UpdatesRemainingTime
ResultView_ShowsWinOrLoss
RestartButton_RestartsRun
```

---

## 13. Scene Manual Test Checklist

For `Game.unity`, manually check:

```txt
Scene opens without blocking errors
SceneRoot exists
GameSystems exists
Elevator exists
Corridor exists
Creature exists
Lighting exists
UI exists
Audio exists if implemented
Camera is inside elevator
Corridor is visible
Creature is visible or can appear
Creature position changes are readable
UI is visible
Play Mode starts
No blocking console errors
```

---

## 14. Core Manual Gameplay Checklist

For every playable build, manually test:

```txt
Start run
First question appears
Timer starts
Tap correct answer quickly
Tap correct answer slowly
Tap wrong answer
Wait for timeout
Verify wrong answer brings creature closer
Verify timeout is worse than wrong answer
Verify correct fast pushes creature away
Trigger death
Reach victory if possible
Result screen appears
Restart works
Check console for errors
```

---

## 15. Horror Feedback Manual Checklist

Wrong answer must feel like a horror event.

Check:

```txt
Wrong answer flashes or glitches
Wrong answer has sound if audio exists
Wrong answer causes blackout or strong visual response if implemented
Creature is visibly closer after wrong answer
Stress feels increased
Gameplay resumes clearly
```

Timeout must feel worse.

Check:

```txt
Timeout feedback is stronger than wrong answer
Question disappears or clearly expires
Creature advances more than wrong answer
Lights or UI behave more aggressively
Player understands hesitation was punished
```

Correct fast must create relief.

Check:

```txt
Creature recedes or pressure drops
Feedback is readable
Player still does not feel fully safe
```

---

## 16. iOS Manual Test Checklist

Initial platform is iOS mobile portrait.

When iOS build preparation begins, check:

```txt
Unity target can switch to iOS
Portrait orientation is configured
Safe area is considered
UI is readable on iPhone aspect ratio
Touch input works
No keyboard is required
No mouse precision is required
Build can export to Xcode if environment allows
Xcode project opens if exported
Development build can run on device if signing is configured
No generated build output is committed
```

Primary device target:

```txt
iPhone 16 Pro
```

If actual device testing is not performed, the agent must say:

```txt
iOS device test not performed.
```

Do not claim iOS device testing passed unless it was actually run.

---

## 17. Performance Test Checklist

Prototype performance target:

```txt
30 FPS minimum on mobile-class hardware
```

Early phases may not measure FPS.

Still check:

```txt
No obvious heavy assets imported
No excessive real-time lights
No heavy post-processing added
No large textures imported
No unnecessary physics added
No repeated expensive scene searches in hot paths
No major allocations in core Update loop if visible
```

If performance was not measured, report:

```txt
Performance not measured.
```

Do not claim performance is good without measurement.

---

## 18. Git Hygiene Checks

Before any commit, run:

```bash
git status --short
```

Do not commit generated Unity folders.

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
*.apk
*.aab
*.ipa
*.app
*.exe
*.dmg
*.zip
.env
.env.local
```

Never use:

```bash
git add .
```

Use targeted adds.

Example:

```bash
git add Docs/PRD.md Docs/GAME_DESIGN.md
git commit -m "📝 docs(project): add prototype design docs"
```

---

## 19. Documentation Checks

After behavior changes, update relevant docs:

```txt
Docs/PRD.md
Docs/GAME_DESIGN.md
Docs/ART_DIRECTION.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
```

If a decision changes, update:

```txt
Docs/DECISIONS.md
```

If a test result or playtest happens, update:

```txt
Docs/PLAYTEST_NOTES.md
```

---

## 20. Agent Delivery Requirements

Every agent task must end with:

````md
### Summary

What changed.

### Files changed

- `path/to/file`

### Tests run

- Test command or Unity Test Runner action.

### Results

Pass/fail result.

### Manual checks

What was checked manually.

### Known limits

What is incomplete or not verified.

### Git status

Output or summary of `git status --short`.

### Recommended commit

```bash
git add path/to/file
git commit -m "type(scope): message"
```
````

````

If tests were not run, say why.

Correct:

```txt
Tests were not run because Unity Editor is unavailable in this environment.
````

Incorrect:

```txt
Tests should pass.
```

---

## 21. Phase-Specific Test Expectations

## 21.1 Phase 0 — Project Foundation

Expected validation:

```txt
Repo structure exists
Docs exist
Skills exist
AGENTS.md exists
.gitignore exists
UnityProject exists
Unity project opens if created
No gameplay required
No commit of generated Unity folders
```

## 21.2 Phase 1 — Core Gameplay Loop

Expected tests:

```txt
ThreatManager EditMode tests
RunController EditMode tests
Death condition tests
Victory condition tests
Restart state tests
```

## 21.3 Phase 2 — Question System

Expected tests:

```txt
QuestionData validation tests
Answer evaluation tests
Answer speed classification tests
Timeout tests
```

## 21.4 Phase 3 — Threat and Creature Distance

Expected checks:

```txt
Creature phase mapping tests if pure logic exists
Manual Play Mode check for distance readability
Wrong answer moves creature closer
Timeout moves creature closer more strongly
```

## 21.5 Phase 4 — Placeholder Scene Assembly

Expected checks:

```txt
Game.unity opens
Camera is inside elevator
Corridor visible
Creature anchor positions exist
UI canvas exists
No blocking console errors
```

## 21.6 Phase 5 — UI and Run Flow

Expected checks:

```txt
Start button works
Question appears
Answer buttons work
Timer visible
Result screen appears
Restart works
Portrait readability checked
```

## 21.7 Phase 6 — Horror Feedback

Expected checks:

```txt
Wrong answer feedback visible
Timeout feedback stronger
Correct fast relief visible
Death feedback visible
Feedback does not block next state
```

## 21.8 Phase 7 — Prototype Floor Content

Expected checks:

```txt
At least 3 floors playable
Preferred 5 floors playable
Each floor has a question
Player can win
Player can die
Difficulty feels progressive
```

## 21.9 Phase 8 — iOS Build Preparation

Expected checks:

```txt
iOS target configured
Portrait orientation configured
Safe area considered
Touch UI works
Xcode export attempted if environment allows
No build outputs committed
```

---

## 22. Prototype v0.1 Acceptance Checklist

Prototype v0.1 can be accepted only if:

```txt
Player can start a run
Question appears
Timer starts
Creature advances
Player can answer
Correct answer affects distance positively
Wrong answer affects distance negatively
Timeout affects distance more negatively
Creature reacts visually to distance
Player can die
Player can win
Result screen appears
Restart works
UI is readable in portrait
No blocking console errors
Core logic tests exist
Git status is clean after commit
```

---

## 22B. Phase 7B.4 Descent Loop Test Expectations

The current gameplay is the Phase 7B.4 descent loop. The EditMode suite must reflect this
behavior (and explicitly NOT the old ascending / score / Door Seal model).

Expected coverage:

```txt
Descent starts at Floor 5 (top floor shown first).
Progression Floor 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor (DescentFloorProfile display order).
Per-floor start distance: Floor 5=85, 4=80, 3=75, 2=70, 1=65 (DescentFloorProfile.StartDistance).
Each floor has 5 trials (PrototypeFloorSet: FloorCount=5, TrialsPerFloor=5, 25 total).
A correct answer consumes the trial and does NOT move the threat back (RecordCorrectSealed).
A wrong answer consumes the trial and moves the threat closer (-20, stress +1).
A timeout consumes the trial and moves the threat closer strongly (-30, stress +2).
The threat never recedes within a floor (NonRecedingThreatTests).
Threat and stress reset at the start of each floor (ThreatManager.ResetTo / ResetThreatForFloor).
Surviving all 5 trials of a non-final floor -> FloorCleared (descend).
Surviving all 5 trials of Floor 1 -> Escaped -> Ground Floor (YOU ESCAPED).
Threat distance <= 0 -> Lost -> SHE GOT IN (TrialFlowResolver).
No score / Door Seal gate is required to clear a floor (Door Seal is absent).
Language switching EN/FR works in code/tests (LocalizationTests, PrototypeLocalization).
Restart resets run, trial progress and threat after a win or a loss.
```

Relevant EditMode test files (existing):

```txt
DescentProgressionTests.cs
NonRecedingThreatTests.cs
TrialFlowResolverTests.cs
RunTrialProgressTests.cs
PrototypeFloorSetTests.cs
LocalizationTests.cs
ThreatManagerTests.cs
RunControllerTests.cs
QuestionManagerTests.cs / QuestionEvaluatorTests.cs
AnswerOutcomeResolverTests.cs / InterQuestionPacingTests.cs
CreatureControllerTests.cs / CreatureDistanceMapperTests.cs / ThreatProximityFeedbackTests.cs
PrototypeQuestionSetTests.cs / PrototypeQuestionCueSetTests.cs
```

The narrative intro before the run and the floor-transition messages (FLOOR CLEARED ->
DOORS CLOSING -> DESCENDING) are verified manually in Play Mode (see playtest checklist).

---

## 22C. Future Test Expectations — Corridor Observation & Evidence Trials (planned)

> **EditMode (pure data) expectations are IMPLEMENTED in Phase 7E**; the PlayMode/visual
> expectations remain planned (see `Docs/CORRIDOR_OBSERVATION_DESIGN.md`). Test files:
> `EvidenceTrialDataModelTests`, `EvidenceTrialValidatorTests`, `PrototypeEvidenceFloorSetTests`.

EditMode (pure data) expectations — covered by Phase 7E tests:

```txt
Every EvidenceTrial references a clueId. (EvidenceTrialValidator)
Every referenced clueId exists in the floor's FloorObservationSet. (validator + prototype test)
Each floor has at least 5 trials. (validator: FloorFewerThanFiveTrials)
Each trial has exactly 4 answers and exactly 1 correct answer. (validator + prototype test)
Each prototype correct answer matches the referenced clue's evidence value. (prototype test)
Clue ids and trial ids are unique within a floor; no empty ids. (validator)
Clue evidence value is non-empty. (validator)
Trial time limit is positive and difficulty >= 1. (validator)
Prompts and answers have English (validator) and EN/FR in the prototype set (prototype test).
```

PlayMode / manual expectations:

```txt
The observation phase can complete and hand off to the trial sequence.
Mobile readability: clue size/contrast readable in portrait; no color-only recognition.
Camera forward/backward pass plays and ends in a state where trials can start.
Threat/descent rules unchanged: correct consumes trial without receding; wrong/timeout move closer.
```

These tests must not change the current threat, descent or localization rules.

---

## 23. Current Test Status

Current status:

```txt
204/204 EditMode tests passing after Phase 7G (189 after 7F, 179 after 7E, 148 after 7B.4).
Descent loop, non-receding threat, trial flow, per-floor reset and EN/FR localization are covered by EditMode tests.
Phase 7E added evidence data-model tests (data containers, validator rules, prototype evidence set).
Phase 7F added PlayableContentLocalizationTests: the live PrototypeFloorSet prompts/answers/cues
  localize EN/FR, all 25 trials have EN+FR, French keeps 4 answers and the same correct index,
  and switching language does not change floor/trial counts or threat/floor progression data.
Phase 7G added CorridorClueDisplayFormatterTests: each displayed floor (5..1) maps to 5 clue
  entries from PrototypeEvidenceFloorSet, EN/FR board text, language-stable clue count, safe
  fallback for unknown floors, and never-null board/line text.
Intro readability, floor transitions, French UI smoke check and the clue board are manual Play Mode checks.
No iOS build checks yet.
```

Next expected test activity:

```txt
Phase 7H+: PlayMode tests for the observation camera pass and handoff when implemented.
```
