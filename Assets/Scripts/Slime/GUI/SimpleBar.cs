using UnityEngine;
using UnityEngine.UI;

namespace ROR.Core
{
    public class SimpleBar : MonoBehaviour{
        public Slider slider;
        public Image fill;

        public double maxValue {
            get => slider.maxValue;
            set { slider.maxValue = (float)value;}
        }

        public void setColor(Color color){
            fill.color = color;
        }

        public void setValue(double newValue){
            slider.value = (float)newValue;
        }
    }
}