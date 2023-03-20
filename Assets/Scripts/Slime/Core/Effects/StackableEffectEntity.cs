namespace ROR.Core
{
    public class StackableEffectEntity : EffectEntity
    {
        public override bool Join(EffectBar bar)
        {
            var joined = false;
            bar.Effects.ForCurrentAndFuture((x) =>
            {
                if (x.Definition == Definition)
                {
                    joined = true;
                    x.Stacks += Stacks;
                    return false;
                }

                return true;
            });
            return joined;
        }
    }
}