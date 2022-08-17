using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using RPGFight.Core;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Slime.Core.Skills
{
    public class DefaultAttack : ISkillImplementation
    {
        public void CastSkill(SkillEntity skillEntity, List<ISkillTarget> targets, Random seed)
        {
            var entity = skillEntity.Owner;

            foreach (var target in targets)
            {
                if (target is LivingEntity livingEntity)
                {
                    Balance.UseDamageSkill(skillEntity.Owner, livingEntity, skillEntity.Definition, seed);

                    EffectState es = new EffectState();
                    es.Caster = entity;
                    es.Target = livingEntity;
                    es.EffectDefinition = "Effect/Slime-SimpleHeal";//D.Instance.Get<EffectDefinition>();
                    
                    var effect = new HealingEffectEntity();
                    effect.Init(es);
                    livingEntity.EffectBar.AddEffect(effect);
                    
                    Debug.LogWarning("Add effect");
                }
            }
            
        }
    }
}