using System;
using System.Collections.Generic;
using Assets.Scripts.MyUnity;
using ROR.Core.Serialization;
using RPGFight;
using RPGFight.Core;
using UnityEngine;

namespace ROR.Core
{
    public class SkillEnitity
    {
        public SkillDefinition Definition;
        
    }
    public class LivingEntity : IEffectable, IEntity
    {
        public long EntityId { get; set; }

        public int Team;
        public EffectBar EffectBar { get; private set; } = new EffectBar();
        
        public Attrs FinalStats = new Attrs();
        private Attrs SumAttrs = new Attrs();

        public SkillEnitity[] Skills = new SkillEnitity[0];
        

        private Attrs BaseStats;
        private Attrs UpgradedAttrs;
        private Attrs EquipmentAttrs;
        
        
        public bool IsImmune = false;
        public bool IsDead = false;
        public string DebugName = "";

        public IEntityController Controller;

        public GameObject GameObjectLink;
        
        public LivingEntity()
        {
            FinalStats = new Attrs();
            EffectBar.Init(this);
            Entities.Add(this);

            FinalStats.HP_MAX = 10;
            FinalStats.HP_NOW = 5;
            FinalStats.EP_MAX = 5;
            FinalStats.EP_NOW = 3;
            
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
                DI.CreateFloatingText(this, $"{amount}", Color.green);
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

                var txt = $"{damage.Damage:F0}";
                DI.CreateFloatingText(this, txt, Attrs.GetElementColor(damage.Id));
                
                if (FinalStats.HP_NOW <= 0)
                {
                    Die();
                }
            };
        }


        public void InitFromDefinition(Attrs baseAttrs, Attrs upgradedAttrs, Attrs equipmentAttrs, SkillDefinition[] skillDefinitions)
        {
            BaseStats = baseAttrs;
            UpgradedAttrs = upgradedAttrs;
            EquipmentAttrs = equipmentAttrs;
            

            SumAttrs.Sum(baseAttrs);
            SumAttrs.Sum(upgradedAttrs);
            SumAttrs.Sum(equipmentAttrs);
            
            FinalStats = new Attrs(SumAttrs);
            FinalStats.CalculateFinalValues();

            Skills = new SkillEnitity[skillDefinitions.Length];
            for (var i = 0; i < skillDefinitions.Length; i++)
            {
                var sd = skillDefinitions[i];
                Skills[i] = new SkillEnitity();
                Skills[i].Definition = sd;
            }
        }

        public override string ToString()
        {
            return $"LivingEntity{EntityId}/{DebugName} Dead={IsDead} Immune={IsImmune}\r\n{FinalStats}";
        }
    }
}