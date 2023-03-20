using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Sugar;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.AbstractNodeEditor
{
    public interface IDefaultANENode
    {
        public ExtendedPort InputPort { get; }
    }
    
    public abstract class DefaultANENode<DATA> : ANENode, IDefaultANENode
    {
        protected DATA Data => (DATA)NodeData;

        public ExtendedPort InputPort { get; set; }

        protected VisualElement root;
        protected Toggle visibilityBtn;
        
        protected Label header;
        protected Label contentText;


        protected DefaultANENode(string uiFile) : base(uiFile)
        {
        }

        public override void CreateGUI()
        {
            
            root = this.Q<VisualElement>("root");
            header = this.Q<Label>("header-label");
            contentText = this.Q<Label>("content-text");
            visibilityBtn = this.Q<Toggle>("visibility-btn");
            visibilityBtn.RegisterValueChangedCallback(VisibilityChanged);

            InputPort = CreateInputPort();
        }

        protected virtual ExtendedPort CreateInputPort()
        {
            var inputPortC = this.Q<VisualElement>("input-port-container");
            
            ExtendedPort titlePort = ExtendedPort.CreateEPort(Graph, Data, Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, Graph.Presentation.OnPortsConnected, Graph.Presentation.OnPortsDisconnected);
            inputPortC.Add(titlePort);
            return titlePort;
        }
        
        protected virtual void VisibilityChanged(ChangeEvent<bool> evt)
        {
            root.SetHidden(!evt.newValue);
        }
        
    }
}