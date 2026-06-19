# Agent Prompt — Phase 7B Floor Progression & Elevator Transition

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
This phase adjusts the core run structure and player-facing state flow of the current Unity prototype. It touches gameplay flow, UI messages, win condition semantics, floor progression and pacing. Keep Claude for continuity and careful integration.
```

Risk level:

```txt
Medium-High
```

Expected commit:

```txt
🎮 feat(gameplay): add floor progression transitions
```

---

## Project

You are working on the Unity project:

```txt
Don’t Let Her In
```

Unity 6 URP, iOS-first portrait horror prototype.

Main promise:

```txt
Every second of hesitation brings her closer.
```

The player is trapped inside an elevator looking into a corridor. A creature approaches while the player answers short survival questions.

Important design clarification:

```txt
The player is not escaping after every question.
The player survives one floor at a time.
Only the final floor completion is the real escape.
```

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/AgentPrompts/PHASE_7B_FLOOR_PROGRESSION.md
Docs/ROADMAP.md
Docs/GAME_DESIGN.md
Docs/ART_DIRECTION.md
Docs/TECH_ARCHITECTURE.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Skills/unity-gameplay-loop/SKILL.md
Skills/horror-game-design/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-mobile-performance/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Also inspect:

```txt
UnityProject/Assets/Scripts/GameLoop/
UnityProject/Assets/Scripts/Threat/
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Scripts/UI/
UnityProject/Assets/Scripts/Creature/
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Current State

Phase 5 implemented the first playable loop.

Phase 5B improved UI readability and added question cues.

Phase 6 added horror feedback.

Phase 7 tuned inter-question pacing.

Current validated state:

```txt
EditMode tests: 106/106 passed
Play Mode: works
Phase 6 user playtest: OK
Phase 7 code committed
Win/loss/restart: works
Question cues: visible
Horror feedback: acceptable
Console: no red errors
```

Relevant recent commits:

```txt
39ac495 — 👻 feat(feedback): add horror response feedback
c1d4a21 — 🎮 tune(gameplay): balance prototype run pacing
```

This phase must preserve the working loop.

Do not rewrite the gameplay architecture.

Do not start an art pass.

---

## Why This Phase Exists

The current prototype still implies:

```txt
question completed = next question
final question completed = YOU ESCAPED
```

That is acceptable technically, but the game design needs a clearer structure:

```txt
floor cleared = doors close = temporary safety = elevator ascends = next floor
final floor cleared = true escape
```

The player should feel:

```txt
When the doors are open, I am vulnerable.
When I clear the floor, the doors close just in time.
When the elevator ascends, I get a short moment of safety.
At the next floor, danger returns stronger.
```

This is why elevator framing and floor progression matter.

---

## Mission

Implement:

```txt
Phase 7B — Floor Progression & Elevator Transition
```

The goal is to turn the current “question-to-question” flow into a clearer “floor-to-floor survival” flow.

Each prototype question represents one floor.

Expected high-level loop:

```txt
1. START.
2. Elevator arrives at Floor 1.
3. Doors open / Floor 1 begins.
4. Question/cue appears.
5. Player answers or times out.
6. If player dies: SHE GOT IN.
7. If floor is cleared and it is not the final floor:
   - show FLOOR CLEARED
   - show DOORS CLOSING
   - creature is blocked outside / danger briefly pauses
   - show ASCENDING
   - proceed to next floor
8. If final floor is cleared:
   - show YOU ESCAPED
   - restart available
```

Important:

```txt
A successful floor is not an escape.
Only the final floor completion is an escape.
```

---

## Strict Scope

Included:

```txt
floor-cleared state/transition
inter-floor transition messages
clear distinction between floor success and final escape
player-facing floor progression wording
small safety pause between floors
optional simple door-state text cue
optional simple UI overlay for transition
small pure tests if new logic is introduced
manual Play Mode verification if possible
all EditMode tests still pass
```

Excluded:

