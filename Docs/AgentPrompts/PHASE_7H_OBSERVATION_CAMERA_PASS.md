# Agent Prompt — Phase 7H Observation Camera Pass Prototype

## Recommended Model

Recommended model:

```txt
Claude
```

Model switch recommendation:

```txt
Do not switch models for this phase.
```

Reason:

```txt
This phase touches the live gameplay loop, camera motion timing, HUD visibility, floor transitions and the static clue board added in Phase 7G. It must stay carefully scoped and regression-safe. Claude should continue for continuity.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎥 feat(gameplay): add observation camera pass
```

---

## Project

You are working on the Unity project:

```txt
Don’t Let Her In
```

Unity 6 URP, iOS-first portrait horror prototype.

Current official concept:

```txt
The player wakes up high inside a sinister building.
They are trapped in an elevator with the doors open.
The goal is to descend floor by floor to the Ground Floor.
Each floor contains 5 trials.
The threat never recedes during a floor.
Correct answers let the player continue but do not push the threat back.
Wrong answers and timeouts move the threat closer.
Surviving all 5 trials of a floor closes the doors and descends to the next floor.
If the threat reaches the elevator, SHE GOT IN.
After surviving Floor 1, the player reaches the Ground Floor and escapes.
```

Recent completed phases:

```txt
Phase 7E — Evidence Trial Data Model
Phase 7F — Question Content Localization EN/FR
Phase 7G — Static Corridor Clue Prototype
```

Current known status:

```txt
204/204 EditMode tests passed after Phase 7G.
Phase 7G playtest was user-validated:
- clue board visible
- clues readable
- no major HUD obstruction
- Floor 5 -> Floor 4 clue update OK
- gameplay still stable
```

Current runtime:

```txt
Playable trials still come from PrototypeFloorSet.
Static clues are displayed through a CLUE_BOARD built by GameplayUIController.
Clue display data comes from PrototypeEvidenceFloorSet through CorridorClueDisplayFormatter.
```

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/PRD.md
Docs/GAME_DESIGN.md
Docs/CORRIDOR_OBSERVATION_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Docs/PLAYTEST_NOTES.md
Docs/AgentPrompts/PHASE_7H_OBSERVATION_CAMERA_PASS.md
Docs/AgentPrompts/PHASE_7G_STATIC_CORRIDOR_CLUE_PROTOTYPE.md
Docs/AgentPrompts/PHASE_7F_QUESTION_CONTENT_LOCALIZATION.md
Docs/AgentPrompts/PHASE_7E_EVIDENCE_TRIAL_DATA_MODEL.md
Docs/AgentPrompts/PHASE_7D_CORRIDOR_OBSERVATION_DESIGN.md
Docs/AgentPrompts/PHASE_7B4_DESCENT_LOOP_INTRO_LOCALIZATION.md
Skills/horror-game-design/SKILL.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-mobile-performance/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Inspect current code and scene:

