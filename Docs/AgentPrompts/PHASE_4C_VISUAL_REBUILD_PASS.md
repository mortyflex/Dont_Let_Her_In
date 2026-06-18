# Agent Prompt — Phase 4C Visual Rebuild Pass

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
This phase corrects a failed visual readability pass in Unity. It modifies Game.unity, camera framing, placeholder geometry, materials and lighting. It must stay controlled, conservative and scene-focused.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎨 fix(scene): rebuild readable horror placeholder
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
Docs/AgentPrompts/PHASE_4C_VISUAL_REBUILD_PASS.md
Docs/ROADMAP.md
Docs/GAME_DESIGN.md
Docs/ART_DIRECTION.md
Docs/TECH_ARCHITECTURE.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
UnityProject/Assets/References/Visuals/README.md
Skills/horror-game-design/SKILL.md
Skills/unity-scene-assembly/SKILL.md
Skills/unity-mobile-performance/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Also inspect:

```txt
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Art/Materials/
UnityProject/Assets/References/Visuals/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Current Project State

Previous phases:

```txt
6dd6b73 — 🎮 feat(gameplay): add core threat run loop
109dcfd — 🎮 feat(questions): add data-driven question system
882eb40 — 👻 feat(creature): add distance-based hallway threat
99da6c1 — 🛗 feat(scene): add elevator corridor prototype
1d3efce — 🎨 art(scene): improve placeholder horror readability
```

Current test status before this phase:

```txt
76 EditMode tests passed
```

The main scene is:

```txt
UnityProject/Assets/Scenes/Game.unity
```

Visual references are in:

```txt
UnityProject/Assets/References/Visuals/
```

---

## Why This Phase Exists

The Phase 4B visual review failed.

The user opened the Game View in iPhone portrait and the result looked wrong.

Observed problems:

```txt
the scene looks abstract and blocky
the elevator does not read as an elevator
the corridor does not read as a hotel corridor
a giant pale green block dominates the top of the frame
a giant pale green block dominates the bottom of the frame
the walls are too plain and empty
there are no readable hotel corridor details
the creature looks like a black oval, not a threatening female-like silhouette
the composition is not close enough to the visual references
the lighting feels accidental, not cinematic
the scene looks like a broken prototype, not a horror greybox
```

This phase must correct that.

---

## Visual References

Use these files as visual references only:

```txt
UnityProject/Assets/References/Visuals/01_elevator_start_view.png
UnityProject/Assets/References/Visuals/02_hotel_corridor_level_1.png
UnityProject/Assets/References/Visuals/03_unstable_lights_level_2.png
UnityProject/Assets/References/Visuals/04_mystery_message_level_3.png
UnityProject/Assets/References/Visuals/05_audio_trial_level_4.png
UnityProject/Assets/References/Visuals/06_anomaly_level_5.png
UnityProject/Assets/References/Visuals/07_final_face_her.png
UnityProject/Assets/References/Visuals/08_creature_distance_states.png
UnityProject/Assets/References/Visuals/09_materials_palette.png
```

Important:

```txt
Do not use these images as textures.
Do not assign them to materials.
Do not import them as in-game art.
Use them only for composition, lighting, color and atmosphere.
```

Main target references for this pass:

```txt
01_elevator_start_view.png
02_hotel_corridor_level_1.png
08_creature_distance_states.png
09_materials_palette.png
```

---

## Mission

Implement:

```txt
Phase 4C — Visual Rebuild Pass
```

The goal is to rebuild the current placeholder scene into a readable horror greybox.

The result should still be placeholder-only, but it must clearly communicate:

```txt
I am inside an elevator.
The elevator opens into a long hotel corridor.
There is a female-like threat in the corridor.
The corridor has depth, doors, walls, ceiling and floor.
The lighting is sick green/yellow with red danger accents.
The scene is dark but readable.
```

This is not a final art pass.

This is a composition and readability rebuild.

---

## Strict Visual Requirements

The Game View in portrait must show:

```txt
clear elevator doorway frame
visible side elevator panels or walls
visible elevator floor threshold
long corridor in front of the elevator
corridor floor, ceiling and side walls distinguishable
simple hotel doors or door panels on both sides
simple wall trim / lower wall panels
simple ceiling lamps or light panels along the corridor
red warning glow deeper in the corridor
sick green-yellow light near the elevator and mid-corridor
female-like silhouette at corridor depth
foreground / midground / background separation
```

The Game View must not show:

```txt
giant flat pale green slabs dominating the top/bottom
abstract tunnel only
empty plain walls
pure black unreadable corridor
creature as only a black oval
overbright cartoon colors
final art
imported textures
asset pack models
```

---

## Required Scene Fixes

Modify only:

```txt
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Art/Materials/
```

You may create a few new simple placeholder materials if needed.

Do not create another scene.

Do not rename `Game.unity`.

Do not delete the main scene.

---

## Elevator Rebuild Requirements

The elevator view should read closer to:

```txt
01_elevator_start_view.png
```

Required improvements:

```txt
remove or resize any giant pale green top/bottom blocks that dominate the camera
make the elevator doorway frame readable but not oversized
add simple vertical side panels around the opening
add a simple threshold at the floor
add a small elevator display panel or button panel if useful
use dark metal / dirty green-grey tones
keep the camera inside the elevator
```

The elevator must frame the corridor without hiding it.

---

## Corridor Rebuild Requirements

The corridor should read closer to:

```txt
02_hotel_corridor_level_1.png
```

Required improvements:

```txt
add simple door rectangles on both sides of the corridor
add simple wall panels or trim along both sides
add simple ceiling lamp objects along the corridor
make floor/walls/ceiling visually distinct
make corridor depth obvious
add a far back wall or darkness plane with red glow
use dirty hotel / old hospital mood
```

Allowed geometry:

```txt
cubes
planes
quads
capsules
simple primitives
```

Do not import models.

Do not use textures.

Use simple material colors and lighting only.

---

## Creature Rebuild Requirements

The current creature reads as a black oval.

Improve it into a simple female-like silhouette using primitives only.

Acceptable placeholder structure:

```txt
Creature
  PlaceholderCreature
    Body
    Head
    HairShape
