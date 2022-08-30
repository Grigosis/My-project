using Assets.Scripts.Library;
using ClassLibrary1;
using ClassLibrary1.Logic;
using ClassLibrary1.Xml;
using Combinator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//TODO Регистрация в static?
class QuestRegisterBehaviour : MonoBehaviour {

    void Start() {
        CombinatorFunctions.Register();

        Library.Instance.RegisterFx("SIMPLE", SimpleQuestionArgs.Fx);
        Library.Instance.RegisterFx("SIMPLE", SimpleAnswerArgs.Fx);

        GameDataXml data = D.Instance.ReadFrom<GameDataXml>("Mictan-Dialog.xml");
        foreach (AcheivementValueXml v in data.AcheivementValues) {
            if (v is AcheivementDoubleValueXml) {
                Acheivents.Instance.RegisterDouble(v.Id, ((AcheivementDoubleValueXml) v).Value);
            }
        }
        foreach (QuestionXml q in data.Questions) {
            Library.Instance.Register(q);
        }
        foreach (AnswerXml a in data.Answers) {
            Library.Instance.Register(a);
        }
        foreach (DialogXml d in data.Dialogs) {
            Library.Instance.Register(d);
        }
    }
}
