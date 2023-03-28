using UnityEngine;

namespace SecondCycleGame
{
    public interface IItem
    {
        public ItemType Type { get; }
        public Sprite Icon { get; }
        public string Name { get; }
        public float Endurance { get; }
    }

    public enum ItemType
    {
        Armor,
        MeleeWeapons,
        RangedWeapons,
    }
}