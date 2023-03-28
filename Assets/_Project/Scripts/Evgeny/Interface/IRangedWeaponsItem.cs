using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SecondCycleGame
{
    public interface IRangedWeaponsItem : IItem
    {
        public float RateOfFire { get; }
        public float Damage { get; }
        public float FiringRange { get; }
        public int NumberOfBullets { get; }

        public void GetSpecification(ref List<TextMeshProUGUI> names, ref List<TextMeshProUGUI> values) {
            List<string> _names = new List<string> { "Прочность", "Количество пуль", "Скоростельность", "Урон", "Дальность стельбы" };
            List<string> _values = new List<string> { Endurance.ToString(), NumberOfBullets.ToString(), RateOfFire.ToString(), Damage.ToString(), FiringRange.ToString() };
            if (_values.Count != _names.Count) {
                Debug.LogError("Add or remove value or name");
            }
            else {
                for (int i = 0; i < _names.Count; i++) {
                    names[i].text = _names[i];
                    values[i].text = _values[i];
                    names[i].gameObject.SetActive(true);
                    values[i].gameObject.SetActive(true);
                }
                for (int i = _names.Count; i < names.Count; i++) {
                    names[i].gameObject.SetActive(false);
                    values[i].gameObject.SetActive(false);
                }
            }
        }
    }
}