using UnityEngine;
using System.IO;

namespace SecondCycleGame
{
    public class Pistol : IRangedWeaponsItem
    {
        private readonly ItemType _type = ItemType.RangedWeapons;
        private readonly Sprite _icon = Resources.Load<Sprite>(Path.Combine("Icons", "Icon36"));
        private readonly string _name = "Пистолет";
        private readonly float _endurance = 100;
        private readonly int _numberOfBullets = 30;
        private readonly float _rateOfFire = 5;
        private readonly float _damage = 5;
        private readonly float _firingRange = 5;

        public ItemType Type => _type;
        public Sprite Icon => _icon;
        public string Name => _name;
        public float Endurance => _endurance;
        public int NumberOfBullets => _numberOfBullets;
        public float RateOfFire => _rateOfFire;
        public float Damage => _damage;
        public float FiringRange => _firingRange;
    }
}