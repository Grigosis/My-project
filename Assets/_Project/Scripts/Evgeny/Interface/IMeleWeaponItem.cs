using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SecondCycleGame
{
    public interface IMeleWeaponItem : IItem
    {
        public float Damage { get; }
        public float ImpactSpeed { get; }

        public void GetSpecification(ref List<TextMeshProUGUI> names, ref List<TextMeshProUGUI> values) {
            List<string> _names = new List<string> { "Прочность", "Урон", "Скорость атаки" };
            List<string> _values = new List<string> { Endurance.ToString(), Damage.ToString(), ImpactSpeed.ToString() };
            if (_values.Count != _names.Count) {
                Debug.LogError("Add value or name");
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