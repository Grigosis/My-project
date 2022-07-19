namespace Assets.Scripts.Slime.Core.Algorythms.Pathfinding
{
    public struct MovementCost
    {
        public int X;
        public int Y;
        public float Cost;

        public MovementCost(int x, int y, float cost)
        {
            X = x;
            Y = y;
            Cost = cost;
        }
    }
}