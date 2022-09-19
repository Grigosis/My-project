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
    public class CombinatorANENode : ANEMultiNode<CombinatorData, CombinatorData, CombinatorRowView>
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

        protected override List<CombinatorData> GetSubNodes()
        {
            return Data.Nodes;
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
                if (Data.Fx == "Constant")
                {
                    header.text = "`"+Data.Value+"`";
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
        
        

        public override void UpdateUI()
        {
            CombinatorOnOnChanged(Combinator);
            
            contentText.text = Data.Fx;
            if (F.Functions.TryGetValue(Data.Fx, out var fxInfo))
            {
                typeOutLabel.text = fxInfo.OutType?.Name ?? "NULL";
                typeInLabel.text = fxInfo.InType?.Name ?? "NULL";
                addSubnodeButton.SetHidden(false);
                subnodesContainer.SetHidden(false);
            }
            else
            {
                if (Data.Fx == "Constant")
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


        protected override void CreateAnswerView(CombinatorData answer, bool isNew)
        {
            base.CreateAnswerView(answer, isNew);
            if (isNew)
            {
                Graph.Presentation.CreateNode(answer, typeof(CombinatorANENode), GetPosition().position + new Vector2(200,200), null);
                ConnectPorts();
            }
        }

        public void BuildCombinator()
        {
            (Graph.Presentation as DialogANEPresentation)?.BuildCombinator(Data);
        }

        public void OnSubNodeNullify(VisualElement view, CombinatorData onDelete)
        {
            var rw = (view as CombinatorRowView);
            if (rw == null) return;
            rw.EPort.DisconnectAll();

            
            Data2ToPorts.Remove(onDelete);
            rw.SetData(null);
        }

        protected override void VisibilityChanged(ChangeEvent<bool> evt)
        {
            //base.VisibilityChanged(evt);
            
        }
        
        public override void OnEditorFinished(object data)
        {
            BuildCombinator();
            base.OnEditorFinished(data);
            UpdateUI();
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
                    Debug.Log($"Connect PORTS [{answerToPort.Key}] not found");
                    continue;
                }
                
                var edge = answerToPort.Value.EPort.ConnectTo(dialogNodeInfo.InputPort);
                Graph.AddElement(edge);
            }
        }
    }
}