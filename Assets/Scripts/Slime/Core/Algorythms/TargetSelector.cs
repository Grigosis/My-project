using System.Collections.Generic;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{

    public class TargetSelectorUnitWrapper : MonoBehaviour
    {
        private List<SkillTarget> GetSelectedTargets;

        public void BeginSelect(Battle battle, LivingEntity caster, SkillEntity entity)
        {
            
        }

        public void EndSelect()
        {
            
        }
    }

    
    public interface TargetSelector
    {
        public List<SkillTarget> GetAllPossibleTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity);
    }
    public interface SkillTarget
    {
        
    }


    
    public class CellSelector
    {
        public List<SkillTarget> GetAllSkillTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            var cell = caster.Cell;
            var range = skillEntity.GetRange();

           
            List<SkillTarget> listOfAllVisibleCells = new List<SkillTarget>();
            battle.BattleMap.Foreach(cell, range, (cell) =>
            {
                if (cell.IsVisibleFrom(cell))
                {
                    listOfAllVisibleCells.Add(cell);
                }
            });

            return listOfAllVisibleCells;
        }
    }
    public class CharacterSelector
    {
        public List<SkillTarget> GetAllSkillTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            var range = skillEntity.GetRange();
            List<SkillTarget> listOfAllVisibleCells = new List<SkillTarget>();
            foreach (var livingEntity in battle.BattleMap.AllLivingEntities)
            {
                if (livingEntity.Cell.DistanceTo(caster.Cell) > range)
                {
                    continue;
                }
                
                if (livingEntity.Cell.IsVisibleFrom(caster.Cell))
                {
                    listOfAllVisibleCells.Add(livingEntity);
                }
            }

            return listOfAllVisibleCells;
        }
    }
    
    
    
    
}