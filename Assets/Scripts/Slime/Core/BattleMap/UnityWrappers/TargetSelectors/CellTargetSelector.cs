using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class CellTargetSelector : ATargetSelector
    {
        public override void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            base.BeginSelection(controller, battle, caster, skillEntity);
            
            
            var range = skillEntity.GetRange();
            battle.BattleMap.Foreach(caster.Cell, range, cell => controller.GetOrCreate(new Vector2Int(cell.X, cell.Y)));
        }

        protected override SkillTarget GetSkillTarget(GameObject gameObject)
        {
            var wrapper = gameObject.GetComponentInParent<MapCellWrapper>();
            if (wrapper != null)
            {
                var cell = m_battle.BattleMap[wrapper.X, wrapper.Y];
                if (cell != null)
                {
                    return cell;
                }
            }
            
            return null;
        }
    }
}