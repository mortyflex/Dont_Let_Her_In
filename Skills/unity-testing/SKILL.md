# Skill — Unity Testing

## Name

unity-testing

## Purpose

Use this skill when adding, modifying or running tests for the Unity project.

This includes:

- EditMode tests
- PlayMode tests
- gameplay logic tests
- scene integration tests
- manual acceptance checklists
- regression tests
- validation of agent tasks

## Project Context

**Don’t Let Her In** is a Unity mobile horror prototype.

The core loop is:

```txt
Question starts
Timer starts
Creature advances
Player answers
Answer is evaluated
Threat distance changes
Next floor or death
```

Testing should focus first on making this loop reliable.

## Testing Philosophy

Use tests to protect the core gameplay rules.

Do not over-test placeholder visuals.

Prioritize:

- `ThreatManager`
- answer evaluation
- question timing
- run progression
- death trigger
- victory trigger
- restart flow
- data validation

## Unity Test Types

### EditMode Tests

Use EditMode tests for pure logic.

Best targets:

- `ThreatManager`
- `QuestionEvaluator`
- `RunController` pure logic
- score calculation
- answer speed classification
- data validation

EditMode tests should not require a full scene.

### PlayMode Tests

Use PlayMode tests for integration.

Best targets:

- scene loads
- UI appears
- question starts
- answer button triggers logic
- death flow works
- restart flow works
- floor transition works

Use PlayMode tests only when scene behavior matters.

## Minimum Test Coverage for Prototype

The prototype should have tests for:

```txt
Threat distance clamps between 0 and 100
Correct fast increases distance
Correct normal increases distance
Correct slow increases distance slightly
Wrong answer decreases distance
Timeout decreases distance more than wrong answer
Stress increases after wrong answer
Stress increases more after timeout
Correct fast can reduce stress
Death triggers when distance reaches 0
Run can mark victory after final floor
Answer speed is classified correctly
```

## ThreatManager Test Cases

Required cases:

### Initial state

Verify:

```txt
distance starts at configured value
stress starts at configured value
isDead is false when distance > 0
```

### Correct fast

Verify:

```txt
distance increases by fast reward
stress decreases by configured amount
distance does not exceed 100
```

### Correct normal

Verify:

```txt
distance increases by normal reward
distance does not exceed 100
```

### Correct slow

Verify:

```txt
distance increases slightly
pressure remains meaningful
```

### Wrong answer

Verify:

```txt
distance decreases by wrong penalty
stress increases
combo resets if combo exists
```

### Timeout

Verify:

```txt
distance decreases by timeout penalty
stress increases more than wrong answer
timeout can kill if distance reaches 0
```

### Clamping

Verify:

```txt
distance cannot go below 0
distance cannot go above 100
stress cannot go below minimum if clamped
```

## Question Evaluation Test Cases

Required cases:

```txt
selected correct answer returns isCorrect true
selected wrong answer returns isCorrect false
timeout returns isTimeout true
answer speed fast/normal/slow is classified from response time
invalid answer index is handled safely
```

## Run Progression Test Cases

Required cases:

```txt
run starts at floor 1 or configured first floor
correct answer can advance to next floor
wrong answer does not accidentally skip floor unless designed
death ends run
completing final floor wins run
restart resets run state
```

## Scene PlayMode Checks

For `Game.unity`, verify manually or with PlayMode tests:

```txt
scene loads
camera exists
UI canvas exists
GameSystems exists
question can appear
answer buttons are clickable
wrong answer creates visible feedback
timeout creates visible feedback
death screen or result state appears
restart works
```

## Manual Test Checklist

For every playable build, manually test:

1. Start run.
2. Let first question appear.
3. Tap correct answer quickly.
4. Tap correct answer slowly.
5. Tap wrong answer.
6. Wait for timeout.
7. Verify creature distance changes.
8. Trigger death.
9. Restart.
10. Complete run if possible.
11. Check console for errors.
12. Check mobile portrait layout if possible.

## Test Naming

Use clear names.

Examples:

```txt
WrongAnswer_DecreasesDistance
Timeout_DecreasesDistanceMoreThanWrongAnswer
CorrectFast_IncreasesDistanceAndReducesStress
Distance_IsClampedBetweenZeroAndOneHundred
Death_IsTriggeredWhenDistanceReachesZero
FinalFloorCompletion_WinsRun
```

## Test File Locations

EditMode tests:

```txt
UnityProject/Assets/Tests/EditMode/
```

PlayMode tests:

```txt
UnityProject/Assets/Tests/PlayMode/
```

Recommended files:

```txt
ThreatManagerTests.cs
QuestionEvaluatorTests.cs
RunControllerTests.cs
GameSceneFlowTests.cs
```

## Running Tests

Prefer Unity Test Runner.

If command-line testing is configured later, document exact commands in `Docs/TEST_PLAN.md`.

For now, every agent delivery must state:

```txt
Tests run: Unity Test Runner EditMode
Result: X passed / Y failed
```

If tests were not run:

```txt
Tests run: not run
Reason: Unity editor unavailable in this environment
```

Never claim tests passed if they were not executed.

## Acceptance Criteria

A testing task is acceptable if:

- tests target meaningful behavior
- test names are readable
- tests are deterministic
- tests do not require final art
- tests avoid fragile timing when possible
- failures are reported honestly
- untested behavior is listed

## Delivery Requirements

At the end of any testing task, report:

- tests added
- files changed
- tests run
- pass/fail result
- failures if any
- untested areas
- git status
- targeted commit command

## Commit Examples

```bash
git add UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs
git commit -m "🧪 test(gameplay): cover threat distance rules"
```

```bash
git add UnityProject/Assets/Tests/EditMode/QuestionEvaluatorTests.cs
git commit -m "🧪 test(questions): cover answer evaluation"
```
