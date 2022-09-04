using System;
using System.Reflection;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    public class StringProperty
    {
        public Func<object,string> GetValue;
        public Action<object,string> SetValue;
        public string Name;
        private Type m_type;

        public StringProperty(MemberInfo info)
        {
            Name = info.Name;
            if (info is PropertyInfo propertyInfo)
            {
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
            }
            else if (info is FieldInfo fieldInfo)
            {
                m_type = fieldInfo.FieldType;
                GetValue = (x) =>
                {
                    var obj = fieldInfo.GetValue(x);
                    return TransformFromObject (obj);
                };
                SetValue = (x, s) =>
                {
                    var toObject = TransformToObject(s);
                    fieldInfo.SetValue(x, new object[] { toObject });
                };
            }
            else
            {
                throw new Exception();
            }
        }

        private string TransformFromObject(object obj)
        {
            return obj.ToString();
        }
        
        private string TransformToObject(string obj)
        {
            //switch (m_type)
            //{
            //    case 
            //}
            
            return obj.ToString();
        }
    }
}