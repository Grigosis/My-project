using UnityEngine;

namespace ROR.Core
{
    public class HealthContainer : MonoBehaviour{

        public SimpleBar healthBar;
        public SimpleBar ShieldBar;
        public LivingEntity target;
        void Start(){
            healthBar.setColor(Color.red);
            if (ShieldBar != null) ShieldBar.setColor(Color.blue);
        }

        void Update(){
            //Debug.Log(target.HP_NOW);
            if(healthBar.maxValue != target.FinalStats.HP_MAX){
                healthBar.maxValue = target.FinalStats.HP_MAX;
            }
            healthBar.setValue(target.FinalStats.HP_NOW);

            if (ShieldBar != null)
            {
                if(ShieldBar.maxValue != target.FinalStats.EP_MAX){
                    ShieldBar.maxValue = target.FinalStats.EP_MAX;
                    ChangeShieldBarEnabled(target.FinalStats.EP_MAX > 0);
                }
                if(ShieldBar.gameObject.activeSelf){
                    ShieldBar.setValue(target.FinalStats.EP_NOW);
                }
            }
        }

        private void ChangeShieldBarEnabled(bool enabled){
            ShieldBar.gameObject.SetActive(enabled);
        }
    }
}