```txt
final door animation
real elevator door system
new 3D art pass
elevator/corridor rebuild
new camera work
new UI redesign
audio implementation
music
voice acting
jumpscare cinematic
advanced animation
Cinemachine
post-processing package changes
new enemy AI
pathfinding
iOS build
VR/XR
monetization
analytics/cloud/online
new question categories beyond current prototype set
```

---

## Required Design Change 1 — Floor Success Is Not Escape

Change player-facing wording and flow so that completing a non-final floor does not feel like winning the whole game.

For non-final floors:

```txt
Do not show YOU ESCAPED.
Do not imply the run is over.
Show FLOOR CLEARED or SURVIVED FLOOR.
Show DOORS CLOSING.
Then show ASCENDING or NEXT FLOOR.
Then start the next floor.
```

For the final floor only:

```txt
Show YOU ESCAPED.
Show final win subtitle.
Show Restart.
```

---

## Required Design Change 2 — Inter-Floor Transition

Add a simple transition between floors.

Suggested sequence after clearing a non-final floor:

```txt
FLOOR CLEARED
Doors closing...
Ascending...
Floor X+1
```

This can be UI-only.

No real door mesh animation is required.

No final art is required.

Duration target:

```txt
about 1.0s to 2.5s total
```

It must be short enough to keep pacing, but long enough to communicate safety and progression.

Possible implementation:

```txt
transition panel
status text
subtitle text
temporary hide/disable answer buttons
temporary hide question/cue
keep corridor visible if possible
```

Avoid long blocking transitions.

---

## Required Design Change 3 — Door/Safety Semantics

During a floor-cleared transition, the player should feel briefly safe.

Implement simple semantics:

```txt
during active floor: danger active
after floor cleared: danger paused / safe transition
next floor: danger active again
```

This can be purely presentation-level for now.

Do not rewrite ThreatManager.

Do not create physical doors.

Do not reset the entire threat state unless there is already a clear design reason.

Acceptable simple behavior:

```txt
On non-final floor clear:
- show relief/safety messaging
- optionally apply current existing correct-answer distance gain as already done
- do not apply additional threat damage during the transition
- keep creature state stable during transition
```

Do not add complex mechanics.

---

## Required Design Change 4 — Floor Indicator

The UI should clearly show current floor progression:

```txt
Floor 1 / 5
Floor 2 / 5
Floor 3 / 5
Floor 4 / 5
Floor 5 / 5
```

If this already exists, ensure wording is player-facing and readable.

Avoid confusing wording like:

```txt
Question 1
Round 1
```

The player is climbing floors.

---

## Required Design Change 5 — Difficulty/Stress Ramp Framing

This phase does not need to change balance values heavily.

But the flow should communicate that floors are getting worse.

Add simple player-facing transition subtitles such as:

```txt
The elevator climbs.
The lights flicker.
It is waiting above.
The next floor feels wrong.
Last floor.
```

Keep it lightweight.

Do not add lore-heavy writing.

Do not add cutscenes.

---

## Required Design Change 6 — Preserve Existing Gameplay and Feedback

Do not break:

```txt
START
question flow
timer
answer buttons
question cues
correct fast/normal/slow
wrong answer
timeout
ThreatManager update
CreatureController update
Phase 6 horror feedback
loss state
restart
```

Preserve Phase 5B and Phase 6 visibility:

```txt
corridor visible during questions
creature/threat area visible
question readable
cues readable
answer buttons clickable
feedback readable
```

---

## Suggested Implementation

Prefer small, contained changes.

Possible files:

```txt
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/FloorTransitionState.cs
UnityProject/Assets/Scripts/GameLoop/FloorTransitionText.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Tests/EditMode/
```

Possible approach:

```txt
1. Add a small floor transition helper/model if useful.
2. Add UI methods such as:
   - ShowFloorTransition(title, subtitle)
   - HideFloorTransition()
   - ShowFloorStart(floorIndex, maxFloors)
3. In PlayableRunFlowController:
   - when a floor is cleared and not final:
     - display floor-cleared transition
     - wait a short duration
     - advance to next floor
   - when final floor cleared:
     - show final win as before
```

