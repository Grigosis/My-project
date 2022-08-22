using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Core.BattleMap;
using Assets.Scripts.Slime.Core.Skills;
using ROR.Core;
using ROR.Core.Serialization;
using UnityEngine;
using Random = System.Random;

namespace RPGFight.Core
{
    public class Balance
    {
        
        public static Future PredictFuture (LivingEntity dealer, LivingEntity receiver, SkillDefinition definition) {
            var hit = new HitCondition();
            hit.HitChance = CalculateHit(dealer, receiver, definition);;
            var damage = new DamageFuture();
            damage.CritChance = CalculateCritChance(dealer, receiver, definition);
            damage.Damage = CalculateDamage(dealer.FinalStats, receiver.FinalStats, definition.Attacks);
            hit.Nodes.Add(damage);
            return hit;
        }

        
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


            var damages = CalculateDamage(dealer.FinalStats, receiver.FinalStats, definition.Attacks);
            foreach (var elementDamage in damages)
            {
                elementDamage.IsCrit = crit;
            }
            receiver.Damage(damages, dealer, seed);
        }

        private static double CalculateCritChance(LivingEntity dealer, LivingEntity receiver, SkillDefinition definition)
        {
            return dealer.FinalStats.CRIT_CHANCE;
        }


        public static List<ElementDamage> CalculateDamage(Attrs dealer, Attrs receiver, ElementAttack[] attacks)
        {
            List<ElementDamage> outAttacks = new List<ElementDamage>();
            foreach (var attack in attacks)
            {
                var id = Attrs.GetElementId(attack.Name);
                var dealerEl = dealer.GetElement(id);
                var receiverEl = receiver.GetElement(id);
                var dmg = CalculateDamage(id, dealer, dealerEl, receiverEl);
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

        public static ElementDamage CalculateDamage(int id, Attrs dealer, Element attack, Element defence)
        {
            var min = attack.ATK_MIN_ABS * (1 + attack.ATK_MIN_MLT);
            var max = attack.ATK_MAX_ABS * (1 + attack.ATK_MAX_MLT);
            var dmgMin = min * (1 - defence.DEF_MLT) - defence.DEF_ABS;
            var dmgMax = max * (1 - defence.DEF_MLT) - defence.DEF_ABS;
            var critMin = dmgMin * (1 + dealer.CRIT_MLT);
            var critMax = dmgMax * (1 + dealer.CRIT_MLT);
            dmgMin = Math.Max(1, dmgMin);
            dmgMax = Math.Max(dmgMin, dmgMax);
            return new ElementDamage(id, dmgMin, dmgMax, critMin, critMax);
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
        
        public static float GetCoverHitMultiplicator(BattleMap battleMap, Vector2Int from, Vector2Int to)
        {
            if (battleMap.IntersectsWall(from, to))
            {
                return 0;
            }

            var covers = battleMap[to].Covers;
            if (covers == null || covers.Count == 0)
            {
                return 1;
            }
            
            var max = 0f;
            foreach (var cover in covers)
            {
                var result = GetCoverBonus(cover, cover.IsUnderCover(to));
                max = Math.Max(max, result);
            }
            
            return max;
        }

        public static float GetCoverBonus(MapCellCover cover, float result)
        {
            switch (cover.Type)
            {
                case CoverEnum.Large:
                    return 0;
                case CoverEnum.Medium:
                    return result * 0.5f;
                case CoverEnum.Small:
                    return result * 0.75f;
                default:
                    return 1;
            }
        }
    }
}