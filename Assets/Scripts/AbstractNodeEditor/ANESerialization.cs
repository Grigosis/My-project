using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.AbstractNodeEditor
{
    [Serializable]
    public class ANEGraphState : ScriptableObject
    {
        [SerializeField]
        public List<ANENodeState> Nodes = new List<ANENodeState>();
        
        [SerializeField]
        public List<ANEGroupState> Groups = new List<ANEGroupState>();
        
        [SerializeField]
        public List<object> Data = new List<object>();
        
        [SerializeField]
        public object PresentationObject; 
    }

    [Serializable]
    public struct ANENodeState
    {
        [SerializeField]
        public Vector2 Position;
        
        [SerializeField]
        public int GroupId;

        [SerializeField] 
        public object Data;
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