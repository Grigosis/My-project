using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core.Algorythms.Pathfinding;

namespace GemCraft2.Pathfinding
{
    public abstract class WaveAlgorythm
    {
        public const int UNCALCULATED = -2;
        public const int UNKNOWN = -1;
        public const int VALID = 0;

        public int w,h;

        private float[,] path; //same size as data;

        private HashSet<P> wavegenerators = new HashSet<P>();
        private HashSet<P> nextWavegenerators = new HashSet<P>();

        protected abstract void GenerateWave (P p, float cost);
        public abstract bool CanMove (P from, P to);

        public float GetValue(int x, int y)
        {
            return path[x, y];
        }

        public virtual void Init (int w, int h)
        {
            this.w = w;
            this.h = h;
            this.path = new float[w,h];

            for (var x =0; x<w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    path[x,y] = UNCALCULATED;
                }
            }
        }

        public bool CanVisit(P from, P to, float wouldBeAtFrame)
        {
            return (path[to.x, to.y] < VALID || wouldBeAtFrame < path[to.x, to.y]);
        }
        
        protected void AddPoint (P to, float wouldBeAtFrame)
        {
            nextWavegenerators.Add(to);
            Visit(to, wouldBeAtFrame); //Can be called more than 1 time
        }

        public virtual void StartWave(P point)
        {
            if (!CanMove (point, point))
            {
                return;
            }

            Visit(point, 0);
            wavegenerators.Add(point);

            while (wavegenerators.Count > 0)
            {
                foreach (var w in wavegenerators)
                {
                    GenerateWave(w, GetValue(w.x, w.y));
                }

                wavegenerators.Clear();

                var a = wavegenerators;
                var b = nextWavegenerators;
                nextWavegenerators = a;
                wavegenerators = b;
            }
        }

        protected virtual void Visit(P p, float wouldBeAtFrame)
        {
            path[p.x, p.y] = wouldBeAtFrame;
        }
        
        //=============== SUGAR ===============
        
        public List<P> FindWayBack(int x, int y, List<P> list = null)
        {
            if (list == null) list = new List<P>();
            //THIS IS NOT CORRECT, AS IT USING UNKNOWN 8-WAY picker.
            
            var v = GetValue(x, y);
            
            if (v < 0) return null;
            if (v == 0) return list;

            P best = new P(x, y);
            For(x - 1, y - 1, x + 1, y + 1, (xx, yy) =>
            {
                var vv = GetValue(xx, yy);
                if (vv < v && vv >= VALID)
                {
                    best.x = xx;
                    best.y = yy;
                    v = vv;
                }
            });

            if (best.x == x && best.y == y) 
                throw new Exception("WTF?");
            
            list.Add(best);
            return FindWayBack(best.x, best.y, list);
        }

        
        public List<P> FindWaysBack(int x, int y, List<P> list = null)
        {
            if (list == null) list = new List<P>();
            //THIS IS NOT CORRECT, AS IT USING UNKNOWN 8-WAY picker.
            
            var v = GetValue(x, y);
            if (v < 0) return null;
            if (v == 0) return list;
            
            var tmp = new List<P>();
            float bestV = v;
            For(x - 1, y - 1, x + 1, y + 1, (xx, yy) =>
            {
                var vv = GetValue(xx, yy);
                if (vv < VALID) return;
                
                if (vv < bestV)
                {
                    tmp.Clear();
                    bestV = vv;
                    tmp.Add(new P(xx,yy));
                } 
                else if (vv == bestV && bestV != v)
                {
                    tmp.Add(new P(xx,yy));
                }
            });

            if (bestV == v) 
                throw new Exception("WTF?");
            
            
            list.AddRange(tmp);
            
            foreach (var p in tmp)
            {
                FindWaysBack(p.x, p.y, list);
            }
            
            return list;
        }

        public void For(int minX, int minY, int maxX, int maxY, Action<int, int> a)
        {
            minX = Math.Max(0, minX);
            maxX = Math.Min(w-1, maxX);
            minY = Math.Max(0, minY);
            maxY = Math.Min(h-1, maxY);

            for (int xx = minX; xx <= maxX; xx++)
            {
                for (int yy = minY; yy <= maxY; yy++)
                {
                    a(xx, yy);
                }
            }
        }
    }
}