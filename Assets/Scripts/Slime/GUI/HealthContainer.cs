using SecondCycleGame;
using UnityEngine;

namespace ROR.Core
{
    public class HealthContainer : MonoBehaviour{
        public SimpleBar HealthBar;
        public SimpleBar ActionBar;
        public BattleLivingEntity Target;

        void Update()
        {
            var stats = Target?.LivingEntity?.FinalStats;
            if (stats == null)
            {
                //Debug.Log($"NULL {Target == null} {Target?.LivingEntity == null} {Target?.LivingEntity?.FinalStats== null}");
                return;    
            }
            
            HealthBar?.SetValues(stats.HP_NOW, stats.HP_MAX);
            ActionBar?.SetValues(stats.EP_NOW, stats.EP_MAX);
        }
    }
}
