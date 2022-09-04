using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    public class EditorProxy : ScriptableObject
    {
        public object Editable;
    }
    
    public class AbstractObjectEditor
    {
     
        interface IPropertyEditor
        {
            void BuildGui(object o, VisualElement e);
        }

        class StringProperty
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

        
        
        class StringEditor : IPropertyEditor
        {
            private StringProperty m_property;

            public StringEditor(StringProperty property)
            {
                m_property = property;
            }

            public void BuildGui(object o, VisualElement e)
            {   
                VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor Default Resources/DialogueSystem/AnswerView.uxml");
                TemplateContainer ui = uiAsset.CloneTree();
                var deleteBtn = ui.Q<Label>("label");
                var textField = ui.Q<TextField>("text");
                deleteBtn.text = m_property.Name;
                textField.value = m_property.GetValue(o);
                
                
                e.Add(ui);
            }
        }
            
        class TypeEditor {

            //List<IPropertyEditor> 




        }
            
        public static VisualElement CreateEditor(object o)
        {
            var type = o.GetType();
            return null;
        }
    }
}