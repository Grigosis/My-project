using System;
using System.Collections.Generic;
using ClassLibrary1;
using UnityEngine;

namespace SecondCycleGame.Assets.Scripts.AbstractNodeEditor
{
    public class QuestContext : ScriptableObject
    {
        [SerializeField]
        public SerializableDictionary<string, object> GLOBAL_VALUES = new SerializableDictionary<string, object>();


        [NonSerialized]
        public Dictionary<string, Subscribable<object>> Subscribables = new Dictionary<string, Subscribable<object>>();
        
    }
}