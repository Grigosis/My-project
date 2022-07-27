using ROR.Core;
using ROR.Core.Serialization;
using UnityEngine;

namespace RPGFight.Core
{
    public class Builder
    {
        
        public static LivingEntity Build(LivingEntityDefinition stateInBattle)
        {
            var livingEntity = new LivingEntity();
            var attrs = new Attrs();
            foreach (var equippedItem in stateInBattle.EquippedItems)
            {
                var equipmentDefinition = D.Instance.Get<EquipmentDefinition>(equippedItem);
                Debug.Log(equippedItem+"::::"+equipmentDefinition.Attributes);
                attrs.Sum(equipmentDefinition.Attributes);
            }

            var skillDefinitions = new SkillDefinition[stateInBattle.Skills.Length];
            for (int i = 0; i < stateInBattle.Skills.Length; i++)
            {
                skillDefinitions[i] = D.Instance.Get<SkillDefinition>(stateInBattle.Skills[i]);
            }
            
            livingEntity.InitFromDefinition(stateInBattle.Icon, stateInBattle.BaseAttributes, stateInBattle.UpgradedAttributes, attrs, skillDefinitions);
            return livingEntity;
        }
    }
}