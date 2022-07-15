using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.Scripts.Sugar;
using ROR.Lib;

namespace ROR.Core.Serialization
{
    public class D
    {
        private static D _instance;
        private readonly Dictionary<string, Definition> Definitions = new Dictionary<string, Definition>();
        private readonly Dictionary<Type, List<Definition>> DefinitionsByType = new Dictionary<Type, List<Definition>>();
        
        public static D Instance
        {
            get
            {
                lock (typeof(D))
                {
                    if (_instance == null)
                    {
                        _instance = new D();
                        _instance.Init();
                    } 
                    return _instance;
                }
            }
        }

        private string AbsolutePathToResources;
        public void Init()
        {
            AbsolutePathToResources = Path.GetFullPath(".").Replace("Temp\\Bin\\Debug", "")+"\\";
            //D:\Unity\ssh\Temp\Bin\Debug\Assets\Resources\Data
            WriteToFile("test.tst", GetExportXML());
            using (new Measure("Known types"))
            {
                var kt = KnownTypes;
            }
            using (new Measure("Load all"))
            {
                LoadAll(ExtraPath);
            }
        }
        
        static Definition GetExportXML()
        {
            return new Definition();
        }
        
       
        
        private static Type[] _knownTypes;
        public static Type[] KnownTypes
        {
            get
            {
                if (_knownTypes == null)
                {
                    var types =  new List<Type>();
                    foreach(Type t in Assembly.GetExecutingAssembly().GetTypes())
                        if (typeof(Definition).IsAssignableFrom(t))
                            types.Add(t);
                    _knownTypes = types.ToArray();
                }

                return _knownTypes;
            }
        }

        public void Register(Definition d)
        {
            Definitions.Add(d.Id, d);
            DefinitionsByType.GetOrCreate(D.Instance.GetType()).Add(d);
        }

        public T Get<T>(string Id) where T : Definition
        {
            return Definitions[Id] as T;
        }
        
        public void GetAll<T>(ICollection<T> where) where T : Definition
        {
            if (DefinitionsByType.TryGetValue(typeof(T), out var defs))
            {
                foreach (var t in defs)
                {
                    where.Add((T)t);
                }
            }
        }

        private static string ExtraPath = "XmlData/";//"Assets/Resources/Data/"
        
        public void WriteToFile(string fname, Definition definition)
        {
            WriteToFile(AbsolutePathToResources+ExtraPath+fname, new Definitions()  { DefinitionList = new SerializableList<Definition>() { definition }});
        }
       

        private void LoadAll(string folder)
        {
            var files = Directory.GetFiles(AbsolutePathToResources + folder, "*.xml", SearchOption.AllDirectories);
            var list = new List<Definition>();
            
            Parallel.ForEach(files, (file) =>
            {
                Definitions defs;
                try
                {
                    defs = ReadFromFile<Definitions>(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    //Debug.LogError("Error in file: " + file + " : " + e);
                    return;
                }

                lock (list)
                {
                    list.AddRange(defs.DefinitionList);
                }
            });

            foreach (var d in list)
            {
                Register(d);
            }
            
        }


        
        public void WriteToFile(string filePath, object objectToWrite, bool append = false) 
        {
            TextWriter writer = null;
            try
            {
                var serializer = GetSerializer(objectToWrite.GetType());
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        
        public T ReadFromFile<T>(string filePath) 
        {
            TextReader reader = null;
            try
            {
                var serializer = GetSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());//Debug.Log("Error at:" + filePath + " " + e);
                throw e;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


        [ThreadStatic] private static Dictionary<Type, XmlSerializer> Serializers;
        
        public static XmlSerializer GetSerializer(Type t)
        {
            if (Serializers == null)
            {
                Serializers = new Dictionary<Type, XmlSerializer>();
            }
            
            if (!Serializers.ContainsKey(t))
            {
                XmlAttributeOverrides xOver = new XmlAttributeOverrides();
                AddAsAttributeOverLoad<Vector2>(xOver,  "x");
                AddAsAttributeOverLoad<Vector2>(xOver,  "y");
                AddAsAttributeOverLoad<Vector3>(xOver, "x");
                AddAsAttributeOverLoad<Vector3>(xOver, "y");
                AddAsAttributeOverLoad<Vector3>(xOver, "z");
                var ser = new XmlSerializer(t, xOver, KnownTypes, null, null);
                Serializers.Add(t, ser);
            }
            return Serializers[t];
        }
        
        private static void AddAsAttributeOverLoad<T>(XmlAttributeOverrides over, string fieldName, string attrName = null)
        {
            attrName ??= fieldName;
            XmlAttributes xAttrs = new XmlAttributes();
            XmlAttributeAttribute xAttribute = new XmlAttributeAttribute(attrName);
            xAttrs.XmlAttribute = xAttribute;
  
            over.Add(typeof(T), fieldName, xAttrs);
        }
    } 
}