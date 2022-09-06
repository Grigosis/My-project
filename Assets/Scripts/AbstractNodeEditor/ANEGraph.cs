using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;
using ROR.Core.Serialization;
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


      
        public void TestSave1()
        {
            try
            {
                TestDialog d1 = new TestDialog();
                TestDialog d2 = new TestDialog();
                TestDialog d3 = new TestDialog();
                TestDialog d4 = new TestDialog();

                var r = new System.Random();
                d1.Name = r.NextString(6);
                d2.Name = r.NextString(6); 
                d3.Name = r.NextString(6); 
                d4.Name = r.NextString(6); 
            
            
                d1.NextDialog = d2;
                d2.NextDialog = d3;
                d3.NextDialog = d4;
                d4.NextDialog = d1;
            
                d1.NextDialogs.Add(d1);
                d1.NextDialogs.Add(d2);
                d1.NextDialogs.Add(d3);
                d1.NextDialogs.Add(d4);
            
                Debug.Log("Before:"+d1);
            
                var s = JsonUtility.ToJson(d1);
                Debug.LogError(s);
                var d_1 = JsonUtility.FromJson<TestDialog>(s);

                Debug.Log("After:"+d_1);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        public void TestSave2()
        {
            try
            {
                QuestDialog qd1 = new QuestDialog();
                QuestDialog qd2 = new QuestDialog();
                var a = new QuestAnswer();
                a.NextQuestionDialog = qd2;
                qd1.Answers.Add(a);

                var s = JsonUtility.ToJson(qd1);
                Debug.LogError(s);
                var d1 = JsonUtility.FromJson<QuestDialog>(s);

                Debug.Log("After:"+d1);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Save1(string folder, string fileName)
        {
            ANEGraphState graphData = R.CreateOrLoadAsset<ANEGraphState>($"{folder}/{fileName}", true);
            graphData.PresentationObject = Presentation.OnSerialize();

            graphData.Nodes.Clear();
            graphData.Groups.Clear();
            graphData.Data.Clear();
            graphData.PresentationObject = null;

            foreach (var nodeAndData in NodesAndData.KeysAndValues)
            {
                var nodeState = new ANENodeState();
                nodeState.Position = nodeAndData.Value.GetPosition().position;
                nodeState.Data = nodeAndData.Key;
                Debug.Log($"Saved Nodes: {nodeAndData.Key}");
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
        
        

        public void Load(string folder, string fileName)
        {
            Load2(folder, fileName);
        }
        
        public void Save(string folder, string fileName)
        {
            //Save1(folder, fileName);
            Save2(folder, fileName);
            
        }

        public void Clear()
        {
            Presentation.OnCreatedNew();
            NodesAndData.Clear();
            Groups.Clear();
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
        }

        public void Load1(string folder, string fileName)
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
        
        
        
        private void Save2(string folder, string fileName)
        {
            var referenceSerializer = new ReferenceSerializer();
            ANEGraphStateJson graphData = new ANEGraphStateJson();
            graphData.PresentationObject = Presentation.OnSerialize();

            graphData.Nodes.Clear();
            graphData.Groups.Clear();
            graphData.PresentationObject = null;

            
            
            foreach (var nodeAndData in NodesAndData.KeysAndValues)
            {
                var nodeState = new ANENodeState();
                nodeState.Position = nodeAndData.Value.GetPosition().position;
                nodeState.Data = nodeAndData.Key;
                
                Debug.Log($"Saved Nodes: {nodeAndData.Key}");
                nodeState.GroupId = nodeAndData.Value.Group;
                graphData.Nodes.Add(nodeState);
                
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
            var t2 = JsonUtility.ToJson(referenceSerializer);
            
            Debug.LogError(t1);
            Debug.LogError(t2);
            
            File.WriteAllText($"{folder}/{fileName}.JSON", t1);
            File.WriteAllText($"{folder}/{fileName}2.JSON", t2);
        }
        
        public void Load2(string folder, string fileName)
        {
            Clear();
            var result1 = File.ReadAllText($"{folder}/{fileName}.JSON");
            var result2 = File.ReadAllText($"{folder}/{fileName}2.JSON");
            
            
            var graphData = JsonUtility.FromJson<ANEGraphStateJson>(result1);
            var serializer = graphData.ReferenceSerializer;//JsonUtility.FromJson<ANEGraphStateJson>(result2);
            
            

            Presentation.OnLoaded(graphData.PresentationObject);
            
            foreach (var group in graphData.Groups)
            {
                Presentation.CreateGroup(group.Name, group.Id, group.Position);
            }
            
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