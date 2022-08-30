using System;

namespace ClassLibrary1
{
    public interface ISubscribable
    {
        public event Action<ISubscribable> OnChanged;
    }
    public class Subscribable<T> : ISubscribable
    {
        private T Value;
        public event Action<ISubscribable> OnChanged; 

        public Subscribable(){}
        public Subscribable(T value){
            Value = value;
        }

        public void Set(T newValue)
        {
            if (!Value.Equals(newValue))
            {
                OnChanged?.Invoke(this);
            }
        }

        public T Get(){
            return Value;
        }
    }
}