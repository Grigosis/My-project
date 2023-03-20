using UnityEngine;

namespace ROR.Core.Serialization
{
    public abstract class BaseDefinition : Definition
    {
        [SerializeField]
        public string Name = "";
        
        [SerializeField]
        public Sprite Icon;
        
        [SerializeField]
        public string Description = "";
        
        [SerializeField]
        public string Type = "";
    }
}