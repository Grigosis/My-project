using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Slime.Core.Skills
{
    public class DefaultMove : ISkillImplementation
    {
        public void CastSkill(SkillEntity skillEntity, List<ISkillTarget> targets, Random seed)
        {
            var entity = skillEntity.Owner;
            var cell = (BattleMapCell)targets[0];

            if (cell == null)
            {
                Debug.LogError($"Cell is == [{targets[0]}]");
                return;
            }

            entity.Cell.Entity = null;
            entity.Cell = cell;
            entity.Cell.Entity = entity;
            entity.GameObjectLink.transform.localPosition = entity.Battle.BattleUnity.battleMapCellController.GetCellPosition(cell.X, cell.Y);
            
        }
    }
}