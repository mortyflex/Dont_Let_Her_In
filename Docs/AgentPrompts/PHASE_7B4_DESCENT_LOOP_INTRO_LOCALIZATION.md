# Agent Prompt — Phase 7B.4 Descent Loop, Intro Context & Localization Prep

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
This phase pivots the core run identity from ascending floors with score-based clear conditions to a descent horror loop without score. It also introduces a simple localized narrative intro and prepares visible gameplay text for French/English. Keep Claude for continuity and careful integration.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎮 feat(gameplay): add descent loop and intro localization
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
Docs/AgentPrompts/PHASE_7B4_DESCENT_LOOP_INTRO_LOCALIZATION.md
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

Phase 7B.3 implemented:

```txt
5 floors
5 trials per floor
25 trials/cues total
Door Seal score per floor
Door Seal threshold per floor
non-receding threat during a floor
threat reset at each new floor
stress reset at each new floor
```

Current tests after Phase 7B.3:

```txt
157/157 EditMode tests passed
```

The user play/design review changed the intended game direction.

Door Seal / score is now considered too abstract for the game’s intended feel.

The better core fantasy is:

```txt
The player wakes up high in a sinister building.
The goal is not to score points.
The goal is to descend floor by floor and reach the ground floor.
Each floor opens onto a hallway threat.
The player must survive 5 trials before the creature enters.
Correct answers let the player continue.
Wrong answers and timeouts make the creature approach.
The creature never recedes during a floor.
If the player survives all 5 trials of the floor, the elevator doors close and the elevator descends.
```

---

## Design Pivot

Replace the score-based Door Seal loop with a simpler survival descent loop.

New official loop:

```txt
Prototype starts at Floor 5.
Goal is to reach the Ground Floor.
Each floor has 5 trials.
Each trial is consumed after answer or timeout.

Correct answer:
  consume trial
  threat does not recede
  continue to next trial if alive

Wrong answer:
  consume trial
  threat moves closer
  continue to next trial if alive

Timeout:
  consume trial
  threat moves closer strongly
  continue to next trial if alive

If threat reaches elevator:
  SHE GOT IN

If all 5 trials of the floor are completed while alive:
  DOORS CLOSING
  DESCENDING
  next lower floor starts
  threat resets to that floor's starting distance
  stress resets to 0

After Floor 1 is completed while alive:
  GROUND FLOOR
  YOU ESCAPED
```

Important:

```txt
No score is required.
No Door Seal threshold is required.
The player clears the floor by surviving all 5 trials.
The pressure comes from the non-receding threat and 5 consecutive trials.
```

---

## Mission

Implement:

```txt
Phase 7B.4 — Descent Loop, Intro Context & Localization Prep
```

The goal is to:

```txt
remove Door Seal from player-facing gameplay
remove score/threshold-based floor clear
keep 5 trials per floor
keep non-receding threat
switch progression from ascending to descending
add a simple narrative intro screen before the run
prepare visible gameplay text for French and English
```

---

## Strict Scope

Included:

```txt
remove or neutralize Door Seal UI
remove Door Seal threshold as a floor clear condition
keep non-receding threat behavior
keep 5 floors × 5 trials
switch floor progression to descent: 5 → 4 → 3 → 2 → 1 → Ground Floor
replace ASCENDING wording with DESCENDING
add simple intro screen before START/run
add lightweight localization prep for visible gameplay text
provide English and French versions for intro and key UI/status text
add/update tests for descent progression, score removal and localization data
all EditMode tests still pass
manual Play Mode verification if possible
```

Excluded:

```txt
final UI design
final art
scene art pass
real animated cinematic
complex typewriter system if risky
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
large localization package integration
Unity Localization package unless already present and safe
large content pipeline
```

---

## Narrative Intro Requirement

Add a simple intro screen before the run starts.

The intro should appear before gameplay starts.

The intro should be skippable/continueable with a button.

Acceptable button labels:

English:

```txt
BEGIN DESCENT
```

French:

```txt
COMMENCER LA DESCENTE
```

The intro does not need final art.

It can reuse existing UI style.

It must not block tests or require scene art changes if avoidable.

