using System;
using System.Collections.Generic;

namespace Combinator
{
    public class CombinatorMultiNode<OBJ, IN, OUT> : ACombinatorMultiNode<OBJ, IN,OUT>, ICombinator<OUT>, IMultiCombinator<IN, OUT>
    {
        private Func<OBJ, List<IN>, OUT> Function;

        public CombinatorMultiNode() { }

        private void ChildCombinatorChanged(ICombinator combinator)
        {
            MarkForRecalculate();
        }
        
        public void SetContext(object obj)
        {
            for (var i = 0; i < Combinators.Count; i++)
            {
                var combinator = Combinators[i];
                combinator.SetContext(obj);
            }
        }

        public void SetFx(object fx)
        {
            if (fx is Func<OBJ, List<IN>, OUT> func)
            {
                SetFx(func);
            }
            else
            {
                throw new Exception($"Wrong Fx {typeof(Func<OBJ, List<IN>, OUT>)} / {fx.GetType()}");
            }
        }

        public void SetFx(Func<OBJ, List<IN>, OUT> fx)
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
            return Function.Invoke(obj, List);
        }
    }
}