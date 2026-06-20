# Corridor Observation Design

> **Status: PARTIALLY IMPLEMENTED.** The design was authored in Phase 7D. The **pure data
> model** (`CorridorClue`, `CorridorClueType`, `EvidenceAnswerOption`, `EvidenceTrial`,
> `FloorObservationSet`, `EvidenceTrialValidator`, `PrototypeEvidenceFloorSet`) is now
> implemented and tested in **Phase 7E (DATA_MODEL_ONLY)**. Still **not** implemented: the
> observation camera pass, the visual clue rendering, and any runtime use of the evidence
> content (the live trial flow still uses `PrototypeFloorSet`). The current game has the
> descent loop with 5 trials per floor (Phase 7B.4); this document defines how those trials
> become **evidence-based corridor observation puzzles** across phases.

## Purpose

Today the 5 trials per floor are a solid technical base (timer, answer evaluation,
non-receding threat, descent flow), but their content can read like an **abstract quiz**:
the prompt asks about something the player has no in-world reason to know.

Corridor observation exists to make every trial feel **grounded, memorable and fair**:

- **Grounded** — each trial asks about something the player actually saw in the hallway.
- **Memorable** — the player observes, then recalls under pressure (the fun tension).
- **Fair** — the correct answer is always deducible from observed evidence, and the
  distractors are plausible alternatives the player might confuse, never random noise.

The guiding sentence we want the player to think:

```txt
I saw this in the hallway, so I know the answer.
```

And never:

```txt
The game asked me a random quiz question.
```

This aligns with the `horror-game-design` skill pillar *"the environment is part of the
question"* and the existing `QuestionCue` concept (each question already carries a short
clue), which this design promotes to a first-class, in-world **corridor clue**.

## Player Experience

Intended rhythm of a floor:

```txt
observe  -> the doors open, the camera travels into the hallway, the player studies it
remember -> the player notes door numbers, symbols, lights, messages, objects, anomalies
return   -> the camera travels back to the elevator (safety shrinks, threat is felt)
answer   -> the 5 trials ask about what was visible during observation
survive  -> wrong/timeout bring her closer; correct consumes the trial (threat unchanged)
descend  -> surviving all 5 trials closes the doors and descends one floor
```

Emotional target: a short, calm-but-uneasy observation window, then rising panic as the
trials force recall while the creature waits. The observation pass is the *false safety*;
the trials are where hesitation costs distance.

## Floor Loop With Observation

Future per-floor flow (planned):

```txt
intro / previous descent transition (existing)
doors open
observation pass begins
  camera slowly travels forward into the hallway   (PLANNED — not implemented)
  clue exposure: floor-specific details are visible (PLANNED)
  camera slowly travels backward to the elevator    (PLANNED)
observation hands off to the trial sequence
trial 1..5
  each trial asks about a clue visible during observation
  correct -> trial consumed, threat does NOT recede (current rule, unchanged)
  wrong/timeout -> trial consumed, threat moves closer (current rule, unchanged)
floor clear (survive all 5 trials) -> doors close -> descend
  or loss (threat distance <= 0) -> SHE GOT IN
```

This loop **wraps** the current trial flow; it does not change the threat rules, the
descent rules, or the win/loss conditions defined in `Docs/GAME_DESIGN.md` Section 0.

## Corridor Structure

The corridor stays **structurally consistent across floors** so the player learns the
space once and then notices what changed. Only the details vary per floor.

Consistent across floors:

```txt
same elevator framing (fixed first-person, portrait)
same hallway depth and perspective
same door positions
same wall panels
same light fixtures and general layout
```

Variable per floor (the clues):

```txt
door numbers
wall messages
symbols
lights (which one is on/off, color, flicker)
objects (placement, presence/absence)
anomalies (something subtly wrong vs. the learned layout)
shadow / silhouette
direction instructions
scratched codes
door states (open / closed / ajar)
```

Because the layout is constant, **anomaly** clues become powerful: "what changed since the
previous floor?" is a fair question only when the baseline is stable and was observed.

## Clue Types

Proposed clue categories (`CorridorClueType`). For each: *what the player sees*, *how a
trial can ask about it*, and *what makes it fair vs. unfair*.

### DoorNumber

- **Sees:** a number plate on a corridor door (e.g. `104`).
- **Trial:** "Which room number was lit / closest to the elevator?"
- **Fair:** number is large, high-contrast, readable in portrait; distractors are
  similar-looking numbers (`104 / 140 / 401`).
- **Unfair:** tiny digits, too many similar numbers on screen at once.

