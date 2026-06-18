# Skill — Horror Game Design

## Name

horror-game-design

## Purpose

Use this skill when designing or modifying the horror experience of **Don’t Let Her In**.

This includes:

- riddles
- micro-challenges
- wrong-answer consequences
- timeout consequences
- creature behavior
- horror pacing
- floor events
- anxiety curve
- visual feedback
- sound feedback
- anomaly design
- player pressure

## Project Context

The game is a mobile horror prototype set inside an elevator.

The player sees a haunted corridor through the open elevator doors. A creepy female entity approaches while the player answers short survival questions.

The game must not feel like a normal quiz with horror visuals.

The ideal feeling is:

> I know what to do, but she is getting closer and I am panicking.

## Design Pillars

### 1. Pressure over complexity

The player should not lose because the puzzle is too intellectual.  
The player should lose because pressure, hesitation and fear caused mistakes.

### 2. The creature is the timer

The timer should be visible through the creature’s approach.

UI timers are allowed, but the real pressure comes from seeing her move closer.

### 3. The environment is part of the question

Questions should come from:

- corridor details
- elevator screen
- symbols
- sounds
- flickering lights
- wall messages
- mirror/reflection
- door numbers
- intercom voices

Avoid generic trivia.

### 4. Wrong answers are horror events

A wrong answer must not simply show “wrong”.

It should cause:

- red UI glitch
- blackout
- aggressive sound cue
- creature jump closer
- increased stress
- short loss of elevator safety

### 5. Timeouts are worse than wrong answers

Timeout should feel like hesitation gave the creature permission to approach.

Timeout must be mechanically and visually stronger than a wrong answer.

### 6. Fear before jumpscare

The game should use jumpscares carefully.

Tension tools:

- silence
- footsteps
- breathing
- flickering light
- distance changes
- corridor depth
- ambiguous movement
- distorted UI
- false safety

## Prototype Challenge Types

Use these in the prototype:

### Observation

The player sees something and must identify it quickly.

Examples:

- “Which room number blinked?”
- “Which symbol was on the left?”
- “Which door was open?”

### Short Memory

The player sees a short sequence or visual set, then recalls it.

Examples:

- three symbols shown for two seconds
- door number shown briefly
- elevator display flashes a floor

### Simple Audio Clue

The player hears a word, number or short code.

Examples:

- intercom whispers “two seven two”
- footsteps stop at a door number
- a voice says “do not press red”

### Environmental Instruction

The wall, mirror or elevator gives a rule.

Examples:

- “The elevator lies.”
- “Do not look left.”
- “Wait until the light dies.”

### Simple Logic

Short pattern or code.

Examples:

- 2, 4, 8, ?
- Which button is missing?
- Which floor comes after the repeated pattern?

### Sang-froid Instruction

The challenge is easy, but the player must resist panic.

Examples:

- wait before pressing
- ignore a flashing button
- avoid tapping the obvious answer
- do nothing for two seconds

### Anomaly

Something changes between floors.

Examples:

- a painting moves
- one door disappears
- a symbol is inverted
- the mirror shows a different answer

## Avoid in Prototype

Do not use:

- long riddles
- lore-heavy puzzles
- obscure trivia
- large paragraphs
- inventory puzzles
- free movement puzzles
- moral choices needing story context
- unfair random answers
- puzzles that cannot be solved under pressure
- puzzles with ambiguous answer validation

## Wrong Answer Design

A wrong answer should have a short sequence lasting about 0.5 to 1.5 seconds.

Recommended sequence:

```txt
Player taps wrong answer
Selected answer flashes red
UI glitches
Lights cut out briefly
Harsh sound cue
Lights return
Creature is visibly closer
Stress increases
Next floor/question resumes
```

Wrong answer mechanical effect:

```txt
Distance -20
Stress +1
Combo reset
```

Possible wrong answer messages:

```txt
Wrong.
She heard you.
Too late.
You lied.
No.
Again.
```

Use sparingly. Do not overload the screen.

## Timeout Design

Timeout should be more severe.

Recommended sequence:

```txt
Timer reaches zero
Question disappears
Elevator light fails
Creature sound jumps forward
Doors jam or twitch
Creature appears much closer
Stress increases strongly
Next question starts under worse pressure
```

Timeout mechanical effect:

```txt
Distance -30
Stress +2
Combo reset
```

## Correct Answer Design

### Correct fast

Should create relief, but not total safety.

Visual/audio:

- light stabilizes
- creature recedes
- elevator door starts closing
- subtle positive sound
- short breath of relief

Mechanical effect:

```txt
Distance +18
Stress -1
Combo +1
```

### Correct normal

Should help, but maintain tension.

Mechanical effect:

```txt
Distance +10
Combo +1
```

### Correct slow

Should barely help.

Mechanical effect:

```txt
Distance +3
Combo may reset
```

Visual:

- creature stops
- door struggles to close
- no real comfort

## Creature Design Rules

For the prototype, use one creature only.

Name placeholder:

```txt
The Hallway Woman
```

French direction name:

```txt
La Dame du Couloir
```

She should be:

- visible from far away as a silhouette
- feminine but not overdesigned
- slow but inevitable
- more frightening when partially hidden
- recognizable by movement and sound
- not fully revealed too early

Prototype phases:

```txt
Far
Visible
MidCorridor
NearDoor
AtDoor
Attack
```

Behavior:

- no real AI needed
- position is driven by threat distance
- reacts to wrong answers
- reacts more strongly to timeout
- attack triggers when distance reaches 0

## Floor Progression

Prototype target:

```txt
3 to 5 floors
5 to 10 questions
3 to 5 minutes
one creature
one corridor
one elevator
```

Recommended 5-floor structure:

```txt
Floor 1: simple observation
Floor 2: short memory
Floor 3: environmental message
Floor 4: audio or logic
Floor 5: panic/sang-froid challenge
```

## Difficulty Curve

Increase difficulty through:

- shorter timers
- closer starting distance
- faster creature movement
- more visual interference
- more misleading UI
- more pressure sounds

Do not increase difficulty by making text longer.

## Horror Feedback Checklist

When designing a challenge, verify:

- Can the player understand it in under two seconds?
- Is the answer clear?
- Does the creature create pressure during the challenge?
- Is the consequence of failure visible?
- Does the sound reinforce the consequence?
- Is the challenge fair on mobile?
- Is the UI readable?
- Does it avoid generic trivia?

## Prototype Riddle Examples

### Observation

Prompt:

```txt
Which room number blinked?
```

Answers:

```txt
101
104
140
401
```

Correct answer:

```txt
104
```

### Memory

Prompt:

```txt
Which symbol was in the center?
```

Answers:

```txt
Eye
Key
Hand
Door
```

Correct answer:

```txt
Key
```

### Environmental instruction

Wall message:

```txt
DO NOT LOOK LEFT
```

Prompt:

```txt
What did the wall say?
```

Answers:

```txt
Do not run
Do not look left
Do not answer
Do not lie
```

Correct answer:

```txt
Do not look left
```

### Audio

Intercom says:

```txt
Two. Seven. Two.
```

Prompt:

```txt
What code did you hear?
```

Answers:

```txt
272
227
722
277
```

Correct answer:

```txt
272
```

### Sang-froid

Elevator screen says:

```txt
PRESS EXIT NOW
```

Wall message says:

```txt
WAIT
```

Correct behavior:

```txt
Wait before pressing.
```

## Delivery Requirements

At the end of any horror design task, report:

- what changed
- why it improves tension
- what player behavior it creates
- whether it is fair
- whether it is prototype-safe
- what should be playtested
