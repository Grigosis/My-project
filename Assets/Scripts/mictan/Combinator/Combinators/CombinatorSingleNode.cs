using System;

namespace Combinator
{
    
    
    public class CombinatorSingleNode<OBJ, OUT> : ACombinator<OBJ, OUT>, ICombinator<OUT>
    {
        private Func<OBJ, OUT> Function;
        public CombinatorSingleNode()  { }
        
        public void SetFx(object fx)
        {
            if (fx is Func<OBJ, OUT> func)
            {
                SetFx(func);
            }
            else
            {
                throw new Exception("Wrong Fx");
            }
        }

        public void SetContext(object obj)
        {
            //DOES NOTHING
        }

        public string GetDebugName()
        {
            return NodeDebugName;
        }

        public void SetFx(Func<OBJ, OUT> fx)
        {
            Function = fx;
            MarkForRecalculate();
        }

        public override OUT Calculate()
        {
            return Function.Invoke(obj);
        }

        public override string ToString()
        {
            return Value+"";
        }
    }
}