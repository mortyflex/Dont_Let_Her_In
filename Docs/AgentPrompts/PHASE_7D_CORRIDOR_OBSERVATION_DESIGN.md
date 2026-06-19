# Agent Prompt — Phase 7D Corridor Observation & Evidence-Based Trials Design

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
This phase defines the next core gameplay layer: corridor observation before trials. It is primarily design/architecture documentation with light code inspection, not a visual/camera implementation phase. Keep Claude for continuity and careful alignment with the current descent loop.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
📝 docs(gameplay): design corridor observation trials
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

Latest completed phases:

```txt
Phase 7B.4 — Descent Loop, Intro Context & Localization Prep
Phase 7C — Documentation Alignment
```

Current known test status:

```txt
148/148 EditMode tests passed after Phase 7B.4.
Phase 7C was documentation-only.
```

---

## Required Reading Before Editing

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/PRD.md
Docs/GAME_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Docs/PLAYTEST_NOTES.md
Docs/AgentPrompts/PHASE_7D_CORRIDOR_OBSERVATION_DESIGN.md
Docs/AgentPrompts/PHASE_7C_DOCUMENTATION_ALIGNMENT.md
Docs/AgentPrompts/PHASE_7B4_DESCENT_LOOP_INTRO_LOCALIZATION.md
Skills/horror-game-design/SKILL.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Also inspect current code to understand the existing trial flow:

```txt
UnityProject/Assets/Scripts/GameLoop/PlayableRunFlowController.cs
UnityProject/Assets/Scripts/GameLoop/DescentFloorProfile.cs
UnityProject/Assets/Scripts/GameLoop/PrototypeLocalization.cs
UnityProject/Assets/Scripts/GameLoop/TrialFlowResolver.cs
UnityProject/Assets/Scripts/Questions/PrototypeFloorSet.cs
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Scripts/UI/GameplayUIController.cs
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code names, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Mission

Implement:

```txt
Phase 7D — Corridor Observation & Evidence-Based Trials Design
```

This is a design and architecture preparation phase.

The goal is to define how future trials will become evidence-based corridor observation puzzles.

Do not implement camera travelling yet.

Do not implement a full visual clue system yet.

Do not change gameplay code unless a tiny comment/documentation-only adjustment is clearly necessary.

This phase should update docs and optionally add one dedicated design document.

---

## Design Direction

The game should evolve from:

```txt
The player answers abstract questions in an elevator.
```

to:

```txt
The player observes a hallway, memorizes details, returns to the elevator, then answers trials based on what was actually visible.
```

Desired floor loop:

```txt
Floor starts
Doors open
Observation pass begins
Camera slowly travels forward into the hallway
Player sees corridor clues: door numbers, symbols, lights, messages, objects, anomalies
Camera slowly travels backward to the elevator
Trials begin
Each trial asks about a clue that was visible during observation
Wrong/timeout makes the threat approach
Correct consumes the trial but does not push threat back
Surviving all 5 trials descends to the next floor
```

Important:

```txt
The corridor should feel structurally similar across floors.
Specific details change from floor to floor.
The player learns the hallway layout, then notices differences/anomalies.
Trials must never feel random or disconnected from the corridor.
```

---

## Core Principle

Every trial should have visible evidence.

Official rule:

```txt
No trial without a corridor clue.
No correct answer without observable evidence.
No answer option that feels completely random.
```

The player should be able to say:

```txt
I saw this in the hallway, so I know the answer.
```

Not:

```txt
The game asked me a random quiz question.
```

---

## Required Documentation Output

Update these docs if needed:

```txt
Docs/GAME_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Docs/PLAYTEST_NOTES.md
```

Create this new document:

```txt
Docs/CORRIDOR_OBSERVATION_DESIGN.md
```

Do not rewrite all docs from scratch.

Make targeted, coherent updates.

---

## New Document Requirements

Create:

```txt
Docs/CORRIDOR_OBSERVATION_DESIGN.md
```

It must define the future observation/trial system.

Include these sections:

```txt
# Corridor Observation Design

## Purpose

## Player Experience

## Floor Loop With Observation

## Corridor Structure

## Clue Types

## Evidence-Based Trial Rules

## Data Model Proposal

## Localization Considerations

## Prototype Implementation Plan

