using ClassLibrary1.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1 {
    class SimpleAnswerArgs : AnswerArgs {
        private QuestionXml Question;
        private AnswerXml Answer;

        public SimpleAnswerArgs(QuestionXml question, AnswerXml answer) {
            Question = question;
            Answer = answer;
        }

        public string GenerateString() {
            return Answer.Text;
        }

        public bool IsVisible() {
            return true;
        }

        public void OnSelected() {
            //DO NOTHING
        }

        public static AnswerArgsFx Fx = (QuestionXml question, AnswerXml answer) => { return new SimpleAnswerArgs(question, answer); };
    }
}
