using Assets.Scripts.AbstractNodeEditor;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.AbstractNodeEditor
{
    public interface IRowListener<DATA>
    {
        void OnSubNodeDelete(VisualElement view, DATA onDelete);
        void OnSubNodeValueChanged(VisualElement view, DATA oldValue, DATA newValue);
        void OnEditRequest(VisualElement view, DATA onDelete);
        void OnMoveRequest(VisualElement view, DATA onDelete, int direction);
    }
}