# Agent Prompt — Phase 7I Elevator Descent Transition Prototype

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
This phase touches the live floor transition flow, elevator visual state, UI timing, camera/feedback pacing, and the already validated observation pass. It must be carefully integrated without changing gameplay rules. Keep Claude for continuity and safe Unity/C# implementation.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🛗 feat(gameplay): add elevator descent transition
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
Phase 7H / 7H.1 — Observation camera pass, tuned and playtest validated
```

Current validated runtime loop:

```txt
Intro
BEGIN DESCENT
Floor starts
Observation travelling pass
Clue board visible during observation
Creature hidden during observation
Question starts
Clue board hidden
Player answers 5 trials from memory
Floor clear
DOORS CLOSING / DESCENDING text
Next floor starts
Observation travelling pass
```

Current known status:

```txt
233/233 EditMode tests passed after Phase 7H.1 correction.
Phase 7H.1 playtest was user-validated:
- travelling timing OK
- travelling distance OK
- clue board visible only during observation
- clue board hidden during questions
- creature invisible during observation
- gameplay stable
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
Docs/AgentPrompts/PHASE_7I_ELEVATOR_DESCENT_TRANSITION.md
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

Inspect current code and scene:

```txt
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/InterQuestionPacing.cs
UnityProject/Assets/Scripts/GameLoop/ObservationPassTiming.cs
UnityProject/Assets/Scripts/GameLoop/ObservationPassState.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
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
Phase 7I — Elevator Descent Transition Prototype
```

The goal is to make the transition between floors feel like an elevator descent.

When the player survives all 5 trials of a floor:

```txt
the question UI disappears
the elevator doors close
the game shows/feels a descent
the floor indicator changes
the doors open again
the next floor observation pass starts
```

This phase is about transition readability and game feel.

It is not final art.

It is not a full elevator simulation.

---

## Target Runtime Loop

Current simplified end-of-floor loop:

```txt
Complete 5 trials
FLOOR CLEARED
DOORS CLOSING
DESCENDING
Next floor starts
Observation pass starts
```

Target loop after Phase 7I:

```txt
Complete 5 trials
Question UI hides
FLOOR CLEARED
Elevator doors close
DESCENDING
Subtle descent motion / shake / vertical cue
Floor indicator updates
Elevator doors open
Next floor observation pass starts
```

For final floor:

```txt
Complete Floor 1
Ground Floor / escape result
Do not run another descent transition to a new floor
```

---

## Strict Scope

Included:

```txt
add prototype elevator door close/open visual state
add descent transition timing/state
add subtle descent feedback
hide question/answers during transition
keep clue board hidden during transition
keep creature hidden/inactive during transition
preserve observation pass after the transition
preserve current gameplay rules
preserve EN/FR localization
add pure tests where possible
run all EditMode tests
manual Play Mode check if possible
update docs/roadmap/test plan/decisions/playtest notes
commit with targeted git add
```

Excluded:

```txt
final door models
complex mesh animation
timeline
Cinemachine
new packages
real audio system
voice acting
complex elevator physics
procedural floor generation
world-space clue anchors
enemy AI/pathfinding
mobile build
Unity Localization package
full evidence runtime replacement
new language settings UI
large scene redesign
```

---

## Recommended Implementation

Prefer a simple deterministic prototype.

Possible pure classes:

```txt
ElevatorTransitionTiming
ElevatorTransitionState
ElevatorTransitionPhase
```

Possible UI/runtime component additions:

```txt
GameplayUIController.ShowElevatorDoorsClosed(bool)
GameplayUIController.ShowDescentOverlay(...)
GameplayUIController.HideTrialHudForTransition()
```

Possible visual approach:

```txt
Use UI panels as simple elevator doors:
- left black/dark panel
- right black/dark panel
- animate or snap them closed/open
```

Recommended for this phase:

```txt
UI overlay prototype doors
```

Reason:

```txt
low scene risk
no final art required
works in portrait
no need to edit Game.unity
```

Acceptable:

```txt
doors close/open with simple timed UI state
descent shown by text + small screen shake / vertical offset cue
```

Do not overbuild.

---

## Door Visual Requirement

Add a prototype visual impression of elevator doors.

Preferred behavior:

