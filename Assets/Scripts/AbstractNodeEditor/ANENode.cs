using System.Linq;
using DS.Windows;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor
{
    public abstract class ANENode : Node
    {
        protected ANEGraph Graph;
        private Color defaultBackgroundColor;
        public Object NodeData;

        protected ANENode()
        {
        }

        protected ANENode(string uiFile) : base(uiFile)
        {
        }

        public int Group { get; set; }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public virtual void Initialize(ANEGraph graph, Object nodeData, Vector2 position)
        {
            NodeData = nodeData;
            Graph = graph;
            SetPosition(new Rect(position, Vector2.zero));
            
        }

        public abstract void CreateGUI();
        public virtual void Draw()
        {
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            CreateGUI();
            
            //mainContainer.AddToClassList("ds-node__main-container");
            //extensionContainer.AddToClassList("ds-node__extension-container");
            
            //titleContainer
            //inputContainer
            //outputContainer
            //extensionContainer
        }

        public override void OnSelected()
        {
            base.OnSelected();
            Graph.OnNodeSelected(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            Graph.OnNodeDeselected(this);
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                Graph.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();

            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }

        public abstract void ConnectPorts();
    }
}