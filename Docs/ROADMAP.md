# Roadmap — Don’t Let Her In

## 1. Roadmap Summary

This roadmap defines the step-by-step development plan for **Don’t Let Her In**.

The project must be built through small, testable phases.

The current goal is not to build the full game.  
The current goal is to build a playable prototype called:

```txt
Prototype v0.1 — First Fear Loop
```

The prototype must prove this core loop:

```txt
Question starts
Timer starts
Creature advances
Player answers
Answer is evaluated
Threat distance changes
Next floor or death
```

---

## 2. Development Principles

The roadmap follows these principles:

```txt
Documentation before implementation
Core loop before art polish
Placeholders before final assets
One creature before multiple creatures
One corridor before multiple environments
One scene before multiple scenes
iOS prototype before broader platform support
Manual testing before public testing
Small commits before large refactors
```

The agent must not skip ahead.

---

## 3. Platform Strategy

Initial platform:

```txt
iOS mobile portrait
```

Primary test device:

```txt
iPhone 16 Pro
```

Secondary platform:

```txt
Android
```

Future platform:

```txt
VR/XR
```

v0.1 is not a VR prototype.  
v0.1 is not an Android-first prototype.  
v0.1 is an iOS-first mobile portrait prototype.

---

## 4. Prototype v0.1 Target

Prototype v0.1 must include:

```txt
one Unity scene
one fixed elevator camera
one elevator placeholder
one corridor placeholder
one creature placeholder
3 to 5 floors
5 to 10 questions
timer
answer buttons
threat distance
stress
wrong answer feedback
timeout feedback
death
victory
restart
basic result screen
basic logic tests
```

Prototype v0.1 must not include:

```txt
VR
ads
shop
monetization
cloud save
online leaderboard
multiple creatures
multiple environments
final art
complex story
procedural generation
free movement
inventory
cinematics
```

---

## 5. Milestone Overview

```txt
Phase 0 — Project foundation
Phase 1 — Core gameplay loop
Phase 2 — Question system
Phase 3 — Threat and creature distance
Phase 4 — Placeholder scene assembly
Phase 5 — UI and run flow
Phase 6 — Horror feedback
Phase 7 — Prototype floor content
Phase 8 — iOS build preparation
Phase 9 — Playtest pass
Phase 10 — v0.1 cleanup and commit checkpoint
```

---

## 6. Phase 0 — Project Foundation

## Goal

Create a clean Unity project and repo structure.

## Scope

Included:

```txt
Unity 6 project
URP setup
folder structure
Game.unity scene
AGENTS.md
Docs/
Skills/
.gitignore
README.md
```

Excluded:

```txt
gameplay
creature logic
question system
final art
audio
mobile build
```

## Deliverables

```txt
UnityProject/
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Scripts/
UnityProject/Assets/Tests/
AGENTS.md
Docs/
Skills/
README.md
.gitignore
```

## Acceptance Criteria

```txt
Project opens in Unity
Game.unity exists
No blocking console errors
Folder structure matches docs
Generated Unity folders are ignored
No heavy assets imported
```

## Recommended Commit

```bash
git add .gitignore README.md AGENTS.md Docs Skills UnityProject/Assets UnityProject/Packages UnityProject/ProjectSettings
git commit -m "🧹 chore(project): initialize Unity horror prototype"
```

Important: use targeted paths. Do not use `git add .`.

---

## 7. Phase 1 — Core Gameplay Loop

## Goal

Implement the first logic-only playable loop.

## Scope

Included:

```txt
GameState enum
GameManager
RunController
ThreatManager
temporary question flow
temporary UI hooks if needed
death condition
victory condition
restart state
EditMode tests for core logic
```

Excluded:

```txt
final scene art
creature model
audio polish
animations
mobile build
complex question data
```

## Deliverables

Suggested files:

```txt
UnityProject/Assets/Scripts/Core/GameState.cs
UnityProject/Assets/Scripts/Core/GameManager.cs
UnityProject/Assets/Scripts/GameLoop/RunController.cs
UnityProject/Assets/Scripts/GameLoop/RunResult.cs
UnityProject/Assets/Scripts/Threat/ThreatManager.cs
UnityProject/Assets/Scripts/Threat/ThreatState.cs
UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs
UnityProject/Assets/Tests/EditMode/RunControllerTests.cs
```

