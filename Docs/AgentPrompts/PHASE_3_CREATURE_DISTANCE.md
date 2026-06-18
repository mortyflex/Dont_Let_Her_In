# Agent Prompt — Phase 3 Creature Distance

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
This phase starts connecting gameplay logic with Unity-facing creature behavior. It must remain conservative, testable and scoped. Keep Claude for continuity after Phase 1 and Phase 2.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
👻 feat(creature): add distance-based hallway threat
```

---

## Project

You are working on the Unity project:

```txt
Don’t Let Her In
```

This is a Unity 6 URP iOS-first portrait horror prototype.

The player is trapped inside an elevator. A female creature approaches from a dark corridor while the player answers short survival questions.

Main promise:

```txt
Every second of hesitation brings her closer.
```

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/AgentPrompts/PHASE_3_CREATURE_DISTANCE.md
Docs/ROADMAP.md
Docs/GAME_DESIGN.md
Docs/ART_DIRECTION.md
Docs/TECH_ARCHITECTURE.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Skills/unity-gameplay-loop/SKILL.md
Skills/horror-game-design/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Also inspect the previous implementations:

```txt
UnityProject/Assets/Scripts/Core/
UnityProject/Assets/Scripts/GameLoop/
UnityProject/Assets/Scripts/Threat/
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Current Project State

Phase 1 has been completed and committed:

```txt
6dd6b73 — 🎮 feat(gameplay): add core threat run loop
```

Phase 1 added:

```txt
GameState
GameManager
RunController
RunResult
ThreatManager
ThreatState
EditMode tests
runtime asmdef
EditMode test asmdef
```

Phase 2 has been completed and committed:

```txt
109dcfd2 — 🎮 feat(questions): add data-driven question system
```

Phase 2 added:

```txt
QuestionType
AnswerSpeed
QuestionData
AnswerResult
QuestionEvaluator
QuestionManager
EditMode tests
```

Current test status:

```txt
56 EditMode tests passed
```

The project is still logic-first.

No final UI, final art, scene assembly, audio or creature visual polish should exist yet.

---

## Mission

Implement:

```txt
Phase 3 — Threat and Creature Distance
```

The goal is to create the first creature distance layer.

This phase should connect the existing threat distance concept to a creature phase model and a minimal Unity-facing controller.

The creature must remain simple.

The creature is not AI-driven in v0.1.

The creature is distance-driven.

---

## Phase 3 Scope

Included:

```txt
CreaturePhase enum
CreatureData ScriptableObject
CreatureDistanceMapper or equivalent pure logic
CreatureController minimal MonoBehaviour
distance-to-phase mapping
optional anchor-based position resolution
EditMode tests for distance-to-phase mapping
safe handling of missing anchors/references
```

Excluded:

```txt
final creature model
final creature art
animations
pathfinding
enemy AI
multiple creatures
jumpscare cinematic
audio
lighting system
full elevator/corridor scene assembly
final UI
PlayMode scene flow unless very small and useful
iOS build/export
VR/XR
Android-specific work
monetization
analytics
cloud save
online features
procedural generation
inventory
free movement
```

---

## Required Folder Locations

Use the existing creature folder:

```txt
UnityProject/Assets/Scripts/Creature/
```

Use the existing tests folder:

```txt
UnityProject/Assets/Tests/EditMode/
```

If you create a minimal placeholder prefab, use:

```txt
UnityProject/Assets/Prefabs/Creature/
```

However, avoid creating prefabs unless strictly useful. This phase can be completed mostly through scripts and tests.

Do not overbuild the scene.

---

## Required Files

Create or update these files:

```txt
UnityProject/Assets/Scripts/Creature/CreaturePhase.cs
UnityProject/Assets/Scripts/Creature/CreatureData.cs
UnityProject/Assets/Scripts/Creature/CreatureDistanceMapper.cs
UnityProject/Assets/Scripts/Creature/CreatureController.cs
UnityProject/Assets/Tests/EditMode/CreatureDistanceMapperTests.cs
```

If a small helper model is needed, keep it minimal and place it in:

```txt
UnityProject/Assets/Scripts/Creature/
```

Do not create unrelated systems.

---

## Required CreaturePhase Values

Create this enum:

```csharp
public enum CreaturePhase
{
    Far,
    Visible,
    MidCorridor,
    NearDoor,
    AtDoor,
    Attack
}
```

---

## Distance-to-Phase Mapping

Use this mapping:

```txt
distance > 80: Far
distance > 60: Visible
distance > 40: MidCorridor
distance > 25: NearDoor
distance > 0: AtDoor
distance <= 0: Attack
```

Boundary behavior must be tested clearly.

This means:

```txt
100 -> Far
81 -> Far
80 -> Visible
61 -> Visible
60 -> MidCorridor
41 -> MidCorridor
40 -> NearDoor
26 -> NearDoor
25 -> AtDoor
1 -> AtDoor
0 -> Attack
negative distance -> Attack
```

Distance values should be clamped or handled safely where appropriate.

Do not assume invalid values will never happen.

---

## CreatureData Requirements

`CreatureData` should be a ScriptableObject.

It should contain at least:

```txt
Id
DisplayName
FarThreshold
VisibleThreshold
MidCorridorThreshold
NearDoorThreshold
AtDoorThreshold
AttackThreshold
BaseAdvanceSpeed
```

Default values should match the prototype mapping.

Keep it simple.

Do not add advanced AI data.

Optional but acceptable if simple:

```txt
WrongAnswerMoveStyle
TimeoutMoveStyle
```

