using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Arkstar
{
    public class Projectile_RicochetBullet : Projectile
    {
        private const float INITIAL_RICOCHET_CHANCE = 1.0f;
        private const float RICOCHET_CHANCE_REDUCTION = 0.15f;
        private const float MINIMUM_RICOCHET_CHANCE = 0.1f;
        private const float RICOCHET_RANGE = 7f;
        private const float STUN_DURATION = 3f;
        private const int MIN_SHOOTING_SKILL = 10;
        private const int DAMAGE_AMOUNT = 5;
        private const float ARMOR_PENETRATION = 0.70f;

        private float currentRicochetChance = INITIAL_RICOCHET_CHANCE;
        private int ricochetCount = 0;
        private HashSet<Thing> hitTargets = new HashSet<Thing>();
        private bool isInitialized = false;
        private Thing originalLauncher = null;
        private Thing originalEquipment = null;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            if (!respawningAfterLoad && !isInitialized)
            {
                currentRicochetChance = INITIAL_RICOCHET_CHANCE;
                ricochetCount = 0;
                hitTargets.Clear();
                originalLauncher = launcher;
                originalEquipment = (launcher as Pawn)?.equipment?.Primary;
                isInitialized = true;
            }
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            Map map = base.Map;
            IntVec3 position = base.Position;

            if (blockedByShield)
            {
                Destroy();
                return;
            }

            if (hitThing != null && hitThing is Pawn hitPawn)
            {
                // 원래 launcher와 equipment 정보 사용
                Thing actualLauncher = originalLauncher ?? launcher;
                Thing equipment = originalEquipment ?? (launcher as Pawn)?.equipment?.Primary;

                hitPawn.TakeDamage(new DamageInfo(
                    DamageDefOf.Cut,
                    DAMAGE_AMOUNT,
                    ARMOR_PENETRATION,
                    -1f,
                    actualLauncher,
                    null,
                    equipment?.def
                ));

                hitTargets.Add(hitThing);

                if (hitPawn.RaceProps.FleshType == FleshTypeDefOf.Mechanoid)
                {
                    if (!hitPawn.Dead && !hitPawn.Downed)
                    {
                        hitPawn.stances?.stunner?.StunFor(
                            Mathf.RoundToInt(STUN_DURATION * 60f),
                            actualLauncher
                        );
                    }
                }

                if (currentRicochetChance > 0)
                {
                    TryRicochet(map, position);
                }
            }

            Destroy();
        }

        private void TryRicochet(Map map, IntVec3 sourcePos)
        {
            if (Rand.Value > currentRicochetChance)
            {
                return;
            }

            Pawn shooter = (originalLauncher ?? launcher) as Pawn;
            Thing equipment = originalEquipment ?? shooter?.equipment?.Primary;

            // 주변 타겟 찾기 (자신 포함!)
            var nearbyPawns = GenRadial.RadialDistinctThingsAround(
                    sourcePos, map, RICOCHET_RANGE, true)
                .OfType<Pawn>()
                .Where(p => !hitTargets.Contains(p))
                .Where(p => !p.Dead && !p.Downed)
                .Where(p => GenSight.LineOfSight(sourcePos, p.Position, map, true))
                .ToList();

            if (!nearbyPawns.Any())
            {
                return;
            }

            int shootingLevel = shooter?.skills?.GetSkill(SkillDefOf.Shooting)?.Level ?? 0;

            Pawn newTarget;

            // 스킬 체크
            if (shootingLevel >= MIN_SHOOTING_SKILL && shooter != null)
            {
                // 고숙련: 적만 타겟
                var validTargets = nearbyPawns
                    .Where(p => p.HostileTo(shooter))
                    .ToList();

                if (!validTargets.Any())
                {
                    return;
                }

                newTarget = validTargets.RandomElement();
            }
            else
            {
                // 저숙련: 무작위 (자신 + 아군 + 적 모두 포함)
                newTarget = nearbyPawns.RandomElement();
            }

            if (newTarget == null || newTarget.Dead || newTarget.Downed) return;

            // 시각 효과
            FleckMaker.Static(sourcePos, map, FleckDefOf.ExplosionFlash, 1f);
            FleckMaker.ThrowLightningGlow(sourcePos.ToVector3Shifted(), map, 1f);
            FleckMaker.ThrowMicroSparks(sourcePos.ToVector3Shifted(), map);

            // 확률 감소
            ricochetCount++;
            float oldChance = currentRicochetChance;
            float reductionRate = RICOCHET_CHANCE_REDUCTION;

            if (shootingLevel > MIN_SHOOTING_SKILL)
            {
                float skillBonus = (shootingLevel - MIN_SHOOTING_SKILL) * 0.005f;
                reductionRate = Mathf.Max(0.05f, reductionRate - skillBonus);
            }

            currentRicochetChance = Mathf.Max(MINIMUM_RICOCHET_CHANCE, currentRicochetChance - reductionRate);

            // 모든 타겟에 대해 발사체 생성 (자신 포함)
            Projectile_RicochetBullet newProjectile = (Projectile_RicochetBullet)ThingMaker.MakeThing(def);
            newProjectile.isInitialized = true;
            newProjectile.currentRicochetChance = this.currentRicochetChance;
            newProjectile.ricochetCount = this.ricochetCount;
            newProjectile.hitTargets = new HashSet<Thing>(this.hitTargets);
            newProjectile.originalLauncher = this.originalLauncher ?? this.launcher;
            newProjectile.originalEquipment = this.originalEquipment ?? equipment;

            GenSpawn.Spawn(newProjectile, sourcePos, map);

            // 자신에게 튕길 때는 launcher를 null로 설정하여 충돌 판정 보장
            Thing launchLauncher = (newTarget == shooter) ? null : launcher;

            newProjectile.Launch(
                launchLauncher,
                sourcePos.ToVector3Shifted(),
                newTarget,
                newTarget,
                ProjectileHitFlags.IntendedTarget,
                false,
                equipment
            );
        }
    }
}