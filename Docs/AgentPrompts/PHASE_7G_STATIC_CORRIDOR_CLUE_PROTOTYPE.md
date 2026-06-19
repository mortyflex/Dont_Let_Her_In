# Agent Prompt — Phase 7G Static Corridor Clue Prototype

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
This phase introduces the first visible corridor clue prototype. It touches scene readability, UI/world text, evidence data, and playtest flow. It must stay scoped: static clues only, no travelling camera yet, no final art. Keep Claude for continuity and careful integration with the current descent loop and evidence model.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎮 feat(scene): add static corridor clue prototype
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

Current evidence direction:

```txt
Before trials, the player should observe corridor details.
Trials should be linked to visible corridor clues.
No trial should feel like a random quiz question.
```

Latest completed phases:

```txt
Phase 7E — Evidence Trial Data Model
Phase 7F — Question Content Localization EN/FR
```

Current known test status:

```txt
189/189 EditMode tests passed after Phase 7F.
Phase 7F Play Mode French smoke was user-validated:
- no clipped text
- accents OK
- French answers short enough
- cues readable
- no red Console errors
- correct answers still work after language switch
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
Docs/AgentPrompts/PHASE_7G_STATIC_CORRIDOR_CLUE_PROTOTYPE.md
Docs/AgentPrompts/PHASE_7F_QUESTION_CONTENT_LOCALIZATION.md
Docs/AgentPrompts/PHASE_7E_EVIDENCE_TRIAL_DATA_MODEL.md
Docs/AgentPrompts/PHASE_7D_CORRIDOR_OBSERVATION_DESIGN.md
Docs/AgentPrompts/PHASE_7B4_DESCENT_LOOP_INTRO_LOCALIZATION.md
Skills/horror-game-design/SKILL.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Inspect current code and scene:

```txt
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/DescentFloorProfile.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
UnityProject/Assets/Scripts/GameLoop/LocalizedText.cs
UnityProject/Assets/Scripts/GameLoop/GameLanguage.cs
UnityProject/Assets/Scripts/Questions/PrototypeFloorSet.cs
UnityProject/Assets/Scripts/Questions/PrototypeEvidenceFloorSet.cs
UnityProject/Assets/Scripts/Questions/CorridorClue.cs
UnityProject/Assets/Scripts/Questions/FloorObservationSet.cs
UnityProject/Assets/Scripts/Questions/EvidenceTrial.cs
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
Phase 7G — Static Corridor Clue Prototype
```

The goal is to make the corridor visibly contain static clue placeholders that correspond to the current playable trial content.

This is the first visual/evidence bridge.

The player should start to understand:

```txt
The answers come from the corridor.
The corridor contains readable details.
Each floor has different visible clues.
The hallway is mostly the same structure, but details change.
```

Important:

```txt
Do not implement camera travelling yet.
Do not implement procedural clue generation.
Do not implement final art.
Do not redesign the whole scene.
Do not replace the current gameplay loop.
```

---

## Strict Scope

Included:

```txt
add static corridor clue placeholders to the existing Game.unity scene
create simple clue display components if useful
show floor-specific clue text/markers in the corridor
make visible clues correspond to the current floor's playable trials
keep all clues readable in mobile portrait
keep the corridor/horror composition usable
keep gameplay behavior unchanged
preserve descent loop
preserve localization
add tests for pure clue mapping/display data where possible
update docs/roadmap/test plan if needed
run all EditMode tests
manual Play Mode check if possible
```

Excluded:

```txt
camera travelling
ObservationPhaseController runtime flow
CorridorObservationController runtime flow if complex
procedural clue placement
final corridor art
new scene art pass beyond minimal readable clue placeholders
complex animations
real audio
music
voice acting
jumpscare cinematic
new enemy AI/pathfinding
Cinemachine
post-processing package changes
mobile build
Unity Localization package
large runtime evidence integration
```

---

## Design Intent

The corridor should remain mostly consistent.

The clues should be simple prototype placeholders, such as:

```txt
door number plates
wall messages
symbols
light labels
warning text
scratched codes
small object labels
anomaly markers
```

This phase is not about beauty.

It is about making the evidence loop visible.

The player should be able to say:

```txt
I saw that number / symbol / message in the hallway.
That is why I know the answer.
```

---

## Visual Direction

Keep the current elevator/corridor greybox.

Add readable clue placeholders.

Acceptable visual forms:

```txt
world-space TextMeshPro labels if TMP is already available
Unity UI Text elements placed as world/canvas overlays if existing setup supports it
simple colored/emissive planes with text-like labels
simple primitive objects with labels
```

Do not install new packages.

Do not require final fonts.

Do not use complex art assets.

Keep the corridor visible and not cluttered.

---

## Floor-Specific Clue Requirement

The prototype has:

```txt
Floor 5
Floor 4
Floor 3
Floor 2
Floor 1
```

Each floor should display a set of static clues matching that floor's 5 playable trials.

At minimum:

```txt
5 clue placeholders per floor
```

Only the current floor's clues should be visible during that floor.

When descending to the next floor:

```txt
old floor clues hide
new floor clues appear
```

If fully dynamic per-floor visibility is too risky for this phase, acceptable fallback:

```txt
display a small current-floor clue board in the corridor/elevator view
update its text according to the current floor
```

But preferred:

```txt
static clue placeholders in corridor space, updated/visible per floor
```

---

## Important Runtime Behavior

The current playable runtime uses:

```txt
PrototypeFloorSet
```

The evidence data model exists as:

```txt
PrototypeEvidenceFloorSet
```

Recommended integration for this phase:

```txt
Do not force full runtime evidence trial replacement.
Use a lightweight display mapping from current floor display number to clue text.
The playable questions can still come from PrototypeFloorSet.
The clue display should be consistent with those questions.
```

Acceptable approaches:

### Option A — Static clue board

Create a simple component such as:

```txt
CorridorClueBoard
```

It shows 5 clue lines for the current floor.

Pros:

```txt
low scene risk
easy to read
fast to validate
clearly ties questions to visible evidence
```

Cons:

```txt
less immersive than placing individual clues on doors/walls
```

### Option B — Multiple static clue anchors

Create several simple clue anchor objects in the corridor, each with a label, and update their content per floor.

Possible components:

```txt
CorridorClueDisplay
CorridorClueDisplaySet
```

Pros:

```txt
closer to future observation gameplay
```

Cons:

```txt
more scene layout risk
```

Recommended:

```txt
Option A or a very simple Option B.
```

Do not overbuild.

---

## Data Source For Clues

Preferred source:

```txt
PrototypeEvidenceFloorSet
```

Reason:

```txt
It already links floors, clues and evidence values.
```

Use it to populate clue display text where practical.

However, do not force gameplay trials to switch to evidence runtime yet.

The clue display can read from:

```txt
PrototypeEvidenceFloorSet
```

while the playable questions still come from:

```txt
PrototypeFloorSet
```

If this creates mismatch, fix the data mapping, not the gameplay loop.

---

## Localization Requirement

The clue display should respect:

```txt
PrototypeLocalization.Language
```

When English:

```txt
clue labels/descriptions should display English
```

When French:

```txt
clue labels/descriptions should display French
```

If only evidence labels/descriptions are localized and not all display text is perfect, report honestly.

Do not add a language settings UI.

---

## Required Scene/Runtime Changes

Add minimal components as needed.

Possible files:

```txt
UnityProject/Assets/Scripts/Questions/CorridorClueDisplayData.cs
UnityProject/Assets/Scripts/Questions/CorridorClueDisplayFormatter.cs
UnityProject/Assets/Scripts/UI/CorridorClueBoard.cs
UnityProject/Assets/Scripts/UI/CorridorClueDisplay.cs
```

or equivalent.

Possible integration point:

```txt
PlayableRunFlowController.BeginFloor
```

or when current trial/floor starts.

The clue display should update when:

```txt
run begins
new floor begins
restart begins
language is changed before run
```

Do not make it depend on timing/coroutines if unnecessary.

---

## UI / Scene Requirements

The clue display must not cover:

```txt
answer buttons
timer
core HUD
creature view
main corridor readability
```

If using a clue board, place it where it is clearly a prototype board and not final UI.

Acceptable label:

```txt
OBSERVED CLUES
```

French:

```txt
INDICES OBSERVÉS
```

But keep player-facing text minimal.

If the board is too UI-like, it is acceptable for this prototype as long as it proves the evidence relationship.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
189 EditMode tests
```

