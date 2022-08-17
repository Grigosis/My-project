using System;

namespace Combinator
{
    public class CombinatorConstNode<OBJ, OUT> : ACombinator<OBJ, OUT>, ICombinator<OUT>, IConstCombinator
    {
        public CombinatorConstNode() { }

        private OUT StoredValue;

        public override OUT Calculate()
        {
            return StoredValue;
        }

        public void SetFx(object fx) { }
        
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

        public void SetContext(object obj)
        {
            //DOES NOTHING
        }
        
        public string GetDebugName()
        {
            return $"`{Value}`";
        }
        
        public override string ToString()
        {
            return $"{Value}";
        }
    }
}