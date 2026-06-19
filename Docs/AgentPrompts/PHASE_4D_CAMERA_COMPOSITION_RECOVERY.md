# Agent Prompt — Phase 4D Camera Composition Recovery

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
This phase corrects a repeated visual failure in Unity. It must modify the camera composition, visible geometry, scene layout and placeholder readability in Game.unity. The work is scene-sensitive and should stay with Claude for continuity.
```

Risk level:

```txt
High
```

Expected commit:

```txt
🎨 fix(scene): recover camera composition readability
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
Docs/AgentPrompts/PHASE_4D_CAMERA_COMPOSITION_RECOVERY.md
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
f07774d — 🎨 fix(scene): rebuild readable horror placeholder
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

The Phase 4B and Phase 4C visual passes failed in the actual Unity Game View.

The user opened `Game.unity` in iPhone portrait Game View after reloading Unity, and the render still looked wrong.

The current visible result shows:

```txt
a giant pale green slab at the top of the frame
a giant pale green slab at the bottom of the frame
a dark abstract tunnel
walls with almost no readable hotel detail
door/panel details not visible in the camera view
creature still reads mostly like a dark oval
the elevator is not clearly readable as an elevator interior
the composition is far from the visual references
the Game View does not communicate a horror hotel/elevator scene
```

This phase must prioritize what the camera actually sees.

Do not merely add objects to the hierarchy.

Objects must be visible in the Main Camera Game View.

---

## Visual References

Use these files as references only:

```txt
UnityProject/Assets/References/Visuals/01_elevator_start_view.png
UnityProject/Assets/References/Visuals/02_hotel_corridor_level_1.png
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

The closest target is:

```txt
01_elevator_start_view.png
```

The Game View should read like:

```txt
inside elevator foreground
open elevator doorway
long hotel corridor ahead
female-like silhouette in the corridor
red danger glow deeper in the hallway
sick green/yellow dirty lighting
```

---

## Mission

Implement:

```txt
Phase 4D — Camera Composition Recovery
```

The goal is to recover a readable Game View composition.

This phase must make the **actual Main Camera render** understandable in portrait.

The main outcome is not “more objects exist”.

The main outcome is:

```txt
When the user opens Game View in iPhone portrait, they can immediately understand the scene.
```

---

## Hard Rule: Camera Visibility First

Before committing, verify that the important scene elements are inside the Main Camera view.

Important elements that must be visible:

```txt
elevator doorway / frame
corridor floor
corridor side walls
corridor ceiling or ceiling lamps
at least two visible side door/panel shapes
creature silhouette
red background glow
green/yellow light source or glow
```

If an object exists in the hierarchy but is not visible in the Game View, it does not count.

---

## Required Visual Fixes

Fix the current composition problems:

```txt
remove, resize or move the giant pale green slabs dominating the top and bottom of the frame
reduce any oversized ceiling/floor/elevator primitives that dominate the camera
reposition the camera so it looks through the elevator opening into the corridor
ensure the corridor occupies the central readable area of the portrait frame
make side doors/panels visible from the camera
make the creature more visible and more humanoid/female-like than an oval
make the floor/walls/ceiling distinguishable
make the elevator frame visible but not overwhelming
```

---

## Suggested Camera Strategy

Use a simple composition close to:

```txt
camera inside elevator, slightly back from the door
looking straight down corridor
moderate FOV
elevator frame visible on top/sides/bottom but not dominating
corridor centered
creature centered or slightly lower center
```

Try values around:

```txt
camera local position: x 0, y 1.45 to 1.65, z -2.5 to -3.5
camera rotation: x 0 to 5 degrees, y 0, z 0
FOV: 50 to 60
```

These are suggestions, not strict values.

The key requirement is what is visible in Game View.

Avoid:

```txt
camera too close to ceiling
camera too close to floor
camera pointed too high
camera pointed too low
FOV so wide that geometry distorts
FOV so narrow that corridor details disappear
```

---

## Elevator Requirements

The elevator should read as a foreground frame, not as giant slabs.

Required:

```txt
visible doorway frame
visible side metal panels or side door edges
visible threshold or floor edge
optional small button/display panel on one side
dark metal/dirty green-gray tones
```

Avoid:

```txt
huge glowing ceiling block
huge glowing floor block
elevator geometry covering most of the image
overbright emission
```

If needed, replace oversized primitives with smaller, thinner frame pieces.

---

## Corridor Requirements

The corridor must read as an old hotel/hospital corridor.

Required:

```txt
long central corridor
left and right walls visible
floor visible
ceiling visible enough for depth
simple door rectangles/panels visible on both sides
wall trim / lower panel line visible
ceiling lamps or small light panels visible down the corridor
background darkness or back wall with red glow
```

Avoid:

```txt
empty black tunnel
plain walls with no scale cues
details placed outside camera frustum
door panels too far or too dark to see
```

Use simple primitives only.

No asset packs.

No textures.

---

## Creature Requirements

The creature must read more like a female-like silhouette and less like a black oval.

Required:

```txt
single placeholder creature
visible from the camera
humanoid/female-like silhouette using simple primitives
head visible
body/dress shape visible
hair-like dark shape visible
standing in or near the red backlight
```

Acceptable structure:

```txt
Creature
  PlaceholderCreature
    Body
    Head
    HairShape
