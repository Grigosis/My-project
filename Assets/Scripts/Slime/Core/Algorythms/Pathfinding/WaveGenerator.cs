using Assets.Scripts.Slime.Core.Algorythms.Pathfinding;

namespace GemCraft2.Pathfinding
{
    public interface WaveGenerator
    {
        void Generate(GlyphFinder finder, P from, float frame);
    }
}