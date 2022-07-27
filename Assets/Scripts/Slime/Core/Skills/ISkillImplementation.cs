using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;

namespace Assets.Scripts.Slime.Core.Skills
{
    public interface ISkillImplementation
    {
        void CastSkill(SkillEntity skillEntity, List<SkillTarget> targets, Random seed);
    }
}