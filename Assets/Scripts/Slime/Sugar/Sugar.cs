using System;
using System.Collections.Generic;
using Assets.Scripts.Slime.Core;
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
        
        public static float IsToLeftOrRight2(Vector2 A, Vector2 B, Vector2 Test)
        {
            return (B.x - A.x) * (Test.y - A.y) - (B.y - A.y) * (Test.x - A.x);
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