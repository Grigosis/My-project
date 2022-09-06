using System;
using System.Collections.Generic;
using System.Reflection;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    public class StringProperty
    {
        public Func<object,string> GetValue;
        public Action<object,string> SetValue;
        public string Name;
        public List<string> Variants;
        private Type m_type;

        public StringProperty(PropertyInfo propertyInfo)
        {
            Name = propertyInfo.Name;
            m_type = propertyInfo.PropertyType;
            GetValue = (x) =>
            {
                var obj = propertyInfo.GetMethod.Invoke(x, new object[0]);
                return TransformFromObject (obj);
            };
            SetValue = (x, s) =>
            {
                var toObject = TransformToObject(s);
                propertyInfo.SetMethod.Invoke(x, new object[] { toObject });
            };
            
            var attribute = propertyInfo.GetCustomAttribute<ComboBoxEditorAttribute>();
            if (attribute != null)
            {
                Variants = attribute.GetVariants();
            }
        }
        
        public StringProperty(FieldInfo fieldInfo)
        {
            Name = fieldInfo.Name;
            m_type = fieldInfo.FieldType;
            GetValue = (x) =>
            {
                var obj = fieldInfo.GetValue(x);
                return TransformFromObject (obj);
            };
            SetValue = (x, s) =>
            {
                var toObject = TransformToObject(s);
                fieldInfo.SetValue(x, toObject);
            };
            
            var attribute = fieldInfo.GetCustomAttribute<ComboBoxEditorAttribute>();
            if (attribute != null)
            {
                Variants = attribute.GetVariants();
            }
        }

        private string TransformFromObject(object obj)
        {
            return obj?.ToString() ?? "NULL";
        }
        
        private object TransformToObject(string obj)
        {
            if (obj == "NULL") return null;


            if (m_type == typeof(string)) return obj;
            
            if (m_type == typeof(int)) return int.TryParse(obj, out var value) ? value : 0;
            if (m_type == typeof(uint)) return uint.TryParse(obj, out var value) ? value : 0;
            if (m_type == typeof(long)) return long.TryParse(obj, out var value) ? value : 0;
            if (m_type == typeof(ulong)) return ulong.TryParse(obj, out var value) ? value : 0;
            if (m_type == typeof(float)) return float.TryParse(obj, out var value) ? value : 0;
            if (m_type == typeof(double)) return double.TryParse(obj, out var value) ? value : 0;

            throw new Exception($"Unknown type: {m_type}");
        }
    }
}