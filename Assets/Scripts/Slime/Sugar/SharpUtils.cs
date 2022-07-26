using System;
using System.Collections.Generic;
using System.Text;


namespace ROR.Lib {
    public static class SharpUtils {

        public static DateTime utcZero = new DateTime(1970, 1, 1);
		public static DateTime y2020 = new DateTime(2020, 1, 1);

        public static float Lerp2 (float Current, float Desired, float Speed)
        {
            if (Current < Desired)
            {
                Current += Speed;
                if (Current > Desired)
                {
                    Current = Desired;
                }
            }
            else
            {
                Current -= Speed;
                if (Current < Desired)
                {
                    Current = Desired;
                }
            }
            return Current;
        }

        public static void EnsureCapacity<T>(ref T[] array, int size, float growFactor = 1)
        {
            if (array == null || array.Length < size)
            {
                var newSize = Math.Max((int) ((array?.Length ?? 0) * growFactor), size);
                Array.Resize(ref array, newSize);
            }
        }

        public static float toRadian (this float v)
        {
            return (float)(v * Math.PI / 180d);
        }

        public static double toRadian(this double v)
        {
            return (v * Math.PI / 180d);
        }

        public static float toDegree (this float v)
        {
            return (float)(v / Math.PI * 180d);
        }

        public static double toDegree (this double v)
        {
            return (v / Math.PI * 180d);
        }

        public static int DayOfWeek (DateTime time) //Saturday = 6, Sunday = 7
		{
			var utcZero = new DateTime(1970, 1, 1);
			if (time < utcZero) return 1;
			var d = (y2020 - utcZero).TotalDays;
			var dd = (int)d + d%1>0 ? 1 : 0;
			dd = dd- (dd / 7)*7 + 4; //1970 was Thursday
			if (dd > 7) dd -=7;
			return dd;
		}

        public static long timeStamp () {
            return (long)(DateTime.UtcNow.Subtract(utcZero)).TotalSeconds;
        }

        public static long timeUtcDif()
        {
            return Math.Abs((long)DateTime.UtcNow.Subtract(DateTime.Now).TotalSeconds);
        }

        public static bool HasFlags(this int x, int f)
        {
            return (x | f) == x;
        }

        public static long msTimeStamp () {
            return (long)(DateTime.UtcNow.Subtract(utcZero)).TotalMilliseconds;
        }

        public static TimeSpan StripMilliseconds(this TimeSpan time)
        {
            return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
        }


        public static void AddOrRemove<T>(this HashSet<T> set, T data, bool add) {
            if (add) { set.Add(data); } else { set.Remove(data);  }
        }


