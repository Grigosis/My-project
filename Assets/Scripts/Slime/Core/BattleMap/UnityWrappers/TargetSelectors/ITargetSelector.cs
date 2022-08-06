using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class CoreCellTargetSelector
    {
        public List<SkillTarget> GetAllPossibleTargets(LivingEntity m_caster, SkillEntity m_skillEntity, Battle m_battle)
        {
            var list = new List<SkillTarget>();
            var from = m_caster.Cell.Position;
            m_battle.BattleMap.Foreach(m_caster.Cell, m_skillEntity.GetMinRange(), m_skillEntity.GetMaxRange(), cell =>
            {
                var to = new Vector2Int(cell.X, cell.Y);
                if (!m_battle.BattleMap.IntersectsWall(from, to))
                {
                    list.Add(cell);
                }
            });
            return list;
        }
    }

    public class EnemyCellTargetSelector
    {
        
    }
    
    public interface ITargetSelector
    {
        List<SkillTarget> GetAllPossibleTargets(LivingEntity m_caster, SkillEntity m_skillEntity, Battle m_battle);
    }
    
    public interface ITargetFilter
    {
        List<SkillTarget> FilterGood(LivingEntity m_caster, SkillEntity m_skillEntity, Battle m_battle, List<SkillTarget> list);
    }
}