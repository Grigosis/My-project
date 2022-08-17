using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class UnitySelfTargetSelector : ATargetSelector
    {
        public override void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            base.BeginSelection(controller, battle, caster, skillEntity);
            Invoke(new List<ISkillTarget>() {caster});
        }

        protected override void Do() { }

        protected override ISkillTarget GetSkillTarget(GameObject gameObject)
        {
            return null;
        }
    }
}