```txt
doors open during gameplay/question/observation
doors close after floor completed
doors stay closed during DESCENDING
doors open before the next observation pass
```

Prototype visual can be:

```txt
two dark UI panels sliding inward/outward
or a single dark overlay with split-line if sliding is too risky
```

Text can display:

```txt
DOORS CLOSING
DESCENDING
FLOOR 4
```

French should use existing localization style:

```txt
PORTES EN FERMETURE
DESCENTE
ÉTAGE 4
```

Do not block on perfect animation.

---

## Descent Feeling Requirement

Add a small descent cue.

Acceptable cues:

```txt
subtle camera vertical bump/shake
screen overlay pulse
floor indicator flicker
slight vertical UI movement
```

Recommended:

```txt
subtle camera vertical movement or UI shake during DESCENDING
```

Keep it mobile-readable and not nauseating.

Do not interfere with the observation camera pass.

The descent cue must complete before the next floor observation pass begins.

---

## Runtime Integration Guidance

Likely current flow:

```txt
Complete floor
ClearFloorThenAdvance coroutine
BeginFloor(nextFloor)
BeginObservationThenTrial
```

Target order:

```txt
Complete floor
Stop current question/timer UI
Hide clue board
Hide creature
Show FLOOR CLEARED
Close elevator doors
Show DESCENDING
Run descent cue
Update to next floor
Open elevator doors
BeginFloor(nextFloor)
Run observation pass
Start first trial
```

Important:

```txt
Observation pass should start only after doors open.
Clue board should remain hidden during descent transition.
Creature should not be visible during transition.
Timer should not run during transition.
Answers should not be clickable during transition.
Trial count should not advance during transition except normal next-floor setup.
```

Do not trigger observation before the doors open.

Do not run descent transition after final Floor 1 escape.

---

## Timing Requirements

Keep transition readable but not too long.

Recommended values:

```txt
floorClearedHoldSeconds = 0.8
doorCloseSeconds = 0.8
descentHoldSeconds = 1.4
doorOpenSeconds = 0.8
```

Total before next observation:

```txt
around 3.5 to 4.5 seconds
```

Do not make it as long as observation.

Observation pass is already long; transition should be shorter.

---

## Localization Requirement

Use existing localization style.

Add localized text if needed:

English:

```txt
FLOOR CLEARED
DOORS CLOSING
DESCENDING
DOORS OPENING
FLOOR {0}
```

French:

```txt
ÉTAGE TERMINÉ
PORTES EN FERMETURE
DESCENTE
PORTES EN OUVERTURE
ÉTAGE {0}
```

If some of these already exist, reuse them.

Do not add Unity Localization package.

---

## Gameplay Preservation Requirements

Must preserve:

```txt
observation once per floor start
no observation between trials
clue board visible only during observation
clue board hidden during questions
clue board hidden during transition
creature hidden during observation
creature hidden during transition
answers disabled during observation/transition
timer inactive during observation/transition
threat inactive during observation/transition
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
doors stuck closed
camera stuck offset
answers clickable during transition
timer ticking during transition
threat movement during transition
duplicate coroutines
observation before doors open
transition after final escape
red Console errors
```

---

## Tests Required

Run all EditMode tests.

Current baseline:

```txt
233 EditMode tests
```

Add tests for pure logic where possible.

Required test coverage:

```txt
ElevatorTransitionTiming defaults are positive
ElevatorTransitionTiming total duration is bounded
ElevatorTransitionState starts inactive/open
ElevatorTransitionState can close doors
ElevatorTransitionState can enter descending
ElevatorTransitionState can open doors
answers should not be active during transition
timer should not be active during transition
clue board should not be visible during transition
creature should be hidden during transition
transition should not run after final floor escape
observation should start after transition for non-final floors
localization text exists in English
localization text exists in French
```

