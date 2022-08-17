using Assets.Scripts.Slime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ROR.Core{
    public class EffectIcon : MonoBehaviour{
        public EffectEntity Target;
        private float m_duration;
        private Image m_icon;
        private Image m_progressbar;        
        private Text m_timerText;

        public void ChangeTarget(EffectEntity target){
            this.Target = target;
            if(m_icon != null){// inited
                UpdateImpl();
            }
        }

        // Start is called before the first frame update
        void Start(){
            m_icon = GetComponent<Image>();
            m_progressbar = transform.GetChild(0).GetComponent<Image>();
            m_timerText = transform.GetChild(1).GetComponent<Text>();
            if(Target != null){// target defined
                UpdateImpl();
            }
        }

        private void UpdateImpl(){
            m_duration = Target.Definition.Duration;
            m_icon.sprite = R.Instance.Load<Sprite>(Target.Definition.Icon);
            if(m_duration > 9999999){
                m_progressbar.enabled = false;
                m_timerText.enabled = false;
            } else {
                m_progressbar.enabled = true;
                m_timerText.enabled = true;
                float time = Target.Time;
                m_progressbar.fillAmount = 1f - (time / m_duration);
                //m_timerText.text = Controls.ToDurationString(m_duration - time);
            }
        }
    }
}