### WallMessage

- **Sees:** a short scrawled message (e.g. `DO NOT LOOK LEFT`).
- **Trial:** "What did the wall say?"
- **Fair:** 1–4 short words, localized; distractors are plausible instructions.
- **Unfair:** long sentences, paragraph text, language-only nuance that breaks in FR.

### Symbol

- **Sees:** an icon/glyph (eye, key, hand, door).
- **Trial:** "Which symbol was in the center?"
- **Fair:** distinct silhouettes recognizable without color; highlight a single one.
- **Unfair:** symbols that differ only by tiny detail or only by color.

### LightState

- **Sees:** which fixture is on/off/flickering.
- **Trial:** "Which light stayed on?"
- **Fair:** clear on/off contrast; position-based, not color-only.
- **Unfair:** subtle brightness differences; color-only distinction (accessibility).

### ObjectPlacement

- **Sees:** an object's position or presence (a chair, a bag, a wheelchair).
- **Trial:** "Where was the object?" / "What was near the third door?"
- **Fair:** object clearly framed during the camera pass.
- **Unfair:** object only visible for a fraction of the pass or off-frame in portrait.

### Anomaly

- **Sees:** something different from the learned/baseline corridor.
- **Trial:** "What changed on this floor?"
- **Fair:** the baseline was observable on earlier floors; the change is visible.
- **Unfair:** requiring memory of a detail never emphasized, or pixel-hunting.

### ColorCue

- **Sees:** a colored element (red door, green light).
- **Trial:** "Which door was red?"
- **Fair:** color **plus** a position/shape so it is not color-only (accessibility).
- **Unfair:** color-only with no secondary cue; relies on perfect color perception.

### AudioProxy

- **Sees/Reads:** a *visual proxy* for sound until real audio exists (e.g. an intercom
  showing `2 · 7 · 2`, a speaker icon with a code).
- **Trial:** "What code came through the intercom?"
- **Fair:** the proxy is on-screen text/visual; does not require headphones.
- **Unfair:** depending on real audio before the audio system exists.

### ShadowOrSilhouette

- **Sees:** a silhouette/shadow at a position in the corridor.
- **Trial:** "Where was the silhouette?" / "Which way did the shadow face?"
- **Fair:** clearly separated from the creature's own threat silhouette; unambiguous.
- **Unfair:** confusing the clue silhouette with the approaching creature.

### DirectionInstruction

- **Sees:** an arrow or directional sign.
- **Trial:** "Which direction did the arrow point?"
- **Fair:** large arrow, 4 clear directions.
- **Unfair:** ambiguous diagonal arrows or tiny signage.

## Evidence-Based Trial Rules

Official rules for future trial content:

```txt
1. Every trial references exactly one clueId.
2. Every referenced clue must be observable during the observation pass, before the trial.
3. The correct answer must be derivable from the observed clue (the evidence).
4. Distractors must be plausible (similar numbers/symbols/instructions), never arbitrary.
5. A clue may be re-shown in the trial UI ONLY as a memory/recall aid, never in a way that
   directly leaks the correct answer (e.g. show the clue label, not the answer).
6. No trial may exist without a visible corridor clue. (No-trial-without-clue rule.)
7. No correct answer may exist without observable evidence.
8. Avoid asking about details too small to read in mobile portrait.
9. Avoid color-only recognition unless an accessible secondary cue exists (shape/position).
10. Avoid audio-only clues until a real audio system exists; use a visual AudioProxy instead.
11. Keep prompts and answers short enough to read and answer under timer pressure.
12. Each floor must expose at least as many usable clues as it has trials (>= 5).
```

These rules extend, they do not replace, the current threat/descent rules: correct still
consumes the trial without pushing the threat back; wrong/timeout still bring her closer.

## Data Model Proposal