Do not over-engineer.

Do not introduce a large state machine unless the current code already naturally supports it.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
106 EditMode tests
```

If adding pure floor transition helpers, add tests.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
START works
Floor indicator shows Floor 1 / 5
Clearing Floor 1 does not show YOU ESCAPED
Clearing Floor 1 shows FLOOR CLEARED / DOORS CLOSING / ASCENDING or equivalent
Floor 2 starts after transition
Floor indicator updates to Floor 2 / 5
Same behavior continues through floors 3 and 4
Only clearing Floor 5 shows YOU ESCAPED
Loss still shows SHE GOT IN
Restart works after win
Restart works after loss
Question cues still show correctly
Horror feedback still appears correctly
Corridor/cues/buttons remain readable
No red Console errors
```

If Play Mode is unavailable, report it honestly.

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
editmode-results.xml
test-run.log
mono_crash.*.json
```

Use targeted adds only.

Recommended add command:

```bash
git add UnityProject/Assets/Scripts/GameLoop \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Tests/EditMode
```

If no tests or some folders are unchanged, omit unnecessary paths.

If `Game.unity` is modified, add it explicitly:

```bash
git add UnityProject/Assets/Scenes/Game.unity
```

Recommended commit message:

```bash
git commit -m "🎮 feat(gameplay): add floor progression transitions"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7B Floor Progression & Elevator Transition

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

## Floor progression changes

Confirm each item:

- Floor success no longer means escape: yes/no
- Final floor only triggers escape: yes/no
- Floor cleared transition added: yes/no
- Doors closing message added: yes/no
- Ascending / next floor message added: yes/no
- Floor indicator uses floor wording: yes/no
- Temporary safety/transition moment added: yes/no
- Phase 6 horror feedback preserved: yes/no

## Transition details

Explain:

- non-final floor clear flow:
- final floor clear flow:
- loss flow:
- restart flow:
- transition timing:
- floor indicator wording:
- transition UI behavior:

## Gameplay preservation

Confirm each item:

- Start works: yes/no
- Correct fast works: yes/no
- Correct normal works: yes/no
- Correct slow works: yes/no
- Wrong answer works: yes/no
- Timeout works: yes/no
- Threat distance updates: yes/no
- Creature update still works: yes/no
- Win works only on final floor: yes/no
- Loss works: yes/no
- Restart works: yes/no

## Visibility preservation

Confirm each item:

- Corridor visible during active questions: yes/no
- Creature/threat area visible during active questions: yes/no
- Question remains readable: yes/no
- Answer buttons remain readable/clickable: yes/no
- Cues remain readable: yes/no
- Feedback remains readable: yes/no
- Floor transition messages do not feel too long: yes/no

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Scene art pass added: yes/no
- Real door animation added: yes/no
- Real audio added: yes/no
- Jumpscare cinematic added: yes/no
- New enemy AI/pathfinding added: yes/no
- iOS build generated: yes/no
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

If Play Mode was not checked, write exactly:

Play Mode was not manually verified because Unity Editor GUI was unavailable in this environment.

## Visual/play check instructions for user

Give precise instructions for the user to test the floor progression.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- what should now feel better
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

Phase 7B is complete only if:

```txt
Game.unity remains the main scene
Play Mode can start
floor indicator shows Floor X / Max
clearing a non-final floor does not show YOU ESCAPED
clearing a non-final floor shows floor-cleared / doors-closing / ascending feedback
next floor starts after a short transition
only clearing the final floor shows YOU ESCAPED
loss still shows SHE GOT IN
restart works after win/loss
question cues still work
Phase 6 horror feedback still works
corridor remains visible during active questions
creature/threat remains visible during active questions
question/cues/buttons remain readable
existing EditMode tests still pass if Unity Test Runner is available
no final UI design added
no final art pass added
no real door animation added
no real audio added
no jumpscare cinematic added
no enemy AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest the floor progression
````
