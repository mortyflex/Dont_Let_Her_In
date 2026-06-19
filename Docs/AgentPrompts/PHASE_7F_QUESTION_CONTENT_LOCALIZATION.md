# Agent Prompt — Phase 7F Question Content Localization EN/FR

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
This phase localizes live playable trial content while preserving the current descent loop. It touches question data, cues, answer display, localization tests and possibly lightweight adapters. Keep Claude for continuity and careful integration with the existing Phase 7B.4 runtime and Phase 7E evidence data model.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🌍 feat(questions): localize playable trial content
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

Current known state:

```txt
Phase 7B.4 implemented the live descent loop and EN/FR UI/status/intro localization prep.
Phase 7C aligned documentation.
Phase 7D documented corridor observation and evidence-based trials.
Phase 7E implemented the pure evidence trial data model.
Phase 7E.1 restored the missing Phase 7E agent prompt.
Current gameplay runtime still uses PrototypeFloorSet for playable trials.
PrototypeEvidenceFloorSet exists as DATA_MODEL_ONLY and is not active at runtime.
```

Current known test status:

```txt
179/179 EditMode tests passed after Phase 7E.
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
Docs/AgentPrompts/PHASE_7F_QUESTION_CONTENT_LOCALIZATION.md
Docs/AgentPrompts/PHASE_7E_EVIDENCE_TRIAL_DATA_MODEL.md
Docs/AgentPrompts/PHASE_7D_CORRIDOR_OBSERVATION_DESIGN.md
Docs/AgentPrompts/PHASE_7C_DOCUMENTATION_ALIGNMENT.md
Docs/AgentPrompts/PHASE_7B4_DESCENT_LOOP_INTRO_LOCALIZATION.md
Skills/horror-game-design/SKILL.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Inspect current code:

```txt
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
UnityProject/Assets/Scripts/GameLoop/LocalizedText.cs
UnityProject/Assets/Scripts/GameLoop/GameLanguage.cs
UnityProject/Assets/Scripts/Questions/PrototypeFloorSet.cs
UnityProject/Assets/Scripts/Questions/PrototypeEvidenceFloorSet.cs
UnityProject/Assets/Scripts/Questions/QuestionData.cs
UnityProject/Assets/Scripts/Questions/QuestionCue.cs
UnityProject/Assets/Scripts/Questions/QuestionManager.cs
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code names, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Mission

Implement:

```txt
Phase 7F — Question Content Localization EN/FR
```

The goal is to make the currently playable trial content support:

```txt
English
French
```

This includes:

```txt
question prompts
answer options
corridor cue text shown to the player
any trial-specific visible text
```

The current UI/status/intro localization already exists from Phase 7B.4.

This phase must extend localization to the playable trial content.

---

## Design Intent

The player should be able to play the prototype in English or French.

Current limitation after Phase 7B.4:

```txt
UI/status/intro texts can be EN/FR.
Question prompts, answer options and cues are still English-only.
```

After this phase:

```txt
PrototypeLocalization.Language = GameLanguage.English
  -> intro, UI, prompts, answers and cues display in English.

PrototypeLocalization.Language = GameLanguage.French
  -> intro, UI, prompts, answers and cues display in French.
```

No settings menu is required yet.

Language switching can remain code/test-driven.

---

## Strict Scope

Included:

```txt
localize the 25 currently playable trials
localize prompts
localize answer options
localize cue text / clue-like text currently displayed during trials
reuse existing GameLanguage and LocalizedText
keep English as default
keep gameplay behavior unchanged
keep threat behavior unchanged
keep floor descent unchanged
add tests for EN/FR playable content
update docs/roadmap/test plan if needed
run all EditMode tests
commit with targeted git add
```

Excluded:

```txt
language settings UI
persistent settings
Unity Localization package
camera travelling
ObservationPhaseController runtime behavior
visual clue GameObjects
scene changes
prefabs
materials
final art
real audio
jumpscare cinematic
new enemy AI/pathfinding
mobile build
full evidence runtime integration unless it is trivial and safe
large copywriting/lore rewrite
```

---

## Runtime Integration Requirement

The playable runtime must display localized question content.

Acceptable implementation approaches:

### Option A — Localize current PrototypeFloorSet directly

