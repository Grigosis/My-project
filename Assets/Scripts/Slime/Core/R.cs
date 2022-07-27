using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Slime.Core
{
    public class R
    {
        public static T CreateInstance<T>(string name)
        {
            var type = Type.GetType(name);
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
                Debug.LogError($"Unable to find [{name}] type");
                return default(T);
            }
        } 
        
        public static T Load<T>(string path) where T : Object
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