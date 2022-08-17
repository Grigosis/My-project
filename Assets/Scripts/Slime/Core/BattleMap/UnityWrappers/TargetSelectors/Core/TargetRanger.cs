using System;
using Assets.Scripts.Slime.Core.Algorythms.Data;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class SingleUnitTargetRanger : ITargetRanger
    {
        public double RangeTargets(LivingEntity m_caster, SkillEntity m_skillEntity, Battle m_battle, ISkillTarget target, Vector2Int fromPosition)
        {
            var filter = m_skillEntity.Definition.RelationShipFilter;
            if (target is LivingEntity ble)
            {
                if (m_skillEntity.Definition.RelationShipFilter == RelationShip.Any) return 1;
                //if (m_skillEntity.Definition.RelationShipFilter.HasFlag(RelationShip.))
                var relationTo = ble.GetRelationTo(m_caster);
                return filter.HasFlag(relationTo) ? 1 : -1;
            }

            throw new Exception($"Wrong type of target {target.GetType()}");
        }
    }

    public class SplashCellTargetRanger : ITargetRanger
    {
        public double RangeTargets(LivingEntity m_caster, SkillEntity m_skillEntity, Battle m_battle, ISkillTarget target, Vector2Int fromPosition)
        {
            var filter = m_skillEntity.Definition.RelationShipFilter;
            if (target is BattleMapCell cell)
            {
                int enemies = 0;
                int neutrals = 0;
                int ally = 0;
                m_skillEntity.SplashProvider.GetSplashCells(m_battle, m_battle.BattleMap[fromPosition], cell, m_skillEntity.Definition.SplashRange, (x) =>
                {
                    var entity = x.Entity;
                    if (x.Position == fromPosition) entity = m_caster;
                    if (entity == null) return;
                    var relationShip = entity.GetRelationTo(m_caster);
                    switch (relationShip)
                    {
                        case RelationShip.Enemy:
                            enemies++;
                            break;
                        case RelationShip.Ally:
                            ally++;
                            break;
                        case RelationShip.Neutral:
                            neutrals++;
                            break;
                    }
                });

                int total = 0;

                if (filter.HasFlag(RelationShip.Enemy)) total += enemies;
                if (filter.HasFlag(RelationShip.Ally)) total += ally;
                if (filter.HasFlag(RelationShip.Neutral)) total += neutrals;

                return total;
            }
            
            throw new Exception($"Wrong type of target {target.GetType()}");
        }
    }
}