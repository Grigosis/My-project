using UnityEngine;

namespace ROR.Core.Serialization
{
    [CreateAssetMenu(fileName = "EffectDefinition", menuName = "GameItems/Effects/Base", order = 51)]
    public class EffectDefinition : BaseDefinition 
    {
        [SerializeField]
        public float Duration = -1;
        
        [SerializeField]
        public float TickInterval = 0;
        
        [SerializeField]
        public int MaxStacks = -1;

        public bool IsInfiniteStacks => MaxStacks < 0;
        public bool IsInfiniteDuration => Duration < 0;
        public bool HasInterval => TickInterval > 0;
    }
}