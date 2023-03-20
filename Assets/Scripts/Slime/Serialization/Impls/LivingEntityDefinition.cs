using Assets.Scripts.Slime.Core.Algorythms;
using RPGFight;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [CreateAssetMenu(fileName = "Character", menuName = "GameItems/Character", order = 51)]
    public class LivingEntityDefinition : BaseDefinition
    {
        /// <summary>
        /// То что игрока по умолчанию. 
        /// </summary>
        public Attrs BaseAttributes = new Attrs();
        
        /// <summary>
        /// То что игрок сам вкачивал во время игры. 
        /// </summary>
        public Attrs UpgradedAttributes = new Attrs();
        
        
        public AIBehaviorDefinition AI;

        public SkillDefinition[] Skills;
        
        public EquipmentDefinition[] EquippedItems;
    }
}