        public static string Print<T, K>(this Dictionary<T, K> dict) {
            StringBuilder sb = new StringBuilder();
            sb.Append("Dict[");
            foreach (var x in dict) {
                sb.Append(x.Key).Append("->").Append(x.Value).Append("\n");
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string Print<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("List[");
            foreach (var x in list)
            {
                sb.Append(x).Append(" ");
            }
            sb.Append("]");
            return sb.ToString();
        }

        
        public static string toHumanWeight (this double num) {
            if (num <0.000001) return String.Format ("{0:N2} µg", num *1000000000);
            if (num <0.001) return String.Format ("{0:N2} mg", num *1000000);
            if (num <1) return String.Format ("{0:N2} g", num *1000);
            if (num <1000) return String.Format ("{0:N2} kg", num);
            if (num <1000000) return String.Format ("{0:N2} t", num /1000);
            if (num <1000000000) return String.Format ("{0:N2} kt", num /1000000);
            if (num <1000000000000) return String.Format ("{0:N2} Mt", num /1000000000);
            if (num <1000000000000000) return String.Format ("{0:N2} Gt", num /1000000000000);
            return "TONS";
        }

        public static string toHumanQuantity (this double num) {
            if (num <1000) return String.Format ("{0:N2}", num);
            if (num <1000000) return String.Format ("{0:N2} K", num /1000);
            if (num <1000000000) return String.Format ("{0:N2} M", num /1000000);
            return "TONS";
        }

        public static string toPhysicQuantity (this double num, String s) {
            var k = 1000d;
            if (num <k) return String.Format ("{0:N2} ", num)+s;
            if (num <k*k) return String.Format ("{0:N2} k", num / k)+s;
            if (num <k*k*k) return String.Format ("{0:N2} M", num /(k*k))+s;
            if (num <k*k*k*k) return String.Format ("{0:N2} G", num / (k*k*k))+s;
            if (num <k*k*k*k*k) return String.Format ("{0:N2} T", num / (k*k*k*k))+s;
            return String.Format ("{0:N2} P", num / (k*k*k*k*k));
        }

        public static string toHumanQuantityCeiled (this double num) {
            if (num <1000) return String.Format ("{0:N0}", num);
            if (num <1000000) return String.Format ("{0:N2} K", num /1000);
            if (num <1000000000) return String.Format ("{0:N2} M", num /1000000);
            if (num <1000000000) return String.Format ("{0:N2} B", num /1000000);
            return "TONS";
        }
        
        public static string toHumanQuantityEnergy (this double num)
        {
            if (Math.Abs(num) > 1000) return $"{num / 1000 :N0} GW";
            if (Math.Abs(num) > 1) return $"{num :N2} MW";
            if (Math.Abs(num) > 0.001) return $"{num * 1000 :N0} KW";
            return $"{num * 1000000 :N0} W";
        }
        
        public static string toHumanQuantityVolume (this double num)
        {
            if (Math.Abs(num) > 1000000000) return $"{num / 1000000000 :N2} GL";
            if (Math.Abs(num) > 1000000) return $"{num / 1000000 :N2} ML";
            if (Math.Abs(num) > 1000) return $"{num / 1000 :N2} kL";
            if (Math.Abs(num) > 100) return $"{num / 100 :N2} hL";
            if (Math.Abs(num) > 10) return $"{num / 10 :N2} daL";
            if (Math.Abs(num) > 1) return $"{num :N2} L";
            if (Math.Abs(num) > 0.1) return $"{num * 10 :N2} dL";
            if (Math.Abs(num) > 0.01) return $"{num * 100 :N2} cL";
            return $"{num * 1000 :N2} mL";
        }


        public static string toHumanTime (this double num) {
            if (num <120) return String.Format ("{0:N0} s", num);
            if (num <3600) return String.Format ("{0:N0} min", num /60);
            if (num <3600*24) return String.Format ("{0:N0} h", num /3600);
            return String.Format ("{0:N0} days", num/3600/24);
        }
        public static string toHumanTime2 (this int num, bool isFullWithDays = false) {
            if (num < 60) return num + "s";
            if (num < 3600) return num / 60 + "m " + num % 60 + "s";
            if (num < 3600 * 24) return num / 3600 + "h " + (num / 60) % 60 + "m " + num % 60 + "s";
            if (num / 3600 / 24 == 1) return isFullWithDays ? "1 day " + num / 3600 + "h " + (num / 60) % 60 + "m " + num % 60 + "s" : "1 day";
            return isFullWithDays ? num / 3600 / 24 + " days " + num / 3600 % 24 + "h " + (num / 60) % 60 + "m " + num % 60 + "s" : num / 3600 / 24 + " days";
        }

        public static string fixZero (this double num) {
            return String.Format ("{0:N2}", num);
        }

        public static string fixZero(this float num)
        {
            return String.Format("{0:N2}", num);
        }

       
        public static bool IsOneKeyMoreThan<T> (this Dictionary<T, int> buffer, Dictionary<T, int> maxLimits)
        {
            foreach (var y in buffer)
            {
                if (y.Value > maxLimits[y.Key])
                {
                    return true;
                }
            }
            return false;
        }
        

        public static void Sum <T> (this Dictionary<T,double> dict, T key, double value) {
            if (!dict.ContainsKey(key)) {
                dict[key] = value;
            } else {
                dict[key] = dict[key] + value;
            }
        }

        public static void Sum <T> (this Dictionary<T,int> dict, T key, int value) {
            if (!dict.ContainsKey(key)) {
                dict[key] = value;
            } else {
                dict[key] = dict[key] + value;
            }
        }

        public static string printContent<T>(this List<T> dict) {
            StringBuilder sb = new StringBuilder();
            sb.Append("List[");
            foreach (var x in dict) {
                sb.Append(x).Append(",\n");
            }
            sb.Append("]");
            return sb.ToString();
        }


        
        public static K GetOr<T, K>(this Dictionary<T, K> dict, T t, K k) {
            if (dict.ContainsKey(t)) {
                return dict[t];
            } else {
                return k;
            }
        }

        public static List<K> GetOrCreate<T, K>(this Dictionary<T, List<K>> dict, T t) {
            if (!dict.ContainsKey(t)) {
                dict.Add (t, new List<K>());
            }
            return dict[t];
        }

        public static HashSet<K> GetOrCreate<T, K>(this Dictionary<T, HashSet<K>> dict, T t) {
            if (!dict.ContainsKey(t)) {
                dict.Add (t, new HashSet<K>());
            }
            return dict[t];
        }

        public static Dictionary<K,V> GetOrCreate<T, K, V>(this Dictionary<T, Dictionary<K,V>> dict, T t) {
            if (!dict.ContainsKey(t)) {
                dict.Add (t, new Dictionary<K,V>());
            }
            return dict[t];
        }

        public static K Set<T, K>(this Dictionary<T, K> dict, T t, K k) {
            K old = default(K);
            if (dict.ContainsKey(t)) {
                old = dict[t];
                dict.Remove(t);
            }
            dict.Add(t, k);
            return old;
        }
    }
}
