# Active Context

## Current Focus
**v1.0.3 - Steam Workshop update preparation**

Major bug fixes complete and tested. Preparing Steam Workshop update with changelog.

## Recent Changes

- **2026-02-17**: v1.0.3 Major Bug Fixes
  - **Bug 1**: Self-ricochet instant damage (no projectile animation)
    - **Root Cause**: Line 155-177 had special handling for self-target with direct damage
    - **Fix**: Removed special case, all targets now spawn projectiles equally
    - **Additional Fix**: Set `launcher=null` when ricocheting to self to force collision detection

  - **Bug 2**: Weapon name not showing in combat log for ricochet hits
    - **Root Cause**: `launcher=null` caused `equipment` to be null in DamageInfo
    - **Fix**: Added `originalLauncher` and `originalEquipment` fields to preserve weapon info
    - **Implementation**: Lines 24-25, 36-37, 56-57, 98-99, 166-167

  - **Files modified**: Projectile_RicochetBullet.cs
  - **Testing**: User confirmed all fixes working correctly

- **2026-02-17**: Balance evaluation
  - Analyzed current balance (7.5/10 - slightly strong)
  - User decision: Keep current balance (no changes)
  - Strong in dense/mechanoid situations, weak in 1v1

- **2026-02-17**: Steam Workshop update prep
  - About.xml updated to v1.0.3
  - Changelog written (Korean + English)
  - Ready for Steam Workshop upload

- **2026-02-17**: Memory bank system introduced
  - 6 core documents created
  - CLAUDE.md configuration file created

## Next Steps

### Immediate
1. Build final v1.0.3 DLL
2. Copy to mod folder
3. Upload to Steam Workshop with changelog

### Future Improvements (awaiting user request)
- Additional weapon variants
- Sound effects
- Mod settings UI (ModSettings)
- Combat Extended compatibility

## Active Decisions
Balance maintained at current levels. Focus on bug fixes only for v1.0.3.

## Current Blockers
**None** (but see Known Bug below)

## Working Files

### Source Project
- Arkstar/Projectile_RicochetBullet.cs - Ricochet projectile class (v1.0.3)
- Arkstar/Arkstar.csproj - Visual Studio project

### Deployed Mod (`D:\SteamLibrary\steamapps\common\RimWorld\Mods\Arkstar\`)
- About/About.xml - Mod metadata (v1.0.3)
- Assemblies/Arkstar.dll - Compiled DLL (needs update)
- Defs/ThingDefs/Arkstar_RangedWeapon.xml - Weapon and projectile definitions
- Languages/Korean/DefInjected/ThingDefs/Arkstar_RangedWeapon.xml - Korean translation
- Textures/Arkstar/WeaponRanged/ArkStar.png - Weapon texture
- Textures/Arkstar/Projectile/ArkStar_small.png - Projectile texture

## Bug Fix Archive

### v1.0.3 - Self-Ricochet & Weapon Name Bugs
**Problem 1**: Self-ricochet applied instant damage without projectile animation
**Root Cause**: Special case handling at Line 155-177 used direct damage for self-target
**Fix**:
- Removed special case code
- All targets spawn projectiles equally
- Set `launcher=null` for self-ricochet to force collision (Line 172)

**Problem 2**: Weapon name not showing in health tab for ricochet damage
**Root Cause**: `launcher=null` caused equipment info loss
**Fix**:
- Added `originalLauncher` and `originalEquipment` fields (Line 24-25)
- Store original values at initialization (Line 36-37)
- Use original values in DamageInfo (Line 56-57, 64, 77)
- Propagate to child projectiles (Line 166-167)

**Result**: All ricochet damage now shows "cut (arcstar)" in health tab with proper projectile animation

## Known Bug (미해결 - 추후 검토)

### Lord postAction 에러
**발견**: 2026-02-19
**에러**: `System.InvalidOperationException: Sequence contains no elements`
**스택**: `Lord:Notify_PawnDamaged → Transition:CheckSignal → Transition:Execute`
**트리거**: `Arkstar.Projectile_RicochetBullet:Impact` → `hitPawn.TakeDamage()`

**원인 분석**:
- `TakeDamage()` 호출 시 `hitAngle = -1f` 로 넘기는 것이 문제로 추정
- RimWorld가 -1f를 받으면 발사체 방향에서 각도를 자동 계산 시도
- 리코쳇 발사체의 비정상적 launcher 상태(null 등)로 인해 Lord의 transition 로직이 빈 컬렉션에서 `.First()`/`.Single()` 호출 → 예외 발생
- 우리 코드 버그라기보다 RimWorld Lord 시스템의 엣지 케이스를 우리가 트리거하는 것

**후보 수정 방법**:
```csharp
// Impact()의 TakeDamage 호출 시 hitAngle을 실제 각도로 계산
float angle = (hitPawn.Position - Position).ToVector3().AngleFlat();
hitPawn.TakeDamage(new DamageInfo(
    DamageDefOf.Cut, DAMAGE_AMOUNT, ARMOR_PENETRATION,
    angle,  // ← -1f 대신 실제 각도
    actualLauncher, null, equipment?.def
));
```

**현재 결정**: 수정 보류, 추후 재검토

---
*Last Updated: 2026-02-19*
