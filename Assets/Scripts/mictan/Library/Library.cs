using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using ClassLibrary1.Xml;
using ClassLibrary1.Logic;
using Assets.Scripts.Slime.Sugar;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using UnityEngine;

namespace ClassLibrary1
{
    public delegate QuestionArgs QuestionArgsFx(QuestionXml q);
    public delegate AnswerArgs AnswerArgsFx(QuestionXml q, AnswerXml a);
    public delegate void SelectionFx(Question q, Answer a);
    public delegate bool CombinatorFx(Dictionary<string, object> args);
    
    public class Library
    {
        private static Library m_Instance;
        public static Library Instance
        {
            get
            {
                lock (typeof(Library))
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new Library();
                    }

                    return m_Instance;
                }
            }
        }

        public QuestContext Context;


        private Library()
        {
            ANEGraphState graphData = R.CreateOrLoadAsset<ANEGraphState>($"Assets/Editor/DialogueSystem/Graphs/TestGraph");
            Debug.Log($"Library Instance: Loaded2: " + graphData.Data.Count);
            foreach (var obj in graphData.Data)
            {
                if (obj is QuestDialog qd)
                {
                    if (!string.IsNullOrEmpty(qd.Id))
                    {
                        RegisterDialog(qd);
                        Debug.Log($"Registered Dialog root {qd.Id}");
                    }
                }
            }
            
            Context = ScriptableObject.CreateInstance<QuestContext>();
            Context.Subscribables["MONEY"] = new Subscribable<double>(99);
            Context.Subscribables["STR"] = new Subscribable<double>(50);
        }

        private Dictionary<string, QuestDialog> DialogRoots = new Dictionary<string, QuestDialog>();//???
        
        public void RegisterDialog(QuestDialog dialog) {
            DialogRoots.AddOnce(dialog.Id, dialog);
        }
    }
}