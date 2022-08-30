using ClassLibrary1.Logic;
using ClassLibrary1.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1 {
    class SimpleQuestionArgs : QuestionArgs {
        private QuestionXml Question;

        public SimpleQuestionArgs(QuestionXml question) {
            Question = question;
        }

        public override string GenerateString() {
            return Question.Text;
        }

        public override bool IsVisible() {
            return true;
        }

        public static QuestionArgsFx Fx = (QuestionXml question) => { return new SimpleQuestionArgs(question); };
    }
}