## Acceptance Criteria

```txt
Run can start
Threat distance initializes
Correct answer can update distance
Wrong answer can update distance
Timeout can update distance
Death triggers when distance reaches 0
Victory can trigger after final floor
Restart resets run state
EditMode tests exist for threat rules
```

## Recommended Commit

```bash
git add UnityProject/Assets/Scripts/Core UnityProject/Assets/Scripts/GameLoop UnityProject/Assets/Scripts/Threat UnityProject/Assets/Tests/EditMode
git commit -m "🎮 feat(gameplay): add core threat run loop"
```

---

## 8. Phase 2 — Question System

## Goal

Create a data-driven question system.

## Scope

Included:

```txt
QuestionData ScriptableObject
QuestionType enum
AnswerSpeed enum
AnswerResult model
QuestionManager
answer evaluation
answer speed classification
timeout handling
EditMode tests
```

Excluded:

```txt
large question bank
complex riddles
audio clues implementation
visual clue system
sang-froid special mechanics
```

## Deliverables

Suggested files:

```txt
UnityProject/Assets/Scripts/Questions/QuestionData.cs
UnityProject/Assets/Scripts/Questions/QuestionType.cs
UnityProject/Assets/Scripts/Questions/AnswerSpeed.cs
UnityProject/Assets/Scripts/Questions/AnswerResult.cs
UnityProject/Assets/Scripts/Questions/QuestionManager.cs
UnityProject/Assets/Tests/EditMode/QuestionManagerTests.cs
UnityProject/Assets/Tests/EditMode/QuestionEvaluatorTests.cs
```

## Acceptance Criteria

```txt
QuestionData can define prompt, answers, correct answer and timer
QuestionManager can start a question
Player answer can be evaluated
Answer speed can be classified as Fast, Normal or Slow
Timeout can be detected
AnswerResult is produced
Tests cover correct answer, wrong answer, timeout and speed classification
```

## Recommended Commit

```bash
git add UnityProject/Assets/Scripts/Questions UnityProject/Assets/Tests/EditMode
git commit -m "🎮 feat(questions): add data-driven question system"
```

---

## 9. Phase 3 — Threat and Creature Distance

## Goal

Connect gameplay threat state to visible creature distance.

## Scope

Included:

```txt
CreaturePhase enum
CreatureController
distance-to-phase mapping
position anchors
placeholder movement
attack trigger
wrong-answer closer feedback
timeout closer feedback
```

Excluded:

```txt
final creature model
complex animation
enemy AI
pathfinding
multiple creatures
```

## Deliverables

Suggested files:

```txt
UnityProject/Assets/Scripts/Creature/CreaturePhase.cs
UnityProject/Assets/Scripts/Creature/CreatureController.cs
UnityProject/Assets/Scripts/Creature/CreatureData.cs
UnityProject/Assets/Prefabs/Creature/PlaceholderCreature.prefab
```

## Acceptance Criteria

```txt
Creature can be placed in scene
Creature reacts to distance
Creature has readable phases
Wrong answer visibly brings creature closer
Timeout visibly brings creature closer more strongly
Death triggers attack phase
No real AI required
```

## Recommended Commit

```bash
git add UnityProject/Assets/Scripts/Creature UnityProject/Assets/Prefabs/Creature UnityProject/Assets/Scenes/Game.unity
git commit -m "👻 feat(creature): add distance-based hallway threat"
```

---

## 10. Phase 4 — Placeholder Scene Assembly

## Goal

Build the first playable elevator/corridor scene.

## Scope

Included:

```txt
Game.unity scene hierarchy
fixed camera inside elevator
placeholder elevator
placeholder corridor
creature anchors
basic lighting
basic UI canvas placeholder
```

Excluded:

```txt
final art
imported asset packs
complex lighting
complex animation
multiple scenes
```

## Deliverables

```txt
UnityProject/Assets/Scenes/Game.unity
UnityProject/Assets/Prefabs/Elevator/
UnityProject/Assets/Prefabs/Corridor/
UnityProject/Assets/Prefabs/Creature/
UnityProject/Assets/Art/Materials/
```

## Acceptance Criteria

```txt
Camera is inside elevator
Corridor is visible
Creature can be seen approaching
Scene hierarchy is clean
UI does not block threat
Portrait framing is considered
No blocking console errors
```

## Recommended Commit

