using System.Collections.Generic;
using ROR.Core;
using RPGFight;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class AI
    {
        private LivingEntity Entity;
        private Battle Battle;
        
        public void Attach(LivingEntity entity, Battle battle)
        {
            Entity = entity;
            Battle = battle;
        }
        
        

        public void Update()
        {
            //var enemies = Battle.GetEnemies(Entity);
            //var skills = Entity.SkillBar.SkillEntities;
//
            //var movespeed = Entity.GetMaxMoveDistance();
            //var movements = DI.GetCellsAvailableForMovement(Battle.BattleMap, new Vector2Int(Entity.Cell.X, Entity.Cell.Y), movespeed);
            
            //foreach (var m in movements)
            //{
            //    Battle.
            //}
        }

        public void PickEnemy(List<LivingEntity> enemies)
        {
            
        }
    }
}