Add tests for pure clue mapping/formatting if possible.

Required test coverage:

```txt
current floor maps to 5 clue display entries
Floor 5 clues exist
Floor 4 clues exist
Floor 3 clues exist
Floor 2 clues exist
Floor 1 clues exist
clue display entries use PrototypeEvidenceFloorSet data
English clue display returns English
French clue display returns French
switching language does not change clue count
missing floor returns empty or safe fallback
display formatter does not return null text
```

If a MonoBehaviour board is added, keep most logic pure and testable via formatter/service.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Manual Play Mode Check

If possible, verify:

```txt
Game.unity opens
Play Mode starts
intro appears
BEGIN DESCENT starts run
Floor 5 shows visible clue placeholders
Floor 5 clue placeholders match Floor 5 questions
answers still work
wrong/timeout still work
descend to Floor 4
Floor 5 clues hide/update
Floor 4 clues appear/update
English default displays English clues
French language switch displays French clues if tested
no red Console errors
corridor remains readable
clue display does not cover answer buttons
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
Phase 7G adds static corridor clue prototype.
Observation camera pass remains future.
Evidence data model exists.
Runtime gameplay still uses PrototypeFloorSet unless changed.
Clue display is a prototype bridge, not final art.
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
git add UnityProject/Assets/Scenes/Game.unity \
        UnityProject/Assets/Scripts/Questions \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Tests/EditMode \
        Docs/CORRIDOR_OBSERVATION_DESIGN.md \
        Docs/TECH_ARCHITECTURE.md \
        Docs/ROADMAP.md \
        Docs/TEST_PLAN.md \
        Docs/DECISIONS.md \
        Docs/PLAYTEST_NOTES.md
```

