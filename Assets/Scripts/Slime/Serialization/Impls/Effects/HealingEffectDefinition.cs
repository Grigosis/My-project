using UnityEngine;

namespace ROR.Core.Serialization
{
    [CreateAssetMenu(fileName = "HealingEffectDefinition", menuName = "GameItems/Effects/Healing", order = 51)]
    public class HealingEffectDefinition : EffectDefinition
    {
        public float Heal;
    }
}