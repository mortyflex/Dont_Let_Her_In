# Agent Prompt — Phase 7B.2 Multi-Trial Floor Flow

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
This phase changes the core playable run structure from one-question-per-floor to multiple trials per floor. It touches gameplay flow, question progression, floor progression, UI wording, tests and prototype content. Keep Claude for continuity and careful integration.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎮 feat(gameplay): add multi-trial floor flow
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

The player is trapped inside an elevator looking into a corridor. A creature approaches while the player answers short survival trials/questions.

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/AgentPrompts/PHASE_7B2_MULTI_TRIAL_FLOOR_FLOW.md
Docs/AgentPrompts/PHASE_7B1_CORRECT_ONLY_FLOOR_CLEAR.md
Docs/AgentPrompts/PHASE_7B_FLOOR_PROGRESSION.md
Docs/ROADMAP.md
Docs/GAME_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Skills/unity-gameplay-loop/SKILL.md
Skills/horror-game-design/SKILL.md
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

Phase 7B added floor progression transitions:

```txt
FLOOR CLEARED
DOORS CLOSING
ASCENDING
```

Phase 7B.1 fixed the bug where wrong answers and timeouts incorrectly cleared the floor.

Current rule after Phase 7B.1:

```txt
Correct answer clears floor.
Wrong answer retries the same floor.
Timeout retries the same floor.
```

Current tests after Phase 7B.1:

```txt
119/119 EditMode tests passed
```

This rule is technically correct for one-question-per-floor, but the game design has evolved.

---

## Design Correction

The desired game loop is not:

```txt
one floor = one question
wrong/timeout = retry same question
correct = floor cleared
```

The desired game loop is:

```txt
one floor = multiple trials/questions
each answered/expired trial is consumed
wrong/timeout does not repeat the same question
correct/wrong/timeout all move to the next trial of the same floor if alive
all trials completed while alive = floor cleared
final floor completed while alive = YOU ESCAPED
distance <= 0 at any point = SHE GOT IN
```

Important:

```txt
Wrong answers and timeouts are not progress toward victory directly.
They are consumed trials that increase danger.
The player clears the floor only by surviving the whole floor sequence.
```

---

## Mission

Implement:

```txt
Phase 7B.2 — Multi-Trial Floor Flow
```

The goal is to replace the one-question-per-floor flow with a small multi-trial floor prototype.

The player should experience:

```txt
Floor 1 / 5
Trial 1 / 2
answer or timeout
Trial 2 / 2
answer or timeout
if alive after Trial 2: FLOOR CLEARED
then DOORS CLOSING
then ASCENDING
then Floor 2 / 5
```

For the prototype, implement:

```txt
5 floors
2 trials/questions per floor
10 total prototype questions/cues
```

Do not build a large content system yet.

Keep it simple and testable.

---

## Core Gameplay Semantics

### Trial result

For every trial/question:

```txt
correct answer = apply correct outcome, consume trial
wrong answer = apply wrong penalty, consume trial
timeout = apply timeout penalty, consume trial
```

### Loss

After any trial result:

```txt
if distance <= 0:
  SHE GOT IN
  stop progression
```

### Continue within same floor

If alive and there are more trials in the current floor:

```txt
advance to next trial in the same floor
do not show FLOOR CLEARED
do not show DOORS CLOSING
do not show ASCENDING
update UI to Trial X / Y
show next trial cue/question
```

### Clear floor

If alive and all trials in the current floor are completed:

```txt
if current floor is not final:
  show FLOOR CLEARED
  show DOORS CLOSING
  show ASCENDING
  move to next floor
else:
  show YOU ESCAPED
```

### Final escape

Only show `YOU ESCAPED` after:

```txt
all trials of the final floor are completed while alive
```

### No retry of same question

Wrong answers and timeouts must not repeat the same question unless the current floor has only one trial, which this prototype should not have.

---

## Prototype Content Requirement

Create 10 prototype questions/cues.

Structure:

```txt
5 floors
2 trials per floor
```

You may reuse the existing 5 questions as the first trial of each floor and add 5 new simple questions as second trials.

Do not over-polish writing.

Keep questions short and readable.

Keep cues simple and screen-readable.

### Floor 1 — Introduction

Trial 1:

```txt
Prompt: Which room number blinked?
Answers: 101, 104, 108, 102
Correct: 104
Cue: ROOM DISPLAY / 104
Time limit: 8s
```

Trial 2:

```txt
Prompt: Which arrow was lit?
Answers: Up, Down, Left, Right
Correct: Up
Cue: ELEVATOR PANEL / UP ARROW
Time limit: 8s
```

