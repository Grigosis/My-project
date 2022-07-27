using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class BattleMapCellController
    {
        public Dictionary<Vector2Int, MapCellWrapper> AllCells = new Dictionary<Vector2Int, MapCellWrapper>();

        public BattleMapUnityWrapper BattleMap;

        public void Init(BattleMapUnityWrapper battleMap)
        {
            this.BattleMap = battleMap;
        }
        
        public MapCellWrapper GetOrCreate(Vector2Int vector)
        {
            if (AllCells.TryGetValue(vector, out var cell))
            {
                return cell;
            }
            
            var newcell = CreateCell(vector.x, vector.y);
            AllCells[vector] = newcell;
            return newcell;
        }

        public void ClearAll()
        {
            foreach (var wrapper in AllCells)
            {
                GameObject.Destroy(wrapper.Value.gameObject);
            }
            
            AllCells.Clear();
        }

        public void GenerateCells(HashSet<Vector2Int> newCells)
        {
            ClearAll();
            
            foreach (var wrapper in newCells)
            {
                var cell = CreateCell(wrapper.x, wrapper.y);
                AllCells.Add(wrapper, cell);
            }
        }

        public void Foreach(Action<Vector2Int, MapCellWrapper> action)
        {
            foreach (var cell in AllCells)
            {
                action.Invoke(cell.Key, cell.Value);
            }
        }

        private MapCellWrapper CreateCell(int x, int y)
        {
            var cell = GameObject.Instantiate(GameAssets.i.mapCell);
            cell.transform.parent = BattleMap.MapCells.transform;
            cell.transform.localPosition = GetCellPosition(x,y, 0);
            cell.GetComponent<ClickableProxy>().ClickableReceiver = BattleMap.gameObject;
            var wrapper = cell.GetComponent<MapCellWrapper>();
            wrapper.X = x;
            wrapper.Y = y;

            var cc = BattleMap.BattleMap[x, y];
            cc.UnityCell = wrapper;

            return wrapper;
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
    }
}