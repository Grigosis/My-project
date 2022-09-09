using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    public interface IPropertyEditor
    {
        void BuildGui(object o, VisualElement e);
    }
}