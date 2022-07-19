using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class MapObject
    {
        public List<Vector2Int> MapCells = new List<Vector2Int>();
        public List<MapCellCover> Covers = new List<MapCellCover>();
        
    }
}