Update `PrototypeFloorSet`, `QuestionData`, `QuestionCue`, or a small adapter so the current runtime can display localized strings based on `PrototypeLocalization.Language`.

Pros:

```txt
directly fixes current player-facing limitation
low conceptual risk
```

### Option B — Convert PrototypeEvidenceFloorSet to runtime questions

Add a safe adapter from `EvidenceTrial` to existing playable `QuestionData` + `QuestionCue`, then use evidence-backed localized content.

Pros:

```txt
aligns runtime with the new evidence model
reduces duplicated content
```

Cons:

```txt
higher integration risk
```

### Recommendation

Prefer:

```txt
Option A
```

Unless Option B is clearly simple, safe and well tested.

The priority is:

```txt
playable content localizes EN/FR without changing gameplay behavior.
```

Do not force full evidence runtime integration in this phase if it risks regressions.

---

## Required Behavior

Default language:

```txt
English
```

When language is English:

```txt
FLOOR 5 — TRIAL 1 / 5
Prompt, answers and cue are English.
```

When language is French:

```txt
ÉTAGE 5 — ÉPREUVE 1 / 5
Prompt, answers and cue are French.
```

The existing status texts should remain localized:

```txt
WRONG — SHE MOVES
FAUX — ELLE AVANCE

TOO LATE — SHE HEARD YOU
TROP TARD — ELLE T’A ENTENDU

DOORS CLOSING
PORTES EN FERMETURE

DESCENDING
DESCENTE
```

This phase extends the same idea to:

```txt
trial prompts
answer labels
trial cues
```

---

## Content Translation Requirements

Translate the 25 current playable trials.

Use clear, simple French.

Do not over-polish.

Do not make the French too literary.

Keep it short enough for mobile portrait.

Examples:

English:

```txt
Which room number blinked?
```

French:

```txt
Quel numéro de chambre a clignoté ?
```

English:

```txt
Which arrow was lit?
```

French:

```txt
Quelle flèche était allumée ?
```

English:

```txt
The elevator says PRESS EXIT NOW, the wall says WAIT. What do you do?
```

French:

```txt
L’ascenseur indique SORTIE MAINTENANT, le mur indique ATTENDS. Que fais-tu ?
```

For answer options, keep them short.

Examples:

```txt
Wait -> Attendre
Open doors -> Ouvrir les portes
Look away -> Détourner le regard
Answer calmly -> Répondre calmement
```

For cues, make them readable.

Examples:

```txt
ROOM DISPLAY / 104
AFFICHAGE / 104
```

```txt
WALL / DO NOT LOOK LEFT
MUR / NE REGARDE PAS À GAUCHE
```

```txt
FINAL WARNING / DO NOT OPEN / ANSWER CALMLY
DERNIER AVERTISSEMENT / N’OUVRE PAS / RÉPONDS CALMEMENT
```

---

## Data Model Expectations

Reuse:

```txt
GameLanguage
LocalizedText
PrototypeLocalization.Language
```

If current `QuestionData` uses raw strings, update carefully.

Possible approaches:

```txt
LocalizedQuestionData
LocalizedAnswerOption
LocalizedQuestionCue
```

or update existing types if low risk.

Important:

```txt
Do not break existing tests unnecessarily.
Do not remove compatibility unless tests are updated.
Do not hardcode language switching inside UI.
Use the current language source consistently.
```

The runtime should not require the player to restart the Unity editor to switch language in tests.

A code/test switch like this is acceptable:

```txt
PrototypeLocalization.Language = GameLanguage.French
```

---

## Required Tests

Run all EditMode tests.

Current expected baseline:

```txt
179 EditMode tests
```

Add or update tests for localization.

Required test coverage:

```txt
default language is English
English prompt displays English
French prompt displays French
English answers display English
French answers display French
English cue displays English
French cue displays French
all 25 playable trials have English prompt
all 25 playable trials have French prompt
all 25 playable trials have English answers
all 25 playable trials have French answers
all 25 playable trials have English cue text
all 25 playable trials have French cue text
French content keeps exactly 4 answers per trial
French content keeps the same correct answer index/identity as English
switching language does not change trial count
switching language does not change floor count
switching language does not change threat/floor progression data
```

