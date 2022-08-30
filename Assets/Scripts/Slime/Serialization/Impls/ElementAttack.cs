using System;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [Serializable]
    public struct ElementAttack
    {
        [SerializeField] 
        public string Name;
        
        [SerializeField] 
        public float Percent;
    }
}