```bash
git add UnityProject/Assets/Scenes/Game.unity UnityProject/Assets/Prefabs UnityProject/Assets/Art/Materials
git commit -m "🛗 feat(scene): add elevator corridor prototype"
```

---

## 11. Phase 5 — UI and Run Flow

## Goal

Make the prototype playable through UI.

## Scope

Included:

```txt
start panel
question panel
answer buttons
timer view
feedback overlay
result panel
restart button
basic mobile portrait layout
```

Excluded:

```txt
final UI art
shop UI
settings menu
leaderboard
account UI
```

## Deliverables

Suggested files:

```txt
UnityProject/Assets/Scripts/UI/QuestionView.cs
UnityProject/Assets/Scripts/UI/TimerView.cs
UnityProject/Assets/Scripts/UI/FeedbackOverlayView.cs
UnityProject/Assets/Scripts/UI/ResultView.cs
UnityProject/Assets/Scripts/UI/GameplayUIView.cs
UnityProject/Assets/Prefabs/UI/
```

## Acceptance Criteria

```txt
Player can start a run
Question appears
Answer buttons work
Timer is visible
Wrong answer feedback appears
Timeout feedback appears
Result screen appears
Restart works
UI is readable in portrait
```

## Recommended Commit

```bash
git add UnityProject/Assets/Scripts/UI UnityProject/Assets/Prefabs/UI UnityProject/Assets/Scenes/Game.unity
git commit -m "📱 feat(ui): add mobile portrait gameplay flow"
```

---

## 12. Phase 6 — Horror Feedback

## Goal

Add the first horror feedback layer.

## Scope

Included:

```txt
wrong answer red glitch
brief blackout
timeout stronger blackout
basic light flicker
basic camera shake if simple
basic audio placeholders
creature closer feedback
attack feedback
```

Excluded:

```txt
advanced post-processing
heavy shaders
final audio mix
complex animation
gore effects
```

## Deliverables

Suggested files:

```txt
UnityProject/Assets/Scripts/UI/FeedbackOverlayView.cs
UnityProject/Assets/Scripts/Audio/AudioDirector.cs
UnityProject/Assets/Scripts/Elevator/ElevatorController.cs
UnityProject/Assets/Scripts/Creature/CreatureController.cs
UnityProject/Assets/Audio/
```

## Acceptance Criteria

```txt
Wrong answer feels like a horror event
Timeout feels worse than wrong answer
Fast correct answer creates relief
Death feedback is clear
Feedback does not break gameplay flow
No blocking console errors
```

## Recommended Commit

```bash
git add UnityProject/Assets/Scripts/UI UnityProject/Assets/Scripts/Audio UnityProject/Assets/Scripts/Elevator UnityProject/Assets/Scripts/Creature UnityProject/Assets/Audio UnityProject/Assets/Scenes/Game.unity
git commit -m "🔊 feat(horror): add answer consequence feedback"
```

---

## 13. Phase 7 — Prototype Floor Content

## Goal

Add the first 3 to 5 floors and 5 to 10 questions.

## Scope

Included:

```txt
FloorData assets
QuestionData assets
3-floor minimum flow
5-floor preferred flow
basic difficulty progression
result stats
```

Excluded:

```txt
large question bank
randomized content
daily challenge
infinite mode
full story
```

## Recommended Floor Structure

Preferred:

```txt
Floor 1: observation
Floor 2: short memory
Floor 3: environmental instruction
Floor 4: audio or simple logic
Floor 5: sang-froid or anomaly
```

Minimum:

```txt
Floor 1: observation
Floor 2: memory
Floor 3: pressure test
```

## Acceptance Criteria

```txt
Prototype has at least 3 playable floors
Preferred version has 5 playable floors
Each floor has a challenge
Timers get tighter or pressure increases
Player can win by completing final floor
Player can die before final floor
Result screen shows outcome
```

## Recommended Commit

```bash
git add UnityProject/Assets/ScriptableObjects UnityProject/Assets/Scenes/Game.unity
git commit -m "🎮 feat(content): add first playable floor sequence"
```

---

## 14. Phase 8 — iOS Build Preparation

## Goal

Prepare the prototype for local iOS testing.

## Scope

Included:

```txt
iOS build target
portrait orientation
safe area check
touch input check
development build settings
Xcode export readiness
```

Excluded:

```txt
TestFlight distribution
App Store setup
monetization SDKs
analytics SDKs
iOS polish pass
```

## Acceptance Criteria

```txt
Unity can switch to iOS target
Portrait orientation is configured
Scene remains readable in mobile aspect
Touch UI works
Build can be exported to Xcode if environment allows
No generated build output is committed
```

## Recommended Commit

```bash
git add UnityProject/ProjectSettings UnityProject/Packages
git commit -m "📱 chore(ios): configure prototype build target"
```

---

## 15. Phase 9 — Playtest Pass

## Goal

Test the prototype and identify what must improve.

## Scope

Included:

```txt
manual playtest checklist
difficulty notes
UI readability notes
wrong-answer feedback notes
timeout feedback notes
creature pressure notes
restart desire notes
bug list
```

Excluded:

```txt
new features
large refactors
art overhaul
public release
```

## Playtest Questions

```txt
Did you understand what to do?
Did you notice the creature getting closer?
Did wrong answers feel dangerous?
Did timeout feel worse than wrong answer?
Did fast correct answers feel relieving?
Was the UI readable?
Were the questions fair?
Did you want to restart?
Did it feel like horror or just quiz?
Was anything confusing?
Was anything too punishing?
Was anything too slow?
```

## Deliverables

```txt
Docs/PLAYTEST_NOTES.md
Docs/DECISIONS.md updated if needed
bug list
priority list
```

## Recommended Commit

```bash
git add Docs/PLAYTEST_NOTES.md Docs/DECISIONS.md
git commit -m "📝 docs(playtest): record first prototype feedback"
```

---

## 16. Phase 10 — v0.1 Cleanup

## Goal

Stabilize the prototype after the first playtest loop.

## Scope

Included:

```txt
fix blocking bugs
adjust difficulty
clean obvious UI issues
clean scene hierarchy
update docs
run tests
prepare v0.1 checkpoint
```

Excluded:

```txt
new major systems
new environments
new creatures
monetization
VR
```

## Acceptance Criteria

```txt
Prototype can be played from start to result
Death works
Victory works
Restart works
No blocking console errors
Core tests pass
Docs match current behavior
Git status clean after commit
```

## Recommended Commit

```bash
git add Docs UnityProject/Assets UnityProject/ProjectSettings UnityProject/Packages
git commit -m "✅ chore(prototype): stabilize v0.1 first fear loop"
```

---

## 17. Post-v0.1 Options

After v0.1, choose one path based on test results.

## Option A — Improve fear

Use if the loop works but is not scary enough.

Possible work:

```txt
better sound
better lighting
better creature silhouette
stronger wrong-answer feedback
better timeout feedback
```

## Option B — Improve gameplay clarity

Use if players are confused.

Possible work:

```txt
clearer tutorial
better UI
better feedback
better distance readability
simpler first questions
```

## Option C — Improve content

Use if players want more.

Possible work:

```txt
more questions
more floor variants
more anomalies
simple randomization
result scoring
```

## Option D — Prepare external iOS testing

Use if v0.1 is already compelling.

Possible work:

```txt
TestFlight preparation
Apple Developer setup
basic analytics
crash checks
better build pipeline
```

Do not choose all options at once.

---

## 17B. Phase 7B Series — Floor Progression and Descent (history)

Phase 7 (floor content) was followed by an iterative 7B series that reshaped floor
progression. This history is kept so the agent does not re-introduce superseded mechanics.

