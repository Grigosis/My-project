using Assets.Scripts.Slime.Core.BattleMap;
using ROR.Core;
using RPGFight.Core;
using UnityEngine;

namespace SecondCycleGame
{
    public class Battle
    {
        public const int FramesInTurn = 100;
        public LivingEntity CurrentLivingEntityTurn;
        public BattleMap BattleMap;
        public Timeline Timeline = new Timeline(FramesInTurn*10);

        
        public void SetCurrentPlayer(LivingEntity currentEntity)
        {
            CurrentLivingEntityTurn = currentEntity;
        }
        
        public void NextPlayerTurn()
        {
            while (true)
            {
                Debug.Log("NextPlayerTurn");
                var act = Timeline.SimulateOneAction();
                if (act != null)
                {
                    break;
                }

                if (act.Type == TimelineActions.PLAYER_TURN)
                {
                    break;
                }
            }
            
        }
        
        
    }
}
