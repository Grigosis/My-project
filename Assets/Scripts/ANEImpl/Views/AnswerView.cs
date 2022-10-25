﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Sugar;
using BlockEditor;
using DS.Windows;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using SecondCycleGame.Assets.Scripts.ANEImpl.Views;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor.Views
{
    public class AnswerView : RowView<QuestDialog, QuestAnswer>
    {
        public AnswerView() : base("Assets/Editor Default Resources/DialogueSystem/AnswerView.uxml") { } 
        public Button ScriptBtn;

        protected override void BindPortData()
        {
            EPort.Data = Data;
            EPort.Data2 = Data;
        }

        public override void OnEditorFinished(QuestAnswer editedObject)
        {
            base.OnEditorFinished(editedObject);
            editedObject.CompileScript();
        }



        protected override ExtendedPort CreatePort(VisualElement element)
        {
            return ExtendedPort.CreateEPort(GraphView, Data, Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, GraphView.Presentation.OnPortsConnected, GraphView.Presentation.OnPortsDisconnected);
        }

        public override void Init(ANEGraph graph, QuestDialog pdata, QuestAnswer data, IRowListener<QuestAnswer> listener)
        {
            base.Init(graph, pdata, data, listener);
            ScriptBtn = this.Q<Button>("script-btn");
            ScriptBtn.clicked += ScriptBtnOnClicked;
            
            EPort.OnConnected += EPortOnOnConnected;
            UpdateUI();
        }

        private void EPortOnOnConnected(ExtendedPort arg1, ExtendedPort arg2)
        {
            Debug.Log($"EPortOnOnConnected {arg1} {arg2}");
            var edge = arg1.GetEdge(arg2);
            if (edge == null)
            {
                Debug.Log($"Edge not found {arg1} {arg2}");
                return;
            }
            
            
            edge.showInMiniMap = true;
            edge.SetHidden(true);
            arg1.AddToClassList("port-hidable");   
            arg2.AddToClassList("port-hidable");  
        }

        private void ScriptBtnOnClicked()
        {
            if (Data.Scriptable != null)
            {
                Data.Scriptable.Invoke();
            }
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            
            Text.text = Data.Text;

            if (string.IsNullOrEmpty(Data.Script))
            {
                ScriptBtn.SetHidden(true);
            }
            else
            {
                ScriptBtn.SetHidden(false);
                
                if (Data.Scriptable != null)
                {
                    ScriptBtn.AddToClassList("success"); 
                    ScriptBtn.RemoveFromClassList("failure"); 
                }
                else
                {
                    ScriptBtn.RemoveFromClassList("success");
                    ScriptBtn.AddToClassList("failure");
                }
                
            }
        }

    }
}