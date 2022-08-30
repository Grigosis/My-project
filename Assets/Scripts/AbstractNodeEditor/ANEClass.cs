using System;
using System.Reflection;

namespace Assets.Scripts.AbstractNodeEditor
{
    public class ANEClass
    {
        public Type AneType;
        public Func<object, string> Identifier;

        public ANEClass(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                foreach (var attribute in field.GetCustomAttributes())
                {
                    if (attribute.GetType() == typeof(IdPointerAttribute))
                    {
                        
                    }

                    if (attribute.GetType() == typeof(IdAttribute))
                    {
                        Identifier = (obj) =>
                        {
                            return (string)field.GetValue(obj);
                        };
                    }
                }
            }
        }
    }
}