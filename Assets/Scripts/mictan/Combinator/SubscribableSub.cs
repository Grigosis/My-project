using ClassLibrary1;

namespace Combinator
{
    public class SubscribableSub : ISubscription
    {
        public ISubscribable Subscribable {get; private set; }
        public ICombinator Combinator {get; private set; }
                
        public SubscribableSub(ISubscribable value, ICombinator combinator)
        {
            Subscribable = value;
            Combinator = combinator;
        }

        public void Subscribe()
        {
            Subscribable.OnChanged += OnChanged;
        }

        private void OnChanged(ISubscribable subscribable)
        {
            Combinator.MarkForRecalculate();
        }

        public void UnSubscribe()
        {
            Subscribable.OnChanged -= OnChanged;
        }
    }
}