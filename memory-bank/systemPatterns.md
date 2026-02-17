# System Patterns

## System Architecture
```
RimWorld Mod Structure
├── Arkstar.dll (compiled assembly)
│   └── Projectile_RicochetBullet : Projectile
│       ├── State management (chance, count, hit targets)
│       ├── Collision handling (Impact)
│       └── Ricochet logic (TryRicochet)
└── Defs/ (XML definition files)
    └── Weapon and projectile definitions
```

## Key Technical Decisions

### 1. RimWorld Projectile Inheritance
- **Decision**: Inherit from `Projectile` class
- **Reason**: Full compatibility with RimWorld's base projectile system
- **Benefit**: Reuse of launch, movement, collision detection logic

### 2. Projectile Creation for All Targets (Updated v1.0.3)
- **Decision**: Create new projectile for ALL targets including self
- **Reason**:
  - Consistent visual feedback and game experience
  - Self-ricochet needs visible animation (not instant damage)
  - Player needs to see when they get hit
- **Implementation**: Unified logic at Line 160-182
- **Special handling**: Set `launcher=null` when targeting self to force collision detection (Line 172)

### 3. HashSet for Target Tracking
- **Decision**: `HashSet<Thing> hitTargets` to store already-hit targets
- **Reason**: Prevent targeting the same Pawn multiple times
- **Benefit**: O(1) lookup performance

### 4. Probability Decay System
- **Decision**: Fixed 15% reduction per bounce + skill bonus
- **Reason**: Prevent infinite chains while rewarding skilled shooters
- **Formula**:
  ```
  reductionRate = 0.15 - (skillLevel - 10) * 0.005
  minimum reductionRate = 0.05
  ```

### 5. Initialization Flag (`isInitialized`)
- **Decision**: Ricochet-spawned projectiles skip initialization
- **Reason**: Must inherit chance/count from parent projectile
- **Implementation**: Flag set at Line 162, checked at Line 31

### 6. Original Launcher/Equipment Preservation (v1.0.3)
- **Decision**: Store `originalLauncher` and `originalEquipment` fields
- **Reason**: Setting `launcher=null` for self-ricochet loses weapon info in DamageInfo
- **Benefit**: Combat log and health tab always show correct weapon name
- **Implementation**:
  - Fields declared at Line 24-25
  - Stored at initialization (Line 36-37)
  - Used in DamageInfo (Line 56-57, 64, 77)
  - Propagated to child projectiles (Line 166-167)

## Design Patterns

### 1. State Pattern
- **Location**: Line 20-25
- **Purpose**: Track projectile state (chance, count, target list, original launcher/equipment)

### 2. Template Method Pattern
- **Location**: `Impact()` method override
- **Purpose**: Insert ricochet logic into RimWorld's projectile collision flow

### 3. Strategy Pattern
- **Location**: Target selection logic in `TryRicochet()`
- **Purpose**: Change targeting strategy based on skill level
  - Line 120-132: Skilled strategy (enemies only)
  - Line 134-138: Unskilled strategy (all pawns)

### 4. Factory Pattern
- **Location**: Line 161 `ThingMaker.MakeThing(def)`
- **Purpose**: Create new ricochet projectiles with copied state

### 5. Null Object Pattern (v1.0.3)
- **Location**: Line 172 conditional launcher assignment
- **Purpose**: Use `null` launcher for self-ricochet to bypass RimWorld's self-collision skip
- **Benefit**: Forces collision detection even when targeting self

## Code Structure
```
Arkstar/ (source project)
├── Arkstar/
│   ├── Projectile_RicochetBullet.cs  # Ricochet projectile class
│   ├── Properties/AssemblyInfo.cs     # Assembly metadata
│   ├── bin/Debug|Release/Arkstar.dll  # Build output
│   └── Arkstar.csproj                 # Project file
├── memory-bank/                       # Memory bank docs
└── CLAUDE.md                          # Claude settings

Deployed Mod (D:\SteamLibrary\...\Mods\Arkstar\)
├── About/About.xml                    # Mod info
├── Assemblies/Arkstar.dll             # Compiled DLL
├── Defs/ThingDefs/Arkstar_RangedWeapon.xml  # Weapon + projectile defs
├── Languages/Korean/DefInjected/      # Korean translation
└── Textures/Arkstar/                  # Weapon + projectile textures
```

## Key Components

### Projectile_RicochetBullet (core class)
- **File**: Arkstar/Projectile_RicochetBullet.cs
- **Key Methods**:
  - `SpawnSetup()`: Initialization (Line 27-40)
  - `Impact()`: Collision handling (Line 42-89)
  - `TryRicochet()`: Ricochet attempt (Line 91-183)
- **State Fields** (Line 20-25):
  - `currentRicochetChance`: Current bounce probability
  - `ricochetCount`: Number of bounces so far
  - `hitTargets`: HashSet of already-hit targets
  - `isInitialized`: Flag to skip re-initialization
  - `originalLauncher`: Original shooter (preserved for DamageInfo)
  - `originalEquipment`: Original weapon (preserved for combat log)

### Constants (Line 11-18)
```csharp
INITIAL_RICOCHET_CHANCE = 1.0f      // Initial 100%
RICOCHET_CHANCE_REDUCTION = 0.15f   // 15% reduction
MINIMUM_RICOCHET_CHANCE = 0.1f      // Minimum 10%
RICOCHET_RANGE = 7f                 // 7 tile range
STUN_DURATION = 3f                  // 3 second stun
MIN_SHOOTING_SKILL = 10             // Skill threshold
DAMAGE_AMOUNT = 5                   // Damage per hit
ARMOR_PENETRATION = 0.70f           // 70% armor pen
```

### Target Selection Algorithm
1. Search Pawns within 7 tiles
2. Exclude already-hit targets
3. Exclude dead or downed targets
4. Line of sight check
5. Skill-based filtering
6. Random selection

### Probability Decay Formula
```csharp
if (skillLevel > 10) {
    skillBonus = (skillLevel - 10) * 0.005
    reductionRate = max(0.05, 0.15 - skillBonus)
}
newChance = max(0.1, currentChance - reductionRate)
```

## RimWorld Integration Points
- **Namespace**: `Arkstar`
- **RimWorld API**: Projectile, DamageInfo, DamageDefOf, FleckMaker, GenRadial, GenSight, ThingMaker, GenSpawn
- **Unity**: `UnityEngine.Mathf`, `Vector3`
