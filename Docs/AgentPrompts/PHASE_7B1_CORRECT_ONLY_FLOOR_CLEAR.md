# Agent Prompt — Phase 7B.1 Correct-Only Floor Clear Fix

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
This is a targeted gameplay logic fix on top of Phase 7B. It touches floor progression semantics, answer outcome handling, timeout/wrong behavior, UI messages and tests. Keep Claude for continuity.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
🐛 fix(gameplay): require correct answer to clear floor
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

Important game design rule:

```txt
Only a correct answer clears the current floor.
Wrong answers and timeouts do not clear the floor.
```

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
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
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/RunController.cs
UnityProject/Assets/Scripts/GameLoop/AnswerOutcome.cs
UnityProject/Assets/Scripts/GameLoop/AnswerOutcomeResolver.cs
UnityProject/Assets/Scripts/GameLoop/FloorTransitionText.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Scripts/Threat/
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

Current tests after Phase 7B:

```txt
112/112 EditMode tests passed
```

User playtest revealed a logic bug:

```txt
When the timer reaches zero, the FLOOR CLEARED / DOORS CLOSING / ASCENDING transition still appears, as if the player had answered correctly.
```

This is wrong.

---

## Bug

Current incorrect behavior:

```txt
Any resolved question outcome appears to advance/clear the floor.
Timeout still triggers FLOOR CLEARED.
Wrong answer may also trigger floor clear.
```

Correct expected behavior:

```txt
Correct answer:
- apply correct outcome
- if player is alive, clear the current floor
- if this was not the final floor, show FLOOR CLEARED / DOORS CLOSING / ASCENDING
- if this was the final floor, show YOU ESCAPED

Wrong answer:
- apply wrong answer penalty
- if distance <= 0, show SHE GOT IN
- otherwise stay on the same floor
- do not show FLOOR CLEARED
- do not show DOORS CLOSING
- do not show ASCENDING
- retry the current floor question after the danger feedback hold

Timeout:
- apply timeout penalty
- if distance <= 0, show SHE GOT IN
- otherwise stay on the same floor
- do not show FLOOR CLEARED
- do not show DOORS CLOSING
- do not show ASCENDING
- retry the current floor question after the danger feedback hold
```

---

## Mission

Implement:

```txt
Phase 7B.1 — Correct-Only Floor Clear Fix
```

The goal is to enforce this core game rule:

```txt
Only a correct answer clears the current floor.
```

Wrong answers and timeouts are not progress.

They are danger.

---

## Required Gameplay Semantics

### Correct answer

For `CorrectFast`, `CorrectNormal`, or `CorrectSlow`:

```txt
1. Apply the existing correct outcome to ThreatManager / RunController.
2. Show existing Phase 6 correct feedback.
3. If the run is lost anyway, show SHE GOT IN.
4. Otherwise, mark the current floor as cleared.
5. If not final floor:
   - show FLOOR CLEARED
   - show DOORS CLOSING
   - show ASCENDING
   - advance to next floor
6. If final floor:
   - show YOU ESCAPED
```

### Wrong answer

For `Wrong`:

```txt
1. Apply existing wrong answer penalty.
2. Show existing Phase 6 wrong feedback.
3. If distance <= 0, show SHE GOT IN.
4. Otherwise, stay on the same floor.
5. Do not call CompleteFloor.
6. Do not advance floor index.
7. Do not show FLOOR CLEARED / DOORS CLOSING / ASCENDING.
8. Retry the current floor question after the danger hold.
```

### Timeout

For `Timeout`:

```txt
1. Apply existing timeout penalty.
2. Show existing Phase 6 timeout feedback.
3. If distance <= 0, show SHE GOT IN.
4. Otherwise, stay on the same floor.
5. Do not call CompleteFloor.
6. Do not advance floor index.
7. Do not show FLOOR CLEARED / DOORS CLOSING / ASCENDING.
8. Retry the current floor question after the danger hold.
```

---

## Important Design Rule

The player is trying to survive each floor.

The loop is:

```txt
floor starts
player sees cue/question
timer creates pressure
correct answer closes the doors and clears the floor
wrong/timeout lets her get closer and the same floor remains unresolved
repeat until correct answer or death
```

This means repeated wrong answers/timeouts on the same floor can kill the player.

That is intended.

---

## UI Requirements

After wrong answer or timeout, the player should clearly understand:

```txt
the floor was not cleared
the creature got closer
the same floor remains active
```

Acceptable messages:

```txt
WRONG — SHE MOVES
TOO LATE — SHE HEARD YOU
STILL ON FLOOR X
ANSWER BEFORE SHE GETS IN
```

Do not add clutter.

Do not hide the corridor.

Do not make a new UI design.

The existing Phase 5B/6 UI should remain.

---

## Transition Rules

