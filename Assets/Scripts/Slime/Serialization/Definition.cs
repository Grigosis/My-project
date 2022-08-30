using System.Xml.Serialization;
using UnityEngine;

namespace ROR.Core.Serialization
{
    public class Definition : ScriptableObject
    {
        [XmlAttribute]
        [SerializeField]
        public string Id;
        
        public override string ToString()
        {
            return $"Definition [{Id}]";
        }
    }
}