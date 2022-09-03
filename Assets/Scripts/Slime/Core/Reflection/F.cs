using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Library;
using Assets.Scripts.Slime.Core.Algorythms.Fx;
using Assets.Scripts.Slime.Sugar;
using ClassLibrary1;
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
        public static Dictionary<string, QuestionArgsFx> QuestionArgsFx = new Dictionary<string, QuestionArgsFx>();
        public static Dictionary<string, AnswerArgsFx> AnswerArgsFx = new Dictionary<string, AnswerArgsFx>();
        public static Dictionary<string, SelectionFx> SelectionFx = new Dictionary<string, SelectionFx>();

        public static Dictionary<string, CombinatorFx> CombinatorFx = new Dictionary<string, CombinatorFx>();
        
        
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
            
            RegisterFx("TestSelectionFx", ((question, answer) =>
            {
                Debug.LogError($"SelectionFx {question} {answer}");
            }));
            
            RegisterFx("TestSelectionFx2", ((question, answer) =>
            {
                Debug.LogError($"SelectionFx2 {question} {answer}");
            }));
            
            RegisterFx("TestAnswerArgsFx", ((questionxml, answer) =>
            {
                Debug.LogError($"AnswerArgsFx {questionxml} {answer}");
                return new MockAnswerArgs(true, "TestAnswerArgs");
            }));

            RegisterFx("TestAnswerArgsFx2", ((questionxml, answer) =>
            {
                Debug.LogError($"AnswerArgsFx2 {questionxml} {answer}");
                return new MockAnswerArgs(false, "TestAnswerArgs2");
            }));
            
            RegisterFx("MockQuestionFx1", ((questionxml) =>
            {
                Debug.LogError($"AnswerArgsFx {questionxml}");
                return new MockQuestionArgs("TestAnswerArgs", true);
            }));

            RegisterFx("MockQuestionFx2", ((questionxml) =>
            {
                Debug.LogError($"MockQuestionArgs2 {questionxml}");
                return new MockQuestionArgs("MockQuestionArgs2", false);
            }));
            
            RegisterFx("SIMPLE", SimpleQuestionArgs.Fx);
            RegisterFx("SIMPLE", SimpleAnswerArgs.Fx);
        }

        
        public static CombinatorFx GetCombinatorFx(string name) {
            if (CombinatorFx.TryGetValue(name, out var cmb)) {
                return cmb;
            } else {
                throw new Exception($"CombinatorFx not found:{name}");
            }
        }

        public static QuestionArgsFx GetQuestionArgsFx(string name) {
            if (QuestionArgsFx.TryGetValue(name, out var cmb)) {
                return cmb;
            } else {
                throw new Exception($"QuestionArgsFx not found:{name}");
            }
        }

        public static AnswerArgsFx GetAnswerArgsFx(string name) {
            if (AnswerArgsFx.TryGetValue(name, out var cmb)) {
                return cmb;
            } else {
                throw new Exception($"AnswerArgsFx not found:{name}");
            }
        }
        
        public static void RegisterFx (string name, QuestionArgsFx fx) {
            QuestionArgsFx.AddOnce(name, fx);
        }
        
        public static void RegisterFx (string name, AnswerArgsFx fx) {
            AnswerArgsFx.AddOnce(name, fx);
        }
        
        public static void RegisterFx (string name, SelectionFx fx) {
            SelectionFx.AddOnce(name, fx);
        }
        
        public static void RegisterFx (string name, CombinatorFx fx) {
            CombinatorFx.AddOnce(name, fx);
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