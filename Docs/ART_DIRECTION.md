# Art Direction — Don’t Let Her In

## 1. Art Direction Summary

**Don’t Let Her In** is a mobile-first horror game set inside an elevator facing a haunted corridor.

The player never freely walks around in the prototype. The camera is fixed inside the elevator. The corridor is the danger space. The elevator is the temporary safe space.

The visual target is:

```txt
Dark elevator
Creepy corridor
Sick green/yellow/red lighting
Dirty hotel or hospital atmosphere
One unsettling female silhouette
Minimal UI
Claustrophobic composition
Strong tension through light, shadow and sound
```

The concept art defines the target mood, not the required quality for prototype v0.1.

Prototype v0.1 can use placeholders if the composition, threat readability and horror mood are clear.

---

## 2. Visual Pillars

## 2.1 Claustrophobia

The player must feel trapped inside the elevator.

The elevator should feel:

- narrow
- metallic
- old
- dirty
- unsafe
- temporarily protective
- surrounded by darkness

The player should see enough of the elevator frame to understand they are inside it.

## 2.2 Corridor as danger

The corridor is not just background.

It is the main danger zone.

The corridor should feel:

- long
- dark
- silent
- abandoned
- hostile
- deeper than it should be
- slightly unreal

The creature approaches from the corridor.

## 2.3 Creature readability

The player must always understand whether the creature is far, mid-distance, close or fatal.

Even with placeholder graphics, distance must be readable.

The creature should have strong silhouette value.

## 2.4 Horror through suggestion

Do not reveal everything.

The prototype should use:

- silhouettes
- flickering lights
- partial visibility
- distant movement
- red warning light
- dirty walls
- broken signage
- distorted UI
- darkness

Avoid relying only on gore or loud jumpscares.

## 2.5 Mobile clarity

Everything must remain readable on a phone screen.

The player must be able to see:

- the question
- the answers
- the timer
- the corridor
- the creature
- the consequence of a wrong answer

---

## 3. Mood Keywords

Use these keywords as art direction references:

```txt
claustrophobic
haunted hotel
abandoned hospital
dirty elevator
sick lighting
deep corridor
flickering neon
distant silhouette
analog horror
liminal space
uneasy symmetry
distorted safety
red emergency light
metallic decay
slow dread
```

Avoid these directions:

```txt
cartoon horror
bright arcade
cute monster
gore-first horror
high fantasy
sci-fi spaceship
colorful casual game
comedy horror
overloaded UI
```

---

## 4. Color Palette

## 4.1 Main palette

Recommended colors:

```txt
Dirty black
Dark green
Sick yellow
Old beige
Rust brown
Emergency red
Cold metal grey
Faded white
```

## 4.2 Functional color use

Use colors with purpose:

```txt
Green/yellow: sick elevator and corridor lighting
Red: danger, wrong answer, emergency, death pressure
White: readable UI text
Grey/black: background, metal, shadow
Dirty beige: walls, old hotel/hospital texture
```

## 4.3 Avoid

Avoid:

- saturated blue UI
- bright purple fantasy tones
- clean white hospital sci-fi look
- neon cyberpunk palette
- colorful mobile casual palette

The game should feel dirty, old and unsafe.

---

## 5. Lighting Direction

Lighting is one of the most important parts of the game.

Prototype lighting can be simple, but it must support fear.

## 5.1 Lighting goals

Lighting must:

- frame the corridor
- reveal the creature enough to create pressure
- hide details until close
- make distance readable
- create instability when stress increases
- support wrong answer and timeout feedback

## 5.2 Recommended prototype lighting

Use a simple setup:

```txt
ElevatorLight: weak warm/cold light inside elevator
CorridorLight: dim sick green/yellow corridor light
FlickerLight: unstable light used for tension
EmergencyLight: red light for danger/death states
```

## 5.3 Lighting states

Recommended mood states:

```txt
Stable: normal low horror light
Warning: mild flicker, darker corridor
WrongAnswer: red flash + blackout
Timeout: longer blackout + stronger red/green shift
NearDeath: red emergency flicker
Death: lights fail or hard red attack frame
```

## 5.4 Do not overbuild

For prototype v0.1:

- do not use complex volumetrics
- do not use many real-time lights
- do not require final baked lighting
- do not use expensive post-processing as core gameplay
- do not rely on perfect shadows

Darkness and composition are enough for first prototype.

---

## 6. Elevator Design

The elevator is the player’s frame.

## 6.1 Elevator identity

The elevator should feel:

- old
- metallic
- narrow
- slightly broken
- dirty
- not fully safe
- maybe haunted itself

Visual elements:

- metal walls
- dirty floor
- old control panel
- digital floor display
- flickering light
- emergency red light
- worn door frame
- scratched surfaces
- maybe a small mirror later

