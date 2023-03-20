using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [Serializable]
    public class NPC
    {
        [SerializeField]
        public string Id;
        
        [SerializeField]
        public string Name;
        
        [SerializeField]
        public Dictionary<string, string> Settings = new Dictionary<string, string>();
    }
}