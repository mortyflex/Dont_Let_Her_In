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

## 2026-06-19 — Use a descent loop instead of ascending progression

### Decision

The run is a **descent**: the player starts on a high floor and descends floor by floor to the Ground Floor to escape. Prototype v0.1 starts at Floor 5 and goes 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor.

### Context

Earlier prototype framing described moving "up" floors and ending on a victory after the final floor without a clear spatial direction. Phase 7B.4 committed an explicit descent with a narrative intro (wake up high, get to the ground floor).

### Reasoning

A descent gives the run a clear, readable goal ("reach the ground floor and escape") and a natural difficulty ramp (the deeper you go, the closer the threat starts).

### Consequences

- The displayed floor number counts DOWN (Floor 5 first, Floor 1 last before the Ground Floor).
- Escape (win) happens at the Ground Floor; ascending/"YOU ESCAPED after climbing" wording is obsolete.
- `DescentFloorProfile` owns the display order and per-floor start distance.

### Status

Accepted

---

## 2026-06-19 — Remove the Door Seal score mechanic from active gameplay

### Decision

The Door Seal scoring mechanic (Phase 7B.3) is removed from active gameplay. Floors are not cleared by reaching a score threshold.

### Context

Phase 7B.3 experimented with correct trials building a "Door Seal" score, where a floor was cleared only if the score passed a threshold and a too-low seal could fail the run. Phase 7B.4 replaced this with a simpler survival model.

### Reasoning

The score/threshold gate added cognitive load and a second failure axis that muddied the core tension. Surviving the trials is a clearer, more honest clear condition.

### Consequences

- A floor is cleared by surviving all 5 trials, with no score gate.
- `RecordCorrectSealed` records a correct answer without threat movement; there is no Door Seal score, `FloorThreatProfile` or `FloorTransitionText` in the active flow.
- Documentation must not describe Door Seal / score-based clear / score-based loss as current gameplay.

### Status

Superseded / Removed from active gameplay (experiment completed in Phase 7B.3, removed in Phase 7B.4)

---

## 2026-06-19 — Make the threat non-receding during a floor

### Decision

The threat never recedes during a floor. A correct answer consumes the trial and lets the player continue but does not push the creature back. Only wrong answers and timeouts move the threat closer.

### Context

The original model let fast/correct answers add distance (+18/+10/+3). Phase 7B.4 made the threat non-receding within a floor and resets it per floor.

### Reasoning

A non-receding threat keeps constant pressure and removes the "farm distance with easy answers" loop. Relief comes from surviving the floor and descending, not from regained distance.

### Consequences

- Confirmed values: correct = no change; wrong = -20 distance / +1 stress; timeout = -30 distance / +2 stress.
- Threat and stress reset at the start of each floor to the floor's start distance (Floor 5=85 .. Floor 1=65).
- The historical receding-threat values in `Docs/GAME_DESIGN.md` Section 6 are kept only as history.

### Status

Accepted

---

## 2026-06-19 — Floor clear is survival-based, not score-based

### Decision

A floor is cleared by surviving all 5 trials of that floor. There is no score requirement to clear a floor or to win.

### Context

Companion decision to removing Door Seal. Clarifies the explicit clear condition.

### Reasoning

Survival-based clearing is the simplest readable rule under pressure and matches the descent fantasy.

### Consequences

- `TrialFlowResolver` maps (isDead, isFinalTrial, isFinalFloor) to Lost / NextTrialSameFloor / FloorCleared / Escaped — no score input.
- A result-screen score may exist in a future phase but must never gate clearing or loss.

### Status

Accepted

---

## 2026-06-19 — Prototype uses 5 floors; full game may use ~15

### Decision

Prototype v0.1 uses 5 floors (5 trials each, 25 trials). The full game may start higher, such as Floor 15, for a longer descent.

### Context

Extends the earlier "3 to 5 floors" decision now that the descent is committed and content is authored as 5 floors x 5 trials.

### Reasoning

5 floors are enough to prove the descent and difficulty ramp without overbuilding content. A deeper start (e.g. Floor 15) is a content-scaling decision for later.

### Consequences

- `PrototypeFloorSet` defines 5 floors x 5 trials; `DescentFloorProfile` clamps start distance for floors 1–5.
- Scaling to ~15 floors later will need additional content and start-distance tuning.

### Status

Accepted

---

## 2026-06-19 — Plan EN/FR localization from the beginning

### Decision

