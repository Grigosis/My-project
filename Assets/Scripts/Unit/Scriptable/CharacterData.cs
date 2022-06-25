using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
    public class CharacterData : UnitData
    {
        [SerializeField] private GameObject _portraitPrefab;

        public GameObject PortraitPrefab => _portraitPrefab;
    }
}
