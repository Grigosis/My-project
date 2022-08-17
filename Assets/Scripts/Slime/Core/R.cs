using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Slime.Core
{
    public class R
    {
        public static R Instance
        {
            get
            {
                lock (typeof(R))
                {
                    if (m_instance == null)
                    {
                        m_instance = new R();
                    }

                    return m_instance;
                }
            }
        }

        private static R m_instance;
        
        private Dictionary<string, Type> m_types = new Dictionary<string, Type>();

        private R()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.FullName.Contains("TMPro")) continue;
                if (t.FullName.Contains("<")) continue;
                if (t.FullName.Contains("+")) continue;
                
                if (m_types.ContainsKey(t.Name))
                {
                    Debug.LogWarning($"Already contains class with this name {t.Name} [{t.FullName}] [{m_types[t.Name].FullName}]");
                }
                else
                {
                    m_types.Add(t.Name, t);
                }
                
            }
        }

        public T CreateInstanceOrNull<T>(string name)
        {
            if (string.IsNullOrEmpty(name)) return default(T);
            return CreateInstance<T>(name);
        }

        public T CreateInstance<T>(string name)
        {
            if (!m_types.TryGetValue(name, out var type) || type == null)
            {
                type = Type.GetType(name);
            }
            
            if (type == null)
            {
                Debug.LogError($"Unable to find [{name}] type");
                return default(T);
            }
            
            var obj = Activator.CreateInstance(type);
            if (obj == null) {
                Debug.LogError($"Unable to find [{name}] type");
                return default(T);
            }
            
            if (obj is T t) {
                return t;
            }
            else
            {
                Debug.LogError($"Wrong type of [{name}] expected [{typeof(T)}] but got [{obj.GetType()}]");
                return default(T);
            }
        } 
        
        public T Load<T>(string path) where T : Object
        {
            var r = Resources.Load<T>(path);
            if (r == null)
            {
                Debug.LogError($"Doesn't exist: [{path}]");
            }
            return r;
        }
    }
}