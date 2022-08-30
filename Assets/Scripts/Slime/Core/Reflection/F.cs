using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Library;
using Assets.Scripts.Slime.Core.Algorythms.Fx;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    
    public class CombinatorFunctionInfo
    {
        public readonly Type OutType;
        public readonly Type InType;
        public readonly Type ContextType;
        public readonly object Func;

        public CombinatorFunctionInfo(Type outType, Type inType, Type contextType, object func)
        {
            OutType = outType;
            InType = inType;
            ContextType = contextType;
            Func = func;
        }
    }
    
    public class F
    {
        public readonly static Dictionary<string, AIFunction> AIFunctions = new Dictionary<string, AIFunction>();

        public static Dictionary<string, CombinatorFunctionInfo> Functions = new Dictionary<string, CombinatorFunctionInfo>();
        
        static F()
        {
            Debug.LogWarning("F:Init");
            AIFunctions.AddScriptFunctions(typeof(AIFunctions));
            
            //RegisterMulti<CombinatorScriptable, string, string>("Concat", CombinatorFunctions.Concat);
            //RegisterMulti<CombinatorScriptable, double, double>("Mlt", CombinatorFunctions.Mlt);
            //RegisterMulti<CombinatorScriptable, double, double>("Sum", CombinatorFunctions.Sum);
            //RegisterMulti<CombinatorScriptable, double, string>("ToStr", CombinatorFunctions.ToStr);
            
            RegisterMulti<CombinatorScriptable, string, string>("Concat", CombinatorFunctions.Concat);
            RegisterMulti<CombinatorScriptable, double, double>("Mlt", CombinatorFunctions.Mlt);
            RegisterMulti<CombinatorScriptable, double, double>("Sum", CombinatorFunctions.Sum);
            RegisterMulti<CombinatorScriptable, double, string>("ToStr", CombinatorFunctions.ToStr);
            RegisterMulti<CombinatorScriptable, double, bool>("IsMore", CombinatorFunctions.IsMore);
            RegisterMulti<CombinatorScriptable, bool, bool>("And", CombinatorFunctions.And);
            RegisterMulti<CombinatorScriptable, bool, bool>("Or", CombinatorFunctions.Or);
        }

        #region Combinator

        public static void RegisterSingle<T,K>(string func, Func<T,K> obj)
        {
            Functions.Add(func, new CombinatorFunctionInfo(typeof(K), null, null, obj));
        }
        
        public static void RegisterSingleDependent<T,CONTEXT,K>(string func, Func<T,CONTEXT, K> obj)
        {
            Functions.Add(func, new CombinatorFunctionInfo(typeof(K), null, typeof(CONTEXT), obj));
        }
        
        public static void RegisterMulti<T,A,K>(string func, Func<T,List<A>,K> obj)
        {
            Functions.Add(func, new CombinatorFunctionInfo(typeof(K), typeof(A), null, obj));
        }
        
        public static void RegisterMultiDependent<T,CONTEXT,A,K>(string func, Func<T,CONTEXT,List<A>,K> obj)
        {
            Functions.Add(func, new CombinatorFunctionInfo(typeof(K), typeof(A), typeof(CONTEXT), obj));
        }

        #endregion
    }
}