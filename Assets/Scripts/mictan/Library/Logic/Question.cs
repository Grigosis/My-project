using Assets.Scripts.Slime.Core;
using ClassLibrary1.Xml;

namespace ClassLibrary1.Logic
{
    public class Question
    {
        public QuestionXml Xml;

        public QuestionArgsFx QuestionFx;

        public Answer[] answers;

        public string QuestionText {
            get {
                return QuestionFx(Xml).GenerateString();
            }
        }

        public Question(QuestionXml xml) {
            Xml = xml;
            QuestionFx = F.GetQuestionArgsFx(xml.TextArgsFx);
            answers = new Answer[xml.Answers.Count];
            for(int i = 0; i < answers.Length; i++) {
                answers[i] = Dialogs.Answers[xml.Answers[i]];
            }
        }
    }
}