If some paths are unchanged, omit them.

If `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🎮 feat(scene): add static corridor clue prototype"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7G Static Corridor Clue Prototype

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

## Static clue prototype confirmation

Confirm each item:

- Static clue display exists: yes/no
- Current floor displays clues: yes/no
- Floor 5 clues display: yes/no
- Floor 4 clues display: yes/no
- Floor 3 clues display: yes/no
- Floor 2 clues display: yes/no
- Floor 1 clues display: yes/no
- Old floor clues hide/update on descent: yes/no
- Clues correspond to playable trial content: yes/no
- Clues use evidence data model where practical: yes/no
- Corridor remains readable: yes/no
- No travelling camera added: yes/no

## Localization confirmation

Confirm each item:

- English clue display exists: yes/no
- French clue display exists: yes/no
- Existing GameLanguage reused: yes/no
- Existing PrototypeLocalization.Language reused: yes/no
- Switching language preserves clue count: yes/no

## Runtime integration

Choose one:

- CLUE_BOARD
- STATIC_ANCHORS
- HYBRID
- OTHER

Then explain:

- what component displays clues:
- what data source it uses:
- how it updates per floor:
- whether PlayableRunFlowController changed:
- whether PrototypeFloorSet changed:
- whether PrototypeEvidenceFloorSet changed:
- whether Game.unity changed:

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

Give precise instructions for the user to test static corridor clues.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- expected Floor 5 clue behavior
- expected Floor 4 clue behavior after descent
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

Phase 7G is complete only if:

```txt
static corridor clue prototype exists
current floor can show at least 5 clue entries
Floor 5/4/3/2/1 have clue display data
clues correspond to playable trial content
clues use PrototypeEvidenceFloorSet or equivalent evidence data where practical
old floor clues hide/update on descent
clue display supports EN/FR
clue display is readable in portrait
corridor remains readable
gameplay behavior is unchanged
no camera travelling is implemented
no final art/audio/AI/pathfinding is added
all EditMode tests pass if Unity Test Runner is available
no generated Unity files are staged
agent final report is complete and written in French
````
