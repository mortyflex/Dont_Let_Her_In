# Agent Prompt — Phase 7C Documentation Alignment

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
This phase consolidates project documentation after several gameplay pivots. It must accurately reflect the current committed game loop, remove obsolete Door Seal / ascending-floor language, and align design, roadmap, tech architecture, test plan and decisions. Keep Claude for continuity and careful project memory handling.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
📝 docs(project): align descent loop documentation
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

Current official concept:

```txt
The player wakes up high inside a sinister building.
They are trapped in an elevator with the doors open.
At each floor, a hallway threat approaches.
The goal is to descend floor by floor and reach the ground floor.
Each floor contains 5 trials.
Correct answers allow the player to continue but do not push the threat back.
Wrong answers and timeouts move the threat closer.
The threat never recedes during a floor.
If the player survives all 5 trials of a floor, the elevator doors close and the elevator descends.
If the threat reaches the elevator, SHE GOT IN.
After surviving Floor 1, the player reaches the Ground Floor and escapes.
```

Prototype v0.1 current structure:

```txt
Prototype starts at Floor 5.
Progression: Floor 5 -> Floor 4 -> Floor 3 -> Floor 2 -> Floor 1 -> Ground Floor.
Each floor has 5 trials.
Total prototype content: 25 trials/cues.
English is the default language.
French localization prep exists for key UI/status/intro strings.
Question content currently remains English-only.
```

---

## Required Reading Before Editing

Read these files before editing documentation:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/PRD.md
Docs/GAME_DESIGN.md
Docs/ART_DIRECTION.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Docs/PLAYTEST_NOTES.md
Docs/AgentPrompts/PHASE_7B4_DESCENT_LOOP_INTRO_LOCALIZATION.md
Docs/AgentPrompts/PHASE_7B3_DOOR_SEAL_SCORING.md
Docs/AgentPrompts/PHASE_7B2_MULTI_TRIAL_FLOOR_FLOW.md
Docs/AgentPrompts/PHASE_7B1_CORRECT_ONLY_FLOOR_CLEAR.md
Docs/AgentPrompts/PHASE_7B_FLOOR_PROGRESSION.md
Skills/game-agent-delivery/SKILL.md
```

Also inspect relevant current code to avoid documenting stale behavior:

```txt
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/DescentFloorProfile.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
UnityProject/Assets/Scripts/GameLoop/TrialFlowResolver.cs
UnityProject/Assets/Scripts/Questions/PrototypeFloorSet.cs
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Current Committed State

The latest gameplay phase is:

```txt
Phase 7B.4 — Descent Loop, Intro Context & Localization Prep
```

Commit:

```txt
9cb1bc779d081a47ec0a472f93312ddead9e6de5
🎮 feat(gameplay): add descent loop and intro localization
```

Current tests:

```txt
148/148 EditMode tests passed
```

Current behavior:

```txt
Door Seal / score has been removed from player-facing gameplay.
The run starts at Floor 5.
The player descends Floor 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor.
Each floor has 5 trials.
Surviving all 5 trials clears the floor.
Correct answers consume trials and do not move the threat back.
Wrong answers consume trials and move the threat closer.
Timeouts consume trials and move the threat closer strongly.
Threat resets at each new floor.
Stress resets at each new floor.
Intro screen exists before the run.
Lightweight EN/FR localization prep exists for key UI/status/intro strings.
Question content remains English-only for now.
```

---

## Mission

Implement:

```txt
Phase 7C — Documentation Alignment
```

Your job is to update project documentation so it matches the current official game direction.

Do not change gameplay code unless you find a tiny typo in comments that is clearly documentation-only.

Do not implement new features.

Do not change Unity scene/art/UI behavior.

This is a documentation consolidation phase.

---

## Documentation Goals

Align all relevant docs around the new identity:

```txt
Don’t Let Her In is a portrait mobile horror elevator trial game.
The player wakes up on a high floor of a sinister building.
The objective is to descend to the Ground Floor and escape.
Each floor opens onto a hallway threat.
The player must survive 5 trials per floor.
The threat never recedes during a floor.
Correct answers let the player continue.
Wrong answers and timeouts bring the threat closer.
Surviving all 5 trials closes the doors and descends to the next floor.
Getting caught means SHE GOT IN.
Prototype v0.1 uses 5 floors for scope.
Full game target may start higher, such as Floor 15.
English and French are planned from the beginning.
```

Remove or mark obsolete the old ideas:

```txt
ascending floors as the main progression
one question = one floor
wrong/timeout retries same question
correct answer pushes the creature away
Door Seal score
Door Seal threshold
score-based floor clear
floor clear because enough points were earned
YOU ESCAPED after climbing upward
```

---

## Files To Update

Update these files if they contain stale or incomplete information:

```txt
README.md
Docs/PRD.md
Docs/GAME_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Docs/PLAYTEST_NOTES.md
```

