# Game Design — Don’t Let Her In

## 1. Design Summary

**Don’t Let Her In** is a mobile first-person horror survival quiz prototype.

The player is trapped in an elevator. At each floor, the doors open onto a creepy corridor. A female entity approaches while the player answers short survival challenges.

The main design promise is:

> Every second of hesitation brings her closer.

The prototype must not feel like a school quiz with a horror skin. It must feel like a survival ritual where the elevator, corridor, sound, light and creature are all part of the threat.

---

## 2. Core Gameplay Loop

The core gameplay loop is:

```txt
Floor starts
Elevator doors open
Question or challenge begins
Timer starts
Creature advances
Player answers or times out
Answer is evaluated
Threat distance changes
Horror feedback plays
Next floor starts or player dies
```

The loop must be readable, fast and repeatable.

The player should understand within seconds:

```txt
Answer quickly and correctly or she gets closer.
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

Prototype answer effects:

```txt
Correct fast: +18 distance, stress -1
Correct normal: +10 distance
Correct slow: +3 distance
Wrong answer: -20 distance, stress +1
Timeout: -30 distance, stress +2
Death: distance <= 0
```

These values are initial balancing values. They can be adjusted after playtesting.

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

## 13. Victory Design

Victory occurs when the player completes the last floor of the prototype.

Victory sequence:

```txt
Final answer resolved
Creature fails to reach elevator
Doors close
Elevator display stabilizes
Result screen appears
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

Preferred v0.1 structure:

```txt
Floor 1: Observation
Floor 2: Short memory
Floor 3: Environmental instruction
Floor 4: Audio or simple logic
Floor 5: Sang-froid or anomaly
```

Fallback minimum version:

```txt
Floor 1: Observation
Floor 2: Short memory
Floor 3: Wrong-answer pressure test
```

The fallback is acceptable if it proves the loop.

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
