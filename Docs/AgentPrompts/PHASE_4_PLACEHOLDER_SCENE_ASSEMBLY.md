# Agent Prompt — Phase 4 Placeholder Scene Assembly

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
This phase edits the Unity scene and creates the first visible prototype layout. It touches GameObjects, camera framing, anchors, lighting, hierarchy and component references. Keep Claude for continuity and caution.
```

Risk level:

```txt
Medium to High
```

Expected commit:

```txt
🛗 feat(scene): add elevator corridor prototype
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
Docs/AgentPrompts/PHASE_4_PLACEHOLDER_SCENE_ASSEMBLY.md
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

Also inspect the previous implementations:

```txt
UnityProject/Assets/Scripts/Core/
UnityProject/Assets/Scripts/GameLoop/
UnityProject/Assets/Scripts/Threat/
UnityProject/Assets/Scripts/Questions/
UnityProject/Assets/Scripts/Creature/
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

Phase 2 has been completed and committed:

```txt
109dcfd — 🎮 feat(questions): add data-driven question system
```

Phase 3 has been completed and committed:

```txt
882eb40 — 👻 feat(creature): add distance-based hallway threat
```

Current test status:

```txt
76 EditMode tests passed
```

The project now has:

```txt
core gameplay loop logic
question system logic
threat distance logic
creature distance phase mapping
minimal CreatureController
```

The project does not yet have:

```txt
elevator scene
corridor scene
visible creature placeholder
camera framing
lighting setup
UI flow
playable scene
audio feedback
```

---

## Mission

Implement:

```txt
Phase 4 — Placeholder Scene Assembly
```

The goal is to create the first visible prototype scene.

This phase should create a simple, clean, mobile portrait-friendly scene layout.

The scene must communicate the basic horror composition:

```txt
Player camera inside elevator
Elevator framing in foreground
Dark corridor visible ahead
Creature position anchors down the corridor
Simple placeholder creature
Basic lighting
Clean hierarchy
```

This phase is not about final art.

This phase is not about gameplay UI.

This phase is not about final horror polish.

---

## Phase 4 Scope

Included:

```txt
clean Game.unity hierarchy
SceneRoot
GameSystems
Elevator placeholder
Corridor placeholder
Creature anchors
Placeholder creature object
CreatureController attached if useful
fixed camera inside elevator
basic portrait-friendly framing
basic lighting
simple materials
minimal scene helper script if necessary
optional editor utility only if useful and safe
scene saved
```

Excluded:

```txt
final art
final creature model
animations
jumpscare cinematic
audio
final UI
question UI
timer UI
answer buttons
full gameplay flow
new game systems beyond scene setup
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
pathfinding
enemy AI
multiple creatures
```

---

## Required Scene

The main scene is:

```txt
UnityProject/Assets/Scenes/Game.unity
```

Do not create another gameplay scene.

Do not rename the scene.

Do not delete the scene.

Do not modify unrelated scenes.

---

## Required Scene Hierarchy

Create or enforce this hierarchy in `Game.unity`:

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

For this phase:

```txt
GameSystems can be mostly empty or contain simple manager placeholders only if already safe.
UI can be an empty parent.
Audio can be an empty parent.
```

Do not build final UI or audio.

---

## Required Elevator Placeholder

Create a simple placeholder elevator interior.

The elevator should frame the camera and face the corridor.

Suggested hierarchy:

```txt
Elevator
  ElevatorInterior
  DoorFrame
  DoorLeft
  DoorRight
  BackWall
  SideWallLeft
  SideWallRight
  Ceiling
  Floor
  CameraAnchor
```

Use simple Unity primitives:

```txt
Cube
Plane
Quad
```

Use simple dark/metal placeholder materials.

Do not import asset packs.

Do not create final meshes.

Do not overbuild details.

---

## Required Corridor Placeholder

Create a simple dark corridor in front of the elevator.

Suggested hierarchy:

```txt
Corridor
  CorridorFloor
  CorridorCeiling
  CorridorWallLeft
  CorridorWallRight
  CorridorBackDarkness
  DoorPlaceholders
  ClueAnchors
  CreatureAnchors
```

Suggested `CreatureAnchors` children:

```txt
Far
Visible
MidCorridor
NearDoor
AtDoor
Attack
```

The anchors must be positioned along the corridor so that the creature can move closer to the elevator.

The anchor names should be exact and readable.

The corridor does not need final textures.

---

## Required Creature Placeholder

Create one simple placeholder creature.

Suggested object:

```txt
Creature
  PlaceholderCreature
