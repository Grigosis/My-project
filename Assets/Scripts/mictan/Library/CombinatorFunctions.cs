using System.Collections.Generic;
using System.Text;
using Assets.Scripts.AbstractNodeEditor;

namespace Assets.Scripts.Library {
    public class CombinatorFunctions {
        public static string Concat(CombinatorData xml, List<string> parts) {
            var sb = new StringBuilder();
            foreach (var part in parts) {
                sb.Append(part);
            }
            return sb.ToString();
        }

        public static double Mlt(CombinatorData xml, List<double> parts) {
            var mlt = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                mlt *= parts[i];
            }

            return mlt;
        }

        public static double Div(CombinatorData xml, List<double> parts) {
            var ret = parts[0];
            for(int i = 1; i < parts.Count; i++) {
                if(parts[i] == 0) {
                    throw new System.Exception($"Divide by 0 (index {i})");//???
                }
                ret /= parts[i];
            }
            return ret;
        }

        public static double Sum(CombinatorData xml, List<double> parts) {
            var sum = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                sum += parts[i];
            }

            return sum;
        }

        public static double Sub(CombinatorData xml, List<double> parts) {
            var ret = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                ret -= parts[i];
            }

            return ret;
        }

        public static double Inv(CombinatorData xml, List<double> parts) {
            return -parts[0];
        }

        public static bool IsMore(CombinatorData xml, List<double> parts) {
            var last = parts[0];
            for(int i = 1; i < parts.Count; i++) {
                if(last <= parts[i]) {//! IsMore
                    return false;
                }
                last = parts[i];
            }

            return true;
        }

        public static bool IsMoreOrEq(CombinatorData xml, List<double> parts) {
            var last = parts[0];
            for (int i = 1; i < parts.Count; i++) {
                if (last < parts[i]) {//! IsMoreOrEq
                    return false;
                }
                last = parts[i];
            }

            return true;
        }

        public static bool IsLess(CombinatorData xml, List<double> parts) {
            var last = parts[0];
            for (int i = 1; i < parts.Count; i++) {
                if (last >= parts[i]) {//! IsLess
                    return false;
                }
                last = parts[i];
            }

            return true;
        }

        public static bool IsLessOrEq(CombinatorData xml, List<double> parts) {
            var last = parts[0];
            for (int i = 1; i < parts.Count; i++) {
                if (last > parts[i]) {//! IsLessOrEq
                    return false;
                }
                last = parts[i];
            }

            return true;
        }

        public static bool Eq(CombinatorData xml, List<double> parts) {
            var first = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                if (parts[i] != first) {
                    return false;
                }
            }

            return true;
        }

        public static bool NotEq(CombinatorData xml, List<double> parts) {
            var first = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                if (parts[i] == first) {
                    return false;
                }
            }

            return true;
        }

        public static bool Eq(CombinatorData xml, List<string> parts) {
            var first = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                if (parts[i] != first) {
                    return false;
                }
            }

            return true;
        }

        public static bool NotEq(CombinatorData xml, List<string> parts) {
            var first = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                if (parts[i] == first) {
                    return false;
                }
            }

            return true;
        }

        public static bool Not(CombinatorData xml, List<bool> parts) {

            return !parts[0];
        }

        public static bool And(CombinatorData xml, List<bool> parts) {
            for (var i = 0; i < parts.Count; i++) {
                if (!parts[i]) {
                    return false;
                }
            }

            return true;
        }

        public static bool Or(CombinatorData xml, List<bool> parts) {
            for (var i = 0; i < parts.Count; i++) {
                if (parts[i]) {
                    return true;
                }
            }

            return false;
        }

        public static bool XOr(CombinatorData xml, List<bool> parts) {
            bool ret = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                ret = ret != parts[i];
            }

            return ret;
        }

        public static string ToStr(CombinatorData xml, List<double> parts) {
            return parts[0].ToString();
        }

        public static string ToStr(CombinatorData xml, List<bool> parts) {
            return parts[0].ToString();
        }

        public static double ToDouble(CombinatorData xml, List<string> parts) {
            double ret;
            if (!double.TryParse(parts[0], out ret)) {
                throw new System.Exception($"Cannot parse \"{parts[0]}\" to double");
            }
            return ret;
        }

        public static double ToDouble(CombinatorData xml, List<bool> parts) {
            return parts[0] ? 1 : 0;
        }

        public static bool ToBool(CombinatorData xml, List<string> parts) {
            return new List<string>() { "true", "1", "t" }.Contains(parts[0].ToLower());
        }

        public static bool ToBool(CombinatorData xml, List<double> parts) {
            return parts[0] != 0;
        }

        public static bool ContainsAll(CombinatorData xml, List<string> parts) {
            var first = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                if (!first.Contains(parts[i])) {
                    return false;
                }
            }

            return true;
        }

        public static bool ContainsAny(CombinatorData xml, List<string> parts) {
            var first = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                if (first.Contains(parts[i])) {
                    return true;
                }
            }

            return false;
        }
    }
}
