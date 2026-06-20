# Game Design — Don’t Let Her In

> **Authoritative design note (Phase 7B.4):** Sections 0 and 0.x below describe the
> current, committed game loop and override any older wording later in this document.
> The game is a **descent**: the player starts high and descends to the Ground Floor.
> The threat **never recedes during a floor**. There is **no score / Door Seal**
> floor-clear mechanic. Older sections that describe correct answers pushing the
> creature back, one-question-per-floor, ascending progression, or score-based clearing
> are kept for history and explicitly marked as superseded.

## 0. Current Official Loop (Phase 7B.4)

### 0.1 Core Fantasy

The player wakes up high inside a sinister building, trapped in an open elevator. A female hallway threat (the creature) waits down the corridor and comes closer with every mistake. The player cannot run or move — they can only answer the trials and try to descend to the Ground Floor and escape.

```txt
You are trapped high in the building. You cannot run.
You can only answer, survive the floor, and descend.
Every second of hesitation brings her closer.
```

### 0.2 Run Structure

```txt
Narrative intro (wake up on Floor 5, BEGIN DESCENT)
-> Floor 5 (5 trials)
-> Floor 4 (5 trials)
-> Floor 3 (5 trials)
-> Floor 2 (5 trials)
-> Floor 1 (5 trials)
-> Ground Floor (YOU ESCAPED)
```

A floor is a fresh danger cycle: when it starts, the threat is reset to that floor's starting distance and stress is cleared (the closed doors blocked the previous floor's threat).

### 0.3 Descent Progression

```txt
The displayed floor number counts DOWN: 5 -> 4 -> 3 -> 2 -> 1 -> Ground Floor.
The deeper the descent, the closer the threat starts (less safety).
After clearing a non-final floor a prototype elevator descent transition plays (Phase 7I):
  FLOOR CLEARED -> doors close -> DESCENDING (subtle vertical descent cue, floor indicator updates)
  -> doors open -> the next floor's observation pass starts ONLY after the doors open.
  The creature and the clue board stay hidden for the whole transition; it is shorter (~3.8s) than
  the observation pass. The transition is a UI prototype (dark sliding panels), not final art.
After clearing Floor 1: GROUND FLOOR -> YOU ESCAPED (no descent transition after the final escape).
```

### 0.4 Floor Structure

```txt
Each floor has exactly 5 trials.
Prototype total: 5 floors x 5 trials = 25 trials.
A trial is one short challenge with a cue, a prompt, answers and a timer.
A floor is cleared by SURVIVING all 5 trials (no score required).
```

### 0.5 Trial Rules

Every trial result consumes the current trial (it is never re-asked):

```txt
Correct answer: trial consumed, player continues, threat does NOT move back.
Wrong answer:   trial consumed, threat moves closer.
Timeout:        trial consumed, threat moves closer strongly (worse than wrong).
```

### 0.6 Threat Rules (non-receding)

The threat distance runs 0..100 (0 = caught). Confirmed prototype values (`ThreatManager`):

```txt
Correct (fast / normal / slow): no distance change, no stress change.
Wrong answer: -20 distance, stress +1.
Timeout:      -30 distance, stress +2.
Caught:       distance <= 0.
```

The threat **never recedes during a floor**. It only resets between floors, to the floor's starting distance (deeper = closer):

```txt
Floor 5 start distance: 85
Floor 4 start distance: 80
Floor 3 start distance: 75
Floor 2 start distance: 70
Floor 1 start distance: 65
```

### 0.7 Win / Loss Conditions

```txt
Win  (escape): survive all 5 trials of Floor 1 -> reach the Ground Floor -> YOU ESCAPED.
Loss (caught): threat distance reaches 0 at any point -> SHE GOT IN.
```

### 0.8 Intro Narrative

A short narrative intro is shown before the run (localized). It establishes the situation and the goal, then offers BEGIN DESCENT:

```txt
You wake up on the 5th floor.
The elevator is open. The hallway should be empty.
It is not.
Answer the trials. Do not let her in. Reach the ground floor.
```

### 0.9 Localization Direction

