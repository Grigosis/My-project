﻿using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class CellSelector
    {
        public List<ISkillTarget> GetAllSkillTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            var cell = caster.Cell;
            var range = skillEntity.GetMinRange();

           
            List<ISkillTarget> listOfAllVisibleCells = new List<ISkillTarget>();
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
        public List<ISkillTarget> GetAllSkillTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            var range = skillEntity.GetMinRange();
            List<ISkillTarget> listOfAllVisibleCells = new List<ISkillTarget>();
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