```

The placeholder can be:

```txt
capsule
thin cube silhouette
simple dark humanoid shape
```

It should look like a temporary silhouette, not final art.

If attaching `CreatureController` is simple and safe, attach it to `PlaceholderCreature` or a parent and wire the anchors.

If wiring anchors is too fragile through automation, create the hierarchy and explain what remains manual.

The creature must remain single.

Do not create multiple enemies.

---

## Required Camera Setup

Create or position the main camera inside the elevator.

The camera should:

```txt
look toward the corridor
feel fixed
show elevator frame
show corridor depth
support portrait framing
not require player movement
```

Suggested:

```txt
Main Camera
```

or under:

```txt
Elevator/CameraAnchor/Main Camera
```

Keep it simple.

Do not add camera free-look.

Do not add joystick.

Do not add Cinemachine unless already present and clearly necessary.

Prefer no Cinemachine in this phase.

---

## Required Lighting Setup

Create basic lighting under:

```txt
Lighting
```

Suggested objects:

```txt
MainDirectionalLight or KeyLight
ElevatorLight
CorridorWarningLight
```

Use mobile-friendly simple lights.

Keep real-time lights limited.

Avoid heavy post-processing.

Avoid final lighting polish.

The scene should be visible but dark.

---

## Required Materials

Create simple placeholder materials if needed under:

```txt
UnityProject/Assets/Art/Materials/
```

Suggested materials:

```txt
M_Elevator_Dark
M_Corridor_Wall
M_Corridor_Floor
M_Creature_Silhouette
M_Warning_Red
M_Darkness
```

Use simple colors.

Do not import textures.

Do not create heavy shaders.

---

## Optional Scene Setup Script

If useful, you may create a small editor/runtime scene setup helper, but only if it reduces risk.

Possible folder:

```txt
UnityProject/Assets/Scripts/Tools/
```

Do not leave messy one-off code unless it is useful and documented.

If you create an editor-only script, place it safely in an Editor folder.

Avoid overengineering.

---

## Required Tests

This phase is scene-heavy, so EditMode tests may be limited.

Existing tests must still pass.

Required:

```txt
Run all EditMode tests
Confirm previous 76 tests still pass
```

Optional tests if practical:

```txt
CreatureController anchor behavior still passes
CreatureDistanceMapper still passes
```

Do not force PlayMode tests if scene automation is fragile.

Manual Unity checks are more important in this phase.

---

## Required Manual Checks

After implementation, check if possible:

```txt
Game.unity opens
SceneRoot exists
GameSystems exists
Elevator exists
Corridor exists
Creature exists
Lighting exists
UI exists
Audio exists
Main Camera exists
Camera is inside or aligned with elevator
Corridor is visible from camera
Creature anchors exist
Placeholder creature exists
No blocking console errors
No final UI added
No final art imported
No generated Unity folders staged
```

If Unity Editor cannot be opened manually, report honestly.

---

## Visual Acceptance Criteria

When the user opens Unity after this phase, they should be able to see:

```txt
a rough elevator interior
a corridor in front of the elevator
a fixed camera view facing the corridor
a simple creature placeholder or silhouette
a set of creature position anchors
basic dark lighting
```

The user should not expect yet:

```txt
final horror visuals
final creature model
animations
gameplay UI
answer buttons
timer
working quiz flow
audio
polished lighting
```

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
git add UnityProject/Assets/Scenes/Game.unity UnityProject/Assets/Scenes/Game.unity.meta UnityProject/Assets/Art/Materials UnityProject/Assets/Prefabs UnityProject/Assets/Scripts/Tools
```

If no prefabs or tools were created, omit those paths.

If scripts in `UnityProject/Assets/Scripts/Creature` were modified, include that folder.

Recommended commit message:

```bash
git commit -m "🛗 feat(scene): add elevator corridor prototype"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 4 Placeholder Scene Assembly

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

## Scene hierarchy created

List the resulting important scene hierarchy.

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

## Visual check instructions for user

Give precise instructions for the user to open Unity and inspect the result.

Include:

- which scene to open
- what view to select
- whether to enter Play Mode or not
- what the user should see
- what the user should not expect yet

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

- READY_FOR_VISUAL_REVIEW
- READY_FOR_REVIEW
- NEEDS_FIX
- NEEDS_USER_ACTION
- SHOULD_REVERT

Then explain in one sentence.

````

Do not summarize freely outside this structure.

---

## Acceptance Criteria

Phase 4 is complete only if:

```txt
Game.unity has a clean placeholder hierarchy
SceneRoot exists
GameSystems exists
Elevator exists
Corridor exists
Creature exists
Lighting exists
UI exists
Audio exists
Camera is positioned for fixed elevator view
Corridor is visible from camera
Creature anchors exist
Placeholder creature exists or absence is clearly justified
Basic lighting exists
No final UI added
No final art imported
No audio system added
No animation system added
No AI/pathfinding added
Existing EditMode tests still pass if Unity Test Runner is available
No forbidden generated folders staged
Agent final report is complete and written in French
User can visually inspect the scene after this phase
````