Preferred English intro text:

```txt
You wake up on the 5th floor.

The elevator is open.
The hallway should be empty.

It is not.

Answer the trials.
Do not let her in.
Reach the ground floor.
```

Preferred French intro text:

```txt
Tu te réveilles au 5e étage.

L’ascenseur est ouvert.
Le couloir devrait être vide.

Il ne l’est pas.

Réponds aux épreuves.
Ne la laisse pas entrer.
Atteins le rez-de-chaussée.
```

If a typewriter effect is easy and low risk, it may be added.

If it risks scope or complexity, use static text.

Do not create a cinematic system.

---

## Localization Prep Requirement

The game must be designed for at least:

```txt
English
French
```

Do not hardcode new player-facing strings directly in flow code when avoidable.

Add a lightweight localization structure.

Possible simple types:

```txt
GameLanguage
LocalizedText
PrototypeLocalization
LocalizedQuestionData
```

or equivalent.

Do not install a localization package unless it is already present and clearly safe.

Keep this small and testable.

Required visible texts to support EN/FR at minimum:

```txt
intro text
begin descent button
floor label
ground floor label
trial label
doors closing
descending
you escaped
she got in
loss subtitle for creature entry
wrong feedback
timeout feedback
correct feedback if touched
restart label if touched
```

English examples:

```txt
FLOOR 5
TRIAL 1 / 5
GROUND FLOOR
DOORS CLOSING
DESCENDING
YOU ESCAPED
SHE GOT IN
She reached the elevator.
WRONG — SHE MOVES
TOO LATE — SHE HEARD YOU
BEGIN DESCENT
RESTART
```

French examples:

```txt
ÉTAGE 5
ÉPREUVE 1 / 5
REZ-DE-CHAUSSÉE
PORTES EN FERMETURE
DESCENTE
TU ES SORTI
ELLE EST ENTRÉE
Elle a atteint l’ascenseur.
FAUX — ELLE AVANCE
TROP TARD — ELLE T’A ENTENDU
COMMENCER LA DESCENTE
RECOMMENCER
```

Default language may be English for now.

There should be an easy way in code to switch the prototype language to French for testing, even if no settings UI exists yet.

Do not localize every internal variable, class or enum.

Code identifiers should remain English.

---

## Descent Progression Requirement

Prototype run starts at:

```txt
Floor 5
```

Then descends:

```txt
Floor 5
Floor 4
Floor 3
Floor 2
Floor 1
Ground Floor
```

HUD should show:

```txt
FLOOR 5 — TRIAL 1 / 5
```

Then after floor clear:

```txt
DOORS CLOSING
DESCENDING
FLOOR 4 — TRIAL 1 / 5
```

After Floor 1 is completed while alive:

```txt
GROUND FLOOR
YOU ESCAPED
```

Do not show:

```txt
ASCENDING
FLOOR 1 / 5
FLOOR 2 / 5
```

unless there is a technical reason while refactoring, and if so it must be reported.

---

## Floor Clear Rule

Remove the Phase 7B.3 Door Seal condition.

New floor clear rule:

```txt
if player is alive after completing Trial 5 / 5 of the current floor:
  floor is cleared
```

New run success rule:

```txt
if player is alive after completing Trial 5 / 5 of Floor 1:
  player reaches Ground Floor
  show YOU ESCAPED
```

Loss rule remains:

```txt
if threat distance <= 0 at any point:
  SHE GOT IN
```

Correct answers do not move the threat back.

Wrong answers and timeouts still move the threat closer.

---

## Threat Behavior

Preserve Phase 7B.3 non-receding threat behavior:

```txt
correct fast: no threat distance increase
correct normal: no threat distance increase
correct slow: no threat distance increase
wrong: threat moves closer
timeout: threat moves closer strongly
```

At the start of each floor, threat resets to a floor-specific distance.

Use the same configured starting distances unless there is a clear reason to adjust them:

```txt
Floor 5 start distance: 85
Floor 4 start distance: 80
Floor 3 start distance: 75
Floor 2 start distance: 70
Floor 1 start distance: 65
```