## 6.2 Prototype elevator

For v0.1, the elevator can be built from simple primitives.

Minimum elements:

```txt
floor
side walls
ceiling
door frame
left door placeholder
right door placeholder
button panel placeholder
digital display placeholder
camera inside elevator
```

The player should see the elevator frame around the corridor.

## 6.3 Elevator safe/unsafe contrast

The elevator should initially feel safer than the corridor.

But wrong answers and timeouts should make it feel less safe:

- lights flicker
- doors twitch
- display glitches
- red warning appears
- metal creaks
- creature sound enters the elevator space

---

## 7. Corridor Design

The corridor is the threat space.

## 7.1 Corridor identity

The corridor should feel like:

```txt
haunted hotel corridor
abandoned hospital corridor
liminal hallway
impossible depth
```

It should be simple enough for prototype but visually meaningful.

## 7.2 Prototype corridor elements

Minimum corridor elements:

```txt
floor
left wall
right wall
ceiling
far end darkness
door shapes
one or two lights
creature path
clue anchors
```

Optional elements:

```txt
room numbers
wall messages
symbols
paintings
bloodless stains
exit sign
broken light
old carpet
service cart
mirror
intercom speaker
```

Do not add too many props in v0.1.

The corridor must remain readable.

## 7.3 Corridor composition

The corridor should guide the eye toward the creature.

Use:

- central vanishing point
- repeated doors
- light pools
- darkness at far end
- creature silhouette centered or slightly off-center
- elevator frame as foreground

---

## 8. Creature Design

Prototype creature:

```txt
The Hallway Woman
La Dame du Couloir
```

## 8.1 Creature role

The creature is the visual timer.

The player must understand danger by looking at her position.

She does not need complex AI.

She should feel:

- slow
- inevitable
- silent at first
- wrong in movement
- human but not quite human
- more frightening when partially hidden

## 8.2 Creature silhouette

The silhouette should be readable from far away.

Suggested silhouette features:

- long hair
- long dress or hanging fabric
- thin limbs
- slightly tilted head
- narrow shoulders
- unnatural stillness
- slow forward posture

Avoid:

- overly detailed face early
- monster with too much visual noise
- cartoon zombie
- gore-focused design
- creature that reads as action enemy

## 8.3 Creature visibility by distance

```txt
100: invisible or barely suggested
80: small silhouette at corridor end
60: body shape readable
40: movement readable
25: close threat, more detail visible
10: at elevator doors, almost fatal
0: attack
```

## 8.4 Prototype creature asset

Acceptable v0.1 creature placeholders:

```txt
black capsule
flat black humanoid silhouette
simple mannequin
billboard sprite
low-poly humanoid
dark plane with alpha texture
```

The placeholder is acceptable if:

- distance is readable
- silhouette is threatening
- movement is visible
- wrong answer clearly brings it closer

## 8.5 Creature animation

Prototype animation can be minimal.

Acceptable:

- lerp between position anchors
- stepwise movement after answers
- slight idle sway
- blackout teleport closer after wrong answer
- attack lunge at death

Do not spend too much time on animation in v0.1.

---

## 9. Creature Position Anchors

The corridor should contain these anchors:

```txt
Far
Visible
MidCorridor
NearDoor
AtDoor
Attack
```

Suggested mapping:

```txt
Distance 100: Far or invisible
Distance 80: Far
Distance 60: Visible
Distance 40: MidCorridor
Distance 25: NearDoor
Distance 10: AtDoor
Distance 0: Attack
```

The visual controller can interpolate between anchors or snap between them.

For wrong answer and timeout, snapping closer after blackout is acceptable and effective.

---

## 10. UI Art Direction

The UI must be usable first, atmospheric second.

## 10.1 UI role

UI must show:

- question
- answer choices
- timer
- feedback
- result
- restart

The UI should not look like a school quiz.

## 10.2 UI style

Recommended direction:

```txt
dark transparent panel
old elevator display influence
red glitch on wrong answer
subtle noise texture later
minimal typography
low saturation
high readability
```

## 10.3 UI placement

Mobile portrait layout:

```txt
top: floor/timer optional
center: corridor and creature visibility
bottom: question and answers
overlay: wrong/timeout feedback
result panel: centered after death/victory
```

Do not block the creature with large UI.

## 10.4 Answer buttons

Answer buttons must be:

- large
- easy to tap
- readable
- visually distinct
- not too close together
- not tiny

Prototype buttons can be simple.

Final direction can make them feel like elevator buttons or corrupted UI.

---

## 11. Feedback Visual Direction

## 11.1 Correct fast

Visual feedback:

```txt
light stabilizes
creature recedes
door starts closing
small UI pulse
reduced red/danger effect
```

## 11.2 Correct normal

Visual feedback:

```txt
slight stabilization
creature slows or recedes slightly
pressure remains
```

