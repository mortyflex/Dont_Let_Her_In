# Agent Prompt — Phase 5B Playability Readability Fix

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
This phase fixes playability issues in the Unity scene after Phase 5. It touches runtime UI, question cues, scene wiring and gameplay readability. Keep Claude for continuity.
```

Risk level:

```txt
Medium-High
```

Expected commit:

```txt
🎮 fix(gameplay): improve playable flow readability
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

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/AgentPrompts/PHASE_5B_PLAYABILITY_READABILITY_FIX.md
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
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Scripts/Questions/PrototypeQuestionSet.cs
UnityProject/Assets/Scripts/Threat/
UnityProject/Assets/Scripts/Creature/
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Current State

Phase 5 was implemented and committed.

Current status from user playtest:

```txt
EditMode tests: 86/86 passed
Play Mode: works
Win/loss/restart: works
Console: no red errors
```

But the playtest revealed important playability issues.

User feedback:

```txt
- Questions hide the corridor, so the player does not understand the risk and does not feel stress while answering.
- UI is not visible/readable enough when questions are displayed.
- Creature is only clearly seen when the player has already lost.
- Creature moves by steps when distance changes, but the player does not visually feel it approaching during the timer.
- Distance and stress values update correctly.
- Win/loss/restart work.
- Questions feel random because the scene does not show the required clues: no blinking number, no visible symbols, no audio/voice cue.
```

This phase must fix playability/readability.

Do not start a final art pass.

---

## Mission

Implement:

```txt
Phase 5B — Playability Readability Fix
```

The goal is to keep the Phase 5 loop working while making the prototype understandable and stressful.

The player must be able to:

```txt
see the corridor while answering
see the creature/threat area while answering
read the question and answers clearly
see timer/distance/stress clearly
understand where the clue for each question came from
feel that time matters
```

---

## Strict Scope

Included:

```txt
compact readable gameplay UI
question panel repositioning
answer button layout improvements
timer/distance/stress readability improvements
simple visual/textual clue layer for prototype questions
minimal creature/threat visibility improvement during active questions
small tests for new pure logic if introduced
manual Play Mode verification
all EditMode tests still pass
```

Excluded:

```txt
final UI design
final art direction
new 3D art pass
complex elevator/corridor rebuild
audio implementation
jumpscare cinematic
advanced animation
Cinemachine
new enemy AI
pathfinding
iOS build
VR/XR
monetization
analytics/cloud/online
```

---

## Required Fix 1 — UI Must Not Hide the Corridor

The current question UI hides too much of the corridor.

Change the runtime HUD so that in portrait Game View:

```txt
top area: small run HUD / distance / stress / floor
middle area: corridor and creature remain visible
bottom area: compact question + answers
```

Requirements:

```txt
question panel should occupy the lower portion of the screen
central corridor view must remain visible
answer buttons must be large enough to tap
text must remain readable
timer must be obvious
distance/stress must be readable
```

Avoid:

```txt
large centered question card covering the corridor
opaque full-screen panels during active questions
tiny unreadable answer buttons
debug text over the creature
```

Use simple uGUI.

No final design needed.

---

## Required Fix 2 — Clues Must Exist For Each Prototype Question

The questions must not feel random.

Add a simple prototype clue display system.

Acceptable implementation options:

```txt
a QuestionCueController MonoBehaviour
or cue methods inside PlayableRunFlowController + GameplayUIController if kept clean
or a small pure cue model + runtime view
```

The clue system can use simple UI text overlays and/or simple scene labels.

It does not need final art.

It must support the 5 prototype questions.

### Floor 1 — Observation

Question:

```txt
Which room number blinked?
```

Required cue:

```txt
Show the number 104 briefly before or during the question.
```

Acceptable prototype display:

```txt
A small red “104” appears on a corridor/elevator display area for a short moment.
```

### Floor 2 — Short Memory

Question:

```txt
Which symbol was in the center?
```

Required cue:

```txt
Show three symbols/texts briefly: Eye — Key — Hand, with Key in the center.
```

Acceptable prototype display:

```txt
A simple text row or three small labels visible before/during the question.
```

### Floor 3 — Environmental Instruction

Question:

```txt
What did the wall say?
```

Required cue:

```txt
Show “DO NOT LOOK LEFT”.
```

Acceptable prototype display:

```txt
A red wall-message style label in the visible scene/UI clue zone.
```

### Floor 4 — Audio Clue Placeholder