English and French are planned from the beginning. Phase 7B.4 ships lightweight code-based localization for key UI/status/intro strings; question content remains English-only for now.

### Context

The user communicates in French and the game targets a French-speaking audience as well as English.

### Reasoning

Building a small localization layer early (English default, French available) avoids retrofitting it later, while deferring the larger task of translating question content.

### Consequences

- `PrototypeLocalization` + `LocalizedText` + `GameLanguage` provide switchable EN/FR strings (no Unity Localization package, no settings UI yet).
- Question / answer / cue content localization is deferred (recommended Phase 7E).

### Status

Accepted

---

## 2026-06-19 — Add a narrative intro before gameplay

### Decision

A short narrative intro is shown before the run, establishing the situation (wake up on Floor 5, the hallway is not empty) and the goal (answer trials, do not let her in, reach the ground floor), ending with BEGIN DESCENT.

### Context

Players need context for why they are trapped and what the objective is before the first trial.

### Reasoning

A brief intro frames the descent and the threat without a long lore dump, improving readability of the loop.

### Consequences

- The intro text is localized (EN/FR) via `PrototypeLocalization`.
- The intro must stay short and readable in portrait.

### Status

Accepted

---

## 2026-06-19 — Trials should become corridor-evidence-based

### Decision

Future trial content should be grounded in visible corridor clues rather than abstract
quiz questions. Each trial asks about something the player observed in the hallway.

### Context

The descent loop has 5 trials per floor, but the current prototype questions can read like
an abstract quiz disconnected from the corridor. Phase 7D defines an evidence-based
direction (see `Docs/CORRIDOR_OBSERVATION_DESIGN.md`).

### Reasoning

Grounding trials in observed evidence makes them feel fair, memorable and diegetic — it
matches the `horror-game-design` pillar "the environment is part of the question" and turns
the corridor into the source of the puzzle instead of a backdrop.

### Consequences

- Future content uses an `EvidenceTrial` that references a `CorridorClue` (`clueId`).
- Distractors must be plausible alternatives derived from the observation, not random.
- The current 25 prototype trials remain as the technical base until converted.

### Status

Accepted (planned — design only; not yet implemented)

---

## 2026-06-19 — Corridor layout stays mostly consistent while clues vary per floor

### Decision

The corridor remains structurally consistent across floors (same elevator framing, depth,
door positions, panels, fixtures); only the details (numbers, symbols, lights, messages,
objects, anomalies) change per floor.

### Context

A stable layout lets the player learn the space once, then notice what changed.

### Reasoning

Consistency makes anomaly-based trials fair ("what changed?" only works against a known
baseline) and keeps art/scope manageable for a prototype (one corridor, varied details).

### Consequences

- A future `FloorObservationSet` binds per-floor clues to shared corridor visual anchors.
- Anomaly clues become a first-class, fair clue type once the baseline is observable.

### Status

Accepted (planned)

---

## 2026-06-19 — Introduce an observation camera pass before the trial sequence (future phase)

### Decision

Before a floor's trials begin, an observation pass will play: the camera slowly travels
forward into the hallway to expose clues, then travels back to the elevator, then hands off
to the trials. This is planned for a future phase, not implemented now.

### Context

Evidence-based trials require the player to have actually seen the clues first.

### Reasoning

A short observe-then-return pass creates the observe -> remember -> answer rhythm and a
moment of false safety before the pressure of the trials, without changing threat/descent rules.

### Consequences

- A future `ObservationPhaseController` sequences the camera travel and raises an
  "observation complete" handoff; `PlayableRunFlowController` starts trials only after it.
- Camera travel, animation and tuning (`observationSeconds`) are deferred to Phase 7H.

### Status

Accepted (planned — not yet implemented)

---

## 2026-06-19 — No trial without visible evidence

### Decision

No trial may exist without a visible corridor clue, and no correct answer may exist without
observable evidence. Every trial references a `clueId` that is observable before the trial.

### Context

This is the core fairness rule for the evidence-based direction.

### Reasoning

It guarantees the player can always reason "I saw this, so I know the answer", eliminating
unfair quiz-style questions and ambiguous answer validation.

### Consequences

- Validation (future tests) must check that every `EvidenceTrial.clueId` exists in the
  floor's `FloorObservationSet` and that the correct answer matches the clue's evidence.
- Each floor must expose at least as many usable clues as it has trials (>= 5).

### Status

Accepted (planned)

---

