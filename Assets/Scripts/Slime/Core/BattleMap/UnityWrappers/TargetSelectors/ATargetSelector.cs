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
        public event Action<List<ISkillTarget>> OnSelected;
        protected HashSet<ISkillTarget> SkillTargets = new HashSet<ISkillTarget>();
        protected int TargetAmount = 1;
        protected BattleMapCellController m_controller;
        protected SkillEntity m_skillEntity;
        protected LivingEntity m_caster;
        protected Battle m_battle;
        
        protected int lastX = 0; 
        protected int lastY = 0;
        protected bool m_dirty = true;
        protected List<ISkillTarget> allAvailableTargets = new List<ISkillTarget>();
        private GameObject mouseDownGameObject;

        public virtual void Update()
        {
            if (m_dirty)
            {
                m_dirty = false;
                FindAndDo();
            }
        }
        
        public virtual void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            Debug.Log("BeginSelection:"+this.GetType());
            m_controller = controller;
            m_skillEntity = skillEntity;
            m_battle = battle;
            m_caster = caster;
            controller.ClearAll();
            SetXY(m_caster.Cell.X, m_caster.Cell.Y);
            FindAndDo();
        }

        public void SetXY(int X, int Y)
        {
            if (lastX != X || lastY != Y)
            {
                lastX = X;
                lastY = Y;
                m_dirty = true;
            }
        }

        protected abstract void Do();

        protected void FindAndDo()
        {
            allAvailableTargets.Clear();
            m_skillEntity.TargetSelector.GetAllPossibleTargets(m_battle, m_caster, m_skillEntity, new Vector2Int(lastX, lastY), allAvailableTargets);
            Do();
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
        
        protected void Invoke (List<ISkillTarget> skillTarget) {
            OnSelected?.Invoke(skillTarget);
        }
        
        public virtual void OnMouseEnterProxy(GameObject gameObject) { }

        public void OnMouseOverProxy(GameObject gameObject)
        {
            var target = GetUniversalSkillTarget(gameObject);
            if (target != null && target is LivingEntity living && allAvailableTargets.Contains(target))
            {
                var battleLivingEntity = living.GameObjectLink.GetComponent<BattleLivingEntity>();
                battleLivingEntity.SetHighlighted(Color.red);
            }
        }

        public void OnMouseExitProxy(GameObject gameObject)
        {
            var target = GetUniversalSkillTarget(gameObject);
            if (target != null && target is LivingEntity living && allAvailableTargets.Contains(target))
            {
                var battleLivingEntity = living.GameObjectLink.GetComponent<BattleLivingEntity>();
                battleLivingEntity.SetHighlighted(Color.white);
            }
        }
        
        
        public void OnMouseDownProxy(GameObject gameObject)
        {
            mouseDownGameObject = gameObject;
        }

        public void OnMouseUpProxy(GameObject gameObject)
        {
            if (gameObject == mouseDownGameObject && gameObject != null)
            {
                var ble = GetSkillTarget(gameObject);
                if (ble != null && allAvailableTargets.Contains(ble))
                {
                    AddOrRemove(ble);
                }
            }
        }
		
		private void AddOrRemove(ISkillTarget skillTarget)
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

        protected abstract ISkillTarget GetSkillTarget(GameObject gameObject);

        protected ISkillTarget GetUniversalSkillTarget(GameObject gameObject)
        {
            var wrapper = gameObject.GetComponentInParent<MapCellWrapper>();
            var wrapper2 = gameObject.GetComponentInParent<BattleLivingEntity>();;
            if (wrapper != null)
            {
                var cell = m_battle.BattleMap[wrapper.X, wrapper.Y];
                if (cell != null)
                {
                    return cell;
                }
            }
            else if (wrapper2 != null)
            {
                return wrapper2.LivingEntity;
            }
            return null;
        }
    }
}