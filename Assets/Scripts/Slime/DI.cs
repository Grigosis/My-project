using ROR.Core;
using UnityEngine;

namespace RPGFight
{
    public class DI
    {
        public static void CreateFloatingText(LivingEntity entity, string text, Color color, float textSize = 14f)
        {
            FloatingText.Create(entity.GameObjectLink.transform.position + entity.GameObjectLink.transform.transform.up * 8, text, color, textSize);
        }
    }
}