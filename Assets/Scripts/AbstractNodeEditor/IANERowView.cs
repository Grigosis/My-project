using Assets.Scripts.AbstractNodeEditor;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.AbstractNodeEditor
{
    public interface IRowListener<DATA>
    {
        public void OnPortsConnected(ExtendedPort output, ExtendedPort input);
        public void OnPortsDisconnected(ExtendedPort output, ExtendedPort input);
        public void OnSubNodeDelete(VisualElement view, DATA onDelete);
        public void OnSubNodeValueChanged(VisualElement view, DATA oldValue, DATA newValue);
        public void OnEditRequest(VisualElement view, DATA onDelete);
        public void OnMoveRequest(VisualElement view, DATA onDelete, int direction);
    }
}