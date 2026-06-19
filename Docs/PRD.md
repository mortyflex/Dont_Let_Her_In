# PRD — Don’t Let Her In

## 1. Product Summary

**Project name:** Don’t Let Her In  
**Genre:** Mobile portrait horror elevator trial game  
**Engine:** Unity 6  
**Rendering:** URP  
**Language:** C#  
**Initial platform:** iOS mobile portrait  
**Future platforms:** Android, then possible VR/XR  
**Current milestone:** Prototype v0.1 — First Fear Loop (descent)

Don’t Let Her In is a mobile horror prototype where the player wakes up high inside a sinister building, trapped in an elevator with the doors open. At each floor, a hallway threat (a female entity) approaches. The core objective is to **descend floor by floor and reach the Ground Floor to escape**.

Each floor holds **5 trials** (short survival challenges). The threat **never recedes during a floor**: a correct answer consumes the trial and lets the player continue, but does not push the threat back; a wrong answer or a timeout consumes the trial and moves the threat closer (timeout strongest). Surviving all 5 trials of a floor closes the doors and descends to the next floor down. If the threat reaches the elevator, the player loses (**SHE GOT IN**). Surviving Floor 1 reaches the **Ground Floor — YOU ESCAPED**.

Main promise:

> Every second of hesitation brings her closer.

---

## 2. Product Goal

The goal of the first prototype is not to build the full game.

The goal is to prove that the core descent loop creates tension:

```txt
Floor starts (threat reset to this floor's start distance)
Trial starts (1 of 5)
Timer starts
Player answers
Answer is evaluated (correct / wrong / timeout)
Trial is consumed; wrong/timeout move the threat closer (threat never recedes)
Repeat until all 5 trials survived -> doors close -> descend one floor
Reach Ground Floor -> escape, or threat reaches elevator -> caught
```

The prototype must answer one key question:

> Is it stressful and fun to answer short trials while seeing a creature approach the elevator during a descent?

If the answer is yes, the project can move toward better art, stronger sound design, more floors and a polished mobile demo.

### Objective and structure

```txt
Core objective: reach the Ground Floor and escape.
Prototype v0.1 starts at Floor 5 and descends: 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor.
Each floor has 5 trials (25 trials total in the prototype).
A floor is cleared by surviving all 5 trials (no score requirement).
Full game target may start higher later, such as Floor 15.
English is the default language; French is planned from the beginning.
```

---

## 3. Target Player

The first target is a mobile player who likes short horror experiences.

The game should appeal to players who enjoy:

- horror games
- short mobile sessions
- quick decision-making
- observation challenges
- memory challenges
- pressure-based gameplay
- replayable runs
- creepy atmosphere
- TikTok/Reels-friendly moments

The prototype should be understandable within 10 seconds.

---

## 4. Platform Strategy

### Initial platform

Android mobile portrait.

### Later platforms

- iOS
- Steam possible later
- VR/XR possible later

### Prototype orientation

Portrait.

### Input model

Touch-based only.

No keyboard, no mouse precision, no controller requirement.

---

## 5. Core Experience

The player should feel:

- trapped
- watched
- pressured
- uncertain
- punished for hesitation
- relieved by fast correct answers
- scared when the creature gets closer

The game should not feel like a normal quiz with horror visuals.

It should feel like a survival ritual where the elevator, corridor, lights, sounds and creature are part of the question.

---

## 6. Gameplay Pillars

## 6.1 Pressure over complexity

The challenges should be simple enough to understand quickly.

The difficulty comes from:

- limited time
- the creature approaching
- stress
- misleading visual cues
- audio pressure
- fear of making a mistake

The player should think:

> I know the answer, but she is getting closer.

## 6.2 Creature as timer

The creature is the real timer.

A UI timer can exist, but the visual pressure should come from her movement through the corridor.

## 6.3 Clear consequences

Every trial must have a clear consequence. The threat never recedes during a floor (Phase 7B.4); progress comes from surviving trials, not from pushing the creature back.

