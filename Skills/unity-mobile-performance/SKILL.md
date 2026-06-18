# Skill — Unity Mobile Performance

## Name

unity-mobile-performance

## Purpose

Use this skill when working on mobile performance, URP settings, build settings, rendering cost, lighting, shadows, UI overdraw, texture size, memory, profiling or Android/iOS readiness.

The game is mobile-first. Performance and readability are more important than cinematic excess.

## Project Context

**Don’t Let Her In** is a mobile horror game prototype built in Unity 6 with URP.

The prototype uses:

- fixed camera
- one elevator
- one corridor
- one creature
- mobile portrait UI
- short sessions
- limited scene complexity

The first performance target is:

```txt
Stable 30 FPS minimum on mobile-class hardware
No blocking console errors
Fast restart
Readable portrait UI
```

## Performance Philosophy

Do not optimize prematurely, but avoid obvious bad choices.

The prototype should be:

- lightweight
- readable
- easy to profile
- easy to build
- simple enough for mobile
- compatible with later polish

## Rendering Rules

Use URP.

Prefer:

- simple materials
- limited real-time lights
- simple shadows or no shadows in early prototype
- baked/static lighting where useful
- low-complexity geometry
- optimized UI
- small scenes

Avoid:

- expensive post-processing
- many real-time lights
- many shadow-casting objects
- excessive transparent materials
- high overdraw
- large textures
- complex shaders
- dense particle systems
- unnecessary physics
- large imported asset packs

## Camera Rules

The fixed camera is a major performance advantage.

Keep:

- fixed first-person elevator view
- no free movement
- no open world
- limited visible scene area
- corridor framed intentionally

This allows:

- simpler level
- fewer visible objects
- easier lighting
- predictable composition
- better mobile performance

## Lighting Rules

Horror does not require many lights.

Use darkness intelligently.

Recommended prototype setup:

```txt
1 elevator light
1 corridor light
1 flicker light
optional ambient low light
```

Avoid:

- many dynamic point lights
- expensive shadows everywhere
- dynamic global illumination
- high intensity bloom as a crutch
- excessive volumetric effects

If using flicker:

- flicker intensity only
- keep it simple
- avoid spawning/destroying lights repeatedly

## Shadows

For early prototype:

- shadows are optional
- use low resolution shadows if needed
- avoid shadows on every object
- creature shadow can be faked later
- silhouette readability matters more than realistic shadowing

## Materials and Textures

Prototype rules:

- use simple materials
- avoid large textures
- avoid 4K textures
- avoid importing asset packs blindly
- keep placeholder materials clear and named

Suggested placeholder materials:

```txt
M_DarkWall
M_DirtyFloor
M_ElevatorMetal
M_CreatureSilhouette
M_RedWarning
M_GreenSickLight
```

## UI Performance

Mobile UI should be simple.

Avoid:

- excessive transparency
- many animated UI elements
- heavy blur
- full-screen effects every frame
- huge canvases with constant rebuilds

Prefer:

- simple panels
- clear text
- few animated elements
- limited glitch effects
- targeted feedback animations

## Audio Performance

Audio is important, but keep it controlled.

Rules:

- use compressed audio where appropriate
- keep looping ambience simple
- do not trigger many overlapping sounds
- use a small number of AudioSources
- avoid loading many clips at runtime in prototype

## Asset Rules

Do not import heavy asset packs without explicit approval.

Before importing assets, check:

- file size
- texture sizes
- polygon count
- license
- whether it is needed now
- whether placeholder is enough

For v0.1, placeholders are preferred.

## Build Rules

Initial target:

```txt
iOS first
Portrait orientation
URP
No VR
No ads
No store SDK
No analytics SDK until needed
```

Do not add monetization SDKs in prototype v0.1.

## Profiling Rules

If performance issues appear:

1. Check console errors.
2. Check object count.
3. Check lights.
4. Check shadows.
5. Check UI overdraw.
6. Check texture sizes.
7. Check unnecessary Update methods.
8. Check allocations.
9. Use Unity Profiler if available.

Do not guess endlessly. Measure when possible.

## Code Performance Rules

Avoid:

- heavy work in `Update`
- frequent allocations in hot paths
- repeated `FindObjectOfType`
- repeated string operations during gameplay
- unnecessary LINQ in hot paths
- spawning/destroying objects repeatedly during core loop

Prefer:

- cached references
- simple state updates
- object reuse
- event-driven changes
- serialized references

## Mobile UX and Performance

Performance includes usability.

Ensure:

- readable text
- large buttons
- no precision clicking
- no keyboard requirement
- no tiny timer
- no UI blocking the central creature view
- fast restart after death

## Acceptance Criteria

A mobile performance task is acceptable if:

- no obvious heavy rendering choice was added
- scene remains mobile-friendly
- UI remains readable
- no large assets were added without approval
- no unnecessary SDK was added
- console has no blocking errors
- any unmeasured performance claims are clearly labeled as unmeasured

## Delivery Requirements

At the end of a performance-related task, report:

- what changed
- why it helps mobile
- what was measured, if anything
- what was not measured
- expected risk
- console status
- build status if tested
- git status
- targeted commit command

## Commit Examples

```bash
git add UnityProject/ProjectSettings/ProjectSettings.asset UnityProject/Assets/Settings/URP.asset
git commit -m "📱 chore(mobile): configure URP prototype settings"
```

```bash
git add UnityProject/Assets/Scenes/Game.unity
git commit -m "📱 perf(scene): simplify prototype lighting for mobile"
```
