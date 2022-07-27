using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Core.BattleMap;
using ROR.Core;
using RPGFight;
using RPGFight.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    
    public abstract class ATargetSelector : IUnityTargetSelector
    {
        public event Action<List<SkillTarget>> OnSelected;
        protected HashSet<SkillTarget> SkillTargets = new HashSet<SkillTarget>();
        protected int TargetAmount = 1;
        protected BattleMapCellController m_controller;
        protected SkillEntity m_skillEntity;
        
        public virtual void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            m_controller = controller;
            m_skillEntity = skillEntity;
            controller.ClearAll();
        }
        
        public virtual void EndSelection()
        {
            Debug.LogError("EndSelection");
            m_controller.ClearAll();
            SkillTargets.Clear();
            m_controller = null;
            OnSelected = null;
        }
        
        public void OnMouseEnter(GameObject gameObject) { }
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
                AddOrRemove(ble);
            }
        }
		
		private void AddOrRemove(SkillTarget skillTarget)
        {
            if (SkillTargets.Add(skillTarget))
            {
                if (SkillTargets.Count == TargetAmount)
                {
                    OnSelected?.Invoke(SkillTargets.ToList());
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
    
    public class UnitTargetSelector : ATargetSelector
    {
        protected override SkillTarget GetSkillTarget(GameObject gameObject)
        {
            return gameObject.GetComponentInParent<BattleLivingEntity>()?.LivingEntity;
        }
    }

    public class CellTargetSelector : ATargetSelector
    {
        public override void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            base.BeginSelection(controller, battle, caster, skillEntity);
            
            
            var range = skillEntity.GetRange();
            battle.BattleMap.Foreach(caster.Cell, range, cell => controller.GetOrCreate(new Vector2Int(cell.X, cell.Y)));
        }

        protected override SkillTarget GetSkillTarget(GameObject gameObject)
        {
            return gameObject.GetComponentInParent<BattleLivingEntity>()?.LivingEntity;
        }
    }

    public class MovementTargetSelector : ATargetSelector
    {
        public override void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            base.BeginSelection(controller, battle, caster, skillEntity);
            var cells = DI.GetCellsAvailableForMovement(battle.BattleMap, new Vector2Int(caster.Cell.X, caster.Cell.Y), Balance.GetMovepoints(caster));
            foreach (var move in cells)
            {
                controller.GetOrCreate(new Vector2Int(move.X, move.Y));
            }
        }

        protected override SkillTarget GetSkillTarget(GameObject gameObject)
        {
            return gameObject.GetComponentInParent<BattleLivingEntity>()?.LivingEntity;
        }
    }
    
    public interface TargetSelector
    {
        public List<SkillTarget> GetAllPossibleTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity);
    }

    public interface IUnityTargetSelector : IMouseReceiver
    {
        void BeginSelection(BattleMapCellController controller, Battle battle, LivingEntity caster, SkillEntity entity);
        void EndSelection();
        event Action<List<SkillTarget>> OnSelected;
    }
    
    public interface SkillTarget
    {
        
    }


    
    public class CellSelector
    {
        public List<SkillTarget> GetAllSkillTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            var cell = caster.Cell;
            var range = skillEntity.GetRange();

           
            List<SkillTarget> listOfAllVisibleCells = new List<SkillTarget>();
            battle.BattleMap.Foreach(cell, range, (cell) =>
            {
                if (cell.IsVisibleFrom(cell))
                {
                    listOfAllVisibleCells.Add(cell);
                }
            });

            return listOfAllVisibleCells;
        }
    }
    public class CharacterSelector
    {
        public List<SkillTarget> GetAllSkillTargets(Battle battle, LivingEntity caster, SkillEntity skillEntity)
        {
            var range = skillEntity.GetRange();
            List<SkillTarget> listOfAllVisibleCells = new List<SkillTarget>();
            foreach (var livingEntity in battle.BattleMap.AllLivingEntities)
            {
                if (livingEntity.Cell.DistanceTo(caster.Cell) > range)
                {
                    continue;
                }
                
                if (livingEntity.Cell.IsVisibleFrom(caster.Cell))
                {
                    listOfAllVisibleCells.Add(livingEntity);
                }
            }

            return listOfAllVisibleCells;
        }
    }
}