using System.Collections.Generic;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap
{
    public class MapObjectUnityWrapper : MonoBehaviour
    {
        public MapObject MapObject
        {
            get
            {
                var covers = GetComponentsInChildren<ICoverUnityWrapper>();
                MapObject obj = new MapObject();
                var allCovers = new List<MapCellCover>();
                foreach (var cover in covers)
                {
                    cover.GetCellCovers(allCovers);
                }

                foreach (var cover in allCovers)
                {
                    obj.Covers.Add(cover);
                }
                obj.MapCells.Add(gameObject.transform.position.ToMapCellCords());
                return obj;
            }
        }

    }
}