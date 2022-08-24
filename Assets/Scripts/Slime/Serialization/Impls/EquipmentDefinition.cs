using RPGFight;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [CreateAssetMenu(fileName = "Item", menuName = "GameItems/Items/Equipable", order = 51)]
    public class EquipmentDefinition : ItemDefinition
    {
        public Attrs Attributes = new Attrs();
    }
}