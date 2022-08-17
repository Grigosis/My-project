using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using RPGFight;
using RPGFight.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class SelfTargetSelector : ITargetSelector
    {
        public void GetAllPossibleTargets(Battle m_battle, LivingEntity m_caster, SkillEntity m_skillEntity, Vector2Int fromPosition, List<ISkillTarget> buffer)
        {
            buffer.Add(m_caster);
        }
    }
    
    public class UnitTargetSelector : ITargetSelector
    {
        public void GetAllPossibleTargets(Battle m_battle, LivingEntity m_caster, SkillEntity m_skillEntity, Vector2Int fromPosition, List<ISkillTarget> buffer)
        {
            var units = m_battle.BattleMap.AllLivingEntities;
            foreach (var unit in units)
            {
                if (unit == m_caster)
                {
                    buffer.Add(unit);
                }
                else
                {
                    var to = new Vector2Int(unit.Cell.X, unit.Cell.Y);
                    if (!m_battle.BattleMap.IntersectsWall(fromPosition, to))
                    {
                        buffer.Add(unit);
                    }
                }
            }
        }
    }
    
    public class MovementTargetSelector : ITargetSelector
    {
        public void GetAllPossibleTargets(Battle m_battle, LivingEntity m_caster, SkillEntity m_skillEntity, Vector2Int fromPosition, List<ISkillTarget> buffer)
        {
            var cells = DI.GetCellsAvailableForMovement(m_battle.BattleMap, fromPosition, Balance.GetMovepoints(m_caster));
            foreach (var move in cells)
            {
                buffer.Add(m_battle.BattleMap[move.Position]);
            }
        }
    }
    
    public class CellTargetSelector : ITargetSelector
    {
        public void GetAllPossibleTargets(Battle m_battle, LivingEntity m_caster, SkillEntity m_skillEntity, Vector2Int fromPosition, List<ISkillTarget> buffer)
        {
            m_battle.BattleMap.Foreach(fromPosition, m_skillEntity.GetMinRange(), m_skillEntity.GetMaxRange(), cell =>
            {
                var to = new Vector2Int(cell.X, cell.Y);
                if (!m_battle.BattleMap.IntersectsWall(fromPosition, to))
                {
                    buffer.Add(cell);
                }
            });
        }
    }
}