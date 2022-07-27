using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Slime.Core;

namespace ROR.Core
{
    public class SkillBtn : MonoBehaviour{
        public SkillEntity Skill{
            get => m_skill;
            set{
                m_skill = value;
                if (m_skill == null)
                {
                    Image = DefaultSprite;
                }
                else
                {
                    Image = R.Load<Sprite>(value.Definition.Icon);
                }
            }
        }
        public KeyCode Key = KeyCode.None;
        public Sprite Image{
            get => gameObject.GetComponent<Image>().sprite;
            set => gameObject.GetComponent<Image>().sprite = value;
        }

        public Sprite DefaultSprite;
    
        public SkillEntity m_skill;
        public Image m_progressbar;        
        public Text m_timerText;
        public Image m_codeTextBg;
        public Text m_codeText;
    
        void Start()
        {
            gameObject.GetComponent<Image>().sprite = m_skill == null ? DefaultSprite : Image = R.Load<Sprite>(m_skill.Definition.Icon);

            //if(Controls.kkToString.ContainsKey(Key)){
            //    
            //    
            //    m_codeText.text = Controls.kkToString[Key];
            //    m_codeTextBg.rectTransform.sizeDelta = new Vector2(m_codeText.preferredWidth * m_codeText.transform.localScale.x + 2, m_codeTextBg.rectTransform.sizeDelta.y);
            //} else {
            //    m_codeText.enabled = false;
            //    m_codeTextBg.enabled = false;
            //}
        }
    
        // Update is called once per frame
        void Update(){
            if (m_skill == null)
            {
                return;
            }
            
            if (Key != KeyCode.None)
            {
                m_codeText.enabled = true;
                m_codeTextBg.enabled = true;
                m_codeText.text = Key.ToKeyName();
                m_codeTextBg.rectTransform.sizeDelta = new Vector2(m_codeText.preferredWidth * m_codeText.transform.localScale.x + 2, m_codeTextBg.rectTransform.sizeDelta.y);
            }
            else
            {
                m_codeText.enabled = false;
                m_codeTextBg.enabled = false;
            }
            
            if(m_skill.Cooldown <= 0){
                m_progressbar.enabled = false;
                m_timerText.enabled = false;
            } else {
                m_progressbar.fillAmount = (float)m_skill.Cooldown / (float)m_skill.Definition.Cooldown;
                m_timerText.text = m_skill.Cooldown.ToDurationString();
                m_progressbar.enabled = true;
                m_timerText.enabled = true;
            }
    
            //if(Key != KeyCode.None && Input.GetKey(Key)){
            //    //StartSkill();
            //}
        }
    
        //public void StartSkill(){
        //    if(m_skill.Cooldown == 0){
        //        m_skill.Cooldown = -1;//duration;
        //        m_progressbar.enabled = true;
        //        m_progressbar.fillAmount = 1;
        //        m_skill.Activate();
        //        m_skill.Cooldown = m_skill.Definition.Duration;
        //        m_timerText.enabled = true;
        //        m_timerText.text = Controls.ToDurationString(m_skill.Cooldown);
        //        //return true;
        //    } else {
        //        //return false;
        //    }
        //}
    }
}
