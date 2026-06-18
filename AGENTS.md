# AGENTS.md — Don’t Let Her In

## Project identity

Project name: Don’t Let Her In  
Genre: mobile first-person horror survival quiz  
Engine: Unity 6  
Rendering: URP  
Language: C#  
Initial platform: iOS mobile portrait  
Future platform: Android, then possible VR/XR  
Current goal: playable prototype, not full game

## Core concept

The player is trapped inside an elevator. At each floor, the doors open onto a haunted corridor. A creepy female entity approaches while the player answers short survival questions.

Fast correct answers push her away.  
Slow correct answers barely help.  
Wrong answers and timeouts bring her closer.  
If she reaches the elevator, the player dies.

Main promise:

> Every second of hesitation brings her closer.

## Language Rule

The user communicates in French.

All agent delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

Do not translate code or commit messages into French.

## Current prototype scope

Build a small playable vertical slice.

Included:

- one elevator
- one corridor
- one creature
- fixed first-person camera
- mobile portrait UI
- 3 to 5 floors
- 5 to 10 questions
- timer
- correct/wrong/timeout handling
- creature distance system
- basic death
- basic victory
- restart
- basic sound placeholders
- basic horror feedback

Excluded:

- VR
- ads
- shop
- multiple monsters
- procedural level generation
- account system
- cloud save
- complex narrative
- final-quality graphics
- online leaderboard

## Required reading before coding

Before implementing any task, read:

- `Docs/PRD.md`
- `Docs/GAME_DESIGN.md`
- `Docs/ART_DIRECTION.md`
- `Docs/TECH_ARCHITECTURE.md`
- `Docs/ROADMAP.md`
- `Docs/TEST_PLAN.md`
- `Docs/DECISIONS.md`

If a requested change conflicts with these documents, follow the user request but explicitly report the conflict.

## Development philosophy

This project must be built in small phases.

Do not try to build the full game.  
Do not chase final graphics first.  
Do not add systems that are not needed for the current milestone.  
Do not refactor unrelated systems.  
Do not add dependencies without justification.

The first goal is to prove this loop:

```txt
Question starts
Timer starts
Creature advances
Player answers
Answer is evaluated
Threat distance changes
Next floor or death
```

## Architecture rules

Use simple modular architecture.

Recommended modules:

- `Core`
- `GameLoop`
- `Questions`
- `Threat`
- `Creature`
- `Elevator`
- `UI`
- `Audio`
- `Save`
- `Tools`
- `Tests`

Core systems:

- `GameManager`
- `RunController`
- `QuestionManager`
- `ThreatManager`
- `CreatureController`
- `ElevatorController`
- `FloorDirector`
- `AudioDirector`

Use a clear game state machine.

Prototype states:

- `Boot`
- `MainMenu`
- `RunStart`
- `ElevatorIdle`
- `QuestionActive`
- `ResolvingAnswer`
- `FloorTransition`
- `CreatureAttack`
- `RunWon`
- `RunLost`
- `Results`

The architecture must stay replaceable. The prototype may use placeholders, but the systems should allow later replacement with better art, better audio, better UI and more complex level data without rewriting the whole game.

Avoid:

- god classes
- circular dependencies
- scene-only logic that cannot be tested
- putting all gameplay logic inside MonoBehaviours
- hardcoding core gameplay data in scripts

Prefer:

- small focused classes
- explicit responsibilities
- simple events
- pure logic classes where possible
- data-driven configuration
- readable names
- clear Inspector references

## Data rules

Do not hardcode question content inside gameplay scripts.

Questions, floors, difficulty, creature settings and horror events should be data-driven where possible.

Use ScriptableObjects for:

- `QuestionData`
- `FloorData`
- `CreatureData`
- `DifficultyData`
- `AudioCueData`
- `HorrorEventData`

Runtime state should not be stored in ScriptableObjects unless explicitly intended.

Question data should support at least:

- `id`
- `type`
- `prompt`
- `answers`
- `correctAnswerIndex`
- `timeLimitSeconds`
- `difficulty`
- `fastCorrectReward`
- `normalCorrectReward`
- `slowCorrectReward`
- `wrongAnswerPenalty`
- `timeoutPenalty`
- `optionalVisualClueId`
- `optionalAudioClueId`
- `tags`

Floor data should support at least:

- `floorIndex`
- `floorLabel`
- `questionList`
- `initialCreatureDistance`
- `creatureAdvanceSpeed`
- `lightingMood`
- `optionalHorrorEvent`
- `optionalClueAnchors`

