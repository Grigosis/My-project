using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using ROR.Core.Serialization;
using RPGFight.Library;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DS.Windows
{
    public class ANEGraph : GraphView
    {
        private MiniMap miniMap;
        public ANEWindow EditorWindow;
        public ANEIPresentation Presentation;

        public DoubleDictionary<object, ANENode> NodesAndData = new DoubleDictionary<object, ANENode>(); 
        public Dictionary<int, ANEGroup> Groups = new Dictionary<int, ANEGroup>(); 

  
        public ANEGraph(ANEWindow editorWindow, ANEIPresentation presentation)
        {
            this.EditorWindow = editorWindow;
            this.Presentation = presentation;
            presentation.SetGraph(this);
            
            miniMap = new MiniMap()
            {
                anchored = true
            };

            miniMap.SetPosition(new Rect(15, 50, 200, 180));

            Add(miniMap);

            miniMap.visible = false;
            
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            presentation.CreateContextMenu();
            Clear();
        }

        public void OnNodeSelected(ANENode node)
        {
            EditorWindow.GetEditor().RequestEditObject(node.NodeData);
        }
        
        public void OnNodeDeselected(ANENode node)
        {
            EditorWindow.GetEditor().FinishEdit();
        }

        public ANEWindow.ObjectEditorWrapper GetEditor()
        {
            return EditorWindow.GetEditor();
        }
        
        public void ToggleMiniMap()
        {
            miniMap.visible = !miniMap.visible;
            
            elementsRemovedFromStackNode += ElementsRemovedFromStackNode;
        }

        private void ElementsRemovedFromStackNode(StackNode arg1, IEnumerable<GraphElement> arg2)
        {
            Debug.LogError($"Remove from {arg1}");
            foreach (var e in arg2)
            {
                if (e is ANENode node)
                {
                    NodesAndData.Remove(node);
                }

                if (e is ANEGroup group)
                {
                    Groups.Remove(group.ID);
                }
            }
        }



        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                {
                    return;
                }

                if (startPort.node == port.node)
                {
                    return;
                }

                if (startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition = EditorWindow.rootVisualElement.ChangeCoordinatesTo(EditorWindow.rootVisualElement.parent, mousePosition - EditorWindow.position.position);
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }
        
        public void Save(string folder, string fileName)
        {
            ANEGraphState graphData = R.CreateOrLoadAsset<ANEGraphState>($"{folder}/{fileName}", true);
            graphData.PresentationObject = Presentation.OnSerialize();

            graphData.Nodes.Clear();
            graphData.Groups.Clear();
            graphData.Data.Clear();
            graphData.PresentationObject = null;

            foreach (var nodeAndData in NodesAndData.KeysAndValues)
            {
                var copy = nodeAndData.Key;//Object.Instantiate(nodeAndData.Key);
                Debug.Log($"Saved Nodes: {copy}");
               
                var nodeState = new ANENodeState();
                nodeState.Position = nodeAndData.Value.GetPosition().position;
                nodeState.Data = copy;
                graphData.Data.Add(copy);
                
                
                if (copy is QuestDialog qd)
                {
                    foreach (var answer in qd.Answers)
                    {
                        graphData.Data.Add(answer);
                    }
                }
                
                nodeState.GroupId = nodeAndData.Value.Group;
                graphData.Nodes.Add(nodeState);
            }
            
            foreach (var group in Groups)
            {
                var nodeState = new ANEGroupState();
                nodeState.Position = group.Value.GetPosition().position;
                nodeState.Name = group.Value.title;
                nodeState.Id = group.Key;

                graphData.Groups.Add(nodeState);
            }
            
            EditorUtility.SetDirty(graphData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Clear()
        {
            Presentation.OnCreatedNew();
            NodesAndData.Clear();
            Groups.Clear();
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
        }

        public void Load(string folder, string fileName)
        {
            Clear();
            ANEGraphState graphData = R.CreateOrLoadAsset<ANEGraphState>($"{folder}/{fileName}");
            
            
            //Debug.LogError("Loaded:"+graphData.PresentationObject);
            
            Presentation.OnLoaded(graphData.PresentationObject);
            
            foreach (var group in graphData.Groups)
            {
                Presentation.CreateGroup(group.Name, group.Id, group.Position);
            }
            
            foreach (var node in graphData.Nodes)
            {
                Debug.Log($"Loaded Node: {node.Data}");
                Groups.TryGetValue(node.GroupId, out var groupNode);
                Presentation.RestoreNode(node, groupNode);
            }

            foreach (var node in graphData.Nodes)
            {
                if (node.Data != null)
                {
                    Presentation.ConnectPorts(node.Data);
                }
            }
        }
    }
}