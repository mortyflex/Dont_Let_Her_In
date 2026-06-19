# Agent Prompt — Phase 4E Portrait Framing Polish

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
This phase is a targeted Unity scene correction focused on portrait camera composition, elevator readability and visible placeholder geometry. It should remain with Claude for continuity and caution.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
🎨 fix(scene): polish portrait horror framing
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
Docs/AgentPrompts/PHASE_4E_PORTRAIT_FRAMING_POLISH.md
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

Previous relevant commits:

```txt
99da6c1 — 🛗 feat(scene): add elevator corridor prototype
1d3efce — 🎨 art(scene): improve placeholder horror readability
f07774d — 🎨 fix(scene): rebuild readable horror placeholder
b6011df — 🎨 fix(scene): recover camera composition readability
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

The Phase 4D scene is improved, but still not visually acceptable in portrait.

The user reviewed both portrait and landscape Game View screenshots.

What improved:

```txt
the corridor is readable
there are side panels / doors
the depth is clearer
the creature is visible
the red background glow exists
green/yellow horror mood exists
```

Main remaining problem:

```txt
the scene still does not read as being inside an elevator
```

Additional problems:

```txt
a large pale ceiling block still dominates the upper portrait frame
the elevator foreground frame is not clear enough
the Game View reads as a corridor camera, not an elevator view
the creature still looks too much like a simple capsule character
the corridor is readable but still too clean/simple
```

This phase should not rebuild everything.

This phase is a targeted polish pass before Phase 5.

---

## Mission

Implement:

```txt
Phase 4E — Portrait Framing Polish
```

The goal is to make the portrait Game View clearly communicate:

```txt
I am inside an elevator looking out into a corridor.
```

The corridor is already acceptable enough as a base.

Do not destroy the current corridor readability.

Improve the elevator framing and the creature silhouette.

---

## Priority Order

Work in this order:

```txt
1. Elevator readability in portrait
2. Remove / reduce the large top pale block
3. Camera composition
4. Creature silhouette shape
5. Small visible corridor details
```

Do not do a broad art pass.

Do not add gameplay.

Do not add UI.

---

## Visual References

Use these files as references only:

```txt
UnityProject/Assets/References/Visuals/01_elevator_start_view.png
UnityProject/Assets/References/Visuals/02_hotel_corridor_level_1.png
UnityProject/Assets/References/Visuals/08_creature_distance_states.png
UnityProject/Assets/References/Visuals/09_materials_palette.png
```

Main reference for this phase:

```txt
01_elevator_start_view.png
```

Important:

```txt
Do not use these images as textures.
Do not assign them to materials.
Do not import them as in-game art.
Use them only for composition, framing, colors and atmosphere.
```

---

## Hard Requirement: Elevator Must Read In Portrait

The portrait Game View must clearly show at least three elevator cues.

Acceptable elevator cues:

```txt
visible elevator side wall or metal side panel
visible door jambs / vertical frame pieces on left and right
visible top door header / lintel that does not dominate the frame
visible bottom threshold / metal sill
visible elevator button panel or floor display on one side
visible inner elevator wall / dark metal surface near camera
```

At least one cue should be on the left or right side of the portrait frame.

At least one cue should be near the bottom or top edge.

The current issue is that the user only sees a corridor and large blocks, not an elevator.

---

## Required Fixes

Fix these issues:

```txt
remove, resize, darken or reposition the large pale block dominating the top of the portrait frame
make the elevator doorway/frame readable without taking over the image
add or reposition side elevator panels / door jambs so they are visible in portrait
add a simple elevator button panel or floor indicator if useful
make the bottom threshold read as metal elevator threshold, not a flat slab
keep the corridor centered and readable
do not make the camera feel like it is floating in the corridor
```

The result should feel closer to:

```txt
camera inside elevator
elevator frame in foreground
corridor beyond the open doors
```

---

## Camera Requirements

The camera must remain:

```txt
fixed
inside the elevator
looking out into the corridor
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
near clipping
far clipping
```

The camera should not be so close to ceiling/floor that large slabs dominate the image.

The camera should not be so far forward that the elevator disappears.

---

## Elevator Geometry Requirements

The elevator foreground should use simple primitives.

Allowed additions:

```txt
thin dark vertical door jambs on left/right
thin top lintel / header
thin bottom metal threshold
side wall panels
button panel rectangle with small button circles
floor display panel with simple dark/red material blocks
```

Use simple shapes only:

```txt
cubes
planes
quads
small cylinders if already safe
```

No textures.

No final modeling.

No asset packs.

---

## Creature Silhouette Improvement

The current creature is more visible, but still too simple.

Improve it slightly without creating final art.

Target:

```txt
female-like silhouette
thin body
long dress-like shape
head visible
hair shape visible
less like a simple capsule
still placeholder
```

Allowed:

```txt
adjust existing Body / Head / HairShape scales
add a simple dress taper shape if possible with primitives
make the head/hair contrast more readable
adjust position slightly if needed
```

Do not add:

```txt
detailed face
gore
animation
jumpscare
AI
pathfinding
multiple creatures
```

---

## Corridor Detail Polish

The corridor is now readable. Keep it.

Only add very small details if they are visible in portrait and do not overcomplicate the scene.

Allowed:

```txt
simple door number plates
simple wall frame rectangles
small red wall mark / warning mark using primitives
slight material contrast on trim or doors
```

Do not add complex props.

Do not add textures.

Do not add final art.

---

## Lighting Rules

Keep the mood:

```txt
dark
dirty
green/yellow sick light
red background danger glow
```

But avoid:

```txt
large glowing green slabs
overbright lamp surfaces near camera
pure black unreadable areas
cartoon brightness
```

If needed:

```txt
reduce emission on large near-camera pieces
move near-camera light sources away from the camera
darken elevator ceiling/floor materials
keep corridor light readable
```

---

## Main Camera Visibility Check

Before committing, verify in the actual Main Camera view or via a temporary screenshot/render that these are visible:

```txt
elevator side/frame cues
elevator threshold or interior cue
corridor floor
corridor side walls
side doors/panels
creature silhouette
red background glow
green/yellow corridor light
```

If you use a temporary screenshot helper, delete it before commit.

Do not commit screenshots.

Do not commit temporary editor scripts.

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

## Required Structural Checks

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
/tmp/
test-results-editmode.xml
test-run.log
```

