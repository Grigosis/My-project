using System.Collections.Generic;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.BattleMap.UnityWrappers
{
    struct Vector2Pair
    {
        private Vector2Int leftTop;
        private Vector2Int bottomRight;

        public Vector2Pair(Vector2Int first, Vector2Int second)
        {
            if (first.y == second.y)
            {
                if (first.x < second.x)
                {
                    this.leftTop = first;
                    this.bottomRight = second;
                }
                else
                {
                    this.bottomRight = first;
                    this.leftTop = second;
                }
            }
            else
            {
                if (first.y < second.y)
                {
                    this.leftTop = first;
                    this.bottomRight = second;
                }
                else
                {
                    this.bottomRight = first;
                    this.leftTop = second;
                }
            }
        }
    }
    
    public class VisibilityCache
    {
        private Dictionary<Vector2Pair, bool> Cache = new Dictionary<Vector2Pair, bool>();
        private BattleMap m_battleMap;

        public void Init(BattleMap battleMap)
        {
            m_battleMap = battleMap;
        }
        
        public void IsVisible(Vector2Int from, Vector2Int to)
        {
            var dx = from.x - to.x;
            var dy = from.x - to.x;

            var vp = new Vector2Pair(from, to);

            var min = from.MinXY(to);
            var max = from.MaxXY(to);
            
            
            m_battleMap.Foreach(min, max, (x) =>
            {
                
                
                //x.Covers
            });
            
        }
    }
}