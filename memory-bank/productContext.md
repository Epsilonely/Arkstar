# Product Context

## Why This Project Exists
To add a unique ranged weapon with **chain-hit mechanics** to RimWorld, increasing combat strategy and fun.

## Problems It Solves
- **Monotony of existing ranged weapons**: Standard firearms are one-shot-one-target
- **Skill utilization**: Reinforces the importance of Shooting skill (skilled shooters don't hurt allies)
- **Mechanoid counter**: New way to deal with powerful Mechanoids (stun effect)

## How It Should Work

### Basic Flow
1. **Fire**: Player fires the ricochet bullet weapon at an enemy
2. **Hit**: Projectile deals 5 damage + 70% armor penetration
3. **Ricochet Check**: Attempts bounce at current ricochet chance (initial 100%)
4. **Target Selection**:
   - **Shooting skill >= 10**: Only hostile targets within 7 tiles
   - **Shooting skill < 10**: All Pawns within 7 tiles (including allies and self)
5. **Chance Reduction**: 15% decrease per bounce (minimum 10%)
6. **Repeat**: Continues bouncing while chance remains

### Special Cases
- **Mechanoid hit**: Additional 3-second (180 ticks) stun effect
- **Self-bounce**: Direct damage applied without projectile creation, continues bouncing
- **Already-hit targets**: Cannot be targeted again (tracked via HashSet)

## User Experience Goals
- **Skilled player reward**: Safe to use with Shooting skill 10+
- **Unskilled player risk**: Potential friendly fire creates tension
- **Visual feedback**: Explosion flash, lightning glow, sparks on each bounce
- **Strategic depth**: Very effective in areas with dense enemy concentrations

## Target Users
- **RimWorld players**: Users wanting new weapons beyond vanilla
- **Combat-focused players**: Users interested in combat mechanics
- **Mod enthusiasts**: Users who prefer unique gameplay mechanics

## Key Features
- **Chain ricochet system**: Probability-based limited bouncing (not infinite)
- **Skill-based targeting**: Safety changes based on proficiency
- **Mechanoid stun**: Specialized additional effect against mechanical enemies
- **Visual effects**: Flashy effects on each bounce
- **Probability decay system**: Higher skill = slower decay rate (max 0.005/level)