Reason:

```txt
The deeper the descent, the worse the starting situation becomes.
```

Stress should reset to 0 at each new floor unless this conflicts with current architecture.

---

## Content Requirement

Keep:

```txt
5 floors
5 trials per floor
25 total trials
```

The existing 25 prototype trials/cues from Phase 7B.3 can be reused.

If the current content references ascending progression or Door Seal explicitly, update those texts.

Every trial must still have:

```txt
prompt
4 answers
1 correct answer
cue
time limit
```

For localization prep, it is acceptable in this phase to localize key UI/status/intro strings first and leave question content as English if full question localization would be too large.

But if question content is already easy to wrap in LocalizedText, do so.

Report honestly what is localized and what remains English-only.

---

## UI Requirements

Remove or hide:

```txt
DOOR SEAL current / required
Door Seal failed text
Door Seal score display
```

Show:

```txt
FLOOR 5 — TRIAL 1 / 5
```

or localized equivalent.

During transition:

```txt
DOORS CLOSING
DESCENDING
```

Final success:

```txt
GROUND FLOOR
YOU ESCAPED
```

Loss:

```txt
SHE GOT IN
```

Do not clutter the UI.

Do not hide the corridor.

Do not do final visual design.

---

## Suggested Implementation Notes

You may remove Door Seal usage from `PlayableRunFlowController`.

You may keep pure DoorSeal classes in the codebase temporarily if removing them creates unnecessary churn, but they must not affect gameplay or UI.

Preferred:

```txt
No player-facing Door Seal display.
No Door Seal threshold check in floor clear.
No score-based loss.
```

If Door Seal test files remain, update or remove tests so they do not imply active gameplay if they are no longer used.

Prefer naming new descent/progression helpers clearly, for example:

```txt
DescentFloorProgress
FloorDescentProfile
RunDescentProgress
PrototypeLocalization
```

Do not over-engineer.

---

## Tests Required

Run all EditMode tests.

Current expected baseline:

```txt
157 EditMode tests
```

Add/update tests for the new official behavior.

Required test coverage:

```txt
run starts at Floor 5
progression goes Floor 5 -> Floor 4 -> Floor 3 -> Floor 2 -> Floor 1 -> Ground Floor
floor has 5 trials
last trial while alive clears floor without score requirement
Floor 1 last trial while alive escapes/reaches Ground Floor
Door Seal threshold is not required for floor clear
correct answer does not move threat back
wrong answer moves threat closer
timeout moves threat closer strongly
threat resets at new floor start
stress resets at new floor start
HUD/progress formatter can output floor/trial labels for descent
localization contains English intro
localization contains French intro
localization contains English/French transition labels
localization has default language
language can be switched in code/test
```

If gameplay flow is hard to test because it is MonoBehaviour/coroutine-bound, extract/update pure helpers and test those helpers.

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Required Manual Play Mode Check

Verify:

```txt
Game.unity opens
Play Mode starts
intro screen appears before the run
intro text gives the descent context
begin button starts the run
HUD shows Floor 5 and Trial 1 / 5
Correct answer advances trial and does not make threat recede
Wrong answer advances trial and makes threat move closer
Timeout advances trial and makes threat move closer strongly
After Trial 5 while alive, doors close and DESCENDING appears
Next floor is Floor 4, not Floor 2 or Floor 6
Door Seal is not visible
No score is required to clear an alive floor
Floor 1 completed alive reaches Ground Floor / YOU ESCAPED
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
real animated cinematic
real door animation
real audio
jumpscare cinematic
new enemy AI
pathfinding
iOS build
analytics/cloud/online
```

This is a gameplay/narrative/localization-prep phase.

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
git commit -m "🎮 feat(gameplay): add descent loop and intro localization"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7B.4 Descent Loop, Intro Context & Localization Prep

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

## Descent loop confirmation

Confirm each item:

