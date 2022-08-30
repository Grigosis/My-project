using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor.Views;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;
using Combinator;
using DS.Windows;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using SecondCycleGame.Assets.Scripts.ANEImpl.Impls;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor.Impls
{
    public class CombinatorANENode : ANEMultiNode<CombinatorScriptable, CombinatorScriptable, CombinatorRowView>
    {
        private Toggle graph;
        private Label typeOutLabel;
        private Label typeInLabel;

        public ICombinator Combinator;

        public void SetCombinator(ICombinator combinator)
        {
            if (Combinator != null)
            {
                Combinator.OnChanged -= CombinatorOnOnChanged;
                Combinator.Destroy();
            }

            Combinator = combinator;
            if (Combinator != null)
            {
                combinator.OnChanged += CombinatorOnOnChanged;
            }
            CombinatorOnOnChanged(combinator);
        }

        public CombinatorANENode(string path) : base(path) { }

        protected override List<CombinatorScriptable> GetSubNodes()
        {
            return Dialog.Nodes;
        }

        public override void CreateGUI()
        {
            base.CreateGUI();
            typeOutLabel = this.Q<Label>("type-out");
            typeInLabel = this.Q<Label>("type-in");
            
            //graph = this.Q<Toggle>("graph");
            //graph.RegisterValueChangedCallback(GraphChanged);
            
            BuildCombinator();
            UpdateUI();
            ConnectPorts();
        }
        
        private void CombinatorOnOnChanged(ICombinator obj)
        {
            if (Combinator == null)
            {
                if (Dialog.Fx == "Constant")
                {
                    header.text = "`"+Dialog.Value+"`";
                }
                else
                {
                    header.text = "Combinator is NULL";
                }
                
            }
            else
            {
                header.text = Combinator.RawValue?.ToString() ?? "NULL";
            }
        }
        
        

        public void UpdateUI()
        {
            CombinatorOnOnChanged(Combinator);
            
            contentText.text = Dialog.Fx;
            if (F.Functions.TryGetValue(Dialog.Fx, out var fxInfo))
            {
                typeOutLabel.text = fxInfo.OutType?.Name ?? "NULL";
                typeInLabel.text = fxInfo.InType?.Name ?? "NULL";
                addSubnodeButton.SetHidden(false);
                subnodesContainer.SetHidden(false);
            }
            else
            {
                if (Dialog.Fx == "Constant")
                {
                    typeOutLabel.text = "Any";
                    typeInLabel.text = "";
                    addSubnodeButton.SetHidden(true);
                    subnodesContainer.SetHidden(true);
                }
                else
                {
                    typeOutLabel.text = "ERROR";
                    typeInLabel.text = "ERROR";
                    addSubnodeButton.SetHidden(false);
                    subnodesContainer.SetHidden(true);
                }
            }

            foreach (var port in Data2ToPorts.KeysAndValues)
            {
                var node = Graph.NodesAndData.Get(port.Key);
                if (node is CombinatorANENode cnode)
                {
                    port.Value.SetCombinator(cnode.Combinator);
                    port.Value.UpdateUI();
                } 
            }
        }

        public override void Unselect(VisualElement selectionContainer)
        {
            base.Unselect(selectionContainer);
            BuildCombinator();
        }


        protected override void CreateAnswerView(CombinatorScriptable answer, bool isNew)
        {
            base.CreateAnswerView(answer, isNew);
            if (isNew)
            {
                Graph.Presentation.CreateNode(answer, typeof(CombinatorANENode), GetPosition().position + new Vector2(200,200), null);
                ConnectPorts();
            }
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            UpdateUI();
            BuildCombinator();
        }

        public override void OnPortsConnected(ExtendedPort output, ExtendedPort input)
        {
            if (input == output)
            {
                Debug.LogWarning($"OnPortsSelfConnected : {input.Data} => {output.Data}");
                return;
            }

            if (input.Data is CombinatorScriptable child && output.Data is CombinatorScriptable root)
            {
                
                if (!root.Nodes.Contains(child))
                {
                    root.Nodes.Add(child);
                    BuildCombinator();
                }
            }
            else
            {
                throw new Exception($"Wrong output/input ({output.Data} | {input.Data})");
            }
        }


        public void OnSubNodeNullify(VisualElement view, CombinatorScriptable onDelete)
        {
            var rw = (view as CombinatorRowView);
            if (rw == null) return;
            rw.EPort.DisconnectAll();

            
            Data2ToPorts.Remove(onDelete);
            rw.SetData(null);
        }

        public override void OnPortsDisconnected(ExtendedPort output, ExtendedPort input)
        {
            Debug.LogWarning($"OnPortsDisconnected");
            if (input.Data is CombinatorScriptable child && output.Data is CombinatorScriptable root)
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
                Debug.LogWarning($"Wrong output/input ({output.Data} | {input.Data})");
            }
        }

        protected override void VisibilityChanged(ChangeEvent<bool> evt)
        {
            //base.VisibilityChanged(evt);
            
        }

        public void BuildCombinator()
        {
            BuildCombinator(Dialog);
        }
        public void BuildCombinator(CombinatorScriptable forObj)
        {
            var top = GetTopMostParent(Graph, forObj).Dialog;
            BuildCombinator(top, Graph);
        }
        
        private void TextVisibilityChanged(ChangeEvent<bool> evt)
        {
            contentText.style.display = new StyleEnum<DisplayStyle>(evt.newValue ? StyleKeyword.None : StyleKeyword.Auto);
        }

        public override void ConnectPorts()
        {
            Debug.Log("Connect PORTS");
            foreach (var answerToPort in Data2ToPorts.KeysAndValues)
            {
                var node = Graph.NodesAndData.Get(answerToPort.Key);
                var dialogNodeInfo = node as CombinatorANENode;
                if (dialogNodeInfo == null)
                {
                    continue;
                }
                
                var edge = answerToPort.Value.EPort.ConnectTo(dialogNodeInfo.InputPort);
                Graph.AddElement(edge);
            }
        }


        
        #region Combinator
        
        public static CombinatorANENode GetTopMostParent(ANEGraph graph, CombinatorScriptable data)
        {
            var top = (CombinatorANENode) graph.NodesAndData.Get(data);
            while (true)
            {
                var other = top.InputPort.GetOther();
                if (other != null)
                {
                    var node = graph.NodesAndData.Get((ScriptableObject)other.Data);
                    if (node is CombinatorANENode cnode)
                    {
                        top = cnode;
                        continue;
                    }
                }
                
                break;
            }

            return top;
        }
        
        
        

        public static void BuildCombinator(CombinatorScriptable root, ANEGraph graph)
        {
            try
            {
                var presentation = graph.Presentation as DialogANEPresentation;
                var combinator = CombinatorBuilder.Build(root, typeof(string), new CombinatorBuilderRules(presentation.QuestContext, graph));
                combinator.SetLiveUpdates(true);
            }
            catch (Exception e)
            {
                var set = new HashSet<CombinatorScriptable>();
                root.GetAllChildNodes(set);
                set.Add(root);
                
                foreach (var cs in set)
                {
                    AttachCombinator(graph, cs, null);
                }
                
                Debug.LogWarning("Cant build graph:" + e);
            }
            
            UpdateUIRecusively(root, graph);
        }

        public static void AttachCombinator(ANEGraph graph, CombinatorScriptable data, ICombinator combi)
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
            }
            else
            {
                Debug.LogError("WTF?");
            }
        }
        
        public static void UpdateUIRecusively(CombinatorScriptable root, ANEGraph graph)
        {
            var set = new HashSet<CombinatorScriptable>();
            root.GetAllChildNodes(set);
            set.Add(root);

            foreach (var scriptable in set)
            {
                var node = graph.NodesAndData.Get(scriptable) as CombinatorANENode;
                if (node == null)
                {
                    Debug.LogError($"No Node {scriptable}");
                    continue;
                }
                    
                node.UpdateUI();
            }
        }

        #endregion  
        
        


    }
}