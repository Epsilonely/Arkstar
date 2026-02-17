# Claude Code - Memory Bank System

## Core System Purpose
This memory bank transforms Claude into a self-documenting development system that maintains project context across sessions.

## Required Core Files (6 files)

All files are located in the `memory-bank/` folder:

1. **projectbrief.md** - Foundation document defining core requirements and project scope
2. **productContext.md** - Project purpose, problems solved, UX goals
3. **systemPatterns.md** - Architecture, technical decisions, design patterns
4. **techContext.md** - Tech stack, setup requirements, constraints, dependencies
5. **activeContext.md** - Current focus, recent changes, next steps, active decisions
6. **progress.md** - What works, remaining tasks, status, known issues

## Key Workflow Rules

### Plan Mode
1. Read Memory Bank
2. Verify completeness
3. Develop strategy
4. Present approach

### Act Mode
1. Check Memory Bank
2. Update documentation
3. Execute
4. Document changes

## Critical Requirements

- **Read ALL memory bank files at the start of EVERY task** (non-optional)
- When user requests "update memory bank", review every file completely
- Focus on `activeContext.md` and `progress.md` as primary change documents
- Create additional context files within `memory-bank/` folder as needed
- **All memory bank .md files MUST be written in English**

## Memory Bank Update Process

When user requests "update memory bank":

1. **Read and review all 6 files**
2. **Analyze changes from current session**
3. **Update relevant files**:
   - `activeContext.md`: Current work status
   - `progress.md`: Completed and remaining tasks
   - `systemPatterns.md`: When new patterns are discovered
   - Other files: As needed
4. **Provide summary of changes**

## Getting Started

At the start of each new session:
1. Read all files in `memory-bank/` folder
2. Check current context from `activeContext.md`
3. Check next tasks from `progress.md`
4. Begin work with the user

## Project Info

- **Project**: Arkstar - RimWorld Mod
- **Type**: C# DLL + XML Defs
- **Source**: `c:\Users\yoong\source\repos\Arkstar\`
- **Deployed Mod**: `D:\SteamLibrary\steamapps\common\RimWorld\Mods\Arkstar\`
- **Steam Workshop ID**: 3602473014

---

**Note**: The effectiveness of this system depends entirely on the accuracy of the memory bank. Maintain precision and clarity. All memory bank files must be written in English.