- Run starts at Floor 5: yes/no
- Run descends Floor 5 -> 4 -> 3 -> 2 -> 1: yes/no
- Ground Floor success exists: yes/no
- ASCENDING is no longer used in gameplay transition: yes/no
- DESCENDING is used in gameplay transition: yes/no
- Floor clear no longer requires Door Seal/score: yes/no
- Door Seal is not visible in HUD: yes/no
- Player clears a floor by surviving all 5 trials: yes/no
- Floor 1 completion while alive triggers final escape: yes/no

## Intro confirmation

Confirm each item:

- Intro screen exists: yes/no
- Intro appears before gameplay starts: yes/no
- Intro explains wake-up/floor/elevator context: yes/no
- Intro has continue/begin button: yes/no
- Intro text has English version: yes/no
- Intro text has French version: yes/no
- Intro is skippable/continueable: yes/no

## Localization confirmation

Confirm each item:

- Lightweight localization structure exists: yes/no
- Default language exists: yes/no
- English visible UI/status strings exist: yes/no
- French visible UI/status strings exist: yes/no
- Language can be switched in code/test: yes/no
- Intro strings are localized: yes/no
- Transition strings are localized: yes/no
- Loss/win strings are localized: yes/no
- Question content localized: yes/no/partial
- Hardcoded new player-facing strings avoided where practical: yes/no

## Threat and trials confirmation

Confirm each item:

- Prototype has 5 floors: yes/no
- Each floor has 5 trials: yes/no
- Prototype has 25 trials: yes/no
- Correct answer consumes trial: yes/no
- Correct answer does not move threat back: yes/no
- Wrong answer consumes trial: yes/no
- Wrong answer moves threat closer: yes/no
- Timeout consumes trial: yes/no
- Timeout moves threat closer strongly: yes/no
- Threat resets at new floor start: yes/no
- Stress resets at new floor start: yes/no
- Restart preserved: yes/no

## Flow details

Explain:

- intro flow:
- start run flow:
- correct answer flow:
- wrong answer flow:
- timeout flow:
- end-of-floor success flow:
- new floor reset flow:
- ground floor escape flow:
- loss flow:
- restart behavior:
- language switching behavior:

## Values configured

List:

- Prototype starting floor:
- Prototype final floor before Ground Floor:
- Trials per floor:
- Floor starting threat distances:
- Stress reset behavior:
- Default language:
- Localized languages:

## Gameplay preservation

Confirm each item:

- Start works: yes/no
- Timer works: yes/no
- Cues work: yes/no
- Threat distance updates: yes/no
- Creature update still works: yes/no
- Phase 6 feedback preserved: yes/no
- Phase 7 pacing preserved: yes/no
- Floor transition preserved after successful floor survival: yes/no
- YOU ESCAPED only after Ground Floor success: yes/no
- SHE GOT IN preserved: yes/no
- Restart preserved: yes/no

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Scene art pass added: yes/no
- Real animated cinematic added: yes/no
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

Give precise instructions for the user to test descent loop, intro and localization prep.

Include:

- scene to open
- Game view portrait setup
- exact test steps
- expected intro behavior
- expected behavior after begin/start
- expected behavior after correct answer
- expected behavior after wrong answer
- expected behavior after timeout
- expected behavior after Trial 5 of a floor
- expected behavior at new lower floor
- expected behavior after Floor 1 success
- expected behavior after loss
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

Phase 7B.4 is complete only if:

```txt
Door Seal is no longer visible in gameplay HUD
Door Seal/score threshold is no longer required for floor clear
prototype starts at Floor 5
prototype descends Floor 5 -> Floor 4 -> Floor 3 -> Floor 2 -> Floor 1 -> Ground Floor
ASCENDING wording is replaced by DESCENDING
intro screen appears before the run
intro explains the wake-up/elevator/building context
intro has EN and FR text available
lightweight localization prep exists for EN/FR visible strings
correct answers do not make threat recede
wrong/timeout still make threat advance
each floor still has 5 trials
completing 5 trials alive clears the current floor
completing Floor 1 alive reaches Ground Floor / escape
threat distance resets per floor
stress resets per floor
restart works after win/loss
existing EditMode tests still pass if Unity Test Runner is available
no final UI/art/audio/AI/pathfinding added
no generated folders staged
agent final report is complete and written in French
user can playtest intro + descent loop behavior
````
