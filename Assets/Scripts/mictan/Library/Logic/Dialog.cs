using ClassLibrary1.Xml;
using Combinator;

namespace ClassLibrary1.Logic
{
    public class Dialog
    {
        public DialogXml Xml;

        public ICombinator<bool> Combinator;
        public Question StartQuestion;

        public bool IsVisible {
            get {
                return Combinator.Value;
            }
        }

        public Dialog(DialogXml xml){
            Xml = xml;
            //Combinator = (ICombinator<bool>)CombinatorBuilder.Build(xml.Combinator, typeof(bool), CombinatorFunctions.Parser, CombinatorFunctions.Subscriber);
            //Combinator.Subscribe();
            //Combinator.IsVisible.OnChanged += IsVisibleChanged;
            Combinator.OnChanged += IsVisibleChanged;
            StartQuestion = Dialogs.Questions[Xml.StartQuestionId];
        }

        private void IsVisibleChanged(ICombinator combinator){
            Dialogs.UpdateState(this);
        }
    }
}