## 2026-06-19 — Implement the evidence trial data model as data-only (DATA_MODEL_ONLY)

### Decision

Phase 7E implements the evidence-based trial data model as pure, testable data
(`CorridorClue`, `CorridorClueType`, `EvidenceAnswerOption`, `EvidenceTrial`,
`FloorObservationSet`), a `EvidenceTrialValidator` (+ `EvidenceValidationResult`) and a
25-trial `PrototypeEvidenceFloorSet` (EN/FR). The runtime trial flow is NOT switched to it:
`PlayableRunFlowController` still uses `PrototypeFloorSet`.

### Context

The corridor-observation direction (Phase 7D) needs a concrete, validatable data model
before any visual or camera work. Switching the live flow at the same time would risk the
verified descent loop.

### Reasoning

Building the data model first, decoupled from the runtime, lets content be authored and
proven well-formed (no trial without a clue, exactly 4 answers, exactly 1 correct, EN/FR
present) with EditMode tests, while keeping the playable game unchanged and reversible.

### Consequences

- Validation lives in `EvidenceTrialValidator`; the data containers stay permissive holders.
- The data types reuse `LocalizedText` / `GameLanguage` and evolve `QuestionData` /
  `QuestionCue` / `FloorTrial` / `FloorDefinition`.
- A future phase will wire `FloorObservationSet` into the runtime (after static clues and the
  observation camera pass). Camera travelling and visual clues remain planned, not implemented.
- EditMode tests increased from 148 to 179, all passing.

### Status

Accepted

---

## 2026-06-19 — Localize live playable trial content via additive French fields (Option A)

### Decision

Phase 7F localizes the live playable content (the 25 `PrototypeFloorSet` trials — prompts,
answers and cues) by adding optional French fields to `QuestionData`
(`promptFrench`, `answersFrench`) and `QuestionCue` (`labelFrench`, `linesFrench`). The
existing player-facing getters (`Prompt`, `Answers`, `Label`, `Lines`) resolve to
`PrototypeLocalization.Language` with English as the fallback. The runtime keeps using
`PrototypeFloorSet`; `PrototypeEvidenceFloorSet` stays data-only.

### Context

After Phase 7B.4 only UI/status/intro were EN/FR; question prompts, answers and cues were
English-only. The player should be able to play the prototype fully in French. Phase 7E added
the evidence data model but did not change the runtime.

### Reasoning

Option A (localize the current `PrototypeFloorSet` directly) is the lowest-risk path: it fixes
the player-facing limitation without wiring the evidence model into the runtime. Keeping the
answer model index-based means the correct answer, answer count and floor/trial structure are
identical across languages, and `GameplayUIController` needs no change (it already reads the
getters). Option B (evidence adapter at runtime) was deferred as higher risk.

### Consequences

- English remains the default; French is selectable in code/tests
  (`PrototypeLocalization.Language = GameLanguage.French`); no settings UI yet.
- `QuestionData`/`QuestionCue` getters now depend on the global current language (consistent
  with the existing `PrototypeLocalization` pattern); tests that switch language must reset it.
- Gameplay, threat tuning and descent are unchanged. EditMode tests increased to 189, all passing.
- A future phase may add a language settings UI and/or a runtime evidence adapter.

### Status

Accepted

---

## 2026-06-19 — Static corridor clues as a code-built HUD clue board (Option A)

### Decision

Phase 7G surfaces the evidence loop with a static "OBSERVED CLUES" board built in code on the
runtime HUD (`GameplayUIController`), not as in-corridor GameObjects. It shows the current
floor's 5 clues from `PrototypeEvidenceFloorSet` via a pure `CorridorClueDisplayFormatter`,
localized EN/FR, updated per floor from `PlayableRunFlowController.BeginFloor`. The playable
trials still come from `PrototypeFloorSet`.

### Context

The corridor-observation direction needs a first visible bridge so the player sees that
answers come from the corridor. A full per-anchor in-world clue layout (Option B) and a
camera observation pass are higher-risk and deferred.

### Reasoning

Option A is the lowest-risk way to prove the evidence relationship: the HUD is already built
in code, so adding a translucent board needs no `Game.unity` edit (no scene-merge risk) and no
new packages. Keeping the mapping/formatting pure makes it fully EditMode-testable. Reading
`PrototypeEvidenceFloorSet` (the preferred evidence source) keeps content in one place.

### Consequences