## 11.3 Correct slow

Visual feedback:

```txt
door hesitates
creature barely recedes
light remains unstable
```

## 11.4 Wrong answer

Visual feedback:

```txt
red glitch
answer flashes
brief blackout
camera shake
creature closer after blackout
elevator display corruption
```

## 11.5 Timeout

Visual feedback:

```txt
timer hits zero
question disappears
light failure
stronger blackout
door twitch/jam
creature much closer
red emergency flicker
```

## 11.6 Death

Visual feedback:

```txt
lights fail
creature reaches elevator
hard red or black frame
attack flash or close-up silhouette
result screen
```

Death should be intense but not visually overloaded.

---

## 12. Audio-Visual Coordination

Visual feedback should match sound feedback.

Examples:

```txt
red flash + harsh metal hit
blackout + sudden footstep closer
light flicker + electrical buzz
creature near door + breathing/scrape
death frame + attack sound
```

Do not create visual events with no audio support if audio exists.

Audio and lighting are more important than detailed models in early prototype.

---

## 13. Placeholder Strategy

## 13.1 Allowed placeholders

The prototype can use:

- cubes for walls
- planes for floor
- capsules for creature
- simple black material for silhouette
- basic Unity lights
- simple TextMeshPro UI
- temporary audio clips
- primitive elevator frame

## 13.2 Placeholder naming

Use clear names:

```txt
PH_ElevatorWall
PH_ElevatorDoorLeft
PH_ElevatorDoorRight
PH_CorridorWallLeft
PH_CorridorWallRight
PH_CreatureSilhouette
PH_RoomNumberClue
PH_WallMessage
```

## 13.3 Placeholder rule

Every placeholder must be easy to replace later.

Do not hardwire logic to placeholder object names if avoidable.

Use serialized references or anchors.

---

## 14. Asset Strategy

Do not import heavy asset packs in v0.1 unless explicitly approved.

When assets are introduced later, prioritize:

```txt
one good elevator asset
one good corridor/hallway asset
one good creature silhouette/model
small horror audio pack
simple UI font/style
```

Avoid importing:

```txt
huge horror hospital pack
full character packs
large animation libraries
massive texture collections
unoptimized post-processing packs
```

Prototype first. Asset polish later.

---

## 15. Visual Quality by Milestone

## 15.1 v0.1 — First Fear Loop

Expected quality:

```txt
placeholder art
simple corridor
simple elevator
simple silhouette
readable distance
basic UI
basic wrong-answer feedback
basic death
```

Goal:

```txt
prove tension
```

## 15.2 v0.2 — Horror Vertical Slice

Expected quality:

```txt
better lighting
better corridor composition
improved creature silhouette
sound polish
stronger wrong-answer/timeout effects
more atmospheric UI
```

Goal:

```txt
make it feel scary
```

## 15.3 v0.3 — Polished Demo

Expected quality:

```txt
real assets or strong stylized placeholders
better animation
mobile build
optimized scene
improved UI
stronger audio mix
full 5-floor demo
```

Goal:

```txt
make someone want to replay and share
```

---

## 16. Do Not Do in v0.1

Do not spend v0.1 effort on:

- photorealism
- multiple locations
- multiple creatures
- VR visuals
- cinematic cutscenes
- expensive shaders
- complex animation graphs
- procedural horror systems
- advanced post-processing
- final menu design
- monetization UI
- store screenshots
- full story presentation

---

## 17. Art Acceptance Criteria

A visual task is acceptable if:

- it supports the core loop
- the corridor is readable
- the creature distance is readable
- the UI remains usable
- mobile portrait framing is respected
- the hierarchy remains clean
- no heavy assets were added without approval
- the mood remains horror, not arcade
- placeholders are clearly named
- the scene remains easy to replace later

---

## 18. Manual Visual Checks

After scene or art changes, check:

```txt
Can I tell I am inside an elevator?
Can I see the corridor clearly?
Can I see where the creature is?
Can I tell if the creature moved closer?
Does a wrong answer visually hurt?
Does timeout feel worse?
Does the UI block the creature?
Does the scene still work in portrait framing?
Does the mood feel horror?
Are there blocking console errors?
```

---

## 19. Current Art Direction Decision Log

Current decisions:

```txt
Camera: fixed inside elevator
Movement: none in prototype
Primary environment: elevator + corridor
Creature: one female silhouette
Style target: stylized realistic horror
Prototype art: placeholders allowed
Lighting: sick green/yellow/red
UI: minimal, readable, dark, glitch-capable
Gore: not a focus
Jumpscares: used sparingly
```

Future decisions:

```txt
final creature name
final creature model style
final elevator asset
final corridor asset
whether to use 3D model or billboard for creature
exact UI typography
exact color palette
audio asset pack
```
