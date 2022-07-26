using Assets.Scripts.Slime.Core.BattleMap;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace Assets.Scripts.Slime.Core
{
    public class MapCellCover
    {
        public CoverEnum Type;
        public readonly Vector2Int Position;
        public readonly Vector2Int Direction;
        public readonly float LeftAngle;
        public readonly float LeftAngle2;
        public readonly float RightAngle;
        public readonly float RightAngle2;

        public MapCellCover(Vector2Int position, Vector2Int direction, float leftAngle, float rightAngle, float leftAngle2, float rightAngle2)
        {
            Position = position;
            Direction = direction;
            LeftAngle = leftAngle;
            RightAngle = rightAngle;
            LeftAngle2 = leftAngle2;
            RightAngle2 = rightAngle2;
            Calculate();
        }

        private bool isDirty = true;
        private Vector2 LeftPoint1;
        private Vector2 LeftPoint2;
        private Vector2 LeftPoint3;
        private Vector2 RightPoint1;
        private Vector2 RightPoint2;
        private Vector2 RightPoint3;

        private void Calculate()
        {
            var direction = new Vector2(Direction.x, Direction.y);
            var edge = new Vector2(Position.x+direction.x/2, Position.y+direction.y/2);
            

            var perp = Vector2.Perpendicular(Direction);
            
            
            LeftPoint1 = edge - perp * BattleMapCell.CellSize / 2; 
            RightPoint1 = edge + perp * BattleMapCell.CellSize / 2;
            
            LeftPoint2 = LeftPoint1 + direction.Rotate(-LeftAngle);
            LeftPoint3 = LeftPoint1 + direction.Rotate(-LeftAngle2);
            RightPoint2 = RightPoint1 + direction.Rotate(RightAngle);
            RightPoint3 = RightPoint1 + direction.Rotate(RightAngle2);
        }

        public float IsUnderCover(Vector2 from2)
        {
            var sign3 = Sugar.Sugar.IsToLeftOrRight(LeftPoint1, RightPoint1, from2);
            if (sign3 > 0)
            {
                return 1;
            }
            var sign1 = Sugar.Sugar.IsToLeftOrRight(LeftPoint1, LeftPoint3, from2);
            var sign2 = Sugar.Sugar.IsToLeftOrRight(RightPoint1, RightPoint3, from2);

            if (sign1 != sign2 || sign1 == 0 || sign2 == 0)
            {
                return 0.33f;
            }
            
            sign1 = Sugar.Sugar.IsToLeftOrRight(LeftPoint1, LeftPoint2, from2);
            sign2 = Sugar.Sugar.IsToLeftOrRight(RightPoint1, RightPoint2, from2);
            
            if (sign1 != sign2 || sign1 == 0 || sign2 == 0)
            {
                return 0.5f;
            }

            return 1f;
        }
    }
}