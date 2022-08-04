using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class CellTargetSelector : ATargetSelector
    {
        public override void Do()
        {
            var from = m_caster.Cell.Position;
            m_battle.BattleMap.Foreach(m_caster.Cell, m_skillEntity.GetMinRange(), m_skillEntity.GetMaxRange(), cell =>
            {
                var to = new Vector2Int(cell.X, cell.Y);
                if (!m_battle.BattleMap.IntersectsWall(from, to))
                {
                    m_controller.GetOrCreate(to, from, Color.gray);
                }
            });
        }

        public override void OnMouseEnter(GameObject gameObject)
        {
            base.OnMouseEnter(gameObject);
            
            
            var wrapper = gameObject.GetComponentInParent<MapCellWrapper>();
            if (wrapper != null)
            {
                foreach (var cells in m_controller.AllCells)
                {
                    cells.Value.Color(Color.gray);
                }
                m_controller.GetOrCreate(new Vector2Int(wrapper.X, wrapper.Y), wrapper.from, Color.green);
            }
            
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