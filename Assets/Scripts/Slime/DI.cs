using System;
using ROR.Core;
using UnityEngine;

namespace RPGFight
{
    public class DI
    {
        public static void CreateFloatingText(LivingEntity entity, string text, Color color)
        {
            Console.WriteLine($"CreateFloatingText:{entity.DebugName}/{entity.EntityId}:{text}");
        }
    }
}