- `Game.unity` is unchanged; the board lives inside the runtime-built HUD and hides with it
  on the start/result screens; it is placed clear of the timer, answers and proximity warning.
- Clue content is theme-aligned to the playable floor by displayed number (~22/25 exact value
  matches); full 1:1 alignment and per-anchor in-world clues are follow-ups.
- Evidence values themselves are not separately localized (numbers/symbols are language-neutral;
  a few English words remain); the clue source label is localized EN/FR.
- Gameplay, threat tuning, descent and question localization are unchanged. EditMode tests: 204.

### Status

Accepted

---

## 2026-06-20 — Implement the observation camera pass as a HYBRID overlay + subtle camera ease (Phase 7H)

### Decision

Phase 7H realizes the deferred "observation camera pass" decision (2026-06-19) as a short
moment played once per floor, before its first trial, from `PlayableRunFlowController`. It is a
HYBRID: a localized `OBSERVE THE CORRIDOR` overlay (EN/FR) plus a subtle ease of the existing
Main Camera toward the corridor and back. The testable timing/state is isolated in pure classes
`ObservationPassTiming` and `ObservationPassState`. If no camera is found it degrades to an
overlay-only fallback with the same pacing. No Cinemachine, no new package, no `Game.unity` edit.

### Context

The static clue board (Phase 7G) shows where answers come from, but the player jumped straight
into a trial with no beat to read the corridor. A short observation pass adds that beat. The
earlier decision flagged camera work as higher-risk, so it had to stay subtle and regression-safe.

### Reasoning

A coroutine in the existing flow controller (run at floor start, after each descent, and after
restart) keeps the change inside one orchestrator. Running the pass while no question is active
means the timer, threat and trial count cannot advance for free — no rule duplication. Isolating
timing/state in pure classes keeps the logic EditMode-testable despite the coroutine/camera being
manual-only. The camera move uses the existing Main Camera with a stored home pose and a settle,
so it cannot drift, and overlay-only fallback guarantees the pass works even without a camera.

### Consequences

- `Game.unity` is unchanged; the overlay lives in the runtime-built HUD over the question/answer
  band only, so the corridor and the clue board above it stay visible during observation.
- During observation: question/answers hidden, status/cue/proximity cleared, timer/threat/trial
  count frozen, clue board visible. After it: normal trial flow resumes exactly as before.
- The observation pass does not run between trials, after answers, on wrong/timeout, or on
  win/loss. A duplicate pass cannot start while one is running; restart/result interrupt it
  safely and restore the camera/overlay.
- This is prototype pacing/readability, not final cinematic polish. EditMode tests: 219.

### Status

Accepted

---

## 2026-06-20 — Tune the observation pass and make the clue board observation-only (Phase 7H.1)

### Decision

Following user playtest feedback on Phase 7H, the observation pass is tuned: the camera moves
slower and farther toward the corridor/red light (move 0.6->1.2s, hold 2.0->2.5s, return
0.4->0.7s, ~4.4s total; forward 0.2->1.5m, height 0.05->0.1m), and the static corridor clue
board becomes **observation-only** — visible during the pass, then hidden the moment the first
question starts. The player observes the clues during the camera pass, then answers from memory.

### Context

Phase 7H worked technically but the camera move was too short/subtle to read as an observation,
and the clue board stayed visible during the questions, which removed the memory challenge. The
intended loop is: observe clues during the pass, then answer without them on screen.

### Reasoning

The timing/distance are plain serialized values on `PlayableRunFlowController`; the scene does
not serialize them (only `ui`/`creature`/`statusHoldSeconds`), so updating the C# field
initializers and the `ObservationPassTiming` defaults changes runtime with **no `Game.unity`
edit** (no scene-merge risk). Moving along the camera's own forward axis aims at the corridor/red
light regardless of world axes and returns to the stored home pose, so the camera never sticks
forward. Clue visibility is expressed as a pure, testable rule (`ObservationPassState.CluesVisible`
= visible only while observing) and wired with `GameplayUIController.UpdateClues` (show during
observation) + `HideClues()` (called when the trial starts).

### Consequences

- `Game.unity` is unchanged; behavior changes come from script defaults only.
- Clue board is no longer visible during the question phase (intended memory challenge); it still
  updates per floor, is shown during each floor's observation, and re-appears on descent/restart.
- The pass is slower (~4.4s) but still bounded (tests assert <= 6s, not excessive).
- World-space clues and persistent always-visible clues remain future work.
- Gameplay rules, threat/descent tuning and EN/FR localization are unchanged. EditMode tests: 227.

