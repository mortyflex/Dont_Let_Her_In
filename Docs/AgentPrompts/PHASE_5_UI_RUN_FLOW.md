# Agent Prompt — Phase 5 UI and Run Flow

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
This phase connects the existing gameplay logic, question system, threat system, creature controller and Unity scene UI. It is integration-heavy and scene-sensitive, so keep Claude for continuity and caution.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎮 feat(gameplay): connect playable question flow
```

---

## Project

You are working on the Unity project:

```txt
Don’t Let Her In
```

This is a Unity 6 URP iOS-first portrait horror prototype.

The player is trapped inside an elevator looking out into a dark corridor. A female-like creature approaches while the player answers short survival questions.

Main promise:

```txt
Every second of hesitation brings her closer.
```

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/AgentPrompts/PHASE_5_UI_RUN_FLOW.md
Docs/ROADMAP.md
Docs/GAME_DESIGN.md
Docs/ART_DIRECTION.md
Docs/TECH_ARCHITECTURE.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
UnityProject/Assets/References/Visuals/README.md
Skills/unity-gameplay-loop/SKILL.md
Skills/horror-game-design/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-mobile-performance/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Also inspect the current implementations:

```txt
UnityProject/Assets/Scripts/Core/
UnityProject/Assets/Scripts/GameLoop/
UnityProject/Assets/Scripts/Threat/
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Scripts/Creature/
UnityProject/Assets/Scripts/UI/
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Current Project State

Previous core commits:

```txt
6dd6b73 — 🎮 feat(gameplay): add core threat run loop
109dcfd — 🎮 feat(questions): add data-driven question system
882eb40 — 👻 feat(creature): add distance-based hallway threat
99da6c1 — 🛗 feat(scene): add elevator corridor prototype
1d3efce — 🎨 art(scene): improve placeholder horror readability
f07774d — 🎨 fix(scene): rebuild readable horror placeholder
b6011df — 🎨 fix(scene): recover camera composition readability
80f7d8c — 🎨 fix(scene): polish portrait horror framing
```

Important note:

```txt
A later Phase 4F visual attempt was reverted by the user and should not be considered part of the current state.
```

Current test status before this phase:

```txt
76 EditMode tests passed
```

Current scene:

```txt
UnityProject/Assets/Scenes/Game.unity
```

Current visual state:

```txt
The scene is an acceptable temporary greybox, not final art.
Do not spend this phase trying to beautify the scene.
Focus on making the prototype playable.
```

---

## Mission

Implement:

```txt
Phase 5 — UI and Run Flow
```

The goal is to make the prototype playable in Unity Play Mode.

The user should be able to:

```txt
open Game.unity
enter Play Mode
press Start
see a question
see a timer
tap/click an answer
see the result affect threat distance
see the creature move to a distance phase/anchor
continue through a short run
win or lose
restart
```

This is the first playable vertical slice.

This phase is about functionality, not visual polish.

---

## Strict Scope

Included:

```txt
minimal in-scene UI
start button
question text
answer buttons
timer display or timer bar
basic status/result text
distance/threat debug text if useful
win state
loss state
restart button
basic wiring to QuestionManager
basic wiring to ThreatManager
basic wiring to RunController
basic wiring to CreatureController
small runtime driver MonoBehaviour
small sample question content for the prototype
EditMode tests for any new pure logic
all existing EditMode tests still pass
```

Excluded:

```txt
final UI design
advanced UI animations
final typography
final art
new scene art pass
audio
jumpscare cinematic
advanced lighting
real mobile build/export
iOS Xcode build
VR/XR
Android-specific work
monetization
analytics
cloud save
online features
procedural generation
inventory
free movement
enemy AI
pathfinding
multiple creatures
```

---

## Required Playable Flow

Implement a simple flow:

```txt
1. Scene loads.
2. Start screen/panel is visible.
3. User presses Start.
4. Run starts at floor/question 1.
5. Question text and answers appear.
6. Timer counts down.
7. User selects an answer OR timer reaches zero.
8. AnswerResult is produced.
9. ThreatManager is updated according to answer outcome:
   - correct fast
   - correct normal
   - correct slow
   - wrong answer
   - timeout
10. CreatureController receives updated distance and moves/updates phase.
11. Status text briefly shows result.
12. Next question/floor begins.
13. If distance <= 0, run is lost.
14. If final question/floor is completed, run is won.
15. Restart returns to initial state.
```

