using System;
using System.Collections.Generic;
using Assets.Scripts.MyUnity;
using RPGFight;
using RPGFight.Core;

namespace ROR.Core
{
    public class LivingEntity : IEffectable, IEntity
    {
        public long EntityId { get; set; }

        public int Team;
        public EffectBar EffectBar { get; private set; } = new EffectBar();
        public Attrs FinalStats;
        
        public bool IsImmune = false;
        public bool IsDead = false;
        public string DebugName = "";

        public IEntityController Controller;
        public LivingEntity()
        {
           
        }
        
        protected virtual void Start()
        {
            Entities.Add(this);
            
            FinalStats = Attrs.NewWithSubs();
            EffectBar.Init(this);
        }
        
        void Update()
        {
            EffectBar.Update(1);
        }

        public event Action<LivingEntity> OnDeath; 
        
        public void Die()
        {
            IsDead = true;
            OnDeath?.Invoke(this);
            //Destroy(gameObject);
            
            Entities.Remove(this);
        }

        public void Heal(float amount, object from)
        {
            if (amount > 0)
            {
                FinalStats.HP_NOW = Math.Min(FinalStats.HP_NOW + amount, FinalStats.HP_MAX);
            }
        }

        public void Damage(List<ElementDamage> damages, object from)
        {
            for (int i = 0; i < damages.Count; i++)
            {
                Damage(damages[i], from);
            }
        }

        public void Damage(ElementDamage damage, object from)
        {
            if (IsImmune) return;
            if (IsDead) return;


            if (damage.Damage > 0)
            {
                FinalStats.HP_NOW = FinalStats.HP_NOW - damage.Damage;

                DI.CreateFloatingText(this, $"{damage}", Attrs.GetElementColor(damage.Id));

                if (FinalStats.HP_NOW <= 0)
                {
                    Die();
                }
            }
        }


        public void InitAttrs(Attrs attributes)
        {
            FinalStats = Attrs.NewWithSubs();
            FinalStats.Sum(attributes);
        }

        public override string ToString()
        {
            return $"LivingEntity{EntityId}/{DebugName} Dead={IsDead} Immune={IsImmune}\r\n{FinalStats}";
        }
    }
}