using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class UnityUnitTargetSelector : ATargetSelector
    {
        protected override ISkillTarget GetSkillTarget(GameObject gameObject)
        {
            var wrapper = gameObject.GetComponentInParent<BattleLivingEntity>();
            if (wrapper != null)
            {
                var to = wrapper.LivingEntity.Cell.Position;
                if (!m_battle.BattleMap.IntersectsWall(new Vector2Int(m_caster.Cell.X, m_caster.Cell.Y), to))
                {
                    return wrapper?.LivingEntity;
                }
            }
            return null;
        }

        protected override void Do()
        {
            m_controller.ClearAll();
            foreach (var target in allAvailableTargets)
            {
                if (target is BattleMapCell cell)
                {
                    m_controller.GetOrCreate(cell.Position, new Vector2Int(lastX, lastY), Color.gray);
                }
            }
        }

        
    }
}