using System;

namespace ClassLibrary1
{
    public class Subscribable<T>
    {
        private T Value;
        public event Action<T, T> OnChanged; 

        public Subscribable(){}
        public Subscribable(T value){
            Value = value;
        }

        public void Set(T newValue)
        {
            if (!Value.Equals(newValue))
            {
                OnChanged?.Invoke(Value, newValue);
            }
        }

        public T Get(){
            return Value;
        }
    }
}