using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core;
using SecondCycleGame;
using UnityEngine;

namespace ROR.Core
{
    public class SkillBarUI : MonoBehaviour
    {
        public BattleLivingEntity Entity;

        public Action<SkillEntity> OnSelectedSkillChanged;
        public SkillEntity m_selectedSkill;
        public SkillEntity SelectedSkill
        {
            get { return m_selectedSkill; }
            set
            {
                if (m_selectedSkill != value)
                {
                    m_selectedSkill = value;
                    OnSelectedSkillChanged?.Invoke(value);
                    Debug.LogWarning($"OnSelectedSkillChanged {value}");
                }
            }
        }

        public void OnUnitChanged()
        {
            SelectedSkill = Entity?.LivingEntity?.SkillBar.SkillEntities[0];
        }
        
        // Start is called before the first frame update
        void Start()
        {
            var btns = GetComponentsInChildren<SkillBtn>();
            for (var i = 0; i < btns.Length; i++)
            {
                btns[i].OnClicked += OnOnClick;
            }
        }
        
        void Update()
        {
            var btns = GetComponentsInChildren<SkillBtn>();
            var skillEntities = Entity?.LivingEntity?.SkillBar.SkillEntities;
            skillEntities ??= new List<SkillEntity>();
            
            for (var i = 0; i < btns.Length; i++)
            {
                if (Input.GetKeyDown(btns[i].Key))
                {
                    btns[i].OnClick();
                }
                
                if (i >= skillEntities.Count)
                {
                    btns[i].Skill = null; 
                }
                else
                {
                    btns[i].Skill = skillEntities[i]; 
                }
                
                btns[i].IsSelected = (btns[i].Skill == SelectedSkill && SelectedSkill != null); 
            }
        }

        private void OnOnClick(SkillBtn obj)
        {
            SelectedSkill = obj.Skill;
        }
    }
}
