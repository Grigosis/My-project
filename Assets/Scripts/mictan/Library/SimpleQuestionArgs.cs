using ClassLibrary1.Xml;

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
