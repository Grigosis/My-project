using System;
using Assets.Scripts.Slime.Sugar;
using ROR.Core;
using RPGFight;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class BattleMapUnityWrapper : MonoBehaviour, IMouseReceiver
    {
        public int W;
        public int H;

        public GameObject MapCells;
        
        public Battle Battle;
        public BattleMap BattleMap => Battle.BattleMap;

        private Vector2Int LastHoveredCell = new Vector2Int(-1,-1);

        void Update()
        {
            Init();
            DrawPath();
        }

        public void NextUnit()
        {
            Debug.Log("NextUnit");
            Battle.NextPlayerTurn();
        }
        
        
        public void Start()
        {
            
        }

        private bool wasInited = false;
        public void Init()
        {
            if (wasInited)
            {
                return;
            }

            wasInited = true;
            
            Battle = new Battle();
            Battle.BattleMap = new BattleMap();
            BattleMap.Init(W,H);


            InitAttachMapObjects();
            InitAttachCellModificators();
            InitStepAttachUnits();
            
            InitStepGenerateCells();
        }
        
        private void InitAttachMapObjects()
        {
            var mapObjects = GetComponentsInChildren<MapObjectUnityWrapper>();
            foreach (var mapObject in mapObjects)
            {
                var o = mapObject.MapObject;
                BattleMap.AddMapObject(o);
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
                    unit.LivingEntity.Attach(Battle, cell);
                    cell.Entity = unit.LivingEntity;
                }
            }
        }

        public void InitStepGenerateCells()
        {
            for (int x = 0; x < BattleMap.W; x++)
            {
                for (int y = 0; y  < BattleMap.H; y++)
                {
                    var cell = Instantiate(GameAssets.i.mapCell);
                    cell.transform.parent = MapCells.transform;
                    cell.transform.localPosition = GetCellPosition(x,y, 0);
                    cell.GetComponent<ClickableProxy>().ClickableReceiver = gameObject;
                    var wrapper = cell.GetComponent<MapCellWrapper>();
                    wrapper.X = x;
                    wrapper.Y = y;

                    var cc = BattleMap[x, y];
                    cc.UnityCell = wrapper;
                }
            }
        }
        
        public Vector3 GetCellPosition(Vector2Int vec)
        {
            return GetCellPosition(vec.x, vec.y);
        }

        public Vector3 GetCellPosition(int x, int y, float height = 0)
        {
            return new Vector3(x * BattleMapCell.CellSize, height, y * BattleMapCell.CellSize);
        }

        private Vector3 GetMapCoordinates(Vector2 vector2)
        {
            //return new Vector3(vector2.x - BattleMapCell.CellSize/2, 1, vector2.y- BattleMapCell.CellSize/2);
            return new Vector3(vector2.x, 1, vector2.y);
        }

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
    }
}