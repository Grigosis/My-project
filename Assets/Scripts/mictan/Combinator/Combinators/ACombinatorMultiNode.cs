using System;
using System.Collections.Generic;
using System.Text;

namespace Combinator
{
    public abstract class ACombinatorMultiNode<OBJ, IN,OUT> : ACombinator<OBJ, OUT>
    {
        protected List<IN> List = new List<IN>();
        protected List<ICombinator<IN>> Combinators = new List<ICombinator<IN>>();
        
        
        public void AddNode(ICombinator<IN> combinator)
        {
            Combinators.Add(combinator);
            List.Add(default(IN));
            combinator.OnChanged += ChildCombinatorChanged;
        }

        private void ChildCombinatorChanged(ICombinator combinator)
        {
            MarkForRecalculate();
        }

        public void AddNode(ICombinator combinator)
        {
            if (combinator is ICombinator<IN> comb)
            {
                AddNode(comb);
            }
            else
            {
                throw new Exception($"Wrong type of combinator {combinator}");
            }
        }

        public override void SetLiveUpdates(bool liveUpdates, bool recalculate = true)
        {
            foreach (var combinator in Combinators)
            {
                combinator.SetLiveUpdates(liveUpdates, recalculate);
            }
            
            base.SetLiveUpdates(liveUpdates, recalculate);
        }

        public override string GetDebugName()
        {
            var sb = new StringBuilder();
            sb.Append($"{NodeDebugName}(");
            for (var i = 0; i < Combinators.Count; i++)
            {
                var combinator = Combinators[i];
                sb.Append(combinator.GetDebugName());
                if (i != Combinators.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(")");

            return sb.ToString();
        }

        public override string ToString()
        {
            return Value+"";
        }
    }
}