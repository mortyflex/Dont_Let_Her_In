# Agent Prompt — Phase 2 Question System

## Recommended Model

Recommended model:

```txt
Claude
```

Model switch recommendation:

```txt
Do not switch models yet.
```

Reason:

```txt
This phase still touches central gameplay logic: question data, answer evaluation, timer rules, answer speed classification, timeout handling and EditMode tests. Keep Claude as the main model for continuity after Phase 1.
```

Risk level:

```txt
Medium
```

Expected commit:

```txt
🎮 feat(questions): add data-driven question system
```

---

## Project

You are working on the Unity project:

```txt
Don’t Let Her In
```

This is a Unity 6 URP iOS-first portrait horror prototype.

The player is trapped in an elevator. A creature approaches while the player answers short survival questions.

Main promise:

```txt
Every second of hesitation brings her closer.
```

---

## Required Reading Before Coding

Read these files before making changes:

```txt
CLAUDE.md
AGENTS.md
README.md
Docs/AgentPrompts/PHASE_2_QUESTION_SYSTEM.md
Docs/ROADMAP.md
Docs/GAME_DESIGN.md
Docs/TECH_ARCHITECTURE.md
Docs/TEST_PLAN.md
Docs/DECISIONS.md
Skills/unity-gameplay-loop/SKILL.md
Skills/unity-testing/SKILL.md
Skills/game-agent-delivery/SKILL.md
```

Also inspect the Phase 1 implementation before coding:

```txt
UnityProject/Assets/Scripts/Core/
UnityProject/Assets/Scripts/GameLoop/
UnityProject/Assets/Scripts/Threat/
UnityProject/Assets/Tests/EditMode/
```

Respect `CLAUDE.md` and `AGENTS.md`.

Delivery reports must be written in French.

Code, class names, method names, test names, file names, technical identifiers and commit messages must stay in English.

---

## Current Project State

Phase 1 has been completed and committed.

Current known commit:

```txt
6dd6b73 — 🎮 feat(gameplay): add core threat run loop
```

Phase 1 added:

```txt
GameState
GameManager
RunController
RunResult
ThreatManager
ThreatState
EditMode tests
runtime asmdef
EditMode test asmdef
```

Phase 1 test result:

```txt
28 EditMode tests passed
```

The project is currently logic-only.

No final UI, final art, scene assembly, audio, creature visuals or question system should exist yet.

---

## Mission

Implement:

```txt
Phase 2 — Question System
```

The goal is to create a data-driven and testable question system.

This phase should support the future core loop:

```txt
Question starts
Timer starts
Player answers or times out
Answer is evaluated
Answer speed is classified
AnswerResult is produced
ThreatManager can later consume the result
```

This phase is still mostly logic-first.

Do not build the final UI.

Do not assemble the elevator/corridor scene.

Do not add final art.

Do not add audio.

Do not create full floor content yet.

Do not add iOS build/export work.

Do not add monetization, analytics, cloud save, online features, VR/XR, Android-specific work, procedural generation, inventory, free movement or multiple creatures.

---

## Phase 2 Scope

Included:

```txt
QuestionType enum
AnswerSpeed enum
QuestionData ScriptableObject
AnswerResult model
QuestionEvaluator or equivalent pure evaluation logic
QuestionManager
timeout handling
answer speed classification
basic validation helper for QuestionData if useful
EditMode tests
```

Excluded:

```txt
final UI
answer button UI
visual clue system
audio clue system
full question bank
floor sequence content
creature visuals
scene assembly
PlayMode scene flow
iOS build
```

---

## Required Folder Locations

Use the existing folder:

```txt
UnityProject/Assets/Scripts/Questions/
```

Use the existing tests folder:

```txt
UnityProject/Assets/Tests/EditMode/
```

Question ScriptableObject assets are not required in this phase.

If you create sample assets, keep them minimal and explain why. Prefer not to create sample assets yet unless necessary.

---

## Required Files

Create or update these files:

```txt
UnityProject/Assets/Scripts/Questions/QuestionType.cs
UnityProject/Assets/Scripts/Questions/AnswerSpeed.cs
UnityProject/Assets/Scripts/Questions/QuestionData.cs
UnityProject/Assets/Scripts/Questions/AnswerResult.cs
UnityProject/Assets/Scripts/Questions/QuestionEvaluator.cs
UnityProject/Assets/Scripts/Questions/QuestionManager.cs
UnityProject/Assets/Tests/EditMode/QuestionEvaluatorTests.cs
UnityProject/Assets/Tests/EditMode/QuestionManagerTests.cs
```

If you need a small helper class, keep it minimal and put it in:

