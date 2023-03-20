using System;

namespace Combinator
{
    public class CombinatorSingleNode<OBJ, OUT> : ACombinator<OBJ, OUT>
    {
        private Func<OBJ, OUT> Function;
        public CombinatorSingleNode()  { }
        
        public override void SetFx(object fx)
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

        public override string GetDebugName()
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