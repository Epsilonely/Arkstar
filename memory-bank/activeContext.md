# Active Context

## Current Focus
**v1.0.5 - Structure damage + quality scaling + Steam Workshop update preparation**

## Recent Changes

- **2026-03-03**: v1.0.5 Changes
  - **Feature**: Buildings/structures can now be directly targeted (`canTargetBuildings=true` in XML verb)
  - **Feature**: Projectile now damages structures on impact (`else if (hitThing != null)` branch in Impact())
  - **Fix**: Damage now scales with weapon quality — replaced hardcoded `DAMAGE_AMOUNT`/`ARMOR_PENETRATION` constants with `DamageAmount`/`ArmorPenetration` Projectile properties
  - **Fix**: `Destroy()` double-call error when hitting non-Pawn things (added `return` after `base.Impact()`, then refactored to direct TakeDamage)
  - **Files modified**: Projectile_RicochetBullet.cs, Arkstar_RangedWeapon.xml, About.xml

- **2026-03-03**: About.xml updated to v1.0.5
  - Description rewritten to focus on weapon features (removed changelog entries)
  - Steam Workshop description finalized (English + Korean)

- **2026-02-19**: v1.0.4 - Lord postAction error fix
  - `hitAngle=-1f` → actual angle calculation
  - Status: Fix complete, included in v1.0.5 build

## Next Steps

### Immediate
1. Build final v1.0.5 DLL
2. Copy to mod folder
3. Upload to Steam Workshop with changelog

### Future Improvements (awaiting user request)
- Additional weapon variants
- Sound effects
- Mod settings UI (ModSettings)
- Combat Extended compatibility

## Active Decisions
- Balance maintained at current levels (7.5/10)
- Structure damage added as feature (not bug fix)
- Damage now quality-scaled via Projectile base properties

## Current Blockers
**None**

## Working Files

### Source Project
- Arkstar/Projectile_RicochetBullet.cs - Ricochet projectile class (v1.0.5)
- Arkstar/Arkstar.csproj - Visual Studio project

### Deployed Mod (`D:\SteamLibrary\steamapps\common\RimWorld\Mods\Arkstar\`)
- About/About.xml - Mod metadata (v1.0.5)
- Assemblies/Arkstar.dll - Compiled DLL (needs update)
- Defs/ThingDefs/Arkstar_RangedWeapon.xml - Weapon and projectile definitions (canTargetBuildings=true)
- Languages/Korean/DefInjected/ThingDefs/Arkstar_RangedWeapon.xml - Korean translation
- Textures/Arkstar/WeaponRanged/ArkStar.png - Weapon texture
- Textures/Arkstar/Projectile/ArkStar_small.png - Projectile texture

## Bug Fix Archive

### v1.0.3 - Self-Ricochet & Weapon Name Bugs
**Problem 1**: Self-ricochet applied instant damage without projectile animation
**Fix**: Removed special case, all targets spawn projectiles equally. Set `launcher=null` for self-ricochet.

**Problem 2**: Weapon name not showing in health tab for ricochet damage
**Fix**: Added `originalLauncher` and `originalEquipment` fields to preserve weapon info across ricochets.

### v1.0.4 - Lord postAction Error
**Error**: `System.InvalidOperationException: Sequence contains no elements`
**Stack**: `Lord:Notify_PawnDamaged → Transition:CheckSignal → Transition:Execute`
**Cause**: `TakeDamage()` called with `hitAngle=-1f`
**Fix**: `float hitAngle = (hitPawn.Position - base.Position).ToVector3().AngleFlat();`

### v1.0.5 - Structure Damage & Quality Scaling
**Problem 1**: Non-Pawn hits (walls, buildings) caused `Destroy() already destroyed` error
**Cause**: `base.Impact()` internally calls `Destroy()`, then our code called it again
**Fix**: Direct `TakeDamage()` call for non-Pawn targets, no `base.Impact()`

**Problem 2**: Damage hardcoded to 5, ignoring weapon quality
**Fix**: Replaced `DAMAGE_AMOUNT`/`ARMOR_PENETRATION` constants with `DamageAmount`/`ArmorPenetration` Projectile properties

---
*Last Updated: 2026-03-03*
