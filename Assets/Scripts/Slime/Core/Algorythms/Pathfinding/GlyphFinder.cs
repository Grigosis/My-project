using System;
using System.Collections.Generic;
using System.Drawing;
using Assets.Scripts.Slime.Core.Algorythms.Pathfinding;
using GemCraft2.Pathfinding;

namespace GemCraft2
{
    
    public class GlyphFinder : WaveAlgorythm
    {
        public int minX=999999, minY=999999;
        public int maxX, maxY;

        public PRect bounds;
        private PointChecker canMoveFunc;
        private WaveGenerator generator;
        
        public void Init (PointChecker canMoveFunc, WaveGenerator generator, PRect bounds=null)
        {
            this.generator = generator;
            this.canMoveFunc = canMoveFunc;

            Init (canMoveFunc.GetWidth(), canMoveFunc.GetHeight());
            
            if (bounds == null)
            {
                this.bounds = new PRect (0,0, canMoveFunc.GetWidth(), canMoveFunc.GetHeight());
            }
            else
            {
                this.bounds = bounds;
            }
        }

        public override void StartWave(P point)
        {
            point = point.Copy();
            canMoveFunc.Startwave(this, point.x, point.y);
            base.StartWave(point);
            
        }

        protected override void GenerateWave(P p, float frame)
        {
            generator.Generate(this, p, frame);
        }

        public void Test(P p, int dx, int dy, float wouldBeAtFrame)
        {
            if (p.y + dy >= bounds.Top && p.y + dy < bounds.Bottom && p.x + dx >= bounds.Left && p.x + dx < bounds.Right)
            {
                var to = new P(p.x + dx,p.y + dy);
                if (!CanVisit(p, to, wouldBeAtFrame))
                {
                    return;
                }
                if(CanMove(p, to))
                {
                    AddPoint(to, wouldBeAtFrame);
                }
            }
        }

        public override bool CanMove(P from, P to)
        {
            return canMoveFunc.CanMove(from, to);
        }

        protected override void Visit(P p, float wouldBeAtFrame)
        {
            base.Visit (p, wouldBeAtFrame);


            minX = Math.Min(minX, p.x);
            maxX = Math.Max(maxX, p.x);
            minY = Math.Min(minY, p.y);
            maxY = Math.Max(maxY, p.y);
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
        
        public List<P> GetBorder(int dx = 1, int dy = 1)
        {
            List<P> borders = new List<P>();
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (GetValue(x, y) >= 0 && IsBorder (x, y, dx, dy))
                    {
                        borders.Add(new P(x,y));                
                    }
                }
            }

            return borders;
        }

        public void GetAll(Action<int, int, float> action)
        {
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var v = GetValue(x, y);
                    if (v >= 0)
                    {
                        action(x,y,GetValue(x, y));                
                    }
                }
            }
        }
        
        public bool IsBorder(int x, int y, int dx = 1, int dy = 1)
        {
            bool isB = false;
            For(x - dx, y - dy, x + dx, y + dy, (xx, yy) =>
            {
                isB |= GetValue(xx, yy) < 0;
            });
            return isB;
        }
    }
}
