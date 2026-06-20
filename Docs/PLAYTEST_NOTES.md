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
Playable descent loop committed (Phase 7B.4): Floor 5 -> Ground Floor, 5 trials per floor.
148/148 EditMode tests passing.
Door Seal / score-based floor clear removed from active gameplay.
Narrative intro + EN/FR UI/status/intro localization present (question content still EN).
No structured human playtest recorded yet — use the checklist below for the next session.
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

## 3B. Phase 7B.4 / 7C Descent Loop Checklist

Use this checklist for the next playtest of the descent loop. Items are unchecked — no
human playtest results are recorded yet.

```txt
[ ] Intro screen is readable in portrait (title + wake-up/descent context fit, not cut off)
[ ] BEGIN DESCENT button is clear and starts the run
[ ] Run starts on Floor 5 (HUD shows the top floor first)
[ ] Descent Floor 5 -> Floor 4 is clear (FLOOR CLEARED -> DOORS CLOSING -> DESCENDING reads as going down)
[ ] No Door Seal / score gate is shown anywhere (floors clear by surviving 5 trials)
[ ] The 5-trials-per-floor rhythm is readable (TRIAL 1/5 .. 5/5)
[ ] Threat feels non-receding within a floor (correct answers do not push her back)
[ ] Wrong answer adds pressure (she moves closer; feedback is visible)
[ ] Timeout feels worse than a wrong answer (she moves closer more strongly)
[ ] Floor clear timing feels right (short transition, not too long/short)
[ ] Reaching Floor 1 -> Ground Floor escape is clear (GROUND FLOOR — YOU ESCAPED)
[ ] Getting caught is clear (SHE GOT IN) and restart works
[ ] French UI smoke check: switch language to French, intro/status/transition/result read correctly
    (ÉTAGE / ÉPREUVE / DESCENTE / REZ-DE-CHAUSSÉE / ELLE EST ENTRÉE / TU ES SORTI)
[ ] No blocking Console errors during a full run (start -> descend -> escape or caught -> restart)
```

Note: question / answer / cue text is still English-only even when the UI is set to French
(expected until Phase 7E).

---

## 3C. Future Checklist — Corridor Observation (planned)

> **Planned, not yet implemented (Phase 7D design).** Use this once the evidence-based
> observation layer exists (see `Docs/CORRIDOR_OBSERVATION_DESIGN.md`). Items are unchecked;
> no observation playtest has been performed.

```txt
[ ] Does the observation phase feel too long or too short?
[ ] Can the player remember 5 clues from one observation pass?
[ ] Are the clues readable in mobile portrait (size, contrast)?
[ ] Do the trials feel fair (answer deducible from what was seen)?
[ ] Do answers feel connected to the corridor (not a random quiz)?
[ ] Are the distractors plausible (similar numbers/symbols/instructions), not arbitrary?
[ ] Does the forward/backward camera travel create tension rather than frustration?
[ ] Is the corridor recognizably the same across floors, with only details changed?
[ ] Are anomalies noticeable against the learned baseline?
[ ] Does the handoff observation -> trials feel clean (no dead time / confusion)?
[ ] French smoke check: clue labels/descriptions, prompts and answers read correctly in FR.
```

---

## 3D. Phase 7G Checklist — Static Corridor Clue Board

Phase 7G adds a code-built "OBSERVED CLUES" board to the runtime HUD (evidence bridge, not
final art; the runtime trials still come from `PrototypeFloorSet`). Items unchecked; run in
Game view portrait (e.g. 1080x1920).

```txt
[ ] Game.unity opens; Play Mode starts with no red Console errors.
[ ] Intro appears; BEGIN DESCENT starts the run.
[ ] On Floor 5 an "OBSERVED CLUES" board is visible with 5 clue lines.
[ ] The board does NOT cover the answer buttons, the timer or the question text.
[ ] The corridor / creature remain readable behind the translucent board.
[ ] Floor 5 clues relate to the Floor 5 questions (e.g. ROOM DISPLAY: 104).
[ ] Answering works; wrong/timeout still move the threat closer.
[ ] After surviving Floor 5, on descent the board updates to Floor 4's clues.
[ ] Same per-floor update down to Floor 1.
[ ] English default shows English clues (OBSERVED CLUES).
[ ] Setting PrototypeLocalization.Language = GameLanguage.French shows French clues
    (INDICES OBSERVÉS, NUMÉRO DE PORTE, ...).
[ ] Restart after win/loss re-shows the top floor's clues.
```

