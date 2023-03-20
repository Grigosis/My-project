using System;

namespace Combinator
{
    public interface ICombinator<OUT> : ICombinator
    {
        OUT Value { get; }
    }

    public interface IConstCombinator
    {
        void SetValue(object obj);
    }

    public interface ISubscription
    {
        public void Subscribe();
        public void UnSubscribe();
    }

    public interface ICombinator
    {
        public bool IsDependent { get; set; }
        public string NodeDebugName { get; set; }
        public object RawValue { get; }
        event Action<ICombinator> OnChanged;
        public void MarkForRecalculate();
        public void SetFx(object fx);
        public void SetSubscription(ISubscription subscription);
        public void SetObject(object obj);
        public void SetContext(object obj);

        public string GetDebugName();
        //public void SetContext(object obj);
        //public void SetContextDependent(object obj);
        public void SetLiveUpdates(bool liveUpdates, bool recalculate = true);
        public void Destroy();
    }

    public interface IContextCombinator<CONTEXT>
    {
        public void SetContext(CONTEXT context);
    }
    
    public interface IContextCombinator
    {
        public void SetContext(object context);
    }

    public interface IMultiCombinator<IN, OUT> : IMultiCombinator, ICombinator<OUT>
    {
        void AddNode(ICombinator<IN> combinator);
    }
    
    public interface IMultiCombinator : ICombinator
    {
        void AddNode(ICombinator combinator);
    }
}