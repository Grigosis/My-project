using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using RPGFight;
using RPGFight.Core;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class DebugTargetSelector : ATargetSelector
    {
        public override void Do()
        {
            m_controller.ClearAll();
            
            var from = new Vector2Int(lastX, lastY);
            var cells = DI.GetCellsAvailableForMovement(m_battle.BattleMap, from, Balance.GetMovepoints(m_caster)+100);
            foreach (var move in cells)
            {
                var to = new Vector2Int(move.X, move.Y);
                if (!m_battle.BattleMap.IntersectsWall(from, to))
                {
                    m_controller.GetOrCreate(to, from, Color.gray);
                }
                else
                {
                    m_controller.GetOrCreate(to, from, Color.red);
                }
            }

            foreach (var wall in m_battle.BattleMap.Walls)
            {
                foreach (var p in wall.Points)
                {
                    m_controller.GetOrCreate(new Vector2Int((int)p.x, (int)p.y), from, Color.blue);
                }
            }
            
            m_controller.GetOrCreate(from, from, Color.green);
        }

        public override void OnMouseEnter(GameObject gameObject)
        {
            var wrapper = gameObject.GetComponentInChildren<MapCellWrapper>();
            SetXY(wrapper.X, wrapper.Y);
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