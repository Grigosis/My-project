using UnityEditor;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    public class TextFieldEditor : IPropertyEditor
    {
        private StringProperty m_property;
        private object obj;

        public TextFieldEditor(StringProperty property)
        {
            m_property = property;
        }

        public void BuildGui(object o, VisualElement e)
        {
            obj = o;
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor Default Resources/DialogueSystem/AnswerView.uxml");
            TemplateContainer ui = uiAsset.CloneTree();
            var textField = ui.Q<TextField>("element");
            textField.value = m_property.GetValue(o);
            textField.label = m_property.Name;
            textField.RegisterValueChangedCallback(OnChanged);
            e.Add(ui);
        }

        private void OnChanged(ChangeEvent<string> data)
        {
            m_property.SetValue.Invoke(obj, data.newValue);
        }
    }
}