Only add optional fields if they do not create unnecessary complexity.

---

## CreatureDistanceMapper Requirements

Create a pure logic class or static helper that can be tested without a Unity scene.

It should support:

```txt
mapping distance to CreaturePhase
using default thresholds
using CreatureData thresholds if provided
handling negative distance safely
handling over-100 distance safely
```

Suggested methods:

```txt
GetPhase(float distance)
GetPhase(float distance, CreatureData creatureData)
```

Exact method names may vary, but the API must be simple and testable.

Do not put phase calculation only inside a MonoBehaviour.

---

## CreatureController Requirements

`CreatureController` should be a minimal MonoBehaviour.

It should:

```txt
hold current CreaturePhase
receive or apply a threat distance
map distance to phase through CreatureDistanceMapper
optionally move to an anchor matching the phase
optionally hide/show the creature based on phase
expose current phase for debugging/testing
handle missing anchors safely
```

Potential anchor references:

```txt
FarAnchor
VisibleAnchor
MidCorridorAnchor
NearDoorAnchor
AtDoorAnchor
AttackAnchor
```

The controller may snap to anchors.

Interpolation is optional and should not be overbuilt.

Do not create animation systems.

Do not create AI.

Do not create pathfinding.

Do not create jumpscare cinematic logic.

---

## Anchor Behavior

If anchors are implemented:

```txt
Far -> FarAnchor
Visible -> VisibleAnchor
MidCorridor -> MidCorridorAnchor
NearDoor -> NearDoorAnchor
AtDoor -> AtDoorAnchor
Attack -> AttackAnchor
```

If an anchor is missing:

```txt
Do not throw a blocking exception.
Keep current transform position.
Still update CurrentPhase.
```

This is important because the scene is not assembled yet.

---

## Integration With Existing ThreatManager

Do not rewrite `ThreatManager`.

Do not duplicate the threat rules.

`ThreatManager` remains responsible for:

```txt
distance
stress
answer outcome effects
death detection
```

Creature Phase 3 is responsible for:

```txt
reading or receiving distance
mapping distance to visual/phase state
```

If integration helpers are added, keep them small.

Do not couple creature logic to the question system yet.

---

## Required EditMode Tests — CreatureDistanceMapper

Create EditMode tests for `CreatureDistanceMapper`.

Required test cases:

```txt
Distance100_ReturnsFarPhase
Distance81_ReturnsFarPhase
Distance80_ReturnsVisiblePhase
Distance61_ReturnsVisiblePhase
Distance60_ReturnsMidCorridorPhase
Distance41_ReturnsMidCorridorPhase
Distance40_ReturnsNearDoorPhase
Distance26_ReturnsNearDoorPhase
Distance25_ReturnsAtDoorPhase
Distance1_ReturnsAtDoorPhase
Distance0_ReturnsAttackPhase
NegativeDistance_ReturnsAttackPhase
DistanceAboveMaximum_ReturnsFarPhase
CustomCreatureDataThresholds_AreUsed
NullCreatureData_UsesDefaultThresholds
```

If exact test names vary, coverage must remain equivalent.

---

## Optional EditMode Tests — CreatureController

If simple to test, add tests for `CreatureController`.

Possible tests:

```txt
ApplyDistance_UpdatesCurrentPhase
ApplyDistance_WithMissingAnchors_DoesNotThrow
ApplyDistance_WithAnchor_MovesToAnchor
```

Do not overcomplicate this.

If MonoBehaviour tests become awkward in EditMode, keep controller manual and focus tests on `CreatureDistanceMapper`.

---

## Manual Checks

After implementation, check if possible:

```txt
Unity project compiles
EditMode tests run
Game.unity still exists
No scene overbuild happened
No generated Unity folders are staged
```

Do not build iOS in this phase.

Do not generate a Xcode project.

Do not assemble the full corridor/elevator scene yet.

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
git add UnityProject/Assets/Scripts/Creature UnityProject/Assets/Tests/EditMode
```

Recommended commit message:

```bash
git commit -m "👻 feat(creature): add distance-based hallway threat"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 3 Creature Distance

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

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Scene assembly added: yes/no
- Audio added: yes/no
- iOS build generated: yes/no
- VR/XR added: yes/no
- Android-specific work added: yes/no
- Monetization added: yes/no
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

## Manual checks

List what was checked manually.

If no manual checks were done, write:

No manual checks were performed.

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
git status --short | grep -E "UnityProject/(Library|Temp|Logs|UserSettings|Build|Builds)|\.slnx|\.csproj"
```

If the output is empty, write:

```txt
<clean>
```

## Known limits

List anything incomplete, unverified, or risky.

## Next recommended action

Choose exactly one:

- READY_FOR_REVIEW
- NEEDS_FIX
- NEEDS_USER_ACTION
- SHOULD_REVERT

Then explain in one sentence.

````

Do not summarize freely outside this structure.

---

## Acceptance Criteria

Phase 3 is complete only if:

```txt
CreaturePhase exists
CreatureData exists as a ScriptableObject
CreatureDistanceMapper or equivalent pure logic exists
CreatureController exists
distance-to-phase mapping works
boundary behavior is tested
custom CreatureData thresholds can be used or safely ignored with documented reason
missing anchors are handled safely if anchors exist
EditMode tests exist
EditMode tests pass if Unity Test Runner is available
No final UI added
No final art added
No scene overbuild
No animation system added
No AI/pathfinding added
No forbidden generated folders staged
Agent final report is complete and written in French
````
