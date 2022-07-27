using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using RPGFight.Core;

namespace Assets.Scripts.Slime.Core.Skills
{
    public class DefaultAttack : ISkillImplementation
    {
        public void CastSkill(SkillEntity skillEntity, List<SkillTarget> targets, Random seed)
        {
            var entity = skillEntity.Owner;

            foreach (var target in targets)
            {
                if (target is LivingEntity livingEntity)
                {
                    Balance.UseDamageSkill(skillEntity.Owner, livingEntity, skillEntity.Definition, seed);
                }
            }
            
        }
    }
}