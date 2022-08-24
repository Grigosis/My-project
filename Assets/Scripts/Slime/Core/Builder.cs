using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.Algorythms.Logic;
using ROR.Core;
using ROR.Core.Serialization;

namespace RPGFight.Core
{
    public class Builder
    {
        
        public static LivingEntity Build(LivingEntityDefinition stateInBattle)
        {
            var livingEntity = new LivingEntity();
            var attrs = new Attrs();
            foreach (var equipmentDefinition in stateInBattle.EquippedItems)
            {
                attrs.Sum(equipmentDefinition.Attributes);
            }

            var skillDefinitions = new SkillDefinition[stateInBattle.Skills.Length];
            for (int i = 0; i < stateInBattle.Skills.Length; i++)
            {
                skillDefinitions[i] = stateInBattle.Skills[i];
            }

            if (stateInBattle.AI != null)
            {
                AIBehavior behavior = new AIBehavior(stateInBattle.AI);
                AIController controller = new AIController();
                controller.Attach(livingEntity, behavior);
            }
            
            livingEntity.InitFromDefinition(stateInBattle.Icon, stateInBattle.BaseAttributes, stateInBattle.UpgradedAttributes, attrs, skillDefinitions);
            return livingEntity;
        }
    }
}