using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.AbstractNodeEditor.Impls;
using Assets.Scripts.Slime.Sugar;
using Combinator;
using DS.Windows;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace SecondCycleGame.Assets.Scripts.ANEImpl.Impls
{
    public class DialogANEPresentation : ANEIPresentation
    {
        public QuestContext QuestContext;
        
        
        public override object OnSerialize()
        {
            return QuestContext; 
        }

        public override void OnLoaded(object obj)
        {
            QuestContext = new QuestContext();
        }

        public override void OnCreatedNew()
        {
            QuestContext = new QuestContext();
        }


        public override void CreateContextMenu()
        {
            AppendToMenu("Add Dialog Starter", (actionEvent, position) => CreateNode(typeof(NPCAnswer), typeof(AnswerANENode), position, null));
            AppendToMenu("Add Dialog", (actionEvent, position) => CreateNode(typeof(QuestDialog), typeof(DialogAneNode), position, null));
            AppendToMenu("Add Combinator", (actionEvent, position) => CreateNode(typeof(CombinatorData), typeof(CombinatorANENode), position, null));
            AppendToMenu("Create Group", (actionEvent, position) => CreateGroup("Unnamed group", new Random().Next(), position));
        }

        public override void RestoreNode(ANENodeState group, ANEGroup groupNode)
        {
            var data = group.Data;
            if (data is QuestDialog qd)
            {
                CreateNode(qd, typeof(DialogAneNode), group.Position, groupNode);
            }
            if (data is CombinatorData cs)
            {
                CreateNode(cs, typeof(CombinatorANENode), group.Position, groupNode);
            }
            if (data is NPCAnswer ass)
            {
                CreateNode(ass, typeof(AnswerANENode), group.Position, groupNode);
            }
        }
        
        public override void OnNewObjectCreated(object o)
        {
            if (o is QuestDialog qd)
            {
                var a = new QuestAnswer(); 
                a.Text = "Hi!";
                a.AnswerFx = "Test";
                qd.Answers.Add(a);
            }
            
            if (o is CombinatorData cs)
            {
                cs.Value = "";
                cs.Fx = "Constant";
            }
            
            if (o is NPCAnswer ass)
            {
                ass.Text = "Starter";
            }
        }
        
        public override void OnPortsConnected(ExtendedPort output, ExtendedPort input)
        {
            if (input == output)
            {
                Debug.LogWarning($"OnPortsSelfConnected : {input.Data} => {output.Data}");
                return;
            }
            
            
            if (output.Data is QuestAnswer answer2 && input.Data is CombinatorData combinator)
            {
                output.DisconnectOfType<CombinatorData>(Graph, combinator);
                answer2.CombinatorData = combinator;
            } 
            else if (output.Data is QuestAnswer answer && input.Data is QuestDialog dialog)
            {
                output.DisconnectOfType<QuestDialog>(Graph, dialog);
                answer.NextQuestionDialog = dialog;
            } 
            else
            {
                Debug.LogError($"Wrong output/input ({output.Data} | {input.Data})");
            }
        }
        
        public override void OnPortsDisconnected(ExtendedPort output, ExtendedPort input)
        {
            if (output.Data is QuestAnswer answer2 && input.Data is CombinatorData combinator)
            {
                answer2.CombinatorData = null;
            }
            else if (output.Data is QuestAnswer answer && input.Data is QuestDialog dialog)
            {
                answer.NextQuestionDialog = null;
            }
            else if (input.Data is CombinatorData child && output.Data is CombinatorData root)
            {
                if (root.Nodes.Contains(child))
                {
                    Debug.LogWarning($"Remove connection : {root} => {child}");
                    root.Nodes.Remove(child);
                    var node = Graph.NodesAndData.Get(root) as CombinatorANENode;
                    if (node == null)
                    {
                        Debug.LogWarning($"Unable to find node: {root}");
                    }
                    else
                    {
                        var view = node.Data2ToPorts.Get(child);
                        node.OnSubNodeNullify(view, child);
                    }
                    
                    BuildCombinator(child);
                    BuildCombinator(root);
                }
                else
                {
                    Debug.LogWarning($"Remove already removed : {root} => {child}");
                }
            }
            else
            {
                Debug.LogError($"Wrong output/input ({output.Data} | {input.Data})");
            }
        }

        public T GetTopMostParent<T>(object data) where T : class, IDefaultANENode
        {
            var top = Graph.NodesAndData.Get(data) as T;
            if (top == null) return null;
            while (true)
            {
                var other = top.InputPort.GetOther();
                if (other != null)
                {
                    var node = Graph.NodesAndData.Get(other.Data);
                    if (node is T cnode)
                    {
                        top = cnode;
                        continue;
                    }
                }
                
                break;
            }

            return top;
        }


        public void UpdateUIRecusively(ANENode node, Func<ANENode, bool> shouldUpdateUI, HashSet<VisualElement> roots = null)
        {
            if (roots == null)
            {
                roots = new HashSet<VisualElement>();
            }
            
            node.UpdateUI();
            var ports = node.GetAllPorts();
            foreach (var port in ports)
            {
                var other = port.GetOther();
                if (other != null)
                {
                    var childnode = other.GetFirstOfType<ANENode>();
                    if (roots.Contains(childnode)) continue;
                    if (shouldUpdateUI(childnode))
                    {
                        roots.Add(childnode);
                        UpdateUIRecusively(childnode, shouldUpdateUI, roots);
                    }
                } 
            }
        }
        
        public void BuildCombinator(CombinatorData forObj)
        {
            var top = GetTopMostParent<CombinatorANENode>(forObj);
            if (top != null)
            {
                BuildCombinator(top.Data, Graph);
            }
        }

        public void BuildCombinator(CombinatorData root, ANEGraph graph)
        {
            try
            {
                var combinator = CombinatorBuilder.Build(root, typeof(string), new CombinatorBuilderRules(new QuestContext(), graph));
                combinator.SetLiveUpdates(true);
            }
            catch (Exception e)
            {
                var set = new HashSet<CombinatorData>();
                root.GetAllChildNodes(set);
                set.Add(root);
                
                foreach (var cs in set)
                {
                    AttachCombinator(graph, cs, null);
                }
                
                Debug.LogWarning("Cant build graph:" + e);
            }

            var node = graph.NodesAndData.Get(root);
            UpdateUIRecusively(node, (x) => x is CombinatorANENode);
        }

        public static void AttachCombinator(ANEGraph graph, CombinatorData data, ICombinator combi)
        {
            var node = graph.NodesAndData.Get(data);
            if (node == null)
            {
                Debug.LogError("Node not found:" + node);
                return;
            }

            if (node is CombinatorANENode nodee)
            {
                nodee.SetCombinator(combi);
                if (combi != null)
                {
                    nodee.style.backgroundColor = new StyleColor(Color.gray);
                }
            }
            else
            {
                Debug.LogError("WTF?");
            }
        }
    }
}