Lightweight, code-based localization (no Unity Localization package, no asset pipeline):

```txt
English is the default language.
French is available for key UI / status / intro / transition / result strings.
The language can be switched in code/tests; there is no settings UI yet.
Question / answer / cue content remains English-only for now.
```

French visible equivalents used in the UI:

```txt
ÉTAGE (FLOOR)
ÉPREUVE (TRIAL)
REZ-DE-CHAUSSÉE (GROUND FLOOR)
DESCENTE / DESCENDING
TU ES SORTI (YOU ESCAPED)
ELLE EST ENTRÉE (SHE GOT IN)
```

### 0.10 Prototype Scope

```txt
5 floors, 5 trials each (25 trials), one creature, one corridor, one fixed camera.
Placeholder art/audio. Mobile portrait. Narrative intro. EN/FR UI prep.
Question content is prototype-quality, code-authored, English-only.
```

### 0.11 Future Expansion

```txt
Full game may start higher (such as Floor 15) for a longer descent.
Future work: question-content localization EN/FR, mobile build readiness, visual/horror polish.
Keep the loop replaceable: trials, floors and threat tuning are data/config, not hardcoded rules.
```

### 0.12 Corridor Observation and Evidence-Based Trials (planned next layer)

> **Planned, not yet implemented (Phase 7D design).** The descent loop already has 5 trials
> per floor. The next gameplay layer makes those trials **evidence-based corridor
> observation puzzles**. Full design lives in `Docs/CORRIDOR_OBSERVATION_DESIGN.md`.

Intended evolution:

```txt
From: the player answers abstract questions in the elevator.
To:   the player observes the hallway, memorizes details, returns to the elevator,
      then answers trials based on what was actually visible.
```

Planned per-floor flow (wraps the current trial flow; threat/descent rules unchanged):

```txt
doors open
observation camera travels forward into the hallway  (PLANNED — not implemented)
floor-specific clues are exposed (door numbers, symbols, lights, messages, objects, anomalies)
observation camera travels backward to the elevator   (PLANNED)
trial sequence begins; each trial asks about a visible clue
correct = trial consumed, threat does NOT recede; wrong/timeout = threat closer
survive all 5 trials -> descend
```

Core principle:

```txt
No trial without a corridor clue. No correct answer without observable evidence.
Distractors must be plausible, never random. The corridor stays structurally consistent
across floors while the details (clues/anomalies) change per floor.
```

Status: the current 25 prototype trials are the technical base; future trial content should
be grounded in visible corridor clues. The camera travel is **not** implemented yet.

---

## 1. Design Summary

**Don’t Let Her In** is a mobile portrait horror elevator trial prototype.

The player wakes up high in a sinister building, trapped in an open elevator. At each floor, the corridor holds a female hallway threat that comes closer with every mistake. The player answers short trials to survive each floor and descend toward the Ground Floor.

The main design promise is:

> Every second of hesitation brings her closer.

The prototype must not feel like a school quiz with a horror skin. It must feel like a survival ritual where the elevator, corridor, sound, light and creature are all part of the threat.

---

## 2. Core Gameplay Loop

See **Section 0** for the authoritative loop. In summary:

```txt
Floor starts (threat reset to this floor's start distance)
Trial begins (1 of 5)
Timer starts
Player answers or times out -> trial is consumed
Wrong/timeout move the threat closer; correct does not move it back
Repeat until all 5 trials survived -> doors close -> descend one floor
Reach Ground Floor (escape) or threat reaches elevator (caught)
```

The loop must be readable, fast and repeatable.

The player should understand within seconds:

```txt
Survive each floor's trials and descend, or she gets in.
```

---

## 3. Game Feel Target

The player should feel:

- trapped
- watched
- pressured
- uncertain
- punished for hesitation
- relieved by fast correct answers
- scared when the creature gets closer
- tempted to panic-tap
- motivated to restart after death

The game should create pressure through speed and fear, not through complicated puzzle logic.

---

## 4. Prototype Scope

Prototype v0.1 is called:

```txt
First Fear Loop
```

