using System;
using System.Collections.Generic;
using ClassLibrary1.Xml;
using ClassLibrary1.Logic;
using Assets.Scripts.Slime.Sugar;
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
        }
        
        private Dictionary<string, AnswerXml> Answers = new Dictionary<string, AnswerXml>();
        private Dictionary<string, QuestionXml> Questions = new Dictionary<string, QuestionXml>();
        
        public Dictionary<string, QuestionArgsFx> QuestionArgsFx = new Dictionary<string, QuestionArgsFx>();
        public Dictionary<string, AnswerArgsFx> AnswerArgsFx = new Dictionary<string, AnswerArgsFx>();
        public Dictionary<string, SelectionFx> SelectionFx = new Dictionary<string, SelectionFx>();

        private Dictionary<string, CombinatorFx> CombinatorFx = new Dictionary<string, CombinatorFx>();

        public CombinatorFx GetCombinatorFx(string name)
        {
            if (CombinatorFx.TryGetValue(name, out var cmb))
            {
                return cmb;
            }
            else
            {
                throw new Exception($"CombinatorFx not found:{name}");
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
        
        public void Register(AnswerXml answerXml)
        {
            Answers.AddOnce(answerXml.Id, answerXml);
        }

        public void Register(QuestionXml questionXml)
        {
            Questions.AddOnce(questionXml.Id, questionXml);
        }
    }
}