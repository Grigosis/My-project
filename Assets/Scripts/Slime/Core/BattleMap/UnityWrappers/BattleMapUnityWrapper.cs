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
using Random = System.Random;

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
            CurrentSelectedSkill = skillEntity;
            
            var targetSelector = skillEntity.Definition.TargetSelector;
            Type type = Type.GetType(targetSelector);
            if (type == null)
            {
                Debug.LogError($"Was unable to find class {targetSelector} {skillEntity.Definition.Id}");
                return;
            }
            var o = Activator.CreateInstance (type) as IUnityTargetSelector;
            if (o == null)
            {
                Debug.LogError($"Was unable to instantinate {targetSelector}");
            }
            
            CurrentTargetSelector = o;
            CurrentTargetSelector.BeginSelection(battleMapCellController, Battle, Battle.CurrentLivingEntityTurn, skillEntity);
            CurrentTargetSelector.OnSelected += CurrentTargetSelectorOnOnSelected;
        }

        private void CurrentTargetSelectorOnOnSelected(List<SkillTarget> obj)
        {
            if (Battle.CurrentLivingEntityTurn?.AIController != null)
            {
                return;
            }
            Debug.Log("CurrentTargetSelectorOnOnSelected");
            CurrentSelectedSkill.Implementation.CastSkill(CurrentSelectedSkill, obj, new Random());
            CurrentTargetSelector = null;
            NextUnit();
        }

        private bool SwitchNextUnit = false;

        void Update()
        {
            Init();
            
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
            
            //ConvexShape shape = new ConvexShape();
            //shape.Points.Add(new Vector2(0,0));
            //shape.Points.Add(new Vector2(100,100));
            //BattleMap.Walls.Add(shape);

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
                    Battle.Attach(unit.LivingEntity, cell);
                }
            }
        }
    }
}



/*class MyClass : IMouseReceiver
        {
            private Vector2Int LastHoveredCell = new Vector2Int(-1,-1);
            public void OnMouseEnter(GameObject gameObject)
            {
            
            }

            public void OnMouseOver(GameObject gameObject)
            {
                var wrapper = gameObject.GetComponent<MapCellWrapper>();
                LastHoveredCell.x = wrapper.X;
                LastHoveredCell.y = wrapper.Y;
                if (BattleMap[LastHoveredCell] != null)
                {
                    BattleMap[LastHoveredCell].Color(Color.cyan);
                }
            
            }
            
            public void OnMouseExit(GameObject gameObject)
            {
            
            }

            public void OnMouseDown(GameObject gameObject)
            {
                //var wrapper = gameObject.GetComponent<MapCellWrapper>();
                //LastHoveredCell.x = wrapper.X;
                //LastHoveredCell.y = wrapper.Y;
                NextUnit();
            }

            public void OnMouseUp(GameObject gameObject)
            {
            
            }
            
            private void DrawPath()
            {
                FillWithColor(Color.gray);

                var all = DI.GetCellsAvailableForMovement(BattleMap, LastHoveredCell, 10);
                
                foreach (var cost in all)
                {
                    BattleMap[cost.X, cost.Y].Color(Color.green);
                }
            }

            private void FillWithColor(Color color)
            {
                for (int i = 0; i < BattleMap.W; i++)
                {
                    for (int j = 0; j < BattleMap.H; j++)
                    {
                        if (!BattleMap[i, j].CanStand)
                        {
                            BattleMap[i, j].Color(Color.red);
                        }
                        else if (BattleMap[i, j].MovementCost > 1)
                        {
                            BattleMap[i, j].Color(Color.yellow);
                        }
                        else
                        {
                            BattleMap[i, j].Color(color);
                        }
                        
                    }
                }
            }

            private void DrawXXX()
            {
                FillWithColor(Color.green);
                
                
                var tt = LastHoveredCell;// testCells[(update / 10) % testCells.Length];
                var cell = BattleMap[tt];
                if (cell != null)
                {
                    var covers = cell.Covers;
                    if (covers.Count > 0)
                    {
                        for (int i = 0; i < BattleMap.W; i++)
                        {
                            for (int j = 0; j < BattleMap.H; j++)
                            {
                                float isUnderCover=1f;
                                foreach (var cover in covers)
                                {
                                    var result = cover.IsUnderCover(new Vector2Int(i, j));
                                    isUnderCover = Math.Min(isUnderCover, result);
                                }
                                
                                if (isUnderCover < 0.4f)
                                {
                                    BattleMap[i, j].Color(Color.red);
                                }
                                else if (isUnderCover < 0.6f)
                                {
                                    BattleMap[i, j].Color(Color.yellow);
                                }
                            }
                        }
                    }
                    
                    cell.Color(Color.magenta);
                }  
            }
        }*/