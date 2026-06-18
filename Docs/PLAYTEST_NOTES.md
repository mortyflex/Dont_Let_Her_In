# Playtest Notes — Don’t Let Her In

## 1. Purpose

This document records playtest sessions, manual checks, feedback, bugs, difficulty notes and design decisions for **Don’t Let Her In**.

The goal is to keep a clear history of how the prototype feels during real testing.

The most important question is:

> Is it tense and fun to answer short questions while watching a creature approach?

---

## 2. Current Prototype Status

Current milestone:

```txt
Prototype v0.1 — First Fear Loop
```

Current status:

```txt
Documentation setup in progress
Unity project not yet implemented
No gameplay code yet
No playable build yet
No playtest performed yet
```

---

## 3. Playtest Session Template

Use this template after each test session.

```md
## YYYY-MM-DD — Playtest Session Title

### Build / Commit

- Build:
- Commit:
- Platform:
- Device:
- Tester:
- Duration:

### Test Goal

What was being tested?

### What Worked

-

### What Failed

-

### Bugs Found

-

### Gameplay Feel

Did the loop feel tense?

### Creature Pressure

Did the creature feel threatening?

### Wrong Answer Feedback

Did wrong answers feel dangerous?

### Timeout Feedback

Did timeout feel worse than a wrong answer?

### UI Readability

Was the UI readable on mobile portrait?

### Difficulty

Too easy / fair / too hard?

### Restart Desire

Did the tester want to replay?

### Notes

-

### Decisions After Test

-

### Next Actions

-
```

---

## 4. Manual Playtest Checklist

For every playable build, test:

```txt
Start run
First question appears
Timer starts
Creature advances
Tap correct answer quickly
Tap correct answer slowly
Tap wrong answer
Wait for timeout
Verify wrong answer brings creature closer
Verify timeout is worse than wrong answer
Verify correct fast pushes creature away
Trigger death
Reach victory if possible
Result screen appears
Restart works
Check console for errors
Check portrait layout
```

---

## 5. Core Questions for Testers

Ask these questions after a test:

```txt
Did you understand what to do?
Did you notice the creature getting closer?
Did wrong answers feel dangerous?
Did timeout feel worse than wrong answer?
Did fast correct answers feel relieving?
Was the UI readable?
Were the questions fair?
Did you want to restart?
Did it feel like horror or just a quiz?
Was anything confusing?
Was anything too slow?
Was anything too punishing?
Was anything too easy?
Was anything visually unclear?
```

---

## 6. Scoring the Playtest

Use a 1 to 5 rating.

```txt
1 = failed
2 = weak
3 = acceptable
4 = good
5 = strong
```

Rate:

```txt
Core loop clarity:
Creature pressure:
Wrong answer feedback:
Timeout feedback:
Mobile UI readability:
Fear/tension:
Fairness:
Restart desire:
Overall prototype promise:
```

---

## 7. Bug Priority Scale

Use this scale:

```txt
P0: blocks the prototype completely
P1: breaks core gameplay
P2: hurts gameplay but workaround exists
P3: polish issue
P4: note for later
```

Examples:

```txt
P0: scene does not open
P1: answer buttons do not work
P1: death never triggers
P1: restart does not work
P2: wrong answer feedback is unclear
P2: timer is hard to read
P3: light flicker feels weak
P3: placeholder creature not scary
P4: final UI should be more atmospheric
```

---

## 8. Current Known Risks

Current risks before first playable build:

```txt
The prototype may feel like a quiz instead of horror.
The creature may not feel threatening enough.
The UI may block the corridor on mobile portrait.
Timeout may not feel worse than wrong answer.
Wrong answer feedback may not be visually clear.
The first Unity scene may become overbuilt too early.
The agent may add systems outside v0.1 scope.
```

---

## 9. First Playable Test Target

The first playable test should validate:

```txt
Can the player start a run?
Does the first question appear?
Does the timer work?
Does the creature get closer over time?
Does a wrong answer bring her closer?
Does timeout bring her closer more strongly?
Can the player die?
Can the player restart?
```

This first test does not need final art.

---

## 10. First Playtest Entry

No playtest performed yet.

Use the template below when the first playable build exists.

```md
## YYYY-MM-DD — First playable loop test

### Build / Commit

- Build:
- Commit:
- Platform: iOS / Editor Play Mode
- Device:
- Tester:
- Duration:

### Test Goal

Validate the first playable loop.

### What Worked

-

### What Failed

-

### Bugs Found

-

### Gameplay Feel

-

### Creature Pressure

-

### Wrong Answer Feedback

-

### Timeout Feedback

-

### UI Readability

-

### Difficulty

-

### Restart Desire

-

### Notes

-

### Decisions After Test

-

### Next Actions

-
```

---

## 11. Update Rule

After each real test:

1. Add a dated entry.
2. Record build or commit if available.
3. Record what was tested.
4. Record bugs clearly.
5. Record subjective feel.
6. Record decisions.
7. Add action items.
8. Update `Docs/DECISIONS.md` if a major decision changes.
9. Update `Docs/ROADMAP.md` if priorities change.
