using System.Xml.Serialization;
using Assets.Scripts.AbstractNodeEditor;
using UnityEngine;

namespace ROR.Core.Serialization
{
    public class Definition : ScriptableObject
    {
        [Id]
        [XmlAttribute]
        [SerializeField]
        public string Id;
        
        public override string ToString()
        {
            return $"Definition [{Id}]";
        }
    }
}