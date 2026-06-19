# Agent Prompt — Phase 7E Evidence Trial Data Model

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
This phase introduces the first technical foundation for evidence-based corridor trials. It must connect floor data, visible corridor clues, trial prompts, answer options and EN/FR localization without changing the scene or implementing camera travelling yet. Keep Claude for continuity and careful integration with the existing descent loop.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎮 feat(questions): add evidence trial data model
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

Current planned next layer:

```txt
Before trials, the player should observe the corridor.
Trials should ask about clues that were actually visible in the corridor.
No trial should feel like a random quiz question.
```

Latest completed phases:

```txt
Phase 7B.4 — Descent Loop, Intro Context & Localization Prep
Phase 7C — Documentation Alignment
Phase 7D — Corridor Observation & Evidence-Based Trials Design
```

Current known test status:

```txt
148/148 EditMode tests passed after Phase 7B.4.
Phase 7C and Phase 7D were documentation-only.
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
Docs/AgentPrompts/PHASE_7E_EVIDENCE_TRIAL_DATA_MODEL.md
Docs/AgentPrompts/PHASE_7D_CORRIDOR_OBSERVATION_DESIGN.md
Docs/AgentPrompts/PHASE_7C_DOCUMENTATION_ALIGNMENT.md
Docs/AgentPrompts/PHASE_7B4_DESCENT_LOOP_INTRO_LOCALIZATION.md
Skills/horror-game-design/SKILL.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Inspect current code:

```txt
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/DescentFloorProfile.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
UnityProject/Assets/Scripts/GameLoop/LocalizedText.cs
UnityProject/Assets/Scripts/GameLoop/GameLanguage.cs
UnityProject/Assets/Scripts/GameLoop/TrialFlowResolver.cs
UnityProject/Assets/Scripts/Questions/PrototypeFloorSet.cs
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code names, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Mission

Implement:

```txt
Phase 7E — Evidence Trial Data Model
```

This phase creates the data model for future corridor evidence-based trials.

The goal is to make it possible to represent:

```txt
a visible corridor clue
a clue type
a floor observation set
a trial linked to a specific clue
localized prompt and answers
a correct answer that is justified by the clue
validation that every trial references a real clue
```

This phase is mostly pure code + tests.

Do not implement camera travelling.

Do not implement visual clue placement in the scene.

Do not modify `Game.unity`.

Do not change the playable runtime flow unless a tiny adapter is necessary and fully tested.

Do not replace the whole question system yet unless strictly necessary.

---

## Design Intent

The future gameplay should move from:

```txt
questions exist by themselves
```

to:

```txt
questions are caused by visible corridor evidence
```

Official future rule:

```txt
No trial without a clue.
No correct answer without observable evidence.
```

A valid evidence trial should answer these questions:

```txt
What did the player see?
Where was it in the corridor?
What is the trial asking?
Which answer is correct?
Why is that answer fair?
Can the text be displayed in English and French?
```

---

## Strict Scope

Included:

```txt
create pure data types for corridor clues and evidence trials
create pure validation helpers
create a prototype evidence floor set for current 5-floor / 5-trial structure
connect every prototype trial to a clueId in data
support EN/FR localized prompt and answers in the model
add tests for clue/trial relationships and localization
update docs/roadmap/test plan only if needed to reflect implementation status
run all EditMode tests
commit with targeted git add
```

Excluded:

```txt
camera travelling
ObservationPhaseController runtime behavior
CorridorObservationController runtime behavior
visual clue GameObjects
scene changes
prefabs
materials
final art
real audio
jumpscare cinematic
new enemy AI/pathfinding
mobile build
Unity Localization package
large content rewrite
full polished EN/FR copywriting
```

---

## Required Data Model

Create small, pure, testable types.

Recommended folder:

```txt
UnityProject/Assets/Scripts/Questions/
```

Possible files:

```txt
CorridorClueType.cs
CorridorClue.cs
EvidenceAnswerOption.cs
EvidenceTrial.cs
FloorObservationSet.cs
EvidenceTrialValidator.cs
PrototypeEvidenceFloorSet.cs
```

You may adjust names if needed, but keep them clear and English.

### CorridorClueType

Create an enum for clue categories.

Recommended values:

```txt
DoorNumber
WallMessage
Symbol
LightState
ObjectPlacement
Anomaly
ColorCue
AudioProxy
ShadowOrSilhouette
DirectionInstruction
ScratchedCode
DoorState
```

Do not implement real audio. `AudioProxy` currently means a visual/text proxy for a future audio clue.

### CorridorClue

Represents one observable clue in the corridor.

Recommended fields/properties:

```txt
string Id
CorridorClueType Type
int FloorDisplayNumber
LocalizedText Label
LocalizedText Description
string VisualAnchor
string EvidenceValue
int DifficultyWeight
bool IsRequiredForTrial
```

Rules:

```txt
Id must not be empty.
EvidenceValue must not be empty.
FloorDisplayNumber should match the displayed floor, such as 5, 4, 3, 2, 1.
DifficultyWeight should be positive.
```

