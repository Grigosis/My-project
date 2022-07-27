using System;
using System.Collections.Generic;
using ROR.Core;
using ROR.Core.Serialization;
using UnityEngine;
using Random = System.Random;

namespace RPGFight.Core
{
    public class Balance
    {
        public static void UseDamageSkill(LivingEntity dealer, LivingEntity receiver, SkillDefinition definition, Random seed = null)
        {
            seed ??= new Random();

            var hit = CalculateHit(dealer, receiver, definition);
            if (seed.NextDouble() < 0)
            {
                DI.CreateFloatingText(dealer, $"MISS [{hit*100:F0}]", Color.red);
                return;
            }

            var damages = CalculateDamage(dealer.FinalStats, receiver.FinalStats, definition.Attacks);
            receiver.Damage(damages, dealer, seed);
        }

        

        private static List<ElementDamage> CalculateDamage(Attrs dealer, Attrs receiver, ElementAttack[] attacks)
        {
            List<ElementDamage> outAttacks = new List<ElementDamage>();
            foreach (var attack in attacks)
            {

                var id = Attrs.GetElementId(attack.Name);
                var dealerEl = dealer.GetElement(id);
                var receiverEl = receiver.GetElement(id);
                var dmg = CalculateDamage(id, dealerEl, receiverEl);
                outAttacks.Add(dmg);
            }

            return outAttacks;
        }

        public static double CalculateHit(LivingEntity dealer, LivingEntity receiver, SkillDefinition definition)
        {
            double hit = CalculateHit (dealer.FinalStats, receiver.FinalStats, definition);
            return hit;
        }
        
        private static double CalculateHit(Attrs dealer, Attrs receiver, SkillDefinition definition)
        {
            var hitChance = definition.IsRangedAttack ? dealer.HIT_FAR_RANGE : receiver.HIT_CLOSE_RANGE;
            var dodgeChance = definition.IsRangedAttack ? dealer.DODGE_FAR_RANGE : receiver.DODGE_CLOSE_RANGE;
            var value = ((1+hitChance)*definition.HitChanceMlt-dodgeChance);
            return value;
        }

        public static ElementDamage CalculateDamage(int id, Element attack, Element defence)
        {
            var min = attack.ATK_MIN_ABS * (1 + attack.ATK_MIN_MLT);
            var max = attack.ATK_MAX_ABS * (1 + attack.ATK_MAX_MLT);
            var dmgMin = min * (1 + defence.DEF_MLT) - defence.DEF_ABS;
            var dmgMax = max * (1 + defence.DEF_MLT) - defence.DEF_ABS;
            dmgMin = Math.Max(1, dmgMin);
            dmgMax = Math.Max(dmgMin, dmgMax);

            return new ElementDamage(id, dmgMin, dmgMax);
        }

        public static void CalculateFinalValues(Attrs attrs)
        {
            attrs.HP_MAX += attrs.STATS.STR * 10;
            attrs.EP_MAX += attrs.STATS.AGI / 5;
            attrs.CRIT_CHANCE += attrs.STATS.PER / 5;
        }

        public static float GetMovepoints(LivingEntity caster)
        {
            return (float)(caster.FinalStats.EP_NOW * (1 + caster.FinalStats.MOVESPEED_MLT)) + 3;
        }
    }
}