### Status

Accepted

---

## 2026-06-20 — Phase 7H.1 correction: slow observation travel + creature hidden during observation

### Decision

This is a correction/adjustment of Phase 7H.1 (NOT a new official phase). Over a few playtest
passes the observation camera move became a real slow travelling: ~8s travel toward the
corridor/red light, a brief ~0.5s hold, then ~8s travel back (~16.5s total, bounded under 17s),
reaching forward ~7m (height ~0.18m) so it stops just before the red light past the last doors.
The camera still returns to the gameplay pose before the question. Additionally, the creature is
hidden for the whole observation travel and only re-appears (per threat state) once the answer
phase starts. Only observation timing/distance and creature visual masking change; the clue board
rule and all gameplay are unchanged.

### Context

The first 7H.1 pass (then an intermediate ~5s pass) was still too fast/too short and stopped too
far from the red light, and the creature was visible during the travel. The user asked for ~8s out
and ~8s back, reaching just before the red light, with the creature only appearing while answering.

### Reasoning

The timing/distance are plain serialized values on `PlayableRunFlowController` plus the
`ObservationPassTiming` defaults; the scene does not serialize them, so updating the C# initializers
changes runtime with no `Game.unity` edit. Moving along the camera's own forward axis aims at the
corridor/red light and returns to the stored home pose, so the camera never sticks forward. The
creature mask is a pure visual toggle (`CreatureController.SetObservationHidden(bool)`) that hides
the visual root regardless of phase and restores phase-based visibility afterwards — it does not
touch distance, phase, stress or threat rules. No Cinemachine, no new package.

### Consequences

- `Game.unity` is unchanged; behavior changes come from script defaults only.
- The pass is much longer (~16.5s) but still bounded (tests assert <= 17s).
- The creature is invisible during observation and appears only in the answer phase per threat state.
- Clue board stays observation-only; descent, threat, trials and EN/FR localization are unchanged.
- A real per-clue / rail-based travelling and world-space clues remain future work. EditMode tests: 233.

### Status

Accepted

---

## 2026-06-20 — Prototype elevator descent transition between floors (Phase 7I)

### Decision

Between floors, after a NON-final floor is cleared, the game plays a prototype elevator descent
transition: hide the trial HUD and clue board, close two dark UI "doors", show DESCENDING with a
subtle vertical descent cue while the floor indicator updates, then open the doors and only then
start the next floor's observation pass. Timing ~0.8/0.8/1.4/0.8s (~3.8s, shorter than the
observation pass). The creature and clue board stay hidden for the whole transition. The final
Floor 1 escape shows the result instead and never runs a transition.

### Context

The descent previously only showed FLOOR CLEARED / DOORS CLOSING / DESCENDING text in a lower band.
It did not read as an elevator descending. Phase 7I adds a real door close/open + descent feel
without final art or scene changes, between the validated observation passes.

### Reasoning

UI overlay doors are the lowest-risk option: no `Game.unity` edit, no door models, no Cinemachine,
mobile-portrait friendly. The doors are two opaque panels driven by a 0..1 progress (open..closed);
the descent cue is a small damped vertical shake on the descent text (it never touches the
observation camera). Timing/distance live in plain serialized fields (scene does not serialize
them) plus a pure `ElevatorTransitionTiming`; gating is a pure `ElevatorTransitionState`
(answers/timer off, clue board hidden, creature hidden while active) mirroring the observation
state classes, so it is fully EditMode-testable. The clue board reveal moved from `BeginFloor` to
the start of the observation pass, so it is hidden during the transition and only shown during
observation (still observation-only). The transition only runs on `TrialResolution.FloorCleared`
(non-final), never on `Escaped`, so there is no transition after the final escape.

### Consequences

- `Game.unity` is unchanged; doors/cue are built in code in the runtime HUD.
- Observation starts ONLY after the doors open; clue board and creature stay hidden during descent.
- The old Phase 7B inter-floor text-band fields (`doorsClosingHoldSeconds`, `ascendingHoldSeconds`)
  are replaced by Phase 7I door fields (`doorCloseSeconds`, `descentHoldSeconds`, `doorOpenSeconds`).
- Descent loop, threat/clue/creature rules, restart and EN/FR localization are unchanged.
- Door/cue are prototype visuals; final door art, audio and richer descent feel remain future work.
  New localized DOORS OPENING / PORTES EN OUVERTURE; other beats reuse existing labels. EditMode tests: 251.

