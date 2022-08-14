using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms.Logic;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class AIController
    {
        public LivingEntity Entity;
        public Battle Battle => Entity.Battle;
        public AIBehavior Behavior;
        
        public void Attach(LivingEntity entity, AIBehavior behavior)
        {
            Entity = entity;
            Entity.AIController = this;
            Behavior = behavior;
        }

        public void Start()
        {
            Debug.LogWarning("AI:Start");
            Battle.BattleUnity.battleMapCellController.ClearAll();
            Behavior.Positioning.Start(Battle, this);
        }

        public void End()
        {
            Debug.LogWarning("AI:End");
            Battle.BattleUnity.battleMapCellController.ClearAll();
            Behavior.Positioning.Start(Battle, this);
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