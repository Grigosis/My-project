using UnityEngine;
using System.IO;

namespace SecondCycleGame
{
    public class Sword : IMeleWeaponItem
    {
        private readonly ItemType _type = ItemType.MeleeWeapons;
        private readonly Sprite _icon = Resources.Load<Sprite>(Path.Combine("Icons", "Icon32"));
        private readonly string _name = "Меч";
        private readonly float _endurance = 100;
        private readonly float _damage = 5;
        private readonly float _impactSpeed = 10;

        public ItemType Type => _type;
        public Sprite Icon => _icon;
        public string Name => _name;
        public float Endurance => _endurance;
        public float Damage => _damage;
        public float ImpactSpeed => _impactSpeed;
    }
}