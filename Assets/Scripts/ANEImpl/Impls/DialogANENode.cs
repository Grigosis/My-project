using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor.Views;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.AbstractNodeEditor.Impls
{
    public class DialogAneNode : ANEMultiNode<QuestDialog, QuestAnswer, AnswerView>
    {
        //UI
        private Toggle answersVisibilityBtn;
        private Toggle textVisibilityBtn;

        public DialogAneNode(string path) : base(path) { }
        
        public override void CreateGUI()
        {
            base.CreateGUI();
            answersVisibilityBtn = this.Q<Toggle>("answers-visibility-btn");
            textVisibilityBtn = this.Q<Toggle>("text-visibility-btn");

            answersVisibilityBtn.RegisterValueChangedCallback(AnswersVisibilityChanged);
            textVisibilityBtn.RegisterValueChangedCallback(TextVisibilityChanged);

            header.text = Data.Text;
        }


        private void AnswersVisibilityChanged(ChangeEvent<bool> evt)
        {
            //answersContainer.visible = evt.newValue;
            subnodesContainer.style.display = new StyleEnum<DisplayStyle>(evt.newValue ? StyleKeyword.None : StyleKeyword.Auto);
        }
        
        private void TextVisibilityChanged(ChangeEvent<bool> evt)
        {
            contentText.style.display = new StyleEnum<DisplayStyle>(evt.newValue ? StyleKeyword.None : StyleKeyword.Auto);
        }

        protected override List<QuestAnswer> GetSubNodes()
        {
            return Data.Answers;
        }



        public override void UpdateUI()
        {
            base.UpdateUI();
            var txt = Data.Text ?? "NULL";
            header.text = txt.Substring(0,Math.Min(txt.Length, 20));
            contentText.text = txt;
        }
        
        public override void OnPortsConnected(ExtendedPort output, ExtendedPort input)
        {
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
            else
            {
                Debug.LogError($"Wrong output/input ({output.Data} | {input.Data})");
            }
        }

        public override void ConnectPorts()
        {
            foreach (var answerToPort in Data2ToPorts.KeysAndValues)
            {
                var answer = answerToPort.Key;
                if (answer.NextQuestionDialog != null)
                {
                    var node = Graph.NodesAndData.Get(answer.NextQuestionDialog);
                    var dialogNodeInfo = node as DialogAneNode;
                    var edge = answerToPort.Value.EPort.ConnectTo(dialogNodeInfo.InputPort);
                    Graph.AddElement(edge);
                }

                if (answer.CombinatorData != null)
                {
                    var node = Graph.NodesAndData.Get(answer.CombinatorData);
                    var combinator = node as CombinatorANENode;
                    var edge = answerToPort.Value.EPort.ConnectTo(combinator.InputPort); 
                    Graph.AddElement(edge);
                }
            }
        }
    }
}