```txt
UnityProject/Assets/Scripts/Questions/
```

Do not create unrelated systems.

---

## Required QuestionType Values

Create this enum:

```csharp
public enum QuestionType
{
    Observation,
    ShortMemory,
    AudioClue,
    EnvironmentalInstruction,
    SimpleLogic,
    SangFroid,
    Anomaly
}
```

---

## Required AnswerSpeed Values

Create this enum:

```csharp
public enum AnswerSpeed
{
    Fast,
    Normal,
    Slow,
    Timeout
}
```

---

## QuestionData Requirements

`QuestionData` should be a ScriptableObject.

It should contain at least:

```txt
Id
Type
Prompt
Answers
CorrectAnswerIndex
TimeLimitSeconds
Difficulty
Tags
```

Optional but acceptable if simple:

```txt
FastCorrectReward
NormalCorrectReward
SlowCorrectReward
WrongAnswerPenalty
TimeoutPenalty
OptionalVisualClueId
OptionalAudioClueId
```

Keep this data asset simple.

Do not hardcode question content in `QuestionManager`.

Use clear validation behavior.

Possible validation method:

```txt
IsValid()
```

Validation should check at least:

```txt
prompt is not empty
answers has at least two entries
correct answer index is in range
time limit is greater than zero
```

---

## AnswerResult Requirements

`AnswerResult` should contain at least:

```txt
QuestionId
IsCorrect
AnswerSpeed
SelectedAnswerIndex
CorrectAnswerIndex
ResponseTimeSeconds
TimeLimitSeconds
IsTimeout
```

Optional if useful:

```txt
DistanceDelta
StressDelta
```

Do not over-couple `AnswerResult` to `ThreatManager` in this phase.

The threat system should be able to consume answer outcome later, but Phase 2 should remain focused on the question system.

---

## Answer Speed Rules

Use this classification:

```txt
Fast: answered in first 35% of timer
Normal: answered after 35% and up to 70% of timer
Slow: answered after 70% of timer
Timeout: no answer before timer reaches 0
```

Example with 10-second timer:

```txt
0.0s to 3.5s: Fast
>3.5s to 7.0s: Normal
>7.0s to 10.0s: Slow
10.0s or more: Timeout
```

Be consistent and test boundary cases clearly.

If a boundary is ambiguous, choose one rule and document it in the test names or assertions.

---

## QuestionEvaluator Requirements

Create a pure logic evaluator.

It should support:

```txt
classifying answer speed
evaluating selected answer index
detecting timeout
handling invalid selected answer safely
returning AnswerResult
```

Suggested methods:

```txt
ClassifyAnswerSpeed(float responseTimeSeconds, float timeLimitSeconds, bool timedOut)
Evaluate(QuestionData question, int selectedAnswerIndex, float responseTimeSeconds)
EvaluateTimeout(QuestionData question)
```

Exact method names may vary, but keep the API clear and testable.

Invalid answer indexes should not crash.

If selected answer index is invalid:

```txt
IsCorrect should be false
SelectedAnswerIndex should preserve the invalid index if useful
AnswerResult should still be produced
```

---

## QuestionManager Requirements

`QuestionManager` can be minimal in this phase.

It may be a MonoBehaviour if useful for future Unity integration, but core evaluation logic must stay testable.

It should support:

```txt
starting a question
tracking whether a question is active
tracking elapsed time or remaining time
submitting an answer
resolving timeout
returning or exposing the last AnswerResult
resetting current question state
```

Do not create UI.

Do not create answer buttons.

Do not create visual feedback.

This manager is allowed to be simple because UI integration happens later.

---

## Required EditMode Tests — QuestionEvaluator

Create EditMode tests for `QuestionEvaluator`.

Required test cases:

```txt
CorrectAnswer_ReturnsIsCorrectTrue
WrongAnswer_ReturnsIsCorrectFalse
Timeout_ReturnsIsTimeoutTrue
InvalidAnswerIndex_IsHandledSafely
FastAnswer_IsClassifiedAsFast
NormalAnswer_IsClassifiedAsNormal
SlowAnswer_IsClassifiedAsSlow
TimeoutAnswer_IsClassifiedAsTimeout
FastBoundary_IsHandledConsistently
NormalBoundary_IsHandledConsistently
QuestionData_WithPrompt_IsValid
QuestionData_WithoutPrompt_IsInvalid
QuestionData_WithAtLeastTwoAnswers_IsValid
QuestionData_WithLessThanTwoAnswers_IsInvalid
QuestionData_WithCorrectIndexInRange_IsValid
QuestionData_WithCorrectIndexOutOfRange_IsInvalid
QuestionData_WithPositiveTimer_IsValid
QuestionData_WithZeroTimer_IsInvalid
```

