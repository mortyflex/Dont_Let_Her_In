# Agent Prompt — Phase 4B Visual Readability Pass

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
This phase edits the existing Unity scene, visual readability, camera framing, lighting, materials and placeholder composition. It is still scene-sensitive work, so keep Claude for continuity and caution.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
🎨 art(scene): improve placeholder horror readability
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
Docs/AgentPrompts/PHASE_4B_VISUAL_READABILITY_PASS.md
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

Also inspect the current scene and references:

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

Phase 4 has been completed and committed:

```txt
99da6c1 — 🛗 feat(scene): add elevator corridor prototype
```

Visual references have been added under:

```txt
UnityProject/Assets/References/Visuals/
```

Current test status before this phase:

```txt
76 EditMode tests passed
```

The current scene exists here:

```txt
UnityProject/Assets/Scenes/Game.unity
```

---

## Current Visual Problem

The current Phase 4 scene is structurally correct but visually too dark.

User visual review showed:

```txt
corridor vertical depth is visible
red light exists
silhouette exists
portrait framing works
```

But also:

```txt
scene is almost unreadable
elevator frame is not clear enough
corridor walls/floor/ceiling are too dark
creature silhouette blends too much into the background
foreground/midground/background separation is weak
red light is interesting but too isolated
the scene does not yet match the visual references closely enough
```

The goal is not to make the scene final.

The goal is to make the placeholder scene readable and closer to the intended horror mood.

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

Important rules:

```txt
Do not use these images as in-game textures.
Do not assign them to materials.
Do not import them as final assets.
Do not trace them into final art.
Do not create heavy assets from them.
Use them only to guide composition, lighting, colors, silhouette readability and atmosphere.
```

The target direction is:

```txt
dirty hotel / hospital corridor
dark elevator frame
long corridor depth
sick green-yellow light
red warning accent
visible female-like silhouette
wet/dark floor impression using simple placeholder material only
clear foreground / midground / background separation
dark mood without becoming unreadable
```

---

## Mission

Implement:

```txt
Phase 4B — Visual Readability Pass
```

The goal is to improve the existing placeholder scene readability.

This phase should make the scene easier to understand visually when opened in the Game view in portrait.

It should stay placeholder-only.

It must not become a final art pass.

---

## Phase 4B Scope

Included:

```txt
adjust existing Game.unity lighting
adjust existing placeholder materials
adjust camera framing if needed
improve elevator frame readability
improve corridor wall/floor/ceiling readability
improve creature silhouette readability
improve depth cues in corridor
add simple primitive details if needed
add simple door panels / wall panels / frames if useful
add simple floor strip / threshold / trim if useful
add subtle red warning accent if useful
add subtle green-yellow light near elevator if useful
keep mobile portrait composition
```

Excluded:

