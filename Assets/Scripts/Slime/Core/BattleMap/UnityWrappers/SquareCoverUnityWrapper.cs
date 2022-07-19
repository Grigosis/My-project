using System.Collections.Generic;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class SquareCoverUnityWrapper : MonoBehaviour, ICoverUnityWrapper
    {
        public CoverEnum Type = CoverEnum.Small;
        
        public MapCellCover mapCellCover;
        public float LeftAngles = 90;
        public float LeftAngles2 = 90;
        public float RightAngles = 90;
        public float RightAngles2 = 90;

        public void GetCellCovers(List<MapCellCover> list)
        {
            var position = gameObject.transform.localPosition;
            var shift = new Vector2Int[]
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(0, 1),
            };

            foreach (var shiftPos in shift)
            {
                mapCellCover = new MapCellCover(shiftPos + position.ToMapCellCords(), -shiftPos, LeftAngles, RightAngles, LeftAngles2, RightAngles2);
                mapCellCover.Type = Type;
                list.Add(mapCellCover);
            }
            
           
        }
    }
}