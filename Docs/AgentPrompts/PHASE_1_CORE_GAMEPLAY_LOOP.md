# Agent Prompt — Phase 1 Core Gameplay Loop

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
This phase creates the first real gameplay architecture foundation: Unity C# scripts, core run logic, threat rules, death/win/restart behavior and EditMode tests. It must stay conservative, scoped and testable.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
🎮 feat(gameplay): add core threat run loop
```

---

## Project

You are working on the Unity project:

```txt
Don’t Let Her In
```

This is a mobile-first iOS portrait horror prototype made with Unity 6 URP.

The player is trapped in an elevator. A creature approaches from a corridor while the player answers short survival questions.

Main promise:

```txt
Every second of hesitation brings her closer.
```

---

## Required Reading Before Coding

Read these files before making changes:

```txt
AGENTS.md
README.md
Docs/PRD.md
Docs/GAME_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/ROADMAP.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Do not start coding until you have understood the current milestone and the scope limits.

---

## Current Project State

The Unity project already exists here:

```txt
UnityProject/
```

The project is:

```txt
Unity 6
URP
iOS-first
Portrait orientation
```

The main scene exists here:

```txt
UnityProject/Assets/Scenes/Game.unity
```

The current milestone is:

```txt
Prototype v0.1 — First Fear Loop
```

The target core loop is:

```txt
Question starts
Timer starts
Creature advances
Player answers
Answer is evaluated
Threat distance changes
Next floor or death
```

---

## Mission

Implement:

```txt
Phase 1 — Core Gameplay Loop
```

The goal is to create the first testable gameplay logic foundation.

This phase is logic-first.

Do not build the full game.

Do not create final UI.

Do not create final art.

Do not assemble the elevator/corridor scene yet.

Do not add creature visuals beyond what is strictly necessary for compilation.

Do not add audio.

Do not add monetization.

Do not add VR/XR.

Do not add Android-specific work.

Do not add cloud save.

Do not add analytics.

Do not add online features.

Do not add procedural generation.

Do not add inventory.

Do not add free movement.

Do not add multiple creatures.

---

## Phase 1 Scope

Included:

```txt
GameState enum
GameManager
RunController
RunResult
ThreatManager
ThreatState
death condition
victory condition
restart/reset logic
EditMode tests for core logic
```

Excluded:

```txt
final UI
final creature visuals
elevator scene assembly
corridor scene assembly
audio
animations
mobile build export
ScriptableObject question bank
complex question system
PlayMode scene tests unless simple and useful
```

---

## Required Folder Locations

Use the existing folders.

Core scripts:

```txt
UnityProject/Assets/Scripts/Core/
```

Game loop scripts:

```txt
UnityProject/Assets/Scripts/GameLoop/
```

Threat scripts:

```txt
UnityProject/Assets/Scripts/Threat/
```

EditMode tests:

```txt
UnityProject/Assets/Tests/EditMode/
```

---

## Required Files

Create or update these files:

```txt
UnityProject/Assets/Scripts/Core/GameState.cs
UnityProject/Assets/Scripts/Core/GameManager.cs
UnityProject/Assets/Scripts/GameLoop/RunController.cs
UnityProject/Assets/Scripts/GameLoop/RunResult.cs
UnityProject/Assets/Scripts/Threat/ThreatManager.cs
UnityProject/Assets/Scripts/Threat/ThreatState.cs
UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs
UnityProject/Assets/Tests/EditMode/RunControllerTests.cs
```

If you need small helper enums or models, keep them minimal and place them in the correct folder.

Do not create unrelated systems.

---

## Required GameState Values

Create this enum:

```csharp
public enum GameState
{
    Boot,
    MainMenu,
    RunStart,
    ElevatorIdle,
    QuestionActive,
    ResolvingAnswer,
    FloorTransition,
    CreatureAttack,
    RunWon,
    RunLost,
    Results
}
```

---

## Threat System Rules

Use these prototype values:

```txt
Initial distance: 70
Minimum distance: 0
Maximum distance: 100

Initial stress: 0
Minimum stress: 0
Maximum stress: 4

Correct fast: +18 distance, -1 stress
Correct normal: +10 distance
Correct slow: +3 distance

Wrong answer: -20 distance, +1 stress
Timeout: -30 distance, +2 stress

Death: distance <= 0
```

Distance must always be clamped between:

```txt
0 and 100
```

Stress must always be clamped between:

```txt
0 and 4
```

---

## ThreatManager Requirements

`ThreatManager` should expose logic for:

```txt
initializing distance and stress
applying correct fast answer
applying correct normal answer
applying correct slow answer
applying wrong answer
applying timeout
resetting state
checking death
returning current ThreatState
```

Prefer testable logic.

It is acceptable for `ThreatManager` to be a regular C# class.

