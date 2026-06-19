# Agent Prompt — Phase 7 Prototype Balancing & Run Tuning

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
This phase tunes the currently playable Unity prototype across gameplay flow, threat distance, timer pressure, answer outcomes, UI feedback and playability. It is integration-sensitive and should preserve the validated Phase 5/5B/6 loop.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
🎮 tune(gameplay): balance prototype run pacing
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

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/AgentPrompts/PHASE_7_PROTOTYPE_BALANCING.md
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

Phase 5B improved UI readability and added cues.

Phase 6 added horror response feedback.

Current validated state:

```txt
EditMode tests: 100/100 passed
Play Mode: works
Phase 6 user playtest: OK
Win/loss/restart: works
Question cues: visible
Horror feedback: acceptable
Console: no red errors
```

Relevant recent commits:

```txt
4f04643 — 🎮 fix(gameplay): improve playable flow readability
39ac495 — 👻 feat(feedback): add horror response feedback
```

This phase must preserve the working loop.

Do not rewrite the gameplay architecture.

Do not start an art pass.

---

## Mission

Implement:

```txt
Phase 7 — Prototype Balancing & Run Tuning
```

The goal is to tune the current playable run so it feels fair, tense and readable.

The player should feel:

```txt
the timer matters
wrong answers are dangerous
timeouts are very dangerous
fast correct answers create relief
slow correct answers barely save the player
the creature progression is understandable
the run can be won
the run can be lost
difficulty ramps across floors
```

This is a balancing/tuning phase, not a new feature phase.

---

## Strict Scope

Included:

```txt
tune prototype question time limits
tune answer outcome threat deltas if needed
tune starting threat distance if needed
tune status hold duration if needed
tune inter-question pacing if needed
tune feedback timing/alpha only if needed
make run progression feel more fair and tense
add small pure tests if constants/models are changed
preserve existing UI/cues/feedback
manual Play Mode verification if possible
all EditMode tests still pass
```

Excluded:

```txt
new art pass
new scene composition work
elevator/corridor rebuild
new UI redesign
audio implementation
jumpscare cinematic
new enemy AI
pathfinding
Cinemachine
post-processing package changes
iOS build
VR/XR
monetization
analytics/cloud/online
new question categories beyond the current prototype set
```

---

## Design Intent

The prototype should be playable in a short session.

Target run length:

```txt
about 2 to 4 minutes
```

Target question count for now:

```txt
5 floors / 5 questions
```

Target feeling:

```txt
Floor 1: tutorial-ish, readable
Floor 2: slightly tense
Floor 3: pressure starts
Floor 4: dangerous
Floor 5: final panic
```

Do not make the game impossible.

Do not make it too easy.

The player should be able to win with mostly correct answers.

The player should be able to lose with repeated wrong answers/timeouts.

---

## Required Tuning 1 — Question Timers

Review current prototype timers.

Current intended timers from previous phases:

```txt
Floor 1: 8s
Floor 2: 7s
Floor 3: 6s
Floor 4: 5s
Floor 5: 4s
```

Tune only if needed.

Acceptable target:

```txt
Floor 1: 9s or 8s
Floor 2: 8s or 7s
Floor 3: 7s or 6s
Floor 4: 6s or 5s
Floor 5: 5s or 4s
```

Make sure cues are readable before the player is forced to answer.

If a cue remains visible during the question, shorter timers can still be acceptable.

---

## Required Tuning 2 — Answer Speed Thresholds

Review how answer speed is classified:

```txt
Fast: first 35% of timer
Normal: 35–70%
Slow: after 70%
Timeout: no answer
```

Keep these thresholds unless there is a clear reason to tune them.

If changed, update tests.

Do not make Fast impossible to hit.

Do not make Slow too forgiving.

---

## Required Tuning 3 — Threat Distance Deltas

Review current threat deltas:

```txt
correct fast: +18 distance
correct normal: +10 distance
correct slow: +3 distance
wrong: -20 distance
timeout: -30 distance
stress +1 on wrong
stress +2 on timeout
```

Keep the existing design unless playability strongly suggests a small adjustment.

Possible acceptable adjustment:

```txt
wrong: -18 to -22
timeout: -28 to -32
correct slow: +0 to +5
```

Do not make large changes.

If constants are changed, tests must be updated.

---

## Required Tuning 4 — Starting Distance and Win/Loss Fairness

Review starting threat distance.

The run should not feel like:

```txt
instant loss after one mistake
```

But it should also not feel like:

```txt
mistakes do not matter
```

Target:

```txt
one wrong answer should be scary but not fatal
one timeout should be very scary but not necessarily fatal from a safe state
multiple mistakes/timeouts should quickly lead to loss
fast correct answers should create meaningful breathing room
```

If starting distance is adjusted, update tests.

---

## Required Tuning 5 — Inter-Question Pacing

Review the delay after answering before the next question appears.

The player needs enough time to register:

```txt
feedback message
distance change
creature movement
danger state
```

But the game should not feel slow.

Target:

```txt
about 0.6s to 1.2s after normal answers
about 0.9s to 1.5s after wrong/timeout
```

Use existing fields if present, such as `statusHoldSeconds`.

Do not introduce complex transition systems.

---

## Required Tuning 6 — Debug/Status Readability

Distance and stress can remain visible for now.

This is still a prototype.

But make sure they are not too noisy.

The player should understand:

```txt
distance decreasing = she is closer
stress increasing = danger rising
```

Optional small change:

```txt
change debug wording to more player-facing wording
```

Example:

```txt
DISTANCE 72 / STRESS 1
```

could become:

```txt
SHE IS FAR — Distance 72 — Stress 1
```

Only do this if it improves readability without clutter.

---

## Required Tuning 7 — Preserve Phase 5B/6 Visibility

Do not regress:

```txt
question UI does not block corridor
corridor visible while answering
creature/threat area visible while answering
cues visible
timer visible
distance/stress visible
feedback flashes not too long/opaque
buttons clickable
```

This is mandatory.

---

## Suggested Implementation

Prefer small, contained changes.

Possible files:

```txt
UnityProject/Assets/Scripts/Questions/PrototypeQuestionSet.cs
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/Threat/ThreatManager.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Tests/EditMode/
```

Do not touch files unnecessarily.

If the balancing values are currently hardcoded, it is acceptable to keep them hardcoded for prototype v0.1.

Do not introduce a large config system unless the code already naturally supports it.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
100 EditMode tests
```

If tuning changes constants covered by tests, update tests.

If adding pure balancing helpers, add tests.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
START works
Floor 1 is understandable and not too punishing
Floor 2 increases pressure
Floor 3 feels tense
Floor 4 feels dangerous
Floor 5 feels like final panic
correct fast creates relief
correct slow feels barely safe
wrong answer feels dangerous
timeout feels very dangerous
player can win with mostly correct answers
player can lose with repeated wrong answers/timeouts
win/loss/restart still work
corridor/cues/buttons remain readable
no red Console errors
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
        UnityProject/Assets/Scripts/Threat \
        UnityProject/Assets/Scripts/Questions \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Tests/EditMode
```

If no tests or some folders are unchanged, omit unnecessary paths.

Recommended commit message:

```bash
git commit -m "🎮 tune(gameplay): balance prototype run pacing"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7 Prototype Balancing & Run Tuning

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

## Balancing changes

Confirm each item:

- Question timers reviewed: yes/no
- Answer speed thresholds reviewed: yes/no
- Threat deltas reviewed: yes/no
- Starting distance reviewed: yes/no
- Inter-question pacing reviewed: yes/no
- Distance/stress readability reviewed: yes/no
- Phase 5B UI visibility preserved: yes/no
- Phase 6 feedback preserved: yes/no

## Values before/after

List any values changed.

If no values changed, write:

No tuning constants were changed; the phase only reviewed and validated current balance.

## Gameplay feel target

Explain how the new or validated balance supports:

- Floor 1:
- Floor 2:
- Floor 3:
- Floor 4:
- Floor 5:
- Win fairness:
- Loss fairness:

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
- Win works: yes/no
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

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Scene art pass added: yes/no
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

Give precise instructions for the user to test the balance.

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

Phase 7 is complete only if:

```txt
Game.unity remains the main scene
Play Mode can start
prototype pacing has been reviewed and tuned if necessary
question timers are fair/readable
answer speed thresholds remain understandable
wrong answer and timeout feel dangerous
correct fast creates relief
correct slow feels weak/barely safe
player can win with mostly correct answers
player can lose with repeated wrong answers/timeouts
Phase 5B visibility is preserved
Phase 6 horror feedback is preserved
Start/correct/wrong/timeout/win/loss/restart still work
existing EditMode tests still pass if Unity Test Runner is available
no final UI design added
no final art pass added
no real audio added
no jumpscare cinematic added
no enemy AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest the tuned pacing
````