Note: clue evidence values are theme-aligned to the playable floor (~22/25 exact value
matches); a few clue lines may not be the literal answer yet. Report any clue that clearly
contradicts its floor's questions.

---

## 3E. Phase 7H Checklist — Observation Camera Pass

Phase 7H adds a short observation pass once per floor, before its trials (prototype pacing, not
final cinematic polish). Items unchecked; run in Game view portrait (e.g. 1080x1920).

```txt
[ ] Game.unity opens; Play Mode starts with no red Console errors.
[ ] Intro appears; BEGIN DESCENT starts the run.
[ ] On Floor 5, the "OBSERVED CLUES" board appears AND an OBSERVE THE CORRIDOR overlay shows
    before the first question.
[ ] During observation: the question/answers are hidden and not clickable.
[ ] During observation: the timer does NOT count down and the threat (DIST/STRESS) does NOT move.
[ ] The camera subtly eases toward the corridor and settles back (or, with no Main Camera, the
    overlay-only fallback shows for the same duration).
[ ] After ~2-3s the overlay disappears and the first question appears; answers become tappable.
[ ] Wrong answer still advances the threat; timeout advances it more strongly.
[ ] Surviving Floor 5 triggers FLOOR CLEARED / DOORS CLOSING / DESCENDING.
[ ] Floor 4 starts with a NEW observation pass and the clue board updates to Floor 4.
[ ] No observation pass appears between trials of the same floor, after answers, or on win/loss.
[ ] Restart after win/loss starts an observation pass again on the top floor.
[ ] English default shows OBSERVE THE CORRIDOR / "Look carefully. The answers are already here."
[ ] PrototypeLocalization.Language = GameLanguage.French shows OBSERVE LE COULOIR /
    "Regarde bien. Les réponses sont déjà là." and the rest of the FR UI stays correct.
```

Note: the camera move is intentionally subtle. Report if it makes the corridor unreadable, if the
overlay covers the clue board, or if any observation appears where it should not (between trials,
after an answer, on win/loss).

---

## 3F. Phase 7H.1 Checklist — Observation Pass Tuning

Phase 7H.1 tunes the pass (slower/farther camera) and makes the clue board observation-only.
Items unchecked; run in Game view portrait (e.g. 1080x1920).

```txt
[ ] On Floor 5 the camera move is clearly SLOWER and reaches FARTHER toward the corridor/red
    light than in Phase 7H (move ~1.2s, hold ~2.5s, return ~0.7s, ~4.4s total).
[ ] The camera returns to the normal gameplay pose BEFORE the first question appears (not stuck forward).
[ ] During observation the "OBSERVED CLUES" board is visible together with the OBSERVE overlay.
[ ] As soon as the first question starts, the clue board DISAPPEARS (no clues during questions).
[ ] No clue board is visible during any of the 5 Floor 5 questions (answer from memory).
[ ] After surviving Floor 5, Floor 4 starts: clue board updates to Floor 4 and is visible during
    its observation, then hides again when the first Floor 4 question starts.
[ ] Same observation-only clue behavior down to Floor 1.
[ ] Restart repeats the same behavior on the top floor (board visible during observation, hidden
    during questions).
[ ] Camera move does not rotate wildly, does not break portrait readability, and (ideally) does
    not clip through walls.
[ ] French: OBSERVE LE COULOIR + INDICES OBSERVÉS visible only during observation; FR
    prompts/answers/cues still correct during questions.
[ ] No red Console errors.
```

Note: report if the camera now moves too far/clips visibly, if it feels too slow, or if the clue
board ever stays visible during a question (it should not in 7H.1).

