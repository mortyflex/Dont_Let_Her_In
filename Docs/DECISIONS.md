# Decisions — Don’t Let Her In

## 1. Purpose

This document records important product, design and technical decisions for **Don’t Let Her In**.

The goal is to keep a clear project memory so the agent does not revisit settled decisions without a valid reason.

Every important change in direction must be added here.

---

## 2. Decision Format

Use this format for future decisions:

```md
## YYYY-MM-DD — Decision title

### Decision

Clear statement of the decision.

### Context

Why this decision was needed.

### Reasoning

Why this option was chosen.

### Consequences

What this decision changes or prevents.

### Status

Accepted / Replaced / Deprecated
```

---

## 3. Current Accepted Decisions

## 2026-06-18 — Use Unity 6 as the game engine

### Decision

The project will use Unity 6 as the game engine.

### Context

The game is a mobile-first 3D horror prototype with a fixed first-person camera inside an elevator, a corridor, a creature, lighting, audio and future VR/XR potential.

### Reasoning

Unity is better suited than React, React Native or Three.js for this project because it provides:

- native 3D scene workflow
- mobile build pipeline
- URP rendering
- animation support
- lighting support
- audio support
- prefab workflow
- Unity Test Framework
- future VR/XR path
- asset ecosystem

The user is a web developer, but the game concept depends more on 3D scene composition, lighting, audio and mobile build support than on web UI skills.

### Consequences

The user must learn enough Unity to:

- open the project
- use Play Mode
- inspect the hierarchy
- assign references
- test the scene
- build to iOS later
- read console errors

The agent can handle most C# and Unity implementation tasks, but the user remains responsible for judging game feel.

### Status

Accepted

---

## 2026-06-18 — Use URP for rendering

### Decision

The project will use Unity URP.

### Context

The project targets mobile first and needs a rendering pipeline appropriate for mobile performance.

### Reasoning

URP is a better fit for mobile than heavier rendering options. The prototype needs controlled lighting, simple materials and mobile-friendly performance.

### Consequences

The art direction must respect mobile constraints:

- limited real-time lights
- limited shadows
- simple materials first
- no heavy post-processing in v0.1

### Status

Accepted

---

## 2026-06-18 — Start with iOS as the initial platform

### Decision

The initial platform is iOS mobile portrait.

Android is secondary.

### Context

The first plan mentioned Android first because Android can be simpler for many mobile prototypes. The user clarified that iOS is preferred and likely better aligned with the expected user base.

The user also has an iPhone 16 Pro and a MacBook Pro M4 Pro, making iOS local testing realistic.

### Reasoning

iOS first is coherent because:

- the user has a Mac
- the user has an iPhone test device
- Unity can export to Xcode on macOS
- iOS is the desired initial audience
- TestFlight can be used later for external testing

### Consequences

Project docs must describe:

- initial platform: iOS mobile portrait
- secondary platform: Android
- future platform: VR/XR
- local build path: Unity to Xcode to iPhone

The prototype must still avoid iOS-specific complexity until the core gameplay loop works.

### Status

Accepted

---

## 2026-06-18 — Use portrait orientation for the prototype

### Decision

The prototype will be mobile portrait.

### Context

The game is intended as a mobile-first horror experience with short sessions.

### Reasoning

Portrait orientation is appropriate because:

- it feels natural on mobile
- it supports fast one-handed play
- it frames an elevator/corridor view effectively
- it helps keep the experience casual-accessible
- it differentiates the game from desktop horror games

### Consequences

All UI and scene composition must consider portrait framing.

The camera must show:

- elevator frame
- corridor
- creature path
- answer UI
- timer

### Status

Accepted

---

## 2026-06-18 — Use a fixed first-person camera inside the elevator

### Decision

The prototype will use a fixed first-person camera inside the elevator.

The player will not walk freely in v0.1.

### Context

The original concept is based on the player being trapped inside an elevator, facing a corridor where a creature approaches.

### Reasoning

A fixed camera makes the prototype more feasible because it reduces:

- movement complexity
- input complexity
- mobile control problems
- level design scope
- performance risk
- camera bugs
- motion sickness concerns for future VR adaptation

It also strengthens the core fantasy:

```txt
You are trapped. You cannot run. You can only answer.
```

### Consequences

The prototype must not include:

- joystick
- walking
- free-look requirement
- inventory navigation
- open level exploration

The tension must come from the creature, timer, questions, sound and lighting.

### Status

Accepted

---

## 2026-06-18 — Build a small playable prototype before final graphics

### Decision

The project will first build a small playable prototype with placeholders.

Final-quality graphics are not required for v0.1.

### Context

The concept art establishes the target atmosphere, but recreating that quality immediately would be unrealistic for a solo developer starting game development.

### Reasoning

The main risk is not whether final graphics can be polished later.

The main risk is whether the core loop is actually tense and fun:

```txt
question
timer
creature approaches
answer
consequence
death or next floor
```

