# Agent Prompt — Phase 7B.3 Door Seal Scoring & Non-Receding Threat

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
This phase changes core scoring/threat semantics on top of the multi-trial floor flow. It touches gameplay rules, floor-clear conditions, threat behavior, UI wording, prototype content expectations and tests. Keep Claude for continuity and careful integration.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎮 feat(gameplay): add door seal scoring
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
Docs/AgentPrompts/PHASE_7B3_DOOR_SEAL_SCORING.md
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

Phase 7B.2 implemented the current multi-trial structure:

```txt
5 floors
2 trials per floor
10 questions/cues total
correct/wrong/timeout consume the current trial
last trial of a non-final floor while alive clears the floor
last trial of the final floor while alive triggers YOU ESCAPED
distance <= 0 triggers SHE GOT IN
```

Current tests after Phase 7B.2:

```txt
133/133 EditMode tests passed
```

This structure is technically working, but user playtest/design review revealed two deeper design issues:

```txt
1. With only 2 trials per floor, a player can ignore both trials and still clear the floor if alive.
2. Correct answers currently push the threat back, but the intended horror feeling is that the creature should not visibly recede during a floor. Mistakes should permanently make the current floor more dangerous.
```

---

## Design Correction

The desired game loop is now:

```txt
Each floor has 5 trials.
Each trial is consumed after answer or timeout.
Correct answers increase Door Seal score.
Wrong answers add no Door Seal and move the threat closer.
Timeouts add no Door Seal and move the threat much closer.
Correct answers do not make the threat recede during the current floor.
At the end of the floor, doors close only if Door Seal reaches the floor threshold.
If Door Seal is too low at the end of the floor, the creature gets in.
When the next floor starts, Door Seal resets and the threat resets to that floor's starting distance.
```

Important horror rule:

```txt
During a floor, the threat never recedes.
It can only stay where it is, move closer, or enter the elevator.
```

---

## Mission

Implement:

```txt
Phase 7B.3 — Door Seal Scoring & Non-Receding Threat
```

The goal is to make floor clear depend on performance, not merely surviving all trials.

The player should understand:

```txt
Fast correct answers help seal the doors.
Correct answers under high danger are worth more.
Wrong answers and timeouts do not help seal the doors.
Wrong answers and timeouts bring the threat closer.
Correct answers do not push the threat away during the floor.
At the end of the floor, the doors close only if enough Door Seal has been built.
```

---

## Strict Scope

Included:

```txt
change floor structure to 5 trials per floor
provide 25 prototype trials/cues total, or a safe temporary generated/reused set if fully documented
add Door Seal scoring per floor
score correct answers based on answer speed and current threat proximity
make correct answers non-receding for threat distance
keep wrong/timeout threat penalties
add clear/fail floor condition based on Door Seal threshold
reset Door Seal at each new floor
reset threat distance at each new floor start
update HUD to show Door Seal clearly
add tests for scoring, floor thresholds and non-receding threat behavior
manual Play Mode verification if possible
all EditMode tests still pass
```

Excluded:

```txt
final UI design
final art
scene art pass
real door animation
real audio
music
voice acting
jumpscare cinematic
new enemy AI
pathfinding
Cinemachine
post-processing package changes
iOS build
VR/XR
monetization
analytics/cloud/online
large content pipeline
ScriptableObject content authoring unless already easy and safe
```

---

## Core Gameplay Semantics

### Trial result

For every trial:

```txt
correct answer:
  consume trial
  add Door Seal score based on answer speed and current threat proximity
  do not increase threat distance
  do not make the creature visibly recede

wrong answer:
  consume trial
  add 0 Door Seal
  apply wrong threat penalty
  increase stress as already designed

timeout:
  consume trial
  add 0 Door Seal
  apply timeout threat penalty
  increase stress as already designed
```

### Threat behavior within a floor

During a floor:

```txt
threat distance must never increase because of a correct answer
correct fast/normal/slow do not push the creature back
wrong answer reduces distance
timeout reduces distance more strongly
distance <= 0 triggers SHE GOT IN
```

### End of floor

After the final trial of a non-final floor:

