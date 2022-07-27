using System;
using System.Collections.Generic;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class BattleMap
    {
        private BattleMapCell[,] Cells;
        public int W;
        public int H;

        public List<MapObject> Objects = new List<MapObject>();
        public List<LivingEntity> AllLivingEntities = new List<LivingEntity>();
        public Dictionary<Vector2Int, MapObject> ObjectsByCells = new Dictionary<Vector2Int, MapObject>();
        public Battle Battle;

        public void Init(Battle battle, int w, int h)
        {
            Battle = battle;
            W = w;
            H = h;
            Cells = new BattleMapCell[w,h];
            
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    var cell = new BattleMapCell();
                    cell.X = i;
                    cell.Y = j;
                    Cells[i, j] = cell;
                }
            }
        }

        public BattleMapCell this [int xx, int yy]
        {
            get
            {
                if (xx <0 || yy < 0 || xx >= Cells.GetLength(0) || yy >= Cells.GetLength(1))
                {
                    //Debug.LogError($"CantFind [{xx}][{yy}]=> [{xx}][{yy}]");
                    return null;
                }
                
                return Cells[xx, yy];
            }
        }

        
        public BattleMapCell this [Vector2Int vec]
        {
            get
            {
                return this[vec.x, vec.y];
            }
        }

        public void AddMapObject(MapObject mo)
        {
            Objects.Add(mo);
            foreach (var cells in mo.MapCells)
            {
                var mapCell = this[cells];
                if (mapCell != null)
                {
                    if (!ObjectsByCells.TryGetValue(cells, out var prev))
                    {
                        ObjectsByCells[cells ] = mo;
                        mapCell.Object = mo;
                    }
                    else
                    {
                        Debug.LogError($"Object at:{cells} already exists [{prev}]/[{mo}]");
                    }
                }
            }
            
            foreach (var cover in mo.Covers)
            {
                var cell = this[cover.Position];
                if (cell != null)
                {
                    //Debug.LogError("Register cover" + cover.Position + " " + cover.Direction);
                    cell.Covers.Add(cover);
                }
            }
        }

        public void Foreach(BattleMapCell cell, int range, Action<BattleMapCell> action)
        {
            var minX = cell.X - range;
            var maxX = cell.X - range;
            var minY = cell.Y - range;
            var maxY = cell.Y - range;


            Foreach(minX, maxX, minY, maxY, action);
        }
        
        public void Foreach(int minX, int maxX, int minY, int maxY, Action<BattleMapCell> action)
        {
            minX = Math.Max(minX, 0);
            maxX = Math.Max(minX, Math.Min(W-1, maxX));
            minY = Math.Max(minY, 0);
            maxY = Math.Max(minY, Math.Min(H-1, maxY));

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    action(this[x,y]);
                }
            }
        }

        public void Attach(LivingEntity unit, BattleMapCell cell)
        {
            unit.Attach(Battle, cell);
            cell.Entity = unit;
        }
    }
}