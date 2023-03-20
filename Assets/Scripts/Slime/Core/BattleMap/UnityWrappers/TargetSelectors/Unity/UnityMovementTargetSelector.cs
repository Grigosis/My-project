using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using Assets.Scripts.Slime.Sugar;
using ROR.Core;
using RPGFight;
using RPGFight.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class UnityMovementTargetSelector : ATargetSelector
    {
        protected override void Do()
        {
            m_controller.ClearAll();
            foreach (var move in allAvailableTargets)
            {
                if (move is BattleMapCell cell)
                {
                    m_controller.GetOrCreate(cell.Position, new Vector2Int(lastX, lastY), Color.gray);
                }
            }
        }

        public override void OnMouseEnterProxy(GameObject gameObject)
        {
            base.OnMouseEnterProxy(gameObject);
            
            var wrapper = gameObject.GetComponentInParent<MapCellWrapper>();
            if (wrapper != null)
            {
                var path = DI.GetPath(m_battle.BattleMap, new Vector2Int(lastX, lastY), new Vector2Int(wrapper.X, wrapper.Y),  Balance.GetMovepoints(m_caster), diagonalPenalty:true);

                var from = new Vector2Int(lastX, lastY);
                foreach (var cells in m_controller.AllCells)
                {
                    cells.Value.Color(Color.gray);
                }

                foreach (var i in path)
                {
                    m_controller.GetOrCreate(i, from, Color.green);
                }
            }

            
        }

        public override void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            base.BeginSelection(controller, battle, caster, skillEntity);
            FindAndDo();
        }

        protected override ISkillTarget GetSkillTarget(GameObject gameObject)
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