using System;
using System.Collections.Generic;

namespace Combinator
{
    public class ContextCombinatorMultiNode<OBJ, CONTEXT, IN, OUT> : ACombinatorMultiNode<OBJ, IN,OUT>, ICombinator<OUT>, IMultiCombinator<IN, OUT>, IContextCombinator<CONTEXT>
    {
        private Func<OBJ, CONTEXT, List<IN>, OUT> Function;
        private CONTEXT Context;
        public ContextCombinatorMultiNode() { }

        private void ChildCombinatorChanged(ICombinator combinator)
        {
            MarkForRecalculate();
        }

        public void SetFx(object fx)
        {
            if (fx is Func<OBJ, CONTEXT, List<IN>, OUT> func)
            {
                SetFx(func);
            }
            else
            {
                throw new Exception($"Wrong Fx {typeof(Func<OBJ, List<IN>, OUT>)} / {fx.GetType()}");
            }
        }

        public void SetFx(Func<OBJ, CONTEXT, List<IN>, OUT> fx)
        {
            Function = fx;
            MarkForRecalculate();
        }

        public override OUT Calculate()
        {
            for (int i = 0; i < Combinators.Count; i++)
            {
                List[i] = Combinators[i].Value;
            }
            return Function.Invoke(obj, Context, List);
        }

        public void SetContext(object context)
        {
            foreach (var combinator in Combinators)
            {
                if (!combinator.IsDependent) continue;
                combinator.SetContext(context);
            }
            
            if (context is CONTEXT ctx)
            {
                SetFx(ctx);
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
    }
}