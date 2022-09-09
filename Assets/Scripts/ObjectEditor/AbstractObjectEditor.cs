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
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor Default Resources/Editor/Editor.uxml");
            TemplateContainer ui = uiAsset.CloneTree();
            var root = ui.Q<VisualElement>("container");
            
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
                
                StringProperty property;
                
                if (info is PropertyInfo propertyInfo)
                {
                    if (info.GetCustomAttribute<HideInInspector>() != null) continue;
                    property = new StringProperty(propertyInfo);
                }
                else if (info is FieldInfo fieldInfo)
                {
                    if (info.GetCustomAttribute<HideInInspector>() != null) continue;
                    property = new StringProperty(fieldInfo);
                }
                else
                {
                    continue;
                }

                if (property.Variants != null)
                {
                    ComboBoxEditor editor = new ComboBoxEditor(property);
                    list.Add(editor);
                }
                else
                {
                    TextFieldEditor editor = new TextFieldEditor(property);
                    list.Add(editor);
                }
            }
            
            return list;
        }
    }
}