Correct answer (fast, normal or slow):

```txt
Trial is consumed
Player continues to the next trial
Threat does NOT move back
Relief comes from surviving, not from gaining distance
```

Wrong answer:

```txt
Trial is consumed
UI glitches
Creature moves closer
Stress increases
```

Timeout:

```txt
Trial is consumed
Lights fail
Creature advances strongly (worse than a wrong answer)
Stress increases more
```

Caught (loss):

```txt
Threat distance reaches 0
Creature reaches elevator
Run ends -> SHE GOT IN
Result screen appears
```

## 6.4 Short sessions

The first prototype should last 3 to 5 minutes.

The player should be able to restart quickly.

## 6.5 Mobile-first readability

The game must be readable on a phone screen.

Answer buttons must be large, fast to tap and not block the main threat.

---

## 7. Prototype Scope

## 7.1 Included in Prototype v0.1

The prototype must include:

- one Unity scene
- one elevator
- one corridor
- one placeholder creature
- fixed first-person camera
- mobile portrait UI
- 3 to 5 floors
- 5 to 10 questions
- timer
- threat distance system
- correct answer handling
- wrong answer handling
- timeout handling
- basic death
- basic victory
- restart
- basic result screen
- basic horror feedback
- basic sound placeholders if possible
- basic tests for core logic

## 7.2 Excluded from Prototype v0.1

The prototype must not include:

- final-quality art
- real/final audio mix
- jumpscare cinematics
- pathfinding / advanced enemy AI
- monetization (ads, shop, IAP)
- online / cloud (account, cloud save, leaderboard)
- VR/XR
- the full campaign (target may start at Floor 15 later; prototype uses 5 floors)
- full question-content localization (UI/status/intro are EN/FR; questions remain English-only for now)
- multiple monsters
- complex story
- procedural generation
- inventory
- free movement
- joystick controls
- a score-based or Door Seal floor-clear mechanic (removed in Phase 7B.4)

---

## 8. Player Flow

## 8.1 First run flow

```txt
Player opens game
Narrative intro screen appears (wake up on Floor 5, BEGIN DESCENT)
Player taps Begin Descent
Elevator scene starts on Floor 5
Trial 1 of 5 appears, timer starts, creature is at this floor's start distance
Player answers (correct / wrong / timeout); the trial is consumed
Wrong/timeout move the threat closer; correct does not move it back
Repeat trials 2..5
Survive all 5 trials -> DOORS CLOSING -> DESCENDING -> next floor down
Reach Ground Floor or get caught
Result screen appears
Player can restart
```

## 8.2 Run win flow (escape)

```txt
Player survives all 5 trials of Floor 1
Elevator doors close
Elevator reaches the Ground Floor
Result screen appears: GROUND FLOOR — YOU ESCAPED
Run marked as survived
Restart button appears
```

## 8.3 Run loss flow (caught)

```txt
Threat distance reaches 0 (after a wrong answer or timeout)
Current trial is cancelled
Creature reaches the elevator
Result screen appears: SHE GOT IN
Run marked as lost
Restart button appears
```

---

## 9. Core Mechanics

## 9.1 Threat distance

The creature distance is represented by a value from 0 to 100.

```txt
100 = creature far away
0 = creature reaches the elevator
```

Distance interpretation:

```txt
100: invisible or very far
80: silhouette at end of corridor
60: clearly visible
40: mid corridor
25: near elevator doors
10: at the doors
0: death
```

## 9.2 Trial outcomes (Phase 7B.4 — non-receding threat)

Each trial result consumes the trial. The threat never recedes during a floor, so correct answers do not change the distance. Confirmed prototype values (`ThreatManager`):

```txt
Correct (fast / normal / slow): trial consumed, no distance change, no stress change
Wrong answer: -20 distance, stress +1
Timeout: -30 distance, stress +2
Caught: distance <= 0 -> SHE GOT IN
```

