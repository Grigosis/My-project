using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.BattleMap.Logic.Interfaces;
using Assets.Scripts.Slime.Core.BattleMap.UnityWrappers;
using Assets.Scripts.Slime.Core.BattleMap.UnityWrappers.TargetSelectors;
using Assets.Scripts.Slime.Sugar;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    
    public class BattleMapUnityWrapper : MonoBehaviour, IMouseReceiverProxy
    {
        public int W;
        public int H;

        public GameObject MapCells;
        
        public Battle Battle;
        public BattleMap BattleMap => Battle.BattleMap;
        public SkillBarUI SkillBarUi;
        public IUnityTargetSelector CurrentTargetSelector;
        public SkillEntity CurrentSelectedSkill;
        public BattleMapCellController battleMapCellController = new BattleMapCellController();
        private bool wasInited = false;
        

        public IMouseReceiver GetMouseReceiver()
        {
            return CurrentTargetSelector; 
        }

        public void SelectSkill(SkillEntity skillEntity)
        {
            if (Battle.CurrentLivingEntityTurn?.AIController != null)
            {
                return;
            }
            CurrentSelectedSkill = skillEntity;
            CurrentTargetSelector = skillEntity.UnityTargetSelector;
            CurrentTargetSelector.BeginSelection(battleMapCellController, Battle, Battle.CurrentLivingEntityTurn, skillEntity);
            CurrentTargetSelector.OnSelected += CurrentTargetSelectorOnOnSelected;
        }

        private void CurrentTargetSelectorOnOnSelected(List<ISkillTarget> obj)
        {
            if (Battle.CurrentLivingEntityTurn?.AIController != null)
            {
                return;
            }
            //Debug.Log("CurrentTargetSelectorOnOnSelected");
            //CurrentSelectedSkill.Implementation.CastSkill(CurrentSelectedSkill, obj, new Random());
            //CurrentTargetSelector = null;
            //NextUnit();
        }

        public List<Action> ActionsToExecute = new List<Action>();
        private bool SwitchNextUnit = false;

        public void AddAction(Action action)
        {
            lock (ActionsToExecute)
            {
                ActionsToExecute.Add(action);
            }
        }
        
        void Update()
        {
            Init();
            
            lock (ActionsToExecute)
            {
                foreach (var action in ActionsToExecute)
                {
                    action();
                }
                ActionsToExecute.Clear();
            }
            
            CurrentTargetSelector?.Update();
            Battle.CurrentLivingEntityTurn?.AIController?.Update();
            
            if (SwitchNextUnit)
            {
                SwitchNextUnit = false;
                Debug.Log("NextUnitImpl");
                Battle.NextPlayerTurn();
                
                if (Battle.CurrentLivingEntityTurn != null)
                {
                    SkillBarUi.Entity = Battle.CurrentLivingEntityTurn.GameObjectLink.GetComponent<BattleLivingEntity>();
                    SkillBarUi.OnUnitChanged();
                }
            }
        }

        private void BattleOnOnLivingEntityChanged(LivingEntity was, LivingEntity neww)
        {
            was?.AIController?.End();
            neww?.AIController?.Start();
            Debug.Log($"{(neww?.AIController != null ? "HAS AI": "HASN'T AI")}");
        }

        public void NextUnit()
        {
            Debug.Log("ScheduleNextUnit");
            SwitchNextUnit = true;
        }
        
        
        public void Start()
        {
            SkillBarUi.OnSelectedSkillChanged += OnSelectedSkillChanged;
        }

        private void OnSelectedSkillChanged(SkillEntity obj)
        {
            SelectSkill(obj);
        }


        public void Init()
        {
            
            if (wasInited)
            {
                return;
            }
            
            var context = ClassLibrary1.Library.Instance.Context;

            wasInited = true;
            
            Battle = new Battle();
            Battle.OnLivingEntityChanged += BattleOnOnLivingEntityChanged;
            
            Battle.BattleUnity = this;
            Battle.BattleMap = new BattleMap();
            BattleMap.Init(Battle, W,H);
            
            battleMapCellController.Init(this);
            
            
            
            InitAttachMapObjects();
            InitAttachCellModificators();
            InitStepAttachUnits();

            NextUnit();
        }
        
        private void InitAttachMapObjects()
        {
            var mapObjects = GetComponentsInChildren<MapObjectUnityWrapper>();
            
            foreach (var mapObject in mapObjects)
            {
                var o = mapObject.MapObject;
                BattleMap.AddMapObject(o);
            }
            
            var walls = GetComponentsInChildren<WallUnityWrapper>();
            foreach (var wall in walls)
            {
                BattleMap.AddWall(wall);
            }
        }

        public void InitAttachCellModificators()
        {
            var noPasses = GetComponentsInChildren<CellOptions>();
            foreach (var pass in noPasses)
            {
                var cords = pass.transform.localPosition.ToMapCellCords();
                var cell = BattleMap[cords];
                if (cell != null)
                {
                    cell.CanStand = pass.CanPass;
                    cell.MovementCost = pass.MovementCost;
                }
            }
        }

        public void InitStepAttachUnits()
        {
            var units = GetComponentsInChildren<BattleLivingEntity>();
            foreach (var unit in units)
            {
                var cords = unit.transform.localPosition.ToMapCellCords();
                var cell = BattleMap[cords];
                if (cell != null)
                {
                    Battle.Attach(unit.LivingEntity, cell, unit.Team);
                }
            }
        }
    }
}