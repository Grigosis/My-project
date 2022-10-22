using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;
using BlockEditor;
using ROR.Core.Serialization;
using ROR.Core.Serialization.Json;
using RPGFight.Library;
using Unity.Mathematics;
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

            OnElementsDeleted();
            
            presentation.CreateContextMenu();
            Clear();
        }

        public void OnNodeSelected(ANENode node)
        {
            EditorWindow.GetEditor().RequestEditObject(node.NodeData, node.OnEditorFinished);
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

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                
                
                List<ANEGroup> groupsToDelete = new List<ANEGroup>();
                List<ANENode> nodesToDelete = new List<ANENode>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is ANENode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (selectedElement is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                        continue;
                    }

                    if (selectedElement is ANEGroup group)
                    {
                        groupsToDelete.Add(group);
                        continue;
                    }
                }
                
                Debug.Log($"OnElementsDeleted {nodesToDelete.Count} {groupsToDelete.Count} {edgesToDelete.Count}");

                foreach (ANEGroup groupToDelete in groupsToDelete)
                {
                    List<ANEGroup> groupNodes = new List<ANEGroup>();

                    foreach (GraphElement groupElement in groupToDelete.containedElements)
                    {
                        if (!(groupElement is ANENode))
                        {
                            continue;
                        }

                        ANEGroup groupNode = (ANEGroup) groupElement;
                        groupNodes.Add(groupNode);
                    }

                    groupToDelete.RemoveElements(groupNodes);

                    RemoveGroup(groupToDelete);

                    RemoveElement(groupToDelete);
                }

                DeleteElements(edgesToDelete);

                foreach (ANENode nodeToDelete in nodesToDelete)
                {
                    //if (nodeToDelete.Group != null)
                    //{
                    //    nodeToDelete.Group.RemoveElement(nodeToDelete);
                    //}

                    RemoveUngroupedNode(nodeToDelete);

                    nodeToDelete.DisconnectAllPorts();

                    RemoveElement(nodeToDelete);
                }
            };
        }
        
        private void RemoveGroup(ANEGroup group)
        {
            
        }
        
        public void RemoveUngroupedNode(ANENode node)
        {
            node.DisconnectAllPorts();
            Debug.Log($"RemoveUngroupedNode {node.NodeData}");
            NodesAndData.Remove(node);
        }
        
        public override EventPropagation DeleteSelection()
        {
            return base.DeleteSelection();
        }

        public void Clear()
        {
            Presentation.OnCreatedNew();
            NodesAndData.Clear();
            Groups.Clear();
    
            while (true)
            {
                int count = 0;
                graphElements.ForEach(graphElement =>
                {
                    count++;
                    RemoveElement(graphElement);
                });
                if (count == 0)
                {
                    break;
                }
            }
        }
        
        
        public void Save(string fileName)
        {
            var referenceSerializer = new ReferenceSerializer();
            ANEGraphState graphData = new ANEGraphState();
            graphData.PresentationObject = Presentation.OnSerialize();

            foreach (var nodeAndData in NodesAndData.KeysAndValues)
            {
                var nodeState = new ANENodeState();
                nodeState.Position = nodeAndData.Value.GetPosition().position;
                nodeState.Data = nodeAndData.Key;
                nodeState.GroupId = nodeAndData.Value.Group;

                referenceSerializer.AddObject(nodeAndData.Key as Linkable);
                referenceSerializer.AddObject(nodeState);
            }

            foreach (var group in Groups)
            {
                var nodeState = new ANEGroupState();
                nodeState.Position = group.Value.GetPosition().position;
                nodeState.Name = group.Value.title;
                nodeState.Id = group.Key;

                graphData.Groups.Add(nodeState);
            }

            graphData.ReferenceSerializer = referenceSerializer;

            var t1 = JsonUtility.ToJson(graphData);

            var backup = "Assets/Database/Dialogs/Backups";
            
            File.WriteAllText($"{backup}/{Path.GetFileNameWithoutExtension(fileName)}_backup_{DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss")}.JSON", t1);
            
            File.WriteAllText($"{fileName}.JSON", t1);
            if (!Directory.Exists(backup))
            {
                Directory.CreateDirectory(backup);
            }
            
            Debug.Log("File saved");
        }

        private void LoadScriptsOneByOne(ReferenceSerializer serializer)
        {
            foreach (var obj in serializer.ObjectsForSerialize)
            {
                if (obj is QuestAnswer answer)
                {
                    answer.CompileScript();
                }
            }
        }
        
        private void LoadScripts(ReferenceSerializer serializer)
        {
            var scripts = new Dictionary<ScriptManager.Script, QuestAnswer>();
            foreach (var obj in serializer.ObjectsForSerialize)
            {
                if (obj is QuestAnswer answer)
                {
                    if (!string.IsNullOrEmpty(answer.Script))
                    {
                        var script = new ScriptManager.Script("node_"+answer.Guid, answer.Script);
                        scripts.Add(script, answer);
                    } 
                }
            }

            ScriptManager.CompileAndCreateFunctions(scripts.Keys.ToList());
            foreach (var kv in scripts)
            {
                kv.Value.Scriptable = (Action) kv.Key.Invoker;
            }
        }
        
        public void Load(string fileName)
        {
            Clear();
            var result1 = File.ReadAllText($"{fileName}.JSON");
            
            var graphData = JsonUtility.FromJson<ANEGraphState>(result1);
            var serializer = graphData.ReferenceSerializer;

            Presentation.OnLoaded(graphData.PresentationObject);
            
            foreach (var group in graphData.Groups)
            {
                Presentation.CreateGroup(group.Name, group.Id, group.Position);
            }

            LoadScriptsOneByOne(serializer);

            foreach (var obj in serializer.ObjectsForSerialize)
            {
                if (obj is ANENodeState node)
                {
                    Debug.Log($"Loaded Node: [{node}] [{node.GUID}] [{node.DataGUID}] [{node.Data}]");
                    Groups.TryGetValue(node.GroupId, out var groupNode);
                    Presentation.RestoreNode(node, groupNode);
                }
            }
            
            foreach (var obj in serializer.ObjectsForSerialize)
            {
                if (obj is ANENodeState node)
                {
                    if (node.Data != null)
                    {
                        Presentation.ConnectPorts(node.Data);
                    }
                }
            }
        }
    }
}