At the start of each floor the threat is reset to that floor's starting distance (deeper = closer):

```txt
Floor 5 start distance: 85
Floor 4 start distance: 80
Floor 3 start distance: 75
Floor 2 start distance: 70
Floor 1 start distance: 65
```

Note: the older "+distance for correct answers" model (and the Door Seal scoring experiment) is no longer the active design. See `Docs/GAME_DESIGN.md` and `Docs/DECISIONS.md`.

## 9.3 Stress

Stress is a secondary system.

Stress should not kill directly in the prototype.

Stress can affect:

- light instability
- UI glitch intensity
- sound pressure
- next question mood
- perceived danger

Prototype stress effects can be minimal at first.

## 9.4 Timer

Each question has a time limit.

Prototype suggested time limits:

```txt
Floor 1: 8 seconds
Floor 2: 7 seconds
Floor 3: 6 seconds
Floor 4: 5 seconds
Floor 5: 4 seconds
```

Exact values can be adjusted during playtesting.

---

## 10. Challenge Types

The prototype can use these challenge types:

## 10.1 Observation

The player sees something in the corridor and must identify it.

Example:

```txt
Question: Which room number blinked?
Answers: 101 / 104 / 140 / 401
Correct: 104
```

## 10.2 Short memory

The player sees symbols for a short duration and must remember one.

Example:

```txt
Question: Which symbol was in the center?
Answers: Eye / Key / Hand / Door
Correct: Key
```

## 10.3 Environmental instruction

The environment gives a short message or rule.

Example:

```txt
Wall message: DO NOT LOOK LEFT
Question: What did the wall say?
Answers: Do not run / Do not look left / Do not answer / Do not lie
Correct: Do not look left
```

## 10.4 Simple audio clue

The player hears a short code or word.

Example:

```txt
Intercom: Two. Seven. Two.
Question: What code did you hear?
Answers: 272 / 227 / 722 / 277
Correct: 272
```

## 10.5 Sang-froid

The player must resist panic.

Example:

```txt
Elevator screen: PRESS EXIT NOW
Wall message: WAIT
Correct behavior: wait before pressing
```

This type can be simplified for v0.1 if implementation is too costly.

---

## 11. Level Structure

## 11.1 Prototype target

The prototype contains 5 floors, descended from the top: Floor 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor. Each floor has 5 trials (25 trials total).

Current v0.1 structure (each floor groups one challenge theme across its 5 trials):

```txt
Floor 5 (descent start): observation
Floor 4: short memory
Floor 3: environmental instruction
Floor 2: audio / codes / logic
Floor 1 (last before escape): panic / sang-froid
```

Note: internally the floors are authored Floor 1..5 by theme, but the player descends from the highest displayed floor (5) down to Floor 1, then reaches the Ground Floor.

Playable prototype:

```txt
5 floors
25 trials (5 per floor)
one creature
descent to the Ground Floor (escape)
loss when the threat reaches the elevator (caught)
restart
basic horror feedback
narrative intro
EN/FR localization prep for UI/status/intro
```

---

## 12. Creature

## 12.1 Prototype creature

The prototype uses one creature only.

Working name:

```txt
The Hallway Woman
```

French reference name:

```txt
La Dame du Couloir
```

## 12.2 Creature behavior

The creature does not need real AI in v0.1.

Its behavior is driven by threat distance.

The creature should have visual phases:

```txt
Far
Visible
MidCorridor
NearDoor
AtDoor
Attack
```

## 12.3 Creature design direction

The creature should be:

- feminine
- slow
- unsettling
- partially hidden
- readable as a silhouette
- recognizable from far away
- more frightening when closer
- not fully shown too early

---

## 13. Visual Direction

The first prototype can use placeholders.

The target art direction is:

- dark elevator
- haunted corridor
- dirty hotel or hospital atmosphere
- sick green/yellow/red lighting
- claustrophobic composition
- minimal UI
- unsettling female silhouette
- strong contrast between elevator safety and corridor danger

