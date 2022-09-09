using System;
using System.Collections.Generic;
using ClassLibrary1;
using UnityEngine;

namespace SecondCycleGame.Assets.Scripts.AbstractNodeEditor
{
    [Serializable]
    public class QuestContext
    {
        //[SerializeField]
        //public SerializableDictionary<string, object> GLOBAL_VALUES = new SerializableDictionary<string, object>();


        [field:NonSerialized]
        public Dictionary<string, ISubscribable> Subscribables = new Dictionary<string, ISubscribable>();
        
    }
}