### Consequences

v0.1 can use:

- primitive elevator
- primitive corridor
- simple silhouette creature
- basic UI
- basic lights
- placeholder sounds

The prototype is successful if the loop works and creates pressure, even before final art.

### Status

Accepted

---

## 2026-06-18 — Use one creature in Prototype v0.1

### Decision

Prototype v0.1 will use one creature only.

Working name:

```txt
The Hallway Woman
```

French internal reference:

```txt
La Dame du Couloir
```

### Context

The game’s fear depends heavily on the creature’s presence and movement.

### Reasoning

One well-directed creature is better than multiple weak creatures.

The prototype must first prove:

- distance readability
- wrong-answer consequence
- timeout consequence
- attack/death sequence
- player fear response

### Consequences

Do not add:

- multiple monsters
- creature variants
- enemy AI
- pathfinding
- random monster behavior

The creature is distance-driven, not AI-driven.

### Status

Accepted

---

## 2026-06-18 — Use 3 to 5 floors for Prototype v0.1

### Decision

Prototype v0.1 will contain 3 to 5 floors.

Minimum acceptable version:

```txt
3 floors
5 questions
```

Preferred version:

```txt
5 floors
10 questions
```

### Context

The game needs enough progression to prove tension, but not so much content that production becomes too large.

### Reasoning

3 floors are enough to test the loop.  
5 floors are enough to show progression and tension escalation.

### Consequences

Do not build 13 floors in v0.1.

The 13-floor structure can be considered later for a fuller demo or story mode.

### Status

Accepted

---

## 2026-06-18 — Do not implement VR in Prototype v0.1

### Decision

VR/XR is a future possibility, not part of v0.1.

### Context

The concept fits VR well, but the user has no game development experience and the first goal is a mobile playable prototype.

### Reasoning

VR would increase complexity:

- performance constraints
- input complexity
- camera comfort
- hardware testing
- build setup
- interaction design
- motion sickness concerns

### Consequences

The architecture should avoid choices that block future VR, but no VR systems should be implemented now.

Do not add:

- XR SDK
- VR input
- headset camera rig
- hand tracking
- VR-specific UI

### Status

Accepted

---

## 2026-06-18 — Avoid monetization systems in Prototype v0.1

### Decision

Prototype v0.1 will not include ads, in-app purchases, subscriptions or monetization SDKs.

### Context

The current objective is to prove gameplay.

### Reasoning

Monetization would add unnecessary complexity before the core loop is validated.

### Consequences

Do not add:

- ads SDK
- IAP SDK
- shop UI
- rewarded ads
- subscription logic
- purchase restoration

Monetization can be discussed after a compelling playable demo exists.

### Status

Accepted

---

## 2026-06-18 — Use ScriptableObjects for gameplay content where possible

### Decision

Gameplay content should be data-driven using ScriptableObjects where practical.

### Context

The game will need configurable questions, floors, creatures, difficulty and horror events.

### Reasoning

ScriptableObjects allow the agent and user to add or adjust content without hardcoding everything inside gameplay managers.

### Consequences

Use ScriptableObjects for:

- `QuestionData`
- `FloorData`
- `CreatureData`
- `DifficultyData`
- `AudioCueData`
- `HorrorEventData`

Runtime state should not be stored in ScriptableObjects unless intentionally designed.

### Status

Accepted

---

## 2026-06-18 — Keep the first agent phase documentation-first

### Decision

The project begins with repo structure, documentation, skills and AGENTS.md before implementation.

### Context

The user wants to work alone with a coding agent and iterate step by step.

### Reasoning

A documentation-first setup reduces agent drift and prevents premature overbuilding.

### Consequences

Before gameplay implementation, the repo should contain:

- `AGENTS.md`
- `Docs/PRD.md`
- `Docs/GAME_DESIGN.md`
- `Docs/ART_DIRECTION.md`
- `Docs/TECH_ARCHITECTURE.md`
- `Docs/ROADMAP.md`
- `Docs/TEST_PLAN.md`
- `Docs/DECISIONS.md`
- `Skills/*/SKILL.md`

### Status

Accepted

---

## 4. Replaced or Deprecated Decisions

No replaced or deprecated decisions yet.

---

## 5. Open Decisions

These decisions remain open:

```txt
Final game name
Final creature name
Final UI typography
Final visual asset strategy
Whether first external test uses TestFlight
Whether the first public demo targets iOS only or iOS + Android
Whether to add haptics in v0.1 or v0.2
Whether to add scoring in v0.1 or postpone it
Whether to create a 3-floor first playable before expanding to 5 floors
```

---

## 6. Decision Update Rule

When an accepted decision changes:

1. Do not delete the old decision.
2. Mark old decision as `Replaced`.
3. Add a new decision entry.
4. Explain why the change happened.
5. Update affected docs.

Example:

```md
### Status

Replaced by `YYYY-MM-DD — New decision title`
```