```txt
Phase 7B   — Floor progression / inter-floor transitions.
Phase 7B.1 — Correct-only floor clear (clear a floor by answering correctly).
             Superseded by the multi-trial flow.
Phase 7B.2 — Multi-trial floors (5 trials per floor instead of one question per floor).
Phase 7B.3 — Door Seal scoring experiment: correct trials built a "Door Seal" score and a
             floor was cleared only if the score passed a threshold; threat was made
             non-receding. COMPLETED EXPERIMENT, then intentionally SUPERSEDED — the
             score/Door Seal mechanic was removed from active gameplay.
Phase 7B.4 — Descent loop + intro + localization prep (CURRENT gameplay):
             * run starts at Floor 5 and descends 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor;
             * each floor has 5 trials, cleared by SURVIVING all 5 (no score, no Door Seal);
             * threat is non-receding during a floor and resets per floor (deeper = closer);
             * correct = trial consumed, threat unchanged; wrong/timeout move threat closer;
             * loss = SHE GOT IN; escape after Floor 1 = GROUND FLOOR / YOU ESCAPED;
             * narrative intro before the run; lightweight EN/FR localization for UI/status/intro
               (question content still English-only).
             Commit: 9cb1bc7 "🎮 feat(gameplay): add descent loop and intro localization".
             Tests: 148/148 EditMode passing.
Phase 7C   — Documentation alignment: align all docs with the Phase 7B.4 descent loop and
             mark Door Seal / ascending / score-based-clear / one-question-per-floor as
             obsolete.
Phase 7D   — Corridor Observation & Evidence-Based Trials DESIGN (documentation/design only):
             define how future trials become evidence-based corridor observation puzzles
             (observe -> remember -> return -> answer). Added Docs/CORRIDOR_OBSERVATION_DESIGN.md.
             No camera/visual/gameplay code implemented.
Phase 7E   — Evidence Trial Data Model (DATA_MODEL_ONLY): implemented the pure, testable
             evidence data types (CorridorClue, CorridorClueType, EvidenceAnswerOption,
             EvidenceTrial, FloorObservationSet), an EvidenceTrialValidator and a 25-trial
             PrototypeEvidenceFloorSet (EN/FR). Runtime trial flow still uses PrototypeFloorSet;
             no camera/visual/scene changes. EditMode tests: 179/179 passing.
Phase 7F   — Question Content Localization EN/FR: localized the LIVE playable trial content
             (the 25 PrototypeFloorSet trials) — prompts, answers and cues — via optional
             French fields on QuestionData/QuestionCue resolved by PrototypeLocalization.Language
             (Option A). English stays the default; gameplay (index-based correct answer,
             floor/trial counts, threat tuning) is unchanged. EditMode tests: 189/189 passing.
             (This phase.)
```

Important: Phase 7B.3 (Door Seal) is a **completed experiment that was intentionally
superseded**, not a current mechanic. Door Seal scoring must not be documented or
re-implemented as active gameplay.

---

## 17C. Recommended Next Phases (after 7F)

Phase 7E implemented the evidence-trial data model (data only) and Phase 7F localized the
live playable trial content EN/FR (see `Docs/CORRIDOR_OBSERVATION_DESIGN.md`). The
recommended sequence to continue:

```txt
Phase 7G — Static Corridor Clue Prototype
           (show one floor's FloorObservationSet clues statically in the corridor; wire
            EvidenceTrials to clueIds in the scene)
Phase 7H — Observation Camera Pass Prototype
           (ObservationPhaseController: slow forward/backward camera travel + handoff to trials)
Phase 7I — Evidence-Based Floor Playtest
           (drive a floor from PrototypeEvidenceFloorSet end to end; playtest observe ->
            remember -> answer)
```

Optional follow-up (not blocking):

```txt
Phase 7F.1 — Language settings UI / persistent language choice (currently code/test-driven only).
```

Later, still planned (not superseded):

```txt
Phase 8 — Mobile Build Readiness (iOS portrait build target, safe area, touch)
Phase 9 — Visual / Horror Scene Polish (lighting, creature silhouette, audio atmosphere)
```

Note: the evidence data model became Phase 7E and question-content localization Phase 7F;
the remaining observation phases shifted accordingly (7G/7H/7I).

Do not jump straight to final art or monetization.

---

## 18. Current Roadmap Status

Current status:

```txt
Phases 0–7 implemented; 7B series complete through Phase 7B.4 (descent loop).
Latest gameplay commit: 9cb1bc7 — Phase 7B.4 descent loop and intro localization.
Tests: 148/148 EditMode passing.
Door Seal / score-based floor clear: removed from active gameplay (Phase 7B.4).
Documentation aligned to the descent loop in Phase 7C.
Corridor observation / evidence-based trials direction designed in Phase 7D
  (design only; see Docs/CORRIDOR_OBSERVATION_DESIGN.md). No camera/visual code yet.
Evidence trial data model implemented in Phase 7E (data only; runtime still uses
  PrototypeFloorSet).
Live playable trial content localized EN/FR in Phase 7F (prompts/answers/cues), English default.
  EditMode tests: 189/189 passing.
No iOS build yet.
```

Next planned step:

```txt
Phase 7G — Static Corridor Clue Prototype (recommended)
```