If direct coroutine testing is hard, isolate behavior in pure state/timing classes and test those.

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
Floor 5 observation works as validated
Answer 5 trials correctly
Floor clear starts transition
question/answers disappear
clue board remains hidden
creature hidden
doors close visually
DESCENDING appears
descent cue is visible
doors open visually
Floor 4 starts observation only after doors open
Floor 4 clue board appears during observation
no board during questions
no observation before doors open
Floor 1 completion escapes without another descent transition
Restart works
No red Console errors
```

If French is quick to test:

```txt
PrototypeLocalization.Language = GameLanguage.French
```

Expected:

```txt
ÉTAGE TERMINÉ
PORTES EN FERMETURE
DESCENTE
PORTES EN OUVERTURE
```

---

## Documentation Updates

Update:

```txt
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Docs/PLAYTEST_NOTES.md
Docs/TECH_ARCHITECTURE.md
Docs/GAME_DESIGN.md
```

Update only the relevant sections.

Document:

```txt
Phase 7I adds prototype elevator descent transition.
The transition is UI/prototype visual, not final art.
Doors close/open between floors.
Descent cue happens before next observation pass.
Clue board and creature remain hidden during transition.
Observation pass starts only after doors open.
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
        UnityProject/Assets/Scripts/Creature \
        UnityProject/Assets/Tests/EditMode \
        Docs/ROADMAP.md \
        Docs/TEST_PLAN.md \
        Docs/DECISIONS.md \
        Docs/PLAYTEST_NOTES.md \
        Docs/TECH_ARCHITECTURE.md \
        Docs/GAME_DESIGN.md
```

If `Game.unity` changes, add it explicitly:

```bash
git add UnityProject/Assets/Scenes/Game.unity
```

If some paths are unchanged, omit them.

If `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🛗 feat(gameplay): add elevator descent transition"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

```md
# Agent Delivery Report — Phase 7I Elevator Descent Transition Prototype

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

## Files changed

## Elevator transition confirmation

- Elevator transition exists: yes/no
- Runs after non-final floor completion: yes/no
- Does not run after final Floor 1 escape: yes/no
- Question UI hides during transition: yes/no
- Answers disabled/hidden during transition: yes/no
- Timer inactive during transition: yes/no
- Clue board hidden during transition: yes/no
- Creature hidden during transition: yes/no
- Doors close visually: yes/no
- Descent cue visible: yes/no
- Doors open before next observation: yes/no
- Next floor observation starts after doors open: yes/no

## Runtime behavior confirmation

- Descent loop preserved: yes/no
- 5 floors preserved: yes/no
- 5 trials per floor preserved: yes/no
- Observation pass preserved: yes/no
- Clue board observation-only preserved: yes/no
- Creature hidden during observation preserved: yes/no
- Wrong/timeout/correct behavior preserved: yes/no
- EN/FR localization preserved: yes/no
- Restart preserved: yes/no

## Visual details

- door close duration:
- descent duration:
- door open duration:
- total transition duration:
- visual implementation:
- descent cue implementation:
- Game.unity changed: yes/no
- Cinemachine used: yes/no
- new package added: yes/no

## Documentation updates

## Tests run

## Test results

## Manual Play Mode checks

If Play Mode was not checked, write exactly:

Play Mode was not manually verified because Unity Editor GUI was unavailable in this environment.

## Visual/play check instructions for user

Include exact steps to test:

- Floor 5 completion
- door close
- descent
- door open
- Floor 4 observation
- final Floor 1 escape
- FR text if tested

## Git status

Paste git status --short.

## Staged/generated file safety check

Paste:
git status --short | grep -E "UnityProject/(Library|Temp|Logs|UserSettings|Build|Builds)|\.slnx|\.csproj|mono_crash"

If empty, write:
<clean>

## Known limits

## Next recommended action

Choose exactly one:

- READY_FOR_REVIEW
- READY_FOR_PLAYTEST
- NEEDS_FIX
- NEEDS_USER_ACTION
- SHOULD_REVERT
```

Do not summarize freely outside this structure.

---

## Acceptance Criteria

Phase 7I is complete only if:

```txt
elevator descent transition exists
doors close/open visually between floors
transition runs only after non-final floor completion
transition does not run after final Floor 1 escape
question/answers/timer inactive during transition
clue board hidden during transition
creature hidden during transition
descent cue visible
next floor observation starts only after doors open
observation pass remains intact
clue board observation-only remains intact
creature hidden during observation remains intact
gameplay rules unchanged
EN/FR localization intact
all EditMode tests pass if Unity Test Runner is available
no generated Unity files are staged
agent final report complete and written in French
```