Keep transitions simple.

No animation is required.

---

## Required Prototype Content

Create a small question set for the playable prototype.

Use 5 questions based on the design docs.

The content can be created either as:

```txt
QuestionData ScriptableObject assets
```

or as:

```txt
a simple serialized list / scene-bound sample provider
```

Preferred if safe:

```txt
Create QuestionData assets under UnityProject/Assets/ScriptableObjects/Questions/
```

Suggested questions:

```txt
Floor 1 — Observation
Prompt: Which room number blinked?
Answers: 101, 104, 108, 102
Correct: 104
Time limit: 8 seconds

Floor 2 — Short Memory
Prompt: Which symbol was in the center?
Answers: Eye, Key, Hand, Door
Correct: Key
Time limit: 7 seconds

Floor 3 — Environmental Instruction
Prompt: What did the wall say?
Answers: Do not run, Do not look left, Do not answer, Do not lie
Correct: Do not look left
Time limit: 6 seconds

Floor 4 — Audio Clue Placeholder
Prompt: Which sequence did the voice repeat?
Answers: 272, 227, 722, 277
Correct: 272
Time limit: 5 seconds

Floor 5 — Sang-Froid
Prompt: The elevator says PRESS EXIT NOW, the wall says WAIT. What do you do?
Answers: Press exit, Wait, Open doors, Look away
Correct: Wait
Time limit: 4 seconds
```

Audio itself is not required in this phase.

For the audio clue question, text placeholder is acceptable.

---

## Required UI

Create a simple mobile portrait UI under the existing scene hierarchy:

```txt
SceneRoot
  UI
```

Required UI elements:

```txt
Start panel
Question panel
Question text
Timer text or timer bar
4 answer buttons
Status/result text
Threat distance text or debug label
Win/Loss panel
Restart button
```

The UI can be basic.

It must be readable in portrait.

Use Unity UI / Canvas.

Do not make final design.

Do not spend time making it beautiful.

Suggested style:

```txt
dark translucent panel
light text
red/green feedback text
large touch-friendly buttons
bottom or lower-middle question area
```

Keep it simple.

---

## Required Runtime Wiring

Create or update minimal runtime scripts to connect existing systems.

Suggested files:

```txt
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
```

Alternative names are acceptable if clear.

The runtime flow should:

```txt
own or reference a RunController
own or reference a ThreatManager
own or reference a QuestionManager
reference the CreatureController in the scene
reference UI controller
load or hold the prototype questions
start run
advance questions
handle answer button click
handle timeout
apply threat outcome
update creature distance
handle win/loss
restart
```

Keep responsibilities clear:

```txt
PlayableRunFlowController = orchestration
GameplayUIController = view/UI references and display
QuestionEvaluator / QuestionManager = question logic
ThreatManager = threat rules
CreatureController = visual distance state
RunController = run progression
```

Do not turn GameManager into a giant god object.

Do not duplicate logic already implemented in Phase 1–3.

---

## Threat Application Rules

Map `AnswerResult` to existing `ThreatManager` methods.

Expected behavior:

```txt
correct + Fast => CorrectFast
correct + Normal => CorrectNormal
correct + Slow => CorrectSlow
wrong => WrongAnswer
timeout => Timeout
```

If exact method names differ, inspect the existing implementation and use the appropriate methods.

Do not rewrite `ThreatManager`.

Do not change Phase 1 threat constants unless absolutely necessary.

---

## Creature Update Rules

After every threat update:

```txt
read current threat distance
send/apply it to CreatureController
CreatureController updates CreaturePhase and anchor position
```

Do not create AI.

Do not create pathfinding.

Creature remains distance-driven.

---

## Timer Rules

Use the existing `QuestionManager` / `QuestionEvaluator` behavior.

Expected:

```txt
timer starts when question starts
timer decreases in Update
if timer reaches zero, resolve timeout once
after answer or timeout, question is no longer active
```

Keep timing simple and deterministic.

---

## Scene Wiring Rules

Modify:

```txt
UnityProject/Assets/Scenes/Game.unity
```

Only as needed to add UI and wire components.

