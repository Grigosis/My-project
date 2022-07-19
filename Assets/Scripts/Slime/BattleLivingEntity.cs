using System;
using ROR.Core;
using ROR.Core.Serialization;
using RPGFight;
using RPGFight.Core;
using UnityEngine;

namespace SecondCycleGame
{
    public class BattleLivingEntity : MonoBehaviour, IMouseReceiver, IAttributeProvider
    {
        public LivingEntity LivingEntity;


        public String DefintionId;
        // Start is called before the first frame update
        void Awake()
        {
            if (DefintionId != null)
            {
                var stateInBattle = D.Instance.Get<LivingEntityDefinition>(DefintionId);
                LivingEntity = Builder.Build(stateInBattle);
            }
            else
            {
                LivingEntity = new LivingEntity();
            }
            
            
            LivingEntity.GameObjectLink = gameObject;
        }


        private FloatingText DebugInfo;
        
        public void OnMouseEnter(GameObject gameObject)
        {
            
        }

        public void OnMouseOver(GameObject gameObject)
        {
            if (DebugInfo != null)
            {
                Destroy(DebugInfo.gameObject);
                DebugInfo = null;
            }

            DebugInfo = FloatingText.CreateStaticText(transform.position + transform.up * 8, LivingEntity.ToString(), Color.black, textSize:6f);
        }

        public void OnMouseExit(GameObject gameObject)
        {
            if (DebugInfo != null)
            {
                Destroy(DebugInfo.gameObject);
                DebugInfo = null;
            }
        }

        public void OnMouseDown(GameObject gameObject)
        {
            var cmp = gameObject.GetComponentInParent<BattleLivingEntity>();
            if (cmp != null)
            {
                UseSkill(cmp);
            }
        }

        private void UseSkill(BattleLivingEntity other)
        {
            Balance.UseDamageSkill(this, other, LivingEntity.Skills[0].Definition);
        }

        public void OnMouseUp(GameObject gameObject)
        {
            
        }

        public Attrs GetAttributes()
        {
            return LivingEntity.FinalStats;
        }

        public Vector3 GetHeadPosition()
        {
            return Vector3.back;
        }
        
        public Vector3 GetFeetPosition()
        {
            return Vector3.back;
        }
        
        public Vector3 GetTorsoPosition()
        {
            return Vector3.back;
        }
    }
}
