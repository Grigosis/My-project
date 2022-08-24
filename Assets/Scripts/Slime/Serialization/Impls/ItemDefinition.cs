using UnityEngine;

namespace ROR.Core.Serialization
{
    [CreateAssetMenu(fileName = "Item", menuName = "GameItems/Items/Basic", order = 51)]
    public class ItemDefinition : BaseDefinition
    {
        public int ItemTypes;
        public int StackSize;
        public float Weight;
        public bool Sellable;
        public bool Storable;
        public bool Equipable;
        public int GearFlags;
    }
}