Use targeted adds only.

Recommended add command:

```bash
git add UnityProject/Assets/Scenes/Game.unity UnityProject/Assets/Art/Materials
```

If new material `.meta` files are created, include them.

Recommended commit message:

```bash
git commit -m "🎨 fix(scene): polish portrait horror framing"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 4E Portrait Framing Polish

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

## Portrait framing details

Explain concretely what changed:

- camera:
- elevator frame:
- elevator cues:
- corridor:
- creature:
- lighting:
- materials:

## Elevator readability check

Confirm each item:

- Side elevator cue visible: yes/no
- Door jambs or vertical frame visible: yes/no
- Top lintel/header visible but not dominant: yes/no
- Bottom threshold/sill visible: yes/no
- Button panel or floor display visible: yes/no
- Corridor still visible through elevator: yes/no

Explain how this was checked.

## Main Camera visibility check

Confirm each item:

- Elevator frame/interior cues visible: yes/no
- Corridor floor visible: yes/no
- Corridor walls visible: yes/no
- Side doors/panels visible: yes/no
- Creature silhouette visible: yes/no
- Red background glow visible: yes/no
- Green/yellow light visible: yes/no

Explain how this was checked.

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

Phase 4E is complete only if:

```txt
Game.unity remains the main scene
portrait Game View no longer feels like only a corridor
elevator side/frame cues are visible
elevator threshold or interior cue is visible
large top pale block is removed/reduced/darkened enough
corridor remains readable
side doors/panels remain visible
creature silhouette is slightly improved
red/green horror lighting remains readable
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
