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
 
namespace BlockEditor
{
    public class ScriptManager {
        
        private static MetadataReference[] m_References;
        private static MetadataReference[] References {
            get {
                
                if (m_References == null)
                {
                    var list = new HashSet<string>();
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    for (int i = 0; i < assemblies.Length; i++)
                    {
                        list.Add(assemblies[i].Location);//, 
                    }
                    
                    var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        list.Add(file);
                    }

                    var arr = new List<MetadataReference>();
                    Parallel.ForEach(list, (s) =>
                    {
                        if (s.ToLower().Contains("magick")) return;
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
                var type = assembly.GetType("Script");
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
                if (filter == null || !filter (m)) continue;
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
                EmitResult result = compilation.Emit(ms, ms2);

                if (!result.Success)
                {
                    // handle exceptions
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    errors = failures;
                    return null;
                    
                }

                errors = null;
                ms.Seek(0, SeekOrigin.Begin);
                ms2.Seek(0, SeekOrigin.Begin);
                return Assembly.Load(ms.ToArray(), ms2.ToArray());
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