# Skill — Game Agent Delivery

## Name

game-agent-delivery

## Purpose

Use this skill at the end of every implementation or documentation task.

This skill ensures the agent delivers cleanly, reports honestly, avoids broad Git operations and keeps the project easy to review.

## Project Context

**Don’t Let Her In** is developed through small agent-driven phases.

The user works alone and needs reliable, reviewable, step-by-step progress.

The agent must behave like a careful junior developer:

- implement only the requested scope
- avoid unrelated changes
- run tests when possible
- report limitations
- suggest targeted commits
- never hide uncertainty

## Delivery Philosophy

Every task must end with a structured report.

The report must make it clear:

- what changed
- why it changed
- which files changed
- what was tested
- what was not tested
- whether there are risks
- what Git command should be used

## Mandatory Final Report Format

Every task must end with:

````md
### Summary

Short explanation of what changed.

### Files changed

- `path/to/file`
- `path/to/file`

### Tests run

- Test command or Unity Test Runner action.

### Results

Pass/fail result.

### Manual checks

- What was checked manually.

### Known limits

- What is incomplete or not verified.

### Git status

Output or summary of `git status --short`.

### Recommended commit

```bash
git add path/to/file path/to/other-file
git commit -m "type(scope): message"
```
````

````

## Git Rules

Never use:

```bash
git add .
````

Always use targeted paths.

Good:

```bash
git add AGENTS.md Docs/PRD.md
```

Bad:

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
*.apk
*.aab
*.ipa
*.app
*.exe
*.dmg
*.zip
.env
.env.local
```

Do not commit large imported assets unless explicitly requested.

## Commit Message Style

Use concise conventional commits with emoji.

Examples:

```txt
🧹 chore(project): initialize Unity horror prototype
📝 docs(project): add prototype product docs
🎮 feat(gameplay): add first playable threat loop
👻 feat(creature): add placeholder hallway entity
🛗 feat(elevator): add door transition loop
🔊 feat(audio): add horror feedback cues
📱 feat(mobile): add portrait gameplay UI
🧪 test(gameplay): cover threat manager rules
🐛 fix(gameplay): clamp threat distance at zero
```

## Scope Control

Before coding, identify:

```txt
Current phase
Requested task
Included scope
Excluded scope
Acceptance criteria
```

If the task is too broad, implement the smallest useful version and report what remains.

Do not add features just because they seem useful.

Do not silently change:

- project direction
- engine choice
- platform target
- prototype scope
- art direction
- gameplay rules
- folder structure
- test strategy

## Honesty Requirements

The agent must not claim:

- tests passed if they were not run
- Play Mode works if it was not checked
- mobile performance is good if it was not measured
- build succeeded if no build was produced
- assets were optimized if they were not inspected
- code is production-ready if it is prototype-only

If something cannot be verified, say so.

Correct wording:

```txt
Tests were not run because Unity Editor is unavailable in this environment.
```

Incorrect wording:

```txt
Tests should pass.
```

## Review Requirements

Before finishing a task, check:

- no unrelated files changed
- no generated folders accidentally included
- no heavy assets added
- no hidden dependency introduced
- no TODO that blocks acceptance
- no console errors if Unity was opened
- docs updated if behavior changed

## Recommended Workflow

For each agent task:

1. Read relevant docs.
2. Restate scope briefly.
3. Implement smallest useful change.
4. Add or update tests.
5. Run tests if possible.
6. Check Git status.
7. Prepare final report.
8. Suggest targeted commit command.

## Delivery Examples

### Example 1 — Gameplay Feature

````md
### Summary

Added `ThreatManager` with distance, stress and answer effect rules.

### Files changed

- `UnityProject/Assets/Scripts/Threat/ThreatManager.cs`
- `UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs`

### Tests run

- Unity Test Runner — EditMode

### Results

- 8 passed
- 0 failed

### Manual checks

- Not checked in Play Mode. Logic-only change.

### Known limits

- Not yet connected to UI or creature movement.

### Git status

- `M UnityProject/Assets/Scripts/Threat/ThreatManager.cs`
- `M UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs`

### Recommended commit

```bash
git add UnityProject/Assets/Scripts/Threat/ThreatManager.cs UnityProject/Assets/Tests/EditMode/ThreatManagerTests.cs
git commit -m "🎮 feat(gameplay): add threat distance system"
```
````

````

### Example 2 — Documentation

```md
### Summary

Added prototype game design rules and challenge categories.

### Files changed

- `Docs/GAME_DESIGN.md`

### Tests run

- Not applicable. Documentation-only change.

### Results

- Documentation updated.

### Manual checks

- Reviewed Markdown headings and sections.

### Known limits

- Does not yet include full question bank.

### Git status

- `M Docs/GAME_DESIGN.md`

### Recommended commit

```bash
git add Docs/GAME_DESIGN.md
git commit -m "📝 docs(game-design): define prototype challenge rules"
````

```

## Final Rule

A task is not complete until the final delivery report is clear enough for the user to decide whether to commit.
```
