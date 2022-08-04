using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class UnitTargetSelector : ATargetSelector
    {
        //protected override SkillTarget GetSkillTarget(GameObject gameObject)
        //{
        //    return gameObject.GetComponentInParent<BattleLivingEntity>()?.LivingEntity;
        //}
        public override void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            base.BeginSelection(controller, battle, caster, skillEntity);
            Do();
        }

        protected override SkillTarget GetSkillTarget(GameObject gameObject)
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
        
        
        
        public override void Do()
        {
            m_controller.ClearAll();
            var cells = m_battle.getAllLivingEntities();
            var from = new Vector2Int(lastX, lastY);
            foreach (var move in cells)
            {
                var to = new Vector2Int(move.Cell.X, move.Cell.Y);
                if (!m_battle.BattleMap.IntersectsWall(from, to))
                {
                    m_controller.GetOrCreate(to, from, Color.gray);
                }
            }
        }

        
    }
}