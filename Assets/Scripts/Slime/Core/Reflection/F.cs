using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Slime.Core.Algorythms.Fx;
using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    public class F
    {
        public readonly static Dictionary<string, AIFunction> AIFunctions = new Dictionary<string, AIFunction>();

        public static void Init()
        {
            Debug.LogWarning("F:Init");
            AddScriptFunctions(typeof(AIFunctions));
        }
        
        public static void AddScriptFunctions(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                AddScriptFunctions(type);
            }
        }
        
        public static void AddScriptFunctions(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                AddFx(method, AIFunctions);
            }
        }

        private static bool AddFx<T>(MethodInfo method, Dictionary<string, T> funcMap) where T : class
        {
            if (IsMethodCompatibleWithDelegate<T>(method))
            {
                Debug.LogWarning($"F:Found method [{method.Name}/{typeof(T).Name}]");
                var function = (T) (object) Delegate.CreateDelegate(typeof (T), method);
                funcMap.Add(method.Name, function);
                return true;
            }

            return false;
        }

        private static bool IsMethodCompatibleWithDelegate<T>(MethodInfo method) where T : class
        {
            Type delegateType = typeof(T);
            MethodInfo delegateSignature = delegateType.GetMethod("Invoke");

            bool parametersEqual = delegateSignature
                .GetParameters()
                .Select(x => x.ParameterType)
                .SequenceEqual(method.GetParameters()
                    .Select(x => x.ParameterType));

            return delegateSignature.ReturnType == method.ReturnType &&
                   parametersEqual;
        }
    }
}