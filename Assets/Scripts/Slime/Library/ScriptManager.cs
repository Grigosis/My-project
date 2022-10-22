using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using UnityEngine;

namespace BlockEditor
{
    public class ScriptManager {
        
        private static MetadataReference[] m_References;
        private static MetadataReference[] References {
            get {
                
                if (m_References == null)
                {
                    var list = new HashSet<string>();

                    list.Add(typeof(Debug).Assembly.Location);
                    
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    for (int i = 0; i < assemblies.Length; i++)
                    {
                        try
                        {
                            //Debug.LogWarning("Loading:" + assemblies[i].Location);
                            if (!assemblies[i].Location.ToLower().Contains("cache"))
                            {
                                list.Add(assemblies[i].Location);
                            }
                            
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Skipped:" + assemblies[i]);
                        }
                        
                    }
                    
                    //var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.AllDirectories);
                    //foreach (var file in files)
                    //{
                    //    list.Add(file);
                    //}
                    

                    
                    var arr = new List<MetadataReference>();
                    Parallel.ForEach(list, (s) =>
                    {
                        var lower = s.ToLower();
                        //if (lower.Contains("magick")) return;
                        //if (lower.Contains("library")) return;
                        //if (lower.Contains("cache")) return;
                        //if (lower.Contains("bee\\artifacts")) return;
                        //if (lower.Contains("unityengine")) return;
                        var m = MetadataReference.CreateFromFile(s);
                        lock (arr)
                        {
                            arr.Add(m);
                        }
                    });
                   
                    m_References = arr.ToArray();
                }
                return m_References;
            }
        }

        public static string DIALOG_SCRIPTS_TEMPLATE;
        public static string DIALOG_FUNCTION_TEMPLATE;
        
        public static string FX_NAME = "<FX_NAME>";
        public static string FX_ARGS = "<ARGS>";
        public static string FX_RETURN_TYPE = "<RETURN_TYPE>";
        public static string YOUR_CODE_HERE = "<YOUR CODE HERE>";
        public static string SCRIPT_CLASS_NAME = "Script";
        
        static ScriptManager()
        {
            DIALOG_SCRIPTS_TEMPLATE = File.ReadAllText("Assets/Resources/DialogScriptsTemplate.txt");
            DIALOG_FUNCTION_TEMPLATE = File.ReadAllText("Assets/Resources/DialogFunctionTemplate.txt");
        }

        
        

        public class Script
        {
            public string Text;
            public string Name;
            public string Args;
            public string Return;
            
            public object Invoker;

            public Script(string name, string text)
            {
                Text = text;
                Name = name;
                Args = "";
                Return = "void";
            }
        }

        public static string GenerateCode(List<Script> scripts)
        {
            var sb = new StringBuilder();
            foreach (var s in scripts)
            {
                var script = DIALOG_FUNCTION_TEMPLATE
                    .Replace(FX_NAME, s.Name)
                    .Replace(YOUR_CODE_HERE, s.Text)
                    .Replace(FX_RETURN_TYPE, s.Return)
                    .Replace(FX_ARGS, s.Args);
                
                sb.Append(script);
            }

            return DIALOG_SCRIPTS_TEMPLATE.Replace(YOUR_CODE_HERE, sb.ToString());
        }

        public static void CompileAndCreateFunctions (List<Script> scripts)
        {
            var txt = GenerateCode(scripts);
            
            var assembly = InnerCompile(txt, out var errors);
            if (errors != null && errors.Count() >= 0)
            {
                var sb = new StringBuilder();
                foreach (var e in errors)
                {
                    sb.AppendLine(e.ToString());
                }
                throw new Exception("Filter error:" + sb + " " + txt);
            }

            if (assembly != null)
            {
                var type = assembly.GetType(SCRIPT_CLASS_NAME);
                var obj = Activator.CreateInstance(type);
                Dictionary<string,Action> actions = ExtractFunctions(obj, ActionWrapper0, null);

                foreach (var script in scripts)
                {
                    if (actions.TryGetValue(script.Name, out var invoker))
                    {
                        script.Invoker = invoker;
                    }
                }
            }
            else
            {
                throw new Exception("Couldn't compile:" +  txt);
            }
        }

