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
            if (seed.NextDouble() > hit)
            {
                DI.CreateFloatingText(dealer, $"MISS [{hit*100:F0}]", Color.red);
                return;
            }
            var critChance = CalculateCritChance(dealer, receiver, definition);
            bool crit = seed.NextDouble() > critChance;


            var damages = CalculateDamage(dealer.FinalStats, receiver.FinalStats, definition.Attacks, crit);
            receiver.Damage(damages, dealer, seed);
        }

        private static double CalculateCritChance(LivingEntity dealer, LivingEntity receiver, SkillDefinition definition)
        {
            return dealer.FinalStats.CRIT_CHANCE;
        }


        private static List<ElementDamage> CalculateDamage(Attrs dealer, Attrs receiver, ElementAttack[] attacks, bool isCritDamage)
        {
            List<ElementDamage> outAttacks = new List<ElementDamage>();
            foreach (var attack in attacks)
            {
                var id = Attrs.GetElementId(attack.Name);
                var dealerEl = dealer.GetElement(id);
                var receiverEl = receiver.GetElement(id);
                var dmg = CalculateDamage(id, dealerEl, receiverEl);
                if (isCritDamage)
                {
                    dmg.Min *= 1 + dealer.CRIT_MLT;
                    dmg.Max *= 1 + dealer.CRIT_MLT;
                    dmg.IsCrit = true;
                }
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
            var hitChance = definition.IsRangedAttack ? dealer.HIT_FAR_RANGE : dealer.HIT_CLOSE_RANGE;
            var dodgeChance = definition.IsRangedAttack ? receiver.DODGE_FAR_RANGE : receiver.DODGE_CLOSE_RANGE;
            
            var value = ((1+hitChance)*definition.HitChanceMlt-dodgeChance);
            Debug.Log($"CaclHit {value}:{1+hitChance} * {definition.HitChanceMlt} / {dodgeChance}");
            return value;
        }

        public static ElementDamage CalculateDamage(int id, Element attack, Element defence)
        {
            var min = attack.ATK_MIN_ABS * (1 + attack.ATK_MIN_MLT);
            var max = attack.ATK_MAX_ABS * (1 + attack.ATK_MAX_MLT);
            var dmgMin = min * (1 - defence.DEF_MLT) - defence.DEF_ABS;
            var dmgMax = max * (1 - defence.DEF_MLT) - defence.DEF_ABS;
            dmgMin = Math.Max(1, dmgMin);
            dmgMax = Math.Max(dmgMin, dmgMax);

            return new ElementDamage(id, dmgMin, dmgMax);
        }

        public static void CalculateFinalValues(Attrs attrs)
        {
            attrs.HP_MAX += attrs.STATS.END * 10;
            attrs.EP_MAX += attrs.STATS.AGI;
            attrs.CRIT_CHANCE += attrs.STATS.PER / 5;
        }

        public static float GetMovepoints(LivingEntity caster)
        {
            return (float)(caster.FinalStats.EP_NOW * (1 + caster.FinalStats.MOVESPEED_MLT)) + 3;
        }
    }
}