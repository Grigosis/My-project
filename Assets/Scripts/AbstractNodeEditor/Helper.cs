using System;
using System.Linq;
using Assets.Scripts.Slime.Core;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slime
{
    public static class Helper
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Port CreatePort(this Node node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;
            return port;
        }

        public static VisualElement AddClasses(this VisualElement element, params string[] classNames)
        {
            foreach (string className in classNames)
            {
                element.AddToClassList(className);
            }

            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
        {
            foreach (string styleSheetName in styleSheetNames)
            {
                StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load(styleSheetName);
                element.styleSheets.Add(styleSheet);
            }

            return element;
        }
        
        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }


        private static void CreateEditor<T>(VisualElement container, T edit)
        {
            //UDateTime n = new UDateTime();
            //var time = Object.Instantiate(n);
            //Editor editor = Editor.CreateEditor(n);
            //IMGUIContainer inspectorIMGUI = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
            //container.Add(inspectorIMGUI);
        }
        /*private static void BuildInspectorProperties(SerializedObject obj, VisualElement container)
        {
            // TODO [Header()] and [Space()] are manually added until Unity supports them.
 
            SerializedProperty iterator = obj.GetIterator();
            Type targetType = obj.targetObject.GetType();
            List<MemberInfo> members = new List<MemberInfo>(targetType.GetMembers());
 
            if (!iterator.NextVisible(true)) return;
            do
            {
                PropertyField propertyField = new PropertyField(iterator.Copy())
                {
                    name = "PropertyField:" + iterator.propertyPath
                };
 
                MemberInfo member = members.Find(x => x.Name == propertyField.bindingPath);
                if (member != null)
                {
                    IEnumerable<Attribute> headers = member.GetCustomAttributes(typeof(HeaderAttribute));
                    IEnumerable<Attribute> spaces = member.GetCustomAttributes(typeof(SpaceAttribute));
 
                    foreach (Attribute x in headers)
                    {
                        HeaderAttribute actual = (HeaderAttribute) x;
                        Label header = new Label { text = actual.header};
                        header.style.unityFontStyleAndWeight = FontStyle.Bold;
                        container.Add(new Label { text = " ", name = "Header Spacer"});
                        container.Add(header);
                    }
                    foreach (Attribute unused in spaces)
                    {
                        container.Add(new Label { text = " " });
                    }
                }
 
                if (iterator.propertyPath == "m_Script" && obj.targetObject != null)
                {
                    propertyField.SetEnabled(value: false);
                }
 
                container.Add(propertyField);
            }
            while (iterator.NextVisible(false));
        }*/

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }

        public static void CreateEditorClassSelector(ref string implementation, Type type, string name)
        {
            var impls = R.Instance.GetInterfaceImpls(type);
            if (impls == null)
            {
                Debug.LogError($"Haven't found {type}");
                return;
            }
            
            
            var copy = implementation;
            var implsA = impls.Select((x)=>x.Name).ToArray();
            if (implsA.Length == 0)
            {
                return;
            }
            
            
            
            var index = Array.IndexOf(implsA, copy);
            
            if (index < 0)
                index = 0;
            
            index = EditorGUILayout.Popup(name, index, implsA);
            
            implementation = implsA[index];
        }
        
        public static void CreateEditorClassSelectorForStructs(Rect position, ref string implementation, Type type, string name)
        {
            var impls = R.Instance.GetInterfaceImpls(type);
            if (impls == null)
            {
                Debug.LogError($"Haven't found {type}");
                return;
            }
            var implsA = impls.Select((x)=>x.Name).ToArray();
            CreateEditorClassSelectorForStructs(position, ref implementation, implsA, name);
        }
        
        public static void CreateEditorClassSelectorForStructs(Rect position, ref string implementation, string[] values, string name)
        {
            var index = Array.IndexOf(values, implementation);
            
            if (index < 0)
                index = 0;
            
            index = EditorGUI.Popup(position, name, index, values);

            try
            {
                implementation = values[index];
            }
            catch (Exception e)
            {
                Debug.LogError($"Accessing {index} of {values}");
            }
            
        }

        public static void CreateFloatEditor(this SerializedProperty property, string propertyName, ref Rect position, GUIContent label)
        {
            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            
            var x = property.FindPropertyRelative(propertyName);
            EditorGUI.BeginProperty(rect, label, x);
            x.floatValue = EditorGUI.FloatField(rect, propertyName, x.floatValue);
            EditorGUI.EndProperty();
            position.y += EditorGUIUtility.singleLineHeight;
        }
        
        public static void CreateStringEditor(this SerializedProperty property, string propertyName, ref Rect position, GUIContent label)
        {
            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            
            var x = property.FindPropertyRelative(propertyName);
            EditorGUI.BeginProperty(rect, label, x);
            x.stringValue = EditorGUI.TextField(rect, propertyName, x.stringValue);
            EditorGUI.EndProperty();
            position.y += EditorGUIUtility.singleLineHeight;
        }

        public static void CreateSelector(this SerializedProperty property, string propertyName, string[] variants, ref Rect position, GUIContent label)
        {
            var rect = position;
            rect.height = EditorGUIUtility.singleLineHeight;
            
            var x = property.FindPropertyRelative(propertyName);
            var v = x.stringValue;
            EditorGUI.BeginProperty(rect, label, x);
            CreateEditorClassSelectorForStructs(rect,ref v, variants, propertyName);
            x.stringValue = v;
            EditorGUI.EndProperty();
            position.y += EditorGUIUtility.singleLineHeight;
        }
    }
}