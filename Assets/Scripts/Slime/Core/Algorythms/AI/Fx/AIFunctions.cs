using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms.Data;
using Assets.Scripts.Slime.Sugar;
using ROR.Core;
using RPGFight.Core;
using SecondCycleGame;

namespace Assets.Scripts.Slime.Core.Algorythms.Fx
{
    public class AIFunctions
    {
        [FxAttribute("MinValue")]
        [FxAttribute("MaxValue")]
        public static float StayInRange(Battle battle, AIController controller, BattleMapCell selector, float moveAP, Dictionary<string, FxParam> args)
        {
            
            var entity = controller.Entity;
            var myPos = selector.Position;
            
            var entities = battle.BattleMap.AllLivingEntities;

            double total = 0;
            foreach (var livingEntity in entities)
            {
                if (livingEntity == entity)
                {
                    continue;
                }
                var distance = (myPos - livingEntity.Position).magnitude;
                total += distance;
            }

            total /= (entities.Count-1);

            return (float)total;
        }

        public static Random random = new Random();
        public static float Random(Battle battle, AIController controller, BattleMapCell selector, float moveAP, Dictionary<string, FxParam> args)
        {
            return (float)random.NextDouble();
        }
        
        public static float StayInRangeOfSkill(Battle battle, AIController controller, BattleMapCell selector, float moveAP, Dictionary<string, FxParam> args)
        {
            return 1f;
        }

        public static float AmountOfSkillCanUse(Battle battle, AIController controller, BattleMapCell selector, float moveAP, Dictionary<string, FxParam> args)
        {
            var stats = controller.Entity.FinalStats;
            var left = (stats.EP_NOW - moveAP).ToIntWithUpperRound();

            var count = controller.Entity.SkillBar.GetAllSkillsAvailableForCast(battle, controller.Entity, selector.Position, left).Count;
            //foreach (var skill in controller.Entity.SkillBar.SkillEntities)
            //{
            //    skill.Definition.TargetSelector.
            //}

            
            return (float)count;
        }
        
        public static float UseCovers(Battle battle, AIController controller, BattleMapCell selector, float moveAP, Dictionary<string, FxParam> args)
        {
            var allVisibleEnemies = battle.BattleMap.GetAllVisibleEnemies(controller.Entity, new List<LivingEntity>());
            if (allVisibleEnemies.Count == 0)
            {
                return 0;
            }
            var sum = 0f;
            foreach (var enemy in allVisibleEnemies)
            {
                sum += Balance.GetCoverHitMultiplicator(battle.BattleMap, enemy.Cell.Position, selector.Position);
            }

            sum /= allVisibleEnemies.Count;
            return 1-sum;
        }
    }
}