Target content:

```txt
one elevator
one corridor
one creature
one fixed camera
3 to 5 floors
5 to 10 questions
3 to 5 minutes
basic death
basic victory
restart
basic horror feedback
```

The prototype should be playable even with placeholder art.

The first goal is not beauty.  
The first goal is tension.

---

## 5. Threat Distance System

The creature is controlled by a threat distance value.

```txt
Distance range: 0 to 100
100 = creature far away
0 = creature reaches elevator and player dies
```

Distance interpretation:

```txt
100: creature invisible or very far
80: silhouette at the end of the corridor
60: visible creature
40: mid corridor
25: near elevator doors
10: at the doors
0: death
```

The creature does not need real AI in the prototype.

The creature position should be driven by `ThreatManager`.

---

## 6. Answer Outcome Rules

> **Superseded by Section 0.6 (Phase 7B.4).** Correct answers no longer add distance —
> the threat is non-receding during a floor. The values below are kept only as history of
> the original receding-threat model. The `ThreatManager` constants still exist (and
> `ApplyCorrectFast/Normal/Slow` are still defined and unit-tested), but the descent flow
> uses `RecordCorrectSealed` (no distance change) for correct answers.

Original (historical) answer effects:

```txt
Correct fast: +18 distance, stress -1
Correct normal: +10 distance
Correct slow: +3 distance
Wrong answer: -20 distance, stress +1
Timeout: -30 distance, stress +2
Death: distance <= 0
```

Current active rules: see Section 0.6.

---

## 7. Answer Speed Classification

Each question has a time limit.

The answer speed should be classified based on response time.

Suggested classification:

```txt
Fast: answered in first 35% of timer
Normal: answered between 35% and 70% of timer
Slow: answered after 70% of timer
Timeout: no answer before timer reaches 0
```

Example with a 6-second timer:

```txt
0.0s to 2.1s: Fast
2.1s to 4.2s: Normal
4.2s to 6.0s: Slow
6.0s+: Timeout
```

---

## 8. Stress System

Stress is a secondary system.

Stress does not directly kill the player in prototype v0.1.

Stress affects presentation:

- stronger UI glitches
- more light flicker
- more aggressive audio
- stronger creature presence
- less sense of elevator safety

Prototype stress range:

```txt
0 to 4
```

Stress interpretation:

```txt
0: stable
1: mild instability
2: visible pressure
3: panic state
4: near-collapse state
```

Stress changes:

```txt
Wrong answer: +1
Timeout: +2
Correct fast: -1
Correct normal: no change
Correct slow: no change
```

Stress should be clamped between 0 and 4.

---

## 9. Wrong Answer Design

A wrong answer must not simply show “wrong”.

It should be a short horror event lasting about 0.5 to 1.5 seconds.

Recommended sequence:

```txt
Player taps wrong answer
Selected answer flashes red
UI glitches
Lights cut out briefly
Harsh sound cue plays
Creature jumps closer
Lights return
Next state continues
```

Mechanical effect:

```txt
Distance -20
Stress +1
Combo reset
```

Visual feedback:

- red UI glitch
- text shake
- brief blackout
- creature closer after blackout
- elevator display corruption
- subtle camera shake

Audio feedback:

- harsh hit
- distorted metal sound
- creature step or scrape
- short silence after impact

Possible short messages:

```txt
Wrong.
She heard you.
No.
Again.
You lied.
Too late.
```

Use these messages sparingly.

---

## 10. Timeout Design

Timeout must feel worse than a wrong answer.

A timeout means the player hesitated too long and gave the creature time to approach.

Recommended sequence:

```txt
Timer reaches zero
Question disappears
Elevator light fails
Creature sound jumps forward
Doors twitch or jam
Creature appears much closer
Stress increases strongly
Next state continues or player dies
```

Mechanical effect:

```txt
Distance -30
Stress +2
Combo reset
```

Timeout should be more dangerous than answering wrong.

---

## 11. Correct Answer Design