`VisualAnchor` can be a conceptual string for now, for example:

```txt
left_wall_near_door_504
right_panel_red_light
far_end_shadow
```

### EvidenceAnswerOption

Represents one possible answer.

Recommended fields/properties:

```txt
string Id
LocalizedText Text
bool IsCorrect
```

Rules:

```txt
Id must not be empty.
Text must have at least English.
Exactly one answer should be correct per trial.
```

### EvidenceTrial

Represents a trial/question linked to one corridor clue.

Recommended fields/properties:

```txt
string Id
string ClueId
LocalizedText Prompt
IReadOnlyList<EvidenceAnswerOption> Answers
float TimeLimitSeconds
int Difficulty
```

Rules:

```txt
Id must not be empty.
ClueId must reference an existing CorridorClue.
Prompt must have at least English.
There must be exactly 4 answers.
There must be exactly 1 correct answer.
TimeLimitSeconds must be > 0.
Difficulty must be positive.
```

### FloorObservationSet

Represents all clues and trials for one displayed floor.

Recommended fields/properties:

```txt
int FloorDisplayNumber
IReadOnlyList<CorridorClue> Clues
IReadOnlyList<EvidenceTrial> Trials
```

Rules:

```txt
FloorDisplayNumber should be 5, 4, 3, 2 or 1 for the prototype.
There must be at least 5 trials.
There must be enough clues for trials.
Every trial must reference an existing clue.
No duplicate clue IDs.
No duplicate trial IDs.
```

### EvidenceTrialValidator

Create pure validation helpers.

Recommended result type:

```txt
EvidenceValidationResult
```

or use a simple list of errors if already consistent with project style.

Validation should catch:

```txt
empty clue id
empty trial id
duplicate clue ids
duplicate trial ids
trial references missing clue
trial has not exactly 4 answers
trial has not exactly 1 correct answer
trial has invalid time limit
trial has invalid difficulty
clue has empty evidence value
floor has fewer than 5 trials
localized prompt missing English
localized answer missing English
```

Tests should assert specific validation failures where practical.

---

## Prototype Evidence Content

Create a prototype evidence-backed floor set matching the current structure:

```txt
5 floors
5 trials per floor
25 trials total
```

Recommended class:

```txt
PrototypeEvidenceFloorSet
```

It should expose something like:

```txt
IReadOnlyList<FloorObservationSet> Create()
```

or:

```txt
IReadOnlyList<FloorObservationSet> Floors
```

Each current trial should have a matching clue.

Example:

```txt
Floor 5:
Clue id: floor5_room_display_104
Type: DoorNumber
EvidenceValue: 104
VisualAnchor: elevator_display_top
Trial: Which room number blinked?
Correct answer: 104
```

Important:

```txt
Keep content prototype-level.
Do not over-polish copywriting.
Every trial must have clueId.
Every clue used by a trial must exist.
Every trial must have 4 answers.
Every trial must have exactly one correct answer.
Every prompt and answer should have English and French if practical.
```

If full French translation of all 25 trials is too large for this phase, do at least the model support and a representative subset, but report honestly.

Preferred for this phase:

```txt
Add EN and FR for all prototype prompts and answers if feasible.
```

But do not spend excessive time polishing French literary style. Simple, clear French is enough.

---

## Runtime Integration Guidance

Do not force the playable runtime to use `PrototypeEvidenceFloorSet` yet if that would create too much churn.

Acceptable options:

### Option A — Data model only

Create `PrototypeEvidenceFloorSet` and tests, but leave `PlayableRunFlowController` / `PrototypeFloorSet` using the current data.

Pros:

```txt
low risk
clean foundation
no gameplay regression
```

Cons:

```txt
evidence model exists but is not active yet
```

### Option B — Safe adapter

Add a pure adapter that converts `EvidenceTrial` to existing `QuestionData` + `QuestionCue`.

Pros:

```txt
runtime can use evidence-backed data soon
```

Cons:

```txt
more integration risk
```

Recommended:

```txt
Option A unless Option B is clearly simple and safe.
```

Do not change Play Mode behavior in this phase unless you can keep tests passing and explain clearly.

---

## Localization Requirements

Use the existing localization types from Phase 7B.4:

```txt
GameLanguage
LocalizedText
PrototypeLocalization
```

Do not introduce a large localization framework.

For evidence data:

```txt
Prompt should be LocalizedText.
Answer text should be LocalizedText.
Clue label/description should be LocalizedText.
Code identifiers remain English.
```

Required tests:

```txt
English prompt returns English.
French prompt returns French.
English answer returns English.
French answer returns French.
Missing or empty required English text is invalid.
```

---

## Documentation Updates

Update docs only as needed.

Recommended updates:

```txt
Docs/CORRIDOR_OBSERVATION_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
```

Do not rewrite docs from scratch.

Make targeted updates:

