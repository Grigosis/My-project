using UnityEngine;
using UnityEngine.UI;
using System;
using ROR.Lib;

namespace ROR.Core
{
    //public class SkillBtn : MonoBehaviour{
    //    public SkillEntity Skill{
    //        get {return m_skill;}
    //        set{
    //            m_skill = value;
    //            Image = Resources.Load<Sprite>("Textures/" + value.Definition.Icon);
    //        }
    //    }
    //    public KeyCode Key = KeyCode.None;
    //    public Sprite Image{
    //        get{return gameObject.GetComponent<Image>().sprite;}
    //        set{
    //            gameObject.GetComponent<Image>().sprite = value;
    //        }
    //    }
    //
    //    private SkillEntity m_skill;
    //    private Image m_progressbar;        
    //    private Text m_timerText;
    //    private Image m_codeTextBg;
    //    private Text m_codeText;
    //
    //    void Start(){
    //        m_progressbar = gameObject.transform.GetChild(0).GetComponent<Image>();
    //        m_timerText = gameObject.transform.GetChild(1).GetComponent<Text>();
    //        m_codeTextBg = gameObject.transform.GetChild(2).GetComponent<Image>();
    //        m_codeText = gameObject.transform.GetChild(3).GetComponent<Text>();
    //
    //        if(Controls.kkToString.ContainsKey(Key)){
    //            m_codeText.enabled = true;
    //            m_codeTextBg.enabled = true;
    //            m_codeText.text = Controls.kkToString[Key];
    //            m_codeTextBg.rectTransform.sizeDelta = new Vector2(m_codeText.preferredWidth * m_codeText.transform.localScale.x + 2, m_codeTextBg.rectTransform.sizeDelta.y);
    //        } else {
    //            m_codeText.enabled = false;
    //            m_codeTextBg.enabled = false;
    //        }
    //    }
    //
    //    // Update is called once per frame
    //    void Update(){
    //        if(m_skill.Cooldown > 0){
    //            m_skill.Cooldown -= Time.deltaTime;
    //            if(m_skill.Cooldown <= 0){
    //                m_skill.Cooldown = 0;
    //                m_progressbar.enabled = false;
    //                m_timerText.enabled = false;
    //            } else {
    //                m_progressbar.fillAmount = m_skill.Cooldown / m_skill.Definition.Duration;
    //                m_timerText.text = Controls.ToDurationString(m_skill.Cooldown);
    //            }
    //        }
    //
    //        if(Key != KeyCode.None && Input.GetKey(Key)){
    //            StartSkill();
    //        }
    //    }
    //
    //    public void StartSkill(){
    //        if(m_skill.Cooldown == 0){
    //            m_skill.Cooldown = -1;//duration;
    //            m_progressbar.enabled = true;
    //            m_progressbar.fillAmount = 1;
    //            m_skill.Activate();
    //            m_skill.Cooldown = m_skill.Definition.Duration;
    //            m_timerText.enabled = true;
    //            m_timerText.text = Controls.ToDurationString(m_skill.Cooldown);
    //            //return true;
    //        } else {
    //            //return false;
    //        }
    //    }
    //}
}
