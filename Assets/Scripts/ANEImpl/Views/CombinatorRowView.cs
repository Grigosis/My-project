using Combinator;
using SecondCycleGame.Assets.Scripts.ANEImpl.Views;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor.Views
{
    public class CombinatorRowView : RowView<CombinatorData, CombinatorData>
    {
        public ICombinator CombinatorImpl;

        public CombinatorRowView() : base("Assets/Editor Default Resources/DialogueSystem/CombinatorRowView.uxml") { }

        protected override void BindPortData()
        {
            EPort.Data = ParentData;
            EPort.Data2 = Data;
        }

        protected override ExtendedPort CreatePort(VisualElement container)
        {
            return ExtendedPort.CreateEPort(ParentData, Orientation.Horizontal, Direction.Output, Port.Capacity.Single, OnConnected, OnDisconnected);
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            OnCombinatorValueChanged(CombinatorImpl);
        }

        public void SetCombinator(ICombinator impl)
        {
            if (CombinatorImpl != null)
            {
                CombinatorImpl.OnChanged -= OnCombinatorValueChanged;
            }

            CombinatorImpl = impl;
            if (impl != null)
            {
                CombinatorImpl.OnChanged += OnCombinatorValueChanged;
            }
            OnCombinatorValueChanged(impl);
        }

        private void OnCombinatorValueChanged(ICombinator obj)
        {
            if (obj == null)
            {
                Text.text = "Combinator is NULL";
                return;
            }
            else
            {
                Text.text = obj.RawValue == null ? "NULL" : obj.RawValue.ToString();
            }
            
        }
    }
}