# Don’t Let Her In

## Project Summary

**Don’t Let Her In** is a mobile portrait horror elevator trial prototype.

The player wakes up high inside a sinister building, trapped in an elevator with the doors open. At each floor, a hallway threat (the creature) approaches. The objective is to **descend floor by floor and reach the Ground Floor to escape**.

Each floor holds **5 trials** (short survival challenges). The threat **never recedes during a floor**:

- Correct answer: consumes the trial, lets the player continue. The threat does not move back.
- Wrong answer: consumes the trial, the threat moves closer.
- Timeout: consumes the trial, the threat moves closer strongly.

Surviving all 5 trials of a floor closes the doors and descends to the next floor down. If the threat reaches the elevator, **SHE GOT IN**. Surviving Floor 1 reaches the **Ground Floor — YOU ESCAPED**.

Main promise:

> Every second of hesitation brings her closer.

---

## Current Goal

Current milestone:

```txt
Prototype v0.1 — First Fear Loop (descent)
```

The goal is not to build the full game yet.

The goal is to prove the core descent loop:

```txt
Floor starts (threat reset to this floor's start distance)
Trial starts (1 of 5)
Timer starts
Player answers
Answer is evaluated (correct / wrong / timeout)
Trial is consumed; wrong/timeout move the threat closer
Repeat until all 5 trials survived -> doors close -> descend
Reach Ground Floor -> escape, or threat reaches elevator -> caught
```

The prototype should answer this question:

> Is it tense and fun to answer short trials while watching a creature approach during a descent?

---

## Platform Strategy

Initial platform:

```txt
iOS mobile portrait
```

Primary test device:

```txt
iPhone 16 Pro
```

Secondary platform:

```txt
Android
```

Future possible platform:

```txt
VR/XR
```

VR is not part of Prototype v0.1.

---

## Tech Stack

```txt
Engine: Unity 6
Rendering: URP
Language: C#
Initial target: iOS
Orientation: portrait
Testing: Unity Test Framework
Data strategy: ScriptableObjects where practical
Version control: Git
```

---

## Prototype v0.1 Scope

Included:

```txt
one Unity scene
one fixed elevator camera
one elevator placeholder
one corridor placeholder
one creature placeholder
descent: Floor 5 -> Floor 4 -> Floor 3 -> Floor 2 -> Floor 1 -> Ground Floor
5 trials per floor (25 trials total)
narrative intro before the run
timer
answer buttons
non-receding threat distance (threat does not move back during a floor)
stress
wrong answer feedback
timeout feedback
loss when the threat reaches the elevator (SHE GOT IN)
escape after surviving Floor 1 (Ground Floor — YOU ESCAPED)
restart
basic result screen
EN/FR localization prep for UI/status/intro strings (question content still EN)
basic logic tests
```

Excluded:

```txt
VR
ads
shop
monetization
cloud save
online leaderboard
multiple creatures
multiple environments
final art
complex story
procedural generation
free movement
inventory
cinematics
```

---

## Repository Structure

```txt
dont-let-her-in/
  AGENTS.md
  README.md

  Docs/
    PRD.md
    GAME_DESIGN.md
    ART_DIRECTION.md
    TECH_ARCHITECTURE.md
    ROADMAP.md
    TEST_PLAN.md
    DECISIONS.md
    PLAYTEST_NOTES.md

  Skills/
    unity-gameplay-loop/
      SKILL.md
    horror-game-design/
      SKILL.md
    unity-scene-assembly/
      SKILL.md
    unity-mobile-performance/
      SKILL.md
    unity-testing/
      SKILL.md
    game-agent-delivery/
      SKILL.md

  UnityProject/
    Assets/
    Packages/
    ProjectSettings/
```

---

## Documentation

Before coding, read:

```txt
AGENTS.md
Docs/PRD.md
Docs/GAME_DESIGN.md
Docs/ART_DIRECTION.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
```

### `AGENTS.md`

Main rule file for coding agents.

Defines:

- scope
- architecture rules
- testing rules
- Git rules
- delivery format
- current milestone

### `Docs/PRD.md`

Product requirements document.

Defines:

- product goal
- target player
- prototype scope
- player flow
- success criteria

### `Docs/GAME_DESIGN.md`

Gameplay design document.

Defines:

