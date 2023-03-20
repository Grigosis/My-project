using System.Collections.Generic;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    public class SkillBar
    {
        public List<SkillEntity> SkillEntities = new List<SkillEntity>();

        public void Add(SkillEntity skillEntity)
        {
            SkillEntities.Add(skillEntity);
        }
        
        public void Update()
        {
            for (var i = 0; i < SkillEntities.Count; i++)
            {
                var se = SkillEntities[i];
                se.Cooldown--;
            }
        }

        public List<SkillEntity> GetAllSkillsAvailableForCast(Battle battle, LivingEntity caster, Vector2Int fromPosition, int ApLeft)
        {
            var list = new List<SkillEntity>();
            foreach (var skill in SkillEntities)
            {
                if (skill.Definition.AP > ApLeft) continue;
                if (skill.HasGoodTargets(battle, caster, fromPosition))
                {
                    list.Add(skill);
                }
            }

            return list;
        }
    }
}