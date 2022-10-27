using System;
using System.Linq;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;
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
            
            var implsA = impls.Select((x)=>x.Name).ToArray();
            if (implsA.Length == 0)
            {
                Debug.LogError($"Zero items found for {type}/{name}");
                return;
            }

            CreateEditorClassSelector(ref implementation, implsA, name);
        }
        
        public static void CreateEditorClassSelector(ref string implementation, string[] values, string name)
        {
            var index = Array.IndexOf(values, implementation);
            
            if (index < 0)
                index = 0;
            
            index = EditorGUILayout.Popup(name, index, values);

            try
            {
                implementation = values[index];
            }
            catch (Exception e)
            {
                Debug.LogError($"Accessing {index} of {values}");
            }
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
            
            if (implsA.Length == 0)
            {
                Debug.LogError($"Zero items found for {type}/{name}");
                return;
            }
            
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

        public interface IHideableConnectionContainer {
            void UpdateConnectionVisible();
        }

        public class HideConnectionHelper {
            private Edge edge;
            private ExtendedPort port1;
            private ExtendedPort port2;

            public static bool GlobalShowAllConnections = false;//???

            public HideConnectionHelper(Edge edge, ExtendedPort port1, ExtendedPort port2) {
                this.edge = edge;
                this.port1 = port1;
                this.port2 = port2;
                //edge.showInMiniMap = true;
            }

            public void SetConnectionHide(bool hidden) {
                edge.SetHidden(hidden && !GlobalShowAllConnections);
                Debug.Log($"{port1.portColor}");
                if (hidden) {
                    port1.portColor = new Color(15f / 255, 80f / 255, 255f / 255, 1f);
                    port2.portColor = new Color(15f / 255, 80f / 255, 255f / 255, 1f);
                    //port1.AddToClassList("port-hidable");
                    //port2.AddToClassList("port-hidable");
                } else {
                    port1.portColor = new Color(240 / 255f, 240 / 255f, 240 / 255f, 1f);
                    port2.portColor = new Color(240 / 255f, 240 / 255f, 240 / 255f, 1f);
                    //port1.RemoveFromClassList("port-hidable");
                    //port2.RemoveFromClassList("port-hidable");
                }
            }
        }
    }
}