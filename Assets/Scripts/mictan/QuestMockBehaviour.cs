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
            context = new QuestContext();

            var dialog = new QuestDialog();
            dialog.Text = "Как ты сегодня\r\n###\r\nХорошо или плохо...?\r\n###\r\nАлитоитпоичтичит итчпотичопитде отячлаи пчяидот яати.";
            dialog.Sounds = "/Assets/Resources/Sounds/sample-9s.wav\n/Assets/Resources/Sounds/sample-12s.wav\n/Assets/Resources/Sounds/sample-15s.wav";
            dialog.TextArgsFx = "SIMPLE";
            var dialogComb1 = new CombinatorData();
            dialogComb1.Fx = "Constant";
            dialogComb1.Value = "true";
            dialog.VisibilityCombinator = dialogComb1;
            dialog.Answers = new List<QuestAnswer>(2);
            {
                QuestAnswer ok = new QuestAnswer();
                ok.AnswerFx = "SIMPLE";
                ok.Text = "Хорошо ({MONEY})";
                ok.CombinatorData = dialogComb1;
                QuestDialog good = new QuestDialog();
                {
                    good.Text = "Хорошо...";
                    good.TextArgsFx = "SIMPLE";
                    good.VisibilityCombinator = dialogComb1;
                    good.Answers = new List<QuestAnswer>(1);

                    QuestAnswer back = new QuestAnswer();
                    back.AnswerFx = "SIMPLE";
                    back.Text = "Назад ({MONEY})";
                    back.CombinatorData = dialogComb1;
                    back.NextQuestionDialog = dialog;
                    good.Answers.Add(back);
                }
                ok.NextQuestionDialog = good;
                dialog.Answers.Add(ok);
                

                QuestAnswer nok = new QuestAnswer();
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