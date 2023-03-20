using UnityEngine;
using UnityEngine.UI;

namespace ROR.Core
{
    public class SimpleBar : MonoBehaviour{
        public Slider slider;
        public Image fill;

        public void SetValues(double now, double max, float min = 0)
        {
            SetValues((float)now, (float)max, (float)min);
        }
        
        public void SetValues(float now, float max, float min = 0)
        {
            slider.maxValue = max;
            slider.minValue = min;
            slider.value = now;
        }
    }
}