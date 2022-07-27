namespace ROR.Core.Serialization
{
    public class EffectDefinition : BaseDefinition 
    {
        public float Duration = -1;
        public float TickInterval = 0;
        public int MaxStacks = -1;
        public SClass<EffectEntity> EntityClass;

        public bool IsInfiniteStacks => MaxStacks < 0;
        public bool IsInfiniteDuration => Duration < 0;
        public bool HasInterval => TickInterval > 0;
    }
}