using System.Collections.Generic;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class CoverUnityWrapper : MonoBehaviour, ICoverUnityWrapper
    {
        public CoverEnum Type = CoverEnum.Small;
        
        public MapCellCover mapCellCover;
        public float LeftAngle = 90;
        public float LeftAngle2 = 45;
        public float RightAngle = 90;
        public float RightAngle2 = 45;

        public void GetCellCovers(List<MapCellCover> list)
        {
            var position = gameObject.transform.position;
            var forward = gameObject.transform.forward;
            mapCellCover = new MapCellCover(position.ToMapCellCords(), forward.ToDirection2D(), LeftAngle, RightAngle, LeftAngle2, RightAngle2);
            mapCellCover.Type = Type;
            list.Add(mapCellCover);
        }
    }
}