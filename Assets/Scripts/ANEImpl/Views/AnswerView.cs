using DS.Windows;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using SecondCycleGame.Assets.Scripts.ANEImpl.Views;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor.Views
{
    public class AnswerView : RowView<QuestDialog, QuestAnswer>
    {
        public AnswerView() : base("Assets/Editor Default Resources/DialogueSystem/AnswerView.uxml") { }


        protected override void BindPortData()
        {
            EPort.Data = Data;
            EPort.Data2 = Data;
        }

        protected override ExtendedPort CreatePort(VisualElement element)
        {
            return ExtendedPort.CreateEPort(Data, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, GraphView.Presentation.OnPortsConnected, GraphView.Presentation.OnPortsDisconnected);
        }

        public override void Init(ANEGraph graph, QuestDialog pdata, QuestAnswer data, IRowListener<QuestAnswer> listener)
        {
            base.Init(graph, pdata, data, listener);
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Text.text = Data.Text;
        }

    }
}