using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Assets.Scripts.Sugar;
using UnityEditor;
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
        private Dictionary<Type, List<Type>> Inherritance = new Dictionary<Type, List<Type>>();

        private R()
        {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in allTypes)
            {
                if (t.FullName.Contains("TMPro")) continue;
                if (t.FullName.Contains("<")) continue;
                if (t.FullName.Contains("+")) continue;
                
                if (m_types.ContainsKey(t.Name))
                {
                    //Debug.LogWarning($"Already contains class with this name {t.Name} [{t.FullName}] [{m_types[t.Name].FullName}]");
                }
                else
                {
                    m_types.Add(t.Name, t);
                }

                if (t.IsInterface)
                {
                    Inherritance.Add(t, new List<Type>());
                }
            }

            using (new Measure("Inherritance"))
            {
                foreach (var t in allTypes)
                {
                    if (t.IsAbstract || t.IsInterface)
                    {
                        continue;
                    }
                    
                    foreach (var i in Inherritance)
                    {
                        if (i.Key.IsAssignableFrom(t))
                        {
                            if (i.Key == t) continue;
                            i.Value.Add(t);
                        }
                    }
                }
            }
        }

        public List<Type> GetInterfaceImpls(Type t)
        {
            if (Inherritance.TryGetValue(t, out var list))
            {
                return list;
            }

            return null;
        }

        public T CreateInstanceOrNull<T>(string name, string errorAddition = "")
        {
            if (string.IsNullOrEmpty(name)) return default(T);
            return CreateInstance<T>(name, errorAddition);
        }

        public T CreateInstance<T>(string name, string errorAddition = "")
        {
            if (!m_types.TryGetValue(name, out var type) || type == null)
            {
                type = Type.GetType(name);
            }
            
            if (type == null)
            {
                Debug.LogError($"Unable to find [{name}] type {errorAddition}");
                return default(T);
            }

            object obj;
            try
            {
                obj = Activator.CreateInstance(type);
                if (obj == null) {
                    Debug.LogError($"Unable to find [{name}] type {errorAddition}");
                    return default(T);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error at creating class of type:{name} {errorAddition}");
                return default(T);
            }

            if (obj is T t) {
                return t;
            }
            else
            {
                Debug.LogError($"Wrong type of [{name}] expected [{typeof(T)}] but got [{obj.GetType()}] {errorAddition}");
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
        
        public static T CreateOrLoadAsset<T>(string path, bool createNew = false) where T : ScriptableObject
        {
            string fullPath = $"{path}.asset";
            if (!File.Exists(fullPath))
            {
                Debug.LogError("Created assert at:" + fullPath);
                T asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
                return asset;
            }
            else
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>($"{path}.asset");
                if (asset != null)
                {
                    return asset;
                    
                }
                
                if (createNew)
                {
                    asset = ScriptableObject.CreateInstance<T>();
                    AssetDatabase.CreateAsset(asset, fullPath);
                    return asset;
                }
                else
                {
                    Debug.LogError($"Error loading {asset}");
                    return asset;
                }
            }
        }
        
        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void AddObjectToAsset(Object qd, Object dataOnly)
        {
            try
            {
                AssetDatabase.AddObjectToAsset(qd, dataOnly);
            }
            catch (Exception e)
            {
                        
            }
        }
    }
}