---

## 3G. Phase 7H.1 correction Checklist — Slow Observation Travel + Creature Hidden

Phase 7H.1 slow-travel correction (only timing/distance and creature visibility change).
Items unchecked; run in Game view portrait (e.g. 1080x1920).

```txt
[ ] On Floor 5 the camera performs a clear, slow TRAVELLING: ~8s forward toward the corridor/
    red light, a brief pause, then ~8s back (~16.5s total). It should read as a real travel, not a nudge.
[ ] The camera reaches FARTHER, stopping just before the red light past the last doors (forward ~7m,
    height ~0.18m), without clipping into the red light or through a wall.
[ ] The camera returns to the normal gameplay pose BEFORE the first question appears (not stuck forward).
[ ] During the whole travel: question/answers hidden, timer not counting, threat (DIST/STRESS) frozen.
[ ] The creature is NEVER visible during the observation travel (hidden the whole time).
[ ] The creature only appears during the answer phase (per threat state), after the travel ends.
[ ] The "OBSERVED CLUES" board is visible during the travel, then hidden when the first question starts.
[ ] After descent, Floor 4 repeats the same slow travel with its own clues; same down to Floor 1.
[ ] Restart repeats the same slow travel on the top floor.
[ ] Camera does not rotate wildly and (ideally) does not clip through walls at ~7m forward.
[ ] No red Console errors.
```

Note: ~16.5s total is intentional. Report if it feels too long, if 7m forward clips into the red
light/geometry, if the camera ever fails to return before the question, or if the creature is
visible at any point during the observation.

---

## 3H. Phase 7I Checklist — Elevator Descent Transition

Phase 7I adds a prototype elevator descent transition between floors (UI doors, not final art).
Items unchecked; run in Game view portrait (e.g. 1080x1920).

```txt
[ ] BEGIN DESCENT; the elevator cabin frame is visible: dark side panels, a floor plate showing
    the current floor on the left, and a button column (5..1, G) on the right with the current
    floor highlighted. The central corridor stays visible between them.
[ ] The cabin frame stays visible during observation, questions AND the descent transition.
[ ] The floor plate / highlighted button update to the next floor on each descent (5 -> 4 -> ...).
[ ] Floor 5 observation works as validated (clue board only during observation).
[ ] Answer the 5 Floor 5 trials so the floor is cleared.
[ ] On floor clear the question/answers disappear and the clue board is hidden.
[ ] FLOOR CLEARED shows, then two dark elevator doors CLOSE — SLOWLY — over the central corridor
    opening only (NOT the whole screen); the side cabin (where buttons/walls would be) stays visible.
[ ] DESCENDING shows with a subtle vertical descent cue (text shake), doors closed, for ~3s.
[ ] The creature is NOT visible at any point during the transition.
[ ] The floor indicator updates to the next floor while the doors are closed.
[ ] The doors OPEN again (DOORS OPENING), SLOWLY.
[ ] Floor 4 observation pass starts ONLY after the doors have opened.
[ ] Floor 4 clue board appears during observation, then hides when the first question starts.
[ ] No clue board and no observation appear before the doors open.
[ ] The transition feels heavier/slower (~6.5-7s) but is still shorter than the observation pass.
[ ] Repeat down the floors; completing Floor 1 ESCAPES (ground floor result) with NO new
    descent transition afterwards.
[ ] Restart works and the doors never start a run already closed.
[ ] French: ÉTAGE FRANCHI / PORTES EN FERMETURE / DESCENTE / PORTES EN OUVERTURE / ÉTAGE 4.
[ ] No red Console errors during play.
[ ] STOPPING Play Mode logs NO "GameObjects can not be made active when they are being destroyed".
```

Note: doors are prototype UI panels (no models) covering only the central aperture (~68% width).
Report if the doors get stuck, if they still feel too fast or cover too much of the screen, if the
creature or clue board appears during the transition, if observation starts before the doors open,
or if a descent transition runs after the final Floor 1 escape.

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
