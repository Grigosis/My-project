using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    [System.Serializable]
    public class MoveStateData
    {
        //public MoveType moveType;
        public string animatorBoolName;
        [Range(1, 10)] public float speed;
    }
}