Only show floor transition UI for correct answers that clear a non-final floor:

```txt
FLOOR CLEARED
DOORS CLOSING
ASCENDING
```

Never show those messages after:

```txt
wrong answer
timeout
```

Only show `YOU ESCAPED` after:

```txt
correct answer on the final floor
```

Only show `SHE GOT IN` after:

```txt
distance <= 0
```

---

## Implementation Guidance

Prefer a small, targeted fix in:

```txt
PlayableRunFlowController.cs
```

Possible approach:

```txt
After resolving AnswerOutcome:
- apply threat outcome as currently done
- check loss
- if outcome is wrong/timeout:
  - delay using InterQuestionPacing
  - retry current question without advancing floor
- if outcome is correct:
  - complete floor
  - if win, show win
  - else run floor transition and start next floor
```

If useful, add a small pure helper such as:

```txt
FloorClearResolver
```

But do not over-engineer.

Do not rewrite:

```txt
ThreatManager
RunController
QuestionManager
CreatureController
```

Do not change threat deltas unless absolutely necessary.

Do not change question timers unless absolutely necessary.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
112 EditMode tests
```

Add tests that prove:

```txt
correct outcome can clear floor
wrong outcome does not clear floor
timeout outcome does not clear floor
wrong outcome retries same floor if alive
timeout outcome retries same floor if alive
final floor correct answer still wins
loss still overrides retry/progression
```

If the new logic is hard to test directly because it is MonoBehaviour/coroutine-bound, extract the minimal decision logic into a pure helper and test that helper.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
START works
Floor 1 / 5 is visible
Timeout on Floor 1 does NOT show FLOOR CLEARED
Timeout on Floor 1 does NOT advance to Floor 2
Timeout on Floor 1 keeps player on Floor 1 if alive
Wrong answer on Floor 1 does NOT show FLOOR CLEARED
Wrong answer on Floor 1 does NOT advance to Floor 2
Wrong answer on Floor 1 keeps player on Floor 1 if alive
Correct answer on Floor 1 shows FLOOR CLEARED / DOORS CLOSING / ASCENDING
Correct answer on Floor 1 advances to Floor 2
Correct answer on Floor 5 shows YOU ESCAPED
Repeated wrong/timeouts can trigger SHE GOT IN
Restart works after loss
Restart works after win
Cues still show correctly when retrying same floor
Timer resets correctly when retrying same floor
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

Do not modify:

```txt
UnityProject/Assets/Scenes/Game.unity
```

unless strictly necessary.

This should be mostly a gameplay flow/code fix.

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

If no UI changes are needed, omit `UnityProject/Assets/Scripts/UI`.

If new `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🐛 fix(gameplay): require correct answer to clear floor"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7B.1 Correct-Only Floor Clear Fix

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

## Bug fix confirmation

Confirm each item:

- Correct answer clears floor: yes/no
- Correct answer on non-final floor triggers transition: yes/no
- Correct answer on final floor triggers YOU ESCAPED: yes/no
- Wrong answer does not clear floor: yes/no
- Wrong answer does not trigger floor transition: yes/no
- Wrong answer keeps same floor if alive: yes/no
- Timeout does not clear floor: yes/no
- Timeout does not trigger floor transition: yes/no
- Timeout keeps same floor if alive: yes/no
- Loss overrides retry/progression: yes/no
- Restart preserved: yes/no

## Flow details

Explain:

- correct answer flow:
- wrong answer flow:
- timeout flow:
- loss flow:
- final floor win flow:
- same-floor retry behavior:
- timer/cue reset behavior:

## Gameplay preservation

Confirm each item:

- Start works: yes/no
- Question flow works: yes/no
- Timer works: yes/no
- Cues work after retry: yes/no
- Threat distance updates: yes/no
- Creature update still works: yes/no
- Phase 6 feedback preserved: yes/no
- Phase 7 pacing preserved: yes/no
- Floor transition preserved for correct answers: yes/no

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

Give precise instructions for the user to test the bug fix.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- expected behavior after wrong answer
- expected behavior after timeout
- expected behavior after correct answer
- expected behavior on final floor
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

Phase 7B.1 is complete only if:

```txt
wrong answer does not clear floor
wrong answer does not advance floor
wrong answer does not show floor-cleared transition
timeout does not clear floor
timeout does not advance floor
timeout does not show floor-cleared transition
wrong/timeout keep the same floor active if player survives
correct answer clears current floor
correct answer on non-final floor shows floor-cleared transition
correct answer on final floor shows YOU ESCAPED
loss still shows SHE GOT IN
restart works after win/loss
cues/timer reset correctly after retrying same floor
Phase 6 feedback is preserved
Phase 7 pacing is preserved
existing EditMode tests still pass if Unity Test Runner is available
no final UI/art/audio/AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest the corrected floor clear behavior
````
