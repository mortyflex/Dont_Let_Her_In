# Skill — Unity Scene Assembly

## Name

unity-scene-assembly

## Purpose

Use this skill when creating or modifying Unity scenes, prefabs, placeholder environments, camera setup, lighting setup, corridor layout, elevator layout, creature placement or scene hierarchy.

This skill is focused on making the prototype scene clean, readable and easy to iterate.

## Project Context

The prototype is a mobile-first horror game.

The player is inside an elevator, looking outward into a creepy corridor. The camera is fixed. The player does not walk. The creature approaches from the corridor.

The first prototype should not chase final graphics. It should prove the gameplay loop.

## Scene Philosophy

The scene must be:

- simple
- readable
- mobile-friendly
- easy to modify
- cleanly organized
- placeholder-friendly
- compatible with later art replacement

Do not create a complex level.

The prototype needs:

- one elevator
- one corridor
- one creature placeholder
- one fixed camera
- one UI canvas
- simple lighting
- basic audio sources
- clean anchors for creature positions

## Required Scene

Primary scene:

```txt
UnityProject/Assets/Scenes/Game.unity
```

Optional later scenes:

```txt
Boot.unity
MainMenu.unity
Results.unity
```

For the first playable prototype, one `Game.unity` scene is enough.

## Recommended Scene Hierarchy

Use this hierarchy:

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

## GameSystems

Recommended children:

```txt
GameSystems
  GameManager
  RunController
  QuestionManager
  ThreatManager
  FloorDirector
  AudioDirector
```

Rules:

- keep manager objects under `GameSystems`
- do not scatter managers around the scene
- avoid duplicate managers
- use clear GameObject names

## Elevator

Recommended children:

```txt
Elevator
  ElevatorInterior
  DoorLeft
  DoorRight
  ButtonPanel
  DigitalDisplay
  CameraAnchor
```

The elevator should frame the corridor.

Prototype requirements:

- camera is inside elevator
- corridor visible through doors
- elevator feels like a confined safe space
- doors can be static first, animated later
- button panel can be placeholder
- digital display can be placeholder

Do not implement complex elevator physics.

## Corridor

Recommended children:

```txt
Corridor
  Floor
  Walls
  Ceiling
  Doors
  Props
  ClueAnchors
  CreatureAnchors
```

The corridor should be long enough to show distance changes.

Prototype requirements:

- simple hallway
- enough depth for creature positions
- at least one light source
- optional door numbers
- optional wall message
- optional symbol anchors

Do not build multiple environments in v0.1.

## Creature

Recommended children:

```txt
Creature
  ModelOrSilhouette
  AnimationRoot
  AudioSource
```

Creature position anchors should exist in the corridor:

```txt
CreatureAnchors
  Far
  Visible
  MidCorridor
  NearDoor
  AtDoor
```

The creature can be:

- a capsule
- a dark plane
- a simple silhouette
- a placeholder humanoid
- a billboard sprite

The first version does not need final animation.

## Camera

Camera rules:

- fixed first-person view
- inside elevator
- portrait mobile framing
- corridor centered
- creature visible along the corridor depth
- no free movement
- no joystick
- no mouse-look requirement

Camera should show:

- part of elevator frame
- corridor entrance
- far corridor
- creature path
- enough empty space for UI

## UI

Recommended UI hierarchy:

```txt
UI
  Canvas
    SafeArea
      QuestionPanel
      AnswerButtons
      TimerView
      FeedbackOverlay
      ResultPanel
```

UI rules:

- mobile portrait first
- large tap targets
- readable text
- do not block creature unless intentional
- answer buttons must be fast to tap
- result screen must allow restart

## Lighting

Prototype lighting can be simple.

Recommended lighting objects:

```txt
Lighting
  DirectionalLight
  CorridorLight
  ElevatorLight
  FlickerLight
```

Rules:

- keep lights limited
- avoid expensive real-time lighting setups
- use darkness and contrast
- make the creature readable
- make the corridor creepy
- do not overbuild post-processing early

## Audio

Recommended hierarchy:

```txt
Audio
  AmbienceSource
  ElevatorSource
  CreatureSource
  UISource
```

Prototype audio should include placeholders for:

- elevator hum
- door sound
- wrong answer hit
- timeout hit
- creature close sound
- attack sound

## Placeholder Art Rules

Placeholders are allowed and encouraged.

Acceptable placeholders:

- cubes for walls
- plane for floor
- capsule for creature
- black material for silhouette
- simple point light
- temporary text labels
- primitive elevator frame

Do not waste time importing final assets in early phases.

## Mobile Performance Rules

For scene assembly:

- keep object count low
- avoid excessive transparent materials
- avoid heavy post-processing
- avoid unnecessary real-time shadows
- avoid large texture imports
- avoid physics unless needed
- keep scene loading simple

## Do Not Add

Unless explicitly requested, do not add:

- free movement
- first-person controller
- inventory
- physics-based doors
- cutscenes
- multiple corridors
- multiple creatures
- procedural generation
- asset packs
- complex shader effects
- VR controls

## Acceptance Criteria

A scene assembly task is acceptable if:

- scene opens without blocking errors
- hierarchy is clean
- player camera is inside elevator
- corridor is visible
- creature placeholder is visible or placeable
- UI canvas exists if needed
- scene supports the gameplay loop
- no heavy assets were added without approval
- mobile portrait framing was considered

## Manual Checks

When modifying the scene, verify:

1. Open `Game.unity`.
2. Press Play.
3. Camera shows elevator and corridor.
4. UI is visible.
5. Creature position is readable.
6. No blocking console errors.
7. No unrelated scene objects were added.
8. Hierarchy is understandable.

## Delivery Requirements

At the end of the task, report:

- scene files modified
- prefabs created or modified
- placeholder objects added
- lighting changes
- UI changes
- manual Play Mode checks
- console errors if any
- git status
- targeted commit recommendation

## Commit Examples

```bash
git add UnityProject/Assets/Scenes/Game.unity
git commit -m "🛗 feat(scene): add elevator corridor prototype scene"
```

```bash
git add UnityProject/Assets/Prefabs/Creature/PlaceholderCreature.prefab UnityProject/Assets/Scenes/Game.unity
git commit -m "👻 feat(creature): add placeholder hallway entity"
```