        public static Func<object, T, object> CompileAndCreateFunction <T>(string txt)
        {
            var assembly = InnerCompile(txt, out var errors);
            if (errors != null && errors.Count() >= 0)
            {
                var sb = new StringBuilder();
                foreach (var e in errors)
                {
                    sb.AppendLine(e.ToString());
                }
                throw new Exception("Filter error:" + sb);
            }

            if (assembly != null)
            {
                var type = assembly.GetType(SCRIPT_CLASS_NAME);
                var obj = Activator.CreateInstance(type);
                var method = type.GetMethod("Formula", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                return (filter, arg) =>
                {
                    try
                    {
                        return method.Invoke(obj, new object[] {filter, arg});
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                };
            }

            throw new Exception("Couldn't compile:");
        }
        
        public static Dictionary<string, T> ExtractFunctions<T>(object instance, 
            Func<MethodInfo,object,T> Creator, 
            Func<MemberInfo, bool> filter = null)
        {
            var type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);

            var d = new Dictionary<string, T>();
            foreach (var m in methods)
            {
                if (filter != null && !filter (m)) continue;
                d[m.Name] = Creator(m,instance);
            }

            return d;
        }

        
        
        private static Assembly InnerCompile(string txt, out IEnumerable<Diagnostic> errors)
        {
            // define source code, then parse it (to the type used for compilation)
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(txt);

            // define other necessary objects for compilation
            string assemblyName = Path.GetRandomFileName();

            

            // analyse and generate IL code from syntax tree
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] {syntaxTree},
                references: References,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            
            
            using (var ms = new MemoryStream()) {
            using (var ms2 = new MemoryStream()) {
                // write IL code into memory
                EmitResult result = compilation.Emit(ms, null);

                if (!result.Success)
                {
                    // handle exceptions
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    Debug.Log($"Errors: {failures.Count()}");
                    errors = failures;
                    return null;
                    
                }

                errors = null;
                ms.Seek(0, SeekOrigin.Begin);
                ms2.Seek(0, SeekOrigin.Begin);

                var arr = ms.ToArray();
                var assembly = Assembly.Load(arr);
                return assembly;//, ms2.ToArray());
            }
            }
        }

        public static Func<A,B,C,D> Wrapper4<A,B,C,D>(MethodInfo m, object o)
        {
            return (a, b, c) =>
            {
                try
                {
                    return (D) m.Invoke(o, new object[] {a, b, c});
                }
                catch (Exception e)
                {
                    var wtf = 0;
                    throw e;
                }
                
            };
        }
        
        public static Action<A,B,C> Wrapper4<A,B,C>(MethodInfo m, object o)
        {
            return (a, b, c) =>
            {
                try
                {
                    m.Invoke(o, new object[] {a, b, c});
                }
                catch (Exception e)
                {
                    var wtf = 0;
                    throw e;
                }
                
            };
        }
        
        public static Func<A,B> Wrapper1<A,B>(MethodInfo m, object o)
        {
            return (a) =>
            {
                return (B) m.Invoke(o, new object[] {a});
            };
        }
        
        public static Func<object[],A> Wrapper<A>(MethodInfo m, object o)
        {
            return (a) =>
            {
                return (A) m.Invoke(o, a);
            };
        }
        
        public static Func<A> Wrapper0<A>(MethodInfo m, object o)
        {
            return () =>
            {
                return (A) m.Invoke(o, new object[0]);
            };
        }
        
        public static Action ActionWrapper0(MethodInfo m, object o)
        {
            return () =>
            {
                m.Invoke(o, new object[0]);
            };
        }
        
        private static List<V> SortByKey<K,V>(Dictionary<K, V> dict, Func<K,K, int> sorter)
        {
            var list = new List<KeyValuePair<K, V>>();
            foreach (var kv in dict)
            {
                list.Add(kv);
            }
            
            list.Sort((a,b)=>sorter(a.Key,b.Key));

            var list2 = new List<V>();
            foreach (var item in list)
            {
                list2.Add(item.Value);
            }

            return list2;
        }
        
        
    }
}