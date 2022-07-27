using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;

namespace Assets.Scripts.Slime.Core.BattleMap.Logic
{
    public interface TargetSelector
    {
        public List<SkillTarget> GetAllPossibleTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity);
    }
}