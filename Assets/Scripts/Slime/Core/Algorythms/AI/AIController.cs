using Assets.Scripts.Slime.Core.Algorythms.Logic;
using ROR.Core;
using SecondCycleGame;
using Debug = UnityEngine.Debug;

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
            
            using (new Measure("AI Positioning"))
            {
                Behavior.Positioning.Start(Battle, this);
            }
            
        }

        public void End()
        {
            Debug.LogWarning("AI:End");
            Battle.BattleUnity.battleMapCellController.ClearAll();
            Behavior.Positioning.Start(Battle, this);
        }

        public void Update()
        {
            
        }
    }
}