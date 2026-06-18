# Don’t Let Her In

## Project Summary

**Don’t Let Her In** is a mobile first-person horror survival quiz prototype.

The player is trapped inside an elevator. At each floor, the doors open onto a creepy corridor. A female entity approaches while the player answers short survival questions.

Fast correct answers push her away.  
Slow correct answers barely help.  
Wrong answers and timeouts bring her closer.  
If she reaches the elevator, the player dies.

Main promise:

> Every second of hesitation brings her closer.

---

## Current Goal

Current milestone:

```txt
Prototype v0.1 — First Fear Loop
```

The goal is not to build the full game yet.

The goal is to prove the core loop:

```txt
Question starts
Timer starts
Creature advances
Player answers
Answer is evaluated
Threat distance changes
Next floor or death
```

The prototype should answer this question:

> Is it tense and fun to answer short questions while watching a creature approach?

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
3 to 5 floors
5 to 10 questions
timer
answer buttons
threat distance
stress
wrong answer feedback
timeout feedback
death
victory
restart
basic result screen
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
Documentation setup in progress
Unity project not yet implemented
No gameplay code yet
No Unity scene yet
No tests yet
No iOS build yet
```

---

## Next Planned Step

After documentation is complete:

```txt
Create Unity 6 URP project inside UnityProject/
Configure iOS as initial target
Create clean Unity folder structure
Create Game.unity
Validate project opens
Commit project foundation
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
