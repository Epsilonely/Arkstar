# Project Brief

## Project Name
**Arkstar** - RimWorld Mod

## Project Type
Mod for RimWorld (Steam game)

## Core Requirements
- Add a new ranged weapon to RimWorld
- Implement a projectile system with ricochet (bouncing) mechanics
- Skill-based targeting system (behavior changes based on Shooting skill)
- Special effect against Mechanoids (stun)

## Project Scope
- Ricochet bullet projectile class implementation
- Skill-based targeting logic
- Visual effects and damage system
- Weapon/projectile XML definitions
- Texture resources
- Korean localization
- Steam Workshop deployment

## Key Objectives
1. **Ricochet Mechanic**: Chance to bounce to other targets on hit
2. **Probability System**: Ricochet chance decreases per bounce (100% -> -15% per bounce -> minimum 10%)
3. **Skill-based Targeting**:
   - Shooting skill >= 10: Targets enemies only
   - Shooting skill < 10: Random targeting (including allies and self)
4. **Mechanoid Specialization**: 3-second stun effect on Mechanoid hit

## Success Criteria
- Ricochet projectile class implemented
- Probability-based bouncing system working
- Skill check system working
- Weapon definition XML complete (Arkstar_RangedWeapon.xml)
- Steam Workshop deployment complete (ID: 3602473014)
- In-game testing in progress (v1.0.0)
- Bug fix version testing (v1.0.1)

## Deployment Info
- **Steam Workshop**: https://steamcommunity.com/sharedfiles/filedetails/?id=3602473014
- **Package ID**: Epsilonely.Arkstar
- **Author**: Epsilonely
- **Current Version**: 1.0.1 (bug fix)
- **Released Version**: 1.0.0 (Steam)
- **RimWorld Version**: 1.6

## Weapon Details
- **Name**: ArcStar
- **Type**: Shuriken - futuristic thrown weapon
- **Concept**: Chain-attack weapon that bounces between multiple enemies
- **Range**: 24 tiles
- **Cooldown**: 3 seconds
- **Weight**: 0.08kg (very light)
