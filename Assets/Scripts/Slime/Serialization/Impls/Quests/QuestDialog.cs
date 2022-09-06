using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;
using ClassLibrary1.Logic;
using SecondCycleGame.Assets.Scripts.ObjectEditor;
using Slime;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ROR.Core.Serialization
{

    public interface Linkable
    {
        /// <summary>
        /// Maybe null, but always non null before StoreLinks called
        /// </summary>
        public string GUID { get; set; }
        public void GetLinks(HashSet<Linkable> links);
        public void RestoreLinks(ReferenceSerializer dictionary);
        public void StoreLinks();
    }

    [Serializable]
    public struct WTF
    {
        [SerializeReference]
        public object Obj;
    }
    
    [Serializable]
    public class ReferenceSerializer : ISerializationCallbackReceiver
    {
        //[field:NonSerialized] 
        //public Dictionary<string, Linkable> Objects = new Dictionary<string, Linkable>();
        
        [SerializeReference]
        public List<object> ObjectsForSerialize = new List<object>();

        private static System.Random Random = new System.Random();
        
        private string GenerateGuid()
        {
            while (true)
            {
                var s = Random.NextString(12);
                //if (!Objects.ContainsKey(s))
                {
                    return s;
                }
            }
        }
        
        public void AddObject(Linkable linkable)
        {
            if (linkable == null) return;

            Debug.LogError("AddObject:" + linkable + " " + linkable.GUID);
            if (linkable.GUID == null)
            {
                linkable.GUID = GenerateGuid();
            }

            if (ObjectsForSerialize.Contains(linkable))
            {
                return;
            }
            
            //if (ObjectsForSerialize.Add(linkable))
            ObjectsForSerialize.Add(linkable);
            {
                var set = new HashSet<Linkable>();
                linkable.GetLinks(set);

                foreach (var link in set)
                {
                    AddObject(link);
                }
            }
        }

        public T GetObject<T>(string key) where T : Linkable
        {
            if (String.IsNullOrEmpty(key))
            {
                return default(T);
            }

            var index = ObjectsForSerialize.Find((x) => (x as Linkable).GUID == key);
            
            
            Debug.LogError($"RestoreLinks [{key}][{index}]");
            
            return (T)index;
            
            
            //if (Objects.TryGetValue(key, out var value))
            //{
            //    if (value is T t)
            //    {
            //        return t;
            //    }
            //}
//
            //return default(T);

        } 

        public void OnBeforeSerialize()
        {
            Debug.LogError("OnBeforeSerialize");
            //ObjectsForSerialize.Clear();
            //foreach (var obj in Objects)
            //{
            //    (obj.Value as Linkable).StoreLinks();
            //    ObjectsForSerialize.Add(obj);
            //}
            
            foreach (var obj in ObjectsForSerialize)
            {
                (obj as Linkable).StoreLinks();
            }
        }

        public void OnAfterDeserialize()
        {
            Debug.LogError("OnAfterDeserialize");
            foreach (var obj in ObjectsForSerialize)
            {
                Debug.LogError($"RestoreLinks [{obj}]");
                (obj as Linkable).RestoreLinks(this);
            }
            
            
            //Objects.Clear();
            //foreach (var obj in ObjectsForSerialize)
            //{
            //    var linkable = obj as Linkable;
            //    Objects.Add(linkable.GUID, linkable);
            //}
            //
            //
            //foreach (var obj in Objects)
            //{
            //    (obj.Value as Linkable).RestoreLinks(this);
            //}
        }
    }
    
    [Serializable]
    public class QuestDialog : Linkable
    {
        [HideInInspector]
        [field:NonSerialized]
        public CombinatorScriptable VisibilityCombinator;

        [SerializeField]
        public string Id;
        
        [SerializeField]
        public string Text;
        
        [SerializeField]
        [ComboBoxEditor("F.QuestionArgsFx")]
        public string TextArgsFx;
        
        [HideInInspector]
        [field:NonSerialized]
        public List<QuestAnswer> Answers = new List<QuestAnswer>();

        [SerializeField]
        public string VisibilityCombinatorGuid;

        [SerializeField]
        public List<string> AnswersGUIDS = new List<string>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var qa in Answers)
            {
                if (qa == null)
                {
                    sb.Append("NULL");
                }
                else
                {
                    sb.Append(qa.ToString());
                }
                
            }
            return $"QuestDialog {GetHashCode()} [{Id}/Answers:{Answers.Count} [{sb.ToString()}]]";;
        }

        public string GUID { 
            get { return Guid; }
            set { Guid = value; }
        }

        [SerializeField] public string Guid;
        
        public void GetLinks(HashSet<Linkable> links)
        {
            links.Add(VisibilityCombinator);
            foreach (var answer in Answers)
            {
                links.Add(answer);
            }
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            Answers.Clear();

            VisibilityCombinator = dictionary.GetObject<CombinatorScriptable>(VisibilityCombinatorGuid);
            foreach (var guid in AnswersGUIDS)
            {
                Answers.Add(dictionary.GetObject<QuestAnswer>(guid));
            }
        }

        public void StoreLinks()
        {
            AnswersGUIDS.Clear();
            VisibilityCombinatorGuid = VisibilityCombinator?.GUID;
            foreach (var answer in Answers)
            {
                AnswersGUIDS.Add(answer.GUID);
            }
        }
    }
}