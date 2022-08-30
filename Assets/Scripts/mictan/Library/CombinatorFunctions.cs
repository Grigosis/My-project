using System.Collections.Generic;
using System.Text;
using Assets.Scripts.AbstractNodeEditor;

namespace Assets.Scripts.Library {
    public class CombinatorFunctions {
        public static string Concat(CombinatorScriptable xml, List<string> parts) {
            var sb = new StringBuilder();
            foreach (var part in parts) {
                sb.Append(part);
            }
            return sb.ToString();
        }

        public static double Mlt(CombinatorScriptable xml, List<double> parts) {
            var mlt = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                mlt *= parts[i];
            }

            return mlt;
        }

        public static double Sum(CombinatorScriptable xml, List<double> parts) {
            var sum = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                sum += parts[i];
            }

            return sum;
        }

        public static string ToStr(CombinatorScriptable xml, List<double> parts) {
            return parts[0].ToString();
        }
    }
}
