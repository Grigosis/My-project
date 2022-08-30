using System.Collections.Generic;

namespace RPGFight.Library
{
    public class DoubleDictionary<K1,K2>
    {
        private Dictionary<K1, K2> D1 = new Dictionary<K1, K2>();
        private Dictionary<K2, K1> D2 = new Dictionary<K2, K1>();


        public Dictionary<K1, K2>.KeyCollection Keys1 => D1.Keys;
        public Dictionary<K1, K2> KeysAndValues => D1;
        public Dictionary<K2, K1>.KeyCollection Keys2 => D2.Keys;
        
        public K2 Get(K1 k1)
        {

            if (D1.TryGetValue(k1, out var value))
            {
                return value;
            }

            return default(K2);
        } 
        
        public K1 Get(K2 k2)
        {
            if (D2.TryGetValue(k2, out var value))
            {
                return value;
            }

            return default(K1);
        }

        public void Add(K1 k1, K2 k2)
        {
            D1[k1] = k2;
            D2[k2] = k1;
        }

        public bool Remove(K1 k1)
        {
            if (!D1.TryGetValue(k1, out var k2))
            {
                return false;
            }

            D1.Remove(k1);
            D2.Remove(k2);
            return true;
        }
        
        public bool Remove(K2 k2)
        {
            if (!D2.TryGetValue(k2, out var k1))
            {
                return false;
            }

            D1.Remove(k1);
            D2.Remove(k2);
            return true;
        }

        public void Clear()
        {
            D2.Clear();
            D1.Clear();
        }
    }
}