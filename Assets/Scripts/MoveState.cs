using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    [System.Serializable]
    public class MoveState
    {
        public string animatorBoolName;
        [Range(1, 10)] public float speed;
    }
}
