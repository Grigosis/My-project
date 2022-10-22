using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Core.BattleMap;
using ClassLibrary1.Inventory;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Random = System.Random;

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
        
        public static bool AreEqual<T, K>(this Dictionary<T, K> a, Dictionary<T, K> b)
        {
            if (a == null || b == null) return a==b;

            a = new Dictionary<T, K>(a);
            b = new Dictionary<T, K>(b);
            
            Deduplicate(a,b);
            var result = a.Count == 0 && b.Count == 0;
            return result;
        }
        public static void Deduplicate<T, K>(this Dictionary<T, K> a, Dictionary<T, K> b)
        {
            if (a == null || b == null) return;

            var keys = new List<T>(a.Keys);
            foreach (var k in keys)
            {
                a.Remove(k);
                b.Remove(k);
            }
        }

        public static int DivideWithUpperRound(this int number, int divide)
        {
            return number / divide + (number % divide == 0 ? 0 : 1);
        }
        
        public static int ToIntWithUpperRound(this double number)
        {
            return (int)Math.Ceiling(number);
        }
        
        public static int DivideWithUpperRound(this uint number, int divide)
        {
            return (int)(number / divide + (number % divide == 0 ? 0 : 1));
        }

        public static T GetAt<T>(this List<T> list, int index)
        {
            return list.Count > index ? list[index] : default(T);
        }
        
        public static T AddList<T>(this List<T> list, int index)
        {
            return list.Count > index ? list[index] : default(T);
        }
        
        public static void AddMultipleTimes<T>(this List<T> list, T value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                list.Add(value);
            }
        }

        public static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        private static char[] alphabet = "qwertyuiopasdfghjklzxcvbnm".ToCharArray(); 
        
        public static string NextString(this Random random, int count, char[] chars = null)
        {
            if (chars == null)
            {
                chars = alphabet;
            }

            var s = "";
            for (int i = 0; i < count; i++)
            {
                s += alphabet[random.Next(chars.Length)];
            }
            return s;
        }

        public static K GetOrNew<T, K>(this Dictionary<T, K> dict, T key) where K : new()
        {
            if (!dict.TryGetValue(key, out var value))
            {
                value = new K();
                dict[key] = value;
            }

            return value;
        }
        
        public static T Next<T>(this Random random, T[] array)
        {
            return array[random.Next(array.Length)];
        }

        public static bool AddOnce<T, K>(this Dictionary<T, K> dictionary, T t, K k) {
            if (dictionary.TryGetValue(t, out var oldK)) {
                return false;
            }
            dictionary.Add(t, k);
            return true;
        }
        
        public static DynamicSettings Clone(this DynamicSettings from)
        {
            if (from == null)
            {
                return null;
            }

            var settings = new DynamicSettings();
            settings.StringSettings = new Dictionary<Type, object>(from.StringSettings);
            return settings;
        }



        #region Reflection

        public static void AddScriptFunctions<T>(this Dictionary<string, T> dictionary, Assembly assembly) where T : class
        {
            foreach (Type type in assembly.GetTypes())
            {
                AddScriptFunctions(dictionary, type);
            }
        }
        
        public static void AddScriptFunctions<T>(this Dictionary<string, T> dictionary, Type type) where T : class
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                AddFx<T>(dictionary, method);
            }
        }

        public static bool AddFx<T>(this Dictionary<string, T> funcMap, MethodInfo method) where T : class
        {
            if (IsMethodCompatibleWithDelegate<T>(method))
            {
                var function = (T) (object) Delegate.CreateDelegate(typeof (T), method);
                funcMap.Add(method.Name, function);
                return true;
            }

            return false;
        }

        private static bool IsMethodCompatibleWithDelegate<T>(this MethodInfo method) where T : class
        {
            Type delegateType = typeof(T);
            MethodInfo delegateSignature = delegateType.GetMethod("Invoke");

            bool parametersEqual = delegateSignature
                .GetParameters()
                .Select(x => x.ParameterType)
                .SequenceEqual(method.GetParameters()
                    .Select(x => x.ParameterType));

            return delegateSignature.ReturnType == method.ReturnType &&
                   parametersEqual;
        }
        
        public static T GetAttribute<T>(this MemberInfo prop) where T : Attribute => Attribute.GetCustomAttribute(prop, typeof (T)) as T;


        public static string TrimCount(this string txt, int count)
        {
            if (txt.Length > count) return txt.Substring(0, count);
            return txt;
        }

        public static void GetChildrenRecursively(this VisualElement element, Func<VisualElement, bool> action)
        {
            foreach (var child in element.Children())
            {
                var result = action(child);
                if (result)
                {
                    GetChildrenRecursively(child, action);
                }
            }
        }
        
        public static HashSet<ExtendedPort> GetAllPorts (this VisualElement element)
        {
            var hashSet = new HashSet<ExtendedPort>();
            element.GetChildrenRecursively((v) =>
            {
                if (v is ExtendedPort extendedPort)
                {
                    hashSet.Add(extendedPort);
                    return false;
                }

                if (v is Edge e)
                {
                    return false;
                }

                return true;
            });
            
            return hashSet;
        }
        
        public static int Sort(MemberInfo a, MemberInfo b)
        {
            if (a.Name == "Id") return -1;
            if (b.Name == "Id") return 1;
            
            var at = a.DeclaringType;
            var bt = b.DeclaringType;

            var aa = 0;
            var bb = 0;
                
                
            while (at.BaseType != null)
            {
                at = at.BaseType;
                aa++;
            }
            while (bt.BaseType != null)
            {
                bt = bt.BaseType;
                bb++;
            }

            return aa - bb;
        } 
        
        public static bool ImplementsGenericInterface(this Type subtype, Type genericInterface) => ((IEnumerable<Type>) subtype.GetInterfaces()).Any<Type>((Func<Type, bool>) (x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterface));

        
        public static Type GetMemberType(this MemberInfo member)
        {
            Type tt = null;
            if (member is FieldInfo fi) tt = fi.FieldType;
            if (member is PropertyInfo pi) tt = pi.PropertyType;
            return tt;
        }
        
        #endregion

        
        public static int Index<T>(this T[] arr, T val)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals(val)) return i;
            }

            return -1;
        }

        public static void SetHidden(this VisualElement element, bool isHidden)
        {
            element.style.display = new StyleEnum<DisplayStyle>(isHidden ? StyleKeyword.None : StyleKeyword.Auto);
        }

        
        
        
        public static void SetStyleSheet(this VisualElement ve, string name, bool enabled)
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load(name);
            //Debug.Log("SetStyleSheet:" + styleSheet);
            
            if (enabled)
            {
                ve.styleSheets.Add(styleSheet);
                
            }
            else
            {
                ve.styleSheets.Remove(styleSheet);
            }
            
            for (int i = 0; i < ve.styleSheets.count; i++)
            {
                Debug.Log(" Styles:" + ve.styleSheets[i]);
            }
        }

        /// <summary>
        /// Только для редактора. Оч костыльно
        /// </summary>
        /// <param name="path"></param>
        public static void PlaySound(string path)
        {
            var clip = Sugar.LoadClip($"file:///{Environment.CurrentDirectory}/{path}");
            PlayClip2(clip);
            Thread.Sleep((int)(clip.length * 1000));
        }
        
        public static void PlayClip2(AudioClip clip, int startSample = 0, bool loop = false)
        {
            System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
                "PlayPreviewClip",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                null,
                new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
            method.Invoke(
                null,
                new object[] { clip, startSample, loop }
            );
        }
        
        public static void PlayClip(AudioClip clip) {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] {
                    typeof(AudioClip)
                },
                null
            );
            method.Invoke(
                null,
                new object[] {
                    clip
                }
            );
        }
        
        public static AudioClip LoadClip(string path)
        {
            AudioClip clip = null;
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
            {
                uwr.SendWebRequest();
 
                // wrap tasks in try/catch, otherwise it'll fail silently
                try
                {
                    while (!uwr.isDone)
                    {
                        Thread.Sleep(10);
                    }
 
                    if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}");
                    else
                    {
                        clip = DownloadHandlerAudioClip.GetContent(uwr);
                    }
                }
                catch (Exception err)
                {
                    Debug.Log($"{err.Message}, {err.StackTrace}");
                }
            }
 
            return clip;
        }
        
    }
}