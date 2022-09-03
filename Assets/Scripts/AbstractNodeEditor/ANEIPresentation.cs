using System;
using Assets.Scripts.AbstractNodeEditor.Impls;
using DS.Windows;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Assets.Scripts.AbstractNodeEditor
{
    public abstract class ANEIPresentation
    {
        protected ANEGraph Graph;

        protected ANEIPresentation() {}

        public void SetGraph(ANEGraph graph)
        {
            Graph = graph;
        }
        
        public abstract void OnNewObjectCreated(Object o);
        public abstract void CreateContextMenu();
        
        public virtual void ConnectPorts(Object obj)
        {
            var aneNode = Graph.NodesAndData.Get(obj);
            if (aneNode == null)
            {
                Debug.LogError($"Node not found! {obj}");
            }
            aneNode.ConnectPorts();
        }

        public abstract Object OnSerialize();
        public abstract void OnLoaded(object obj);
        public abstract void OnCreatedNew();
        
        protected void AppendToMenu(string actionTitle, Action<DropdownMenuAction, Vector2> action)
        {
            ContextualMenuManipulator manipulator = new ContextualMenuManipulator(menuEvent =>
            {
                menuEvent.menu.AppendAction(actionTitle, (actionEvent)=>action(actionEvent, Graph.GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)));
            });
            Graph.AddManipulator(manipulator);
        }
        
        public void CreateNode(Type type, Type typeOfNode, Vector2 position, ANEGroup group)
        {
            var obj = (Object)ScriptableObject.CreateInstance(type);
            OnNewObjectCreated(obj);
            CreateNode(obj, typeOfNode, position, group);
        }

        public void CreateNode(Object obj, Type typeOfNode, Vector2 position, ANEGroup group)
        {
            ANENode node;
            if (typeOfNode == typeof(DialogAneNode))
            {
                node = new DialogAneNode("Assets/Editor Default Resources/DialogueSystem/NodeView.uxml");
            }
            else if (typeOfNode == typeof(CombinatorANENode) )
            {
                node = new CombinatorANENode("Assets/Editor Default Resources/DialogueSystem/CombinatorNodeView.uxml");
            }
            else
            {
                throw new Exception();
            }
            //var node = (ANENode)Activator.CreateInstance(typeOfNode);
            
            node.Initialize(Graph, obj, position);
            
            Graph.NodesAndData.Add(obj, node);
            
            node.Draw();

            Graph.AddElement(node);
            if (group != null)
            {
                group.AddElement(node);
            }
        }
        
        public void CreateGroup(String name, int id, Vector2 position)
        {
            var obj = new ANEGroup(name, id, position);
            Graph.Groups.Add(id, obj);
            Graph.AddElement(obj);
        }

        public abstract void RestoreNode(ANENodeState group, ANEGroup groupNode);
    }

    
}