Question:

```txt
Which sequence did the voice repeat?
```

Required cue:

```txt
Show a textual proxy for audio: “VOICE: 272”.
```

Do not implement real audio.

### Floor 5 — Sang-Froid

Question:

```txt
The elevator says PRESS EXIT NOW, the wall says WAIT. What do you do?
```

Required cue:

```txt
Show two conflicting instructions:
- ELEVATOR: PRESS EXIT NOW
- WALL: WAIT
```

The correct answer remains:

```txt
Wait
```

---

## Required Fix 3 — Threat Must Be Visible While Answering

The user must feel the risk while the timer counts down.

Do not create complex animation.

Implement a simple, safe improvement:

```txt
keep the creature/corridor visible while answering
make the distance/stress HUD readable
optionally show a subtle “threat approaching” text/state as timer gets low
optionally call CreatureController with current distance each frame or at question start if needed
```

Important:

```txt
Do not rewrite CreatureController.
Do not create pathfinding.
Do not add final animation.
```

If possible with low risk, add a simple visual timer pressure cue:

```txt
timer below 50%: status gets more tense
timer below 25%: warning text such as “SHE IS CLOSER”
```

But keep it simple.

---

## Required Fix 4 — Preserve Existing Gameplay Logic

Do not break Phase 5.

Must preserve:

```txt
Start works
question flow works
timer works
answer buttons work
correct/wrong/timeout handling works
ThreatManager updates
CreatureController updates after threat changes
win works
loss works
restart works
```

Do not rewrite core systems.

Patch the presentation and cue layer.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
86 EditMode tests
```

If adding pure logic, add tests.

Do not claim tests passed unless actually executed.

If Unity Editor is already open and batch mode cannot run because of lock, report it honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
Start works
question panel no longer blocks the corridor
timer is readable
answer buttons are readable/clickable
distance/stress are readable
Floor 1 cue “104” appears
Floor 2 cue “Eye / Key / Hand” appears
Floor 3 cue “DO NOT LOOK LEFT” appears
Floor 4 cue “VOICE: 272” appears
Floor 5 conflicting cues appear
correct answer still works
wrong answer still works
timeout still works
win still works
loss still works
restart still works
no red Console errors
```

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
git add UnityProject/Assets/Scripts/GameLoop \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Scripts/Questions \
        UnityProject/Assets/Scenes/Game.unity \
        UnityProject/Assets/Tests/EditMode
```

If no scene changes are needed, omit `Game.unity`.

If any `.meta` files are created, include them with their asset/script.

Recommended commit message:

```bash
git commit -m "🎮 fix(gameplay): improve playable flow readability"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 5B Playability Readability Fix

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

## Playability fixes

Confirm each item:

- Question UI no longer blocks the corridor: yes/no
- Active gameplay keeps corridor visible: yes/no
- Active gameplay keeps creature/threat visible: yes/no
- Timer readability improved: yes/no
- Distance/stress readability improved: yes/no
- Answer buttons readability improved: yes/no
- Floor 1 cue exists: yes/no
- Floor 2 cue exists: yes/no
- Floor 3 cue exists: yes/no
- Floor 4 cue exists: yes/no
- Floor 5 cue exists: yes/no

## Cue implementation

Explain how cues are implemented.

## UI layout details

Explain:

- active question layout:
- timer placement:
- distance/stress placement:
- answer button placement:
- clue placement:
- win/loss/restart layout:

## Gameplay preservation

Confirm each item:

- Start works: yes/no
- Correct answer works: yes/no
- Wrong answer works: yes/no
- Timeout works: yes/no
- Threat distance updates: yes/no
- Creature update still works: yes/no
- Win works: yes/no
- Loss works: yes/no
- Restart works: yes/no

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Scene art pass added: yes/no
- Audio added: yes/no
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

Give precise instructions for the user to test the result.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- what should now be better than Phase 5
- what is still not expected yet

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

Phase 5B is complete only if:

```txt
Game.unity remains the main scene
Play Mode can start
question UI no longer blocks the corridor
corridor remains visible while answering
creature/threat remains visible while answering
timer is readable
distance/stress are readable
answer buttons are readable/clickable
prototype question cues exist for all 5 floors
questions no longer feel random
Start/correct/wrong/timeout/win/loss/restart still work
existing EditMode tests still pass if Unity Test Runner is available
no final UI design added
no final art pass added
no audio added
no enemy AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest the improved flow
````