### Floor 2 — Memory

Trial 1:

```txt
Prompt: Which symbol was in the center?
Answers: Eye, Key, Hand, Door
Correct: Key
Cue: SYMBOLS / Eye — Key — Hand
Time limit: 7s
```

Trial 2:

```txt
Prompt: Which word appeared twice?
Answers: Wait, Open, Run, Hide
Correct: Wait
Cue: WALL WORDS / WAIT — OPEN — WAIT
Time limit: 7s
```

### Floor 3 — Instructions

Trial 1:

```txt
Prompt: What did the wall say?
Answers: Do not run, Do not look left, Do not answer, Do not lie
Correct: Do not look left
Cue: WALL / DO NOT LOOK LEFT
Time limit: 6s
```

Trial 2:

```txt
Prompt: Which button should you avoid?
Answers: Alarm, Door Open, Floor 3, Light
Correct: Door Open
Cue: PANEL WARNING / DO NOT OPEN
Time limit: 6s
```

### Floor 4 — Audio Proxy / Codes

Trial 1:

```txt
Prompt: Which sequence did the voice repeat?
Answers: 272, 227, 722, 277
Correct: 272
Cue: VOICE / VOICE: 272
Time limit: 5s
```

Trial 2:

```txt
Prompt: Which code was scratched into the wall?
Answers: 914, 941, 491, 149
Correct: 914
Cue: SCRATCHED CODE / 914
Time limit: 5s
```

### Floor 5 — Final Panic

Trial 1:

```txt
Prompt: The elevator says PRESS EXIT NOW, the wall says WAIT. What do you do?
Answers: Press exit, Wait, Open doors, Look away
Correct: Wait
Cue: ELEVATOR: PRESS EXIT NOW / WALL: WAIT
Time limit: 4s
```

Trial 2:

```txt
Prompt: She is at the door. What should you do?
Answers: Hold the door, Answer calmly, Open it, Look closer
Correct: Answer calmly
Cue: FINAL WARNING / DO NOT OPEN / ANSWER CALMLY
Time limit: 4s
```

---

## Required UI Updates

The UI must clearly show both floor and trial progression.

Required wording:

```txt
FLOOR 1 / 5
TRIAL 1 / 2
```

or a compact equivalent:

```txt
FLOOR 1 / 5 — TRIAL 1 / 2
```

Do not use:

```txt
Question 1
Round 1
```

The player is climbing floors and surviving trials.

---

## Required Flow Wording

After a trial result that does not finish the floor:

```txt
do not show FLOOR CLEARED
do not show DOORS CLOSING
do not show ASCENDING
```

Possible status messages:

```txt
NEXT TRIAL
STAY FOCUSED
SHE IS STILL THERE
```

Keep this minimal.

After the final trial of a non-final floor:

```txt
FLOOR CLEARED
DOORS CLOSING
ASCENDING
```

After final trial of final floor:

```txt
YOU ESCAPED
```

After death:

```txt
SHE GOT IN
```

---

## Required Data / Model Changes

Prefer a small, clear structure.

Possible approaches:

```txt
FloorTrialSet
FloorTrial
PrototypeFloorSet
RunTrialProgress
```

The implementation should make it easy to ask:

```txt
current floor index
total floors
current trial index within floor
trials per floor
current QuestionData
current QuestionCue
is final floor
is final trial in floor
```

Do not over-engineer.

Do not create a full content pipeline.

Do not require ScriptableObject assets unless already easy.

Runtime code provider is acceptable for this prototype.

---

## Required Logic Changes

Replace the Phase 7B.1 retry behavior.

Wrong/timeout should no longer retry the same question if alive.

New behavior:

```txt
Handle answer outcome
Apply threat outcome
Show Phase 6 feedback
If lost:
  show loss
Else:
  mark current trial consumed
  If more trials remain in current floor:
    after pacing hold, start next trial same floor
  Else:
    clear current floor
    If final floor:
      show win
    Else:
      show floor transition and advance next floor
```

Correct answer follows the same trial-consumption structure, but with positive threat delta.

---

## Preserve Existing Systems

Do not rewrite:

```txt
ThreatManager
RunController
QuestionManager
CreatureController
```

Do not change threat deltas unless necessary.

Do not change Phase 6 feedback unless necessary.

Do not change Phase 7 pacing unless necessary.

