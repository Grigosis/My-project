using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using ROR.Core.Serialization.Json;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [Serializable]
    public class QuestText : Linkable
    {
        [SerializeField]
        public string Text;
        
        [NonSerialized]
        [SerializeField]
        public CombinatorData IsVisible;
        
        [HideInInspector]
        [SerializeField]
        public string IsVisibleGuid;
        
        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        public string Guid;
        
        
        public void GetLinks(HashSet<Linkable> links)
        {
            links.Add(IsVisible);
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            IsVisible = dictionary.GetObject<CombinatorData>(IsVisibleGuid);
        }

        public void StoreLinks()
        {
            IsVisibleGuid = IsVisible.Guid;
        }
    }
}