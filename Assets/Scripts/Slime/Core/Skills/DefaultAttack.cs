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
        public void PredictFuture(SkillEntity skillEntity, List<ISkillTarget> targets, Random seed)
        {
            var dealer = skillEntity.Owner;
            var future = new Future();
            foreach (var target in targets)
            {
                if (target is LivingEntity receiver)
                {
                    //var hit = new HitCondition();
                    //var damage = new DamageFuture();
                    //damage.Effect = new HealingEffectEntity();
                    var damages = Balance.PredictFuture(dealer, receiver, skillEntity.Definition);
                    //hit.Nodes.Add(damage);
                    //future.Nodes.Add(hit);
                }
            }
        }

        public void ExecuteFuture(Future future, bool animate)
        {
            future.Execute();
        }

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