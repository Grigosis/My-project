using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using RPGFight;
using RPGFight.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class MovementTargetSelector : ATargetSelector
    {
        public override void Update()
        {
            base.Update();
            if ((int)m_caster.GameObjectLink.transform.localPosition.x != lastX || (int)m_caster.GameObjectLink.transform.localPosition.z != lastY)
            {
                Do();
            }
        }

        private int lastX = 0; 
        private int lastY = 0; 
        private void Do()
        {
            lastX = (int)m_caster.GameObjectLink.transform.localPosition.x;
            lastY = (int)m_caster.GameObjectLink.transform.localPosition.z;
            m_controller.ClearAll();
            var cells = DI.GetCellsAvailableForMovement(m_battle.BattleMap, new Vector2Int(lastX, lastY), Balance.GetMovepoints(m_caster));
            var from = new Vector2Int(lastX, lastY);
            foreach (var move in cells)
            {
                var to = new Vector2Int(move.X, move.Y);
                if (!m_battle.BattleMap.IntersectsWall(from, to))
                {
                    var mapCellWrapper = m_controller.GetOrCreate(new Vector2Int(move.X, move.Y));
                    mapCellWrapper.from = from;
                    var _renderer = mapCellWrapper.gameObject.GetComponentInChildren<Renderer>();
                    _renderer.material.color = Color.gray;
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