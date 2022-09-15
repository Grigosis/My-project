using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using ClassLibrary1;
using ROR.Core.Serialization.Json;
using UnityEngine;

namespace ROR.Core.Serialization
{
    [Serializable]
    public class QuestDefinition : Linkable
    {
        //HumanId
        [SerializeField]
        public string Id;
        
        [SerializeField]
        public string Name;
        
        [SerializeField]
        public Dictionary<string, object> Settings = new Dictionary<string, object>(); //Например сколько кого нужно убить
        
        //Варианты того что пишется в описании текста
        [NonSerialized]
        [HideInInspector]
        public List<QuestText> QuestTexts = new List<QuestText>();
        
        [SerializeField]
        [HideInInspector]
        public List<string> QuestTextsGuids = new List<string>();
        
        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        private string Guid;

        public void GetLinks(HashSet<Linkable> links)
        {
            foreach (var text in QuestTexts)
            {
                links.Add(text);
            }
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            QuestTexts.Clear();
            foreach (var guid in QuestTextsGuids)
            {
                QuestTexts.Add(dictionary.GetObject<QuestText>(guid));
            }
        }

        public void StoreLinks()
        {
            QuestTextsGuids.Clear();
            foreach (var guid in QuestTexts)
            {
                QuestTextsGuids.Add(guid.Guid);
            }
        }
    }
}