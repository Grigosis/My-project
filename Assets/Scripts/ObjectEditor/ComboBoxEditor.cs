using UnityEditor;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    public class ComboBoxEditor : IPropertyEditor
    {
        private StringProperty m_property;
        private object obj;

        public ComboBoxEditor(StringProperty property)
        {
            m_property = property;
        }

        public void BuildGui(object o, VisualElement e)
        {
            obj = o;
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor Default Resources/Editor/DropDown.uxml");
            TemplateContainer ui = uiAsset.CloneTree();
            var textField = ui.Q<DropdownField>("element");
            
            textField.value = m_property.GetValue(o);
            textField.label = m_property.Name;
            //textField.choices = m_property.Variants;
            textField.RegisterValueChangedCallback(OnChanged);
            e.Add(ui);
        }

        private void OnChanged(ChangeEvent<string> data)
        {
            m_property.SetValue.Invoke(obj, data.newValue);
        }
    }
}