using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor.Views;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;
using Combinator;
using DS.Windows;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using SecondCycleGame.Assets.Scripts.ANEImpl.Impls;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor.Impls
{
    public class AnswerANENode : DefaultANENode<NPCAnswer>
    {
        public AnswerANENode(string path) : base(path) { }
        
        public override void UpdateUI()
        {
            string headerTxt;

            if (string.IsNullOrEmpty(Data.Npc))
            {
                if (string.IsNullOrEmpty(Data.DialogId))
                {
                    headerTxt = "NULL";
                }
                else
                {
                    headerTxt = Data.DialogId;
                }
            }
            else
            {
                headerTxt = Data.Npc;
            }
            
            contentText.text = Data.Text ?? "EMPTY";
            header.text = headerTxt.TrimCount(20);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            UpdateUI();
        }

        protected override ExtendedPort CreateInputPort()
        {
            var inputPortC = this.Q<VisualElement>("input-port-container");
            ExtendedPort titlePort = ExtendedPort.CreateEPort(Graph, Data, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, Graph.Presentation.OnPortsConnected, Graph.Presentation.OnPortsDisconnected);
            inputPortC.Add(titlePort);
            return titlePort;
        }

        public override void ConnectPorts()
        {
            if (Data.NextQuestionDialog != null)
            {
                var node = Graph.NodesAndData.Get(Data.NextQuestionDialog);
                var dialogNodeInfo = node as DialogAneNode;
                var edge = InputPort.ConnectTo(dialogNodeInfo.InputPort);
                Graph.AddElement(edge);
            }
        }
    }
}