# Skill — Unity Gameplay Loop

## Name

unity-gameplay-loop

## Purpose

Use this skill when implementing or modifying the core gameplay loop of the Unity mobile horror game.

This includes:

- `GameManager`
- `RunController`
- `QuestionManager`
- `ThreatManager`
- timers
- answer evaluation
- creature distance
- win/loss states
- floor transitions
- run restart
- score calculation
- combo logic
- stress logic

## Project Context

The game is called **Don’t Let Her In**.

The player is trapped inside an elevator. At each floor, the doors open onto a creepy corridor. A female entity approaches while the player answers short survival questions.

The main promise is:

> Every second of hesitation brings her closer.

The prototype must prove this gameplay loop:

```txt
Question starts
Timer starts
Creature advances
Player answers
Answer is evaluated
Threat distance changes
Next floor or death
```

## Development Priorities

When working on gameplay loop systems:

1. Keep the loop simple.
2. Keep the logic testable.
3. Do not chase final graphics.
4. Do not add unnecessary features.
5. Do not hardcode question content inside gameplay logic.
6. Keep pure logic separate from Unity scene objects when practical.
7. Prefer data-driven configuration.
8. Avoid god classes.
9. Avoid hidden scene dependencies.
10. Keep the prototype easy to change.

## Core Gameplay Rules

Threat distance range:

```txt
0 to 100
100 = creature far away
0 = creature reaches elevator and player dies
```

Prototype answer effects:

```txt
Correct fast: +18 distance, stress -1
Correct normal: +10 distance
Correct slow: +3 distance
Wrong answer: -20 distance, stress +1
Timeout: -30 distance, stress +2
Death: distance <= 0
```

Distance interpretation:

```txt
100: creature invisible or very far
80: silhouette at the end of the corridor
60: visible creature
40: mid corridor
25: near the elevator doors
10: at the doors
0: death
```

## Required Runtime Concepts

The gameplay loop should support these runtime concepts:

- current run state
- current floor index
- current question
- remaining time
- answer speed
- answer correctness
- creature distance
- stress level
- combo count
- mistakes count
- timeout count
- win state
- loss state

## Recommended Core Classes

### `GameManager`

Responsible for global game state.

Should know:

- current state
- when a run starts
- when a run ends
- when to transition to results

Should not contain all gameplay logic.

### `RunController`

Responsible for the current run.

Should know:

- current floor
- floor count
- current score
- current run result
- how to progress to next floor
- how to trigger victory
- how to trigger defeat

### `QuestionManager`

Responsible for questions.

Should handle:

- loading current question
- starting question timer
- accepting player answer
- detecting timeout
- returning an `AnswerResult`

Should not hardcode the question bank.

### `ThreatManager`

Responsible for danger.

Should handle:

- distance
- stress
- answer effects
- timeout effects
- death detection
- distance clamping

This should be heavily tested.

### `FloorDirector`

Responsible for floor-level orchestration.

Should handle:

- which question appears
- initial distance
- floor mood
- optional horror event
- transition to next floor

### `CreatureController`

Responsible for visual response to threat state.

Should not decide gameplay logic.

It reads threat state and updates:

- position
- animation phase
- visibility
- sound intensity
- attack trigger

## Recommended Data Models

### `AnswerSpeed`

Suggested values:

```txt
Fast
Normal
Slow
Timeout
```

### `AnswerResult`

Suggested fields:

```txt
questionId
isCorrect
answerSpeed
selectedAnswerIndex
correctAnswerIndex
responseTimeSeconds
distanceDelta
stressDelta
isTimeout
```

### `ThreatState`

Suggested fields:

```txt
distance
stressLevel
isDead
lastDistanceDelta
lastStressDelta
```

### `RunResult`

Suggested fields:

```txt
won
lost
floorsCompleted
correctAnswers
wrongAnswers
timeouts
averageResponseTime
finalDistance
score
```

## Implementation Rules

Do:

- use small classes
- keep methods short
- make core rules readable
- test calculations
- clamp distance between 0 and 100
- clamp stress to a reasonable range
- separate UI from gameplay logic
- expose settings through data or serialized config

Do not:

- implement final art
- implement VR
- implement ads
- implement shop
- implement procedural generation
- implement multiple creatures
- put all logic into one MonoBehaviour
- rely on `FindObjectOfType` everywhere
- create hidden dependencies
- hardcode every question in C# gameplay scripts

## Minimum Acceptance Criteria

A gameplay-loop task is acceptable only if:

- a run can start
- a question can start
- a timer can run
- a correct answer updates threat state
- a wrong answer updates threat state
- a timeout updates threat state
- death is detected when distance reaches 0
- victory is possible after the final floor
- restart is possible or explicitly planned
- relevant logic has EditMode tests where possible
- no unrelated systems were modified

## Test Requirements

Add or update EditMode tests for:

- distance starts at expected value
- correct fast increases distance
- correct normal increases distance
- correct slow increases distance slightly
- wrong answer decreases distance
- timeout decreases distance more than wrong answer
- distance clamps at 0
- distance clamps at 100
- stress increases after wrong answer
- stress increases after timeout
- stress can decrease after fast correct answer
- death triggers at distance 0

## Manual Checks

For any playable loop change, manually verify in Play Mode if possible:

1. Start run.
2. Answer correctly fast.
3. Answer correctly slowly.
4. Answer wrong.
5. Wait for timeout.
6. Trigger death.
7. Trigger victory.
8. Restart.

## Delivery Requirements

At the end of the task, report:

- summary
- files changed
- tests added
- tests run
- test results
- manual checks
- known limits
- git status
- recommended targeted commit command

## Commit Examples

```bash
git add UnityProject/Assets/Scripts/Threat/ThreatManager.cs UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs
git commit -m "🎮 feat(gameplay): add threat distance system"
```

```bash
git add UnityProject/Assets/Scripts/Questions/QuestionManager.cs UnityProject/Assets/Scripts/Questions/QuestionData.cs
git commit -m "🎮 feat(gameplay): add data-driven question flow"
```
