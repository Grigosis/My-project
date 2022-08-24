using System;
using System.Linq;
using DS.Windows;
using Slime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor
{
    public class ANENode : Node
    {
        public string ID { get; set; }

        protected ANEGraph graphView;
        private Color defaultBackgroundColor;
        public CombinatorScriptable Node;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public virtual void Initialize(string nodeName, ANEGraph dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

            Node = ScriptableObject.CreateInstance(typeof(CombinatorScriptable)) as CombinatorScriptable;

            SetPosition(new Rect(position, Vector2.zero));

            graphView = dsGraphView;
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            //mainContainer.AddToClassList("ds-node__main-container");
            //extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            Port titlePort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            Port titlePort2 = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            titleContainer.Add(titlePort);
            titleContainer.Add(titlePort2);
            
            Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);
            
            Port outPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
            outputContainer.Add(outPort);

            //titleContainer
            //inputContainer
            //outputContainer
            //extensionContainer
        }

        public override void OnSelected()
        {
            base.OnSelected();
            graphView.OnNodeSelected(this);
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

                graphView.DeleteElements(port.connections);
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
    }
}