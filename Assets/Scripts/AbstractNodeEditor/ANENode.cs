using System;
using System.Collections.Generic;
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
        public object NodeData;

        protected ANENode()
        {
        }

        protected ANENode(string uiFile) : base(uiFile)
        {
        }

        public int Group { get; set; }

        protected override void ExecuteDefaultAction(EventBase evt)
        {
            if (evt is DetachFromPanelEvent)
            {
                var ports = new List<ExtendedPort>();
                FindPorts(this, ports);
                foreach (var port in ports)
                {
                    port.DisconnectAll();
                }
                Debug.LogError($"DetachFromPaneslEvent:{ports.Count}");
            }
            base.ExecuteDefaultAction(evt);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());
        }

        public virtual void Initialize(ANEGraph graph, object nodeData, Vector2 position)
        {
            NodeData = nodeData;
            Graph = graph;
            SetPosition(new Rect(position, Vector2.zero));
            
        }

        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            var ports = new List<ExtendedPort>();
            FindPorts(this, ports);

            foreach (var port in ports)
            {
                var aneNode = port.GetFirstOfType<ANENode>();
                if (conditionFunc.Invoke(aneNode))
                {
                    collectedElementSet.Add(aneNode);
                }
            }
        }

        private void FindPorts(VisualElement element, List<ExtendedPort> ports)
        {
            foreach (var child in element.Children())
            {
                if (child is ExtendedPort ep)
                {
                    ports.Add(ep);
                }
                else
                {
                    FindPorts(child, ports);
                }
            }
        }
        
        
        public abstract void CreateGUI();
        public virtual void Draw()
        {
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
            CreateGUI();
        }

        public virtual void UpdateUI() { }

        public override void OnSelected()
        {
            base.OnSelected();
            Graph.OnNodeSelected(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            Graph.OnNodeDeselected(this);
            UpdateUI();
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