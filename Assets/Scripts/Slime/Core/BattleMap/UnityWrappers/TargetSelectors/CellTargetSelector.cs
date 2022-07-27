using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public class UnitTargetSelector : ATargetSelector
    {
        protected override SkillTarget GetSkillTarget(GameObject gameObject)
        {
            return gameObject.GetComponentInParent<BattleLivingEntity>()?.LivingEntity;
        }
    }
}