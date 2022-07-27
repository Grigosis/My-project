using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    public static class T
    {
        public static string ToDurationString(this int time)
        {
            return "" + time;
        }

        public static string ToKeyName(this KeyCode key)
        {
            return "" + key;
        }
    }
}