If there is a runtime adapter, add tests that it preserves:

```txt
clue/cue text
time limit
answer count
correct answer
floor/trial count
```

Do not claim tests passed unless actually executed.

If Unity Editor is open and batch mode cannot run because of lock, report honestly and do not commit unverified code.

---

## Manual Play Mode Check

If possible, verify:

```txt
Game.unity opens
Play Mode starts
English default intro works
BEGIN DESCENT starts run
Floor 5 Trial 1 prompt/answers/cue are English
switch PrototypeLocalization.Language to French in code/test hook
French intro/UI/prompt/answers/cue display in French
answers still work
wrong/timeout still work
descending still works
no Console errors
```

If Play Mode is unavailable, report honestly.

No manual Play Mode is required if runtime behavior is covered by EditMode tests and Unity GUI is unavailable.

---

## Documentation Updates

Update docs only as needed.

Recommended updates:

```txt
Docs/CORRIDOR_OBSERVATION_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
```

Document:

```txt
Phase 7F localizes currently playable question content EN/FR.
Evidence data model remains available.
Camera travelling remains planned.
Static corridor clue prototype remains future.
Runtime may still use PrototypeFloorSet unless adapter is added.
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
git add UnityProject/Assets/Scripts/Questions \
        UnityProject/Assets/Scripts/UI \
        UnityProject/Assets/Tests/EditMode \
        Docs/CORRIDOR_OBSERVATION_DESIGN.md \
        Docs/TECH_ARCHITECTURE.md \
        Docs/ROADMAP.md \
        Docs/TEST_PLAN.md \
        Docs/DECISIONS.md
```

If some docs or folders are unchanged, omit them.

If `.meta` files are created, include them with their script/test.

Recommended commit message:

```bash
git commit -m "🌍 feat(questions): localize playable trial content"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7F Question Content Localization EN/FR

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

## Localization confirmation

Confirm each item:

- Playable prompts localized EN/FR: yes/no
- Playable answers localized EN/FR: yes/no
- Playable cues localized EN/FR: yes/no
- English remains default: yes/no
- French can be selected in code/test: yes/no
- Existing GameLanguage reused: yes/no
- Existing LocalizedText reused: yes/no
- Existing PrototypeLocalization.Language reused: yes/no
- 25 playable trials covered: yes/no
- Correct answer identity preserved across languages: yes/no
- Floor/trial counts unchanged: yes/no
- Gameplay behavior unchanged: yes/no

## Runtime integration

Choose one:

- LOCALIZED_PROTOTYPE_FLOOR_SET
- EVIDENCE_ADAPTER_USED
- OTHER

Then explain:

- what runtime uses now:
- whether PlayableRunFlowController changed:
- whether PrototypeFloorSet changed:
- whether QuestionData changed:
- whether QuestionCue changed:
- whether Play Mode behavior changed beyond localization:

## Content coverage

Confirm each item:

- 25 English prompts exist: yes/no
- 25 French prompts exist: yes/no
- All English answers exist: yes/no
- All French answers exist: yes/no
- All English cues exist: yes/no
- All French cues exist: yes/no
- Mobile-friendly French copy: yes/no
- No large lore rewrite: yes/no

## Gameplay preservation

Confirm each item:

- Descent loop preserved: yes/no
- 5 floors preserved: yes/no
- 5 trials per floor preserved: yes/no
- Threat non-receding preserved: yes/no
- Wrong advances threat: yes/no
- Timeout advances threat strongly: yes/no
- Correct does not move threat back: yes/no
- Floor clear by surviving 5 trials preserved: yes/no
- Ground Floor escape preserved: yes/no
- Restart preserved: yes/no

## Documentation updates

List docs updated and why.

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

Phase 7F is complete only if:

```txt
playable question prompts support EN/FR
playable answer options support EN/FR
playable cue text supports EN/FR
all 25 playable trials are covered
English remains default
French can be selected in code/test
correct answer identity is preserved across languages
floor/trial counts are unchanged
descent gameplay is unchanged
threat behavior is unchanged
all EditMode tests pass if Unity Test Runner is available
no scene/art/camera/audio changes are made
no Unity Localization package is introduced
no generated Unity files are staged
agent final report is complete and written in French
````
