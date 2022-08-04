using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Core.BattleMap;
using UnityEngine;

namespace Assets.Scripts.Slime.Sugar
{
    public static class Sugar
    {
        public static string[] Split(this string s, string by, StringSplitOptions options = StringSplitOptions.None)
        {
            return s.Split(new string[]{by}, StringSplitOptions.None);
        }
        
        public static string[] Split(this string s, string by1, string by2, StringSplitOptions options = StringSplitOptions.None)
        {
            return s.Split(new string[]{by1,by2}, StringSplitOptions.None);
        }

        public static Vector2Int MinXY(this Vector2Int vector, Vector2Int vector2)
        {
            return new Vector2Int(Math.Min(vector.x, vector2.x), Math.Min(vector.y, vector2.y));
        }
        
        public static Vector2Int MaxXY(this Vector2Int vector, Vector2Int vector2)
        {
            return new Vector2Int(Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector2.y));
        }
        
        public static Vector2 Cross(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.y, a.y * b.x);
        }
        
        public static bool IsZero(this Vector2 a)
        {
            return a.x == 0 && a.y == 0;
        }
        
        public static double Multiply (this Vector2 a, Vector2 b)
        {
            return (a.x * b.x+a.y * b.y);
        }
        
        public static Vector2 Multiply (this Vector2 a, float mlt)
        {
            return new Vector2(a.x * mlt, a.y * mlt);
        }

        
        public static Vector2 Rotate(this Vector2 v, float degrees) {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
            float tx = v.x;
            float ty = v.y;
            
            var vector = new Vector2();
            vector.x = (cos * tx) - (sin * ty);
            vector.y = (sin * tx) + (cos * ty);
            return vector;
        }

        public static int IsToLeftOrRight(Vector2 A, Vector2 B, Vector2 Test)
        {
            return Math.Sign((B.x - A.x) * (Test.y - A.y) - (B.y - A.y) * (Test.x - A.x));
        }
        
        //public static bool PointBelongsToLine(Vector2D a, Vector2D b, Vector2D test)
        //{
        //    //var c = (a + b) / 2;
        //    //var d = (a - b).magnitude;
        //    //var d2 = (test - c).magnitude;
        //    var minX = Math.Min(a.x, b.x);
        //    var maxX = Math.Max(a.x, b.x);
        //    var minY = Math.Min(a.y, b.y);
        //    var maxY = Math.Max(a.y, b.y);
        //    return
        //        (minX <= test.x && test.x <= maxX) ||
        //        (minY <= test.y && test.y <= maxY);
        //}

        public static void Color(this MonoBehaviour behaviour, Color color)
        {
            var _renderer = behaviour.gameObject.GetComponentInChildren<Renderer>();
            _renderer.material.color = color;
        }
        
        public static bool PointBelongsToLine(Vector2D a, Vector2D b, Vector2D test)
        {
            var c = (a + b) / 2;
            var d = (a - b).magnitude / 2;
            var d2 = (test - c).magnitude;
            
            
            return d2 <= d;
        }
        
       

        public static Vector3 TransformToParentLocal(this Transform transform, Vector3 vector)
        {
            var worldPos = transform.TransformPoint(vector);
            var localPos = transform.parent.parent.InverseTransformPoint(worldPos);
            return localPos;
        }
        
        public static Vector2D? LineIntersectPoint(Vector2D line1V1, Vector2D line1V2, Vector2D line2V1, Vector2D line2V2)
        {
            //Line1
            double A1 = line1V2.y - line1V1.y;
            double B1 = line1V1.x - line1V2.x;
            double C1 = A1*line1V1.x + B1*line1V1.y;

            //Line2
            double A2 = line2V2.y - line2V1.y;
            double B2 = line2V1.x - line2V2.x;
            double C2 = A2 * line2V1.x + B2 * line2V1.y;

            double det = A1*B2 - A2*B1;
            if (det == 0)
            {
                return null;//parallel lines
            }
            else
            {
                double x = (B2*C1 - B1*C2)/det;
                double y = (A1 * C2 - A2 * C1) / det;
                return new Vector2D((float)x,(float)y);
            }
        }
        
        public static Vector3 LocalForward(this Transform transform)
        {
            return transform.worldToLocalMatrix.MultiplyVector(transform.forward);
        }
        
        public static Vector2Int ToDirection2D(this Vector3 forward)
        {
            var direction = new Vector2Int();
            if (Math.Abs(forward.x) > Math.Abs(forward.z))
            {
                direction.x = forward.x > 0 ? 1 : -1;
            }
            else if (Math.Abs(forward.x) < Math.Abs(forward.z))
            {
                direction.y = forward.y > 0 ? 1 : -1;
            } 
            else
            {
                throw new Exception($"WrongDirectionVector [{forward}]");
            }

            return direction;
        }

        public static double Clamp(this double v, double min, double max)
        {
            return Math.Min(max, Math.Max(min, v));
        } 
        
        public static List<GameObject> GetAllChildren(this GameObject Go)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i< Go.transform.childCount; i++)
            {
                list.Add(Go.transform.GetChild(i).gameObject);
            }
            return list;
        }

        public static Vector2Int ToMapCellCords(this Vector3 position)
        {
            Vector2Int intpos = new Vector2Int();
            intpos.x = (int)Math.Round(position.x / BattleMapCell.CellSize);
            intpos.y = (int)Math.Round(position.z / BattleMapCell.CellSize);
            return intpos;
        }
    }
}