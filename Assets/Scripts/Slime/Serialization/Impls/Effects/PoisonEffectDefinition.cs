using UnityEngine;

namespace ROR.Core.Serialization
{
        [CreateAssetMenu(fileName = "PoisonEffect", menuName = "GameItems/Effects/PoisonEffect", order = 51)]
        public class PoisonEffectDefinition : EffectDefinition 
        {
                public float Damage;
        }
}