> **Superseded by Section 0.5 / 0.6 (Phase 7B.4).** In the current design a correct answer
> consumes the trial and lets the player continue, but does NOT push the creature back and
> does not change stress. Relief comes from surviving the floor and descending, not from
> regained distance. The combo/relief wording below is historical.

## 11.1 Correct fast

A fast correct answer should create relief.

Mechanical effect:

```txt
Distance +18
Stress -1
Combo +1
```

Feedback:

- creature recedes
- elevator light stabilizes
- doors start to close
- subtle positive audio cue
- short feeling of control

Important: do not make the player feel completely safe. The creature can recede but should remain threatening if already close.

## 11.2 Correct normal

Mechanical effect:

```txt
Distance +10
Combo +1
```

Feedback:

- creature slows down or recedes slightly
- lights stabilize briefly
- pressure remains

## 11.3 Correct slow

Mechanical effect:

```txt
Distance +3
Combo may reset
```

Feedback:

- creature almost does not move back
- doors struggle
- no strong relief
- player remains under pressure

---

## 12. Death Design

Death occurs when:

```txt
distance <= 0
```

Death sequence:

```txt
Current question is cancelled
Timer stops
Lights fail or turn red
Creature reaches elevator
Door fails
Attack or jumpscare feedback plays
Run ends
Result screen appears
Restart is available
```

Death must be clear.

The player should understand why they died:

```txt
She reached the elevator because I was too slow or answered wrong.
```

---

## 13. Victory Design (escape)

Victory occurs when the player survives all 5 trials of Floor 1 and the elevator reaches the Ground Floor.

Victory sequence:

```txt
Final trial of Floor 1 survived
Doors close
Elevator descends to the Ground Floor
Result screen appears: GROUND FLOOR — YOU ESCAPED
Run marked as survived
Restart is available
```

Victory should feel like survival, not total safety.

The result screen can imply that this was only one short escape.

---

## 14. Challenge Types

The prototype should use short survival challenges.

Allowed types:

```txt
Observation
Short memory
Simple audio clue
Environmental instruction
Simple logic
Sang-froid instruction
Anomaly
```

Avoid:

```txt
Generic trivia
Long riddles
Complex lore puzzles
Inventory puzzles
Free-movement puzzles
Large text blocks
Ambiguous answers
Random unfair questions
```

---

## 15. Challenge Type — Observation

The player sees something in the corridor and must identify it.

Example:

```txt
Visual clue: Room number 104 blinks.
Question: Which room number blinked?
Answers: 101 / 104 / 140 / 401
Correct: 104
```

Escalation options:

- shorter visibility
- flickering light
- creature partially blocks clue
- similar answer choices
- clue appears in reflection

---

## 16. Challenge Type — Short Memory

The player sees a symbol, sequence or visual set briefly, then recalls it.

Example:

```txt
Visual clue: Eye / Key / Hand
Question: Which symbol was in the center?
Answers: Eye / Key / Hand / Door
Correct: Key
```

Escalation options:

- less display time
- more symbols
- similar-looking symbols
- visual distortion
- light cuts during clue

---

## 17. Challenge Type — Simple Audio Clue

The player hears a short clue.

Example:

```txt
Intercom voice: Two. Seven. Two.
Question: What code did you hear?
Answers: 272 / 227 / 722 / 277
Correct: 272
```

Escalation options:

- whispering
- static noise
- repeated distorted voice
- similar answer choices
- wrong voice tries to mislead

Audio clues must remain fair.  
The player should not need expensive headphones to solve the prototype.

---

## 18. Challenge Type — Environmental Instruction

The environment gives a short rule or message.

Example:

```txt
Wall message: DO NOT LOOK LEFT
Question: What did the wall say?
Answers: Do not run / Do not look left / Do not answer / Do not lie
Correct: Do not look left
```

Escalation options:

- message appears briefly
- message is partially damaged
- elevator display contradicts the wall
- creature movement distracts from message

---

## 19. Challenge Type — Simple Logic

Short pattern or code.

Example:

```txt
Prompt: 2 / 4 / 8 / ?
Answers: 10 / 12 / 16 / 18
Correct: 16
```

