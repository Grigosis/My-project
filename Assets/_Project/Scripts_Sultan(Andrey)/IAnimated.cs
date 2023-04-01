using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public interface IAnimated
    {
        public void SetBool(string animationName, bool trueOrFalse);

        public void SetTrigger(string animationName);

        public void SetInteger(string animationName, int value);

        public void SetFloat(string animationName, float value);
    }
}