Do not rebuild the scene visuals.

Do not run another art pass.

Do not move the camera/geometry unless absolutely necessary for UI visibility.

Do not replace the current scene.

---

## Required Tests

Run all EditMode tests.

Expected baseline:

```txt
76 EditMode tests
```

Add EditMode tests only for new pure logic if you introduce any.

Optional tests:

```txt
PlayableRunFlowController pure helper tests if helper logic is separated
```

Do not add fragile PlayMode tests unless simple and reliable.

Manual Play Mode verification is important in this phase.

---

## Required Manual Play Mode Check

After implementation, verify in Unity if possible:

```txt
Game.unity opens
Play Mode starts
Start button works
Question appears
Timer counts down
Answer buttons work
Correct answer changes threat distance correctly
Wrong answer changes threat distance correctly
Timeout changes threat distance correctly
Creature moves/phase updates after answer
Run can be won
Run can be lost
Restart works
No blocking console errors
```

If the agent cannot use Unity GUI/Play Mode, it must report that honestly and rely on batch import/tests only.

The user will do the final Play Mode review.

---

## Important Visual Rule

Do not try to improve the scene art in this phase.

The current greybox is accepted as temporary.

This phase should not create another visual correction loop.

Only UI and gameplay flow matter.

---

## Git Rules

Do not use:

```bash
git add .
```

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
test-run.log
mono_crash.*.json
```

Use targeted adds only.

Recommended add command:

```bash
git add UnityProject/Assets/Scripts/GameLoop UnityProject/Assets/Scripts/UI UnityProject/Assets/ScriptableObjects/Questions UnityProject/Assets/Scenes/Game.unity UnityProject/Assets/Tests/EditMode
```

If no tests or assets are added, omit unnecessary paths.

If any `.meta` files are created, include them with their asset/script.

Recommended commit message:

```bash
git commit -m "🎮 feat(gameplay): connect playable question flow"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 5 UI and Run Flow

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

## Playable flow implemented

Confirm each item:

- Start button: yes/no
- Question display: yes/no
- Timer: yes/no
- Answer buttons: yes/no
- Correct answer handling: yes/no
- Wrong answer handling: yes/no
- Timeout handling: yes/no
- Threat distance update: yes/no
- Creature distance/phase update: yes/no
- Win state: yes/no
- Loss state: yes/no
- Restart: yes/no

## Scene/UI wiring

Explain what was wired in `Game.unity`:

- UI:
- Runtime controllers:
- Question content:
- Threat:
- Creature:
- Run flow:

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Scene art pass added: yes/no
- Audio added: yes/no
- iOS build generated: yes/no
- VR/XR added: yes/no
- Android-specific work added: yes/no
- Monetization added: yes/no
- Analytics/cloud/online added: yes/no
- Generated Unity folders staged: yes/no

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

If the agent could not run Play Mode, write:

Play Mode was not manually verified because Unity Editor GUI was unavailable in this environment.

## Structural checks

List structural/import checks performed.

## Visual/play check instructions for user

Give precise instructions for the user to open Unity and test the playable prototype.

Include:

- which scene to open
- what Game view aspect/resolution to use
- whether to enter Play Mode
- exact test steps
- what the user should see
- what the user should not expect yet
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

List anything incomplete, unverified, or risky.

## Next recommended action

Choose exactly one:

- READY_FOR_PLAYTEST
- READY_FOR_REVIEW
- NEEDS_FIX
- NEEDS_USER_ACTION
- SHOULD_REVERT

Then explain in one sentence.

````

Do not summarize freely outside this structure.

---

## Acceptance Criteria

Phase 5 is complete only if:

```txt
Game.unity remains the main scene
Play Mode can start
Start button exists
Question appears
Timer appears and counts down
4 answer buttons exist
Answer selection produces AnswerResult
Correct/wrong/timeout affect ThreatManager
CreatureController receives updated threat distance
Creature moves/updates phase based on distance
Win state exists
Loss state exists
Restart works
Prototype questions exist
UI is readable in portrait
No final UI design added
No final art pass added
No audio added
No monetization/analytics/cloud/online added
Existing EditMode tests still pass if Unity Test Runner is available
No forbidden generated folders staged
Agent final report is complete and written in French
User can playtest the flow in Unity
````
