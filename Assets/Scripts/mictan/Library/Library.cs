using System;
using System.Collections.Generic;
using ClassLibrary1.Xml;
using ClassLibrary1.Logic;
using Assets.Scripts.Slime.Sugar;

namespace ClassLibrary1
{
    public delegate QuestionArgs QuestionArgsFx(QuestionXml q);
    public delegate AnswerArgs AnswerArgsFx(QuestionXml q, AnswerXml a);
    public delegate void SelectionFx(Question q, Answer a);
    public delegate bool CombinatorFx(Dictionary<string, object> args);
    
    public class Library
    {
        public static Library Instance;
        
        private Dictionary<string, AnswerXml> Answers = new Dictionary<string, AnswerXml>();
        private Dictionary<string, QuestionXml> Questions = new Dictionary<string, QuestionXml>();
        
        private Dictionary<string, QuestionArgsFx> QuestionArgsFx = new Dictionary<string, QuestionArgsFx>();
        private Dictionary<string, AnswerArgsFx> AnswerArgsFx = new Dictionary<string, AnswerArgsFx>();

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