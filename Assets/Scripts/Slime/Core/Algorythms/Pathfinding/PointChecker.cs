using Assets.Scripts.Slime.Core.Algorythms.Pathfinding;

namespace GemCraft2.Pathfinding
{
    public interface PointChecker
    {
        void Startwave(GlyphFinder finder, int x, int y);
        bool CanMove(P from, P to);
        int GetWidth();
        int GetHeight();
    }
}