Rules:

- logic must be understandable quickly
- no math-heavy puzzles
- no long reasoning
- no obscure knowledge
- no trick without clue

---

## 20. Challenge Type — Sang-froid Instruction

The player must resist panic.

Example:

```txt
Elevator screen: PRESS EXIT NOW
Wall message: WAIT
Correct behavior: wait before pressing
```

This type is powerful but can be harder to implement.

For prototype v0.1, keep it simple:

- press only after delay
- ignore a flashing wrong button
- tap a non-obvious answer indicated by the wall
- do not tap for two seconds

The mechanic must be clearly introduced before punishing the player harshly.

---

## 21. Challenge Type — Anomaly

Something changes or contradicts expectations.

Example:

```txt
Previous floor: three paintings
Current floor: four paintings
Question: What changed?
Answers: Door / Painting / Light / Carpet
Correct: Painting
```

For v0.1, anomalies should be simple and visible.

Do not require the player to memorize complex layouts.

---

## 22. Prototype Floor Structure

> **Updated for Phase 7B.4.** Each floor now has **5 trials** (not one question), and the
> player **descends** from Floor 5 to the Ground Floor. The themes below describe the
> challenge flavour grouped per floor; the floors are authored Floor 1..5 by theme but
> displayed in descending order (5 first).

Current v0.1 structure (per displayed floor, descent order, 5 trials each):

```txt
Floor 5 (descent start): Observation
Floor 4: Short memory
Floor 3: Environmental instruction
Floor 2: Audio / codes / logic
Floor 1 (last before escape): Sang-froid / panic
```

The 25 prototype trials are code-authored (`PrototypeFloorSet`), English-only for now.

---

## 23. Suggested Prototype Questions

## 23.1 Floor 1 — Observation

```txt
Visual clue: Room 104 blinks.
Question: Which room number blinked?
Answers: 101 / 104 / 140 / 401
Correct: 104
Timer: 8 seconds
```

## 23.2 Floor 2 — Short memory

```txt
Visual clue: Eye / Key / Hand
Question: Which symbol was in the center?
Answers: Eye / Key / Hand / Door
Correct: Key
Timer: 7 seconds
```

## 23.3 Floor 3 — Environmental instruction

```txt
Wall message: DO NOT LOOK LEFT
Question: What did the wall say?
Answers: Do not run / Do not look left / Do not answer / Do not lie
Correct: Do not look left
Timer: 6 seconds
```

## 23.4 Floor 4 — Audio clue

```txt
Audio clue: Two. Seven. Two.
Question: What code did you hear?
Answers: 272 / 227 / 722 / 277
Correct: 272
Timer: 5 seconds
```

## 23.5 Floor 5 — Sang-froid

```txt
Elevator screen: PRESS EXIT NOW
Wall message: WAIT
Question: What should you do?
Answers: Press exit / Wait / Open doors / Look away
Correct: Wait
Timer: 4 seconds
```

If the wait mechanic is not implemented yet, replace Floor 5 with a simple anomaly question.

---

## 24. Creature Design

Prototype creature:

```txt
The Hallway Woman
La Dame du Couloir
```

The creature should be:

- a single entity
- mostly silent at first
- visible as a silhouette from far away
- increasingly readable as distance decreases
- not fully explained
- not over-animated in v0.1
- driven by distance, not AI

Visual phases:

```txt
Far
Visible
MidCorridor
NearDoor
AtDoor
Attack
```

Behavior by phase:

```txt
Far: barely visible or invisible
Visible: silhouette appears
MidCorridor: body shape readable
NearDoor: threatening presence
AtDoor: almost fatal
Attack: death sequence
```

---

## 25. Creature Feedback by Answer

> **Superseded by Section 0.5 / 0.6 (Phase 7B.4).** Correct answers no longer make the
> creature recede. The "recede" entries below are historical. In the current design the
> creature holds position on a correct answer and only advances on wrong/timeout.

Correct (fast / normal / slow), current behaviour:

```txt
Creature holds position (does not recede)
Trial is consumed; player continues
Relief comes from surviving and descending
```