Optional only if needed:

```txt
AGENTS.md
CLAUDE.md
```

Do not update `AGENTS.md` or `CLAUDE.md` unless there is a clear documentation-process reason.

Do not rewrite all docs from scratch.

Make targeted, coherent updates.

---

## Required Documentation Updates

### README.md

Update the project overview to reflect:

```txt
mobile portrait horror prototype
descent loop
Floor 5 -> Ground Floor prototype
5 trials per floor
non-receding threat
intro context
EN/FR localization prep
current latest phase and test count
```

Mention current status:

```txt
Latest gameplay commit: Phase 7B.4 descent loop and intro localization.
Current tests: 148/148 EditMode passing.
```

Do not overpromise final art/audio.

---

### Docs/PRD.md

Update product direction:

```txt
The player wakes up high in a sinister building.
The core objective is to reach the Ground Floor.
Prototype v0.1 starts at Floor 5.
Full target may start at Floor 15 later.
Each floor contains 5 trials.
The player clears a floor by surviving all trials.
No score requirement in current design.
No Door Seal mechanic in current design.
English/French are planned initial languages.
```

Clarify excluded v0.1 features:

```txt
final art
real audio
jumpscare cinematic
pathfinding enemy AI
monetization
online/cloud
VR/XR
full 15-floor campaign
full question localization
```

---

### Docs/GAME_DESIGN.md

This is the most important file.

It must clearly define the official loop.

Include or update sections for:

```txt
Core Fantasy
Run Structure
Descent Progression
Floor Structure
Trial Rules
Threat Rules
Win/Loss Conditions
Intro Narrative
Localization Direction
Prototype Scope
Future Expansion
```

Official rules to document:

```txt
Prototype starts at Floor 5.
Floor order: 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor.
Each floor has 5 trials.
Every trial is consumed after correct/wrong/timeout.
Correct answer: consumes trial, threat does not recede.
Wrong answer: consumes trial, threat moves closer.
Timeout: consumes trial, threat moves closer strongly.
Threat never recedes during a floor.
Threat resets at the start of each floor.
Stress resets at the start of each floor.
Survive 5 trials = floor cleared.
After clearing a non-final floor: DOORS CLOSING -> DESCENDING -> next floor.
After clearing Floor 1: GROUND FLOOR -> YOU ESCAPED.
Threat reaches elevator: SHE GOT IN.
```

Document current prototype values:

```txt
Floor 5 start distance: 85
Floor 4 start distance: 80
Floor 3 start distance: 75
Floor 2 start distance: 70
Floor 1 start distance: 65
Wrong penalty: threat closer
Timeout penalty: threat much closer
Correct answer: no threat movement
```

Only include exact numeric wrong/timeout penalties if confirmed in code.

Mark question content as prototype.

---

### Docs/TECH_ARCHITECTURE.md

Update architecture to mention current systems:

```txt
PlayableRunFlowController
DescentFloorProfile
PrototypeLocalization
LocalizedText
GameLanguage
PrototypeFloorSet
TrialFlowResolver
ThreatManager
CreatureController
GameplayUIController
```

Remove or mark obsolete:

```txt
DoorSealScoring
DoorSealScore
FloorThreatProfile
FloorTransitionText
Door Seal gameplay dependency
```

If old classes were deleted, state that Door Seal was removed in Phase 7B.4.

Document localization approach:

```txt
lightweight code-based localization
English default
French available for key UI/status/intro strings
question content remains English-only for now
no Unity Localization package yet
```

---

### Docs/ROADMAP.md

Update roadmap history through Phase 7B.4.

Include:

```txt
Phase 7B — floor transitions
Phase 7B.1 — correct-only floor clear, later superseded by multi-trial flow
Phase 7B.2 — multi-trial floors
Phase 7B.3 — Door Seal scoring experiment, later superseded
Phase 7B.4 — descent loop, intro, localization prep
Phase 7C — documentation alignment
```

Important: describe Phase 7B.3 as a completed experiment that was intentionally superseded, not as a current mechanic.

Current recommended next phases after 7C may include:

```txt
Phase 7D — Playtest Polish / Flow Readability
Phase 7E — Question Content Localization EN/FR
Phase 8 — Mobile Build Readiness
Phase 9 — Visual/Horror Scene Polish
```

Do not jump to final art or monetization.

---

### Docs/TEST_PLAN.md

Update test expectations to cover:

```txt
descent starts at Floor 5
progression Floor 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor
intro appears before run
language switching EN/FR works in code/test
Door Seal not visible
floor clear no longer requires score
5 trials per floor
correct answer does not move threat back
wrong/timeout move threat closer
threat/stress reset each floor
Floor 1 clear triggers Ground Floor success
restart after win/loss
```

Mention current passing test count:

```txt
148/148 EditMode tests passing after Phase 7B.4.
```

---

### Docs/DECISIONS.md

Add Architecture Decision Records or decision notes for:

```txt
Descent loop replaces ascending loop.
Door Seal score mechanic was removed from active gameplay.
Threat is non-receding during each floor.
Floor clear is survival-based, not score-based.
Prototype uses 5 floors; full game may use 15.
Localization is planned from the beginning with EN/FR.
Intro context is added before gameplay to clarify situation.
```

Each decision should include:

```txt
Context
Decision
Consequence
Status
```

Status should be something like:

```txt
Accepted
```

For Door Seal:

```txt
Superseded / Removed from active gameplay
```

---

### Docs/PLAYTEST_NOTES.md

Add a new playtest checklist for Phase 7B.4.

Include:

```txt
intro readability in portrait
BEGIN DESCENT clarity
descent Floor 5 -> Floor 4 clarity
absence of Door Seal
5 trials rhythm
threat non-receding feeling
wrong/timeout pressure
floor clear timing
Ground Floor escape clarity
French UI smoke check
Console errors
```

Do not claim user playtest results unless already provided.

Use a template with unchecked items if needed.

---

## Style Requirements

Documentation should be clear and practical.

Avoid fluffy marketing language.

Avoid long lore dumps.

The project is still a prototype.

Use terms consistently:

```txt
trial
floor
descent
Ground Floor
threat
elevator
hallway
SHE GOT IN
YOU ESCAPED
```

For French localization references, use:

```txt
ÉTAGE
ÉPREUVE
REZ-DE-CHAUSSÉE
DESCENTE
ELLE EST ENTRÉE
TU ES SORTI
```

Do not mix “question”, “round”, “level” as primary terms unless explaining.

Primary term:

```txt
trial
```

French visible equivalent:

```txt
épreuve
```

---

## Tests

This is a documentation-only phase.

Run lightweight validation:

```bash
git status --short
```

If Markdown tooling exists in the repo, run it.

If no Markdown tooling exists, do not invent tooling.

You do not need to run Unity EditMode tests unless you changed code.

If you do run Unity tests, report exact command and result.

Do not claim Unity tests were run unless actually executed.

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
git add README.md \
        Docs/PRD.md \
        Docs/GAME_DESIGN.md \
        Docs/TECH_ARCHITECTURE.md \
        Docs/ROADMAP.md \
        Docs/TEST_PLAN.md \
        Docs/DECISIONS.md \
        Docs/PLAYTEST_NOTES.md
```

Omit any file that was not changed.

Recommended commit message:

```bash
git commit -m "📝 docs(project): align descent loop documentation"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7C Documentation Alignment

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

## Documentation alignment confirmation

Confirm each item:

- Descent loop documented: yes/no
- Floor 5 -> Ground Floor documented: yes/no
- 5 trials per floor documented: yes/no
- Non-receding threat documented: yes/no
- Door Seal removed/superseded in docs: yes/no
- Score-based floor clear removed/superseded in docs: yes/no
- Intro context documented: yes/no
- EN/FR localization prep documented: yes/no
- Question content localization limitation documented: yes/no
- Current test count documented: yes/no

## Current official gameplay documented

Confirm each item:

- Correct answer consumes trial: yes/no
- Correct answer does not move threat back: yes/no
- Wrong answer consumes trial: yes/no
- Wrong answer moves threat closer: yes/no
- Timeout consumes trial: yes/no
- Timeout moves threat closer strongly: yes/no
- Surviving 5 trials clears floor: yes/no
- Clearing Floor 1 reaches Ground Floor: yes/no
- SHE GOT IN loss condition documented: yes/no
- YOU ESCAPED success condition documented: yes/no

## Roadmap updates

Explain:

- phases added/updated:
- obsolete/superseded mechanics marked:
- next recommended phases:

## Decisions added/updated

List decisions added or updated.

## Tests / validation run

List exact commands run.

If no tests were run, write:

```txt
Unity tests were not run because this was a documentation-only phase.
```
````

## Validation results

Use one of:

- PASS
- FAIL
- NOT_RUN

Then explain briefly.

## Git status

Paste the exact output of:

```bash
git status --short
```

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

Phase 7C is complete only if:

```txt
documentation reflects the current Phase 7B.4 gameplay loop
ascending progression is no longer described as current gameplay
Door Seal is not described as current gameplay
score-based floor clear is not described as current gameplay
descent Floor 5 -> Ground Floor is documented
5 trials per floor is documented
non-receding threat is documented
intro context is documented
EN/FR localization prep is documented
question localization limitation is documented
roadmap reflects Phase 7B.4 and Phase 7C
decisions include the descent pivot and Door Seal removal
test plan reflects current behavior
playtest notes include current checklist
no gameplay code is changed
no generated Unity files are staged
final report is complete and written in French
````
