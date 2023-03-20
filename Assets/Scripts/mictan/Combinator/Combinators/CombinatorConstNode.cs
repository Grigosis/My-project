using System;

namespace Combinator
{
    public class CombinatorConstNode<OBJ, OUT> : ACombinator<OBJ, OUT>, IConstCombinator
    {
        public CombinatorConstNode() { }

        private OUT StoredValue;

        public override OUT Calculate()
        {
            return StoredValue;
        }

        public override void SetFx(object fx) { }
        
        public void SetValue(object obj)
        {
            if (obj is OUT ob)
            {
                SetValue(ob);
            }
            else
            {
                throw new Exception($"Wrong value type {obj}/{typeof(OUT)}");
            }
        }
        
        public void SetValue(OUT value)
        {
            StoredValue = value;
            MarkForRecalculate();
        }

        public override string GetDebugName()
        {
            return $"`{Value}`";
        }
        
        public override string ToString()
        {
            return $"{Value}";
        }
    }
}