```

The creature must remain:

```txt
single
still
unanimated
placeholder
distance-driven
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

---

## Lighting Requirements

Lighting must be dark but readable.

Target:

```txt
dark horror mood
dirty green/yellow light
red background danger glow
visible silhouette separation
```

Avoid:

```txt
overbright green slabs
pure black unreadable scene
too many real-time lights
cartoon colors
heavy post-processing
```

Allowed:

```txt
adjust light positions/intensities/ranges
adjust fog/ambient
reduce or disable problematic emission/bloom sources
move red light behind creature
add small non-overpowering lamp meshes
```

If the current post-processing/bloom causes giant blocks, reduce emissions or disable bloom contribution from those primitives.

---

## Materials Requirements

Use simple URP Lit placeholder materials.

You may modify existing materials:

```txt
M_Elevator_Dark
M_Corridor_Wall
M_Corridor_Floor
M_Creature_Silhouette
M_Warning_Red
M_Darkness
M_Elevator_Edge
M_Sickly_Light_Green
M_Door_Dark
M_Wall_Trim
M_Creature_Dress
```

You may create one or two new simple materials if necessary.

Do not use visual reference images as textures.

Do not create final materials.

---

## Optional Helper Script For Camera-Frustum Sanity Check

If useful, create a temporary or editor-only helper to check whether important objects are inside the Main Camera frustum.

If created, keep it editor-only or remove it before commit.

Do not commit throwaway scripts unless they are clean and useful.

The final report must state how visibility was checked.

---

## Optional Screenshot Export

If feasible, generate a temporary screenshot from the Main Camera after the scene is rebuilt.

The screenshot should not be committed.

Possible output:

```txt
/tmp/dlhi_phase4d_gameview.png
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

## Required Camera Visibility Checks

The final report must explicitly say whether these are visible in Main Camera view:

```txt
elevator doorway/frame
corridor floor
corridor walls
corridor ceiling or lamps
side doors/panels
creature silhouette
red background glow
green/yellow light source or glow
```

If the agent cannot actually render or inspect the Game View, it must say so clearly and explain the structural/frustum checks used instead.

Do not claim visual success without either a real render/screenshot or a clear frustum/structural verification.

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
git commit -m "🎨 fix(scene): recover camera composition readability"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 4D Camera Composition Recovery

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

## Composition recovery details

Explain concretely what changed:

- camera:
- elevator:
- corridor:
- creature:
- lighting:
- materials:

## Main Camera visibility check

Confirm each item:

- Elevator doorway/frame visible: yes/no
- Corridor floor visible: yes/no
- Corridor walls visible: yes/no
- Corridor ceiling or lamps visible: yes/no
- Side doors/panels visible: yes/no
- Creature silhouette visible: yes/no
- Red background glow visible: yes/no
- Green/yellow light source or glow visible: yes/no

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

Phase 4D is complete only if:

```txt
Game.unity remains the main scene
giant pale green slabs no longer dominate the Game View
camera composition is corrected
elevator doorway/frame is readable in Main Camera view
corridor reads as a hotel/hospital corridor placeholder in Main Camera view
simple side doors or panels are visible in Main Camera view
floor/walls/ceiling are distinguishable in Main Camera view
creature reads more like a female-like silhouette than a black oval
red backlight helps separate the creature
green/yellow sick lighting supports the mood without overpowering the frame
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