> **Implemented in Phase 7E (data only).** These types now exist as pure-data classes in
> `UnityProject/Assets/Scripts/Questions/` (`CorridorClue`, `CorridorClueType`,
> `EvidenceAnswerOption`, `EvidenceTrial`, `FloorObservationSet`, plus `EvidenceTrialValidator`
> / `EvidenceValidationResult` for validation and `PrototypeEvidenceFloorSet` for 25 sample
> trials). They evolve the existing `QuestionData` / `QuestionCue` / `FloorTrial` /
> `FloorDefinition`: `EvidenceTrial` generalizes `FloorTrial` (question + cue), and
> `CorridorClue` generalizes `QuestionCue` with an in-world visual anchor and an explicit
> evidence value. The fields below match the implementation (minor naming aligned to C#).
> Not yet built: the `ObservationPhaseController` / `CorridorObservationController`
> MonoBehaviours and any runtime use of this content.

### CorridorClueType (enum)

```txt
DoorNumber
WallMessage
Symbol
LightState
ObjectPlacement
Anomaly
ColorCue
AudioProxy
ShadowOrSilhouette
DirectionInstruction
```

### CorridorClue

```txt
CorridorClue:
- id                 : stable string id (English), referenced by trials
- type               : CorridorClueType
- floorDisplayNumber : which displayed floor this clue belongs to (5..1)
- label              : short source header (e.g. "ROOM DISPLAY", "WALL") — localizable
- localizedDescription : EN/FR description of what is visible (for UI/recall aid)
- visualAnchor       : reference to the corridor position where it appears (anchor id)
- evidenceValue      : the ground-truth value the clue establishes (e.g. "104", "KEY")
- difficultyWeight   : relative difficulty contribution (for pacing/selection)
- isRequiredForTrial : true if at least one trial must reference this clue
```

### FloorObservationSet

```txt
FloorObservationSet:
- floorDisplayNumber : 5..1
- clues              : ordered list of CorridorClue for this floor
- observationSeconds : suggested duration of the observation pass (tuning)
- minCluesForTrials  : must be >= number of trials on the floor (>= 5)
```

### EvidenceAnswerOption

```txt
EvidenceAnswerOption:
- id                 : stable string id (English)
- localizedText      : EN/FR answer text (or a language-independent visual reference)
- isCorrect          : whether this option matches the clue's evidenceValue
- plausibilityNote   : design note explaining why this distractor is plausible (not random)
```

### EvidenceTrial

```txt
EvidenceTrial:
- id              : stable string id (English)
- clueId          : the CorridorClue this trial is about (REQUIRED, must exist)
- prompt          : EN/FR question text
- answers         : list of EvidenceAnswerOption
- correctAnswerId : id of the correct EvidenceAnswerOption (must match clue evidence)
- timeLimit       : seconds (reuses current per-floor timer tuning)
- difficulty      : difficulty tier
- localization    : EN/FR coverage marker for prompt/answers
```

### ObservationPhaseController (planned MonoBehaviour)

```txt
Responsibility (future): drive the observation pass for the current floor:
- play the forward camera travel into the hallway
- expose/enable the floor's CorridorClue visuals
- play the backward camera travel to the elevator
- raise an "observation complete" event so the trial sequence can begin
Owns NO trial/threat rules. It only sequences observation and hands off.
```

### CorridorObservationController (planned MonoBehaviour)

```txt
Responsibility (future): own the corridor scene content for observation:
- bind a FloorObservationSet to corridor visual anchors (visualAnchor -> scene position)
- show/hide per-floor clue visuals as the floor changes
- keep the corridor structurally consistent while swapping details per floor
Owns NO trial/threat rules.
```

Integration note: `PlayableRunFlowController` would, in a future phase, request an
observation pass (via `ObservationPhaseController`) when a floor begins, and start the trial
sequence only after the "observation complete" handoff. The current trial flow, threat
rules and descent transitions stay unchanged.

### Phase 7H implementation status (first observation pass prototype)

```txt
Phase 7H adds the FIRST observation camera pass, but inside PlayableRunFlowController itself
(a coroutine), not yet as the separate ObservationPhaseController/CorridorObservationController
MonoBehaviours above (those remain the future, scene-driven target).

What is implemented:
- a short OBSERVE THE CORRIDOR overlay (localized EN/FR via PrototypeLocalization.ObserveTitle /
  ObserveSubtitle) shown once per floor, BEFORE the first trial.
- a subtle ease of the existing Main Camera toward the corridor and back (HYBRID), with an
  overlay-only fallback if no camera is found. No Cinemachine, no new package, no Game.unity edit.
- runs at run start, after each descent (Floor 4/3/2/1), and after restart; never between trials,
  after answers, on wrong/timeout, or on win/loss.
- during the pass: question/answers hidden, timer/threat/trial count frozen, clue board visible.
- testable timing/state isolated in pure classes ObservationPassTiming / ObservationPassState.

Still future (unchanged from the plan above):
- per-anchor in-world clue visuals revealed by the pass (still the static HUD clue board for now).
- the dedicated ObservationPhaseController / CorridorObservationController scene MonoBehaviours.
- a real forward/back camera travel along corridor rails; Phase 7H is intentionally subtle.
```

## Localization Considerations

```txt
EN/FR must apply to: trial prompts, answer text, clue descriptions/labels, intro, UI/status.
Code identifiers (ids, type names, field names) stay in English.
Visual clues should avoid language dependence when possible (a door number, an arrow, a
  symbol reads the same in EN and FR); prefer language-independent visuals for the evidence.
Text-based clues (wall messages, instructions) MUST be localized EN/FR.
The existing PrototypeLocalization / LocalizedText / GameLanguage approach should be reused
  (lightweight, code-based, English default, French available; no Unity Localization package).
Phase 7F localized the live 25 prototype questions/answers/cues (PrototypeFloorSet) EN/FR via
  optional French fields on QuestionData/QuestionCue. The evidence prototype set
  (PrototypeEvidenceFloorSet) is also EN/FR. Future evidence content should follow the same
  EN/FR pattern.
```

Caution for **anomaly** and **wall message** clues: keep them short so the FR translation
stays readable in portrait and does not overflow the cue panel.

## Prototype Implementation Plan

Sequence (status reflects actual implementation):

```txt
Phase 7E — Evidence Trial Data Model — DONE (DATA_MODEL_ONLY)
           CorridorClue, CorridorClueType, EvidenceAnswerOption, EvidenceTrial,
           FloorObservationSet, EvidenceTrialValidator/EvidenceValidationResult and a 25-trial
           PrototypeEvidenceFloorSet, all pure data and EditMode-tested. The runtime trial
           flow still uses PrototypeFloorSet; nothing visual was added.

Phase 7F — Question Content Localization EN/FR — DONE
           Localized the live 25 prototype questions/answers/cues used by PrototypeFloorSet
           via optional French fields on QuestionData/QuestionCue, resolved by
           PrototypeLocalization.Language. Runtime localizes EN/FR with no gameplay change.

Phase 7G — Static Corridor Clue Prototype — DONE (CLUE_BOARD)
           A code-built "OBSERVED CLUES" board on the runtime HUD shows the current floor's
           5 evidence clues (CorridorClueDisplayFormatter reading PrototypeEvidenceFloorSet),
           localized EN/FR, updated per floor on BeginFloor. Display only — no in-corridor
           anchors yet, no camera travel, no Game.unity edit. (Option B per-anchor clues and
           1:1 evidence/PrototypeFloorSet value alignment remain follow-ups.)

Phase 7H — Observation Camera Pass Prototype (planned)
           Add ObservationPhaseController: slow forward/backward camera travel and the
           handoff to the trial sequence. Tuning of observationSeconds.

Phase 7I — Evidence-Based Floor Playtest (planned)
           Drive a floor (e.g. Floor 5) from PrototypeEvidenceFloorSet end to end and
           playtest the observe -> remember -> answer rhythm.
```

Rationale: build the pure data model and its validation first (7E, done) so content is
authorable and provably well-formed before any visual/runtime work; then localization of the
live questions (7F), static visuals (7G), motion (7H), and a full vertical-slice playtest (7I).
Each phase is independently shippable and reversible.

## Out of Scope For Now

Explicitly excluded from this design phase and from the near-term prototype:

```txt
camera travelling implementation
animation polish
final corridor art
procedural clue generation
full localization of all questions (only planned, sequenced in 7E)
real audio clue system (use a visual AudioProxy until audio exists)
jumpscare cinematic
pathfinding enemy
changes to the current 25 prototype questions
changes to PlayableRunFlowController / ThreatManager / CreatureController
changes to Game.unity or any prefab/art/audio asset
```

## Acceptance Checklist

A future agent can consider an evidence-based floor ready when:

```txt
[ ] Every EvidenceTrial references a clueId that exists in the floor's FloorObservationSet.
[ ] Every referenced clue is observable during the observation pass, before its trial.
[ ] Each correct answer is derivable from the observed clue's evidenceValue.
[ ] Distractors are plausible (documented plausibilityNote), never arbitrary.
[ ] No trial exists without a corridor clue (no-trial-without-clue rule holds).
[ ] Each floor exposes >= 5 usable clues (one per trial, minCluesForTrials satisfied).
[ ] All clue text, prompts and answers are localized EN/FR (or language-independent visuals).
[ ] Clues are readable in mobile portrait (size/contrast); no color-only recognition.
[ ] The observation pass can complete and hand off cleanly to the trial sequence.
[ ] Threat/descent rules are unchanged: correct consumes trial without receding the threat;
    wrong/timeout bring the threat closer; surviving 5 trials descends.
[ ] No generated Unity files, art or audio are introduced by the data/design work.
```
