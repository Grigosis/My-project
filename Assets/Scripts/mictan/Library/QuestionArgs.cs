namespace ClassLibrary1
{
    public abstract class QuestionArgs
    {
        public abstract bool IsVisible();
        public abstract string GenerateString();
    }

    public class MockQuestionArgs : QuestionArgs
    {
        private string Text;
        private bool Visible;

        public MockQuestionArgs(string text, bool visible)
        {
            Text = text;
            Visible = visible;
        }

        public override bool IsVisible()
        {
            return Visible;
        }

        public override string GenerateString()
        {
            return Text;
        }
    }
}