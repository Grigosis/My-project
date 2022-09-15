using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

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


        private static List<Type> SimpleTypes = new List<Type>();

        static AbstractObjectEditor()
        {
            SimpleTypes.Add(typeof(string));
            SimpleTypes.Add(typeof(int));
            SimpleTypes.Add(typeof(uint));
            SimpleTypes.Add(typeof(short));
            SimpleTypes.Add(typeof(ushort));
            SimpleTypes.Add(typeof(byte));
            SimpleTypes.Add(typeof(float));
            SimpleTypes.Add(typeof(double));
        }

        private static IPropertyEditor CreateSimple(MemberInfo info)
        {
            StringProperty property;
                
            if (info is PropertyInfo propertyInfo)
            {
                if (info.GetCustomAttribute<HideInInspector>() != null) return null;
                property = new StringProperty(propertyInfo);
            }
            else if (info is FieldInfo fieldInfo)
            {
                
                property = new StringProperty(fieldInfo);
            }
            else
            {
                return null;
            }

            if (property.Variants != null)
            {
                return new ComboBoxEditor(property);
            }
            else
            {
                return new TextFieldEditor(property);
            }
        }
        
        public static List<IPropertyEditor> PrepareEditor(object o)
        {
            var type = o.GetType();

            var list = new List<IPropertyEditor>();
            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var info in members)
            {
                if (info.GetCustomAttribute<HideInInspector>() != null) continue;
                var editor = CreateSimple(info);
                if (editor != null)
                {
                    list.Add(editor);
                }
            }
            
            return list;
        }
    }
}