Historical (receding-threat) feedback, kept for reference:

Correct fast:

```txt
Creature recedes
Sound pressure drops
Light stabilizes
```

Correct normal:

```txt
Creature slows or recedes slightly
Pressure remains
```

Correct slow:

```txt
Creature barely stops
Still close
```

Wrong answer:

```txt
Creature jumps closer after blackout
Sound hit
Position visibly worse
```

Timeout:

```txt
Creature advances more than wrong answer
Doors may jam
Audio becomes aggressive
```

Death:

```txt
Creature reaches elevator
Attack feedback
Run lost
```

---

## 26. Difficulty Progression

Difficulty should increase by:

- reducing timer duration
- increasing creature speed
- starting creature closer
- increasing stress effects
- adding misleading but fair cues
- increasing similarity between answers

Do not increase difficulty by:

- adding long text
- adding obscure knowledge
- making clues unreadable
- making answers ambiguous
- requiring perfect audio hardware

---

## 27. Scoring

> **Note (Phase 7B.4).** There is **no score-based floor clear** and **no Door Seal**
> mechanic in the active design — a floor is cleared by surviving its 5 trials. The
> Door Seal scoring experiment (Phase 7B.3) was intentionally removed (see
> `Docs/DECISIONS.md`). A result-screen score remains optional for a future phase and
> must never gate floor clearing or loss.

Scoring is optional for the earliest prototype, but recommended for result screen.

Suggested score inputs:

```txt
floors completed
correct answers
wrong answers
timeouts
average response time
final distance
max combo
survived or died
```

Simple score formula:

```txt
score = floorsCompleted * 100
      + correctAnswers * 50
      + maxCombo * 25
      + finalDistance
      - wrongAnswers * 50
      - timeouts * 75
```

Score must not block the prototype.

If scoring delays the playable loop, postpone it.

---

## 28. Result Screen

The result screen should show:

```txt
Survived or caught
Floors completed
Correct answers
Wrong answers
Timeouts
Average response time
Final score if implemented
Restart button
```

Tone should remain horror-themed.

Avoid cheerful arcade presentation.

---

## 29. Replayability

Prototype replayability is limited but should support fast restart.

Future replayability may come from:

- random question order
- more question banks
- more corridor variants
- daily challenge
- infinite mode
- secret rules
- alternate endings
- new creature variants

Do not implement these in v0.1.

---

## 30. Playtest Questions

After testing a build, ask:

```txt
Did you understand what to do?
Did you notice the creature getting closer?
Did wrong answers feel dangerous?
Did timeout feel worse than wrong answer?
Was the UI readable?
Were the questions fair?
Did you want to restart?
Did it feel like horror or just quiz?
Was anything confusing?
Was anything too slow?
Was anything too punishing?
```

---

## 31. Design Risks

## 31.1 Risk: It feels like a generic quiz

Solution:

- use environment-based challenges
- make creature movement central
- avoid trivia
- add horror feedback to outcomes

## 31.2 Risk: It is too hard

Solution:

- longer timers at first
- clear feedback
- fewer answer choices
- fair clues
- progressive difficulty

## 31.3 Risk: It is too easy

Solution:

- shorter timers later
- creature starts closer
- stronger timeout penalty
- more similar answer choices
- higher stress effects

## 31.4 Risk: Death feels unfair

Solution:

- make distance readable
- make wrong-answer consequence visible
- make timeout consequence visible
- allow recovery through fast correct answers

## 31.5 Risk: The agent overbuilds

Solution:

- keep scope v0.1 strict
- one creature
- one corridor
- no VR
- no monetization
- no procedural generation
- no final art requirement

---

## 32. Prototype Definition of Done

The gameplay design is implemented well enough when:

- the player can start a run
- each floor launches a short challenge
- the creature approaches during challenge time
- correct answers push the creature back
- wrong answers bring the creature closer
- timeouts are worse than wrong answers
- the player can die
- the player can win
- restart works
- the loop is understandable
- the loop creates pressure
- the UI is playable on mobile portrait
- basic logic tests exist
