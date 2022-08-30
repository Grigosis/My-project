using ClassLibrary1.Xml;
using Combinator;

namespace ClassLibrary1.Logic
{
    public class Dialog
    {
        public DialogXml Xml;

        public ICombinator<bool> VisibleCombinator;
        public Question StartQuestion;

        public bool IsVisible {
            get {
                return VisibleCombinator.Value;
            }
        }

        public Dialog(DialogXml xml){
            Xml = xml;
            //Combinator = (ICombinator<bool>)CombinatorBuilder.Build(xml.Combinator, typeof(bool), CombinatorFunctions.Parser, CombinatorFunctions.Subscriber);
            //Combinator.Subscribe();
            //Combinator.IsVisible.OnChanged += IsVisibleChanged;
            VisibleCombinator.OnChanged += IsVisibleChanged;
            StartQuestion = Dialogs.Questions[Xml.StartQuestionId];
        }

        private void IsVisibleChanged(ICombinator combinator){
            Dialogs.UpdateState(this);
        }
    }
}
