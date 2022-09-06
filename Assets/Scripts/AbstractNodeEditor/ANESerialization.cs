using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ROR.Core.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.AbstractNodeEditor
{

    [Serializable]
    public class TestScriptable : ScriptableObject
    {
        [SerializeReference]
        public List<object> Data = new List<object>();
    }


    [Serializable]
    public class TestDialog
    {
        [SerializeField]
        public string Name;
        
        [SerializeReference]
        public TestDialog NextDialog;

        [SerializeReference]
        public List<TestDialog> NextDialogs = new List<TestDialog>();

        public override string ToString()
        {
            return $"TestDialog {Name} ";
        }
    }
        
    [Serializable]
    public class ANEGraphStateJson
    {
        [SerializeField]
        public List<ANENodeState> Nodes = new List<ANENodeState>();
        
        [SerializeField]
        public List<ANEGroupState> Groups = new List<ANEGroupState>();

        [SerializeReference]
        public object PresentationObject; 
        
        [SerializeReference]
        public ReferenceSerializer ReferenceSerializer;
    }
    
    [Serializable]
    public class ANEGraphState : ScriptableObject
    {
        [SerializeField]
        public List<ANENodeState> Nodes = new List<ANENodeState>();
        
        [SerializeField]
        public List<ANEGroupState> Groups = new List<ANEGroupState>();
        
        //[SerializeReference]
        public List<object> Data = new List<object>();
        
        //[SerializeReference]
        public object PresentationObject; 
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
        
        public string GUID { 
            get { return Guid; }
            set { Guid = value; }
        }

        [SerializeField] public string Guid;
        
        
        
        public void GetLinks(HashSet<Linkable> links)
        {
            if (Data == null)
            {
                Debug.LogError("GetLinks ANENodeState IS NULL");
            }
            links.Add(Data as Linkable);
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            Data = dictionary.GetObject<Linkable>(DataGUID);
            Debug.LogError($"RestoreLinks ANENodeState {Data}");
        }

        public void StoreLinks()
        {
            var isL = (Data as Linkable) != null;
            Debug.LogError($"StoreLinks ANENodeState [{Data}] [{isL}] {(Data as Linkable)?.GUID}");
            if (!(Data is Linkable))
            {
                Debug.LogError("Not Linkable:"+ Data);
            }
            DataGUID = (Data as Linkable)?.GUID;
        }
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