using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Slime.Core.Algorythms.Pathfinding;
using Assets.Scripts.Slime.Core.BattleMap;
using GemCraft2;
using ROR.Core;
using UnityEngine;

namespace RPGFight
{
    public class DI
    {
        public static void CreateFloatingText(LivingEntity entity, string text, Color color, float textSize = 14f)
        {
            FloatingText.Create(entity.GameObjectLink.transform.position + entity.GameObjectLink.transform.transform.up * 8, text, color, textSize);
        }

        /// <summary>
        /// Get all possible cells for movement
        /// Use moveunits == -1 for infinite search
        /// </summary>
        public static List<MovementCost> GetCellsAvailableForMovement(BattleMap map, Vector2Int from, float moveunits)
        {
            var pathfinding = new BattleMapPathfinding(map);
            var pointGenerator = new BattleMapFourSides(map, moveunits);
            GlyphFinder finder = new GlyphFinder();
            finder.Init(pathfinding, pointGenerator);
            finder.StartWave(new P(from.x, from.y));
            List<MovementCost> costs = new List<MovementCost>();
            finder.GetAll((x, y, v) => costs.Add(new MovementCost(x, y, v)));
            return costs;
        }

        /// <summary>
        /// Returns path from P1 to P2, or null
        /// Use moveunits == -1 for infinite search
        /// </summary>
        public static List<Vector2Int> GetPath(BattleMap map, Vector2Int from, Vector2Int to, float moveunits = -1)
        {
            var pathfinding = new BattleMapPathfinding(map);
            var pointGenerator = new BattleMapFourSides(map, moveunits);
            GlyphFinder finder = new GlyphFinder();
            finder.Init(pathfinding, pointGenerator);
            finder.StartWave(new P(from.x, from.y));
            
            var path = finder
                .FindWayBack(to.x, to.y)
                .Select((x)=>new Vector2Int(x.x, x.y))
                .Reverse()
                .ToList();
            
            return path;
        }
    }
}