```txt
if distance <= 0:
  SHE GOT IN
else if Door Seal >= required threshold for this floor:
  FLOOR CLEARED
  DOORS CLOSING
  ASCENDING
  start next floor
else:
  SHE GOT IN
```

### Final floor

After the final trial of the final floor:

```txt
if distance <= 0:
  SHE GOT IN
else if Door Seal >= final floor threshold:
  YOU ESCAPED
else:
  SHE GOT IN
```

### New floor start

At the start of each floor:

```txt
Door Seal resets to 0
trial index resets to Trial 1 / 5
threat distance resets to that floor's starting distance
stress can reset or partially reduce, but must be explicitly decided and tested/documented
creature updates to the new starting distance
```

Preferred simple behavior for stress:

```txt
reset stress to 0 at each new floor
```

Reason:

```txt
The elevator doors closed, the previous threat was blocked out, and the next floor starts as a new danger cycle.
```

---

## Door Seal Scoring

Implement a clear scoring model.

Preferred base score:

```txt
CorrectFast: 100
CorrectNormal: 70
CorrectSlow: 40
Wrong: 0
Timeout: 0
```

Apply a threat proximity multiplier based on current distance before the correct answer is resolved:

```txt
distance >= 80: x1.00
distance >= 50 and < 80: x1.15
distance >= 25 and < 50: x1.35
distance > 0 and < 25: x1.60
```

Examples:

```txt
CorrectFast at distance 90 = 100
CorrectFast at distance 20 = 160
CorrectNormal at distance 45 = 94.5
CorrectSlow at distance 20 = 64
```

You may store score as float, but display it as integer or percentage.

Do not expose the full formula to the player.

---

## Door Seal Thresholds

For 5 trials per floor, use thresholds that make the run fair but tense.

Recommended thresholds:

```txt
Floor 1: 180
Floor 2: 220
Floor 3: 260
Floor 4: 300
Floor 5: 340
```

This allows multiple clear paths:

```txt
several correct answers
or fewer but faster correct answers
or clutch correct answers when threat is close
```

The player cannot clear by timeout/wrong only because Door Seal would remain 0.

---

## Floor Threat Reset

At the beginning of each floor, reset threat distance to a floor-specific value.

Recommended floor starting distances:

```txt
Floor 1: 85
Floor 2: 80
Floor 3: 75
Floor 4: 70
Floor 5: 65
```

Design intent:

```txt
Each floor starts as a new threat cycle.
Higher floors begin with less safety.
The previous threat is blocked by the elevator doors, but the next floor starts worse.
```

Make sure the creature visual updates to the reset distance at the start of each floor.

Do not carry the exact previous threat distance into the next floor.

---

## Prototype Content Requirement

The prototype should now support:

```txt
5 floors
5 trials per floor
25 total trials
```

Do not over-polish content.

If authoring 25 fully unique questions/cues is too much for this phase, it is acceptable to:

```txt
keep the 10 existing high-quality prototype trials
add 15 simple placeholder-but-playable trials
clearly label them as prototype content in code/comments
ensure every trial has a matching cue
ensure every trial has 4 answers and 1 correct answer
ensure every floor has exactly 5 trials
```

Do not leave any floor with fewer than 5 trials.

Do not create a large content pipeline.

---

## Suggested Prototype Content Direction

Keep content short and readable.

Examples of simple additional trial types:

```txt
Which number was shown?
Which word repeated?
Which arrow lit up?
Which warning should you obey?
Which code appeared?
Which instruction was safe?
Which symbol moved?
Which light stayed red?
Which floor number glitched?
Which button should you avoid?
```

Cues can remain text-based:

```txt
ROOM DISPLAY / 104
ELEVATOR PANEL / UP ARROW
WALL WORDS / WAIT — OPEN — WAIT
SCRATCHED CODE / 914
FINAL WARNING / DO NOT OPEN / ANSWER CALMLY
```

No audio implementation.

No final art.

---

## Required UI Updates

The HUD should show:

```txt
FLOOR X / 5 — TRIAL Y / 5
DOOR SEAL current / required
```

Acceptable display examples:

```txt
DOOR SEAL 140 / 220
```

or:

```txt
DOOR SEAL 64%
```

Preferred for prototype debugging:

```txt
DOOR SEAL 140 / 220
```

The player should understand:

```txt
correct answers increase Door Seal
wrong/timeout do not increase Door Seal
floor clears only when Door Seal is high enough
```

Do not clutter the UI.

Do not hide the corridor.

Do not make a final UI redesign.

---

## Required Flow Wording

When a correct answer adds score:

```txt
FAST — DOOR SEAL RISING
CORRECT — SEAL HOLDING
TOO SLOW — BARELY SEALED
```

If changing Phase 6 messages is low risk, update them to match Door Seal.

If not, preserve existing messages and add Door Seal HUD.

When final trial is completed and Door Seal is too low:

```txt
DOOR SEAL FAILED
SHE GOT IN
```

If adding a separate `DOOR SEAL FAILED` state is too much, use existing loss panel with subtitle:

```txt
The doors would not close.
```

---

## Required Logic Changes

Replace the Phase 7B.2 floor clear condition.

Current Phase 7B.2 behavior:

```txt
last trial completed while alive = floor cleared
```

New behavior:

```txt
last trial completed while alive + Door Seal >= threshold = floor cleared
last trial completed while alive + Door Seal < threshold = loss
```

Correct answers consume trials but do not automatically clear a floor unless it is the final trial and the threshold is met.

Wrong/timeout consume trials and can still lead to floor clear only if the player had already built enough Door Seal earlier in the floor and survives the final trial.

---

## Required Threat Changes

Current previous design may still apply positive deltas on correct answers.

Change behavior so:

```txt
CorrectFast: no threat distance increase
CorrectNormal: no threat distance increase
CorrectSlow: no threat distance increase
Wrong: keep existing negative distance penalty
Timeout: keep existing negative distance penalty
```

If this requires avoiding existing RunController correct methods that apply positive threat deltas, add a safe new path or method that records correct trial results for score/feedback without increasing threat distance.

Do not break existing tests without updating them to the new official rule.

Do not remove old methods if used by tests unless appropriate.

Prefer adding explicit non-receding behavior in the playable flow rather than destroying reusable lower-level logic abruptly.

---

## Required Data / Model Changes

Possible new pure types:

```txt
DoorSealScore
DoorSealScoring
FloorClearRequirement
FloorThreatProfile
FloorResultResolver
```

Possible updates:

```txt
PrototypeFloorSet
RunTrialProgress
TrialFlowResolver
PlayableRunFlowController
GameplayUIController
ThreatManager / RunController only if needed
```

Keep it small and testable.

---

## Preserve Existing Systems

Do not rewrite:

```txt
QuestionManager
CreatureController
large UI architecture
scene hierarchy
```

Avoid modifying `Game.unity` unless strictly necessary.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
133 EditMode tests
```

Add tests for new pure logic.

Required test coverage:

```txt
5 floors exist
each floor has 5 trials
prototype has 25 trials
Door Seal score is 0 for wrong
Door Seal score is 0 for timeout
CorrectFast scores more than CorrectNormal
CorrectNormal scores more than CorrectSlow
closer threat increases score multiplier
floor thresholds are configured for all 5 floors
floor start distances are configured for all 5 floors
correct answer does not increase threat distance in playable flow or decision model
wrong answer still advances threat
timeout still advances threat more strongly
last trial with enough Door Seal clears floor
last trial with insufficient Door Seal loses
final floor with enough Door Seal escapes
loss overrides Door Seal success
Door Seal resets on new floor
threat distance resets on new floor
floor/trial indicator values are correct
```

If gameplay flow is hard to test because it is MonoBehaviour/coroutine-bound, extract pure helpers and test those helpers.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
START works
HUD shows FLOOR 1 / 5 — TRIAL 1 / 5
HUD shows Door Seal value
Correct answer increases Door Seal
Correct answer does not make threat recede
Wrong answer does not increase Door Seal
Wrong answer makes threat advance
Timeout does not increase Door Seal
Timeout makes threat advance strongly
Trial advances after correct/wrong/timeout if alive
After Trial 5, floor clears only if Door Seal >= required threshold
After Trial 5 with low Door Seal, SHE GOT IN
Next floor starts with Door Seal reset
Next floor starts with threat distance reset
Floor 2 starts at its own configured threat distance
Final floor clears to YOU ESCAPED only if Door Seal threshold is met
Restart works after win
Restart works after loss
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

This is a gameplay/scoring structure phase.

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
        UnityProject/Assets/Scripts/Threat \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Tests/EditMode
```

