using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace Combinator
{
    public class D
    {
        private static D _instance;

        private static string ExtraPath = "Assets/XmlData/";//"Assets/Resources/Data/"
        private string AbsolutePathToResources;
        
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

        
        public void Init()
        {
            AbsolutePathToResources = Path.GetFullPath(".")+"/";
        }

        private static Type[] _knownTypes;
        public static Type[] KnownTypes
        {
            get
            {
                if (_knownTypes == null)
                {
                    var types =  new List<Type>();
                    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                    {
                        if (t.Name.EndsWith("Xml"))
                        {
                            types.Add(t);
                        }
                    }
                        
                            
                    _knownTypes = types.ToArray();
                }

                return _knownTypes;
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

        public T ReadFrom<T>(string filePath)
        {
            return ReadFromFile<T>(AbsolutePathToResources+ExtraPath+filePath);
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