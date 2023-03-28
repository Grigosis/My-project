using UnityEngine;
using System.IO;

namespace SecondCycleGame
{
    public class ClothArmor : IArmorItem
    {
        private readonly ItemType _type = ItemType.Armor;
        private readonly Sprite _icon = Resources.Load<Sprite>(Path.Combine("Icons", "Icon43"));
        private readonly string _name = "Броня из ткани";
        private readonly float _endurance = 100;
        private readonly float _protection = 5;

        public ItemType Type => _type;
        public Sprite Icon => _icon;
        public string Name => _name;
        public float Endurance => _endurance;
        public float Protection => _protection;
    }
}