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

            if (!CalculateHit(dealer.FinalStats, receiver.FinalStats, definition, seed))
            {
                DI.CreateFloatingText(dealer, "MISS", Color.red);
                return;
            }

            var damages = CalculateDamage(dealer.FinalStats, receiver.FinalStats, definition.Attacks, seed);
            receiver.Damage(damages, dealer);
        }

        private static bool CalculateHit(Attrs dealer, Attrs receiver, SkillDefinition definition, Random seed)
        {
            var hitChance = definition.IsRangedAttack ? dealer.HIT_FAR_RANGE : receiver.HIT_CLOSE_RANGE;
            var dodgeChance = definition.IsRangedAttack ? dealer.DODGE_FAR_RANGE : receiver.DODGE_CLOSE_RANGE;
            var value = seed.NextDouble()*((1+hitChance)*definition.HitChanceMlt-dodgeChance);
            return value > 0;
        }

        private static List<ElementDamage> CalculateDamage(Attrs dealer, Attrs receiver, ElementAttack[] attacks, Random seed)
        {


            List<ElementDamage> outAttacks = new List<ElementDamage>();
            foreach (var attack in attacks)
            {

                var id = Attrs.GetElementId(attack.Name);
                var dealerEl = dealer.GetElement(id);
                var receiverEl = receiver.GetElement(id);
                var dmg = CalculateDamage(dealerEl, receiverEl, seed);

                outAttacks.Add(new ElementDamage(id, dmg));
            }

            return outAttacks;
        }

        public static double CalculateDamage(Element attack, Element defence, Random seed)
        {
            var min = attack.ATK_MIN_ABS * (1 + attack.ATK_MIN_MLT);
            var max = attack.ATK_MAX_ABS * (1 + attack.ATK_MAX_MLT);
            var dmg = seed.Between(min, max) * (1 + defence.DEF_MLT) - defence.DEF_ABS;
            dmg = Math.Max(1, dmg);
            return dmg;
        }

        public static void CalculateFinalValues(Attrs attrs)
        {
            attrs.HP_MAX += attrs.STATS.STR * 10;
            attrs.EP_MAX += attrs.STATS.AGI / 5;
            attrs.CRIT_CHANCE += attrs.STATS.PER / 5;
        }
    }
}