```txt
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/DescentFloorProfile.cs
UnityProject/Assets/Scripts/GameLoop/InterQuestionPacing.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
UnityProject/Assets/Scripts/Questions/PrototypeFloorSet.cs
UnityProject/Assets/Scripts/Questions/PrototypeEvidenceFloorSet.cs
UnityProject/Assets/Scripts/Questions/CorridorClueDisplayFormatter.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Scripts/Creature/CreatureController.cs
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code names, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Mission

Implement:

```txt
Phase 7H — Observation Camera Pass Prototype
```

The goal is to add a short observation moment before each floor's trials.

The player should feel:

```txt
Doors open.
The camera looks slightly deeper into the corridor.
The clue board / clue details are visible.
The player gets a short moment to observe.
Then the trial UI becomes active.
```

This phase is about pacing and readability.

It is not about final cinematic polish.

---

## Target Runtime Loop

Current loop:

```txt
Intro
BEGIN DESCENT
Floor starts
Question appears immediately
Player answers 5 trials
Doors closing
Descending
Next floor
```

Target loop after Phase 7H:

```txt
Intro
BEGIN DESCENT
Floor starts
OBSERVATION PASS
Question appears
Player answers 5 trials
Doors closing
Descending
Next floor
OBSERVATION PASS
Question appears
...
```

Observation pass should happen:

```txt
at the start of Floor 5
after each descent when a new floor starts
after restart when a new run begins
```

Observation pass should not happen:

```txt
between every trial of the same floor
after every answer
after wrong answers
after timeouts
after win/loss end states
```

---

## Strict Scope

Included:

```txt
add a simple observation camera pass before trials
show/hide or soften question controls during observation
keep static clue board visible during observation
keep observation duration short and testable
add pure timing/state tests where possible
preserve all current gameplay rules
preserve EN/FR localization
preserve clue board behavior
update docs/roadmap/test plan if needed
run all EditMode tests
manual Play Mode check if possible
```

Excluded:

```txt
Cinemachine
new packages
final cinematic animation
complex camera rails
camera travelling per clue
interactive look controls
touch drag controls
free camera
procedural clue placement
world-space clue refactor
final art
new audio system
voice acting
jumpscare cinematic
enemy AI/pathfinding
mobile build
Unity Localization package
full evidence runtime replacement
```

---

## Recommended Implementation

Prefer a simple, deterministic implementation.

Possible components:

```txt
ObservationCameraPass
ObservationPassTiming
ObservationPassState
```

or equivalent.

The camera movement can be very simple:

```txt
start pose = current camera pose
observe pose = slightly forward / slightly higher / slightly more centered on corridor
return or settle pose = normal gameplay pose
```

Acceptable simpler alternative:

```txt
No physical camera movement if scene risk is too high.
Instead, add a timed OBSERVE THE CORRIDOR overlay and temporarily hide/disable answers.
```

But preferred:

```txt
Subtle camera move + timed observation overlay.
```

Do not overbuild.

Do not use Cinemachine.

Do not require new scene assets.

---

## Runtime Integration Guidance

Likely integration point:

```txt
PlayableRunFlowController.BeginFloor
```

Current Phase 7G added:

```txt
ui.UpdateClues(displayFloor)
```

Observation pass should occur after clues are updated and before the first trial of that floor becomes answerable.

Target order:

```txt
BeginFloor
Update clue board for current floor
Run observation pass
Show first trial
Enable answers/timer
```

Important:

```txt
During observation, the player should not be able to answer a trial.
Timer should not count down during observation.
Threat should not move during observation.
Trial count should not advance during observation.
```

After observation:

```txt
normal trial behavior resumes exactly as before.
```

---

## UI Requirements

During observation, show a short localized hint.

English examples:

```txt
OBSERVE THE CORRIDOR
Look carefully. The answers are already here.
```

French examples:

```txt
OBSERVE LE COULOIR
Regarde bien. Les réponses sont déjà là.
```

Use existing localization approach.

Do not create a settings menu.

Observation overlay should not permanently cover the board.

Acceptable behavior:

```txt
During observation:
- show overlay/hint
- clue board visible
- answer buttons hidden or disabled
- question either hidden or not yet shown
```

After observation:

```txt
- overlay hidden
- question shown
- answer buttons enabled
- clue board can remain visible
```

---

## Timing Requirements

Keep observation short for playtest.

Recommended values:

```txt
observationHoldSeconds = 2.0
cameraMoveSeconds = 0.6
cameraReturnSeconds = 0.4
```

If adding a config class:

```txt
ObservationPassTiming
```

Make it testable.

Avoid long unskippable sequences.

Optional but useful:

```txt
allow skip/tap/keyboard Enter to end observation early
```

Only add skip if it is simple and safe.

Do not let skip break state.

---

## Camera Requirements

If camera movement is implemented:

```txt
use existing Main Camera
store original local/world position and rotation
move to a subtle observe pose
return or settle before trial starts
```

Keep movement subtle.

Do not risk making the scene unreadable.

Do not permanently change the camera after observation unless intended and documented.

Mobile portrait readability matters more than cinematic ambition.

If the current scene setup makes camera movement risky, implement the overlay-only fallback and document it honestly.

---

## Gameplay Preservation Requirements

Must preserve:

```txt
descent loop
5 floors
5 trials per floor
25 playable trials
question localization EN/FR
static clue board EN/FR
threat non-receding
wrong advances threat
timeout advances threat strongly
correct does not move threat back
floor clear by surviving 5 trials
Ground Floor escape
restart after win/loss
timer behavior during trials
answer buttons during trials
Phase 6 feedback
Phase 7 pacing
```

Must not introduce:

```txt
answers possible during observation
timer countdown during observation
threat movement during observation
trial count increment during observation
double-question display
duplicate coroutines on restart
stuck state after skip
stuck state after descent
```

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
204 EditMode tests
```

