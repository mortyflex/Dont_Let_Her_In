# Agent Prompt — Phase 7H.1 Observation Pass Tuning

## Recommended Model

Recommended model:

```txt
Claude
```

Model switch recommendation:

```txt
Do not switch models for this correction phase.
```

Reason:

```txt
This is a focused correction on the Phase 7H observation pass: camera distance, camera timing, and clue board visibility. It touches live runtime flow and UI visibility. Keep Claude for continuity.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
🎥 tune(gameplay): improve observation pass readability
```

---

## Project

You are working on the Unity project:

```txt
Don’t Let Her In
```

Unity 6 URP, iOS-first portrait horror prototype.

Current state:

```txt
Phase 7H added an observation pass before the first trial of each floor.
It uses a localized overlay and a subtle Main Camera movement.
The implementation is committed and tests passed: 219/219 EditMode.
```

User playtest feedback:

```txt
The camera movement exists, but it is too short.
It should be slightly slower.
It should move farther into the corridor, ideally toward the red light at the end.
The clue board should only be visible during the observation/travelling camera phase.
When questions start, the clue board should disappear.
This better reflects the intended game: clues are visible during observation, then the player answers from memory.
Some clues may later remain visible from the elevator, but not in this prototype.
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
Docs/AgentPrompts/PHASE_7H1_OBSERVATION_PASS_TUNING.md
Docs/AgentPrompts/PHASE_7H_OBSERVATION_CAMERA_PASS.md
Docs/AgentPrompts/PHASE_7G_STATIC_CORRIDOR_CLUE_PROTOTYPE.md
Docs/AgentPrompts/PHASE_7F_QUESTION_CONTENT_LOCALIZATION.md
Skills/horror-game-design/SKILL.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-mobile-performance/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Inspect current code:

```txt
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/ObservationPassTiming.cs
UnityProject/Assets/Scripts/GameLoop/ObservationPassState.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Scripts/Questions/CorridorClueDisplayFormatter.cs
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code names, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Mission

Implement:

```txt
Phase 7H.1 — Observation Pass Tuning
```

This is a focused correction based on immediate user playtest feedback.

Do not redesign the system.

Do not add new gameplay rules.

Do not add new packages.

Do not edit final art.

---

## Required Changes

### 1. Make the camera observation movement slower

Current movement feels too short.

Tune timing to feel more like an observation pass.

Recommended values:

```txt
cameraMoveSeconds: from 0.6 to around 1.2
observationHoldSeconds: from 2.0 to around 2.2 or 2.5
cameraReturnSeconds: from 0.4 to around 0.7
```

The total pass should feel short but readable:

```txt
around 4 seconds total
```

Do not make it painfully long.

---

### 2. Move the camera farther into the corridor

Current movement is too subtle.

The camera should travel farther toward the end of the corridor / red light.

Current reported movement:

```txt
forward +0.2m
height +0.05m
```

Recommended new movement:

```txt
forward around +1.2m to +1.8m
height around +0.05m to +0.15m
```

Choose the safest values after inspecting the scene/camera axes.

Important:

```txt
Do not rotate wildly.
Do not clip through walls if avoidable.
Do not break portrait readability.
Do not permanently leave the camera forward after observation.
Camera should return to normal gameplay pose before questions start.
```

If the current axis makes “forward” ambiguous, inspect the scene and use the direction that visually moves toward the corridor/red light.

---

### 3. Show the clue board only during observation

Current Phase 7G/7H keeps the clue board visible during questions.

Change behavior:

```txt
During observation:
- clue board visible
- observation overlay visible
- question hidden/inactive
- answers hidden/inactive
- timer inactive

After observation / when first question starts:
- clue board hidden
- observation overlay hidden
- question visible
- answers visible
- timer active
```

This is important for design:

```txt
The player observes clues during the camera pass.
Then they answer from memory.
```

For now, do not implement partial always-visible clues.

Do not implement world-space clues yet.

---

### 4. Preserve per-floor update behavior

The clue board should still update per floor.

Expected flow:

```txt
BeginFloor
Update clue board content for current floor
Show clue board during observation
Hide clue board when question starts
```

After descent:

```txt
Floor 4 starts
clue board content updates to Floor 4
board is visible during observation
board hides when Floor 4 first question begins
```

On restart:

```txt
run restarts
Floor 5 clue board content is ready
board visible during observation
board hidden during question phase
```

---

## Strict Scope

Included:

```txt
tune observation camera timing
tune observation camera travel distance
hide clue board during question phase
show clue board during observation phase
update tests
update docs/playtest notes if needed
run all EditMode tests
commit with targeted git add
```

Excluded:

```txt
Cinemachine
new packages
camera rails
interactive camera controls
skip/tap implementation
world-space clue anchors
procedural clues
final art
scene redesign
new audio
jumpscare cinematic
enemy AI/pathfinding
full evidence runtime replacement
language settings UI
mobile build
```

---

## Gameplay Preservation Requirements

Must preserve:

```txt
observation once per floor start
no observation between trials
answers disabled during observation
timer inactive during observation
threat inactive during observation
trial count does not advance during observation
normal questions start after observation
wrong advances threat
timeout advances threat strongly
correct does not move threat back
5 floors
5 trials per floor
Ground Floor escape
restart behavior
EN/FR localization
```

Must not introduce:

```txt
clue board visible during questions
timer ticking during observation
answers clickable during observation
camera stuck forward
duplicate observation coroutine
observation after every answer
observation on win/loss
red Console errors
```

---

## Implementation Guidance

Likely files:

```txt
UnityProject/Assets/Scripts/GameLoop/ObservationPassTiming.cs
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Tests/EditMode/
```

Possible UI methods:

```txt
ShowClueBoard()
HideClueBoard()
SetClueBoardVisible(bool visible)
PrepareObservation()
HideObservationHint()
```

Do not duplicate UI state if a clean method already exists.

Keep visibility behavior explicit and easy to test/reason about.

---

## Tests Required

Current baseline:

```txt
219 EditMode tests
```

Add or update tests where possible.

Required coverage:

```txt
ObservationPassTiming default total duration is longer than Phase 7H
camera move seconds is positive and slower than before
camera return seconds is positive
observation timing remains bounded and not excessive
clue board should be visible during observation state
clue board should hide when trial starts
question phase should not require clue board visible
restart can show clue board again during observation
language switching still preserves observation text
all existing localization tests still pass
```

If clue board visibility is difficult to test through MonoBehaviour, isolate simple state in a pure class or test public UI state if available.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Manual Play Mode Check

If possible, verify:

```txt
Game.unity opens
Game view portrait
Play Mode starts
BEGIN DESCENT
Floor 5 observation starts
camera moves slower than before
camera moves farther toward the corridor/red light
clue board visible during observation
question/answers hidden during observation
timer inactive during observation
after observation, clue board disappears
question/answers appear
timer starts normally
wrong/timeout still work
finish Floor 5
Floor 4 starts with clue board visible again during observation
Floor 4 clue board hides when first question starts
no clue board visible during questions
restart repeats the same behavior
no red Console errors
```

If French is quick to test:

```txt
PrototypeLocalization.Language = GameLanguage.French
```

Expected:

```txt
OBSERVE LE COULOIR
INDICES OBSERVÉS visible only during observation
questions/réponses/cues FR still OK
```

---

## Documentation Updates

Update only if needed:

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
Phase 7H.1 tunes observation timing/distance.
Clue board is now observation-only.
During questions, clue board is hidden to make the player answer from memory.
World-space clues and persistent visible clues are still future work.
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
        UnityProject/Assets/Tests/EditMode \
        Docs/CORRIDOR_OBSERVATION_DESIGN.md \
        Docs/TECH_ARCHITECTURE.md \
        Docs/ROADMAP.md \
        Docs/TEST_PLAN.md \
        Docs/DECISIONS.md \
        Docs/PLAYTEST_NOTES.md
```

If some files are unchanged, omit them.

If `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🎥 tune(gameplay): improve observation pass readability"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7H.1 Observation Pass Tuning

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

## Tuning confirmation

Confirm each item:

- Camera movement slower: yes/no
- Camera movement farther toward corridor/red light: yes/no
- Camera returns before question starts: yes/no
- Observation duration still short enough: yes/no
- Clue board visible during observation: yes/no
- Clue board hidden during questions: yes/no
- Clue board updates per floor before observation: yes/no
- Clue board reappears on next floor observation: yes/no
- Clue board reappears after restart observation: yes/no

## Runtime behavior confirmation

Confirm each item:

- Observation once per floor preserved: yes/no
- No observation between trials: yes/no
- Answers disabled/hidden during observation: yes/no
- Timer inactive during observation: yes/no
- Threat inactive during observation: yes/no
- Trial count unchanged during observation: yes/no
- Normal trial flow resumes after observation: yes/no
- Wrong/timeout/correct behavior preserved: yes/no
- EN/FR localization preserved: yes/no

## Camera / visual details

Explain:

- camera move duration:
- observation hold duration:
- camera return duration:
- camera travel distance:
- camera height offset:
- whether Main Camera is restored:
- whether Game.unity changed:
- whether any new package was added:
- whether Cinemachine was used:

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

Give precise instructions for the user to test the correction.

Include:

- expected camera movement difference
- expected clue board visibility during observation
- expected clue board hidden during questions
- expected Floor 4 behavior after descent
- what feedback the user should report

## Documentation updates

List docs updated and why.

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

Phase 7H.1 is complete only if:

```txt
camera observation feels slower than Phase 7H
camera travels farther toward corridor/red light
camera returns before questions start
clue board is visible during observation
clue board is hidden during questions
clue board updates/reappears on each new floor observation
clue board reappears after restart observation
answers remain disabled during observation
timer remains inactive during observation
threat remains inactive during observation
normal gameplay resumes after observation
EN/FR localization remains intact
all EditMode tests pass if Unity Test Runner is available
no generated Unity files are staged
agent final report is complete and written in French
````
