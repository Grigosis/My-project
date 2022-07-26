using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    public class R
    {
        public static T Load<T>(string path) where T : Object
        {
            var r = Resources.Load<T>(path);
            if (r == null)
            {
                Debug.LogError($"Doesn't exist: [{path}]");
            }
            return r;
        }
    }
}