- core loop
- threat distance
- answer outcomes
- wrong answer behavior
- timeout behavior
- challenge types
- floor structure

### `Docs/ART_DIRECTION.md`

Visual and atmosphere direction.

Defines:

- elevator look
- corridor look
- creature silhouette
- lighting direction
- UI mood
- placeholder rules

### `Docs/TECH_ARCHITECTURE.md`

Technical structure.

Defines:

- Unity folder structure
- scene hierarchy
- core systems
- ScriptableObjects
- runtime models
- tests
- build rules

### `Docs/ROADMAP.md`

Step-by-step production roadmap.

Defines:

- phases
- deliverables
- acceptance criteria
- recommended commits

### `Docs/TEST_PLAN.md`

Testing strategy.

Defines:

- EditMode tests
- PlayMode tests
- manual checks
- iOS checks
- Git hygiene checks

### `Docs/DECISIONS.md`

Project decision log.

Defines:

- accepted decisions
- open decisions
- replaced decisions

---

## Skills

The `Skills/` folder contains task-specific instructions for agents.

### `unity-gameplay-loop`

Use for:

- `GameManager`
- `RunController`
- `QuestionManager`
- `ThreatManager`
- timer
- answer evaluation
- death/victory/restart

### `horror-game-design`

Use for:

- riddles
- wrong-answer consequences
- timeout consequences
- creature pressure
- floor pacing
- fear design

### `unity-scene-assembly`

Use for:

- Unity scene hierarchy
- elevator setup
- corridor setup
- creature anchors
- camera framing
- placeholder scene layout

### `unity-mobile-performance`

Use for:

- URP settings
- mobile performance
- lighting limits
- texture limits
- iOS/Android performance considerations

### `unity-testing`

Use for:

- EditMode tests
- PlayMode tests
- manual test checklists
- regression testing

### `game-agent-delivery`

Use at the end of every task.

Defines:

- final report format
- Git rules
- targeted commit style
- honesty requirements

---

## Current Development Status

```txt
Latest gameplay commit: Phase 7B.4 — descent loop, intro context and localization prep
Current tests: 148/148 EditMode passing
Playable descent loop: Floor 5 -> Ground Floor, 5 trials per floor
Door Seal / score-based floor clear: removed from active gameplay (Phase 7B.4)
Narrative intro before the run: present
EN/FR localization prep: present for UI/status/intro strings (question content still EN)
No iOS build yet
```

Art and audio remain placeholders; this is a prototype, not a final-quality build.

---

## Next Planned Step

The descent loop and its documentation (Phase 7C) are in place. Recommended next phases:

```txt
Phase 7D — Playtest Polish / Flow Readability
Phase 7E — Question Content Localization EN/FR
Phase 8 — Mobile Build Readiness (iOS portrait)
Phase 9 — Visual / Horror Scene Polish
```

---

## Development Rules

Do not build the full game immediately.

Do:

```txt
build small
test often
commit cleanly
use placeholders
validate gameplay before art polish
keep mobile portrait in mind
keep the agent scoped
```

Do not:

```txt
add VR early
add monetization early
import huge asset packs
add multiple creatures
add procedural generation
create free movement
overbuild systems
commit generated Unity folders
use git add .
```

---

## Git Rules

Never use:

```bash
git add .
```

Use targeted adds.

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
*.apk
*.aab
*.ipa
*.app
*.exe
*.dmg
*.zip
.env
.env.local
```

Example targeted commit:

```bash
git add README.md AGENTS.md Docs Skills .gitignore
git commit -m "📝 docs(project): add prototype planning framework"
```

---

## Prototype v0.1 Definition of Done

Prototype v0.1 is done when:

```txt
player can start a run
question appears
timer starts
creature advances
player can answer
correct answer affects distance positively
wrong answer affects distance negatively
timeout affects distance more negatively
creature reacts visually to distance
player can die
player can win
result screen appears
restart works
UI is readable in portrait
no blocking console errors
core logic tests exist
docs match current behavior
git status is clean after commit
```

---

## Current Project Direction

The first playable version can be visually simple.

The priority order is:

```txt
functional
readable
testable
tense
mobile-friendly
atmospheric
polished
```

Not:

```txt
beautiful first
complex first
VR first
monetized first
content-heavy first
```
