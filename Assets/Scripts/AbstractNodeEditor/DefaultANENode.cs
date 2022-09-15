using Assets.Scripts.AbstractNodeEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.AbstractNodeEditor
{
    public abstract class DefaultANENode<DATA> : ANENode
    {
        protected DATA Data => (DATA)NodeData;
        
        public ExtendedPort InputPort;
        
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
            ExtendedPort titlePort = ExtendedPort.CreateEPort(Data, Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, OnPortsConnected, OnPortsDisconnected);
            inputPortC.Add(titlePort);
            return titlePort;
        }
        
        protected virtual void VisibilityChanged(ChangeEvent<bool> evt)
        {
            root.style.display = new StyleEnum<DisplayStyle>(evt.newValue ? StyleKeyword.None : StyleKeyword.Auto);
        }
        
        public abstract void OnPortsConnected(ExtendedPort input, ExtendedPort output);
        public abstract void OnPortsDisconnected(ExtendedPort input, ExtendedPort output);
    }
}