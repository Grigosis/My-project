using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Scripts;

namespace ROR.Core.Serialization
{
    public class SClass<T> where T : ICreatable
    {
        private static Dictionary<string, Type> NamesToClasses = new Dictionary<string, Type>();

        static SClass()
        {
            using (new Measure("SClass"))
            {
                foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in ass.GetTypes())
                    {
                        if (t?.Namespace?.StartsWith("ROR") ?? false)
                        {
                            NamesToClasses[t.Name] = t;
                        }
                    }
                }
            }
        }
        
        
        
        [XmlIgnore]
        public Type Type
        {
            get
            {
                return NamesToClasses[m_type];
            }
            set
            {
                if (!typeof(T).IsAssignableFrom(value))
                {
                    throw new Exception("Wrong class : " + value.FullName + " must assign from : " + typeof(T).FullName);
                }
                
                m_type = value.Name;
            }
        }

        [XmlAttribute("Type")]
        public string m_type;

        public T Create()
        {
            return (T)Activator.CreateInstance(Type);
        }
    }
}