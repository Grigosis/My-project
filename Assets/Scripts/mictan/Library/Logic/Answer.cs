﻿using ClassLibrary1.Xml;

namespace ClassLibrary1.Logic
{
    
    public class Answer
    {
        public AnswerXml Xml;
        
        public AnswerArgsFx AnswerFx; //Id функции возвращающей AnswerArgs
        public string Requirements = "STR:20";//TODO parse
        
        public SelectionFx SelectionFx; // Когда выбрали ответ

        public Answer(AnswerXml xml) {
            Xml = xml;
            AnswerFx = Library.Instance.GetAnswerArgsFx(xml.AnswerFx);
            Requirements = xml.Requirements;
            //SelectionFx = xml.SelectionFx;
        }
    }
    
}