The concept art is a mood target, not a v0.1 requirement.

---

## 14. Audio Direction

Audio is critical.

Prototype sound placeholders should include:

- elevator hum
- elevator door sound
- flickering light
- wrong answer hit
- timeout hit
- creature step or scrape
- creature close sound
- attack sound
- subtle ambience loop

Audio should become more intense when the creature is closer.

Do not rely only on loud jumpscares.

Use:

- silence
- low frequencies
- footsteps
- breathing
- grating metal
- distant whispers

---

## 15. UI Requirements

## 15.1 Main gameplay UI

The prototype UI should include:

- question text
- answer buttons
- timer
- feedback overlay
- result panel
- restart button

## 15.2 Mobile requirements

The UI must be:

- portrait-first
- readable
- thumb-friendly
- fast to use
- not too small
- not dependent on keyboard
- not dependent on mouse precision

## 15.3 Visual style

The UI can be simple at first.

It should avoid looking like a school quiz.

Preferred direction:

- dark panel
- subtle glitch effects
- red feedback for wrong answer
- low-saturation colors
- minimal typography
- no colorful casual-game style

---

## 16. Technical Requirements

## 16.1 Engine

Unity 6.

## 16.2 Rendering

URP.

## 16.3 Language

C#.

## 16.4 Architecture

Use modular systems:

- `GameManager`
- `RunController`
- `QuestionManager`
- `ThreatManager`
- `CreatureController`
- `ElevatorController`
- `FloorDirector`
- `AudioDirector`

Use ScriptableObjects for data where possible:

- `QuestionData`
- `FloorData`
- `CreatureData`
- `DifficultyData`
- `AudioCueData`
- `HorrorEventData`

## 16.5 Testing

Use Unity Test Framework.

Prioritize EditMode tests for:

- `ThreatManager`
- answer evaluation
- run progression
- death condition
- victory condition

---

## 17. Success Criteria

The prototype is successful if:

- the player understands what to do quickly
- the creature creates pressure
- wrong answers visibly make things worse
- timeout feels dangerous
- fast correct answers create relief
- death is clear
- restart is fast
- the loop is playable for 3 to 5 minutes
- the project remains easy to iterate
- the core logic has basic tests

The prototype fails if:

- it feels like a generic quiz
- the creature does not matter
- the player does not understand why they died
- the UI is unreadable on mobile
- the scope becomes too large
- the agent starts building unrelated systems

---

## 18. Definition of Done — Prototype v0.1

Prototype v0.1 is done when:

- `Game.unity` can be opened
- player can start a run
- first question appears
- timer starts
- creature distance changes over time
- correct answer changes distance positively
- wrong answer changes distance negatively
- timeout changes distance negatively more strongly
- creature visual state reacts to distance
- player can die
- player can win
- result screen appears
- restart works
- there are no blocking console errors
- basic logic tests exist
- project docs are up to date

---

## 19. Current Decision Log

Current decisions:

```txt
Engine: Unity 6
Rendering: URP
Platform: iOS first
Orientation: portrait
Camera: fixed inside elevator
Movement: no free movement in prototype
Creature count: one
Prototype length: 3 to 5 minutes
Prototype floors: 3 to 5
Prototype questions: 5 to 10
Graphics target: placeholder first, horror polish later
VR: not in v0.1
Monetization: not in v0.1
```

Future decisions to revisit:

```txt
Exact final name
Final creature name
Full art asset strategy
Android-only or Android+iOS for first external test
Whether to add daily challenge
Whether to add mode infinite
Whether to add VR after mobile validation
```

---

## 20. Immediate Next Development Step

After documentation setup, the first implementation phase is:

```txt
Phase 1 — Core gameplay loop without final art
```

Phase 1 should implement:

- `GameState`
- `ThreatManager`
- `QuestionData`
- `QuestionManager`
- `RunController`
- temporary UI
- death condition
- victory condition
- restart
- basic EditMode tests
