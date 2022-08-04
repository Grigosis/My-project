using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.BattleMap.UnityWrappers;
using Assets.Scripts.Slime.Sugar;
using ROR.Core;
using SecondCycleGame;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{

    public struct Vector2D
    {
        public double x;
        public double y;

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2D operator + (Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x+ b.x, a.y + b.y);
        }
        
        public static Vector2D operator - (Vector2D a, Vector2D b)
        {
            return new Vector2D(a.x- b.x, a.y - b.y);
        }
        
        public static Vector2D operator / (Vector2D a, double mlt)
        {
            return new Vector2D(a.x / mlt, a.y / mlt);
        }
        
        public static Vector2D operator * (Vector2D a, double mlt)
        {
            return new Vector2D(a.x * mlt, a.y * mlt);
        }

        public double magnitude => Math.Sqrt(x * x + y * y);
    }
    
    public class ConvexShape
    {
        public List<Vector2D> Points = new List<Vector2D>();
        
        public bool Intersects(Vector2D start, Vector2D end)
        {
            for (var index = 0; index < Points.Count; index++)
            {
                var point = Sugar.Sugar.LineIntersectPoint(start, end, Points[index], Points[(index+1)%Points.Count]);
                if (point.HasValue)
                {
                    if (Sugar.Sugar.PointBelongsToLine(Points[index], Points[(index+1)%Points.Count], point.Value))//start, end
                    {
                        //Debug.Log(Points[index]+ " " + Points[(index+1)%Points.Count] + " " + point.Value);
                        return true;
                    }
                }

                return false;
            }

            return false;
        }
        
        public bool Intersects2(Vector2D start, Vector2D end)
        {
            for (var index = 0; index < Points.Count; index++)
            {
                var point = Sugar.Sugar.LineIntersectPoint(start, end, Points[index], Points[(index+1)%Points.Count]);
                if (point.HasValue)
                {
                    if (Sugar.Sugar.PointBelongsToLine(Points[index], Points[(index+1)%Points.Count], point.Value) && Sugar.Sugar.PointBelongsToLine(start, end, point.Value))//start, end //
                    {
                        //Debug.Log("Intersects2:"+start+" " + end + " => " +Points[index]+ " " + Points[(index+1)%Points.Count] + " : " + point.Value);
                        return true;
                    }
                }

                //return false;
            }

            return false;
        }
    }
    

    public enum Intersection
    {
        Intersects,
        Touch,
        DontIntersects
    }
    
    public class BattleMap
    {
        private BattleMapCell[,] Cells;
        public int W;
        public int H;

        public List<ConvexShape> Walls = new List<ConvexShape>();

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
                //if (cover.Type == CoverEnum.Large)
                //{
                //    //Walls.Add();
                //}
                //else
                {
                    var cell = this[cover.Position];
                    if (cell != null)
                    {
                        //Debug.LogError("Register cover" + cover.Position + " " + cover.Direction);
                        cell.Covers.Add(cover);
                    } 
                }
            }
        }

        

        public bool IntersectsWall(Vector2Int from, Vector2Int to)
        {
            foreach (var wall in Walls)
            {
                if (wall.Intersects2(new Vector2D(from.x + 0.5f, from.y+0.5f), new Vector2D(to.x + 0.5f, to.y+0.5f)))
                {
                    return true;
                }
            }

            return false;
        }

        public void Foreach(BattleMapCell cell, int range, Action<BattleMapCell> action)
        {
            var minX = cell.X - range;
            var maxX = cell.X - range;
            var minY = cell.Y - range;
            var maxY = cell.Y - range;


            Foreach(minX, maxX, minY, maxY, action);
        }
        
        public void Foreach(BattleMapCell cell, int minRange, int maxRange, Action<BattleMapCell> action)
        {
            var minX = cell.X - maxRange;
            var maxX = cell.X + maxRange;
            var minY = cell.Y - maxRange;
            var maxY = cell.Y + maxRange;

            var excludeMinX = cell.X - (minRange-1);
            var excludeMaxX = cell.X + (minRange-1);
            var excludeMinY = cell.Y - (minRange-1);
            var excludeMaxY = cell.Y + (minRange-1);

            Foreach(minX, maxX, minY, maxY, excludeMinX, excludeMaxX, excludeMinY, excludeMaxY, action);
        }

        public void Foreach(Vector2Int a, Vector2Int b, Action<BattleMapCell> action)
        {
            Foreach(a.x, b.x, a.y, b.y, action);
        }
        
        public void Foreach(Vector2Int a, Vector2Int b, Vector2Int excludeA, Vector2Int excludeB, Action<BattleMapCell> action)
        {
            Foreach(a.x, b.x, a.y, b.y, excludeA.x, excludeB.x, excludeA.y, excludeB.y, action);
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
        
        public void Foreach(int minX, int maxX, int minY, int maxY, int excludeMinX, int excludeMaxX, int excludeMinY, int excludeMaxY, Action<BattleMapCell> action)
        {
            minX = Math.Max(minX, 0);
            maxX = Math.Max(minX, Math.Min(W-1, maxX));
            minY = Math.Max(minY, 0);
            maxY = Math.Max(minY, Math.Min(H-1, maxY));

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (excludeMinX <= x && x <= excludeMaxX && excludeMinY <= y && y <= excludeMaxY)
                    {
                        continue;
                    }
                    action(this[x,y]);
                }
            }
        }

        public void Attach(LivingEntity unit, BattleMapCell cell)
        {
            unit.Attach(Battle, cell);
            cell.Entity = unit;
        }

        public void AddWall(WallUnityWrapper wall)
        {
            ConvexShape shape = new ConvexShape();
            foreach (var gameObject in wall.Points)
            {
                var pos =  gameObject.transform.TransformToParentLocal(gameObject.transform.localPosition);
                shape.Points.Add(new Vector2D((int)pos.x+0.5f, (int)pos.z+0.5f));
            }
            
            Walls.Add(shape);
        }
        
        
    }
}