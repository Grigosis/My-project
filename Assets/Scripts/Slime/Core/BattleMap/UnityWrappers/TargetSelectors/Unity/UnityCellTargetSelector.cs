using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class UnityCellTargetSelector : ATargetSelector
    {
        protected override void Do()
        {
            var from = m_caster.Cell.Position;
            foreach (var target in allAvailableTargets)
            {
                if (target is BattleMapCell cell)
                {
                    var to = new Vector2Int(cell.X, cell.Y);
                    m_controller.GetOrCreate(to, from, Color.gray);
                }
            }
        }

        public override void OnMouseEnterProxy(GameObject gameObject)
        {
            base.OnMouseEnterProxy(gameObject);
            
            
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