If you choose to make it a MonoBehaviour, keep the rules testable through pure methods or a separate logic layer.

Do not make the gameplay rules impossible to test outside the scene.

---

## ThreatState Requirements

`ThreatState` should contain at least:

```txt
Distance
StressLevel
IsDead
LastDistanceDelta
LastStressDelta
```

Use clear naming.

Keep it simple.

---

## RunController Requirements

`RunController` should support:

```txt
start run
restart run
track current floor
track total floors
track correct answers
track wrong answers
track timeouts
track floors completed
detect win after final floor
detect loss when threat death occurs
produce RunResult
```

Default prototype floor count:

```txt
5
```

Run logic must be testable without final UI.

---

## RunResult Requirements

`RunResult` should contain at least:

```txt
Won
Lost
FloorsCompleted
CorrectAnswers
WrongAnswers
Timeouts
FinalDistance
FinalStress
```

Optional if simple:

```txt
Score
```

Do not overbuild scoring.

Scoring is not required for Phase 1.

---

## GameManager Requirements

`GameManager` can be minimal in this phase.

It should:

```txt
hold current GameState
start a run
restart a run
set run won
set run lost
expose state changes in a simple way if useful
```

Do not make `GameManager` a giant god object.

Do not put all rules inside `GameManager`.

The main logic should remain in:

```txt
RunController
ThreatManager
```

---

## Required EditMode Tests — ThreatManager

Create EditMode tests for `ThreatManager`.

Required test cases:

```txt
InitialDistance_IsSet
InitialStress_IsSet
Distance_IsClampedAtZero
Distance_IsClampedAtOneHundred
CorrectFast_IncreasesDistance
CorrectFast_ReducesStress
CorrectNormal_IncreasesDistance
CorrectSlow_IncreasesDistanceSlightly
WrongAnswer_DecreasesDistance
WrongAnswer_IncreasesStress
Timeout_DecreasesDistanceMoreThanWrongAnswer
Timeout_IncreasesStressMoreThanWrongAnswer
Death_IsTriggeredWhenDistanceReachesZero
Death_IsNotTriggeredWhenDistanceIsAboveZero
Reset_RestoresInitialState
```

---

## Required EditMode Tests — RunController

Create EditMode tests for `RunController`.

Required test cases:

```txt
Run_StartsAtFirstFloor
Run_TracksCurrentFloor
Run_AdvancesAfterFloorCompleted
Run_WinsAfterFinalFloor
Run_LosesWhenThreatDeathOccurs
Run_RestartResetsState
Run_TracksCorrectAnswers
Run_TracksWrongAnswers
Run_TracksTimeouts
RunResult_ReflectsCurrentRunState
```

Use Unity Test Framework.

If Unity Test Runner cannot be executed in your environment, still create the tests and clearly state that they were not run.

Never claim tests passed unless they were actually executed.

---

## Manual Checks

After implementation, check if possible:

```txt
Unity project opens
No blocking console errors
Scripts compile
Game.unity still exists
No generated Unity folders are staged
```

Do not build iOS in this phase.

Do not generate a Xcode project.

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
```

Use targeted adds only.

Recommended add command:

```bash
git add UnityProject/Assets/Scripts/Core UnityProject/Assets/Scripts/GameLoop UnityProject/Assets/Scripts/Threat UnityProject/Assets/Tests/EditMode
```

Recommended commit message:

```bash
git commit -m "🎮 feat(gameplay): add core threat run loop"
```

---

## Required Final Report

End your response with this exact structure:

````md
### Model used

Claude

### Summary

What changed.

### Files changed

- `path/to/file`

### Tests run

- Exact test command or Unity Test Runner action.

### Results

Pass/fail result.

### Manual checks

What was checked manually.

### Known limits

What is incomplete or not verified.

### Git status

Output of `git status --short`.

### Commit

Commit hash and message if committed.

If not committed, provide the recommended commit:

```bash
git add UnityProject/Assets/Scripts/Core UnityProject/Assets/Scripts/GameLoop UnityProject/Assets/Scripts/Threat UnityProject/Assets/Tests/EditMode
git commit -m "🎮 feat(gameplay): add core threat run loop"
```
````

````

If tests were not run, write:

```txt
Tests were not run because Unity Editor / Unity Test Runner was unavailable in this environment.
````

Do not write:

```txt
Tests should pass.
```

---

## Acceptance Criteria

Phase 1 is complete only if:

```txt
GameState exists
GameManager exists
RunController exists
RunResult exists
ThreatManager exists
ThreatState exists
Threat rules are implemented
Death condition works
Win condition works
Restart/reset works
EditMode tests exist
No final UI added
No final art added
No scene overbuild
No forbidden generated folders staged
Agent final report is complete
```
