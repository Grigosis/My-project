using System;
using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.Slime.Sugar;

namespace Assets.Scripts.Slime.Core.Algorythms.Pathfinding
{
    public class R
    {
        private int X;
        private int Y;
        private int W;
        private int H;

        public R(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public int Top => Y;
        public int Bottom => Y + H;
        public int Left => X;
        public int Right => X + W;
    }
    
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public class P
    {
        public int x;
        public int y;

        public P()
        {
        }

        public P(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(P other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            return obj is P other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return y * 1024*16 + x;
        }

        public double Distance(P vKey)
        {
            int dx = x - vKey.x;
            int dy = y - vKey.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        
        public double Distance(int xx, int yy)
        {
            int dx = x - xx;
            int dy = y - yy;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        
        public static double Distance(int x1, int y1, int x2, int y2)
        {
            int dx = x1 - x2;
            int dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString()
        {
            return $"{x}:{y}";
        }

        public P Copy()
        {
            return new P(x, y);
        }

        public static P From(string line)
        {
            var split = line.Split(":");
            if (split.Length == 2 && double.TryParse(split[0], out var x) && double.TryParse(split[1], out var y))
            {
                return new P((int)x,(int)y);
            }

            return null;
        }

        public void InCircle(double r, Action<int, int> action, Func<int, int, bool> validateRange = null)
        { 
            for (int xx = x-(int)r-1; xx <= x+(int)r+1; xx++)
            {
                for (int yy = y-(int)r-1; yy <= y+(int)r+1; yy++)
                {
                    if (validateRange == null || validateRange(xx, yy))
                    {
                        if (Distance(xx, yy) <= r)
                        {
                            action(xx, yy);
                        }
                    }
                }
            }
        }
        
        public void InSquare(int r, Action<int, int> action, Func<int, int, bool> validateRange = null)
        {
            for (int xx = x-r; xx <= x+r; xx++)
            {
                for (int yy = y-r; yy <= y+r; yy++)
                {
                    if (validateRange == null || validateRange(xx, yy))
                    {
                        action(xx, yy);
                    }
                }
            }
        }
    }
}