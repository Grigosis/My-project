using System;
using System.Collections.Generic;

namespace ROR.Lib
{
    public class LList<T>
    {
        public readonly List<T> Data = new List<T>();
        public readonly List<T> Remove = new List<T>();
        public readonly List<T> Add = new List<T>();

        public void AddItem(T t)
        {
            Add.Add(t);
        }
        
        public void RemoveItem(T t)
        {
            Remove.Add(t);
        }

        
        public void Apply()
        {
            foreach (var r in Remove)
            {
                Data.Remove(r);
            }
            Remove.RemoveAll(t => !Data.Contains(t));
            
            foreach (var r in Add)
            {
                Data.Add(r);
            }
            Add.RemoveAll(t => Data.Contains(t));
        }

        public int Count()
        {
            return Data.Count;
        }

        public void ForCurrentAndFuture(Func<T, bool> iter)
        {
            foreach (var item in Add)
            {
                if (!iter(item)) return;
            }
            foreach (var item in Data)
            {
                if (!iter(item)) return;
            }
        }
    }
}