## Out of Scope For Now

## Acceptance Checklist
```

### Purpose

Explain that corridor observation exists to make trials feel grounded, memorable, and fair.

### Player Experience

Describe the intended rhythm:

```txt
observe
remember
return
answer
survive
descend
```

### Floor Loop With Observation

Document the future flow:

```txt
intro / previous descent
doors open
observation camera travel forward
clue exposure
observation camera travel backward
trial sequence
floor clear / loss
```

### Corridor Structure

Document that the corridor can remain mostly consistent while details vary.

Examples:

```txt
same elevator framing
same hallway depth
same door positions
same wall panels
same light fixtures
different numbers/symbols/messages/objects/anomalies per floor
```

### Clue Types

Define clue categories.

Examples:

```txt
DoorNumber
WallMessage
Symbol
LightState
ObjectPlacement
Anomaly
ColorCue
AudioProxy
ShadowOrSilhouette
DirectionInstruction
```

For each category, include:

```txt
what the player sees
how a trial can ask about it
what makes it fair/unfair
```

### Evidence-Based Trial Rules

Define rules such as:

```txt
each trial must reference a clueId
each clue must be observable before the trial
each correct answer must be present in the observed clue
distractors should be plausible but not arbitrary
a cue can be repeated in UI only if it represents memory/recall, not direct answer leaking
avoid asking about details too small for mobile portrait
avoid requiring color-only recognition unless accessible alternative exists
avoid relying on audio-only clue until real audio exists
```

### Data Model Proposal

Propose simple future types.

Possible names:

```txt
CorridorClue
CorridorClueType
FloorObservationSet
EvidenceTrial
EvidenceAnswerOption
ObservationPhaseController
CorridorObservationController
```

Define fields conceptually.

Example:

```txt
CorridorClue:
- id
- type
- floorDisplayNumber
- label
- localizedDescription
- visualAnchor
- evidenceValue
- difficultyWeight
- isRequiredForTrial
```

Example:

```txt
EvidenceTrial:
- id
- clueId
- prompt
- answers
- correctAnswerId
- timeLimit
- difficulty
- localization
```

Do not implement these classes unless explicitly needed later.

For this phase, document the model.

### Localization Considerations

Document:

```txt
EN/FR should apply to prompts, answers, clue descriptions, intro, UI/status.
Code identifiers stay English.
Visual clues should avoid language dependence when possible, but text clues require localization.
Question content is currently English-only and should be localized in a future phase.
```

### Prototype Implementation Plan

Recommend future implementation in phases.

Example:

```txt
Phase 7E — Question Content Localization EN/FR
Phase 7F — Evidence Trial Data Model
Phase 7G — Static Corridor Clue Prototype
Phase 7H — Observation Camera Pass Prototype
Phase 7I — Evidence-Based Floor 5 Playtest
```

Or propose a better sequence if justified.

### Out of Scope For Now

Explicitly exclude:

```txt
camera travelling implementation
animation polish
final corridor art
procedural clue generation
full localization of all questions
real audio clue system
jumpscare cinematic
pathfinding enemy
```

### Acceptance Checklist

Include a checklist future agents can use.

---

## Required Updates In Existing Docs

### Docs/GAME_DESIGN.md

Add a section for:

```txt
Corridor Observation and Evidence-Based Trials
```

Document that this is the intended next layer, not fully implemented yet.

Make clear:

```txt
Current gameplay already has 5 trials per floor.
Future trial content should be grounded in corridor clues.
```

Do not claim camera travel is implemented.

### Docs/TECH_ARCHITECTURE.md

Add a future architecture subsection for observation/trial evidence.

Mention proposed future systems:

```txt
CorridorClue
FloorObservationSet
EvidenceTrial
ObservationPhaseController
CorridorObservationController
```

Clearly mark them as planned/proposed, not current implementation.

### Docs/ROADMAP.md

Add Phase 7D as completed/current design phase.

Add possible next phases.

Recommended next sequence:

```txt
Phase 7E — Question Content Localization EN/FR
Phase 7F — Evidence Trial Data Model
Phase 7G — Static Corridor Clue Prototype
Phase 7H — Observation Camera Pass Prototype
Phase 7I — Evidence-Based Floor Playtest
```

Important: do not jump directly to final art.

### Docs/TEST_PLAN.md

Add future test requirements:

```txt
every EvidenceTrial references a clueId
every referenced clue exists in the FloorObservationSet
each floor has enough visible clues for its trials
clue text and trial prompt localize EN/FR
mobile readability constraints for clue size/contrast
observation phase can complete and hand off to trials
```

### Docs/DECISIONS.md

Add decisions:

```txt
Trials should become corridor-evidence-based.
Corridor layout should remain mostly consistent while clues vary.
Observation camera pass should be introduced before trial sequence in a future phase.
No trial should be disconnected from visible evidence.
```

Each decision should include:

```txt
Context
Decision
Consequence
Status
```

Status:

```txt
Accepted / Planned
```

### Docs/PLAYTEST_NOTES.md

Add a future playtest checklist for observation mechanics:

```txt
Does the observation phase feel too long?
Can the player remember 5 clues?
Are clues readable in portrait?
Do trials feel fair?
Do answers feel connected to what was seen?
Are distractors plausible?
Does the camera travel create tension or frustration?
```

---

## Current Implementation Boundaries

Do not remove or rewrite the current 25 prototype questions.

Do not implement the observation camera in this phase.

Do not modify `Game.unity` unless strictly necessary for documentation comments, which should not be necessary.

Do not change `PlayableRunFlowController` logic.

Do not change threat tuning.

Do not change localization code.

This phase is intended to prepare the next implementation phases.

---

## Validation

Run:

```bash
git status --short
```

If Markdown tooling exists in the repo, run it.

If no Markdown tooling exists, do not invent tooling.

Unity tests are not required because this should be documentation-only.

If any code changes occur unexpectedly, stop and explain why before committing.

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
git add Docs/CORRIDOR_OBSERVATION_DESIGN.md \
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
git commit -m "📝 docs(gameplay): design corridor observation trials"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 7D Corridor Observation & Evidence-Based Trials Design

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

## Corridor observation design confirmation

Confirm each item:

- Corridor observation loop documented: yes/no
- Observation before trials documented: yes/no
- Slow forward/backward camera pass documented as planned: yes/no
- Corridor mostly consistent across floors documented: yes/no
- Floor-specific clues/anomalies documented: yes/no
- Evidence-based trial rule documented: yes/no
- No-trial-without-clue rule documented: yes/no
- Mobile portrait readability constraints documented: yes/no
- EN/FR localization considerations documented: yes/no

## New design document confirmation

Confirm each required section exists in `Docs/CORRIDOR_OBSERVATION_DESIGN.md`:

- Purpose: yes/no
- Player Experience: yes/no
- Floor Loop With Observation: yes/no
- Corridor Structure: yes/no
- Clue Types: yes/no
- Evidence-Based Trial Rules: yes/no
- Data Model Proposal: yes/no
- Localization Considerations: yes/no
- Prototype Implementation Plan: yes/no
- Out of Scope For Now: yes/no
- Acceptance Checklist: yes/no

## Existing docs updated

Confirm each item:

- GAME_DESIGN updated: yes/no
- TECH_ARCHITECTURE updated: yes/no
- ROADMAP updated: yes/no
- TEST_PLAN updated: yes/no
- DECISIONS updated: yes/no
- PLAYTEST_NOTES updated: yes/no

## Decisions added/updated

List decisions added or updated.

## Proposed next phases

List the recommended next phases.

## Validation run

List exact commands run.

If no Unity tests were run, write:

```txt
Unity tests were not run because this was a documentation-only design phase.
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

Phase 7D is complete only if:

```txt
Docs/CORRIDOR_OBSERVATION_DESIGN.md exists
corridor observation loop is clearly documented
observation before trials is documented as planned
slow forward/backward camera pass is documented as planned, not implemented
corridor consistency + changing details/anomalies are documented
evidence-based trial rule is documented
no-trial-without-clue rule is documented
proposed data model exists in documentation
EN/FR localization considerations are documented
mobile portrait clue readability constraints are documented
existing docs are updated with planned observation layer
roadmap includes next observation/evidence phases
test plan includes future evidence-trial validation
decisions record evidence-based trial direction
playtest notes include observation checklist
no gameplay code is changed
no scene/art changes are made
no generated Unity files are staged
final report is complete and written in French
````