```txt
Phase 7E is now implementation of the evidence trial data model.
Camera travelling remains planned, not implemented.
Static visual clue prototype remains future.
Runtime trial flow may still use old PrototypeFloorSet unless integrated explicitly.
```

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
148 EditMode tests
```

Add tests for the new pure data model.

Required test coverage:

```txt
CorridorClue stores id/type/floor/evidence value
EvidenceTrial stores clueId/prompt/answers/time limit
EvidenceTrial requires exactly 4 answers
EvidenceTrial requires exactly 1 correct answer
FloorObservationSet rejects duplicate clue ids
FloorObservationSet rejects duplicate trial ids
validator rejects missing clue reference
validator rejects empty evidence value
validator rejects invalid time limit
validator rejects invalid difficulty
validator rejects missing English prompt
PrototypeEvidenceFloorSet has 5 floors
each prototype floor has 5 trials
prototype has 25 total trials
every prototype trial references an existing clue
every prototype trial has 4 answers
every prototype trial has exactly 1 correct answer
every prototype trial prompt supports EN/FR if implemented
every prototype answer supports EN/FR if implemented
```

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Manual Play Mode

Manual Play Mode is not required for this phase if there is no runtime integration.

If runtime is changed, perform a smoke Play Mode check if possible:

```txt
Game.unity opens
intro appears
BEGIN DESCENT works
Floor 5 starts
trials still display
answers still work
no Console errors
```

If Play Mode is unavailable, report honestly.

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
git add UnityProject/Assets/Scripts/Questions \
        UnityProject/Assets/Tests/EditMode \
        Docs/CORRIDOR_OBSERVATION_DESIGN.md \
        Docs/TECH_ARCHITECTURE.md \
        Docs/ROADMAP.md \
        Docs/TEST_PLAN.md \
        Docs/DECISIONS.md
```

If some docs are unchanged, omit them.

If `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🎮 feat(questions): add evidence trial data model"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7E Evidence Trial Data Model

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

## Data model confirmation

Confirm each item:

- CorridorClueType exists: yes/no
- CorridorClue exists: yes/no
- EvidenceAnswerOption exists: yes/no
- EvidenceTrial exists: yes/no
- FloorObservationSet exists: yes/no
- EvidenceTrialValidator exists: yes/no
- PrototypeEvidenceFloorSet exists: yes/no
- Validation result structure exists: yes/no
- Data model is pure/testable: yes/no

## Evidence rules confirmation

Confirm each item:

- Trials reference clueId: yes/no
- Validator rejects missing clue references: yes/no
- Validator rejects duplicate clue ids: yes/no
- Validator rejects duplicate trial ids: yes/no
- Validator requires exactly 4 answers: yes/no
- Validator requires exactly 1 correct answer: yes/no
- Validator rejects empty evidence value: yes/no
- Validator rejects invalid time limit: yes/no
- Validator rejects invalid difficulty: yes/no
- Validator checks required English text: yes/no

## Prototype evidence content confirmation

Confirm each item:

- Prototype evidence set has 5 floors: yes/no
- Each floor has 5 trials: yes/no
- Prototype has 25 trials total: yes/no
- Every prototype trial references an existing clue: yes/no
- Every prototype trial has 4 answers: yes/no
- Every prototype trial has exactly 1 correct answer: yes/no
- Every prototype clue has evidence value: yes/no
- English prompts exist: yes/no
- French prompts exist: yes/no/partial
- English answers exist: yes/no
- French answers exist: yes/no/partial

## Runtime integration

Choose one:

- DATA_MODEL_ONLY
- SAFE_ADAPTER_ADDED
- RUNTIME_NOW_USES_EVIDENCE_DATA

Then explain:

- what runtime uses now:
- whether PlayableRunFlowController changed:
- whether PrototypeFloorSet changed:
- whether Play Mode behavior changed:

## Localization confirmation

Confirm each item:

- Existing LocalizedText reused: yes/no
- Existing GameLanguage reused: yes/no
- Prompt localization supported: yes/no
- Answer localization supported: yes/no
- Clue label localization supported: yes/no
- Clue description localization supported: yes/no
- EN/FR test coverage exists: yes/no/partial

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

Play Mode was not manually verified because this phase did not change runtime gameplay, or Unity Editor GUI was unavailable in this environment.

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

Phase 7E is complete only if:

```txt
pure evidence trial data model exists
prototype evidence floor set exists
5 floors are represented
5 trials per floor are represented
25 evidence trials total are represented
every trial references a corridor clue
every referenced clue exists
validator catches missing clue references
validator catches duplicate IDs
validator catches invalid answer counts
validator catches invalid correct answer counts
validator catches missing evidence values
validator catches invalid time/difficulty
localized prompt/answer model supports EN/FR
tests cover prototype evidence set integrity
tests cover validator failure cases
all EditMode tests pass if Unity Test Runner is available
no camera travelling is implemented
no scene/art changes are made
runtime gameplay is unchanged unless explicitly and safely adapted
no generated Unity files are staged
agent final report is complete and written in French
````
