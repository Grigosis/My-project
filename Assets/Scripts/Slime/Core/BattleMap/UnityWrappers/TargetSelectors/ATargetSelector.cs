using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors
{
    public abstract class ATargetSelector : IUnityTargetSelector
    {
        public event Action<List<SkillTarget>> OnSelected;
        protected HashSet<SkillTarget> SkillTargets = new HashSet<SkillTarget>();
        protected int TargetAmount = 1;
        protected BattleMapCellController m_controller;
        protected SkillEntity m_skillEntity;
        protected LivingEntity m_caster;
        protected Battle m_battle;

        public virtual void Update() { }
        
        public virtual void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            Debug.Log("BeginSelection:"+this.GetType());
            m_controller = controller;
            m_skillEntity = skillEntity;
            m_battle = battle;
            m_caster = caster;
            controller.ClearAll();
        }
        
        public virtual void EndSelection()
        {
            try
            {
                m_controller.ClearAll();
                SkillTargets.Clear();
                m_controller = null;
                OnSelected = null;
            }
            catch (Exception e)
            {
                Debug.LogError(this.GetType()+" "+e);
            }
            
        }
        
        protected void Invoke (List<SkillTarget> skillTarget) {
            OnSelected?.Invoke(skillTarget);
        }
        
        public virtual void OnMouseEnter(GameObject gameObject) { }
        public void OnMouseOver(GameObject gameObject) { }
        public void OnMouseExit(GameObject gameObject) { }
        
        private GameObject mouseDownGameObject;
        public void OnMouseDown(GameObject gameObject)
        {
            mouseDownGameObject = gameObject;
        }

        public void OnMouseUp(GameObject gameObject)
        {
            if (gameObject == mouseDownGameObject && gameObject != null)
            {
                var ble = GetSkillTarget(gameObject);
                if (ble != null)
                {
                    AddOrRemove(ble);
                }
            }
        }
		
		private void AddOrRemove(SkillTarget skillTarget)
        {
            if (SkillTargets.Add(skillTarget))
            {
                if (SkillTargets.Count == TargetAmount)
                {
                    Invoke(SkillTargets.ToList());
                    EndSelection();
                }
            }
            else
            {
                SkillTargets.Remove(skillTarget);
            }
        }

        protected abstract SkillTarget GetSkillTarget(GameObject gameObject);
    }
}