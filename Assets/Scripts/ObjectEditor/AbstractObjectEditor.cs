using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    public class AbstractObjectEditor
    {
        public static VisualElement CreateEditor(object o)
        {
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor Default Resources/DialogueSystem/AnswerView.uxml");
            TemplateContainer ui = uiAsset.CloneTree();
            var root = ui.Q<VisualElement>("element");
            
            var editors = PrepareEditor (o);
            foreach (var editor in editors)
            {
                editor.BuildGui(o, root);
            }

            return root;
        }
        
        public static List<IPropertyEditor> PrepareEditor(object o)
        {
            var type = o.GetType();

            var list = new List<IPropertyEditor>();
            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var info in members)
            {
                if (info is MethodInfo mi) continue;

                Type m_type = null;
                if (info is PropertyInfo propertyInfo)
                {
                    m_type = propertyInfo.PropertyType;
                }
                else if (info is FieldInfo fieldInfo)
                {
                    m_type = fieldInfo.FieldType;
                }

                TextFieldEditor editor = new TextFieldEditor(new StringProperty(info));
                list.Add(editor);
            }
            
            return list;
        }
    }
}