Add tests for pure logic where possible.

Required test coverage:

```txt
observation pass occurs once per floor start
observation pass does not count as a trial
timer should not be active during observation
answers should not be active during observation
question becomes active after observation
floor count remains 5
trial count remains 5 per floor
restart can start observation again
localization text exists in English
localization text exists in French
observation timing values are positive
```

If the camera pass itself is MonoBehaviour/coroutine-heavy and hard to test, isolate testable logic in a pure class:

```txt
ObservationPassTiming
ObservationPassState
ObservationPassResolver
```

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Manual Play Mode Check

If possible, verify:

```txt
Game.unity opens
Game view portrait
Play Mode starts
intro appears
BEGIN DESCENT starts run
Floor 5 clue board appears
observation overlay appears before first question
answers are not clickable during observation
timer does not count down during observation
camera subtly moves or overlay-only fallback is visible
first question appears after observation
answers work normally after observation
wrong answer still advances threat
timeout still advances threat strongly
surviving Floor 5 triggers doors closing / descending
Floor 4 starts with a new observation pass
clue board updates to Floor 4
restart starts observation again
no red Console errors
```

If French is easy to test:

```txt
PrototypeLocalization.Language = GameLanguage.French
```

Expected:

```txt
OBSERVE LE COULOIR
Regarde bien. Les réponses sont déjà là.
INDICES OBSERVÉS
French prompts/answers/cues remain OK
```

If Play Mode is unavailable, report honestly.

---

## Documentation Updates

Update docs only as needed:

```txt
Docs/CORRIDOR_OBSERVATION_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Docs/PLAYTEST_NOTES.md
```

Document:

```txt
Phase 7H adds the first observation camera pass prototype.
It happens once per floor before trials.
Question answering is disabled during observation.
Timer/threat/trial progression do not advance during observation.
Static clue board remains the evidence bridge.
This is not final cinematic polish.
```

Do not rewrite docs from scratch.

---

## Git Rules

Do not use:

```bash
git add .
```

Do not commit generated files.

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
*.sln
*.slnx
*.csproj
*.user
.env
.env.local
/tmp/
test-results-editmode.xml
editmode-results.xml
test-run.log
mono_crash.*.json
```

Use targeted adds only.

Recommended add command:

```bash
git add UnityProject/Assets/Scripts/GameLoop \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Scripts/Questions \
        UnityProject/Assets/Tests/EditMode \
        Docs/CORRIDOR_OBSERVATION_DESIGN.md \
        Docs/TECH_ARCHITECTURE.md \
        Docs/ROADMAP.md \
        Docs/TEST_PLAN.md \
        Docs/DECISIONS.md \
        Docs/PLAYTEST_NOTES.md
```

If `Game.unity` changes, add it explicitly:

```bash
git add UnityProject/Assets/Scenes/Game.unity
```

If some paths are unchanged, omit them.

If `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🎥 feat(gameplay): add observation camera pass"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7H Observation Camera Pass Prototype

## Model used

Claude

## Task status

Choose exactly one:

- COMPLETED_AND_COMMITTED
- COMPLETED_NOT_COMMITTED
- BLOCKED
- FAILED

## Commit

If committed:

- Commit hash:
- Commit message:

If not committed:

- Commit hash: N/A
- Commit message: N/A
- Reason not committed:

## Summary

Short summary of what changed.

## Files changed

List every changed file:

- `path/to/file` — created/modified/deleted + short reason

## Observation pass confirmation

Confirm each item:

