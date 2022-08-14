using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms.Data;
using Assets.Scripts.Slime.Core.BattleMap.Logic;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms.Fx
{
    public class AIFunctions
    {
        [FxAttribute("MinValue")]
        [FxAttribute("MaxValue")]
        public static float StayInRange(Battle battle, AIController controller, BattleMapCell selector,  Dictionary<string, FxParam> args)
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
        
        public static float StayInRangeOfSkill(Battle battle, AIController controller, BattleMapCell selector,Dictionary<string, FxParam> args)
        {
            return 1f;
        }
        
        public static float UseCovers(Battle battle, AIController controller, BattleMapCell selector, Dictionary<string, FxParam> args)
        {
            return (float)0f;
        }
    }
}