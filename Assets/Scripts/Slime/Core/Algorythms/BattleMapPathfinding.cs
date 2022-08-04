using Assets.Scripts.Slime.Core.Algorythms.Pathfinding;
using GemCraft2;
using GemCraft2.Pathfinding;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    public class BattleMapPathfinding : PointChecker
    {
        private BattleMap.BattleMap battleMap;
        
        public BattleMapPathfinding(BattleMap.BattleMap battleMap)
        {
            this.battleMap = battleMap;
        }
        
        public bool CanMove(P from, P to)
        {
            if (from.x == to.x && from.y == to.y)
            {
                return true;
            }
            
            var cell = battleMap[to.x, to.y];
            if (cell == null)
            {
                //Debug.Log($"CantMove [{from}]=>[{to}]");
                return false;
            }

            if (!cell.CanStand)
            {
                //Debug.Log($"CantStand [{from}]=>[{to}]");
                return false;
            }

            if (cell.Entity != null)
            {
                //Debug.Log($"EntityAlready [{from}]=>[{to}]");
                return false;
            }
            return true;
        }
        public int GetWidth() { return battleMap.W; }
        public int GetHeight() { return battleMap.H; }
        public void Startwave(GlyphFinder finder, int x, int y) { }
    }
    
    public class BattleMapFourSides : WaveGenerator
    {
        private BattleMap.BattleMap battleMap;
        private float maxMovement;

        public BattleMapFourSides(BattleMap.BattleMap battleMap, float maxMovement = -1f)
        {
            this.battleMap = battleMap;
            this.maxMovement = maxMovement;
        }

        public void Generate(GlyphFinder finder, P from, float frame)
        {
            Add (finder, from, 1, 0, frame);
            Add (finder, from, -1, 0, frame);
            Add (finder, from, 0, 1, frame);
            Add (finder, from, 0, -1, frame);
            
            //Diagonal
            Add (finder, from, 1, 1, frame);
            Add (finder, from, 1, -1, frame);
            Add (finder, from, -1, 1, frame);
            Add (finder, from, -1, -1, frame);
        }

        private void Add(GlyphFinder finder, P from, int dx, int dy, float frame)
        {
            var cell = battleMap[from.x+dx, from.y+dy];
            if (cell == null)
            {
                return;
            }
            var movementPenalty = cell.MovementCost;
            if (cell.CanStand == false)
            {
                return;
            }
            if (frame + movementPenalty < maxMovement || maxMovement == -1f)
            {
                finder.Test(from, dx, dy, frame + movementPenalty);
            }
        }
    }
    
    public class BattleMapFourSidesDiagonalPenalty : WaveGenerator
    {
        private BattleMap.BattleMap battleMap;
        private float maxMovement;

        public BattleMapFourSidesDiagonalPenalty(BattleMap.BattleMap battleMap, float maxMovement = -1f)
        {
            this.battleMap = battleMap;
            this.maxMovement = maxMovement;
        }

        public void Generate(GlyphFinder finder, P from, float frame)
        {
            Add (finder, from, 1, 0, frame);
            Add (finder, from, -1, 0, frame);
            Add (finder, from, 0, 1, frame);
            Add (finder, from, 0, -1, frame);
            
            //Diagonal
            Add (finder, from, 1, 1, frame+0.01f);
            Add (finder, from, 1, -1, frame+0.01f);
            Add (finder, from, -1, 1, frame+0.01f);
            Add (finder, from, -1, -1, frame+0.01f);
        }

        private void Add(GlyphFinder finder, P from, int dx, int dy, float frame)
        {
            var cell = battleMap[from.x+dx, from.y+dy];
            if (cell == null)
            {
                return;
            }
            var movementPenalty = cell.MovementCost;
            if (cell.CanStand == false)
            {
                return;
            }
            if (frame + movementPenalty < maxMovement || maxMovement == -1f)
            {
                finder.Test(from, dx, dy, frame + movementPenalty);
            }
        }
    }
}