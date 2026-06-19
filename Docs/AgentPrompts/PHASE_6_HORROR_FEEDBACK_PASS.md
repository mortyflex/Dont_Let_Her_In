# Agent Prompt — Phase 6 Horror Feedback Pass

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
This phase touches gameplay feedback, UI feedback, threat state presentation, scene-safe runtime effects and playability. Keep Claude for continuity and careful integration.
```

Risk level:

```txt
Medium-High
```

Expected commit:

```txt
👻 feat(feedback): add horror response feedback
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
Docs/AgentPrompts/PHASE_6_HORROR_FEEDBACK_PASS.md
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
UnityProject/Assets/Scripts/Questions/PrototypeQuestionCueSet.cs
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

Phase 5 implemented the first playable loop.

Phase 5B improved playability/readability.

Current validated state:

```txt
EditMode tests: 94/94 passed
Play Mode: works
Phase 5B user playtest: OK
Win/loss/restart: works
Question cues: visible
UI readability: acceptable
Console: no red errors
```

Relevant recent commits:

```txt
🎮 feat(gameplay): connect playable question flow
4f04643 — 🎮 fix(gameplay): improve playable flow readability
```

This phase should build on the current working flow.

Do not rewrite the Phase 5/5B loop.

---

## Mission

Implement:

```txt
Phase 6 — Horror Feedback Pass
```

The goal is to make the current playable loop feel more tense and responsive.

The player should clearly feel:

```txt
correct fast = relief
correct normal = small relief
correct slow = barely survived
wrong answer = danger spike
timeout = serious danger spike
near death = panic state
loss = clear horror failure
win = brief escape relief
```

This is not a final art pass.

This is not a final audio pass.

This is a lightweight feedback layer on top of the existing playable prototype.

---

## Strict Scope

Included:

```txt
runtime visual feedback for answer outcomes
screen flash / overlay feedback
timer pressure feedback refinement
threat proximity feedback
near-death warning feedback
simple loss feedback
simple win feedback
small UI polish only if needed for feedback clarity
small pure logic helpers/tests if introduced
manual Play Mode verification
all EditMode tests still pass
```

Excluded:

```txt
final UI design
final art direction
new 3D art pass
elevator/corridor rebuild
real audio implementation
music
voice acting
jumpscare cinematic
advanced animation
Cinemachine
post-processing dependency if not already safe
new enemy AI
pathfinding
iOS build
VR/XR
monetization
analytics/cloud/online
```

---

## Design Goals

Feedback must be readable and immediate.

Use simple effects only:

```txt
colored overlay flashes
short status messages
UI shake or pulse if safe
timer color changes
threat warning text
screen darkening near death
brief blackout on timeout
loss overlay
win overlay
```

Avoid heavy scene/art changes.

Avoid adding complex packages.

Avoid post-processing if it requires project/package changes.

---

## Required Feedback 1 — Answer Outcome Feedback

When the player answers, show distinct feedback based on outcome.

### Correct Fast

Expected feeling:

```txt
relief / she recoils
```

Required feedback:

```txt
green or pale flash
message such as "FAST — SHE RECOILS"
distance increase remains visible
```

### Correct Normal

Expected feeling:

```txt
small relief
```

Required feedback:

```txt
subtle green/white feedback
message such as "CORRECT — KEEP MOVING"
```

### Correct Slow

Expected feeling:

```txt
barely survived
```

Required feedback:

```txt
weak relief
warning tone in text
message such as "TOO SLOW — BARELY"
```

Do not add audio.

### Wrong Answer

Expected feeling:

```txt
danger spike
```

Required feedback:

```txt
red flash
short shake/pulse if safe
message such as "WRONG — SHE MOVES"
distance loss remains visible
stress increase remains visible
```

### Timeout

Expected feeling:

```txt
major danger spike
```

Required feedback:

```txt
strong red/dark flash
brief blackout or dark overlay if safe
message such as "TOO LATE — SHE HEARD YOU"
distance loss remains visible
stress increase remains visible
```

---

## Required Feedback 2 — Threat Proximity Feedback

As threat distance decreases, the player should feel danger increasing.

Use the existing distance/phase logic.

Add simple feedback states based on distance:

```txt
distance > 80: calm-ish
distance <= 80: observed
distance <= 60: visible danger
distance <= 40: danger close
distance <= 25: near door
distance <= 10: panic / at door
distance <= 0: loss
```

Required effects:

```txt
distance/stress HUD becomes more alarming as distance decreases
near-death warning text appears at distance <= 25
panic warning appears at distance <= 10
optional subtle dark/red overlay increases near death
```

Possible messages:

```txt
SHE IS WATCHING
SHE IS IN THE HALL
SHE IS CLOSE
SHE IS AT THE DOOR
DO NOT LET HER IN
```

Keep it readable, not spammy.

---

## Required Feedback 3 — Timer Pressure Feedback

Phase 5B already added some timer pressure.

Refine it if needed.

Expected behavior:

