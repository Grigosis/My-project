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

            header.text = Dialog.Text;
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
            return Dialog.Answers;
        }

        public override void OnPortsConnected(ExtendedPort output, ExtendedPort input)
        {
            if (output.Data is QuestAnswer answer && input.Data is QuestDialog dialog)
            {
                answer.NextQuestionDialog = dialog;
            }
            else
            {
                Debug.LogError($"Wrong output/input ({output.Data} | {input.Data})");
            }
        }
        
        public override void OnPortsDisconnected(ExtendedPort output, ExtendedPort input)
        {
            if (output.Data is QuestAnswer answer && input.Data is QuestDialog dialog)
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
                var dialog = answerToPort.Key.NextQuestionDialog;
                if (dialog != null)
                {
                    var node = Graph.NodesAndData.Get(dialog);
                    var dialogNodeInfo = node as DialogAneNode;
                    var edge = answerToPort.Value.EPort.ConnectTo(dialogNodeInfo.InputPort);
                    Graph.AddElement(edge);
                }
            }
        }
    }
}