## Prototype gameplay rules

Threat distance:

```txt
Range: 0 to 100
100: creature far
0: death
```

Distance interpretation:

```txt
100: creature invisible or very far
80: silhouette at the end of the corridor
60: visible creature
40: mid corridor
25: near the elevator doors
10: at the doors
0: death
```

Answer effects:

```txt
Correct fast: +18 distance, stress -1
Correct normal: +10 distance
Correct slow: +3 distance
Wrong answer: -20 distance, stress +1
Timeout: -30 distance, stress +2
Death: distance <= 0
```

Wrong answer feedback:

- red UI glitch
- brief blackout
- harsh sound cue
- creature jumps closer
- combo reset
- stress increase
- next question may start with slightly worse conditions

Timeout feedback:

- stronger than wrong answer
- question disappears
- light failure
- creature moves closer
- stress increases more
- doors may jam briefly

Correct fast feedback:

- creature gets pushed back
- elevator light stabilizes briefly
- subtle positive sound cue
- combo increases
- short feeling of relief

Correct slow feedback:

- creature stops or barely recedes
- no strong relief
- combo may reset depending on implementation
- pressure remains high

Death feedback:

- distance reaches 0
- question is cancelled
- doors fail
- creature reaches elevator
- jumpscare or attack event plays
- run ends
- result screen appears

## Riddle and challenge rules

The game must not feel like a school quiz.

Prototype challenge types allowed:

- observation
- short memory
- simple audio clue
- environmental instruction
- simple logic
- sang-froid instruction
- anomaly

Avoid in the prototype:

- long riddles
- obscure trivia
- complex lore puzzles
- large text blocks
- puzzles requiring inventory
- puzzles requiring free movement
- unfair random answers

Every challenge must be short enough to understand under pressure.

The ideal challenge creates this feeling:

> I know what to do, but she is getting closer and I am panicking.

## Scene rules

The player does not move freely in the prototype.

Camera:

- fixed first-person viewpoint inside elevator
- portrait mobile framing
- corridor visible through open doors
- no free-look requirement in prototype
- no joystick
- no walking
- no inventory

Do not add:

- free movement
- inventory
- complex physics
- multiple camera modes
- cutscenes longer than a few seconds
- complex enemy AI

Scene hierarchy should stay clean.

Recommended scene hierarchy:

```txt
SceneRoot
  GameSystems
  Elevator
  Corridor
  Creature
  Lighting
  UI
  Audio
```

Recommended `GameSystems` children:

```txt
GameSystems
  GameManager
  RunController
  QuestionManager
  ThreatManager
  FloorDirector
  AudioDirector
```

Recommended `Elevator` children:

```txt
Elevator
  ElevatorInterior
  DoorLeft
  DoorRight
  ButtonPanel
  DigitalDisplay
```

Recommended `Corridor` children:

```txt
Corridor
  Floor
  Walls
  Ceiling
  Doors
  Props
  ClueAnchors
```

Recommended `Creature` children:

```txt
Creature
  ModelOrSilhouette
  AnimationRoot
  AudioSource
  PositionAnchors
```

Creature position anchors:

- `Far`
- `Visible`
- `MidCorridor`
- `NearDoor`
- `AtDoor`

## Mobile rules

Mobile-first means:

- large touch targets
- readable text
- portrait layout
- short sessions
- fast restart
- no tiny UI buttons
- no desktop-only controls
- performance-conscious scene setup

Target:

- 30 FPS minimum for prototype
- stable Play Mode
- no blocking console errors
- no layout that only works on desktop
- no required keyboard input
- no required mouse precision

Touch UI rules:

- primary buttons must be thumb-friendly
- answer buttons must be easy to tap quickly
- no tiny close buttons
- no hidden critical actions
- no long text in answer buttons
- no UI blocking the creature unless intentionally designed

## Art rules

The concept art defines mood, not v0.1 production quality.

Prototype art can use placeholders.

Keep:

- dark elevator
- creepy corridor
- one visible silhouette
- flickering light
- dirty hotel/hospital tone
- green/yellow/red sick lighting
- claustrophobic framing
- strong contrast between safe elevator and unsafe corridor

Avoid:

- bright cartoon horror
- overloaded UI
- gore focus
- excessive jumpscares
- photorealism requirement in prototype
- too many different locations in v0.1
- too many creatures in v0.1

The first playable prototype can be ugly if the gameplay loop is readable.

Final art direction target:

- stylized realistic 3D
- dark corridor
- cinematic lighting
- dirty elevator
- unsettling female silhouette
- strong audio atmosphere
- minimal but polished UI

## Audio rules

Audio is a priority for the horror feeling.

Prototype audio placeholders should include:

- elevator hum
- door open/close
- flickering light
- wrong answer hit
- timeout hit
- creature step or scrape
- creature close sound
- attack sound
- subtle ambience loop

Audio should become more intense when the creature is closer.

Do not overuse loud jumpscares.  
Use silence, low frequencies, footsteps and breathing to build tension.

## Code style

Use C# conventions:

- PascalCase for public types, methods and properties
- camelCase for local variables
- `_camelCase` or `camelCase` for private fields, but stay consistent once chosen
- `[SerializeField] private` fields for Inspector-exposed references
- avoid public mutable fields
- keep MonoBehaviours thin when possible
- keep pure logic testable when possible

Do not create god classes.

Prefer:

- small classes
- explicit dependencies
- clear events
- simple data containers
- readable names

Naming examples:

```txt
ThreatManager
ThreatState
AnswerResult
QuestionData
QuestionManager
RunController
CreatureController
FloorDirector
ElevatorController
```

## Testing rules

Use Unity Test Framework.

Use EditMode tests for:

- `ThreatManager`
- answer evaluation
- score calculations
- run progression logic
- question data validation

Use PlayMode tests for:

- scene integration
- UI flow
- floor transition
- death flow
- restart flow

Minimum test expectations for gameplay changes:

- wrong answer changes threat distance
- timeout changes threat distance
- correct answer changes threat distance
- distance clamps correctly
- death triggers at 0
- victory triggers after final floor
- stress increases after wrong answer
- stress increases more after timeout
- correct fast can reduce stress
- run can restart after loss

Every implementation delivery must say:

- what tests were added
- what tests were run
- whether they passed
- what remains untested

If tests cannot be run, state exactly why.

## Git rules

Never use:

```bash
git add .
```

Use targeted `git add` only.

Do not commit:

- `UnityProject/Library/`
- `UnityProject/Temp/`
- `UnityProject/Logs/`
- `UnityProject/UserSettings/`
- build outputs
- generated cache files
- large imported assets unless explicitly requested
- local environment files
- recordings
- screenshots unless explicitly requested

Commit style:

```txt
🎮 feat(gameplay): add threat distance system
👻 feat(creature): add placeholder hallway entity
🛗 feat(elevator): add door transition loop
🔊 feat(audio): add horror feedback cues
📱 feat(mobile): add portrait gameplay UI
🧪 test(gameplay): cover threat manager rules
🧹 chore(project): initialize unity project structure
📝 docs(project): add prototype roadmap
```

After each task:

1. show git status
2. list changed files
3. suggest exact git add command
4. suggest exact commit message

## Delivery format

Every task must end with:

### Summary

What changed.

### Files changed

List files.

### Tests run

Commands or Unity Test Runner actions.

### Results

Pass/fail.

### Manual checks

What was tested in Play Mode.

### Known limits

What is incomplete.

### Git status

Clean or list modified files.

### Recommended commit

Targeted git add and commit command.

## Agent behavior

Be conservative.

Do not invent new product direction.  
Do not add extra features because they seem useful.  
Do not optimize prematurely.  
Do not hide errors.  
Do not claim a build works unless it was actually tested.  
Do not claim mobile performance unless it was measured or manually checked.

When uncertain:

- implement the smallest useful version
- document the uncertainty
- keep the code easy to replace

## Current development priority

Priority 1:

Playable core loop with placeholders.

Priority 2:

Creature visual distance feedback.

Priority 3:

Wrong answer and timeout horror feedback.

Priority 4:

Mobile portrait usability.

Priority 5:

Atmosphere polish.

Anything else is lower priority.

## Current milestone

Milestone: Prototype v0.1 — First Fear Loop

Target:

- one Unity scene
- one fixed elevator camera
- one corridor
- one placeholder creature
- 3 to 5 floors
- 5 to 10 questions
- timer
- threat distance
- correct/wrong/timeout consequences
- death
- victory
- restart
- basic mobile portrait UI
- basic horror feedback

Definition of done:

- player can complete a short run
- player can lose if the creature reaches the elevator
- wrong answers visibly bring the creature closer
- timeout is more dangerous than a wrong answer
- fast correct answers create relief
- restart works
- no blocking console errors
- basic logic has tests