### Status

Accepted (adjusted — see the Phase 7I door framing/timing adjustment below)

---

## 2026-06-20 — Phase 7I door framing/timing adjustment (playtest correction)

### Decision

This is a playtest correction of Phase 7I (NOT a new phase). The descent transition is slower and
heavier (doorClose 0.8->1.5s, descent 1.4->3.0s, doorOpen 0.8->1.5s; ~6.8s total, bounded <= 8s,
still shorter than the observation pass), and the doors no longer cover the whole screen: they only
cover the central corridor aperture (`GameplayUIController.DoorApertureWidthRatio` = 0.68), so the
side cabin (buttons/walls) stays visible and the in-elevator feel is preserved.

### Context

The first Phase 7I doors closed too fast and covered the full screen, which lost the "inside the
elevator" feel. The user asked for a slower, heavier descent and for the doors to close around the
corridor opening only, keeping the cabin sides visible.

### Reasoning

Timing is plain serialized fields + the `ElevatorTransitionTiming` defaults (scene does not
serialize them, so no `Game.unity` edit). The doors are re-anchored within a centred aperture: each
leaf grows from its aperture edge to the centre as it closes and collapses to zero width at the
edge when open, so the side margins are never covered. The aperture ratio is a public const, making
the "not full-screen" rule unit-testable.

### Consequences

- `Game.unity` is unchanged; the cabin sides stay visible during the transition.
- The transition is longer (~6.8s) but still bounded and shorter than the observation pass.
- Gameplay, threat/clue/creature rules, observation pass, restart and EN/FR localization are unchanged.
- Real cabin/button art and door models remain future work. EditMode tests: 255.

### Status

Accepted

---

## 2026-06-20 — Phase 7I elevator cabin framing + destroy-safe creature visibility (playtest correction)

### Decision

This is a playtest correction of Phase 7I (NOT a new phase). Two changes: (1) a prototype elevator
cabin frame is built in code in the HUD — dark side panels filling the margins around the central
aperture, an amber floor plate showing the current floor (`ElevatorCabin.FloorPlateText`), and a
non-interactive button column (5..1 then G) with the current floor highlighted — visible during
observation/questions/transition so the player feels inside the cabin; (2) the Play Mode teardown
error "GameObjects can not be made active when they are being destroyed" is fixed.

### Context

The doors already closed around the central aperture, but the visible side margins read as empty,
not as a cabin. The user wanted buttons on one side and a floor plate on the left. Separately,
stopping Play Mode threw a SetActive-during-destroy error from `CreatureController.UpdateVisibility`
via `PlayableRunFlowController.OnDestroy -> StopObservationRoutine -> SetObservationHidden`.

### Reasoning

The cabin is pure UI built in code (no `Game.unity` edit, no assets), placed only in the side
margins so it never covers the corridor aperture; the testable values live in a pure `ElevatorCabin`
helper. For the teardown fix, `CreatureController` sets `_isDestroying` in `OnDestroy` and
`UpdateVisibility` returns early (no SetActive) while destroying; `PlayableRunFlowController` sets
its own `_isDestroying`, and `StopObservationRoutine(restoreVisuals)` is called with `false` from
`OnDestroy`, so no visual restore touches objects being destroyed.

### Consequences

- `Game.unity` is unchanged; the cabin frame and the fix are code-only.
- Stopping Play Mode no longer logs the SetActive-during-destroy error.
- The cabin stays visible during observation/questions/transition; the floor plate/button highlight
  update per floor; the doors still cover only the central aperture (ratio 0.68).
- Creature masking, threat/clue rules, observation pass, restart and EN/FR localization are unchanged.
- Real cabin/button art remains future work. EditMode tests: 261.

### Status

Accepted

---

## 4. Replaced or Deprecated Decisions

- **Door Seal scoring (Phase 7B.3)** — completed as an experiment, then removed from active
  gameplay in Phase 7B.4. See the 2026-06-19 decision above. Floors are now cleared by
  surviving 5 trials, not by a score threshold.
- **Receding threat / "correct answer pushes the creature back"** — replaced by the
  non-receding threat model (2026-06-19). The historical +distance values remain only as
  history in `Docs/GAME_DESIGN.md` Section 6.
- **Ascending / one-question-per-floor framing** — replaced by the descent loop with 5
  trials per floor (2026-06-19).

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
