using Assets.Scripts.AbstractNodeEditor;
using Combinator;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using SecondCycleGame.Assets.Scripts.ANEImpl.Impls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SecondCycleGame.Assets.Scripts.mictan {
    class QuestMockBehaviour : MonoBehaviour {
        public QuestDialogBehaviour behaviour;

        QuestContext context;
        void Start() {
            context = ScriptableObject.CreateInstance<QuestContext>();

            var dialog = ScriptableObject.CreateInstance<QuestDialog>();
            dialog.Text = "Как ты сегодня";
            dialog.TextArgsFx = "SIMPLE";
            var dialogComb1 = ScriptableObject.CreateInstance<CombinatorScriptable>();
            dialogComb1.Fx = "Constant";
            dialogComb1.Value = "true";
            dialog.VisibilityCombinator = (ICombinator<bool>)CombinatorBuilder.Build(dialogComb1, typeof(bool), new CombinatorBuilderRules(context, null));
            dialog.Answers = new List<QuestAnswer>(2);
            {
                QuestAnswer ok = ScriptableObject.CreateInstance<QuestAnswer>();
                ok.AnswerFx = "SIMPLE";
                ok.Text = "Хорошо ({MONEY})";
                ok.CombinatorData = dialogComb1;
                QuestDialog good = ScriptableObject.CreateInstance<QuestDialog>();
                {
                    good.Text = "Хорошо...";
                    good.TextArgsFx = "SIMPLE";
                    good.VisibilityCombinator = (ICombinator<bool>)CombinatorBuilder.Build(dialogComb1, typeof(bool), new CombinatorBuilderRules(context, null));
                    good.Answers = new List<QuestAnswer>(1);

                    QuestAnswer back = ScriptableObject.CreateInstance<QuestAnswer>();
                    back.AnswerFx = "SIMPLE";
                    back.Text = "Назад ({MONEY})";
                    back.CombinatorData = dialogComb1;
                    back.NextQuestionDialog = dialog;
                    good.Answers.Add(back);
                }
                ok.NextQuestionDialog = good;
                dialog.Answers.Add(ok);
                

                QuestAnswer nok = ScriptableObject.CreateInstance<QuestAnswer>();
                nok.AnswerFx = "SIMPLE";
                nok.Text = "Плохо ({MONEY})";
                nok.CombinatorData = dialogComb1;

                dialog.Answers.Add(nok);
            }

            behaviour.QuestContext = context;
            behaviour.SetDialog(dialog);
        }
    }
}