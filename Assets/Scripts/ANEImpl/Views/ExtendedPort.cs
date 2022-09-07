using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Sugar;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

namespace Assets.Scripts.AbstractNodeEditor
{
    class MyIEdgeConnectorListener : IEdgeConnectorListener
    {
        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            throw new NotImplementedException();
        }
    }
    
    public class ExtendedPort : Port
    {
        
        protected ExtendedPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
            DebugName = new Random().NextString(8);
            
        }
        

        public event Action<ExtendedPort, ExtendedPort> OnConnected; 
        public event Action<ExtendedPort, ExtendedPort> OnDisconnected; 
        public object Data;
        public object Data2;
        public string DebugName;

        public ExtendedPort GetOther()
        {
            foreach (var edge in connections)
            {
                if (edge.input == this) return (ExtendedPort)edge.output;
                if (edge.output == this) return (ExtendedPort)edge.input;
            }

            return null;
        }

        public override void Connect(Edge edge)
        {
            if (!connections.Contains(edge))
            {
                base.Connect(edge);
                if (edge.input.direction == Direction.Input)
                {
                    OnConnected?.Invoke((ExtendedPort)edge.output, (ExtendedPort)edge.input);
                }
                else
                {
                    OnConnected?.Invoke((ExtendedPort)edge.input, (ExtendedPort)edge.output);
                }
            }
        }

        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);
            DisconnectInternal(edge);
        }

        private void DisconnectInternal(Edge edge)
        {
            if (edge.input.direction == Direction.Input)
            {
                OnDisconnected?.Invoke((ExtendedPort)edge.output, (ExtendedPort)edge.input);
            }
            else
            {
                OnDisconnected?.Invoke((ExtendedPort)edge.input, (ExtendedPort)edge.output);
            }
        }

        public override void DisconnectAll()
        {
            foreach (var edge in connections)
            {
                DisconnectInternal(edge);
            }
            
            base.DisconnectAll();
        }

        public static ExtendedPort CreateEPort(object data, Orientation orientation, Direction direction, Port.Capacity capacity, Action<ExtendedPort, ExtendedPort> onConnected, Action<ExtendedPort, ExtendedPort> onDisconnected = null,string text = "")
        {
            if (data == null)
            {
                throw new Exception("Data is null");
            }
            
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            ExtendedPort ele = new ExtendedPort(orientation, direction, capacity, typeof(bool))
            {
                m_EdgeConnector = new EdgeConnector<Edge>(listener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            ele.Data = data;
            ele.m_ConnectorText.text = text;
            if (onConnected != null)
            {
                ele.OnConnected += onConnected;
            }
            if (onDisconnected != null)
            {
                ele.OnDisconnected += onDisconnected;
            }
            
            return ele;
        }
        
        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
            {
              private GraphViewChange m_GraphViewChange;
              private List<Edge> m_EdgesToCreate;
              private List<GraphElement> m_EdgesToDelete;
        
              public DefaultEdgeConnectorListener()
              {
                this.m_EdgesToCreate = new List<Edge>();
                this.m_EdgesToDelete = new List<GraphElement>();
                this.m_GraphViewChange.edgesToCreate = this.m_EdgesToCreate;
              }
        
              public void OnDropOutsidePort(Edge edge, Vector2 position)
              {
              }
        
              public void OnDrop(UnityEditor.Experimental.GraphView.GraphView graphView, Edge edge)
              {
                this.m_EdgesToCreate.Clear();
                this.m_EdgesToCreate.Add(edge);
                this.m_EdgesToDelete.Clear();
                if (edge.input.capacity == Port.Capacity.Single)
                {
                  foreach (Edge connection in edge.input.connections)
                  {
                    if (connection != edge)
                      this.m_EdgesToDelete.Add((GraphElement) connection);
                  }
                }
                if (edge.output.capacity == Port.Capacity.Single)
                {
                  foreach (Edge connection in edge.output.connections)
                  {
                    if (connection != edge)
                      this.m_EdgesToDelete.Add((GraphElement) connection);
                  }
                }
                if (this.m_EdgesToDelete.Count > 0)
                  graphView.DeleteElements((IEnumerable<GraphElement>) this.m_EdgesToDelete);
                List<Edge> edgesToCreate = this.m_EdgesToCreate;
                if (graphView.graphViewChanged != null)
                  edgesToCreate = graphView.graphViewChanged(this.m_GraphViewChange).edgesToCreate;
                foreach (Edge edge1 in edgesToCreate)
                {
                  graphView.AddElement((GraphElement) edge1);
                  edge.input.Connect(edge1);
                  edge.output.Connect(edge1);
                }
              }
            }

        public void DisconnectOfType<T>(GraphView view, T excluded) where T : class
        {
            var connectionsCopy = new List<Edge>(connections);
            foreach (var edge in connectionsCopy)
            {
                var other = edge.input;
                if (other == this) other = edge.output;

                var port = other as ExtendedPort;
                if (port.Data is T t && excluded != t)
                {
                    this.Disconnect(edge);
                    other.Disconnect(edge);
                    view.RemoveElement(edge);
                }
            }
        }
    }
}