```txt
timer above 50%: normal
timer below 50%: warning
timer below 25%: panic
```

Required:

```txt
timer color changes remain clear
status/warning text remains visible
timer warning must not hide question/corridor
```

Do not overdo it.

---

## Required Feedback 4 — Loss Feedback

When the player loses:

```txt
distance <= 0
```

Show a clear horror loss state.

Required:

```txt
dark/red screen overlay
large message: SHE GOT IN
small subtitle such as "You hesitated too long."
restart button visible
```

No cinematic required.

No jumpscare required.

No audio required.

---

## Required Feedback 5 — Win Feedback

When the player wins:

```txt
all prototype floors completed
```

Show a clear escape state.

Required:

```txt
large message: YOU ESCAPED
small subtitle such as "The doors finally close."
restart button visible
```

No final ending needed.

---

## Required Feedback 6 — Preserve Visibility

Feedback must not recreate the Phase 5 problem.

During active questions:

```txt
corridor must remain visible
creature/threat area must remain visible
question and answer buttons must remain readable
cues must remain readable
timer/distance/stress must remain readable
```

Avoid:

```txt
full-screen opaque overlays during active gameplay
large centered feedback blocking the corridor for too long
flashes that make buttons unreadable
constant warnings covering clues/questions
```

Short flashes are OK.

Transparent overlays are OK.

---

## Suggested Implementation

Acceptable implementation options:

```txt
Add GameplayFeedbackController
or extend GameplayUIController if kept clean
or add a small pure feedback model + UI view
```

Preferred clean approach:

```txt
GameplayFeedbackController or feedback section inside GameplayUIController
```

Possible files:

```txt
UnityProject/Assets/Scripts/UI/GameplayFeedbackController.cs
UnityProject/Assets/Scripts/GameLoop/ThreatFeedbackState.cs
UnityProject/Assets/Tests/EditMode/ThreatFeedbackStateTests.cs
```

Only add pure tests if pure logic is introduced.

Do not over-engineer.

Keep it simple and robust.

---

## Integration Rules

Do not duplicate core gameplay rules.

Use existing information:

```txt
AnswerOutcome
AnswerResult
ThreatManager distance/stress
CreaturePhase
Run state
Question timer ratio
```

Do not rewrite:

```txt
ThreatManager
QuestionManager
CreatureController
RunController
```

Only call existing APIs or add presentation-focused helpers.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
94 EditMode tests
```

If adding pure logic, add tests.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
START works
correct fast feedback appears
correct normal/slow feedback appears if reachable
wrong answer feedback appears
timeout feedback appears
distance/stress remain readable
near-death warning appears around distance <= 25
panic warning appears around distance <= 10
loss shows SHE GOT IN
win shows YOU ESCAPED
restart works after win/loss
corridor remains visible during active questions
question/cues/buttons remain readable
no red Console errors
```

If slow/normal timing is difficult to hit manually, report what was tested and what was inferred.

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
        UnityProject/Assets/Scenes/Game.unity \
        UnityProject/Assets/Tests/EditMode
```

If no scene changes are needed, omit `Game.unity`.

If any `.meta` files are created, include them with their asset/script.

Recommended commit message:

```bash
git commit -m "👻 feat(feedback): add horror response feedback"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 6 Horror Feedback Pass

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

## Feedback implemented

Confirm each item:

- Correct fast feedback: yes/no
- Correct normal feedback: yes/no
- Correct slow feedback: yes/no
- Wrong answer feedback: yes/no
- Timeout feedback: yes/no
- Timer pressure feedback: yes/no
- Threat proximity feedback: yes/no
- Near-death warning: yes/no
- Panic warning: yes/no
- Loss feedback: yes/no
- Win feedback: yes/no
- Restart preserved: yes/no

## Feedback details

Explain:

- overlay/flash behavior:
- status message behavior:
- timer pressure behavior:
- threat proximity behavior:
- loss feedback:
- win feedback:

## Visibility preservation

Confirm each item:

- Corridor visible during active questions: yes/no
- Creature/threat area visible during active questions: yes/no
- Question remains readable: yes/no
- Answer buttons remain readable/clickable: yes/no
- Cues remain readable: yes/no
- Feedback avoids long opaque overlays: yes/no

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

Give precise instructions for the user to test the result.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- what should now feel better than Phase 5B
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

Phase 6 is complete only if:

```txt
Game.unity remains the main scene
Play Mode can start
correct fast/normal/slow feedback exists
wrong answer feedback exists
timeout feedback exists
timer pressure feedback is clear
threat proximity feedback exists
near-death/panic warnings exist
loss feedback shows SHE GOT IN
win feedback shows YOU ESCAPED
corridor remains visible while answering
creature/threat remains visible while answering
question/cues/buttons remain readable
Start/correct/wrong/timeout/win/loss/restart still work
existing EditMode tests still pass if Unity Test Runner is available
no final UI design added
no final art pass added
no real audio added
no jumpscare cinematic added
no enemy AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest the improved horror feedback
````