```txt
final art
final textures
asset packs
downloaded models
final creature model
animations
jumpscare cinematic
audio
final UI
question UI
timer UI
answer buttons
full gameplay flow
new gameplay systems
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

Modify only the main scene:

```txt
UnityProject/Assets/Scenes/Game.unity
```

Do not create another gameplay scene.

Do not rename the scene.

Do not delete the scene.

Do not modify unrelated scenes.

---

## Visual Goals

After this pass, the Game view in portrait should clearly show:

```txt
the player is inside an elevator
the elevator doorway frames the corridor
the corridor extends forward into darkness
the floor/walls/ceiling are distinguishable
there is a readable red warning accent deeper in the corridor
there is a readable sick green-yellow light near the elevator or ceiling
the creature silhouette is visible enough to feel threatening
foreground/midground/background are visually separated
```

The scene should still feel:

```txt
dark
dirty
claustrophobic
oppressive
unsafe
horror-oriented
```

But it should not be:

```txt
almost completely black
flat
visually confusing
too bright
cartoonish
gory
polished final art
```

---

## Specific Improvements To Consider

Consider these changes if useful:

```txt
increase ambient visibility slightly without killing darkness
increase elevator light intensity slightly
change corridor wall/floor materials from nearly black to dark green-brown / dirty concrete tones
make corridor floor subtly more readable
make elevator door frame brighter on edges
add simple wall panels or trim to help read scale
add simple door rectangles along corridor sides
add simple ceiling lights as primitives or existing light objects
add a faint back/rim light behind or above the creature
move or scale creature silhouette so it is readable in portrait
slightly reduce fog density if it hides everything
adjust camera FOV/position so elevator frame and corridor are both visible
ensure the red light remains a warning accent, not the only visible element
```

Do not overdo it.

This is a readability pass, not an art production pass.

---

## Creature Readability Rules

The creature should remain:

```txt
one placeholder silhouette
female-like only by general shape
simple
dark
still
unanimated
distance-driven
```

Acceptable:

```txt
capsule silhouette adjusted
simple primitive body/head/hair-like block shape
slightly brighter rim or backlight
position adjusted to be visible
```

Not acceptable:

```txt
final creature model
detailed face
gore
animation system
jumpscare sequence
multiple creatures
AI/pathfinding
```

---

## Materials Rules

You may modify existing placeholder materials:

```txt
UnityProject/Assets/Art/Materials/M_Elevator_Dark.mat
UnityProject/Assets/Art/Materials/M_Corridor_Wall.mat
UnityProject/Assets/Art/Materials/M_Corridor_Floor.mat
UnityProject/Assets/Art/Materials/M_Creature_Silhouette.mat
UnityProject/Assets/Art/Materials/M_Warning_Red.mat
UnityProject/Assets/Art/Materials/M_Darkness.mat
```

You may create a small number of new simple materials if truly useful.

Allowed examples:

```txt
M_Sickly_Light_Green
M_Elevator_Edge
M_Door_Dark
M_Wall_Trim
```

Do not create final texture-based materials.

Do not assign reference images as textures.

Use simple URP Lit colors/emission only if needed.

---

## Lighting Rules

Keep lighting mobile-friendly.

Allowed:

```txt
adjust existing Directional Light intensity/color
adjust ElevatorLight intensity/color/range
adjust CorridorWarningLight intensity/color/range
add one or two simple additional lights if necessary for readability
adjust RenderSettings fog/ambient
```

Avoid:

```txt
many real-time lights
heavy post-processing
complex shadows
expensive effects
cinematic lighting system
volumetric effects
```

If shadows are used, keep them minimal.

---

## Camera Rules

The camera should remain:

```txt
fixed
inside the elevator
looking toward the corridor
portrait-friendly
no free look
no joystick
no Cinemachine unless already present and necessary
```

It is acceptable to adjust:

```txt
position
rotation
field of view
near/far clipping if needed
clear color
```

The Game view should communicate the concept quickly.

---

## Required Tests

This phase is scene-heavy.

Required:

```txt
Run all EditMode tests
Confirm previous 76 tests still pass
```

Do not write new gameplay logic tests unless new logic is added.

Do not add PlayMode tests unless clearly useful and stable.

Manual visual inspection is required from the user after this phase.

---

## Required Manual Checks By Agent

After implementation, check if possible:

```txt
Game.unity still opens/imports
SceneRoot hierarchy still exists
Elevator still exists
Corridor still exists
Creature still exists
Lighting still exists
UI still exists
Audio still exists
Main Camera exists
Creature anchors still exist
CreatureController references still exist
No final UI added
No final art imported
No reference images assigned as textures/materials
No generated Unity folders staged
```

If Unity Editor GUI cannot be opened, report honestly.

Batch import/test validation is acceptable, but the user will do the visual review.

---

## Visual Check Instructions For User

The final report must include a precise visual check section.

It must explain:

```txt
how to open Unity
which scene to open
how to use Game view in portrait
whether to use Play Mode
what the user should see
what the user should not expect yet
what to report back if the scene is still too dark or unreadable
```

The next user review should focus on:

```txt
elevator frame readability
corridor depth
creature visibility
lighting mood
overall readability in portrait
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
git add UnityProject/Assets/Scenes/Game.unity UnityProject/Assets/Art/Materials
```

If new material `.meta` files are created, include them.

If no new files are created, only add modified scene/material files.

Recommended commit message:

```bash
git commit -m "🎨 art(scene): improve placeholder horror readability"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 4B Visual Readability Pass

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

## Visual changes

List the important visual changes:

- lighting:
- camera:
- materials:
- creature readability:
- corridor readability:
- elevator readability:

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

## Manual checks

List what was checked manually or structurally.

If no manual checks were done, write:

No manual checks were performed.

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

Phase 4B is complete only if:

```txt
Game.unity remains the main scene
visual readability is improved
elevator frame is clearer
corridor is more readable
creature silhouette is more visible
lighting remains horror-oriented
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