If some folders are unchanged, omit them.

If new `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🎮 feat(gameplay): add door seal scoring"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7B.3 Door Seal Scoring & Non-Receding Threat

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

## Door Seal confirmation

Confirm each item:

- Door Seal score exists: yes/no
- Door Seal resets each floor: yes/no
- Door Seal threshold exists per floor: yes/no
- Correct answers increase Door Seal: yes/no
- Correct fast scores more than correct normal: yes/no
- Correct normal scores more than correct slow: yes/no
- Closer threat increases correct-answer score: yes/no
- Wrong answer adds 0 Door Seal: yes/no
- Timeout adds 0 Door Seal: yes/no
- Floor clear requires Door Seal threshold: yes/no
- Insufficient Door Seal causes loss: yes/no

## Non-receding threat confirmation

Confirm each item:

- Correct fast does not move threat back: yes/no
- Correct normal does not move threat back: yes/no
- Correct slow does not move threat back: yes/no
- Wrong answer moves threat closer: yes/no
- Timeout moves threat closer strongly: yes/no
- Threat resets at new floor start: yes/no
- Floor start distances are configured per floor: yes/no

## Multi-trial content confirmation

Confirm each item:

- Prototype has 5 floors: yes/no
- Each floor has 5 trials: yes/no
- Prototype has 25 trials: yes/no
- Each trial has a cue: yes/no
- Each trial has 4 answers: yes/no
- Each trial has exactly 1 correct answer: yes/no
- HUD shows FLOOR X / 5: yes/no
- HUD shows TRIAL Y / 5: yes/no
- HUD shows Door Seal: yes/no

## Flow details

Explain:

- correct answer flow:
- wrong answer flow:
- timeout flow:
- end-of-floor success flow:
- end-of-floor insufficient Door Seal flow:
- new floor reset flow:
- final floor escape flow:
- loss flow:
- restart behavior:

## Values configured

List:

- Door Seal base scores:
- Door Seal threat multipliers:
- Floor thresholds:
- Floor starting threat distances:
- Stress reset behavior:

## Gameplay preservation

Confirm each item:

- Start works: yes/no
- Timer works: yes/no
- Cues work: yes/no
- Threat distance updates: yes/no
- Creature update still works: yes/no
- Phase 6 feedback preserved: yes/no
- Phase 7 pacing preserved: yes/no
- Floor transition preserved after successful floor clear: yes/no
- YOU ESCAPED only after final floor success: yes/no
- SHE GOT IN preserved: yes/no
- Restart preserved: yes/no

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

Give precise instructions for the user to test Door Seal and non-receding threat.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- expected behavior after correct answer
- expected behavior after wrong answer
- expected behavior after timeout
- expected behavior at end of floor with enough Door Seal
- expected behavior at end of floor with insufficient Door Seal
- expected behavior at new floor start
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

Phase 7B.3 is complete only if:

```txt
prototype has 5 floors
each floor has 5 trials
prototype has 25 trials/cues
Door Seal score exists and displays in HUD
correct answers increase Door Seal based on speed and threat proximity
wrong/timeout add 0 Door Seal
correct answers do not make threat recede during a floor
wrong/timeout make threat advance
floor clear requires Door Seal >= threshold
insufficient Door Seal at end of floor causes loss
Door Seal resets each floor
threat distance resets each floor using configured floor starting distance
floor/trial UI is clear
Phase 6 feedback is preserved
Phase 7 pacing is preserved
floor transitions still work on successful clear
YOU ESCAPED only after final floor success
SHE GOT IN still works
restart works after win/loss
existing EditMode tests still pass if Unity Test Runner is available
no final UI/art/audio/AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest Door Seal and non-receding threat behavior
````
