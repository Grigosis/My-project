using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ROR.Core.Serialization;
using ROR.Core.Serialization.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.AbstractNodeEditor
{
    [Serializable]
    public class ANEGraphState
    {
        [SerializeField]
        public List<ANEGroupState> Groups = new List<ANEGroupState>();

        [SerializeReference]
        public object PresentationObject; 
        
        [SerializeReference]
        public ReferenceSerializer ReferenceSerializer;
    }

    [Serializable]
    public struct ANENodeState : Linkable
    {
        [SerializeField]
        public Vector2 Position;
        
        [SerializeField]
        public int GroupId;

        [field:NonSerialized] 
        public object Data;
        

        [SerializeField]
        public string DataGUID;
        
        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        public string Guid;
        
        
        
        public void GetLinks(HashSet<Linkable> links) { links.Add(Data as Linkable); }

        public void RestoreLinks(ReferenceSerializer dictionary) { Data = dictionary.GetObject<Linkable>(DataGUID); }

        public void StoreLinks() { DataGUID = (Data as Linkable)?.GUID; }
    }
    
    [Serializable]
    public struct ANEGroupState
    {
        [SerializeField]
        public Vector2 Position;
        
        [SerializeField]
        public int Id;
        
        [SerializeField]
        public string Name;
    }
    
    
}