```

or equivalent.

Suggested shape:

```txt
thin vertical body
small head
dark hair-like block or elongated shape
slightly pale/dark dress-like body
still mostly silhouette
```

The creature must remain:

```txt
single
still
unanimated
distance-driven
placeholder
dark
not final
```

Do not add:

```txt
detailed face
gore
jumpscare animation
AI
pathfinding
multiple creatures
```

If possible, keep or rewire `CreatureController` safely.

The silhouette should be visible against the red backlight.

---

## Camera Requirements

The camera must remain:

```txt
fixed
inside the elevator
looking toward the corridor
portrait-friendly
no free look
no joystick
no Cinemachine
```

Adjust if needed:

```txt
position
rotation
FOV
clipping
```

The Game View should not be dominated by the ceiling or floor.

The corridor must occupy the central readable area.

The elevator frame should be visible but not overwhelming.

---

## Lighting Requirements

The lighting should move toward:

```txt
sick green/yellow near and mid corridor
red warning glow in the background
dark but readable
```

Allowed:

```txt
adjust existing lights
move existing lights
add a small number of simple point lights if necessary
adjust ambient and fog
use subtle emission on simple lamp meshes
```

Avoid:

```txt
too many real-time lights
overbright scene
huge glowing slabs
heavy post-processing
volumetric effects
complex shadows
```

Target:

```txt
mobile-friendly placeholder lighting
```

---

## Materials Requirements

Use simple URP Lit placeholder materials.

You may modify:

```txt
M_Elevator_Dark
M_Corridor_Wall
M_Corridor_Floor
M_Creature_Silhouette
M_Warning_Red
M_Darkness
M_Elevator_Edge
M_Sickly_Light_Green
```

You may create simple additional materials if needed:

```txt
M_Door_Dark
M_Wall_Trim
M_Ceiling_Lamp
M_Dim_Wood
M_Creature_Dress
```

Do not use image references as texture maps.

All reference images must remain references only.

---

## Optional Screenshot Export

If feasible, generate a temporary screenshot from the Main Camera after the scene is rebuilt.

The screenshot should not be committed.

Possible output:

```txt
/tmp/dlhi_phase4c_gameview.png
```

If not feasible, skip and explain.

Do not block the task on screenshot export.

---

## Tests Required

Run all EditMode tests.

Expected baseline:

```txt
76 EditMode tests
```

If more tests exist, run all EditMode tests.

Do not claim tests passed unless they actually ran.

---

## Required Manual / Structural Checks

After implementation, verify:

```txt
Game.unity imports
SceneRoot still exists
GameSystems still exists
Elevator still exists
Corridor still exists
Creature still exists
Lighting still exists
UI still exists
Audio still exists
Main Camera exists
CreatureAnchors still exist
CreatureController still exists or explain if changed
reference images are not used as textures/materials
no generated Unity folders staged
```

Because the agent may not have GUI access, structural checks are acceptable.

The user will do the final visual review.

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
git add UnityProject/Assets/Scenes/Game.unity UnityProject/Assets/Art/Materials
```

If new material `.meta` files are created, include them.

Recommended commit message:

```bash
git commit -m "🎨 fix(scene): rebuild readable horror placeholder"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 4C Visual Rebuild Pass

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

## Visual rebuild details

Explain concretely what was changed:

- elevator:
- corridor:
- creature:
- camera:
- lighting:
- materials:

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Reference images used as textures/materials: yes/no
- Scene assembly added beyond placeholder: yes/no
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

## Structural checks

List structural/import checks performed.

## Screenshot export

If generated:

- Path:
- Committed: no

If not generated:

- Not generated. Reason:

## Visual check instructions for user

Give precise instructions for the user to open Unity and inspect the result.

Include:

- which scene to open
- what Game view aspect/resolution to use
- whether to enter Play Mode or not
- what the user should see
- what the user should not expect yet
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

Phase 4C is complete only if:

```txt
Game.unity remains the main scene
giant pale green slabs no longer dominate the Game View
elevator doorway frame is readable
corridor reads as a hotel/hospital corridor placeholder
simple side doors or panels exist
floor/walls/ceiling are distinguishable
creature reads more like a female-like silhouette than a black oval
red backlight helps separate the creature
green/yellow sick lighting supports the mood
scene remains dark but readable
scene remains mobile portrait-friendly
reference images are not used as textures/materials
no final art imported
no final UI added
no audio added
no gameplay flow added
no AI/pathfinding added
existing EditMode tests still pass if Unity Test Runner is available
no forbidden generated folders staged
agent final report is complete and written in French
user can visually inspect the scene after this phase
````
