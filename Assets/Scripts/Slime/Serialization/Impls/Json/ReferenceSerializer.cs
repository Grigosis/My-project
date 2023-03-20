using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace ROR.Core.Serialization.Json
{
    [Serializable]
    public class ReferenceSerializer : ISerializationCallbackReceiver
    {
        [field:NonSerialized] 
        public Dictionary<string, Linkable> Objects = new Dictionary<string, Linkable>();
        
        [SerializeReference]
        public List<object> ObjectsForSerialize = new List<object>();

        private static System.Random Random = new System.Random();
        
        private string GenerateGuid()
        {
            while (true)
            {
                var s = Random.NextString(12);
                if (!Objects.ContainsKey(s))
                {
                    return s;
                }
            }
        }
        
        public void AddObject(Linkable linkable)
        {
            if (linkable == null) return;

            if (linkable.GUID == null)
            {
                linkable.GUID = GenerateGuid();
            }

            if (!Objects.ContainsKey(linkable.GUID))
            {
                Objects.Add(linkable.GUID, linkable);
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

            
            if (Objects.TryGetValue(key, out var value))
            {
                if (value is T t)
                {
                    return t;
                }
            }
            return default(T);

        } 

        public void OnBeforeSerialize()
        {
            ObjectsForSerialize.Clear();
            foreach (var obj in Objects)
            {
                (obj.Value as Linkable).StoreLinks();
                ObjectsForSerialize.Add(obj.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Objects.Clear();
            foreach (var obj in ObjectsForSerialize)
            {
                var linkable = obj as Linkable;
                Objects.Add(linkable.GUID, linkable);
            }
            
            foreach (var obj in Objects)
            {
                (obj.Value as Linkable).RestoreLinks(this);
            }
        }
    }
}