Do not modify `Game.unity` unless strictly necessary.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
119 EditMode tests
```

Add tests for the new pure logic.

Required test coverage:

```txt
floor has multiple trials
current trial starts at trial 1
correct answer consumes trial
wrong answer consumes trial
timeout consumes trial
wrong/timeout do not retry same trial
more trials remaining means same floor next trial
last trial completed clears floor
final floor last trial escapes
loss overrides trial/floor progression
floor/trial indicator values are correct
```

If gameplay flow is difficult to test because it is MonoBehaviour/coroutine-bound, extract the minimal decision logic into pure helpers and test those helpers.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
START works
HUD shows FLOOR 1 / 5 and TRIAL 1 / 2
Wrong answer on Trial 1 moves to Trial 2 of Floor 1 if alive
Timeout on Trial 1 moves to Trial 2 of Floor 1 if alive
Correct answer on Trial 1 moves to Trial 2 of Floor 1 if alive
Trial 2 completed while alive clears Floor 1
Floor 1 clear shows FLOOR CLEARED / DOORS CLOSING / ASCENDING
Floor 2 starts with TRIAL 1 / 2
Wrong/timeout never repeats the same trial
Wrong/timeout never shows floor transition unless it completed the final trial of the floor while alive
Final trial of Floor 5 while alive shows YOU ESCAPED
Repeated wrong/timeouts can still show SHE GOT IN
Restart works after win
Restart works after loss
Cues match each trial
Timer resets on each new trial
No red Console errors
```

If Play Mode is unavailable, report it honestly.

---

## Scope Constraints

Do not add:

```txt
final UI
final art
scene art pass
real door animation
real audio
jumpscare cinematic
new enemy AI
pathfinding
iOS build
analytics/cloud/online
```

This is a gameplay structure fix.

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
        UnityProject/Assets/Scripts/Questions \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Tests/EditMode
```

If no UI changes are needed, omit `UnityProject/Assets/Scripts/UI`.

If new `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🎮 feat(gameplay): add multi-trial floor flow"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7B.2 Multi-Trial Floor Flow

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

## Multi-trial flow confirmation

Confirm each item:

- Floors contain multiple trials: yes/no
- Prototype has 5 floors: yes/no
- Prototype has 2 trials per floor: yes/no
- Prototype has 10 questions/cues: yes/no
- Correct answer consumes current trial: yes/no
- Wrong answer consumes current trial: yes/no
- Timeout consumes current trial: yes/no
- Wrong/timeout do not retry same trial: yes/no
- More trials remaining starts next trial same floor: yes/no
- Last trial while alive clears floor: yes/no
- Final floor last trial while alive triggers YOU ESCAPED: yes/no
- Loss overrides progression: yes/no
- Restart preserved: yes/no

## Flow details

Explain:

- trial result flow:
- wrong answer flow:
- timeout flow:
- correct answer flow:
- same-floor next-trial behavior:
- floor clear behavior:
- final escape behavior:
- loss behavior:
- restart behavior:

## Prototype content

List the 5 floors and their 2 trials each.

## UI changes

Explain:

- floor indicator:
- trial indicator:
- cue behavior:
- question behavior:
- transition behavior:

## Gameplay preservation

Confirm each item:

- Start works: yes/no
- Timer works: yes/no
- Cues work: yes/no
- Threat distance updates: yes/no
- Creature update still works: yes/no
- Phase 6 feedback preserved: yes/no
- Phase 7 pacing preserved: yes/no
- Floor transition preserved after final trial of non-final floor: yes/no
- YOU ESCAPED only after final floor final trial: yes/no
- SHE GOT IN preserved: yes/no

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

Give precise instructions for the user to test the multi-trial flow.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- expected behavior after correct answer
- expected behavior after wrong answer
- expected behavior after timeout
- expected behavior after final trial of a floor
- expected behavior after final floor
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

Phase 7B.2 is complete only if:

```txt
prototype has 5 floors
each floor has 2 trials
prototype has 10 questions/cues
correct answer consumes current trial
wrong answer consumes current trial
timeout consumes current trial
wrong/timeout do not repeat same trial
if more trials remain, next trial starts on same floor
if last trial of non-final floor completes while alive, FLOOR CLEARED transition plays
if last trial of final floor completes while alive, YOU ESCAPED appears
loss still shows SHE GOT IN
restart works after win/loss
cues/timer reset correctly for each new trial
Phase 6 feedback is preserved
Phase 7 pacing is preserved
floor/trial UI is clear
existing EditMode tests still pass if Unity Test Runner is available
no final UI/art/audio/AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest the multi-trial floor behavior
````