- Observation pass exists: yes/no
- Runs before first trial of each floor: yes/no
- Runs on Floor 5 start: yes/no
- Runs after descent to Floor 4/3/2/1: yes/no
- Does not run between every trial: yes/no
- Clue board visible during observation: yes/no
- Question hidden or inactive during observation: yes/no
- Answers disabled or hidden during observation: yes/no
- Timer paused/inactive during observation: yes/no
- Threat does not move during observation: yes/no
- Trial count does not advance during observation: yes/no
- Observation ends and first trial starts: yes/no
- Restart can trigger observation again: yes/no

## Camera / visual behavior

Choose one:

- SUBTLE_CAMERA_MOVE
- OVERLAY_ONLY_FALLBACK
- HYBRID
- OTHER

Then explain:

- what moves visually:
- duration:
- whether Main Camera is modified:
- whether Game.unity changed:
- whether any new package was added:
- whether Cinemachine was used:

## Localization confirmation

Confirm each item:

- English observation text exists: yes/no
- French observation text exists: yes/no
- Existing GameLanguage reused: yes/no
- Existing PrototypeLocalization.Language reused: yes/no
- Existing question localization preserved: yes/no
- Existing clue board localization preserved: yes/no

## Runtime integration

Explain:

- what component manages observation:
- how BeginFloor changed:
- how UI is disabled/enabled:
- how timer is prevented from running:
- how duplicate observations/coroutines are avoided:
- what happens on restart:
- what happens on win/loss:

## Gameplay preservation

Confirm each item:

- Descent loop preserved: yes/no
- 5 floors preserved: yes/no
- 5 trials per floor preserved: yes/no
- Threat non-receding preserved: yes/no
- Wrong advances threat: yes/no
- Timeout advances threat strongly: yes/no
- Correct does not move threat back: yes/no
- Floor clear by surviving 5 trials preserved: yes/no
- Ground Floor escape preserved: yes/no
- Static clue board preserved: yes/no
- Question localization preserved: yes/no
- Restart preserved: yes/no

## Documentation updates

List docs updated and why.

## Tests run

List exact commands or Unity Test Runner actions used.

If no tests were run, write exactly:

Tests were not run because Unity Editor / Unity Test Runner was unavailable in this environment.

## Test results

Use one of:

- PASS
- FAIL
- NOT_RUN

Then explain briefly.

## Manual Play Mode checks

List exact Play Mode checks performed.

If Play Mode was not checked, write exactly:

Play Mode was not manually verified because Unity Editor GUI was unavailable in this environment.

## Visual/play check instructions for user

Give precise instructions for the user to test the observation pass.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- expected Floor 5 observation behavior
- expected behavior after descent to Floor 4
- expected answer/timer behavior during observation
- expected EN behavior
- expected FR behavior if language is switched
- what feedback the user should report

## Git status

Paste the exact output of:

```bash
git status --short
```
````

If the output is empty, write:

```txt
<clean>
```

## Staged/generated file safety check

Paste the exact output of:

```bash
git status --short | grep -E "UnityProject/(Library|Temp|Logs|UserSettings|Build|Builds)|\.slnx|\.csproj|mono_crash"
```

If the output is empty, write:

```txt
<clean>
```

## Known limits

List anything incomplete, intentionally deferred, or risky.

## Next recommended action

Choose exactly one:

- READY_FOR_REVIEW
- READY_FOR_PLAYTEST
- NEEDS_FIX
- NEEDS_USER_ACTION
- SHOULD_REVERT

Then explain in one sentence.

````

Do not summarize freely outside this structure.

---

## Acceptance Criteria

Phase 7H is complete only if:

```txt
observation pass exists
observation pass runs once per floor before trials
answers are disabled/hidden during observation
timer does not run during observation
threat does not move during observation
trial count does not advance during observation
clue board remains visible during observation
normal trial flow resumes after observation
descent loop remains unchanged
question localization remains unchanged
static clue board remains unchanged
no Cinemachine or new package is added
no final cinematic/art/audio is added
all EditMode tests pass if Unity Test Runner is available
no generated Unity files are staged
agent final report is complete and written in French
````
