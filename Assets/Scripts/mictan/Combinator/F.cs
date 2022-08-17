using System;
using System.Collections.Generic;

namespace Combinator
{
    public class F
    {
        public static Dictionary<string, Tuple<Type, Type, Type, object>> Functions = new Dictionary<string, Tuple<Type, Type, Type, object>>();

        public static void RegisterSingle<T,K>(string func, Func<T,K> obj)
        {
            Functions.Add(func, new Tuple<Type, Type, Type, object>(typeof(K), null, null, obj));
        }
        
        public static void RegisterSingleDependent<T,CONTEXT,K>(string func, Func<T,CONTEXT, K> obj)
        {
            Functions.Add(func, new Tuple<Type, Type, Type, object>(typeof(K), null, typeof(CONTEXT), obj));
        }
        
        public static void RegisterMulti<T,A,K>(string func, Func<T,List<A>,K> obj)
        {
            Functions.Add(func, new Tuple<Type, Type, Type, object>(typeof(K), typeof(A), null, obj));
        }
        
        public static void RegisterMultiDependent<T,CONTEXT,A,K>(string func, Func<T,CONTEXT,List<A>,K> obj)
        {
            Functions.Add(func, new Tuple<Type, Type, Type, object>(typeof(K), typeof(A), typeof(CONTEXT), obj));
        }
    }
}