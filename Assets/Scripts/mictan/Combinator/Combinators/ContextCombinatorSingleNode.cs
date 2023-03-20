using System;

namespace Combinator
{
    public class ContextCombinatorSingleNode<OBJ, CONTEXT, OUT> : ACombinator<OBJ, OUT>, IContextCombinator<CONTEXT>
    {
        private Func<OBJ,CONTEXT, OUT> Function;
        private CONTEXT Context;
        public ContextCombinatorSingleNode()  { }
        
        public override void SetFx(object fx)
        {
            if (fx is Func<OBJ,CONTEXT, OUT> func)
            {
                SetFx(func);
            }
            else
            {
                throw new Exception("Wrong Fx");
            }
        }

        public void SetFx(Func<OBJ,CONTEXT, OUT> fx)
        {
            Function = fx;
            MarkForRecalculate();
        }

        public override OUT Calculate()
        {
            return Function.Invoke(obj, Context);
        }

        public override void SetContext(object context)
        {
            if (context is CONTEXT ctx)
            {
                SetContext(ctx);
            }
        }

        public void SetContext(CONTEXT context)
        {
           if (!Equals(Context,context))
           {
               Context = context;
               MarkForRecalculate();
           }
        }
        
        public override string GetDebugName()
        {
            return NodeDebugName;
        }
        
        public override string ToString()
        {
            return Value+"";
        }
    }
}