using System;
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
            RegisterFx("TestSelectionFx", ((question, answer) =>
            {
                Debug.LogError($"SelectionFx {question} {answer}");
            }));
            
            RegisterFx("TestSelectionFx2", ((question, answer) =>
            {
                Debug.LogError($"SelectionFx2 {question} {answer}");
            }));
            
            RegisterFx("TestAnswerArgsFx", ((questionxml, answer) =>
            {
                Debug.LogError($"AnswerArgsFx {questionxml} {answer}");
                return new MockAnswerArgs(true, "TestAnswerArgs");
            }));

            RegisterFx("TestAnswerArgsFx2", ((questionxml, answer) =>
            {
                Debug.LogError($"AnswerArgsFx2 {questionxml} {answer}");
                return new MockAnswerArgs(false, "TestAnswerArgs2");
            }));
            
            RegisterFx("MockQuestionFx1", ((questionxml) =>
            {
                Debug.LogError($"AnswerArgsFx {questionxml}");
                return new MockQuestionArgs("TestAnswerArgs", true);
            }));

            RegisterFx("MockQuestionFx2", ((questionxml) =>
            {
                Debug.LogError($"MockQuestionArgs2 {questionxml}");
                return new MockQuestionArgs("MockQuestionArgs2", false);
            }));
            
            Library.Instance.RegisterFx("SIMPLE", SimpleQuestionArgs.Fx);
            Library.Instance.RegisterFx("SIMPLE", SimpleAnswerArgs.Fx);

            ANEGraphData graphData = R.CreateOrLoadAsset<ANEGraphData>($"Assets/Editor/DialogueSystem/Graphs/TestGraph.DATA.assert");

            int x = 0;
       
            foreach (var obj in graphData.Data)
            {
                if (obj is QuestDialog qd)
                {
                    Library.Instance.RegisterDialogRoot($"dialog-{x}", qd);
                }
            }
            
            Context = ScriptableObject.CreateInstance<QuestContext>();
            Context.GLOBAL_VALUES["MONEY"] = new Subscribable<double>(99);
            Context.GLOBAL_VALUES["STR"] = new Subscribable<double>(50);
        }

        private Dictionary<string, QuestDialog> DialogRoots = new Dictionary<string, QuestDialog>();//???

        public Dictionary<string, QuestionArgsFx> QuestionArgsFx = new Dictionary<string, QuestionArgsFx>();
        public Dictionary<string, AnswerArgsFx> AnswerArgsFx = new Dictionary<string, AnswerArgsFx>();
        public Dictionary<string, SelectionFx> SelectionFx = new Dictionary<string, SelectionFx>();

        private Dictionary<string, CombinatorFx> CombinatorFx = new Dictionary<string, CombinatorFx>();

        public CombinatorFx GetCombinatorFx(string name) {
            if (CombinatorFx.TryGetValue(name, out var cmb)) {
                return cmb;
            } else {
                throw new Exception($"CombinatorFx not found:{name}");
            }
        }

        public QuestionArgsFx GetQuestionArgsFx(string name) {
            if (QuestionArgsFx.TryGetValue(name, out var cmb)) {
                return cmb;
            } else {
                throw new Exception($"QuestionArgsFx not found:{name}");
            }
        }

        public AnswerArgsFx GetAnswerArgsFx(string name) {
            if (AnswerArgsFx.TryGetValue(name, out var cmb)) {
                return cmb;
            } else {
                throw new Exception($"AnswerArgsFx not found:{name}");
            }
        }

        public void RegisterFx (string name, QuestionArgsFx fx) {
            QuestionArgsFx.AddOnce(name, fx);
        }
        
        public void RegisterFx (string name, AnswerArgsFx fx) {
            AnswerArgsFx.AddOnce(name, fx);
        }
        
        public void RegisterFx (string name, SelectionFx fx) {
            SelectionFx.AddOnce(name, fx);
        }
        
        public void RegisterFx (string name, CombinatorFx fx) {
            CombinatorFx.AddOnce(name, fx);
        }
        
        public void RegisterDialogRoot(string name, QuestDialog dialog) {
            DialogRoots.AddOnce(name, dialog);
        }
    }
}