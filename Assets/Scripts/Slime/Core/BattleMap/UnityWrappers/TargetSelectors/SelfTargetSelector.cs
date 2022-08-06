using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class SelfTargetSelector : ATargetSelector
    {
        public override void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            base.BeginSelection(controller, battle, caster, skillEntity);
            Invoke(new List<SkillTarget>() {caster});
        }

        public override void Do() { }

        protected override SkillTarget GetSkillTarget(GameObject gameObject)
        {
            return null;
        }
    }
}