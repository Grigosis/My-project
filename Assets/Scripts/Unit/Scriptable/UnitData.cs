using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
    public class UnitData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private GameObject _prefab;

        public string Name => _name;
        public Sprite Portrait => _portrait;
        public GameObject Prefab => _prefab;
    }
}
