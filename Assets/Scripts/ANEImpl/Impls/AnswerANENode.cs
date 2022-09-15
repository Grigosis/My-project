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
        
        public void UpdateUI()
        {
            contentText.text = Data.Text;
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            UpdateUI();
        }

        public override void ConnectPorts()
        {
            
        }

        public override void OnPortsConnected(ExtendedPort input, ExtendedPort output)
        {
            
        }

        public override void OnPortsDisconnected(ExtendedPort input, ExtendedPort output)
        {
            
        }
    }
}