---

## Required EditMode Tests — QuestionManager

Create EditMode tests for `QuestionManager`.

Required test cases:

```txt
StartQuestion_SetsQuestionActive
StartQuestion_ResetsElapsedTime
SubmitAnswer_ProducesAnswerResult
SubmitAnswer_EndsQuestion
Timeout_ProducesTimeoutResult
Timeout_EndsQuestion
Reset_ClearsCurrentQuestion
CannotSubmitAnswer_WhenNoQuestionActive
```

If `QuestionManager` is a MonoBehaviour and harder to test directly, either:

```txt
create it in an EditMode test with a temporary GameObject
```

or:

```txt
move the pure state logic into a small testable class and keep the MonoBehaviour thin
```

Keep the design simple.

---

## Assembly Definition Notes

Phase 1 added runtime and test assembly definitions.

If new question scripts are under the existing runtime assembly, make sure the tests can reference them.

Do not create unnecessary extra asmdefs unless required.

If an asmdef reference must be updated, do it carefully and explain it in the final report.

---

## Manual Checks

After implementation, check if possible:

```txt
Unity project compiles
EditMode tests run
Game.unity still exists
No scene overbuild happened
No generated Unity folders are staged
```

Do not build iOS in this phase.

Do not generate a Xcode project.

---

## Git Rules

Do not use:

```bash
git add .
```

Do not commit:

```txt
UnityProject/Library/
UnityProject/Temp/
UnityProject/Obj/
UnityProject/Build/
UnityProject/Builds/
UnityProject/Logs/
UnityProject/UserSettings/
UnityProject/MemoryCaptures/
UnityProject/Recordings/
*.sln
*.slnx
*.csproj
*.user
.env
.env.local
```

Use targeted adds only.

Recommended add command:

```bash
git add UnityProject/Assets/Scripts/Questions UnityProject/Assets/Tests/EditMode
```

Recommended commit message:

```bash
git commit -m "🎮 feat(questions): add data-driven question system"
```

---

## Required Final Report

End your response with exactly this structure.

Write the report in French.

Keep code names, class names, method names, test names, file names, technical identifiers and commit messages in English.

````md
# Agent Delivery Report — Phase 2 Question System

## Model used

Claude

## Task status

Choose exactly one:

- COMPLETED_AND_COMMITTED
- COMPLETED_NOT_COMMITTED
- BLOCKED
- FAILED

## Commit

If committed:

- Commit hash:
- Commit message:

If not committed:

- Commit hash: N/A
- Commit message: N/A
- Reason not committed:

## Summary

Short summary of what changed.

## Files changed

List every changed file:

- `path/to/file` — created/modified/deleted + short reason

## Scope confirmation

Confirm each item:

- Final UI added: yes/no
- Final art added: yes/no
- Scene assembly added: yes/no
- Audio added: yes/no
- iOS build generated: yes/no
- VR/XR added: yes/no
- Android-specific work added: yes/no
- Monetization added: yes/no
- Generated Unity folders staged: yes/no

## Tests run

List exact commands or Unity Test Runner actions used.

If no tests were run, write exactly:

Tests were not run because Unity Editor / Unity Test Runner was unavailable in this environment.

## Test results

Use one of:

- PASS
- FAIL
- NOT_RUN

Then explain briefly.

## Manual checks

List what was checked manually.

If no manual checks were done, write:

No manual checks were performed.

## Git status

Paste the exact output of:

```bash
git status --short
```
````

If the output is empty, write:

```txt
<clean>
```

## Staged/generated file safety check

Paste the exact output of:

```bash
git status --short | grep -E "UnityProject/(Library|Temp|Logs|UserSettings|Build|Builds)|\.slnx|\.csproj"
```

If the output is empty, write:

```txt
<clean>
```

## Known limits

List anything incomplete, unverified, or risky.

## Next recommended action

Choose exactly one:

- READY_FOR_REVIEW
- NEEDS_FIX
- NEEDS_USER_ACTION
- SHOULD_REVERT

Then explain in one sentence.

````

Do not summarize freely outside this structure.

---

## Acceptance Criteria

Phase 2 is complete only if:

```txt
QuestionType exists
AnswerSpeed exists
QuestionData exists as a ScriptableObject
AnswerResult exists
QuestionEvaluator or equivalent pure logic exists
QuestionManager exists
answer evaluation works
timeout handling works
answer speed classification works
QuestionData validation exists
EditMode tests exist
EditMode tests pass if Unity Test Runner is available
No final UI added
No final art added
No scene overbuild
No forbidden generated folders staged
Agent final report is complete and written in French
````
