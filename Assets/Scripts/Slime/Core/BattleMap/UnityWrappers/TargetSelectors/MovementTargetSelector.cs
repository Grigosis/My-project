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
    public class MovementTargetSelector : ATargetSelector
    {
        public override void Do()
        {
            m_controller.ClearAll();
            var cells = DI.GetCellsAvailableForMovement(m_battle.BattleMap, new Vector2Int(lastX, lastY), Balance.GetMovepoints(m_caster));
            var from = new Vector2Int(lastX, lastY);
            foreach (var move in cells)
            {
                m_controller.GetOrCreate(move.Position, from, Color.gray);
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
            Do();
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