using System;
using ROR.Lib;

namespace RPGFight.Library
{
    public class DirtyList<T>
    {
        private T[] Data = new T[10];
        public int Count { get; private set; } = 0;
        private int MaxCount = 0;

        public void Add(T item)
        {
            SharpUtils.EnsureCapacity(ref Data, Count + 1);
            Data[Count] = item;
            Count++;
            MaxCount = Math.Max(Count, MaxCount);
        }

        public T Get(int index)
        {
            return Data[index];
        }

        public void Set(int index, T data)
        {
            Data[index] = data;
        }

        public T this[int key]
        {
            get => Get(key);
            set => Set(key, value);
        }

        public void Clear()
        {
            Count = 0;
        }

        public void RealClear()
        {
            for (var index = 0; index < MaxCount; index++)
            {
                Data[index] = default(T);
            }
        }
    }
}